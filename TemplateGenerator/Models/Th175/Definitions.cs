using System;
using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th175;
using ThScoreFileConverter.Core.Resources;

namespace TemplateGenerator.Models.Th175;

public static class Definitions
{
    private static string ToPairCharaNames<T>(T chara)
        where T : struct, Enum
    {
        return $"{chara.ToCharaFullName(0)} &amp; {chara.ToCharaName(1)}";  // FIXME
    }

    public static string Title { get; } = StringResources.TH175;

    public static IReadOnlyDictionary<string, string> LevelNames { get; } =
        EnumHelper<Level>.Enumerable.Where(level => level is Level.Normal or Level.Hard).ToPatternDictionary();

    public static IReadOnlyDictionary<string, string> CharacterNames { get; } =
        EnumHelper<Chara>.Enumerable.ToDictionary(
            static chara => chara.ToPattern(),
            static chara => (chara == Chara.JoonShion) ? ToPairCharaNames(chara) : chara.ToCharaFullName());

    public static IReadOnlyDictionary<string, string> CharacterWithTotalNames { get; } =
        EnumHelper<CharaWithTotal>.Enumerable.ToDictionary(
            static chara => chara.ToPattern(),
            static chara => chara switch
            {
                CharaWithTotal.Total => "全主人公合計",  // FIXME
                CharaWithTotal.JoonShion => ToPairCharaNames(chara),
                _ => chara.ToCharaFullName(),
            });

    public static IEnumerable<string> CharacterKeysTotalFirst { get; } = CharacterWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> CharacterKeysTotalLast { get; } = CharacterWithTotalNames.Keys;
}
