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
    }.ToStringKeyedDictionary();

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
    }.ToStringKeyedDictionary();

    public static IEnumerable<string> RouteKeysTotalFirst { get; } = RouteWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> RouteKeysTotalLast { get; } = RouteWithTotalNames.Keys;

    public static new IReadOnlyDictionary<string, string> StageNames { get; } = new[]
    {
        (Stage.A_1,   "Stage A-1"),
        (Stage.A1_2,  "Stage A1-2"),
        (Stage.A1_3,  "Stage A1-3"),
        (Stage.A2_2,  "Stage A2-2"),
        (Stage.A2_3,  "Stage A2-3"),
        (Stage.B_1,   "Stage B-1"),
        (Stage.B1_2,  "Stage B1-2"),
        (Stage.B1_3,  "Stage B1-3"),
        (Stage.B2_2,  "Stage B2-2"),
        (Stage.B2_3,  "Stage B2-3"),
        (Stage.C_1,   "Stage C-1"),
        (Stage.C1_2,  "Stage C1-2"),
        (Stage.C1_3,  "Stage C1-3"),
        (Stage.C2_2,  "Stage C2-2"),
        (Stage.C2_3,  "Stage C2-3"),
        (Stage.Extra, "Extra"),
    }.ToStringKeyedDictionary();

    public static new IReadOnlyDictionary<string, string> StageWithTotalNames { get; } = new[]
    {
        (StageWithTotal.A_1,   "Stage A-1"),
        (StageWithTotal.A1_2,  "Stage A1-2"),
        (StageWithTotal.A1_3,  "Stage A1-3"),
        (StageWithTotal.A2_2,  "Stage A2-2"),
        (StageWithTotal.A2_3,  "Stage A2-3"),
        (StageWithTotal.B_1,   "Stage B-1"),
        (StageWithTotal.B1_2,  "Stage B1-2"),
        (StageWithTotal.B1_3,  "Stage B1-3"),
        (StageWithTotal.B2_2,  "Stage B2-2"),
        (StageWithTotal.B2_3,  "Stage B2-3"),
        (StageWithTotal.C_1,   "Stage C-1"),
        (StageWithTotal.C1_2,  "Stage C1-2"),
        (StageWithTotal.C1_3,  "Stage C1-3"),
        (StageWithTotal.C2_2,  "Stage C2-2"),
        (StageWithTotal.C2_3,  "Stage C2-3"),
        (StageWithTotal.Extra, "Extra"),
        (StageWithTotal.Total, "Total"),
    }.ToStringKeyedDictionary();

    public static new IEnumerable<string> StageKeysTotalFirst { get; } = StageWithTotalNames.Keys.RotateRight();

    public static new IEnumerable<string> StageKeysTotalLast { get; } = StageWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } =
        EnumHelper<Level>.Enumerable.ToDictionary(
            static level => level.ToShortName(),
            static level => CardTable.Count(pair => pair.Value.Level == level));

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } =
        EnumHelper<Stage>.Enumerable.ToDictionary(
            static stage => stage.ToShortName(),
            static stage => CardTable.Count(pair => pair.Value.Stage == stage));
}
