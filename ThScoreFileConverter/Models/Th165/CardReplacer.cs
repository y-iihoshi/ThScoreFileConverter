//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th165;

// %T165CARD[xx][y][z]
internal sealed class CardReplacer(IReadOnlyList<IScore> scores, bool hideUntriedCards) : IStringReplaceable
{
    private static readonly IntegerParser TypeParser = new(@"[12]");
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}CARD({Parsers.DayParser.Pattern})({Parsers.SceneParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var day = Parsers.DayParser.Parse(match.Groups[1]);
        var scene = Parsers.SceneParser.Parse(match.Groups[2]);
        var type = TypeParser.Parse(match.Groups[3]);

        var key = (day, scene);
        if (!Definitions.SpellCards.TryGetValue(key, out var enemyCardPair))
            return match.ToString();

        if (hideUntriedCards)
        {
            var score = scores.FirstOrDefault(elem =>
                (elem is not null) &&
                (elem.Number >= 0) &&
                (elem.Number < Definitions.SpellCards.Count) &&
                Definitions.SpellCards.ElementAt(elem.Number).Key.Equals(key));
            if ((score is null) || (score.ChallengeCount <= 0))
                return "??????????";
        }

        if (type == 1)
            return string.Join(" &amp; ", enemyCardPair.Enemies.Select(enemy => enemy.ToCharaFullName()));
        else
            return enemyCardPair.Card;
    });

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
