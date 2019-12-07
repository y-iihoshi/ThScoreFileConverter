﻿//-----------------------------------------------------------------------
// <copyright file="ScoreReplacer.cs" company="None">
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

namespace ThScoreFileConverter.Models.Th165
{
    // %T165SCR[xx][y][z]
    internal class ScoreReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T165SCR({0})([1-7])([1-4])", Parsers.DayParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ScoreReplacer(IReadOnlyList<IScore> scores)
        {
            if (scores is null)
                throw new ArgumentNullException(nameof(scores));

            this.evaluator = new MatchEvaluator(match =>
            {
                var day = Parsers.DayParser.Parse(match.Groups[1].Value);
                var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                var key = (day, scene);
                if (!Definitions.SpellCards.ContainsKey(key))
                    return match.ToString();

                var score = scores.FirstOrDefault(elem =>
                    (elem != null) &&
                    (elem.Number >= 0) &&
                    (elem.Number < Definitions.SpellCards.Count) &&
                    Definitions.SpellCards.ElementAt(elem.Number).Key.Equals(key));

                switch (type)
                {
                    case 1:     // high score
                        return (score != null) ? Utils.ToNumberString(score.HighScore) : "0";
                    case 2:     // challenge count
                        return (score != null) ? Utils.ToNumberString(score.ChallengeCount) : "0";
                    case 3:     // cleared count
                        return (score != null) ? Utils.ToNumberString(score.ClearCount) : "0";
                    case 4:     // num of photos
                        return (score != null) ? Utils.ToNumberString(score.NumPhotos) : "0";
                    default:    // unreachable
                        return match.ToString();
                }
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}