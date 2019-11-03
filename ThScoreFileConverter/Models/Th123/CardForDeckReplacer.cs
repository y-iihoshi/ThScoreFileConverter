//-----------------------------------------------------------------------
// <copyright file="CardForDeckReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

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
            bool hideUntriedCards)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var chara = Parsers.CharaParser.Parse(match.Groups[1].Value);
                var cardType = Parsers.CardTypeParser.Parse(match.Groups[2].Value);
                var number = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
                var type = match.Groups[4].Value.ToUpperInvariant();

                if (chara == Chara.Oonamazu)
                    return match.ToString();

                if (cardType == Th105.CardType.System)
                {
                    if (Definitions.SystemCardNameTable.ContainsKey(number - 1))
                    {
                        var card = systemCards[number - 1];
                        if (type == "N")
                        {
                            if (hideUntriedCards)
                            {
                                if (card.MaxNumber <= 0)
                                    return "??????????";
                            }

                            return Definitions.SystemCardNameTable[number - 1];
                        }
                        else
                        {
                            return Utils.ToNumberString(card.MaxNumber);
                        }
                    }
                    else
                    {
                        return match.ToString();
                    }
                }
                else
                {
                    var key = GetCharaCardIdPair(chara, cardType, number - 1);
                    if (key != null)
                    {
                        var card = clearDataDictionary[key.Value.Chara].CardsForDeck[key.Value.CardId];
                        if (type == "N")
                        {
                            if (hideUntriedCards)
                            {
                                if (card.MaxNumber <= 0)
                                    return "??????????";
                            }

                            return Definitions.CardNameTable[key.Value];
                        }
                        else
                        {
                            return Utils.ToNumberString(card.MaxNumber);
                        }
                    }
                    else
                    {
                        return match.ToString();
                    }
                }
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);

        // serialNumber: 0-based
        private static (Chara Chara, int CardId)? GetCharaCardIdPair(Chara chara, Th105.CardType type, int serialNumber)
        {
#if false
            if (type == Th105.CardType.System)
                return null;

            Func<(Chara Chara, int CardId), bool> matchesCharaAndType;
            if (type == Th105.CardType.Skill)
                matchesCharaAndType = pair => (pair.Chara == chara) && (pair.CardId / 100 == 1);
            else
                matchesCharaAndType = pair => (pair.Chara == chara) && (pair.CardId / 100 == 2);

            return Definitions.CardNameTable.Keys.Where(matchesCharaAndType).ElementAtOrDefault(serialNumber);
#else
            if (Definitions.CardOrderTable.TryGetValue(chara, out var cardTypeIdDict))
            {
                if (cardTypeIdDict.TryGetValue(type, out var cardIds))
                {
                    if (serialNumber < cardIds.Count)
                        return (chara, cardIds[serialNumber]);
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
#endif
        }
    }
}
