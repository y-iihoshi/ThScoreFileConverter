//-----------------------------------------------------------------------
// <copyright file="AchievementReplacerBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th17
{
    internal class AchievementReplacerBase : IStringReplaceable
    {
        private readonly string pattern;
        private readonly MatchEvaluator evaluator;

        protected AchievementReplacerBase(string formatPrefix, IReadOnlyList<string> achievementNames, IStatus status)
        {
            this.pattern = Utils.Format(@"{0}ACHV(\d{{2}})", formatPrefix);
            this.evaluator = new MatchEvaluator(match =>
            {
                var number = IntegerHelper.Parse(match.Groups[1].Value);

                if (number <= 0 || number > achievementNames.Count)
                    return match.ToString();

                return (status.Achievements.ElementAt(number - 1) > 0) ? achievementNames[number - 1] : "??????????";
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, this.pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
