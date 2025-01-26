using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th155;
using ThScoreFileConverter.Core.Resources;

namespace TemplateGenerator.Models.Th155;

public static class Definitions
{
    public static string Title { get; } = StringResources.TH155;

    public static IReadOnlyDictionary<string, string> LevelNames { get; } =
        EnumHelper<Level>.Enumerable.ToDictionary(
            static level => level.ToPattern(),
            static level => level.ToDisplayName());

    public static IReadOnlyDictionary<string, string> StoryCharacterNames { get; } =
        EnumHelper<StoryChara>.Enumerable.ToDictionary(
            static chara => chara.ToPattern(),
            static chara => $"{chara.ToCharaName(0)} &amp; {chara.ToCharaName(1)}");  // FIXME

}
