﻿//-----------------------------------------------------------------------
// <copyright file="AchievementReplacer.cs" company="None">
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
    // %T18ACHV[xx]
    internal class AchievementReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(@"{0}ACHV(\d{{2}})", Definitions.FormatPrefix);

        private readonly MatchEvaluator evaluator;

        public AchievementReplacer(IStatus status)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var number = IntegerHelper.Parse(match.Groups[1].Value);

                if (number <= 0 || number > Definitions.Achievements.Count)
                    return match.ToString();

                return (status.Achievements.ElementAt(number - 1) > 0)
                    ? Definitions.Achievements[number - 1] : "??????????";
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
