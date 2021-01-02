//-----------------------------------------------------------------------
// <copyright file="ShotReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;

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
                var scene = IntegerHelper.Parse(match.Groups[2].Value);

                var key = (day, scene);
                if (!Definitions.SpellCards.TryGetValue(key, out var enemyCardPair))
                    return match.ToString();

                if (bestshots.TryGetValue(key, out var bestshot) &&
                    Uri.TryCreate(outputFilePath, UriKind.Absolute, out var outputFileUri) &&
                    Uri.TryCreate(bestshot.Path, UriKind.Absolute, out var bestshotUri))
                {
                    var relativePath = outputFileUri.MakeRelativeUri(bestshotUri).OriginalString;
                    var alternativeString = Utils.Format("SpellName: {0}", enemyCardPair.Card);
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
