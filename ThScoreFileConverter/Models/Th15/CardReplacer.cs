﻿//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th15
{
    // %T15CARD[xxx][y]
    internal class CardReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(@"{0}CARD(\d{{3}})([NR])", Definitions.FormatPrefix);

        private readonly MatchEvaluator evaluator;

        public CardReplacer(IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, bool hideUntriedCards)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var number = IntegerHelper.Parse(match.Groups[1].Value);
                var type = match.Groups[2].Value.ToUpperInvariant();

                if (Definitions.CardTable.TryGetValue(number, out var cardInfo))
                {
                    if (type == "N")
                    {
                        if (hideUntriedCards)
                        {
                            var tried = clearDataDictionary.TryGetValue(CharaWithTotal.Total, out var clearData)
                                && clearData.GameModeData.Any(
                                    data => data.Value.Cards.TryGetValue(number, out var card) && card.HasTried);
                            if (!tried)
                                return "??????????";
                        }

                        return cardInfo.Name;
                    }
                    else
                    {
                        return cardInfo.Level.ToString();
                    }
                }
                else
                {
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
