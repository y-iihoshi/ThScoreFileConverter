using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th07;

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

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } = new[]
    {
        (Level.Easy,     28),
        (Level.Normal,   28),
        (Level.Hard,     30),
        (Level.Lunatic,  30),
        (Level.Extra,    12),
        (Level.Phantasm, 13),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } = new[]
    {
        (Stage.One,      10),
        (Stage.Two,      16),
        (Stage.Three,    18),
        (Stage.Four,     24),
        (Stage.Five,     20),
        (Stage.Six,      28),
        (Stage.Extra,    12),
        (Stage.Phantasm, 13),
    }.ToStringKeyedDictionary();

    public static bool CanPractice(Level level)
    {
        return (level != Level.Extra) && (level != Level.Phantasm);
    }

    public static bool CanPractice(Stage stage)
    {
        return (stage != Stage.Extra) && (stage != Stage.Phantasm);
    }
}
