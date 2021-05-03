//-----------------------------------------------------------------------
// <copyright file="ClearReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Extensions;
using static ThScoreFileConverter.Models.Th08.Parsers;
using IHighScore = ThScoreFileConverter.Models.Th08.IHighScore<
    ThScoreFileConverter.Models.Th08.Chara,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th08.StageProgress>;

namespace ThScoreFileConverter.Models.Th08
{
    // %T08CLEAR[x][yy]
    internal class ClearReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"{0}CLEAR({1})({2})", Definitions.FormatPrefix, LevelParser.Pattern, CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ClearReplacer(
            IReadOnlyDictionary<(Chara Chara, Level Level), IReadOnlyList<IHighScore>> rankings,
            IReadOnlyDictionary<CharaWithTotal, IClearData> clearData)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var level = LevelParser.Parse(match.Groups[1].Value);
                var chara = CharaParser.Parse(match.Groups[2].Value);

                var key = (chara, level);
                if (rankings.TryGetValue(key, out var ranking) && ranking.Any())
                {
                    var stageProgress = ranking.Max(rank => rank.StageProgress);
                    if ((stageProgress == StageProgress.FourUncanny) || (stageProgress == StageProgress.FourPowerful))
                    {
                        return "Stage 4";
                    }
                    else if (stageProgress == StageProgress.Extra)
                    {
                        return "Not Clear";
                    }
                    else if (stageProgress == StageProgress.Clear)
                    {
                        if ((level != Level.Extra) &&
                            ((clearData[(CharaWithTotal)chara].StoryFlags[level]
                                & PlayableStages.Stage6B) != PlayableStages.Stage6B))
                            return "FinalA Clear";
                        else
                            return stageProgress.ToShortName();
                    }
                    else
                    {
                        return stageProgress.ToShortName();
                    }
                }
                else
                {
                    return "-------";
                }
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
