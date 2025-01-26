//-----------------------------------------------------------------------
// <copyright file="CareerReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Immutable;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th13;
using ThScoreFileConverter.Helpers;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Th13.LevelPractice,
    ThScoreFileConverter.Core.Models.Th13.LevelPractice,
    ThScoreFileConverter.Core.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th13.StagePractice,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;

namespace ThScoreFileConverter.Models.Th13;

// %T13C[w][xxx][yy][z]
internal sealed class CareerReplacer : IStringReplaceable
{
    private static readonly IntegerParser CardNumberParser = new(@"\d{3}");
    private static readonly IntegerParser TypeParser = new(@"[12]");
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}C({Parsers.GameModeParser.Pattern})({CardNumberParser.Pattern})({Parsers.CharaWithTotalParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator;

    public CareerReplacer(
        IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, INumberFormatter formatter)
    {
        this.evaluator = new MatchEvaluator(match =>
        {
            var mode = Parsers.GameModeParser.Parse(match.Groups[1]);
            var number = CardNumberParser.Parse(match.Groups[2]);
            var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[3]);
            var type = TypeParser.Parse(match.Groups[4]);

#pragma warning disable IDE0072 // Add missing cases to switch expression
            Func<ISpellCard<LevelPractice>, bool> isValidLevel = mode switch
            {
                GameMode.Story => card => card.Level != LevelPractice.OverDrive,
                _ => FuncHelper.True,
            };
#pragma warning restore IDE0072 // Add missing cases to switch expression

            Func<ISpellCard<LevelPractice>, int> getCount = (mode, type) switch
            {
                (GameMode.Story, 1) => card => card.ClearCount,
                (GameMode.Story, _) => card => card.TrialCount,
                (_, 1) => card => card.PracticeClearCount,
                _ => card => card.PracticeTrialCount,
            };

            var cards = clearDataDictionary.TryGetValue(chara, out var clearData)
                ? clearData.Cards : ImmutableDictionary<int, ISpellCard<LevelPractice>>.Empty;
            if (number == 0)
            {
                return formatter.FormatNumber(cards.Values.Where(isValidLevel).Sum(getCount));
            }
            else if (Definitions.CardTable.ContainsKey(number))
            {
                if (cards.TryGetValue(number, out var card))
                {
                    return isValidLevel(card) ? formatter.FormatNumber(getCount(card)) : match.ToString();
                }
                else
                {
                    return formatter.FormatNumber(default(int));
                }
            }
            else
            {
                return match.ToString();
            }
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
