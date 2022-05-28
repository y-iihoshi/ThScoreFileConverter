//-----------------------------------------------------------------------
// <copyright file="ClearReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th15;

namespace ThScoreFileConverter.Models.Th15;

// %T15CLEAR[x][y][zz]
internal class ClearReplacer : IStringReplaceable
{
    private static readonly string Pattern = Utils.Format(
        @"{0}CLEAR({1})({2})({3})",
        Definitions.FormatPrefix,
        Parsers.GameModeParser.Pattern,
        Parsers.LevelParser.Pattern,
        Parsers.CharaParser.Pattern);

    private readonly MatchEvaluator evaluator;

    public ClearReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary)
    {
        this.evaluator = new MatchEvaluator(match =>
        {
            var mode = Parsers.GameModeParser.Parse(match.Groups[1].Value);
            var level = (LevelWithTotal)Parsers.LevelParser.Parse(match.Groups[2].Value);
            var chara = (CharaWithTotal)Parsers.CharaParser.Parse(match.Groups[3].Value);

#if false   // FIXME
            if (level == LevelWithTotal.Extra)
                mode = GameMode.Pointdevice;
#endif

            var scores = clearDataDictionary.TryGetValue(chara, out var clearData)
                && clearData.GameModeData.TryGetValue(mode, out var clearDataPerGameMode)
                && clearDataPerGameMode.Rankings.TryGetValue(level, out var ranking)
                ? ranking.Where(score => score.DateTime > 0)
                : ImmutableList<IScoreData>.Empty;
            var stageProgress = scores.Any() ? scores.Max(score => score.StageProgress) : Th13.StageProgress.None;

            if (stageProgress == Th13.StageProgress.Extra)
                return "Not Clear";
            else if (stageProgress == Th13.StageProgress.ExtraClear)
                return Th13.StageProgress.Clear.ToShortName();
            else
                return stageProgress.ToShortName();
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
