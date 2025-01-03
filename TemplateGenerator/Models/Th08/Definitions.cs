﻿using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th08.Definitions;

namespace TemplateGenerator.Models.Th08;

public class Definitions : Models.Definitions
{
    private static readonly IEnumerable<(LevelPractice, int)> NumCardsPerLevelImpl =
        EnumHelper<LevelPractice>.Enumerable.Select(
            static level => (level, CardTable.Count(pair => pair.Value.Level == level)));

    public static string Title { get; } = StringResources.TH08;

    public static IReadOnlyDictionary<string, string> LevelSpellPracticeNames { get; } =
        EnumHelper<LevelPractice>.Enumerable.ToDictionary(
            static level => level.ToPattern(),
            static level => level.ToDisplayName());

    public static IReadOnlyDictionary<string, (string ShortName, string LongName)> CharacterNames { get; } = new[]
    {
        (Chara.ReimuYukari,   ("霊夢 &amp; 紫",       "霊夢 &amp; 紫")),
        (Chara.MarisaAlice,   ("魔理沙 &amp; アリス", "魔理沙 &amp; アリス")),
        (Chara.SakuyaRemilia, ("咲夜 &amp; レミリア", "咲夜 &amp; レミリア")),
        (Chara.YoumuYuyuko,   ("妖夢 &amp; 幽々子",   "妖夢 &amp; 幽々子")),
        (Chara.Reimu,         ("霊夢",                "博麗 霊夢")),
        (Chara.Yukari,        ("紫",                  "八雲 紫")),
        (Chara.Marisa,        ("魔理沙",              "霧雨 魔理沙")),
        (Chara.Alice,         ("アリス",              "アリス・マーガトロイド")),
        (Chara.Sakuya,        ("咲夜",                "十六夜 咲夜")),
        (Chara.Remilia,       ("レミリア",            "レミリア・スカーレット")),
        (Chara.Youmu,         ("妖夢",                "魂魄 妖夢")),
        (Chara.Yuyuko,        ("幽々子",              "西行寺 幽々子")),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, (string ShortName, string LongName)> CharacterWithTotalNames { get; } = new[]
    {
        (CharaWithTotal.ReimuYukari,   ("霊夢 &amp; 紫",       "霊夢 &amp; 紫")),
        (CharaWithTotal.MarisaAlice,   ("魔理沙 &amp; アリス", "魔理沙 &amp; アリス")),
        (CharaWithTotal.SakuyaRemilia, ("咲夜 &amp; レミリア", "咲夜 &amp; レミリア")),
        (CharaWithTotal.YoumuYuyuko,   ("妖夢 &amp; 幽々子",   "妖夢 &amp; 幽々子")),
        (CharaWithTotal.Reimu,         ("霊夢",                "博麗 霊夢")),
        (CharaWithTotal.Yukari,        ("紫",                  "八雲 紫")),
        (CharaWithTotal.Marisa,        ("魔理沙",              "霧雨 魔理沙")),
        (CharaWithTotal.Alice,         ("アリス",              "アリス・マーガトロイド")),
        (CharaWithTotal.Sakuya,        ("咲夜",                "十六夜 咲夜")),
        (CharaWithTotal.Remilia,       ("レミリア",            "レミリア・スカーレット")),
        (CharaWithTotal.Youmu,         ("妖夢",                "魂魄 妖夢")),
        (CharaWithTotal.Yuyuko,        ("幽々子",              "西行寺 幽々子")),
        (CharaWithTotal.Total,         ("全主人公合計",        "全主人公合計")),
    }.ToStringKeyedDictionary();

    public static IEnumerable<string> CharacterKeysTotalFirst { get; } = CharacterWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> CharacterKeysTotalLast { get; } = CharacterWithTotalNames.Keys;

    public static new IReadOnlyDictionary<string, string> StageNames { get; } =
        EnumHelper<Stage>.Enumerable.ToDictionary(
            static stage => stage.ToPattern(),
            static stage => stage.ToDisplayName());

    public static new IReadOnlyDictionary<string, string> StagePracticeNames { get; } =
        EnumHelper<Stage>.Enumerable.Where(CanPractice).ToDictionary(
            static stage => stage.ToPattern(),
            static stage => stage.ToDisplayName());

    public static IReadOnlyDictionary<string, string> StageSpellPracticeNames { get; } =
        EnumHelper<StagePractice>.Enumerable.ToDictionary(
            static stage => stage.ToPattern(),
            static stage => stage.ToDisplayName());

    public static new IReadOnlyDictionary<string, string> StageWithTotalNames { get; } =
        EnumHelper<StageWithTotal>.Enumerable.ToDictionary(
            static stage => stage.ToPattern(),
            static stage => stage.ToDisplayName());

    public static new IEnumerable<string> StageKeysTotalFirst { get; } = StageWithTotalNames.Keys.RotateRight();

    public static new IEnumerable<string> StageKeysTotalLast { get; } = StageWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } =
        NumCardsPerLevelImpl.ToPatternKeyedDictionary();

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } =
        EnumHelper<StagePractice>.Enumerable.ToDictionary(
            static stage => stage.ToPattern(),
            static stage => CardTable.Count(pair => pair.Value.Stage == stage));

    public static IReadOnlyDictionary<(string, string), int> NumCardsPerStage4Level { get; } =
        new[] { StagePractice.FourUncanny, StagePractice.FourPowerful }
            .Cartesian(EnumHelper<LevelPractice>.Enumerable)
            .ToDictionary(
                static pair => (pair.First.ToPattern(), pair.Second.ToPattern()),
                static pair => CardTable.Values.Count(card => (card.Stage, card.Level) == pair));

    public static int NumCardsWithLastWord { get; } =
        NumCardsPerLevelImpl.Sum(static pair => pair.Item2);

    public static int NumCardsWithoutLastWord { get; } =
        NumCardsPerLevelImpl.Where(static pair => pair.Item1 != LevelPractice.LastWord).Sum(static pair => pair.Item2);

    public static IReadOnlyDictionary<string, string> UnreachableStagesPerCharacter { get; } = new[]
    {
        (Chara.ReimuYukari,   Stage.FourUncanny),
        (Chara.MarisaAlice,   Stage.FourPowerful),
        (Chara.SakuyaRemilia, Stage.FourPowerful),
        (Chara.YoumuYuyuko,   Stage.FourUncanny),
        (Chara.Reimu,         Stage.FourUncanny),
        (Chara.Yukari,        Stage.FourUncanny),
        (Chara.Marisa,        Stage.FourPowerful),
        (Chara.Alice,         Stage.FourPowerful),
        (Chara.Sakuya,        Stage.FourPowerful),
        (Chara.Remilia,       Stage.FourPowerful),
        (Chara.Youmu,         Stage.FourUncanny),
        (Chara.Yuyuko,        Stage.FourUncanny),
    }.ToDictionary(static pair => pair.Item1.ToShortName(), static pair => pair.Item2.ToPattern());
}
