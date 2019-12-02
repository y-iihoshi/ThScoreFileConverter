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
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th165
{
    // %T165SHOT[xx][y]
    internal class ShotReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(@"%T165SHOT({0})([1-7])", Parsers.DayParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ShotReplacer(
            IReadOnlyDictionary<(Day, int), (string Path, IBestShotHeader Header)> bestshots, string outputFilePath)
        {
            this.evaluator = new MatchEvaluator(match =>
            {
                var day = Parsers.DayParser.Parse(match.Groups[1].Value);
                var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                var key = (day, scene);
                if (!Definitions.SpellCards.ContainsKey(key))
                    return match.ToString();

                if (!string.IsNullOrEmpty(outputFilePath) && bestshots.TryGetValue(key, out var bestshot))
                {
                    var relativePath = new Uri(outputFilePath)
                        .MakeRelativeUri(new Uri(bestshot.Path)).OriginalString;
                    var alternativeString = Utils.Format("SpellName: {0}", Definitions.SpellCards[key].Card);
                    return Utils.Format(
                        "<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" border=0>",
                        relativePath,
                        alternativeString);
                }
                else
                {
                    return string.Empty;
                }
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
