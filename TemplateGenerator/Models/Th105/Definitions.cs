using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th105;
using ThScoreFileConverter.Models.Th105;

namespace TemplateGenerator.Models.Th105;

public class Definitions
{
    private static readonly IEnumerable<(Chara, CardType, int)> NumCardsPerCharacterAndTypeImpl = new[]
    {
        (Chara.Reimu,     CardType.System,  6),
        (Chara.Reimu,     CardType.Skill,  11),
        (Chara.Reimu,     CardType.Spell,   8),
        (Chara.Marisa,    CardType.System,  6),
        (Chara.Marisa,    CardType.Skill,  11),
        (Chara.Marisa,    CardType.Spell,  10),
        (Chara.Sakuya,    CardType.System,  6),
        (Chara.Sakuya,    CardType.Skill,   8),
        (Chara.Sakuya,    CardType.Spell,  10),
        (Chara.Alice,     CardType.System,  6),
        (Chara.Alice,     CardType.Skill,  10),
        (Chara.Alice,     CardType.Spell,  10),
        (Chara.Patchouli, CardType.System,  6),
        (Chara.Patchouli, CardType.Skill,  10),
        (Chara.Patchouli, CardType.Spell,   9),
        (Chara.Youmu,     CardType.System,  6),
        (Chara.Youmu,     CardType.Skill,  10),
        (Chara.Youmu,     CardType.Spell,   8),
        (Chara.Remilia,   CardType.System,  6),
        (Chara.Remilia,   CardType.Skill,   9),
        (Chara.Remilia,   CardType.Spell,   8),
        (Chara.Yuyuko,    CardType.System,  6),
        (Chara.Yuyuko,    CardType.Skill,   9),
        (Chara.Yuyuko,    CardType.Spell,   9),
        (Chara.Yukari,    CardType.System,  6),
        (Chara.Yukari,    CardType.Skill,   9),
        (Chara.Yukari,    CardType.Spell,   9),
        (Chara.Suika,     CardType.System,  6),
        (Chara.Suika,     CardType.Skill,   9),
        (Chara.Suika,     CardType.Spell,   9),
        (Chara.Reisen,    CardType.System,  6),
        (Chara.Reisen,    CardType.Skill,  11),
        (Chara.Reisen,    CardType.Spell,   8),
        (Chara.Aya,       CardType.System,  6),
        (Chara.Aya,       CardType.Skill,  10),
        (Chara.Aya,       CardType.Spell,   8),
        (Chara.Komachi,   CardType.System,  6),
        (Chara.Komachi,   CardType.Skill,  10),
        (Chara.Komachi,   CardType.Spell,   8),
        (Chara.Iku,       CardType.System,  6),
        (Chara.Iku,       CardType.Skill,   9),
        (Chara.Iku,       CardType.Spell,   8),
        (Chara.Tenshi,    CardType.System,  6),
        (Chara.Tenshi,    CardType.Skill,   8),
        (Chara.Tenshi,    CardType.Spell,   8),
    };

    public static string Title { get; } = "東方緋想天";

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
        (Chara.Reisen,    "鈴仙",       "鈴仙・優曇華院・イナバ"),
        (Chara.Aya,       "文",         "射命丸 文"),
        (Chara.Komachi,   "小町",       "小野塚 小町"),
        (Chara.Iku,       "衣玖",       "永江 衣玖"),
        (Chara.Tenshi,    "天子",       "比那名居 天子"),
    }.ToDictionary(
        static tuple => tuple.Item1.ToShortName(),
        static tuple => (tuple.Item1.ToString(), tuple.Item2, tuple.Item3));

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } = new[]
    {
        (Level.Easy,    23),
        (Level.Normal,  24),
        (Level.Hard,    25),
        (Level.Lunatic, 25),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, int> NumCardsPerCharacter { get; } = new[]
    {
        (Chara.Reimu,      76),
        (Chara.Marisa,     76),
        (Chara.Sakuya,     76),
        (Chara.Alice,      76),
        (Chara.Patchouli,  76),
        (Chara.Youmu,      76),
        (Chara.Remilia,    96),
        (Chara.Yuyuko,     76),
        (Chara.Yukari,     76),
        (Chara.Suika,      52),
        (Chara.Reisen,     60),
        (Chara.Aya,        60),
        (Chara.Komachi,    76),
        (Chara.Iku,        76),
        (Chara.Tenshi,    100),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, string> CardTypeNames { get; } =
        EnumHelper<CardType>.Enumerable.ToStringDictionary();

    public static IReadOnlyDictionary<(string Chara, string CardType), int> NumCardsPerCharacterAndType { get; } =
        NumCardsPerCharacterAndTypeImpl.ToDictionary(
            static tuple => (tuple.Item1.ToShortName(), tuple.Item2.ToShortName()),
            static tuple => tuple.Item3);

    public static IReadOnlyDictionary<string, int> MaxNumCardsPerType { get; } =
        EnumHelper<CardType>.Enumerable.ToDictionary(
            static type => type.ToShortName(),
            static type => NumCardsPerCharacterAndTypeImpl
                .Where(tuple => tuple.Item2 == type).Max(static tuple => tuple.Item3));
}
