using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th11;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th11.Definitions;

namespace TemplateGenerator.Models.Th11;

public class Definitions : Models.Definitions
{
    public static string Title { get; } = StringResources.TH11;

    public static IReadOnlyDictionary<string, string> CharacterNames { get; } = new[]
    {
        (Chara.ReimuYukari,     "霊夢 &amp; 紫"),
        (Chara.ReimuSuika,      "霊夢 &amp; 萃香"),
        (Chara.ReimuAya,        "霊夢 &amp; 文"),
        (Chara.MarisaAlice,     "魔理沙 &amp; アリス"),
        (Chara.MarisaPatchouli, "魔理沙 &amp; パチュリー"),
        (Chara.MarisaNitori,    "魔理沙 &amp; にとり"),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, string> CharacterWithTotalNames { get; } = new[]
    {
        (CharaWithTotal.ReimuYukari,     "霊夢 &amp; 紫"),
        (CharaWithTotal.ReimuSuika,      "霊夢 &amp; 萃香"),
        (CharaWithTotal.ReimuAya,        "霊夢 &amp; 文"),
        (CharaWithTotal.MarisaAlice,     "魔理沙 &amp; アリス"),
        (CharaWithTotal.MarisaPatchouli, "魔理沙 &amp; パチュリー"),
        (CharaWithTotal.MarisaNitori,    "魔理沙 &amp; にとり"),
        (CharaWithTotal.Total,           "全主人公合計"),
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
