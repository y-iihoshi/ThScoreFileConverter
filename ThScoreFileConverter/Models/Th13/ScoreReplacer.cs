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
using ThScoreFileConverter.Extensions;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th13.StagePractice>;

namespace ThScoreFileConverter.Models.Th13
{
    // %T13SCR[w][xx][y][z]
    internal class ScoreReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T13SCR({0})({1})(\d)([1-5])", Parsers.LevelParser.Pattern, Parsers.CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ScoreReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary)
        {
            if (clearDataDictionary is null)
                throw new ArgumentNullException(nameof(clearDataDictionary));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = (LevelPracticeWithTotal)Parsers.LevelParser.Parse(match.Groups[1].Value);
                var chara = (CharaWithTotal)Parsers.CharaParser.Parse(match.Groups[2].Value);
                var rank = Utils.ToZeroBased(int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                var ranking = clearDataDictionary.TryGetValue(chara, out var clearData)
                    && clearData.Rankings.TryGetValue(level, out var rankings)
                    && (rank < rankings.Count)
                    ? rankings[rank] : new ScoreData();
                switch (type)
                {
                    case 1:     // name
                        return (ranking.Name != null)
                            ? Encoding.Default.GetString(ranking.Name.ToArray()).Split('\0')[0] : "--------";
                    case 2:     // score
                        return Utils.ToNumberString((ranking.Score * 10) + ranking.ContinueCount);
                    case 3:     // stage
                        if (ranking.DateTime == 0)
                            return StageProgress.None.ToShortName();
                        if (ranking.StageProgress == StageProgress.Extra)
                            return "Not Clear";
                        if (ranking.StageProgress == StageProgress.ExtraClear)
                            return StageProgress.Clear.ToShortName();
                        return ranking.StageProgress.ToShortName();
                    case 4:     // date & time
                        if (ranking.DateTime == 0)
                            return "----/--/-- --:--:--";
                        return new DateTime(1970, 1, 1).AddSeconds(ranking.DateTime).ToLocalTime()
                            .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                    case 5:     // slow
                        if (ranking.DateTime == 0)
                            return "-----%";
                        return Utils.Format("{0:F3}%", ranking.SlowRate);
                    default:    // unreachable
                        return match.ToString();
                }
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
