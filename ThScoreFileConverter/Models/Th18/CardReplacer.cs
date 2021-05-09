﻿//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th18.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Stage,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;

namespace ThScoreFileConverter.Models.Th18
{
    // %T18CARD[xxx][y]
    internal class CardReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(@"{0}CARD(\d{{2}})([NR])", Definitions.FormatPrefix);

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
                            if (!clearDataDictionary.TryGetValue(CharaWithTotal.Total, out var clearData)
                                || !clearData.Cards.TryGetValue(number, out var card)
                                || !card.HasTried)
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