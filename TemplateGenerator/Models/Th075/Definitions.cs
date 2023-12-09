using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th075;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th075.Definitions;

namespace TemplateGenerator.Models.Th075;

public static class Definitions
{
    public static string Title { get; } = StringResources.TH075;

    public static IReadOnlyDictionary<string, string> LevelNames { get; } =
        EnumHelper<Level>.Enumerable.ToStringDictionary();

    public static IReadOnlyDictionary<string, string> LevelWithTotalNames { get; } =
        EnumHelper<LevelWithTotal>.Enumerable.ToStringDictionary();

    public static IEnumerable<string> LevelKeysTotalFirst { get; } = LevelWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> LevelKeysTotalLast { get; } = LevelWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, (string Id, string ShortName, string LongName)> CharacterNames { get; } = new[]
    {
        (Chara.Reimu,     "霊夢",       "博麗 霊夢"),
        (Chara.Marisa,    "魔理沙",     "霧雨 魔理沙"),
        (Chara.Sakuya,    "咲夜",       "十六夜 咲夜"),
        (Chara.Alice,     "アリス",     "アリス・マーガトロイド"),
        (Chara.Patchouli, "パチュリー", "パチュリー・ノーレッジ"),
        (Chara.Youmu,     "妖夢",       "魂魄 妖夢"),
        (Chara.Remilia,   "レミリア",   "レミリア・スカーレット"),
        (Chara.Yuyuko,    "幽々子",     "西行寺 幽々子"),
        (Chara.Yukari,    "紫",         "八雲 紫"),
        (Chara.Suika,     "萃香",       "伊吹 萃香"),
    }.ToDictionary(
        static tuple => tuple.Item1.ToShortName(),
        static tuple => (tuple.Item1.ToString(), tuple.Item2, tuple.Item3));

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } =
        EnumHelper<Level>.Enumerable.ToDictionary(
            static level => level.ToShortName(),
            static level => CardTable.Count(pair => CardIdTable[Chara.Reimu].Any(id => id == pair.Key) && (pair.Value.Level == level)));
}
