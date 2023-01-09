//-----------------------------------------------------------------------
// <copyright file="ScoreReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th08;

// %T08SCR[w][xx][y][z]
internal class ScoreReplacer : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create(
        $@"{Definitions.FormatPrefix}SCR({Parsers.LevelParser.Pattern})({Parsers.CharaParser.Pattern})(\d)([\dA-G])");

    private readonly MatchEvaluator evaluator;

    public ScoreReplacer(
        IReadOnlyDictionary<(Chara Chara, Level Level), IReadOnlyList<IHighScore>> rankings,
        INumberFormatter formatter)
    {
        this.evaluator = new MatchEvaluator(match =>
        {
            var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
            var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
            var rank = IntegerHelper.ToZeroBased(IntegerHelper.Parse(match.Groups[3].Value));
            var type = match.Groups[4].Value.ToUpperInvariant();

            var key = (chara, level);
            var score = (rankings.TryGetValue(key, out var ranking) && (rank < ranking.Count))
                ? ranking[rank] : Definitions.InitialRanking[rank];
            IEnumerable<string> cardStrings;

            switch (type)
            {
                case "1":   // name
                    return EncodingHelper.Default.GetString(score.Name.ToArray()).Split('\0')[0];
                case "2":   // score
                    return formatter.FormatNumber((score.Score * 10) + score.ContinueCount);
                case "3":   // stage
                    if ((level == Level.Extra) &&
                        (EncodingHelper.Default.GetString(score.Date.ToArray()).TrimEnd('\0') == "--/--"))
                        return StageProgress.Extra.ToShortName();
                    else
                        return score.StageProgress.ToShortName();
                case "4":   // date
                    return EncodingHelper.Default.GetString(score.Date.ToArray()).TrimEnd('\0');
                case "5":   // slow rate
                    return formatter.FormatPercent(score.SlowRate, 3);
                case "6":   // play time
                    return new Time(score.PlayTime).ToString();
                case "7":   // initial number of players
                    return formatter.FormatNumber(score.PlayerNum + 1);
                case "8":   // point items
                    return formatter.FormatNumber(score.PointItem);
                case "9":   // time point
                    return formatter.FormatNumber(score.TimePoint);
                case "0":   // miss count
                    return formatter.FormatNumber(score.MissCount);
                case "A":   // bomb count
                    return formatter.FormatNumber(score.BombCount);
                case "B":   // last spell count
                    return formatter.FormatNumber(score.LastSpellCount);
                case "C":   // pause count
                    return formatter.FormatNumber(score.PauseCount);
                case "D":   // continue count
                    return formatter.FormatNumber(score.ContinueCount);
                case "E":   // human rate
                    return formatter.FormatPercent(score.HumanRate / 100.0, 2);
                case "F":   // got spell cards
                    cardStrings = score.CardFlags.Where(pair => pair.Value > 0).Select(pair =>
                    {
                        return Definitions.CardTable.TryGetValue(pair.Key, out var card)
                            ? StringHelper.Create($"No.{card.Id:D3} {card.Name}") : string.Empty;
                    });
                    return string.Join(Environment.NewLine, cardStrings);
                case "G":   // number of got spell cards
                    return formatter.FormatNumber(score.CardFlags.Values.Count(flag => flag > 0));
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
