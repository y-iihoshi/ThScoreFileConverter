using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th123;
using CardType = ThScoreFileConverter.Models.Th105.CardType;

namespace TemplateGenerator.Models.Th123;

public class Definitions : Th105.Definitions
{
    private static readonly IEnumerable<(Chara, string, string)> CharacterNamesImpl = new[]
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
        (Chara.Sanae,     "早苗",       "東風谷 早苗"),
        (Chara.Cirno,     "チルノ",     "チルノ"),
        (Chara.Meiling,   "美鈴",       "紅 美鈴"),
        (Chara.Utsuho,    "空",         "霊烏路 空"),
        (Chara.Suwako,    "諏訪子",     "洩矢 諏訪子"),
    };

    private static readonly IEnumerable<(Chara, CardType, int)> NumCardsPerCharacterAndTypeImpl = new[]
    {
        (Chara.Reimu,     CardType.System, 21),
        (Chara.Reimu,     CardType.Skill,  12),
        (Chara.Reimu,     CardType.Spell,  10),
        (Chara.Marisa,    CardType.System, 21),
        (Chara.Marisa,    CardType.Skill,  12),
        (Chara.Marisa,    CardType.Spell,  14),
        (Chara.Sakuya,    CardType.System, 21),
        (Chara.Sakuya,    CardType.Skill,  12),
        (Chara.Sakuya,    CardType.Spell,  13),
        (Chara.Alice,     CardType.System, 21),
        (Chara.Alice,     CardType.Skill,  12),
        (Chara.Alice,     CardType.Spell,  12),
        (Chara.Patchouli, CardType.System, 21),
        (Chara.Patchouli, CardType.Skill,  15),
        (Chara.Patchouli, CardType.Spell,  12),
        (Chara.Youmu,     CardType.System, 21),
        (Chara.Youmu,     CardType.Skill,  12),
        (Chara.Youmu,     CardType.Spell,  10),
        (Chara.Remilia,   CardType.System, 21),
        (Chara.Remilia,   CardType.Skill,  12),
        (Chara.Remilia,   CardType.Spell,  10),
        (Chara.Yuyuko,    CardType.System, 21),
        (Chara.Yuyuko,    CardType.Skill,  12),
        (Chara.Yuyuko,    CardType.Spell,  11),
        (Chara.Yukari,    CardType.System, 21),
        (Chara.Yukari,    CardType.Skill,  12),
        (Chara.Yukari,    CardType.Spell,  10),
        (Chara.Suika,     CardType.System, 21),
        (Chara.Suika,     CardType.Skill,  12),
        (Chara.Suika,     CardType.Spell,  10),
        (Chara.Reisen,    CardType.System, 21),
        (Chara.Reisen,    CardType.Skill,  12),
        (Chara.Reisen,    CardType.Spell,  11),
        (Chara.Aya,       CardType.System, 21),
        (Chara.Aya,       CardType.Skill,  12),
        (Chara.Aya,       CardType.Spell,  10),
        (Chara.Komachi,   CardType.System, 21),
        (Chara.Komachi,   CardType.Skill,  12),
        (Chara.Komachi,   CardType.Spell,   9),
        (Chara.Iku,       CardType.System, 21),
        (Chara.Iku,       CardType.Skill,  12),
        (Chara.Iku,       CardType.Spell,  10),
        (Chara.Tenshi,    CardType.System, 21),
        (Chara.Tenshi,    CardType.Skill,  12),
        (Chara.Tenshi,    CardType.Spell,  10),
        (Chara.Sanae,     CardType.System, 21),
        (Chara.Sanae,     CardType.Skill,  12),
        (Chara.Sanae,     CardType.Spell,   9),
        (Chara.Cirno,     CardType.System, 21),
        (Chara.Cirno,     CardType.Skill,  12),
        (Chara.Cirno,     CardType.Spell,  11),
        (Chara.Meiling,   CardType.System, 21),
        (Chara.Meiling,   CardType.Skill,  12),
        (Chara.Meiling,   CardType.Spell,  11),
        (Chara.Utsuho,    CardType.System, 21),
        (Chara.Utsuho,    CardType.Skill,  12),
        (Chara.Utsuho,    CardType.Spell,  14),
        (Chara.Suwako,    CardType.System, 21),
        (Chara.Suwako,    CardType.Skill,  12),
        (Chara.Suwako,    CardType.Spell,  11),
    };

    public static new string Title { get; } = "東方非想天則";

    public static new IReadOnlyDictionary<string, (string Id, string ShortName, string LongName)> CharacterNames { get; } =
        CharacterNamesImpl.ToDictionary(
            static tuple => tuple.Item1.ToShortName(),
            static tuple => (tuple.Item1.ToString(), tuple.Item2, tuple.Item3));

    public static IReadOnlyDictionary<string, (string Id, string ShortName, string LongName)> StoryCharacterNames { get; } =
        CharacterNamesImpl.Where(static tuple => HasStory(tuple.Item1)).ToDictionary(
            static tuple => tuple.Item1.ToShortName(),
            static tuple => (tuple.Item1.ToString(), tuple.Item2, tuple.Item3));

    public static new IReadOnlyDictionary<string, int> NumCardsPerCharacter { get; } = new[]
    {
        (Chara.Sanae,   64),
        (Chara.Cirno,   64),
        (Chara.Meiling, 64),
    }.ToStringKeyedDictionary();

    public static new IReadOnlyDictionary<(string Chara, string CardType), int> NumCardsPerCharacterAndType { get; } =
        NumCardsPerCharacterAndTypeImpl.ToDictionary(
            static tuple => (tuple.Item1.ToShortName(), tuple.Item2.ToShortName()),
            static tuple => tuple.Item3);

    public static new IReadOnlyDictionary<string, int> MaxNumCardsPerType { get; } =
        EnumHelper<CardType>.Enumerable.ToDictionary(
            static type => type.ToShortName(),
            static type => NumCardsPerCharacterAndTypeImpl
                .Where(tuple => tuple.Item2 == type).Max(static tuple => tuple.Item3));

    private static bool HasStory(Chara chara)
    {
        return chara is Chara.Sanae or Chara.Cirno or Chara.Meiling;
    }
}
