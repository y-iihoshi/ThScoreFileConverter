//-----------------------------------------------------------------------
// <copyright file="ScoreReplacerBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th13
{
    internal class ScoreReplacerBase<TChara> : IStringReplaceable
        where TChara : struct, Enum
    {
        private readonly string pattern;
        private readonly MatchEvaluator evaluator;

        protected ScoreReplacerBase(
            string formatPrefix,
            EnumShortNameParser<Level> levelParser,
            EnumShortNameParser<TChara> charaParser,
            Func<Level, TChara, int, Th10.IScoreData<StageProgress>> getScore,
            INumberFormatter formatter)
        {
            this.pattern = Utils.Format(
                @"{0}SCR({1})({2})(\d)([1-5])", formatPrefix, levelParser.Pattern, charaParser.Pattern);
            this.evaluator = new MatchEvaluator(match =>
            {
                var level = levelParser.Parse(match.Groups[1].Value);
                var chara = charaParser.Parse(match.Groups[2].Value);
                var rank = IntegerHelper.ToZeroBased(IntegerHelper.Parse(match.Groups[3].Value));
                var type = IntegerHelper.Parse(match.Groups[4].Value);

                var score = getScore(level, chara, rank);

                switch (type)
                {
                    case 1:     // name
                        return score.Name.Any()
                            ? Encoding.Default.GetString(score.Name.ToArray()).Split('\0')[0] : "--------";
                    case 2:     // score
                        return formatter.FormatNumber((score.Score * 10) + score.ContinueCount);
                    case 3:     // stage
                        if (score.DateTime == 0)
                            return StageProgress.None.ToShortName();
                        if (score.StageProgress == StageProgress.Extra)
                            return "Not Clear";
                        if (score.StageProgress == StageProgress.ExtraClear)
                            return StageProgress.Clear.ToShortName();
                        return score.StageProgress.ToShortName();
                    case 4:     // date & time
                        return DateTimeHelper.GetString(score.DateTime == 0 ? null : score.DateTime);
                    case 5:     // slow
                        if (score.DateTime == 0)
                            return "-----%";
                        return formatter.FormatPercent(score.SlowRate, 3);
                    default:    // unreachable
                        return match.ToString();
                }
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, this.pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
