using System;
using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th145;
using ThScoreFileConverter.Core.Resources;

namespace TemplateGenerator.Models.Th145;

public static class Definitions
{
    private static string ToPairCharaFullNames<T>(T chara)
        where T : struct, Enum
    {
        return $"{chara.ToCharaFullName(0)} &amp; {chara.ToCharaFullName(1)}";  // FIXME
    }

    public static string Title { get; } = StringResources.TH145;

    public static IReadOnlyDictionary<string, string> LevelNames { get; } =
        EnumHelper<Level>.Enumerable.ToPatternDictionary();

    public static IReadOnlyDictionary<string, string> LevelWithTotalNames { get; } =
        EnumHelper<LevelWithTotal>.Enumerable.ToPatternDictionary();

    public static IEnumerable<string> LevelKeysTotalFirst { get; } = LevelWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> LevelKeysTotalLast { get; } = LevelWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, string> CharacterNames { get; } =
        EnumHelper<Chara>.Enumerable.ToDictionary(
            static chara => chara.ToPattern(),
            static chara => (chara == Chara.IchirinUnzan) ? ToPairCharaFullNames(chara) : chara.ToCharaFullName());

    public static IReadOnlyDictionary<string, string> CharacterWithTotalNames { get; } =
        EnumHelper<CharaWithTotal>.Enumerable.ToDictionary(
            static chara => chara.ToPattern(),
            static chara => chara switch
            {
                CharaWithTotal.Total => "全キャラ合計",  // FIXME
                CharaWithTotal.IchirinUnzan => ToPairCharaFullNames(chara),
                _ => chara.ToCharaFullName(),
            });

    public static IEnumerable<string> CharacterKeysTotalFirst { get; } = CharacterWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> CharacterKeysTotalLast { get; } = CharacterWithTotalNames.Keys;
}
