﻿//-----------------------------------------------------------------------
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
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th095
{
    // %T95SHOT[x][y]
    internal class ShotReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"{0}SHOT({1})([1-9])", Definitions.FormatPrefix, Parsers.LevelParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ShotReplacer(
            IReadOnlyDictionary<(Level Level, int Scene), (string Path, IBestShotHeader<Level> Header)> bestshots,
            INumberFormatter formatter,
            string outputFilePath)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                var scene = IntegerHelper.Parse(match.Groups[2].Value);

                var key = (level, scene);
                if (!Definitions.SpellCards.ContainsKey(key))
                    return match.ToString();

                if (bestshots.TryGetValue(key, out var bestshot) &&
                    Uri.TryCreate(outputFilePath, UriKind.Absolute, out var outputFileUri) &&
                    Uri.TryCreate(bestshot.Path, UriKind.Absolute, out var bestshotUri))
                {
                    var relativePath = outputFileUri.MakeRelativeUri(bestshotUri).OriginalString;
                    var alternativeString = Utils.Format(
                        "ClearData: {0}{3}Slow: {1}{3}SpellName: {2}",
                        formatter.FormatNumber(bestshot.Header.ResultScore),
                        formatter.FormatPercent(bestshot.Header.SlowRate, 6),
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
