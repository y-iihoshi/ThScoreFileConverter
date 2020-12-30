//-----------------------------------------------------------------------
// <copyright file="CharaReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th075
{
    // %T75CHR[x][yy][z]
    internal class CharaReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T75CHR({0})({1})([1-4])", Parsers.LevelParser.Pattern, Parsers.CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CharaReplacer(
            IReadOnlyDictionary<(CharaWithReserved, Level), IClearData> clearData, INumberFormatter formatter)
        {
            if (clearData is null)
                throw new ArgumentNullException(nameof(clearData));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                var type = IntegerHelper.Parse(match.Groups[3].Value);

                if (chara == Chara.Meiling)
                    return match.ToString();

                var data = clearData.TryGetValue(((CharaWithReserved)chara, level), out var value)
                    ? value : new ClearData();
                return type switch
                {
                    1 => formatter.FormatNumber(data.UseCount),
                    2 => formatter.FormatNumber(data.ClearCount),
                    3 => formatter.FormatNumber(data.MaxCombo),
                    4 => formatter.FormatNumber(data.MaxDamage),
                    _ => match.ToString(),  // unreachable
                };
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
