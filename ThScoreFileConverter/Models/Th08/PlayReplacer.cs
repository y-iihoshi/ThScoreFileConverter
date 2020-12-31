//-----------------------------------------------------------------------
// <copyright file="PlayReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Text.RegularExpressions;
using static ThScoreFileConverter.Models.Th08.Parsers;

namespace ThScoreFileConverter.Models.Th08
{
    // %T08PLAY[x][yy]
    internal class PlayReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T08PLAY({0})({1}|CL|CN|PR)", LevelWithTotalParser.Pattern, CharaWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public PlayReplacer(IPlayStatus playStatus, INumberFormatter formatter)
        {
            if (playStatus is null)
                throw new ArgumentNullException(nameof(playStatus));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = LevelWithTotalParser.Parse(match.Groups[1].Value);
                var charaAndMore = match.Groups[2].Value.ToUpperInvariant();

                var playCount = (level == LevelWithTotal.Total)
                    ? playStatus.TotalPlayCount : playStatus.PlayCounts[(Level)level];

                switch (charaAndMore)
                {
                    case "CL":  // clear count
                        return formatter.FormatNumber(playCount.TotalClear);
                    case "CN":  // continue count
                        return formatter.FormatNumber(playCount.TotalContinue);
                    case "PR":  // practice count
                        return formatter.FormatNumber(playCount.TotalPractice);
                    default:
                        {
                            var chara = CharaWithTotalParser.Parse(match.Groups[2].Value);
                            return formatter.FormatNumber((chara == CharaWithTotal.Total)
                                ? playCount.TotalTrial : playCount.Trials[(Chara)chara]);
                        }
                }
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
