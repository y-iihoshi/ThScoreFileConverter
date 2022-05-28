using System.Collections.Generic;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th18;

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

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } = new[]
    {
        (Level.Easy,    21),
        (Level.Normal,  21),
        (Level.Hard,    21),
        (Level.Lunatic, 21),
        (Level.Extra,   13),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } = new[]
    {
        (Stage.One,    8),
        (Stage.Two,   12),
        (Stage.Three, 12),
        (Stage.Four,  12),
        (Stage.Five,  16),
        (Stage.Six,   24),
        (Stage.Extra, 13),
    }.ToStringKeyedDictionary();

    public static int NumAbilityCards { get; } = 56;

    public static int NumAchievements { get; } = 30;
}
