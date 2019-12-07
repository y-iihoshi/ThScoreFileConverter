//-----------------------------------------------------------------------
// <copyright file="AchievementReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th17
{
    // %T17ACHV[xx]
    internal class AchievementReplacer : IStringReplaceable
    {
        private const string Pattern = @"%T17ACHV(\d{2})";

        private readonly MatchEvaluator evaluator;

        public AchievementReplacer(IStatus status)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);

                if (number <= 0 || number > Definitions.Achievements.Count)
                    return match.ToString();

                return (status.Achievements.ElementAt(number - 1) > 0)
                    ? Definitions.Achievements[number - 1] : "??????????";
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
