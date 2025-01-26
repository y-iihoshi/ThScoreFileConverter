//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th06;

// %T06CRG[x][y]
internal sealed class CollectRateReplacer : IStringReplaceable
{
    private static readonly IntegerParser TypeParser = new(@"[12]");
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}CRG({Parsers.StageWithTotalParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator;

    public CollectRateReplacer(IReadOnlyDictionary<int, ICardAttack> cardAttacks, INumberFormatter formatter)
    {
        this.evaluator = new MatchEvaluator(match =>
        {
            var stage = Parsers.StageWithTotalParser.Parse(match.Groups[1]);
            var type = TypeParser.Parse(match.Groups[2]);

#pragma warning disable IDE0072 // Add missing cases to switch expression
            Func<ICardAttack, bool> findByStage = stage switch
            {
                StageWithTotal.Total => FuncHelper.True,
                _ => attack => Definitions.CardTable.Any(
                    pair => (pair.Key == attack.CardId) && (pair.Value.Stage == (Stage)stage)),
            };
#pragma warning restore IDE0072 // Add missing cases to switch expression

            Func<ICardAttack, bool> findByType = type switch
            {
                1 => attack => attack.ClearCount > 0,
                _ => attack => attack.TrialCount > 0,
            };

            return formatter.FormatNumber(
                cardAttacks.Values.Count(FuncHelper.MakeAndPredicate(findByStage, findByType)));
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
