using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
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
        (Level.One,   ("Level1",  "Level 1")),
        (Level.Two,   ("Level2",  "Level 2")),
        (Level.Three, ("Level3",  "Level 3")),
        (Level.Four,  ("Level4",  "Level 4")),
        (Level.Five,  ("Level5",  "Level 5")),
        (Level.Six,   ("Level6",  "Level 6")),
        (Level.Seven, ("Level7",  "Level 7")),
        (Level.Eight, ("Level8",  "Level 8")),
        (Level.Nine,  ("Level9",  "Level 9")),
        (Level.Ten,   ("Level10", "Level 10")),
        (Level.Extra, ("Extra",   "Level Extra")),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, int> NumScenesPerLevel { get; } =
        EnumHelper<Level>.Enumerable.ToDictionary(
            static level => level.ToShortName(),
            static level => SpellCards.Keys.Count(key => key.Level == level));
}
