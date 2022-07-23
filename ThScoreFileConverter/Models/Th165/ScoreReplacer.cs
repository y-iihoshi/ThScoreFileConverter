//-----------------------------------------------------------------------
// <copyright file="ScoreReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th165;

// %T165SCR[xx][y][z]
internal class ScoreReplacer : IStringReplaceable
{
    private static readonly string Pattern = Utils.Format(
        @"{0}SCR({1})([1-7])([1-4])", Definitions.FormatPrefix, Parsers.DayParser.Pattern);

    private readonly MatchEvaluator evaluator;

    public ScoreReplacer(IReadOnlyList<IScore> scores, INumberFormatter formatter)
    {
        this.evaluator = new MatchEvaluator(match =>
        {
            var day = Parsers.DayParser.Parse(match.Groups[1].Value);
            var scene = IntegerHelper.Parse(match.Groups[2].Value);
            var type = IntegerHelper.Parse(match.Groups[3].Value);

            var key = (day, scene);
            if (!Definitions.SpellCards.ContainsKey(key))
                return match.ToString();

            var score = scores.FirstOrDefault(elem =>
                (elem?.Number >= 0) &&
                (elem.Number < Definitions.SpellCards.Count) &&
                Definitions.SpellCards.ElementAt(elem.Number).Key.Equals(key));

            return type switch
            {
                1 => formatter.FormatNumber(score?.HighScore ?? default),
                2 => formatter.FormatNumber(score?.ChallengeCount ?? default),
                3 => formatter.FormatNumber(score?.ClearCount ?? default),
                4 => formatter.FormatNumber(score?.NumPhotos ?? default),
                _ => match.ToString(),  // unreachable
            };
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
