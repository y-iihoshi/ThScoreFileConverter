using System.Collections.Generic;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th11;

namespace TemplateGenerator.Models.Th11;

public class Definitions : Models.Definitions
{
    public static string Title { get; } = "東方地霊殿";

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

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } = new[]
    {
        (Level.Easy,    40),
        (Level.Normal,  40),
        (Level.Hard,    41),
        (Level.Lunatic, 41),
        (Level.Extra,   13),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } = new[]
    {
        (Stage.One,   10),
        (Stage.Two,   16),
        (Stage.Three, 16),
        (Stage.Four,  76),
        (Stage.Five,  20),
        (Stage.Six,   24),
        (Stage.Extra, 13),
    }.ToStringKeyedDictionary();
}
