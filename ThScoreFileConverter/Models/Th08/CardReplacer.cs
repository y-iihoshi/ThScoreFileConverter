//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th08
{
    // %T08CARD[xxx][y]
    internal class CardReplacer : IStringReplaceable
    {
        private const string Pattern = @"%T08CARD(\d{3})([NR])";

        private readonly MatchEvaluator evaluator;

        public CardReplacer(IReadOnlyDictionary<int, ICardAttack> cardAttacks, bool hideUntriedCards)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var number = IntegerHelper.Parse(match.Groups[1].Value);
                var type = match.Groups[2].Value.ToUpperInvariant();

                if (Definitions.CardTable.TryGetValue(number, out var cardInfo))
                {
                    if (hideUntriedCards)
                    {
                        if (!cardAttacks.TryGetValue(number, out var attack) || !attack.HasTried())
                            return (type == "N") ? "??????????" : "?????";
                    }

                    if (type == "N")
                    {
                        return cardInfo.Name;
                    }
                    else
                    {
                        var levelName = cardInfo.Level.ToLongName();
                        return (levelName.Length > 0) ? levelName : cardInfo.Level.ToString();
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
