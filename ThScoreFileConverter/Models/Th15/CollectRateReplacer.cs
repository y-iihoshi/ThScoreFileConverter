﻿//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th15;
using ThScoreFileConverter.Helpers;
using GameMode = ThScoreFileConverter.Core.Models.Th15.GameMode;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Models.Th15;

// %T15CRG[v][w][xx][y][z]
internal sealed class CollectRateReplacer : Th13.CollectRateReplacerBase<
    GameMode,
    CharaWithTotal,
    Level,
    LevelWithTotal,
    Core.Models.Th14.LevelPractice,
    Core.Models.Th14.LevelPracticeWithTotal,
    Core.Models.Th14.StagePractice,
    IScoreData>
{
    public CollectRateReplacer(
        IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary, INumberFormatter formatter)
        : base(
              Definitions.FormatPrefix,
              Parsers.GameModeParser,
              Parsers.LevelWithTotalParser,
              Parsers.CharaWithTotalParser,
              Parsers.StageWithTotalParser,
              CanReplace,
              FindCardByModeType,
              FindCardByLevel,
              FindCardByLevelStage,
              (mode, chara) => GetSpellCards(clearDataDictionary, mode, chara),
              formatter)
    {
    }

    private static bool CanReplace(
        GameMode mode, LevelWithTotal level, CharaWithTotal chara, StageWithTotal stage)
    {
        return stage != StageWithTotal.Extra;
    }

    private static Func<ISpellCard, bool> FindCardByModeType(GameMode mode, int type)
    {
        return type switch
        {
            1 => card => card.ClearCount > 0,
            _ => card => card.TrialCount > 0,
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

    private static IEnumerable<ISpellCard> GetSpellCards(
        IReadOnlyDictionary<CharaWithTotal, IClearData> clearDataDictionary,
        GameMode mode,
        CharaWithTotal chara)
    {
        return clearDataDictionary.TryGetValue(chara, out var clearData)
            && clearData.GameModeData.TryGetValue(mode, out var clearDataPerGameMode)
            ? clearDataPerGameMode.Cards.Values : ImmutableList<ISpellCard>.Empty;
    }
}
