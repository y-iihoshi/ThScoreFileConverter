using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th18;
using static ThScoreFileConverter.Core.Models.Th18.Definitions;

namespace TemplateGenerator.Models.Th18;

public class Definitions : Models.Definitions
{
    public static string Title { get; } = "東方虹龍洞";

    public static IReadOnlyDictionary<string, string> CharacterNames { get; } = new[]
    {
        (Chara.Reimu,  "博麗 霊夢"),
        (Chara.Marisa, "霧雨 魔理沙"),
        (Chara.Sakuya, "十六夜 咲夜"),
        (Chara.Sanae,  "東風谷 早苗"),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, string> CharacterWithTotalNames { get; } = new[]
    {
        (CharaWithTotal.Reimu,  "博麗 霊夢"),
        (CharaWithTotal.Marisa, "霧雨 魔理沙"),
        (CharaWithTotal.Sakuya, "十六夜 咲夜"),
        (CharaWithTotal.Sanae,  "東風谷 早苗"),
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

    public static int NumAbilityCards { get; } = AbilityCardTable.Count;

    public static int NumAchievements { get; } = Achievements.Count;
}
