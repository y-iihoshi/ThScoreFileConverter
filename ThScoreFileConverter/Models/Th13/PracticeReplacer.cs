//-----------------------------------------------------------------------
// <copyright file="PracticeReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th13.StagePractice>;

namespace ThScoreFileConverter.Models.Th13
{
    // %T13PRAC[x][yy][z]
    internal class PracticeReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T13PRAC({0})({1})({2})",
            Parsers.LevelParser.Pattern,
            Parsers.CharaParser.Pattern,
            Parsers.StageParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public PracticeReplacer(
            IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, INumberFormatter formatter)
        {
            if (clearDataDictionary is null)
                throw new ArgumentNullException(nameof(clearDataDictionary));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = (LevelPractice)Parsers.LevelParser.Parse(match.Groups[1].Value);
                var chara = (CharaWithTotal)Parsers.CharaParser.Parse(match.Groups[2].Value);
                var stage = (StagePractice)Parsers.StageParser.Parse(match.Groups[3].Value);

                if (level == LevelPractice.Extra)
                    return match.ToString();
                if (stage == StagePractice.Extra)
                    return match.ToString();

                return formatter.FormatNumber(
                    clearDataDictionary.TryGetValue(chara, out var clearData)
                    && clearData.Practices.TryGetValue((level, stage), out var practice)
                    ? (practice.Score * 10) : default);
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
