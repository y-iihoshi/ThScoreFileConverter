//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacer.cs" company="None">
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
    // %T123CRG[x][yy][z]
    internal class CollectRateReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"{0}CRG({1})({2})([1-2])",
            Definitions.FormatPrefix,
            Parsers.LevelWithTotalParser.Pattern,
            Parsers.CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CollectRateReplacer(
            IReadOnlyDictionary<Chara, Th105.IClearData<Chara>> clearDataDictionary, INumberFormatter formatter)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1].Value);
                var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                var type = IntegerHelper.Parse(match.Groups[3].Value);

                if ((chara != Chara.Sanae) && (chara != Chara.Cirno) && (chara != Chara.Meiling))
                    return match.ToString();

                Func<KeyValuePair<(Chara, int), Th105.ISpellCardResult<Chara>>, bool> findByLevel = level switch
                {
                    Th105.LevelWithTotal.Total => FuncHelper.True,
                    _ => pair => pair.Value.Level == (Th105.Level)level,
                };

                Func<KeyValuePair<(Chara, int), Th105.ISpellCardResult<Chara>>, bool> countByType = type switch
                {
                    1 => pair => pair.Value.GotCount > 0,
                    _ => pair => pair.Value.TrialCount > 0,
                };

                var spellCardResults = clearDataDictionary.TryGetValue(chara, out var clearData)
                    ? clearData.SpellCardResults
                    : ImmutableDictionary<(Chara, int), Th105.ISpellCardResult<Chara>>.Empty;
                return formatter.FormatNumber(spellCardResults.Where(findByLevel).Count(countByType));
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
