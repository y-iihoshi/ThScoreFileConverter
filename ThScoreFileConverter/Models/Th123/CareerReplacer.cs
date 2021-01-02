﻿//-----------------------------------------------------------------------
// <copyright file="CareerReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th123
{
    // %T123C[xx][yy][z]
    internal class CareerReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T123C(\d{{2}})({0})([1-3])", Parsers.CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CareerReplacer(
            IReadOnlyDictionary<Chara, Th105.IClearData<Chara>> clearDataDictionary, INumberFormatter formatter)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var number = IntegerHelper.Parse(match.Groups[1].Value);
                var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                var type = IntegerHelper.Parse(match.Groups[3].Value);

                if ((chara != Chara.Sanae) && (chara != Chara.Cirno) && (chara != Chara.Meiling))
                    return match.ToString();

                Func<Th105.ISpellCardResult<Chara>, long> getValue = type switch
                {
                    1 => result => result.GotCount,
                    2 => result => result.TrialCount,
                    _ => result => result.Frames,
                };

                Func<long, string> toString = type switch
                {
                    3 => value =>
                    {
                        var time = new Time(value);
                        return Utils.Format(
                            "{0:D2}:{1:D2}.{2:D3}",
                            (time.Hours * 60) + time.Minutes,
                            time.Seconds,
                            time.Frames * 1000 / 60);
                    },
                    _ => formatter.FormatNumber,
                };

                var spellCardResults = clearDataDictionary.TryGetValue(chara, out var clearData)
                    ? clearData.SpellCardResults
                    : ImmutableDictionary<(Chara, int), Th105.ISpellCardResult<Chara>>.Empty;
                if (number == 0)
                {
                    return toString(spellCardResults.Values.Sum(getValue));
                }
                else
                {
                    var numLevels = EnumHelper<Th105.Level>.NumValues;
                    var index = (number - 1) / numLevels;
                    if (Definitions.EnemyCardIdTable.TryGetValue(chara, out var enemyCardIdPairs)
                        && (index < enemyCardIdPairs.Count()))
                    {
                        var (enemy, cardId) = enemyCardIdPairs.ElementAt(index);
                        var key = (enemy, (cardId * numLevels) + ((number - 1) % numLevels));
                        return toString(spellCardResults.TryGetValue(key, out var result) ? getValue(result) : default);
                    }
                    else
                    {
                        return match.ToString();
                    }
                }
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
