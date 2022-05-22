using System.Collections.Generic;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Models.Th17;

namespace TemplateGenerator.Models.Th17;

public class Definitions : Models.Definitions
{
    public static string Title { get; } = "東方鬼形獣";

    public static IReadOnlyDictionary<string, string> CharacterNames { get; } = new[]
    {
        (Chara.ReimuA,  "博麗 霊夢 (狼)"),
        (Chara.ReimuB,  "博麗 霊夢 (獺)"),
        (Chara.ReimuC,  "博麗 霊夢 (鷲)"),
        (Chara.MarisaA, "霧雨 魔理沙 (狼)"),
        (Chara.MarisaB, "霧雨 魔理沙 (獺)"),
        (Chara.MarisaC, "霧雨 魔理沙 (鷲)"),
        (Chara.YoumuA,  "魂魄 妖夢 (狼)"),
        (Chara.YoumuB,  "魂魄 妖夢 (獺)"),
        (Chara.YoumuC,  "魂魄 妖夢 (鷲)"),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, string> CharacterWithTotalNames { get; } = new[]
    {
        (CharaWithTotal.ReimuA,  "博麗 霊夢 (狼)"),
        (CharaWithTotal.ReimuB,  "博麗 霊夢 (獺)"),
        (CharaWithTotal.ReimuC,  "博麗 霊夢 (鷲)"),
        (CharaWithTotal.MarisaA, "霧雨 魔理沙 (狼)"),
        (CharaWithTotal.MarisaB, "霧雨 魔理沙 (獺)"),
        (CharaWithTotal.MarisaC, "霧雨 魔理沙 (鷲)"),
        (CharaWithTotal.YoumuA,  "魂魄 妖夢 (狼)"),
        (CharaWithTotal.YoumuB,  "魂魄 妖夢 (獺)"),
        (CharaWithTotal.YoumuC,  "魂魄 妖夢 (鷲)"),
        (CharaWithTotal.Total,   "全主人公合計"),
    }.ToStringKeyedDictionary();

    public static IEnumerable<string> CharacterKeysTotalFirst { get; } = CharacterWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> CharacterKeysTotalLast { get; } = CharacterWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } = new[]
    {
        (Level.Easy,    22),
        (Level.Normal,  22),
        (Level.Hard,    22),
        (Level.Lunatic, 22),
        (Level.Extra,   13),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } = new[]
    {
        (Stage.One,    8),
        (Stage.Two,   12),
        (Stage.Three, 12),
        (Stage.Four,  12),
        (Stage.Five,  16),
        (Stage.Six,   28),
        (Stage.Extra, 13),
    }.ToStringKeyedDictionary();

    public static int NumAchievements { get; } = 40;
}
