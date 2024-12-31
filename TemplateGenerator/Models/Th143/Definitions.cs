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
    public static string Title { get; } = StringResources.TH143;

    public static IReadOnlyDictionary<string, (string Id, string Name)> DayNames { get; } = new[]
    {
        (Day.First,   "Day1"),
        (Day.Second,  "Day2"),
        (Day.Third,   "Day3"),
        (Day.Fourth,  "Day4"),
        (Day.Fifth,   "Day5"),
        (Day.Sixth,   "Day6"),
        (Day.Seventh, "Day7"),
        (Day.Eighth,  "Day8"),
        (Day.Ninth,   "Day9"),
        (Day.Last,    "LastDay"),
    }.ToDictionary(
        static pair => pair.Item1.ToPattern(),
        static pair => (pair.Item2, pair.Item1.ToDisplayName()));

    public static IReadOnlyDictionary<string, int> NumScenesPerDay { get; } =
        EnumHelper<Day>.Enumerable.ToDictionary(
            EnumExtensions.ToPattern,
            static day => SpellCards.Count(pair => pair.Key.Day == day));

    public static IReadOnlyDictionary<string, (string ShortName, string LongName)> ItemWithTotalNames { get; } =
        EnumHelper<ItemWithTotal>.Enumerable.ToDictionary(
            EnumExtensions.ToPattern,
            static item => (item.ToDisplayShortName(), item.ToDisplayName()));

    public static IReadOnlyDictionary<string, (string ShortName, string LongName)> ItemNames { get; } =
        EnumHelper<ItemWithTotal>.Enumerable.Where(static item => item != ItemWithTotal.Total).ToDictionary(
            EnumExtensions.ToPattern,
            static item => (item.ToDisplayShortName(), item.ToDisplayName()));

    public static IEnumerable<string> ItemKeysTotalFirst { get; } = ItemWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> ItemKeysTotalLast { get; } = ItemWithTotalNames.Keys;

    public static int NumNicknames { get; } = Nicknames.Count;
}
