//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th165
{
    // %T165CARD[xx][y][z]
    internal class CardReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T165CARD({0})([1-7])([12])", Parsers.DayParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CardReplacer(IReadOnlyList<IScore> scores, bool hideUntriedCards)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var day = Parsers.DayParser.Parse(match.Groups[1].Value);
                var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                var key = (day, scene);
                if (!Definitions.SpellCards.ContainsKey(key))
                    return match.ToString();

                if (hideUntriedCards)
                {
                    var score = scores.FirstOrDefault(elem =>
                        (elem != null) &&
                        (elem.Number >= 0) &&
                        (elem.Number < Definitions.SpellCards.Count) &&
                        Definitions.SpellCards.ElementAt(elem.Number).Key.Equals(key));
                    if ((score == null) || (score.ChallengeCount <= 0))
                        return "??????????";
                }

                if (type == 1)
                {
                    return string.Join(
                        " &amp; ", Definitions.SpellCards[key].Enemies.Select(enemy => enemy.ToLongName()).ToArray());
                }
                else
                {
                    return Definitions.SpellCards[key].Card;
                }
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
