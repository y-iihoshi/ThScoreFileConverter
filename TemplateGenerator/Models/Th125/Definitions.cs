using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th125;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th125.Definitions;

namespace TemplateGenerator.Models.Th125;

public static class Definitions
{
    private static readonly IEnumerable<(Chara, IEnumerable<int>)> SpoilerScenesPerCharacterImpl = new[]
    {
        (Chara.Aya, Enemy.Hatate),
        (Chara.Hatate, Enemy.Aya),
    }.Select(static pair => (
        pair.Item1,
        SpellCards.Where(card => card.Key is (Level.Spoiler, _) && card.Value.Enemy == pair.Item2).Select(static card => card.Key.Scene)));

    private static readonly IEnumerable<(Level, int)> NumScenesPerLevelImpl =
        EnumHelper<Level>.Enumerable.Select(static level => (level, SpellCards.Count(pair => pair.Key.Level == level)));

    private static readonly int NumScenesWithSpoiler =
        NumScenesPerLevelImpl.Sum(static pair => pair.Item2);

    private static readonly int NumScenesWithoutSpoiler =
        NumScenesPerLevelImpl.Where(static pair => pair.Item1 != Level.Spoiler).Sum(static pair => pair.Item2);

    public static string Title { get; } = StringResources.TH125;

    public static IReadOnlyDictionary<string, (string Id, string ShortName, string LongName)> LevelNames { get; } = new[]
    {
        (Level.One,     "Level1"),
        (Level.Two,     "Level2"),
        (Level.Three,   "Level3"),
        (Level.Four,    "Level4"),
        (Level.Five,    "Level5"),
        (Level.Six,     "Level6"),
        (Level.Seven,   "Level7"),
        (Level.Eight,   "Level8"),
        (Level.Nine,    "Level9"),
        (Level.Ten,     "Level10"),
        (Level.Eleven,  "Level11"),
        (Level.Twelve,  "Level12"),
        (Level.Extra,   "LevelEX"),
        (Level.Spoiler, "SPOILER"),
    }.ToDictionary(
        static pair => pair.Item1.ToPattern(),
        static pair => (pair.Item2, pair.Item1.ToDisplayShortName(), pair.Item1.ToDisplayName()));

    public static IReadOnlyDictionary<string, (string Id, string ShortName, string LongName)> CharacterNames { get; } =
        EnumHelper<Chara>.Enumerable.ToDictionary(
            static chara => chara.ToPattern(),
            static chara => (chara.ToName(), chara.ToCharaName(), chara.ToCharaFullName()));

    public static IReadOnlyDictionary<string, IEnumerable<int>> SpoilerScenesPerCharacter { get; } =
        SpoilerScenesPerCharacterImpl.ToPatternKeyedDictionary();

    public static IReadOnlyDictionary<string, int> NumScenesPerLevel { get; } =
        NumScenesPerLevelImpl.ToPatternKeyedDictionary();

    public static IReadOnlyDictionary<string, int> NumScenesPerCharacter { get; } =
        SpoilerScenesPerCharacterImpl.ToDictionary(
            static pair => pair.Item1.ToPattern(),
            static pair => pair.Item2.Count() + NumScenesWithoutSpoiler);

    public static IReadOnlyDictionary<string, int> NumScenesPerCharacterInGame { get; } = new[]
    {
        (Chara.Aya,    NumScenesWithSpoiler),
        (Chara.Hatate, NumScenesWithoutSpoiler),
    }.ToPatternKeyedDictionary();
}
