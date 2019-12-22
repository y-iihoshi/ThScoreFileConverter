//-----------------------------------------------------------------------
// <copyright file="PlayReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Text.RegularExpressions;
using static ThScoreFileConverter.Models.Th07.Parsers;

namespace ThScoreFileConverter.Models.Th07
{
    // %T07PLAY[x][yy]
    internal class PlayReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T07PLAY({0})({1}|CL|CN|PR|RT)", LevelWithTotalParser.Pattern, CharaWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public PlayReplacer(PlayStatus playStatus)
        {
            if (playStatus is null)
                throw new ArgumentNullException(nameof(playStatus));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = LevelWithTotalParser.Parse(match.Groups[1].Value);
                var charaAndMore = match.Groups[2].Value.ToUpperInvariant();

                var playCount = playStatus.PlayCounts[level];
                switch (charaAndMore)
                {
                    case "CL":  // clear count
                        return Utils.ToNumberString(playCount.TotalClear);
                    case "CN":  // continue count
                        return Utils.ToNumberString(playCount.TotalContinue);
                    case "PR":  // practice count
                        return Utils.ToNumberString(playCount.TotalPractice);
                    case "RT":  // retry count
                        return Utils.ToNumberString(playCount.TotalRetry);
                    default:
                        {
                            var chara = CharaWithTotalParser.Parse(match.Groups[2].Value);
                            return Utils.ToNumberString((chara == CharaWithTotal.Total)
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
