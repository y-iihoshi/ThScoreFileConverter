using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th105;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th105.Definitions;

namespace TemplateGenerator.Models.Th105;

#pragma warning disable CA1052 // Static holder types should be Static or NotInheritable
public class Definitions
#pragma warning restore CA1052 // Static holder types should be Static or NotInheritable
{
    private static readonly IEnumerable<(Chara, CardType, int)> NumCardsPerCharacterAndTypeImpl =
        EnumHelper.Cartesian<Chara, CardType>().Select(
            static pair => (pair.First, pair.Second, pair.Second == CardType.System
                ? SystemCardNameTable.Count
                : CardNameTable.Keys.Count(key => key.Chara == pair.First && key.CardId / 100 == (int)pair.Second)));

    public static string Title { get; } = StringResources.TH105;

    public static IReadOnlyDictionary<string, string> LevelNames { get; } =
        EnumHelper<Level>.Enumerable.ToPatternDictionary();

    public static IReadOnlyDictionary<string, string> LevelWithTotalNames { get; } =
        EnumHelper<LevelWithTotal>.Enumerable.ToPatternDictionary();

    public static IEnumerable<string> LevelKeysTotalFirst { get; } = LevelWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> LevelKeysTotalLast { get; } = LevelWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, (string Id, string ShortName, string LongName)> CharacterNames { get; } =
        EnumHelper<Chara>.Enumerable.ToDictionary(
            static chara => chara.ToPattern(),
            static chara => (chara.ToName(), chara.ToCharaName(), chara.ToCharaFullName()));

    public static IReadOnlyDictionary<string, int> NumCardsPerCharacter { get; } =
        StageInfoTable.ToDictionary(
            static pair => pair.Key.ToPattern(),
            static pair => pair.Value.Sum(static stageInfo => stageInfo.CardIds.Count()) * EnumHelper<Level>.NumValues);

    public static IReadOnlyDictionary<string, string> CardTypeNames { get; } =
        EnumHelper<CardType>.Enumerable.ToPatternDictionary();

    public static IReadOnlyDictionary<(string Chara, string CardType), int> NumCardsPerCharacterAndType { get; } =
        NumCardsPerCharacterAndTypeImpl.ToDictionary(
            static tuple => (tuple.Item1.ToPattern(), tuple.Item2.ToPattern()),
            static tuple => tuple.Item3);

    public static IReadOnlyDictionary<string, int> MaxNumCardsPerType { get; } =
        EnumHelper<CardType>.Enumerable.ToDictionary(
            static type => type.ToPattern(),
            static type => NumCardsPerCharacterAndTypeImpl.Where(tuple => tuple.Item2 == type).Max(static tuple => tuple.Item3));
}
