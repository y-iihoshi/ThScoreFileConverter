//-----------------------------------------------------------------------
// <copyright file="ScoreTotalReplacer.cs" company="None">
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

namespace ThScoreFileConverter.Models.Th143
{
    // %T143SCRTL[x][y]
    internal class ScoreTotalReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T143SCRTL({0})([1-4])", Parsers.ItemWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ScoreTotalReplacer(
            IReadOnlyList<IScore> scores, IReadOnlyDictionary<ItemWithTotal, IItemStatus> itemStatuses)
        {
            if (scores is null)
                throw new ArgumentNullException(nameof(scores));
            if (itemStatuses is null)
                throw new ArgumentNullException(nameof(itemStatuses));

            this.evaluator = new MatchEvaluator(match =>
            {
                var item = Parsers.ItemWithTotalParser.Parse(match.Groups[1].Value);
                var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                switch (type)
                {
                    case 1:     // total score
                        return Utils.ToNumberString(scores.Sum(score => score.HighScore * 10L));
                    case 2:     // total of challenge counts
                        if (item == ItemWithTotal.NoItem)
                        {
                            return "-";
                        }
                        else
                        {
                            return itemStatuses.TryGetValue(item, out var status)
                                ? Utils.ToNumberString(status.UseCount) : "0";
                        }

                    case 3:     // total of cleared counts
                        {
                            return itemStatuses.TryGetValue(item, out var status)
                                ? Utils.ToNumberString(status.ClearedCount) : "0";
                        }

                    case 4:     // num of cleared scenes
                        {
                            return itemStatuses.TryGetValue(item, out var status)
                                ? Utils.ToNumberString(status.ClearedScenes) : "0";
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
}
