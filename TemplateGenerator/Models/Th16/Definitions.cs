using System.Collections.Generic;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th16;

namespace TemplateGenerator.Models.Th16;

public class Definitions : Models.Definitions
{
    public static string Title { get; } = "東方天空璋";

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

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } = new[]
    {
        (Level.Easy,    26),
        (Level.Normal,  26),
        (Level.Hard,    27),
        (Level.Lunatic, 27),
        (Level.Extra,   13),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } = new[]
    {
        (Stage.One,    8),
        (Stage.Two,   12),
        (Stage.Three, 14),
        (Stage.Four,  12),
        (Stage.Five,  24),
        (Stage.Six,   36),
        (Stage.Extra, 13),
    }.ToStringKeyedDictionary();
}
