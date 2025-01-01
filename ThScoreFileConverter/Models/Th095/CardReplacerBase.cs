//-----------------------------------------------------------------------
// <copyright file="CardReplacerBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th095;

internal class CardReplacerBase<TLevel, TEnemy> : IStringReplaceable
    where TLevel : struct, Enum
    where TEnemy : struct, Enum
{
    private static readonly IntegerParser TypeParser = new(@"[12]");

    private readonly string pattern;
    private readonly MatchEvaluator evaluator;

    protected CardReplacerBase(
        string formatPrefix,
        IRegexParser<TLevel> levelParser,
        IRegexParser<int> sceneParser,
        IReadOnlyDictionary<(TLevel Level, int Scene), (TEnemy Enemy, string Card)> spellCards,
        bool hideUntriedCards,
        Func<TLevel, int, bool> levelSceneHasTried)
    {
        this.pattern = StringHelper.Create($"{formatPrefix}CARD({levelParser.Pattern})({sceneParser.Pattern})({TypeParser.Pattern})");
        this.evaluator = new MatchEvaluator(match =>
        {
            var level = levelParser.Parse(match.Groups[1]);
            var scene = sceneParser.Parse(match.Groups[2]);
            var type = TypeParser.Parse(match.Groups[3]);

            if (!spellCards.TryGetValue((level, scene), out var enemyCardPair))
                return match.ToString();

            if (hideUntriedCards && !levelSceneHasTried(level, scene))
                return "??????????";

            return (type == 1) ? enemyCardPair.Enemy.ToLongName() : enemyCardPair.Card;
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, this.pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
