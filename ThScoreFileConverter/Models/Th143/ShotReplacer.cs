//-----------------------------------------------------------------------
// <copyright file="ShotReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models.Th143;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th143;

// %T143SHOT[x][y]
internal sealed class ShotReplacer(
    IReadOnlyDictionary<(Day Day, int Scene), (string Path, IBestShotHeader Header)> bestshots, string outputFilePath)
    : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}SHOT({Parsers.DayParser.Pattern})([0-9])");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var day = Parsers.DayParser.Parse(match.Groups[1]);
        var scene = IntegerHelper.Parse(match.Groups[2].Value);
        scene = (scene == 0) ? 10 : scene;

        var key = (day, scene);
        if (!Definitions.SpellCards.TryGetValue(key, out var enemyCardPair))
            return match.ToString();

        if (bestshots.TryGetValue(key, out var bestshot) &&
            UriHelper.TryGetRelativePath(outputFilePath, bestshot.Path, out var relativePath))
        {
            var alternativeString = StringHelper.Create($"SpellName: {enemyCardPair.Card}");
            return StringHelper.Create(
                $"""<img src="{relativePath}" alt="{alternativeString}" title="{alternativeString}" border=0>""");
        }
        else
        {
            return string.Empty;
        }
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
