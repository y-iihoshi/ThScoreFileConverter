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
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th17;

internal class AchievementReplacerBase : IStringReplaceable
{
    private static readonly IntegerParser NumberParser = new(@"\d{2}");

    private readonly string pattern;
    private readonly MatchEvaluator evaluator;

    protected AchievementReplacerBase(string formatPrefix, IReadOnlyList<string> achievementNames, IAchievementHolder achievementHolder)
    {
        this.pattern = StringHelper.Create($@"{formatPrefix}ACHV({NumberParser.Pattern})");
        this.evaluator = new MatchEvaluator(match =>
        {
            var number = NumberParser.Parse(match.Groups[1]);

            if (number <= 0 || number > achievementNames.Count)
                return match.ToString();

            var index = number - 1;
            return (achievementHolder.Achievements.ElementAt(index) > 0) ? achievementNames[index] : "??????????";
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, this.pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
