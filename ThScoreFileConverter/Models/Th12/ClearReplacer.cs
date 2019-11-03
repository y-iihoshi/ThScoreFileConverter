//-----------------------------------------------------------------------
// <copyright file="ClearReplacer.cs" company="None">
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
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<
    ThScoreFileConverter.Models.Th12.CharaWithTotal, ThScoreFileConverter.Models.Th10.StageProgress>;

namespace ThScoreFileConverter.Models.Th12
{
    // %T12CLEAR[x][yy]
    internal class ClearReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T12CLEAR({0})({1})", Parsers.LevelParser.Pattern, Parsers.CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ClearReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary)
        {
            if (clearDataDictionary is null)
                throw new ArgumentNullException(nameof(clearDataDictionary));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                var chara = (CharaWithTotal)Parsers.CharaParser.Parse(match.Groups[2].Value);

                var scores = clearDataDictionary.TryGetValue(chara, out var clearData)
                    && clearData.Rankings.TryGetValue(level, out var ranking)
                    ? ranking.Where(score => score.DateTime > 0)
                    : new List<Th10.IScoreData<Th10.StageProgress>>();
                var stageProgress = scores.Any() ? scores.Max(score => score.StageProgress) : Th10.StageProgress.None;

                return (stageProgress == Th10.StageProgress.Extra) ? "Not Clear" : stageProgress.ToShortName();
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
