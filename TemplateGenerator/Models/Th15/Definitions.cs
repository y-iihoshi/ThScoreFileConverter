using System.Collections.Generic;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th15;
using GameMode = ThScoreFileConverter.Models.Th15.GameMode;

namespace TemplateGenerator.Models.Th15;

public class Definitions : Models.Definitions
{
    public static string Title { get; } = "東方紺珠伝";

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
        (Stage.One,   10),
        (Stage.Two,   12),
        (Stage.Three, 20),
        (Stage.Four,  16),
        (Stage.Five,  20),
        (Stage.Six,   28),
        (Stage.Extra, 13),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, string> GameModes { get; } = new[]
    {
        (GameMode.Pointdevice, "完全無欠モード"),
        (GameMode.Legacy,      "レガシーモード"),
    }.ToStringKeyedDictionary();
}
