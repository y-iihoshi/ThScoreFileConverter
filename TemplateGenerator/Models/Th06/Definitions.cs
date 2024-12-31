using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th06;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th06.Definitions;

namespace TemplateGenerator.Models.Th06;

public class Definitions : Models.Definitions
{
    public static string Title { get; } = StringResources.TH06;

    public static IReadOnlyDictionary<string, string> CharacterNames { get; } = new[]
    {
        (Chara.ReimuA,  "博麗 霊夢（霊）"),
        (Chara.ReimuB,  "博麗 霊夢（夢）"),
        (Chara.MarisaA, "霧雨 魔理沙（魔）"),
        (Chara.MarisaB, "霧雨 魔理沙（恋）"),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } =
        EnumHelper<Stage>.Enumerable.ToDictionary(
            static stage => stage.ToPattern(),
            static stage => CardTable.Count(pair => pair.Value.Stage == stage));

    public static bool CanPractice(string levelKey, string stageKey)
    {
        return !(levelKey == Level.Easy.ToPattern() && stageKey == Stage.Six.ToPattern());
    }
}
