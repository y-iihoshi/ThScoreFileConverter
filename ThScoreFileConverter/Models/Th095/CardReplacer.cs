//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th095
{
    // %T95CARD[x][y][z]
    internal class CardReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T95CARD({0})([1-9])([12])", Parsers.LevelParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CardReplacer(IReadOnlyList<IScore> scores, bool hideUntriedCards)
        {
            if (scores is null)
                throw new ArgumentNullException(nameof(scores));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                var scene = IntegerHelper.Parse(match.Groups[2].Value);
                var type = IntegerHelper.Parse(match.Groups[3].Value);

                var key = (level, scene);
                if (!Definitions.SpellCards.TryGetValue(key, out var enemyCardPair))
                    return match.ToString();

                if (hideUntriedCards)
                {
                    var score = scores.FirstOrDefault(elem => (elem is not null) && elem.LevelScene.Equals(key));
                    if (score is null)
                        return "??????????";
                }

                return (type == 1) ? enemyCardPair.Enemy.ToLongName() : enemyCardPair.Card;
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
