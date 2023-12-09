//-----------------------------------------------------------------------
// <copyright file="ShotExReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models.Th143;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th143;

// %T143SHOTEX[w][x][y]
internal sealed class ShotExReplacer : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}SHOTEX({Parsers.DayParser.Pattern})([0-9])([1-4])");

    private readonly MatchEvaluator evaluator;

    public ShotExReplacer(
        IReadOnlyDictionary<(Day Day, int Scene), (string Path, IBestShotHeader Header)> bestshots,
        string outputFilePath)
    {
        this.evaluator = new MatchEvaluator(match =>
        {
            var day = Parsers.DayParser.Parse(match.Groups[1].Value);
            var scene = IntegerHelper.Parse(match.Groups[2].Value);
            scene = (scene == 0) ? 10 : scene;
            var type = IntegerHelper.Parse(match.Groups[3].Value);

            var key = (day, scene);
            if (!Definitions.SpellCards.ContainsKey(key))
                return match.ToString();

            if (bestshots.TryGetValue(key, out var bestshot))
            {
                return type switch
                {
                    1 => UriHelper.GetRelativePath(outputFilePath, bestshot.Path),
                    2 => bestshot.Header.Width.ToString(CultureInfo.InvariantCulture),
                    3 => bestshot.Header.Height.ToString(CultureInfo.InvariantCulture),
                    4 => DateTimeHelper.GetString(bestshot.Header.DateTime),
                    _ => match.ToString(),
                };
            }
            else
            {
                return type switch
                {
                    1 => string.Empty,
                    2 => "0",
                    3 => "0",
                    4 => DateTimeHelper.GetString(null),
                    _ => match.ToString(),
                };
            }
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
