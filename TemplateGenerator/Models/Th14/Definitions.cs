using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th14;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th14.Definitions;

namespace TemplateGenerator.Models.Th14;

public class Definitions : Models.Definitions
{
    public static string Title { get; } = StringResources.TH14;

    public static IReadOnlyDictionary<string, string> CharacterNames { get; } = new[]
    {
        (Chara.ReimuA,  "博麗 霊夢 (A)"),
        (Chara.ReimuB,  "博麗 霊夢 (B)"),
        (Chara.MarisaA, "霧雨 魔理沙 (A)"),
        (Chara.MarisaB, "霧雨 魔理沙 (B)"),
        (Chara.SakuyaA, "十六夜 咲夜 (A)"),
        (Chara.SakuyaB, "十六夜 咲夜 (B)"),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, string> CharacterWithTotalNames { get; } = new[]
    {
        (CharaWithTotal.ReimuA,  "博麗 霊夢 (A)"),
        (CharaWithTotal.ReimuB,  "博麗 霊夢 (B)"),
        (CharaWithTotal.MarisaA, "霧雨 魔理沙 (A)"),
        (CharaWithTotal.MarisaB, "霧雨 魔理沙 (B)"),
        (CharaWithTotal.SakuyaA, "十六夜 咲夜 (A)"),
        (CharaWithTotal.SakuyaB, "十六夜 咲夜 (B)"),
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
}
