﻿//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th14;
using ThScoreFileConverter.Helpers;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Models.Th14;

// %T14CRG[v][w][xx][y][z]
internal sealed class CollectRateReplacer(
    IReadOnlyDictionary<CharaWithTotal, Th13.IClearData<
        CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice, IScoreData>> clearDataDictionary,
    INumberFormatter formatter)
    : Th13.CollectRateReplacerBase<
        GameMode, CharaWithTotal, Level, LevelWithTotal, LevelPractice, LevelPracticeWithTotal, StagePractice, IScoreData>(
        Definitions.FormatPrefix,
        Parsers.GameModeParser,
        Parsers.LevelWithTotalParser,
        Parsers.CharaWithTotalParser,
        Parsers.StageWithTotalParser,
        CanReplace,
        FindCardByModeType,
        FindCardByLevel,
        FindCardByLevelStage,
        clearDataDictionary,
        formatter)
{
    private static bool CanReplace(
        GameMode mode, LevelWithTotal level, CharaWithTotal chara, StageWithTotal stage)
    {
        return stage != StageWithTotal.Extra;
    }

    private static Func<ISpellCard, bool> FindCardByModeType(GameMode mode, int type)
    {
        return (mode, type) switch
        {
            (GameMode.Story, 1) => card => card.ClearCount > 0,
            (GameMode.Story, _) => card => card.TrialCount > 0,
            (_, 1) => card => card.PracticeClearCount > 0,
            _ => card => card.PracticeTrialCount > 0,
        };
    }

    private static Func<ISpellCard, bool> FindCardByLevel(LevelWithTotal level)
    {
#pragma warning disable IDE0072 // Add missing cases to switch expression
        return level switch
        {
            LevelWithTotal.Total => FuncHelper.True,
            LevelWithTotal.Extra => FuncHelper.True,
            _ => card => card.Level == (Level)level,
        };
#pragma warning restore IDE0072 // Add missing cases to switch expression
    }

    private static Func<ISpellCard, bool> FindCardByLevelStage(LevelWithTotal level, StageWithTotal stage)
    {
        return (level, stage) switch
        {
            (LevelWithTotal.Extra, _) => card => Definitions.CardTable[card.Id].Stage == Stage.Extra,
            (_, StageWithTotal.Total) => FuncHelper.True,
            _ => card => Definitions.CardTable[card.Id].Stage == (Stage)stage,
        };
    }
}
