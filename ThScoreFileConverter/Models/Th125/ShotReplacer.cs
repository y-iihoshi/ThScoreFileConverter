//-----------------------------------------------------------------------
// <copyright file="ShotReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models.Th125;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th125;

// %T125SHOT[x][y][z]
internal sealed class ShotReplacer(
    IReadOnlyDictionary<(Chara Chara, Level Level, int Scene), (string Path, IBestShotHeader Header)> bestshots,
    INumberFormatter formatter,
    string outputFilePath)
    : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}SHOT({Parsers.CharaParser.Pattern})({Parsers.LevelParser.Pattern})([1-9])");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var chara = Parsers.CharaParser.Parse(match.Groups[1].Value);
        var level = Parsers.LevelParser.Parse(match.Groups[2]);
        var scene = IntegerHelper.Parse(match.Groups[3].Value);

        if (!Definitions.SpellCards.ContainsKey((level, scene)))
            return match.ToString();

        if (bestshots.TryGetValue((chara, level, scene), out var bestshot) &&
            UriHelper.TryGetRelativePath(outputFilePath, bestshot.Path, out var relativePath))
        {
            var resultScore = formatter.FormatNumber(bestshot.Header.ResultScore);
            var slowRate = formatter.FormatPercent(bestshot.Header.SlowRate, 6);
            var cardName = EncodingHelper.Default.GetString(bestshot.Header.CardName.ToArray()).TrimEnd('\0');
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
