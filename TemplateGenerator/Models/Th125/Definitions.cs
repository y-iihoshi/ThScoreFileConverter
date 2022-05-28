using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models.Th125;
using ThScoreFileConverter.Models.Th125;

namespace TemplateGenerator.Models.Th125;

public class Definitions
{
    private static readonly IEnumerable<(Chara, IEnumerable<int>)> SpoilerScenesPerCharacterImpl = new[]
    {
        (Chara.Aya,    Enumerable.Range(start: 1, count: 4)),
        (Chara.Hatate, Enumerable.Range(start: 5, count: 5)),
    };

    private static readonly IEnumerable<(Level, int)> NumScenesPerLevelImpl = new[]
    {
        (Level.One,     6),
        (Level.Two,     6),
        (Level.Three,   8),
        (Level.Four,    7),
        (Level.Five,    8),
        (Level.Six,     8),
        (Level.Seven,   7),
        (Level.Eight,   8),
        (Level.Nine,    8),
        (Level.Ten,     8),
        (Level.Eleven,  8),
        (Level.Twelve,  8),
        (Level.Extra,   9),
        (Level.Spoiler, SpoilerScenesPerCharacterImpl.Sum(static pair => pair.Item2.Count())),
    };

    private static readonly int NumScenesWithSpoiler =
        NumScenesPerLevelImpl.Sum(static pair => pair.Item2);

    private static readonly int NumScenesWithoutSpoiler =
        NumScenesPerLevelImpl.Where(static pair => pair.Item1 != Level.Spoiler).Sum(static pair => pair.Item2);

    public static string Title { get; } = "ダブルスポイラー";

    public static IReadOnlyDictionary<string, (string Id, string ShortName, string LongName)> LevelNames { get; } = new[]
    {
        (Level.One,     ("Level1",  "1",  "Level 1")),
        (Level.Two,     ("Level2",  "2",  "Level 2")),
        (Level.Three,   ("Level3",  "3",  "Level 3")),
        (Level.Four,    ("Level4",  "4",  "Level 4")),
        (Level.Five,    ("Level5",  "5",  "Level 5")),
        (Level.Six,     ("Level6",  "6",  "Level 6")),
        (Level.Seven,   ("Level7",  "7",  "Level 7")),
        (Level.Eight,   ("Level8",  "8",  "Level 8")),
        (Level.Nine,    ("Level9",  "9",  "Level 9")),
        (Level.Ten,     ("Level10", "10", "Level 10")),
        (Level.Eleven,  ("Level11", "11", "Level 11")),
        (Level.Twelve,  ("Level12", "12", "Level 12")),
        (Level.Extra,   ("LevelEX", "EX", "Level EX")),
        (Level.Spoiler, ("SPOILER", "??", "SPOILER")),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, (string Id, string ShortName, string LongName)> CharacterNames { get; } = new[]
    {
        (Chara.Aya,    "文",     "射命丸 文"),
        (Chara.Hatate, "はたて", "姫海棠 はたて"),
    }.ToDictionary(
        static tuple => tuple.Item1.ToShortName(),
        static tuple => (tuple.Item1.ToString(), tuple.Item2, tuple.Item3));

    public static IReadOnlyDictionary<string, IEnumerable<int>> SpoilerScenesPerCharacter { get; } =
        SpoilerScenesPerCharacterImpl.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, int> NumScenesPerLevel { get; } =
        NumScenesPerLevelImpl.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, int> NumScenesPerCharacter { get; } =
        SpoilerScenesPerCharacterImpl.ToDictionary(
            static pair => pair.Item1.ToShortName(),
            static pair => pair.Item2.Count() + NumScenesWithoutSpoiler);

    public static IReadOnlyDictionary<string, int> NumScenesPerCharacterInGame { get; } = new[]
    {
        (Chara.Aya,    NumScenesWithSpoiler),
        (Chara.Hatate, NumScenesWithoutSpoiler),
    }.ToStringKeyedDictionary();
}
