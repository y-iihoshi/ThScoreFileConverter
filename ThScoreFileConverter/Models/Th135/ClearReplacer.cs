//-----------------------------------------------------------------------
// <copyright file="ClearReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models.Th135;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th135;

// %T135CLEAR[x][yy]
internal sealed class ClearReplacer(IReadOnlyDictionary<Chara, Levels> storyClearFlags) : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}CLEAR({Parsers.LevelParser.Pattern})({Parsers.CharaParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var level = Parsers.LevelParser.Parse(match.Groups[1]);
        var chara = Parsers.CharaParser.Parse(match.Groups[2]);

        var cleared = false;
        if (storyClearFlags.TryGetValue(chara, out var flags))
        {
            switch (level)
            {
                case Level.Easy:
                    cleared = (flags & Levels.Easy) == Levels.Easy;
                    break;
                case Level.Normal:
                    cleared = (flags & Levels.Normal) == Levels.Normal;
                    break;
                case Level.Hard:
                    cleared = (flags & Levels.Hard) == Levels.Hard;
                    break;
                case Level.Lunatic:
                    cleared = (flags & Levels.Lunatic) == Levels.Lunatic;
                    break;
                default:    // unreachable
                    break;
            }
        }

        return cleared ? "Clear" : "Not Clear";
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
