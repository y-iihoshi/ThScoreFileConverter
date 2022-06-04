using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th07;
using static ThScoreFileConverter.Core.Models.Th07.Definitions;

namespace TemplateGenerator.Models.Th07;

public class Definitions
{
    private static readonly IEnumerable<(Stage, string)> StageNamesImpl = new[]
    {
        (Stage.One,      "Stage 1"),
        (Stage.Two,      "Stage 2"),
        (Stage.Three,    "Stage 3"),
        (Stage.Four,     "Stage 4"),
        (Stage.Five,     "Stage 5"),
        (Stage.Six,      "Stage 6"),
        (Stage.Extra,    "Extra"),
        (Stage.Phantasm, "Phantasm"),
    };

    public static string Title { get; } = "東方妖々夢";

    public static IReadOnlyDictionary<string, string> LevelNames { get; } =
        EnumHelper<Level>.Enumerable.ToStringDictionary();

    public static IReadOnlyDictionary<string, string> LevelPracticeNames { get; } =
        EnumHelper<Level>.Enumerable.Where(CanPractice).ToStringDictionary();

    public static IReadOnlyDictionary<string, string> LevelWithTotalNames { get; } =
        EnumHelper<LevelWithTotal>.Enumerable.ToStringDictionary();

    public static IEnumerable<string> LevelKeysTotalFirst { get; } = LevelWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> LevelKeysTotalLast { get; } = LevelWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, string> CharacterNames { get; } = new[]
    {
        (Chara.ReimuA,  "博麗 霊夢（霊）"),
        (Chara.ReimuB,  "博麗 霊夢（夢）"),
        (Chara.MarisaA, "霧雨 魔理沙（魔）"),
        (Chara.MarisaB, "霧雨 魔理沙（恋）"),
        (Chara.SakuyaA, "十六夜 咲夜（幻）"),
        (Chara.SakuyaB, "十六夜 咲夜（時）"),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, string> CharacterWithTotalNames { get; } = new[]
    {
        (CharaWithTotal.ReimuA,  "博麗 霊夢（霊）"),
        (CharaWithTotal.ReimuB,  "博麗 霊夢（夢）"),
        (CharaWithTotal.MarisaA, "霧雨 魔理沙（魔）"),
        (CharaWithTotal.MarisaB, "霧雨 魔理沙（恋）"),
        (CharaWithTotal.SakuyaA, "十六夜 咲夜（幻）"),
        (CharaWithTotal.SakuyaB, "十六夜 咲夜（時）"),
        (CharaWithTotal.Total,   "全主人公合計"),
    }.ToStringKeyedDictionary();

    public static IEnumerable<string> CharacterKeysTotalFirst { get; } = CharacterWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> CharacterKeysTotalLast { get; } = CharacterWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, string> StageNames { get; } =
        StageNamesImpl.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, string> StagePracticeNames { get; } =
        StageNamesImpl.Where(static pair => CanPractice(pair.Item1)).ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, string> StageWithTotalNames { get; } = new[]
    {
        (StageWithTotal.One,      "Stage 1"),
        (StageWithTotal.Two,      "Stage 2"),
        (StageWithTotal.Three,    "Stage 3"),
        (StageWithTotal.Four,     "Stage 4"),
        (StageWithTotal.Five,     "Stage 5"),
        (StageWithTotal.Six,      "Stage 6"),
        (StageWithTotal.Extra,    "Extra"),
        (StageWithTotal.Phantasm, "Phantasm"),
        (StageWithTotal.Total,    "Total"),
    }.ToStringKeyedDictionary();

    public static IEnumerable<string> StageKeysTotalFirst { get; } = StageWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> StageKeysTotalLast { get; } = StageWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } =
        EnumHelper<Level>.Enumerable.ToDictionary(
            static level => level.ToShortName(),
            static level => CardTable.Count(pair => pair.Value.Level == level));

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } =
        EnumHelper<Stage>.Enumerable.ToDictionary(
            static stage => stage.ToShortName(),
            static stage => CardTable.Count(pair => pair.Value.Stage == stage));

    public static bool CanPractice(Level level)
    {
        return (level != Level.Extra) && (level != Level.Phantasm);
    }

    public static bool CanPractice(Stage stage)
    {
        return (stage != Stage.Extra) && (stage != Stage.Phantasm);
    }
}
