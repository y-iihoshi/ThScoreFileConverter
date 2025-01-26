using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th095;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th095.Definitions;

namespace TemplateGenerator.Models.Th095;

public static class Definitions
{
    public static string Title { get; } = StringResources.TH095;

    public static IReadOnlyDictionary<string, (string Id, string Name)> LevelNames { get; } = new[]
    {
        (Level.One,   "Level1"),
        (Level.Two,   "Level2"),
        (Level.Three, "Level3"),
        (Level.Four,  "Level4"),
        (Level.Five,  "Level5"),
        (Level.Six,   "Level6"),
        (Level.Seven, "Level7"),
        (Level.Eight, "Level8"),
        (Level.Nine,  "Level9"),
        (Level.Ten,   "Level10"),
        (Level.Extra, "Extra"),
    }.ToDictionary(
        static pair => pair.Item1.ToPattern(),
        static pair => (pair.Item2, pair.Item1.ToDisplayName()));

    public static IReadOnlyDictionary<string, int> NumScenesPerLevel { get; } =
        EnumHelper<Level>.Enumerable.ToDictionary(
            static level => level.ToPattern(),
            static level => SpellCards.Keys.Count(key => key.Level == level));
}
