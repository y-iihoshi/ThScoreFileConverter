using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th12;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th12.Definitions;

namespace TemplateGenerator.Models.Th12;

public class Definitions : Models.Definitions
{
    public static string Title { get; } = StringResources.TH12;

    public static IReadOnlyDictionary<string, string> CharacterNames { get; } = new[]
    {
        (Chara.ReimuA,  "博麗 霊夢（夢）"),
        (Chara.ReimuB,  "博麗 霊夢（霊）"),
        (Chara.MarisaA, "霧雨 魔理沙（恋）"),
        (Chara.MarisaB, "霧雨 魔理沙（魔）"),
        (Chara.SanaeA,  "東風谷 早苗（蛇）"),
        (Chara.SanaeB,  "東風谷 早苗（蛙）"),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, string> CharacterWithTotalNames { get; } = new[]
    {
        (CharaWithTotal.ReimuA,  "博麗 霊夢（夢）"),
        (CharaWithTotal.ReimuB,  "博麗 霊夢（霊）"),
        (CharaWithTotal.MarisaA, "霧雨 魔理沙（恋）"),
        (CharaWithTotal.MarisaB, "霧雨 魔理沙（魔）"),
        (CharaWithTotal.SanaeA,  "東風谷 早苗（蛇）"),
        (CharaWithTotal.SanaeB,  "東風谷 早苗（蛙）"),
        (CharaWithTotal.Total,   "全主人公合計"),
    }.ToStringKeyedDictionary();

    public static IEnumerable<string> CharacterKeysTotalFirst { get; } = CharacterWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> CharacterKeysTotalLast { get; } = CharacterWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } =
        EnumHelper<Level>.Enumerable.ToDictionary(
            static level => level.ToPattern(),
            static level => CardTable.Count(pair => pair.Value.Level == level));

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } =
        EnumHelper<Stage>.Enumerable.ToDictionary(
            static stage => stage.ToPattern(),
            static stage => CardTable.Count(pair => pair.Value.Stage == stage));
}
