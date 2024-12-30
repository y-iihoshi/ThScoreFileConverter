using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th15;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th15.Definitions;
using GameMode = ThScoreFileConverter.Core.Models.Th15.GameMode;

namespace TemplateGenerator.Models.Th15;

public class Definitions : Models.Definitions
{
    public static string Title { get; } = StringResources.TH15;

    public static IReadOnlyDictionary<string, string> CharacterNames { get; } = new[]
    {
        (Chara.Reimu,  "博麗 霊夢"),
        (Chara.Marisa, "霧雨 魔理沙"),
        (Chara.Sanae,  "東風谷 早苗"),
        (Chara.Reisen, "鈴仙・優曇華院・イナバ"),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, string> CharacterWithTotalNames { get; } = new[]
    {
        (CharaWithTotal.Reimu,  "博麗 霊夢"),
        (CharaWithTotal.Marisa, "霧雨 魔理沙"),
        (CharaWithTotal.Sanae,  "東風谷 早苗"),
        (CharaWithTotal.Reisen, "鈴仙・優曇華院・イナバ"),
        (CharaWithTotal.Total,  "全主人公合計"),
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

    public static IReadOnlyDictionary<string, string> GameModes { get; } = new[]
    {
        (GameMode.Pointdevice, "完全無欠モード"),
        (GameMode.Legacy,      "レガシーモード"),
    }.ToPatternKeyedDictionary();
}
