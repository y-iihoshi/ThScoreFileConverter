﻿//-----------------------------------------------------------------------
// <copyright file="ClearRankReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th145
{
    // %T145CLEAR[x][yy]
    internal class ClearRankReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"{0}CLEAR({1})({2})", Definitions.FormatPrefix, Parsers.LevelParser.Pattern, Parsers.CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ClearRankReplacer(IReadOnlyDictionary<Level, IReadOnlyDictionary<Chara, int>> clearRanks)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);

                if (clearRanks.TryGetValue(level, out var ranks) && ranks.TryGetValue(chara, out var rank))
                {
                    // FIXME
                    return rank switch
                    {
                        1 => "Bronze",
                        2 => "Silver",
                        3 => "Gold",
                        _ => "Not Clear",
                    };
                }
                else
                {
                    return "Not Clear";
                }
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
