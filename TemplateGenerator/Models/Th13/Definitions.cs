using System.Collections.Generic;
using System.Linq;
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
            static level => (level.ToLongName().Length > 0) ? level.ToLongName() : level.ToString());

    public static IReadOnlyDictionary<string, string> CharacterNames { get; } = new[]
    {
        (Chara.Reimu,  "博麗 霊夢"),
        (Chara.Marisa, "霧雨 魔理沙"),
        (Chara.Sanae,  "東風谷 早苗"),
        (Chara.Youmu,  "魂魄 妖夢"),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, string> CharacterWithTotalNames { get; } = new[]
    {
        (CharaWithTotal.Reimu,  "博麗 霊夢"),
        (CharaWithTotal.Marisa, "霧雨 魔理沙"),
        (CharaWithTotal.Sanae,  "東風谷 早苗"),
        (CharaWithTotal.Youmu,  "魂魄 妖夢"),
        (CharaWithTotal.Total,  "全主人公合計"),
    }.ToStringKeyedDictionary();

    public static IEnumerable<string> CharacterKeysTotalFirst { get; } = CharacterWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> CharacterKeysTotalLast { get; } = CharacterWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, string> StageSpellPracticeNames { get; } = new[]
    {
        (StagePractice.One,       "Stage 1"),
        (StagePractice.Two,       "Stage 2"),
        (StagePractice.Three,     "Stage 3"),
        (StagePractice.Four,      "Stage 4"),
        (StagePractice.Five,      "Stage 5"),
        (StagePractice.Six,       "Stage 6"),
        (StagePractice.Extra,     "Extra"),
        (StagePractice.OverDrive, "Over Drive"),
    }.ToPatternKeyedDictionary();

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } =
        NumCardsPerLevelImpl.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } =
        EnumHelper<StagePractice>.Enumerable.ToDictionary(
            static stage => stage.ToShortName(),
            static stage => CardTable.Count(pair => pair.Value.Stage == stage));

    public static int NumCardsWithOverDrive { get; } =
        NumCardsPerLevelImpl.Sum(static pair => pair.Item2);

    public static int NumCardsWithoutOverDrive { get; } =
        NumCardsPerLevelImpl.Where(static pair => pair.Item1 != LevelPractice.OverDrive).Sum(static pair => pair.Item2);
}
