//-----------------------------------------------------------------------
// <copyright file="CareerReplacer.cs" company="None">
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

namespace ThScoreFileConverter.Models.Th123
{
    // %T123C[xx][yy][z]
    internal class CareerReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T123C(\d{{2}})({0})([1-3])", Parsers.CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CareerReplacer(IReadOnlyDictionary<Chara, Th105.IClearData<Chara>> clearDataDictionary)
        {
            if (clearDataDictionary is null)
                throw new ArgumentNullException(nameof(clearDataDictionary));

            this.evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                if ((chara != Chara.Sanae) && (chara != Chara.Cirno) && (chara != Chara.Meiling))
                    return match.ToString();

                Func<Th105.ISpellCardResult<Chara>, long> getValue;
                Func<long, string> toString;
                if (type == 1)
                {
                    getValue = result => result.GotCount;
                    toString = Utils.ToNumberString;
                }
                else if (type == 2)
                {
                    getValue = result => result.TrialCount;
                    toString = Utils.ToNumberString;
                }
                else
                {
                    getValue = result => result.Frames;
                    toString = value =>
                    {
                        var time = new Time(value);
                        return Utils.Format(
                            "{0:D2}:{1:D2}.{2:D3}",
                            (time.Hours * 60) + time.Minutes,
                            time.Seconds,
                            time.Frames * 1000 / 60);
                    };
                }

                var spellCardResults = clearDataDictionary.TryGetValue(chara, out var clearData)
                    ? clearData.SpellCardResults : new Dictionary<(Chara, int), Th105.ISpellCardResult<Chara>>();
                if (number == 0)
                {
                    return toString(spellCardResults.Values.Sum(getValue));
                }
                else
                {
                    var numLevels = Enum.GetValues(typeof(Th105.Level)).Length;
                    var index = (number - 1) / numLevels;
                    if (Definitions.EnemyCardIdTable.TryGetValue(chara, out var enemyCardIdPairs)
                        && (index < enemyCardIdPairs.Count()))
                    {
                        var (enemy, cardId) = enemyCardIdPairs.ElementAt(index);
                        var key = (enemy, (cardId * numLevels) + ((number - 1) % numLevels));
                        return toString(spellCardResults.TryGetValue(key, out var result) ? getValue(result) : 0);
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
