using System.Collections.Generic;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th10;

namespace TemplateGenerator.Models.Th10;

public class Definitions : Models.Definitions
{
    public static string Title { get; } = "東方風神録";

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

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } = new[]
    {
        (Level.Easy,    23),
        (Level.Normal,  24),
        (Level.Hard,    25),
        (Level.Lunatic, 25),
        (Level.Extra,   13),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } = new[]
    {
        (Stage.One,   10),
        (Stage.Two,   16),
        (Stage.Three, 16),
        (Stage.Four,  15),
        (Stage.Five,  20),
        (Stage.Six,   20),
        (Stage.Extra, 13),
    }.ToStringKeyedDictionary();
}
