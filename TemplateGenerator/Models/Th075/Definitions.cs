using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th075;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th075.Definitions;

namespace TemplateGenerator.Models.Th075;

public static class Definitions
{
    public static string Title { get; } = StringResources.TH075;

    public static IReadOnlyDictionary<string, string> LevelNames { get; } =
        EnumHelper<Level>.Enumerable.ToPatternDictionary();

    public static IReadOnlyDictionary<string, string> LevelWithTotalNames { get; } =
        EnumHelper<LevelWithTotal>.Enumerable.ToPatternDictionary();

    public static IEnumerable<string> LevelKeysTotalFirst { get; } = LevelWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> LevelKeysTotalLast { get; } = LevelWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, (string Id, string ShortName, string LongName)> CharacterNames { get; } =
        EnumHelper<Chara>.Enumerable.ToDictionary(
            static chara => chara.ToPattern(),
            static chara => (chara.ToString(), chara.ToCharaName(), chara.ToCharaFullName()));

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } =
        EnumHelper<Level>.Enumerable.ToDictionary(
            static level => level.ToPattern(),
            static level => CardTable.Count(pair => CardIdTable[Chara.Reimu].Any(id => id == pair.Key) && (pair.Value.Level == level)));
}
