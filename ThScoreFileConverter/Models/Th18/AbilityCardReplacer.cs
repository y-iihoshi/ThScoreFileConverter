//-----------------------------------------------------------------------
// <copyright file="AbilityCardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th18
{
    // %T18ABIL[xx]
    internal class AbilityCardReplacer : IStringReplaceable
    {
        private const string Pattern = @"%T18ABIL(\d{2})";

        private readonly MatchEvaluator evaluator;

        public AbilityCardReplacer(IStatus status)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var number = IntegerHelper.Parse(match.Groups[1].Value);

                if (!Definitions.AbilityCardTable.TryGetValue(number - 1, out var card))
                    return match.ToString();

                return (status.AbilityCards.ElementAt(card.Id) > 0) ? card.Name : "??????????";
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
