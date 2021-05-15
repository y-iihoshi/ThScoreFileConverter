//-----------------------------------------------------------------------
// <copyright file="ClearReplacerBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th06
{
    internal class ClearReplacerBase<TLevel, TChara, TStageProgress> : IStringReplaceable
        where TLevel : struct, Enum
        where TChara : struct, Enum
        where TStageProgress : struct, Enum
    {
        private readonly string pattern;
        private readonly MatchEvaluator evaluator;

        public ClearReplacerBase(
            string formatPrefix,
            EnumShortNameParser<TLevel> levelParser,
            EnumShortNameParser<TChara> charaParser,
            Func<TLevel, TChara, IReadOnlyList<IHighScore<TChara, TLevel, TStageProgress>>?> getRanking,
            Func<TStageProgress, bool> isAdditionalStage)
        {
            this.pattern = Utils.Format(@"{0}CLEAR({1})({2})", formatPrefix, levelParser.Pattern, charaParser.Pattern);
            this.evaluator = new MatchEvaluator(match =>
            {
                var level = levelParser.Parse(match.Groups[1].Value);
                var chara = charaParser.Parse(match.Groups[2].Value);

                if (getRanking(level, chara) is { } ranking)
                {
                    var stageProgress = ranking.Count > 0 ? ranking.Max(static score => score.StageProgress) : default;
                    return isAdditionalStage(stageProgress) ? "Not Clear" : stageProgress.ToShortName();
                }
                else
                {
                    return StageProgress.None.ToShortName();
                }
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, this.pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
