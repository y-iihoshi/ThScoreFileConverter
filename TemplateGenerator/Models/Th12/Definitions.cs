using System.Collections.Generic;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Models.Th12;

namespace TemplateGenerator.Models.Th12;

public class Definitions : Models.Definitions
{
    public static string Title { get; } = "東方星蓮船";

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

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } = new[]
    {
        (Level.Easy,    23),
        (Level.Normal,  25),
        (Level.Hard,    26),
        (Level.Lunatic, 26),
        (Level.Extra,   13),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } = new[]
    {
        (Stage.One,   10),
        (Stage.Two,   16),
        (Stage.Three, 16),
        (Stage.Four,  15),
        (Stage.Five,  20),
        (Stage.Six,   23),
        (Stage.Extra, 13),
    }.ToStringKeyedDictionary();
}
