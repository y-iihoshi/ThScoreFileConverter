using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;

namespace TemplateGenerator.Models
{
    public class Definitions
    {
        public static IReadOnlyDictionary<string, string> LevelNames { get; } = EnumHelper<Level>.Enumerable
            .ToDictionary(level => level.ToShortName(), level => level.ToString());

        public static IReadOnlyDictionary<string, string> LevelPracticeNames { get; } = EnumHelper<Level>.Enumerable
            .Where(level => level != Level.Extra)
            .ToDictionary(level => level.ToShortName(), level => level.ToString());

        public static IReadOnlyDictionary<string, string> StageNames { get; } = new[]
        {
            (Stage.One,   "Stage 1"),
            (Stage.Two,   "Stage 2"),
            (Stage.Three, "Stage 3"),
            (Stage.Four,  "Stage 4"),
            (Stage.Five,  "Stage 5"),
            (Stage.Six,   "Stage 6"),
            (Stage.Extra, "Extra"),
        }.ToDictionary(pair => pair.Item1.ToShortName(), pair => pair.Item2);

        public static IReadOnlyDictionary<string, string> StagePracticeNames { get; } =
            StageNames.Where(pair => pair.Key != Stage.Extra.ToShortName()).ToDictionary();

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
        }.ToDictionary(pair => pair.Item1.ToShortName(), pair => pair.Item2);

        public static IEnumerable<string> StageKeysTotalFirst { get; } =
            StageWithTotalNames.Keys.TakeLast(1).Concat(StageWithTotalNames.Keys.SkipLast(1));

        public static IEnumerable<string> StageKeysTotalLast { get; } =
            StageWithTotalNames.Keys;
    }
}
