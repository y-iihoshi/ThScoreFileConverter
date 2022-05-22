using System.Collections.Generic;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Models.Th06;

namespace TemplateGenerator.Models.Th06;

public class Definitions : Models.Definitions
{
    public static string Title { get; } = "東方紅魔郷";

    public static IReadOnlyDictionary<string, string> CharacterNames { get; } = new[]
    {
        (Chara.ReimuA,  "博麗 霊夢（霊）"),
        (Chara.ReimuB,  "博麗 霊夢（夢）"),
        (Chara.MarisaA, "霧雨 魔理沙（魔）"),
        (Chara.MarisaB, "霧雨 魔理沙（恋）"),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } = new[]
    {
        (Stage.One,    3),
        (Stage.Two,    4),
        (Stage.Three,  7),
        (Stage.Four,  18),
        (Stage.Five,   8),
        (Stage.Six,   11),
        (Stage.Extra, 13),
    }.ToStringKeyedDictionary();

    public static bool CanPractice(string levelKey, string stageKey)
    {
        return !(levelKey == Level.Easy.ToShortName() && stageKey == Stage.Six.ToShortName());
    }
}
