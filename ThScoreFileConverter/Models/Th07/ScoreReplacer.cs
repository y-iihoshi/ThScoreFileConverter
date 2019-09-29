//-----------------------------------------------------------------------
// <copyright file="ScoreReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using static ThScoreFileConverter.Models.Th07.Parsers;

namespace ThScoreFileConverter.Models.Th07
{
    // %T07SCR[w][xx][y][z]
    internal class ScoreReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T07SCR({0})({1})(\d)([1-5])", LevelParser.Pattern, CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ScoreReplacer(IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> rankings)
        {
            if (rankings is null)
                throw new ArgumentNullException(nameof(rankings));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = LevelParser.Parse(match.Groups[1].Value);
                var chara = CharaParser.Parse(match.Groups[2].Value);
                var rank = Utils.ToZeroBased(int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                var key = (chara, level);
                var score = (rankings.TryGetValue(key, out var ranking) && (rank < ranking.Count))
                    ? ranking[rank] : Definitions.InitialRanking[rank];

                switch (type)
                {
                    case 1:     // name
                        return Encoding.Default.GetString(score.Name.ToArray()).Split('\0')[0];
                    case 2:     // score
                        return Utils.ToNumberString((score.Score * 10) + score.ContinueCount);
                    case 3:     // stage
                        return score.StageProgress.ToShortName();
                    case 4:     // date
                        return Encoding.Default.GetString(score.Date.ToArray()).TrimEnd('\0');
                    case 5:     // slow rate
                        return Utils.Format("{0:F3}%", score.SlowRate);
                    default:    // unreachable
                        return match.ToString();
                }
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
