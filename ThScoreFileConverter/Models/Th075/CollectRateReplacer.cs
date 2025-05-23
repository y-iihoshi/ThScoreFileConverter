﻿//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models.Th075;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th075;

// %T75CRG[x][yy][z]
internal sealed class CollectRateReplacer : IStringReplaceable
{
    private static readonly Core.Models.IntegerParser TypeParser = new(@"[1-3]");
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}CRG({Parsers.LevelWithTotalParser.Pattern})({Parsers.CharaParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator;

    public CollectRateReplacer(
        IReadOnlyDictionary<(CharaWithReserved Chara, Level Level), IClearData> clearData,
        INumberFormatter formatter)
    {
        this.evaluator = new MatchEvaluator(match => EvaluatorImpl(match, clearData, formatter));

        static string EvaluatorImpl(
            Match match,
            IReadOnlyDictionary<(CharaWithReserved Chara, Level Level), IClearData> clearData,
            INumberFormatter formatter)
        {
            var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1]);
            var chara = Parsers.CharaParser.Parse(match.Groups[2]);
            var type = TypeParser.Parse(match.Groups[3]);

            if (chara == Chara.Meiling)
                return match.ToString();

            Func<IClearData, IEnumerable<short>> getValues = type switch
            {
                1 => data => data.CardGotCount,
                2 => data => data.CardTrialCount,
                _ => data => data.CardTrulyGot.Select(got => (short)got),
            };

            IEnumerable<(int CardId, int CardIndex)> MakeCardIdIndexPairs(Level lv)
            {
                return Definitions.CardIdTable[chara]
                    .Select((id, index) => (id, index))
                    .Where(pair => Definitions.CardTable[pair.id].Level == lv);
            }

            static bool IsPositive(short value)
            {
                return value > 0;
            }

            if (level == LevelWithTotal.Total)
            {
                return formatter.FormatNumber(clearData
                    .Where(dataPair => dataPair.Key.Chara == (CharaWithReserved)chara)
                    .Sum(dataPair => getValues(dataPair.Value)
                        .Where((value, index) =>
                            MakeCardIdIndexPairs(dataPair.Key.Level).Any(pair => pair.CardIndex == index))
                        .Count(IsPositive)));
            }
            else
            {
                return formatter.FormatNumber(clearData
                    .Where(dataPair => dataPair.Key == ((CharaWithReserved)chara, (Level)level))
                    .Sum(dataPair => getValues(dataPair.Value)
                        .Where((value, index) =>
                            MakeCardIdIndexPairs((Level)level).Any(pair => pair.CardIndex == index))
                        .Count(IsPositive)));
            }
        }
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
