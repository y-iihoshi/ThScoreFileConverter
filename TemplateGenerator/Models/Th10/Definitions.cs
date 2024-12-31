using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th10;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th10.Definitions;

namespace TemplateGenerator.Models.Th10;

public class Definitions : Models.Definitions
{
    public static string Title { get; } = StringResources.TH10;

    public static IReadOnlyDictionary<string, (string Name, string Equip)> CharacterNames { get; } = new[]
    {
        (Chara.ReimuA,  ("博麗 霊夢",   "（誘導装備）")),
        (Chara.ReimuB,  ("博麗 霊夢",   "（前方集中装備）")),
        (Chara.ReimuC,  ("博麗 霊夢",   "（封印装備）")),
        (Chara.MarisaA, ("霧雨 魔理沙", "（高威力装備）")),
        (Chara.MarisaB, ("霧雨 魔理沙", "（貫通装備）")),
        (Chara.MarisaC, ("霧雨 魔理沙", "（魔法使い装備）")),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, (string Name, string Equip)> CharacterWithTotalNames { get; } = new[]
    {
        (CharaWithTotal.ReimuA,  ("博麗 霊夢",    "（誘導装備）")),
        (CharaWithTotal.ReimuB,  ("博麗 霊夢",    "（前方集中装備）")),
        (CharaWithTotal.ReimuC,  ("博麗 霊夢",    "（封印装備）")),
        (CharaWithTotal.MarisaA, ("霧雨 魔理沙",  "（高威力装備）")),
        (CharaWithTotal.MarisaB, ("霧雨 魔理沙",  "（貫通装備）")),
        (CharaWithTotal.MarisaC, ("霧雨 魔理沙",  "（魔法使い装備）")),
        (CharaWithTotal.Total,   ("全主人公合計", string.Empty)),
    }.ToStringKeyedDictionary();

    public static IEnumerable<string> CharacterKeysTotalFirst { get; } = CharacterWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> CharacterKeysTotalLast { get; } = CharacterWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } =
        EnumHelper<Level>.Enumerable.ToDictionary(
            static level => level.ToPattern(),
            static level => CardTable.Count(pair => pair.Value.Level == level));

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } =
        EnumHelper<Stage>.Enumerable.ToDictionary(
            static stage => stage.ToShortName(),
            static stage => CardTable.Count(pair => pair.Value.Stage == stage));
}
