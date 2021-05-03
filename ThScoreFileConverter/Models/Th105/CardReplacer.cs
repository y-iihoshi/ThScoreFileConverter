//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th105
{
    // %T105CARD[xxx][yy][z]
    internal class CardReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"{0}CARD(\d{{3}})({1})([NR])", Definitions.FormatPrefix, Parsers.CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CardReplacer(IReadOnlyDictionary<Chara, IClearData<Chara>> clearDataDictionary, bool hideUntriedCards)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var number = IntegerHelper.Parse(match.Groups[1].Value);
                var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                var type = match.Groups[3].Value.ToUpperInvariant();

                if (number <= 0)
                    return match.ToString();

                var numLevels = EnumHelper<Level>.NumValues;
                var index = (number - 1) / numLevels;
                if (Definitions.EnemyCardIdTable.TryGetValue(chara, out var enemyCardIdPairs)
                    && (index < enemyCardIdPairs.Count()))
                {
                    var level = (Level)((number - 1) % numLevels);
                    var enemyCardIdPair = enemyCardIdPairs.ElementAt(index);
                    if (hideUntriedCards)
                    {
                        var spellCardResults = clearDataDictionary.TryGetValue(chara, out var clearData)
                            ? clearData.SpellCardResults
                            : ImmutableDictionary<(Chara, int), ISpellCardResult<Chara>>.Empty;
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
