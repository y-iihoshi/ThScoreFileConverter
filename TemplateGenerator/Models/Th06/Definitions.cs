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

    public static IReadOnlyDictionary<string, string> CharacterNames { get; } =
        EnumHelper<Chara>.Enumerable.ToDictionary(
            static chara => chara.ToPattern(),
            static chara => $"{chara.ToCharaFullName()}（{chara.ToShotTypeName()}）");  // FIXME

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } =
        EnumHelper<Stage>.Enumerable.ToDictionary(
            static stage => stage.ToPattern(),
            static stage => CardTable.Count(pair => pair.Value.Stage == stage));

    public static bool CanPractice(string levelKey, string stageKey)
    {
        return !(levelKey == Level.Easy.ToPattern() && stageKey == Stage.Six.ToPattern());
    }
}
