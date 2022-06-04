using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th123;
using static ThScoreFileConverter.Core.Models.Th123.Definitions;
using CardType = ThScoreFileConverter.Core.Models.Th105.CardType;
using Level = ThScoreFileConverter.Core.Models.Th105.Level;

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

    private static readonly IEnumerable<(Chara, CardType, int)> NumCardsPerCharacterAndTypeImpl =
        EnumHelper<Chara>.Enumerable.SelectMany(chara =>
            EnumHelper<CardType>.Enumerable.Select(cardType =>
                (chara, cardType, cardType == CardType.System
                    ? SystemCardNameTable.Count
                    : CardNameTable.Keys.Count(key => key.Chara == chara && key.CardId / 100 == (int)cardType))));

    public static new string Title { get; } = "東方非想天則";

    public static new IReadOnlyDictionary<string, (string Id, string ShortName, string LongName)> CharacterNames { get; } =
        CharacterNamesImpl.ToDictionary(
            static tuple => tuple.Item1.ToShortName(),
            static tuple => (tuple.Item1.ToString(), tuple.Item2, tuple.Item3));

    public static IReadOnlyDictionary<string, (string Id, string ShortName, string LongName)> StoryCharacterNames { get; } =
        CharacterNamesImpl.Where(static tuple => HasStory(tuple.Item1)).ToDictionary(
            static tuple => tuple.Item1.ToShortName(),
            static tuple => (tuple.Item1.ToString(), tuple.Item2, tuple.Item3));

    public static new IReadOnlyDictionary<string, int> NumCardsPerCharacter { get; } =
        StageInfoTable.ToDictionary(
            pair => pair.Key.ToShortName(),
            pair => pair.Value.Sum(stageInfo => stageInfo.CardIds.Count()) * EnumHelper<Level>.NumValues);

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
