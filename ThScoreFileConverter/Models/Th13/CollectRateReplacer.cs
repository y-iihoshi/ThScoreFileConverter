//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th13;
using ThScoreFileConverter.Helpers;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Core.Models.Th13.LevelPractice>;

namespace ThScoreFileConverter.Models.Th13;

// %T13CRG[v][w][xx][y][z]
internal class CollectRateReplacer : CollectRateReplacerBase<
    GameMode,
    CharaWithTotal,
    LevelPractice,
    LevelPracticeWithTotal,
    LevelPractice,
    LevelPracticeWithTotal,
    StagePractice,
    IScoreData>
{
    public CollectRateReplacer(
        IReadOnlyDictionary<CharaWithTotal, IClearData<
            CharaWithTotal,
            LevelPractice,
            LevelPractice,
            LevelPracticeWithTotal,
            StagePractice,
            IScoreData>> clearDataDictionary,
        INumberFormatter formatter)
        : base(
              Definitions.FormatPrefix,
              Parsers.GameModeParser,
              Parsers.LevelPracticeWithTotalParser,
              Parsers.CharaWithTotalParser,
              Parsers.StageWithTotalParser,
              CanReplace,
              FindCardByModeType,
              FindCardByLevel,
              FindCardByLevelStage,
              clearDataDictionary,
              formatter)
    {
    }

    private static bool CanReplace(
        GameMode mode, LevelPracticeWithTotal level, CharaWithTotal chara, StageWithTotal stage)
    {
        return (stage != StageWithTotal.Extra)
            && !((mode == GameMode.Story) && (level == LevelPracticeWithTotal.OverDrive));
    }

    private static Func<ISpellCard, bool> FindCardByModeType(GameMode mode, int type)
    {
        return (mode, type) switch
        {
            (GameMode.Story, 1) => card => (card.Level != LevelPractice.OverDrive) && (card.ClearCount > 0),
            (GameMode.Story, _) => card => (card.Level != LevelPractice.OverDrive) && (card.TrialCount > 0),
            (_, 1) => card => card.PracticeClearCount > 0,
            _ => card => card.PracticeTrialCount > 0,
        };
    }

    private static Func<ISpellCard, bool> FindCardByLevel(LevelPracticeWithTotal level)
    {
        return level switch
        {
            LevelPracticeWithTotal.Total => FuncHelper.True,
            LevelPracticeWithTotal.Extra => FuncHelper.True,
            LevelPracticeWithTotal.OverDrive => FuncHelper.True,
            _ => card => card.Level == (LevelPractice)level,
        };
    }

    private static Func<ISpellCard, bool> FindCardByLevelStage(LevelPracticeWithTotal level, StageWithTotal stage)
    {
        return (level, stage) switch
        {
            (LevelPracticeWithTotal.Extra, _) =>
                card => Definitions.CardTable[card.Id].Stage == StagePractice.Extra,
            (LevelPracticeWithTotal.OverDrive, _) =>
                card => Definitions.CardTable[card.Id].Stage == StagePractice.OverDrive,
            (_, StageWithTotal.Total) => FuncHelper.True,
            _ => card => Definitions.CardTable[card.Id].Stage == (StagePractice)stage,
        };
    }
}
