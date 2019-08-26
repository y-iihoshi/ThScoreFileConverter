//-----------------------------------------------------------------------
// <copyright file="ClearReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static ThScoreFileConverter.Models.Th06.Parsers;

namespace ThScoreFileConverter.Models.Th06
{
    // %T06CLEAR[x][yy]
    internal class ClearReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T06CLEAR({0})({1})", LevelParser.Pattern, CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ClearReplacer(IReadOnlyDictionary<(Chara, Level), List<HighScore>> rankings)
        {
            if (rankings is null)
                throw new ArgumentNullException(nameof(rankings));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = LevelParser.Parse(match.Groups[1].Value);
                var chara = CharaParser.Parse(match.Groups[2].Value);

                var key = (chara, level);
                if (rankings.TryGetValue(key, out var ranking))
                {
                    var stageProgress = ranking.Select(rank => rank.StageProgress).DefaultIfEmpty().Max();
                    if (stageProgress == StageProgress.Extra)
                        return "Not Clear";
                    else
                        return stageProgress.ToShortName();
                }
                else
                {
                    return StageProgress.None.ToShortName();
                }
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
