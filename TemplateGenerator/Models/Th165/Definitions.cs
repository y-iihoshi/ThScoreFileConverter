using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th165;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th165.Definitions;

namespace TemplateGenerator.Models.Th165;

public static class Definitions
{
    public static string Title { get; } = StringResources.TH165;

    public static IReadOnlyDictionary<string, (string Id, string Name)> DayNames { get; } =
        EnumHelper<Day>.Enumerable.ToDictionary(
            static day => day.ToPattern(),
            static day => (day.ToString(), day.ToDisplayName()));

    public static IReadOnlyDictionary<string, int> NumDreamsPerDay { get; } =
        EnumHelper<Day>.Enumerable.ToDictionary(
            static day => day.ToPattern(),
            static day => SpellCards.Count(pair => pair.Key.Day == day));

    public static int NumNicknames { get; } = Nicknames.Count;
}
