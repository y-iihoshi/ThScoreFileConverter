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
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th075
{
    // %T75SCR[w][xx][y][z]
    internal class ScoreReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T75SCR({0})({1})(\d)([1-3])", Parsers.LevelParser.Pattern, Parsers.CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ScoreReplacer(IReadOnlyDictionary<(CharaWithReserved, Level), IClearData> clearData)
        {
            if (clearData is null)
                throw new ArgumentNullException(nameof(clearData));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                var rank = Utils.ToZeroBased(int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                if (chara == Chara.Meiling)
                    return match.ToString();

                var key = ((CharaWithReserved)chara, level);
                var score = clearData.TryGetValue(key, out var data) && rank < data.Ranking.Count
                    ? data.Ranking[rank] : new HighScore();

                return type switch
                {
                    1 => score.Name,
                    2 => Utils.ToNumberString(score.Score),
                    3 => Utils.Format("{0:D2}/{1:D2}", score.Month, score.Day),
                    _ => match.ToString(),  // unreachable
                };
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
