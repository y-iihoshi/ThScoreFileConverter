using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th123;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th123.Definitions;
using CardType = ThScoreFileConverter.Core.Models.Th105.CardType;
using Level = ThScoreFileConverter.Core.Models.Th105.Level;

namespace TemplateGenerator.Models.Th123;

public class Definitions : Th105.Definitions
{
    private static readonly IEnumerable<(Chara, CardType, int)> NumCardsPerCharacterAndTypeImpl =
        EnumHelper.Cartesian<Chara, CardType>().Select(
            static pair => (pair.First, pair.Second, pair.Second == CardType.System
                ? SystemCardNameTable.Count
                : CardNameTable.Keys.Count(key => key.Chara == pair.First && key.CardId / 100 == (int)pair.Second)));

    public static new string Title { get; } = StringResources.TH123;

    public static new IReadOnlyDictionary<string, (string Id, string ShortName, string LongName)> CharacterNames { get; } =
        EnumHelper<Chara>.Enumerable.Where(static chara => chara != Chara.Catfish).ToDictionary(
            static chara => chara.ToPattern(),
            static chara => (chara.ToName(), chara.ToCharaName(), chara.ToCharaFullName()));

    public static IReadOnlyDictionary<string, (string Id, string ShortName, string LongName)> StoryCharacterNames { get; } =
        EnumHelper<Chara>.Enumerable.Where(static chara => HasStory(chara)).ToDictionary(
            static chara => chara.ToPattern(),
            static chara => (chara.ToName(), chara.ToCharaName(), chara.ToCharaFullName()));

    public static new IReadOnlyDictionary<string, int> NumCardsPerCharacter { get; } =
        StageInfoTable.ToDictionary(
            static pair => pair.Key.ToPattern(),
            static pair => pair.Value.Sum(static stageInfo => stageInfo.CardIds.Count()) * EnumHelper<Level>.NumValues);

    public static new IReadOnlyDictionary<(string Chara, string CardType), int> NumCardsPerCharacterAndType { get; } =
        NumCardsPerCharacterAndTypeImpl.ToDictionary(
            static tuple => (tuple.Item1.ToPattern(), tuple.Item2.ToPattern()),
            static tuple => tuple.Item3);

    public static new IReadOnlyDictionary<string, int> MaxNumCardsPerType { get; } =
        EnumHelper<CardType>.Enumerable.ToDictionary(
            static type => type.ToPattern(),
            static type => NumCardsPerCharacterAndTypeImpl
                .Where(tuple => tuple.Item2 == type).Max(static tuple => tuple.Item3));
}
