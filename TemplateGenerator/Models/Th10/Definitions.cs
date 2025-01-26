using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th10;
using ThScoreFileConverter.Core.Resources;
using static ThScoreFileConverter.Core.Models.Th10.Definitions;

namespace TemplateGenerator.Models.Th10;

public class Definitions : Models.Definitions
{
    public static string Title { get; } = StringResources.TH10;

    public static IReadOnlyDictionary<string, (string Name, string Equip)> CharacterNames { get; } =
        EnumHelper<Chara>.Enumerable.ToDictionary(
            static chara => chara.ToPattern(),
            static chara => (chara.ToCharaFullName(), $"（{chara.ToShotTypeFullName()}）"));  // FIXME

    public static IReadOnlyDictionary<string, (string Name, string Equip)> CharacterWithTotalNames { get; } =
        EnumHelper<CharaWithTotal>.Enumerable.ToDictionary(
            static chara => chara.ToPattern(),
            static chara => (chara == CharaWithTotal.Total)
                ? ("全主人公合計", string.Empty)
                : (chara.ToCharaFullName(), $"（{chara.ToShotTypeFullName()}）"));  // FIXME

    public static IEnumerable<string> CharacterKeysTotalFirst { get; } = CharacterWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> CharacterKeysTotalLast { get; } = CharacterWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } =
        EnumHelper<Level>.Enumerable.ToDictionary(
            static level => level.ToPattern(),
            static level => CardTable.Count(pair => pair.Value.Level == level));

    public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } =
        EnumHelper<Stage>.Enumerable.ToDictionary(
            static stage => stage.ToPattern(),
            static stage => CardTable.Count(pair => pair.Value.Stage == stage));
}
