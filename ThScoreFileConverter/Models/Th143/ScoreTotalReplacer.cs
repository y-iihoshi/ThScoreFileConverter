//-----------------------------------------------------------------------
// <copyright file="ScoreTotalReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th143;

// %T143SCRTL[x][y]
internal class ScoreTotalReplacer : IStringReplaceable
{
    private static readonly string Pattern = Utils.Format(
        @"{0}SCRTL({1})([1-4])", Definitions.FormatPrefix, Parsers.ItemWithTotalParser.Pattern);

    private readonly MatchEvaluator evaluator;

    public ScoreTotalReplacer(
        IReadOnlyList<IScore> scores,
        IReadOnlyDictionary<ItemWithTotal, IItemStatus> itemStatuses,
        INumberFormatter formatter)
    {
        this.evaluator = new MatchEvaluator(match =>
        {
            var item = Parsers.ItemWithTotalParser.Parse(match.Groups[1].Value);
            var type = IntegerHelper.Parse(match.Groups[2].Value);

            switch (type)
            {
                case 1:     // total score
                    return formatter.FormatNumber(scores.Sum(score => (long)((score?.HighScore * 10) ?? default)));
                case 2:     // total of challenge counts
                    if (item == ItemWithTotal.NoItem)
                    {
                        return "-";
                    }
                    else
                    {
                        return formatter.FormatNumber(
                            itemStatuses.TryGetValue(item, out var status) ? status.UseCount : default);
                    }

                case 3:     // total of cleared counts
                    {
                        return formatter.FormatNumber(
                            itemStatuses.TryGetValue(item, out var status) ? status.ClearedCount : default);
                    }

                case 4:     // num of cleared scenes
                    {
                        return formatter.FormatNumber(
                            itemStatuses.TryGetValue(item, out var status) ? status.ClearedScenes : default);
                    }

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
