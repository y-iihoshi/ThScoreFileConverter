//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
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

namespace ThScoreFileConverter.Models.Th075
{
    // %T75CARD[xxx][yy][z]
    internal class CardReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T75CARD(\d{{3}})({0})([NR])", Parsers.CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CardReplacer(
            IReadOnlyDictionary<(CharaWithReserved chara, Level level), IClearData> clearData, bool hideUntriedCards)
        {
            if (clearData is null)
                throw new ArgumentNullException(nameof(clearData));

            this.evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                var type = match.Groups[3].Value.ToUpperInvariant();

                if (chara == Chara.Meiling)
                    return match.ToString();

                if ((number > 0) && (number <= Definitions.CardIdTable[chara].Count()))
                {
                    if (hideUntriedCards)
                    {
                        var dataList = clearData
                            .Where(pair => pair.Key.chara == (CharaWithReserved)chara).Select(pair => pair.Value);
                        if (dataList.All(data => data.CardTrialCount[number - 1] <= 0))
                            return (type == "N") ? "??????????" : "?????";
                    }

                    var cardId = Definitions.CardIdTable[chara].ElementAt(number - 1);
                    return (type == "N")
                        ? Definitions.CardTable[cardId].Name : Definitions.CardTable[cardId].Level.ToString();
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
