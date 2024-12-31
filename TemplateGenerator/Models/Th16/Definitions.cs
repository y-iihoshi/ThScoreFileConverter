using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th16;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th16.Definitions;

namespace TemplateGenerator.Models.Th16;

public class Definitions : Models.Definitions
{
    public static string Title { get; } = StringResources.TH16;

    public static IReadOnlyDictionary<string, string> CharacterNames { get; } = new[]
    {
        (Chara.Reimu,  "博麗 霊夢"),
        (Chara.Cirno,  "日焼けしたチルノ"),
        (Chara.Aya,    "射命丸 文"),
        (Chara.Marisa, "霧雨 魔理沙"),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, string> CharacterWithTotalNames { get; } = new[]
    {
        (CharaWithTotal.Reimu,  "博麗 霊夢"),
        (CharaWithTotal.Cirno,  "日焼けしたチルノ"),
        (CharaWithTotal.Aya,    "射命丸 文"),
        (CharaWithTotal.Marisa, "霧雨 魔理沙"),
        (CharaWithTotal.Total,  "全主人公合計"),
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
