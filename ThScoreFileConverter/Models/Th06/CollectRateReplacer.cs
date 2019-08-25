//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacer.cs" company="None">
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
using static ThScoreFileConverter.Models.Th06.Parsers;

namespace ThScoreFileConverter.Models.Th06
{
    // %T06CRG[x][y]
    internal class CollectRateReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T06CRG({0})([12])", StageWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CollectRateReplacer(IReadOnlyDictionary<int, CardAttack> cardAttacks)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var stage = StageWithTotalParser.Parse(match.Groups[1].Value);
                var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                Func<CardAttack, bool> findByStage;
                if (stage == StageWithTotal.Total)
                    findByStage = attack => true;
                else
                    findByStage = attack => Definitions.CardTable[attack.CardId].Stage == (Stage)stage;

                Func<CardAttack, bool> findByType;
                if (type == 1)
                    findByType = attack => attack.ClearCount > 0;
                else
                    findByType = attack => attack.TrialCount > 0;

                return cardAttacks.Values
                    .Count(Utils.MakeAndPredicate(findByStage, findByType))
                    .ToString(CultureInfo.CurrentCulture);
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
