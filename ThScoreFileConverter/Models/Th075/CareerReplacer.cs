//-----------------------------------------------------------------------
// <copyright file="CareerReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models.Th075;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th075;

// %T75C[xxx][yy][z]
internal sealed class CareerReplacer(
    IReadOnlyDictionary<(CharaWithReserved Chara, Level Level), IClearData> clearData,
    INumberFormatter formatter)
    : IStringReplaceable
{
    private static readonly Core.Models.IntegerParser TypeParser = new(@"[1-4]");
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}C({Parsers.CardNumberParser.Pattern})({Parsers.CharaParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var number = Parsers.CardNumberParser.Parse(match.Groups[1]);
        var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
        var type = TypeParser.Parse(match.Groups[3]);

        if (chara == Chara.Meiling)
            return match.ToString();

        if (type == 4)
        {
            if ((number > 0) && (number <= Definitions.CardIdTable[chara].Count()))
            {
                return clearData.Where(pair => pair.Key.Chara == (CharaWithReserved)chara)
                    .Any(pair => pair.Value.CardTrulyGot[number - 1] != 0x00) ? "★" : string.Empty;
            }
            else
            {
                return match.ToString();
            }
        }

        static int ToInteger(short value)
        {
            return value;
        }

        Func<IClearData, IEnumerable<int>> getValues = type switch
        {
            1 => data => data.MaxBonuses,
            2 => data => data.CardGotCount.Select(ToInteger),
            _ => data => data.CardTrialCount.Select(ToInteger),
        };

        if (number == 0)
        {
            return formatter.FormatNumber(clearData
                .Where(pair => pair.Key.Chara == (CharaWithReserved)chara)
                .Sum(pair => getValues(pair.Value).Sum()));
        }
        else if (number <= Definitions.CardIdTable[chara].Count())
        {
            return formatter.FormatNumber(clearData
                .Where(pair => pair.Key.Chara == (CharaWithReserved)chara)
                .Sum(pair => getValues(pair.Value).ElementAt(number - 1)));
        }
        else
        {
            return match.ToString();
        }
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
