﻿//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacer.cs" company="None">
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
using static ThScoreFileConverter.Models.Th06.Parsers;

namespace ThScoreFileConverter.Models.Th06
{
    // %T06CRG[x][y]
    internal class CollectRateReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"{0}CRG({1})([12])", Definitions.FormatPrefix, StageWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CollectRateReplacer(IReadOnlyDictionary<int, ICardAttack> cardAttacks, INumberFormatter formatter)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var stage = StageWithTotalParser.Parse(match.Groups[1].Value);
                var type = IntegerHelper.Parse(match.Groups[2].Value);

                Func<ICardAttack, bool> findByStage = stage switch
                {
                    StageWithTotal.Total => FuncHelper.True,
                    _ => attack => Definitions.CardTable.Any(
                        pair => (pair.Key == attack.CardId) && (pair.Value.Stage == (Stage)stage)),
                };

                Func<ICardAttack, bool> findByType = type switch
                {
                    1 => attack => attack.ClearCount > 0,
                    _ => attack => attack.TrialCount > 0,
                };

                return formatter.FormatNumber(
                    cardAttacks.Values.Count(FuncHelper.MakeAndPredicate(findByStage, findByType)));
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
