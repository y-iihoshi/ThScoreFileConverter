//-----------------------------------------------------------------------
// <copyright file="CardReplacerBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th095
{
    internal class CardReplacerBase<TLevel, TEnemy> : IStringReplaceable
        where TLevel : struct, Enum
        where TEnemy : struct, Enum
    {
        private readonly string pattern;
        private readonly MatchEvaluator evaluator;

        protected CardReplacerBase(
            string formatPrefix,
            EnumShortNameParser<TLevel> levelParser,
            IReadOnlyDictionary<(TLevel Level, int Scene), (TEnemy Enemy, string Card)> spellCards,
            bool hideUntriedCards,
            Func<TLevel, int, bool> levelSceneHasTried)
        {
            this.pattern = Utils.Format(@"{0}CARD({1})([1-9])([12])", formatPrefix, levelParser.Pattern);
            this.evaluator = new MatchEvaluator(match =>
            {
                var level = levelParser.Parse(match.Groups[1].Value);
                var scene = IntegerHelper.Parse(match.Groups[2].Value);
                var type = IntegerHelper.Parse(match.Groups[3].Value);

                if (!spellCards.TryGetValue((level, scene), out var enemyCardPair))
                    return match.ToString();

                if (hideUntriedCards && !levelSceneHasTried(level, scene))
                    return "??????????";

                return (type == 1) ? enemyCardPair.Enemy.ToLongName() : enemyCardPair.Card;
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, this.pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
