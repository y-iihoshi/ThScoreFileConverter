﻿//-----------------------------------------------------------------------
// <copyright file="ShotReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models.Th095;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th095;

// %T95SHOT[x][y]
internal sealed class ShotReplacer(
    IReadOnlyDictionary<(Level Level, int Scene), (string Path, IBestShotHeader<Level> Header)> bestshots,
    INumberFormatter formatter,
    string outputFilePath)
    : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}SHOT({Parsers.LevelParser.Pattern})({Parsers.SceneParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var level = Parsers.LevelParser.Parse(match.Groups[1]);
        var scene = Parsers.SceneParser.Parse(match.Groups[2]);

        var key = (level, scene);
        if (!Definitions.SpellCards.ContainsKey(key))
            return match.ToString();

        if (bestshots.TryGetValue(key, out var bestshot) &&
            UriHelper.TryGetRelativePath(outputFilePath, bestshot.Path, out var relativePath))
        {
            var resultScore = formatter.FormatNumber(bestshot.Header.ResultScore);
            var slowRate = formatter.FormatPercent(bestshot.Header.SlowRate, 6);
            var cardName = EncodingHelper.Default.GetString([.. bestshot.Header.CardName]).TrimEnd('\0');
            var alternativeString = StringHelper.Create(
                $"ClearData: {resultScore}{Environment.NewLine}Slow: {slowRate}{Environment.NewLine}SpellName: {cardName}");
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
