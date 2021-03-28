//-----------------------------------------------------------------------
// <copyright file="ScoreReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th16.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.LevelWithTotal,
    ThScoreFileConverter.Models.Th16.StagePractice,
    ThScoreFileConverter.Models.Th16.IScoreData>;

namespace ThScoreFileConverter.Models.Th16
{
    // %T16SCR[w][xx][y][z]
    internal class ScoreReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T16SCR({0})({1})(\d)([1-6])", Parsers.LevelParser.Pattern, Parsers.CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ScoreReplacer(
            IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, INumberFormatter formatter)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var level = (LevelWithTotal)Parsers.LevelParser.Parse(match.Groups[1].Value);
                var chara = (CharaWithTotal)Parsers.CharaParser.Parse(match.Groups[2].Value);
                var rank = IntegerHelper.ToZeroBased(IntegerHelper.Parse(match.Groups[3].Value));
                var type = IntegerHelper.Parse(match.Groups[4].Value);

                var ranking = clearDataDictionary.TryGetValue(chara, out var clearData)
                    && clearData.Rankings.TryGetValue(level, out var rankings)
                    && (rank < rankings.Count)
                    ? rankings[rank] : new ScoreData();
                switch (type)
                {
                    case 1:     // name
                        return ranking.Name.Any()
                            ? Encoding.Default.GetString(ranking.Name.ToArray()).Split('\0')[0] : "--------";
                    case 2:     // score
                        return formatter.FormatNumber((ranking.Score * 10) + ranking.ContinueCount);
                    case 3:     // stage
                        if (ranking.DateTime == 0)
                            return Th13.StageProgress.None.ToShortName();
                        if (ranking.StageProgress == Th13.StageProgress.Extra)
                            return "Not Clear";
                        if (ranking.StageProgress == Th13.StageProgress.ExtraClear)
                            return Th13.StageProgress.Clear.ToShortName();
                        return ranking.StageProgress.ToShortName();
                    case 4:     // date & time
                        return DateTimeHelper.GetString(ranking.DateTime == 0 ? null : ranking.DateTime);
                    case 5:     // slow
                        if (ranking.DateTime == 0)
                            return "-----%";
                        return formatter.FormatPercent(ranking.SlowRate, 3);
                    case 6:     // season
                        if (ranking.DateTime == 0)
                            return "-----";
                        return ranking.Season.ToShortName();
                    default:    // unreachable
                        return match.ToString();
                }
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
