﻿using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using static ThScoreFileConverter.Core.Models.Definitions;

namespace TemplateGenerator.Models;

#pragma warning disable CA1052 // Static holder types should be Static or NotInheritable
public class Definitions
#pragma warning restore CA1052 // Static holder types should be Static or NotInheritable
{
    public static IReadOnlyDictionary<string, string> LevelNames { get; } =
        EnumHelper<Level>.Enumerable.ToPatternDictionary();

    public static IReadOnlyDictionary<string, string> LevelPracticeNames { get; } =
        EnumHelper<Level>.Enumerable.Where(CanPractice).ToPatternDictionary();

    public static IReadOnlyDictionary<string, string> LevelWithTotalNames { get; } =
        EnumHelper<LevelWithTotal>.Enumerable.ToPatternDictionary();

    public static IEnumerable<string> LevelKeysTotalFirst { get; } = LevelWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> LevelKeysTotalLast { get; } = LevelWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, string> StageNames { get; } =
        EnumHelper<Stage>.Enumerable.ToDictionary(EnumExtensions.ToPattern, EnumExtensions.ToDisplayName);

    public static IReadOnlyDictionary<string, string> StagePracticeNames { get; } =
        EnumHelper<Stage>.Enumerable.Where(CanPractice).ToDictionary(EnumExtensions.ToPattern, EnumExtensions.ToDisplayName);

    public static IReadOnlyDictionary<string, string> StageWithTotalNames { get; } =
        EnumHelper<StageWithTotal>.Enumerable.ToDictionary(EnumExtensions.ToPattern, EnumExtensions.ToDisplayName);

    public static IEnumerable<string> StageKeysTotalFirst { get; } = StageWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> StageKeysTotalLast { get; } = StageWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, string> CareerKinds { get; } =
        EnumHelper<GameMode>.Enumerable.ToDictionary(EnumExtensions.ToPattern, EnumExtensions.ToDisplayName);
}
