using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th13;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th13.Definitions;

namespace TemplateGenerator.Models.Th13;

public class Definitions : Models.Definitions
{
    private static readonly IEnumerable<(LevelPractice, int)> NumCardsPerLevelImpl =
        EnumHelper<LevelPractice>.Enumerable.Select(
            static level => (level, CardTable.Count(pair => pair.Value.Level == level)));

    public static string Title { get; } = StringResources.TH13;

    public static IReadOnlyDictionary<string, string> LevelSpellPracticeNames { get; } =
        EnumHelper<LevelPractice>.Enumerable.ToDictionary(
            static level => level.ToPattern(),
            static level => level.ToDisplayName());

    public static IReadOnlyDictionary<string, string> CharacterNames { get; } =
        EnumHelper<Chara>.Enumerable.ToDictionary(
            static chara => chara.ToPattern(),
            static chara => chara.ToCharaFullName());

    public static IReadOnlyDictionary<string, string> CharacterWithTotalNames { get; } =
        EnumHelper<CharaWithTotal>.Enumerable.ToDictionary(
            static chara => chara.ToPattern(),
            static chara => (chara == CharaWithTotal.Total) ? "全主人公合計" : chara.ToCharaFullName());  // FIXME

    public static IEnumerable<string> CharacterKeysTotalFirst { get; } = CharacterWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> CharacterKeysTotalLast { get; } = CharacterWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, string> StageSpellPracticeNames { get; } =
        EnumHelper<StagePractice>.Enumerable.ToDictionary(
            static stage => stage.ToPattern(),
            static stage => stage.ToDisplayName());

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } =
        NumCardsPerLevelImpl.ToDictionary(
            static pair => pair.Item1.ToPattern(),
            static pair => pair.Item2);

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } =
        EnumHelper<StagePractice>.Enumerable.ToDictionary(
            static stage => stage.ToPattern(),
            static stage => CardTable.Count(pair => pair.Value.Stage == stage));

    public static int NumCardsWithOverDrive { get; } =
        NumCardsPerLevelImpl.Sum(static pair => pair.Item2);

    public static int NumCardsWithoutOverDrive { get; } =
        NumCardsPerLevelImpl.Where(static pair => pair.Item1 != LevelPractice.OverDrive).Sum(static pair => pair.Item2);
}
