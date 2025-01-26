using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th135;
using ThScoreFileConverter.Core.Resources;

namespace TemplateGenerator.Models.Th135;

public static class Definitions
{
    public static string Title { get; } = StringResources.TH135;

    public static IReadOnlyDictionary<string, string> LevelNames { get; } =
        EnumHelper<Level>.Enumerable.ToDictionary(
            static level => level.ToPattern(),
            static level => level.ToName());

    public static IReadOnlyDictionary<string, string> CharacterNames { get; } =
        EnumHelper<Chara>.Enumerable.ToDictionary(
            static chara => chara.ToPattern(),
            static chara => (chara == Chara.IchirinUnzan)
                ? $"{chara.ToCharaFullName(0)} &amp; {chara.ToCharaFullName(1)}"  // FIXME
                : chara.ToCharaFullName());
}
