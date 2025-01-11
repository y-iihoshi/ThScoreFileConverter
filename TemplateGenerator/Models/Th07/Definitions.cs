using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th07;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th07.Definitions;

namespace TemplateGenerator.Models.Th07;

public static class Definitions
{
    public static string Title { get; } = StringResources.TH07;

    public static IReadOnlyDictionary<string, string> LevelNames { get; } =
        EnumHelper<Level>.Enumerable.ToPatternDictionary();

    public static IReadOnlyDictionary<string, string> LevelPracticeNames { get; } =
        EnumHelper<Level>.Enumerable.Where(CanPractice).ToPatternDictionary();

    public static IReadOnlyDictionary<string, string> LevelWithTotalNames { get; } =
        EnumHelper<LevelWithTotal>.Enumerable.ToPatternDictionary();

    public static IEnumerable<string> LevelKeysTotalFirst { get; } = LevelWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> LevelKeysTotalLast { get; } = LevelWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, string> CharacterNames { get; } =
        EnumHelper<Chara>.Enumerable.ToDictionary(
            static chara => chara.ToPattern(),
            static chara => $"{chara.ToCharaFullName()}（{chara.ToShotTypeName()}）");  // FIXME

    public static IReadOnlyDictionary<string, string> CharacterWithTotalNames { get; } =
        EnumHelper<CharaWithTotal>.Enumerable.ToDictionary(
            static chara => chara.ToPattern(),
            static chara => (chara == CharaWithTotal.Total)
                ? "全主人公合計" : $"{chara.ToCharaFullName()}（{chara.ToShotTypeName()}）");  // FIXME

    public static IEnumerable<string> CharacterKeysTotalFirst { get; } = CharacterWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> CharacterKeysTotalLast { get; } = CharacterWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, string> StageNames { get; } =
        EnumHelper<Stage>.Enumerable.ToDictionary(
            static stage => stage.ToPattern(),
            static stage => stage.ToDisplayName());

    public static IReadOnlyDictionary<string, string> StagePracticeNames { get; } =
        EnumHelper<Stage>.Enumerable.Where(CanPractice).ToDictionary(
            static stage => stage.ToPattern(),
            static stage => stage.ToDisplayName());

    public static IReadOnlyDictionary<string, string> StageWithTotalNames { get; } =
        EnumHelper<StageWithTotal>.Enumerable.ToDictionary(
            static stage => stage.ToPattern(),
            static stage => stage.ToDisplayName());

    public static IEnumerable<string> StageKeysTotalFirst { get; } = StageWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> StageKeysTotalLast { get; } = StageWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } =
        EnumHelper<Level>.Enumerable.ToDictionary(
            static level => level.ToPattern(),
            static level => CardTable.Count(pair => pair.Value.Level == level));

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } =
        EnumHelper<Stage>.Enumerable.ToDictionary(
            static stage => stage.ToPattern(),
            static stage => CardTable.Count(pair => pair.Value.Stage == stage));
}
