using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Models;

namespace TemplateGenerator.Models;

public class Definitions
{
    private static readonly IEnumerable<(Stage, string)> StageNamesImpl = new[]
    {
        (Stage.One,   "Stage 1"),
        (Stage.Two,   "Stage 2"),
        (Stage.Three, "Stage 3"),
        (Stage.Four,  "Stage 4"),
        (Stage.Five,  "Stage 5"),
        (Stage.Six,   "Stage 6"),
        (Stage.Extra, "Extra"),
    };

    public static IReadOnlyDictionary<string, string> LevelNames { get; } =
        EnumHelper<Level>.Enumerable.ToStringDictionary();

    public static IReadOnlyDictionary<string, string> LevelPracticeNames { get; } =
        EnumHelper<Level>.Enumerable.Where(CanPractice).ToStringDictionary();

    public static IReadOnlyDictionary<string, string> LevelWithTotalNames { get; } =
        EnumHelper<LevelWithTotal>.Enumerable.ToStringDictionary();

    public static IEnumerable<string> LevelKeysTotalFirst { get; } = LevelWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> LevelKeysTotalLast { get; } = LevelWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, string> StageNames { get; } =
        StageNamesImpl.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, string> StagePracticeNames { get; } =
        StageNamesImpl.Where(static pair => CanPractice(pair.Item1)).ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, string> StageWithTotalNames { get; } = new[]
    {
        (StageWithTotal.One,   "Stage 1"),
        (StageWithTotal.Two,   "Stage 2"),
        (StageWithTotal.Three, "Stage 3"),
        (StageWithTotal.Four,  "Stage 4"),
        (StageWithTotal.Five,  "Stage 5"),
        (StageWithTotal.Six,   "Stage 6"),
        (StageWithTotal.Extra, "Extra"),
        (StageWithTotal.Total, "Total"),
    }.ToStringKeyedDictionary();

    public static IEnumerable<string> StageKeysTotalFirst { get; } = StageWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> StageKeysTotalLast { get; } = StageWithTotalNames.Keys;

    public static bool CanPractice(Level level)
    {
        return level != Level.Extra;
    }

    public static bool CanPractice(Stage stage)
    {
        return stage != Stage.Extra;
    }

    public static IReadOnlyDictionary<string, string> CareerKinds { get; } = new[]
    {
        (GameMode.Story,         "ゲーム本編"),
        (GameMode.SpellPractice, "スペルプラクティス"),
    }.ToStringKeyedDictionary();
}
