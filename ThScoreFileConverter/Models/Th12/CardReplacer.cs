﻿//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<
    ThScoreFileConverter.Models.Th12.CharaWithTotal, ThScoreFileConverter.Models.Th10.StageProgress>;

namespace ThScoreFileConverter.Models.Th12
{
    // %T12CARD[xxx][y]
    internal class CardReplacer : IStringReplaceable
    {
        private const string Pattern = @"%T12CARD(\d{3})([NR])";

        private readonly MatchEvaluator evaluator;

        public CardReplacer(
            IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, bool hideUntriedCards)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var type = match.Groups[2].Value.ToUpperInvariant();

                if (Definitions.CardTable.ContainsKey(number))
                {
                    if (type == "N")
                    {
                        if (hideUntriedCards)
                        {
                            var cards = clearDataDictionary[CharaWithTotal.Total].Cards;
                            if (!cards.TryGetValue(number, out var card) || !card.HasTried)
                                return "??????????";
                        }

                        return Definitions.CardTable[number].Name;
                    }
                    else
                    {
                        return Definitions.CardTable[number].Level.ToString();
                    }
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
