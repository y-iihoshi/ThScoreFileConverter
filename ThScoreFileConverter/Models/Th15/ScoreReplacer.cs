//-----------------------------------------------------------------------
// <copyright file="ScoreReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th15;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th15;

// %T15SCR[v][w][xx][y][z]
internal sealed class ScoreReplacer : IStringReplaceable
{
    private static readonly IntegerParser RankParser = new(@"\d");
    private static readonly IntegerParser TypeParser = new(@"[1-6]");
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}SCR({Parsers.GameModeParser.Pattern})({Parsers.LevelParser.Pattern})({Parsers.CharaParser.Pattern})({RankParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator;

    public ScoreReplacer(
        IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, INumberFormatter formatter)
    {
        this.evaluator = new MatchEvaluator(match =>
        {
            var mode = Parsers.GameModeParser.Parse(match.Groups[1]);
            var level = (LevelWithTotal)Parsers.LevelParser.Parse(match.Groups[2]);
            var chara = (CharaWithTotal)Parsers.CharaParser.Parse(match.Groups[3]);
            var rank = IntegerHelper.ToZeroBased(RankParser.Parse(match.Groups[4]));
            var type = TypeParser.Parse(match.Groups[5]);

#if false   // FIXME
            if (level == LevelWithTotal.Extra)
                mode = GameMode.Pointdevice;
#endif

            var ranking = clearDataDictionary.TryGetValue(chara, out var clearData)
                && clearData.GameModeData.TryGetValue(mode, out var clearDataPerGameMode)
                && clearDataPerGameMode.Rankings.TryGetValue(level, out var rankings)
                && (rank < rankings.Count)
                ? rankings[rank] : new ScoreData();
            switch (type)
            {
                case 1:     // name
                    return ranking.Name.Any()
                        ? EncodingHelper.Default.GetString(ranking.Name.ToArray()).Split('\0')[0] : "--------";
                case 2:     // score
                    return formatter.FormatNumber((ranking.Score * 10) + ranking.ContinueCount);
                case 3:     // stage
                    if (ranking.DateTime == 0)
                        return Th13.StageProgress.None.ToDisplayName();
                    if (ranking.StageProgress == Th13.StageProgress.Extra)
                        return "Not Clear";
                    if (ranking.StageProgress == Th13.StageProgress.ExtraClear)
                        return Th13.StageProgress.Clear.ToDisplayName();
                    return ranking.StageProgress.ToDisplayName();
                case 4:     // date & time
                    return DateTimeHelper.GetString(ranking.DateTime == 0 ? null : ranking.DateTime);
                case 5:     // slow
                    if (ranking.DateTime == 0)
                        return "-----%";
                    return formatter.FormatPercent(ranking.SlowRate, 3);
                case 6:     // retry
                    if (ranking.DateTime == 0)
                        return "-----";
                    return formatter.FormatNumber(ranking.RetryCount);
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
