//-----------------------------------------------------------------------
// <copyright file="CardForDeckReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th105
{
    // %T105DC[ww][x][yy][z]
    internal class CardForDeckReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T105DC({0})({1})(\d{{2}})([NC])", Parsers.CharaParser.Pattern, Parsers.CardTypeParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CardForDeckReplacer(
            IReadOnlyDictionary<int, ICardForDeck> systemCards,
            IReadOnlyDictionary<Chara, IClearData<Chara>> clearDataDictionary,
            INumberFormatter formatter,
            bool hideUntriedCards)
        {
            if (systemCards is null)
                throw new ArgumentNullException(nameof(systemCards));
            if (clearDataDictionary is null)
                throw new ArgumentNullException(nameof(clearDataDictionary));

            this.evaluator = new MatchEvaluator(match =>
            {
                var chara = Parsers.CharaParser.Parse(match.Groups[1].Value);
                var cardType = Parsers.CardTypeParser.Parse(match.Groups[2].Value);
                var number = IntegerHelper.Parse(match.Groups[3].Value);
                var type = match.Groups[4].Value.ToUpperInvariant();

                ICardForDeck cardForDeck;
                string cardName;

                if (cardType == CardType.System)
                {
                    if (Definitions.SystemCardNameTable.TryGetValue(number - 1, out var name))
                    {
                        cardForDeck = systemCards.TryGetValue(number - 1, out var card) ? card : new CardForDeck();
                        cardName = name;
                    }
                    else
                    {
                        return match.ToString();
                    }
                }
                else
                {
                    if (TryGetCharaCardIdPair(chara, cardType, number - 1, out var key)
                        && Definitions.CardNameTable.TryGetValue(key, out var name))
                    {
                        cardForDeck = clearDataDictionary.TryGetValue(key.Chara, out var clearData)
                            && clearData.CardsForDeck.TryGetValue(key.CardId, out var card)
                            ? card : new CardForDeck();
                        cardName = name;
                    }
                    else
                    {
                        return match.ToString();
                    }
                }

                if (type == "N")
                {
                    if (hideUntriedCards)
                    {
                        if (cardForDeck.MaxNumber <= 0)
                            return "??????????";
                    }

                    return cardName;
                }
                else
                {
                    return formatter.FormatNumber(cardForDeck.MaxNumber);
                }
            });

            // serialNumber : 0-based
            static bool TryGetCharaCardIdPair(
                Chara chara, CardType cardType, int serialNumber, out (Chara Chara, int CardId) charaCardIdPair)
            {
                var charaCardIdPairs = Definitions.CardNameTable.Keys.Where(
                    pair => (pair.Chara == chara) && (pair.CardId / 100 == (int)cardType));

                if (serialNumber < charaCardIdPairs.Count())
                {
                    charaCardIdPair = charaCardIdPairs.ElementAt(serialNumber);
                    return true;
                }

                charaCardIdPair = default;
                return false;
            }
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
