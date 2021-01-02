﻿//-----------------------------------------------------------------------
// <copyright file="CardForDeckReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th123
{
    // %T123DC[ww][x][yy][z]
    internal class CardForDeckReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T123DC({0})({1})(\d{{2}})([NC])", Parsers.CharaParser.Pattern, Parsers.CardTypeParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CardForDeckReplacer(
            IReadOnlyDictionary<int, Th105.ICardForDeck> systemCards,
            IReadOnlyDictionary<Chara, Th105.IClearData<Chara>> clearDataDictionary,
            INumberFormatter formatter,
            bool hideUntriedCards)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var chara = Parsers.CharaParser.Parse(match.Groups[1].Value);
                var cardType = Parsers.CardTypeParser.Parse(match.Groups[2].Value);
                var number = IntegerHelper.Parse(match.Groups[3].Value);
                var type = match.Groups[4].Value.ToUpperInvariant();

                if (chara == Chara.Oonamazu)
                    return match.ToString();

                Th105.ICardForDeck cardForDeck;
                string cardName;

                if (cardType == Th105.CardType.System)
                {
                    if (Definitions.SystemCardNameTable.TryGetValue(number - 1, out var name))
                    {
                        cardForDeck = systemCards.TryGetValue(number - 1, out var card)
                            ? card : new Th105.CardForDeck();
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
                            ? card : new Th105.CardForDeck();
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
                Chara chara, Th105.CardType cardType, int serialNumber, out (Chara Chara, int CardId) charaCardIdPair)
            {
                if (Definitions.CardOrderTable.TryGetValue(chara, out var cardTypeIdDict)
                    && cardTypeIdDict.TryGetValue(cardType, out var cardIds)
                    && serialNumber < cardIds.Count)
                {
                    charaCardIdPair = (chara, cardIds[serialNumber]);
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
