using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th143;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th143.Definitions;

namespace TemplateGenerator.Models.Th143;

public static class Definitions
{
    private static readonly IEnumerable<(ItemWithTotal, string)> ItemWithTotalNamesImpl =
    [
        (ItemWithTotal.Fablic,   "布"),
        (ItemWithTotal.Camera,   "カメラ"),
        (ItemWithTotal.Umbrella, "傘"),
        (ItemWithTotal.Lantern,  "提灯"),
        (ItemWithTotal.Orb,      "陰陽玉"),
        (ItemWithTotal.Bomb,     "ボム"),
        (ItemWithTotal.Jizou,    "地蔵"),
        (ItemWithTotal.Doll,     "人形"),
        (ItemWithTotal.Mallet,   "小槌"),
        (ItemWithTotal.NoItem,   "未使用"),
        (ItemWithTotal.Total,    "合計"),
    ];

    public static string Title { get; } = StringResources.TH143;

    public static IReadOnlyDictionary<string, (string Id, string Name)> DayNames { get; } = new[]
    {
        (Day.First,   ("Day1",    "一日目")),
        (Day.Second,  ("Day2",    "二日目")),
        (Day.Third,   ("Day3",    "三日目")),
        (Day.Fourth,  ("Day4",    "四日目")),
        (Day.Fifth,   ("Day5",    "五日目")),
        (Day.Sixth,   ("Day6",    "六日目")),
        (Day.Seventh, ("Day7",    "七日目")),
        (Day.Eighth,  ("Day8",    "八日目")),
        (Day.Ninth,   ("Day9",    "九日目")),
        (Day.Last,    ("LastDay", "最終日")),
    }.ToPatternKeyedDictionary();

    public static IReadOnlyDictionary<string, int> NumScenesPerDay { get; } =
        EnumHelper<Day>.Enumerable.ToDictionary(
            static day => day.ToPattern(),
            static day => SpellCards.Count(pair => pair.Key.Day == day));

    public static IReadOnlyDictionary<string, (string ShortName, string LongName)> ItemWithTotalNames { get; } =
        ItemWithTotalNamesImpl.ToDictionary(
            static pair => pair.Item1.ToPattern(),
            static pair => (pair.Item2, pair.Item1.ToDisplayName()));

    public static IReadOnlyDictionary<string, (string ShortName, string LongName)> ItemNames { get; } =
        ItemWithTotalNamesImpl.Where(static pair => pair.Item1 != ItemWithTotal.Total).ToDictionary(
            static pair => pair.Item1.ToPattern(),
            static pair => (pair.Item2, pair.Item1.ToDisplayName()));

    public static IEnumerable<string> ItemKeysTotalFirst { get; } = ItemWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> ItemKeysTotalLast { get; } = ItemWithTotalNames.Keys;

    public static int NumNicknames { get; } = Nicknames.Count;
}
