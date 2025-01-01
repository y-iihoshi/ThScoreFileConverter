//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models.Th075;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th075;

// %T75CARD[xxx][yy][z]
internal sealed class CardReplacer(
    IReadOnlyDictionary<(CharaWithReserved Chara, Level Level), IClearData> clearData, bool hideUntriedCards)
    : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create(
        $@"{Definitions.FormatPrefix}CARD({Parsers.CardNumberParser.Pattern})({Parsers.CharaParser.Pattern})([NR])");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var number = Parsers.CardNumberParser.Parse(match.Groups[1]);
        var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
        var type = match.Groups[3].Value.ToUpperInvariant();

        if (chara == Chara.Meiling)
            return match.ToString();

        if ((number > 0) && (number <= Definitions.CardIdTable[chara].Count()))
        {
            if (hideUntriedCards)
            {
                var dataList = clearData
                    .Where(pair => pair.Key.Chara == (CharaWithReserved)chara).Select(pair => pair.Value);
                if (dataList.All(data => data.CardTrialCount[number - 1] <= 0))
                    return (type == "N") ? "??????????" : "?????";
            }

            var cardId = Definitions.CardIdTable[chara].ElementAt(number - 1);
            return (type == "N")
                ? Definitions.CardTable[cardId].Name : Definitions.CardTable[cardId].Level.ToString();
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
