//-----------------------------------------------------------------------
// <copyright file="ShotReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th125
{
    // %T125SHOT[x][y][z]
    internal class ShotReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T125SHOT({0})({1})([1-9])", Parsers.CharaParser.Pattern, Parsers.LevelParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ShotReplacer(
            IReadOnlyDictionary<(Chara, Level, int), (string Path, IBestShotHeader Header)> bestshots,
            string outputFilePath)
        {
            if (bestshots is null)
                throw new ArgumentNullException(nameof(bestshots));

            this.evaluator = new MatchEvaluator(match =>
            {
                var chara = Parsers.CharaParser.Parse(match.Groups[1].Value);
                var level = Parsers.LevelParser.Parse(match.Groups[2].Value);
                var scene = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                if (!Definitions.SpellCards.ContainsKey((level, scene)))
                    return match.ToString();

                if (bestshots.TryGetValue((chara, level, scene), out var bestshot) &&
                    Uri.TryCreate(outputFilePath, UriKind.Absolute, out var outputFileUri) &&
                    Uri.TryCreate(bestshot.Path, UriKind.Absolute, out var bestshotUri))
                {
                    var relativePath = outputFileUri.MakeRelativeUri(bestshotUri).OriginalString;
                    var alternativeString = Utils.Format(
                        "ClearData: {0}{3}Slow: {1:F6}%{3}SpellName: {2}",
                        Utils.ToNumberString(bestshot.Header.ResultScore),
                        bestshot.Header.SlowRate,
                        Encoding.Default.GetString(bestshot.Header.CardName.ToArray()).TrimEnd('\0'),
                        Environment.NewLine);
                    return Utils.Format(
                        "<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" border=0>", relativePath, alternativeString);
                }
                else
                {
                    return string.Empty;
                }
            });
        }

        public string Replace(string input)
        {
            return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }
    }
}
