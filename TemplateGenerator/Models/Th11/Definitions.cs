using System;
using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th11;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th11.Definitions;

namespace TemplateGenerator.Models.Th11;

public class Definitions : Models.Definitions
{
    private static string ToPairCharaNames<T>(T chara)
        where T : struct, Enum
    {
        return $"{chara.ToCharaName(0)} &amp; {chara.ToCharaName(1)}";  // FIXME
    }

    public static string Title { get; } = StringResources.TH11;

    public static IReadOnlyDictionary<string, string> CharacterNames { get; } =
        EnumHelper<Chara>.Enumerable.ToDictionary(
            static chara => chara.ToPattern(),
            static chara => ToPairCharaNames(chara));

    public static IReadOnlyDictionary<string, string> CharacterWithTotalNames { get; } =
        EnumHelper<CharaWithTotal>.Enumerable.ToDictionary(
            static chara => chara.ToPattern(),
            static chara => (chara == CharaWithTotal.Total) ? "全主人公合計" : ToPairCharaNames(chara));  // FIXME

    public static IEnumerable<string> CharacterKeysTotalFirst { get; } = CharacterWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> CharacterKeysTotalLast { get; } = CharacterWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } =
        EnumHelper<Level>.Enumerable.ToDictionary(
            static level => level.ToPattern(),
            static level => CardTable.Count(pair => pair.Value.Level == level));

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } =
        EnumHelper<Stage>.Enumerable.ToDictionary(
            static stage => stage.ToPattern(),
            static stage => CardTable.Count(pair => pair.Value.Stage == stage));
}
