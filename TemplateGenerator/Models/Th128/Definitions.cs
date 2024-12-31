using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th128;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th128.Definitions;
using Level = ThScoreFileConverter.Core.Models.Level;

namespace TemplateGenerator.Models.Th128;

public class Definitions : Models.Definitions
{
    public static string Title { get; } = StringResources.TH128;

    public static IReadOnlyDictionary<string, string> RouteNames { get; } = new[]
    {
        (Route.A1,    "ルート A1"),
        (Route.A2,    "ルート A2"),
        (Route.B1,    "ルート B1"),
        (Route.B2,    "ルート B2"),
        (Route.C1,    "ルート C1"),
        (Route.C2,    "ルート C2"),
        (Route.Extra, "ルート EX"),
    }.ToPatternKeyedDictionary();

    public static IReadOnlyDictionary<string, string> RouteWithTotalNames { get; } = new[]
    {
        (RouteWithTotal.A1,    "ルート A1"),
        (RouteWithTotal.A2,    "ルート A2"),
        (RouteWithTotal.B1,    "ルート B1"),
        (RouteWithTotal.B2,    "ルート B2"),
        (RouteWithTotal.C1,    "ルート C1"),
        (RouteWithTotal.C2,    "ルート C2"),
        (RouteWithTotal.Extra, "ルート EX"),
        (RouteWithTotal.Total, "全ルート合計"),
    }.ToPatternKeyedDictionary();

    public static IEnumerable<string> RouteKeysTotalFirst { get; } = RouteWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> RouteKeysTotalLast { get; } = RouteWithTotalNames.Keys;

    public static new IReadOnlyDictionary<string, string> StageNames { get; } =
        EnumHelper<Stage>.Enumerable.ToDictionary(EnumExtensions.ToPattern, EnumExtensions.ToDisplayName);

    public static new IReadOnlyDictionary<string, string> StageWithTotalNames { get; } =
        EnumHelper<StageWithTotal>.Enumerable.ToDictionary(EnumExtensions.ToPattern, EnumExtensions.ToDisplayName);

    public static new IEnumerable<string> StageKeysTotalFirst { get; } = StageWithTotalNames.Keys.RotateRight();

    public static new IEnumerable<string> StageKeysTotalLast { get; } = StageWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } =
        EnumHelper<Level>.Enumerable.ToDictionary(
            static level => level.ToPattern(),
            static level => CardTable.Count(pair => pair.Value.Level == level));

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } =
        EnumHelper<Stage>.Enumerable.ToDictionary(
            static stage => stage.ToPattern(),
            static stage => CardTable.Count(pair => pair.Value.Stage == stage));
}
