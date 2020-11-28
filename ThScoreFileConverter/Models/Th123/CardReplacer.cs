//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th123
{
    // %T123CARD[xx][yy][z]
    internal class CardReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T123CARD(\d{{2}})({0})([NR])", Parsers.CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CardReplacer(
            IReadOnlyDictionary<Chara, Th105.IClearData<Chara>> clearDataDictionary, bool hideUntriedCards)
        {
            if (clearDataDictionary is null)
                throw new ArgumentNullException(nameof(clearDataDictionary));

            this.evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                var type = match.Groups[3].Value.ToUpperInvariant();

                if (number <= 0)
                    return match.ToString();
                if ((chara != Chara.Sanae) && (chara != Chara.Cirno) && (chara != Chara.Meiling))
                    return match.ToString();

                var numLevels = EnumHelper<Th105.Level>.NumValues;
                var index = (number - 1) / numLevels;
                if (Definitions.EnemyCardIdTable.TryGetValue(chara, out var enemyCardIdPairs)
                    && (index < enemyCardIdPairs.Count()))
                {
                    var level = (Th105.Level)((number - 1) % numLevels);
                    var enemyCardIdPair = enemyCardIdPairs.ElementAt(index);
                    if (hideUntriedCards)
                    {
                        var spellCardResults = clearDataDictionary.TryGetValue(chara, out var clearData)
                            ? clearData.SpellCardResults
                            : ImmutableDictionary<(Chara, int), Th105.ISpellCardResult<Chara>>.Empty;
                        var key = (enemyCardIdPair.Enemy, (enemyCardIdPair.CardId * numLevels) + (int)level);
                        if (!spellCardResults.TryGetValue(key, out var result) || (result.TrialCount <= 0))
                            return (type == "N") ? "??????????" : "?????";
                    }

                    return (type == "N") ? Definitions.CardNameTable[enemyCardIdPair] : level.ToString();
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
