using System.Collections.Generic;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Models.Th095;

namespace TemplateGenerator.Models.Th095
{
    public class Definitions
    {
        public static string Title { get; } = "東方文花帖";

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

        public static IReadOnlyDictionary<string, int> NumScenesPerLevel { get; } = new[]
        {
            (Level.One,   6),
            (Level.Two,   6),
            (Level.Three, 8),
            (Level.Four,  9),
            (Level.Five,  8),
            (Level.Six,   8),
            (Level.Seven, 8),
            (Level.Eight, 8),
            (Level.Nine,  8),
            (Level.Ten,   8),
            (Level.Extra, 8),
        }.ToStringKeyedDictionary();
    }
}
