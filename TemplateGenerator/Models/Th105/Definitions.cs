﻿using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th105;
using static ThScoreFileConverter.Core.Models.Th105.Definitions;

namespace TemplateGenerator.Models.Th105;

public class Definitions
{
    private static readonly IEnumerable<(Chara, CardType, int)> NumCardsPerCharacterAndTypeImpl =
        EnumHelper<Chara>.Enumerable.SelectMany(static chara =>
            EnumHelper<CardType>.Enumerable.Select(cardType =>
                (chara, cardType, cardType == CardType.System
                    ? SystemCardNameTable.Count
                    : CardNameTable.Keys.Count(key => key.Chara == chara && key.CardId / 100 == (int)cardType))));

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

    public static IReadOnlyDictionary<string, int> NumCardsPerCharacter { get; } =
        EnumHelper<Chara>.Enumerable.ToDictionary(
            static chara => chara.ToShortName(),
            static chara => StageInfoTable[chara].Sum(static stageInfo => stageInfo.CardIds.Count()) * EnumHelper<Level>.NumValues);

    public static IReadOnlyDictionary<string, string> CardTypeNames { get; } =
        EnumHelper<CardType>.Enumerable.ToStringDictionary();

    public static IReadOnlyDictionary<(string Chara, string CardType), int> NumCardsPerCharacterAndType { get; } =
        NumCardsPerCharacterAndTypeImpl.ToDictionary(
            static tuple => (tuple.Item1.ToShortName(), tuple.Item2.ToShortName()),
            static tuple => tuple.Item3);

    public static IReadOnlyDictionary<string, int> MaxNumCardsPerType { get; } =
        EnumHelper<CardType>.Enumerable.ToDictionary(
            static type => type.ToShortName(),
            static type => NumCardsPerCharacterAndTypeImpl.Where(tuple => tuple.Item2 == type).Max(static tuple => tuple.Item3));
}
