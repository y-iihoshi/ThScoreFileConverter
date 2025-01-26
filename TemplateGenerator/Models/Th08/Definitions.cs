using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th08.Definitions;

namespace TemplateGenerator.Models.Th08;

public class Definitions : Models.Definitions
{
    private static readonly IEnumerable<(LevelPractice, int)> NumCardsPerLevelImpl =
        EnumHelper<LevelPractice>.Enumerable.Select(
            static level => (level, CardTable.Count(pair => pair.Value.Level == level)));

    private static string ToPairCharaNames<T>(T chara)
        where T : struct, Enum
    {
        return $"{chara.ToCharaName(0)} &amp; {chara.ToCharaName(1)}";  // FIXME
    }

    public static string Title { get; } = StringResources.TH08;

    public static IReadOnlyDictionary<string, string> LevelSpellPracticeNames { get; } =
        EnumHelper<LevelPractice>.Enumerable.ToDictionary(
            static level => level.ToPattern(),
            static level => level.ToDisplayName());

    public static IReadOnlyDictionary<string, (string ShortName, string LongName)> CharacterNames { get; } =
        EnumHelper<Chara>.Enumerable.ToDictionary(
            static chara => chara.ToPattern(),
            static chara => chara switch
            {
                <= Chara.YoumuYuyuko => (ToPairCharaNames(chara), ToPairCharaNames(chara)),
                _ => (chara.ToCharaName(), chara.ToCharaFullName()),
            });

    public static IReadOnlyDictionary<string, (string ShortName, string LongName)> CharacterWithTotalNames { get; } =
        EnumHelper<CharaWithTotal>.Enumerable.ToDictionary(
            static chara => chara.ToPattern(),
            static chara => chara switch
            {
                CharaWithTotal.Total => ("全主人公合計", "全主人公合計"),  // FIXME
                <= CharaWithTotal.YoumuYuyuko => (ToPairCharaNames(chara), ToPairCharaNames(chara)),
                _ => (chara.ToCharaName(), chara.ToCharaFullName()),
            });

    public static IEnumerable<string> CharacterKeysTotalFirst { get; } = CharacterWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> CharacterKeysTotalLast { get; } = CharacterWithTotalNames.Keys;

    public static new IReadOnlyDictionary<string, string> StageNames { get; } =
        EnumHelper<Stage>.Enumerable.ToDictionary(
            static stage => stage.ToPattern(),
            static stage => stage.ToDisplayName());

    public static new IReadOnlyDictionary<string, string> StagePracticeNames { get; } =
        EnumHelper<Stage>.Enumerable.Where(CanPractice).ToDictionary(
            static stage => stage.ToPattern(),
            static stage => stage.ToDisplayName());

    public static IReadOnlyDictionary<string, string> StageSpellPracticeNames { get; } =
        EnumHelper<StagePractice>.Enumerable.ToDictionary(
            static stage => stage.ToPattern(),
            static stage => stage.ToDisplayName());

    public static new IReadOnlyDictionary<string, string> StageWithTotalNames { get; } =
        EnumHelper<StageWithTotal>.Enumerable.ToDictionary(
            static stage => stage.ToPattern(),
            static stage => stage.ToDisplayName());

    public static new IEnumerable<string> StageKeysTotalFirst { get; } = StageWithTotalNames.Keys.RotateRight();

    public static new IEnumerable<string> StageKeysTotalLast { get; } = StageWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } =
        NumCardsPerLevelImpl.ToDictionary(
            static pair => pair.Item1.ToPattern(),
            static pair => pair.Item2);

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } =
        EnumHelper<StagePractice>.Enumerable.ToDictionary(
            static stage => stage.ToPattern(),
            static stage => CardTable.Count(pair => pair.Value.Stage == stage));

    public static IReadOnlyDictionary<(string, string), int> NumCardsPerStage4Level { get; } =
        new[] { StagePractice.FourUncanny, StagePractice.FourPowerful }
            .Cartesian(EnumHelper<LevelPractice>.Enumerable)
            .ToDictionary(
                static pair => (pair.First.ToPattern(), pair.Second.ToPattern()),
                static pair => CardTable.Values.Count(card => (card.Stage, card.Level) == pair));

    public static int NumCardsWithLastWord { get; } =
        NumCardsPerLevelImpl.Sum(static pair => pair.Item2);

    public static int NumCardsWithoutLastWord { get; } =
        NumCardsPerLevelImpl.Where(static pair => pair.Item1 != LevelPractice.LastWord).Sum(static pair => pair.Item2);

    public static IReadOnlyDictionary<string, string> UnreachableStagesPerCharacter { get; } = new[]
    {
        (Chara.ReimuYukari,   Stage.FourUncanny),
        (Chara.MarisaAlice,   Stage.FourPowerful),
        (Chara.SakuyaRemilia, Stage.FourPowerful),
        (Chara.YoumuYuyuko,   Stage.FourUncanny),
        (Chara.Reimu,         Stage.FourUncanny),
        (Chara.Yukari,        Stage.FourUncanny),
        (Chara.Marisa,        Stage.FourPowerful),
        (Chara.Alice,         Stage.FourPowerful),
        (Chara.Sakuya,        Stage.FourPowerful),
        (Chara.Remilia,       Stage.FourPowerful),
        (Chara.Youmu,         Stage.FourUncanny),
        (Chara.Yuyuko,        Stage.FourUncanny),
    }.ToDictionary(static pair => pair.Item1.ToPattern(), static pair => pair.Item2.ToPattern());
}
