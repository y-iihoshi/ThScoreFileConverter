﻿//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th06
{
    // %T06CARD[xx][y]
    internal class CardReplacer : IStringReplaceable
    {
        private const string Pattern = @"%T06CARD(\d{2})([NR])";

        private readonly MatchEvaluator evaluator;

        public CardReplacer(IReadOnlyDictionary<int, CardAttack> cardAttacks, bool hideUntriedCards)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var type = match.Groups[2].Value.ToUpperInvariant();

                if (Definitions.CardTable.ContainsKey(number))
                {
                    if (hideUntriedCards)
                    {
                        if (!cardAttacks.TryGetValue(number, out var attack) || !attack.HasTried())
                            return type == "N" ? "??????????" : "?????";
                    }

                    return type == "N"
                        ? Definitions.CardTable[number].Name
                        : string.Join(", ", Definitions.CardTable[number].Levels.Select(lv => lv.ToString()).ToArray());
                }
                else
                {
                    return match.ToString();
                }
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
