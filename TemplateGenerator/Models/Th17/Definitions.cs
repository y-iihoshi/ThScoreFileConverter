using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th17;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th17.Definitions;

namespace TemplateGenerator.Models.Th17;

public class Definitions : Models.Definitions
{
    public static string Title { get; } = StringResources.TH17;

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

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } =
        EnumHelper<Level>.Enumerable.ToDictionary(
            static level => level.ToShortName(),
            static level => CardTable.Count(pair => pair.Value.Level == level));

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } =
        EnumHelper<Stage>.Enumerable.ToDictionary(
            static stage => stage.ToShortName(),
            static stage => CardTable.Count(pair => pair.Value.Stage == stage));

    public static int NumAchievements { get; } = Achievements.Count;
}
