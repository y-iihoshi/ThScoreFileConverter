using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models.Th09;
using ThScoreFileConverter.Core.Resources;

namespace TemplateGenerator.Models.Th09;

public class Definitions : Models.Definitions
{
    public static string Title { get; } = StringResources.TH09;

    public static IReadOnlyDictionary<string, (string Id, string ShortName, string LongName)> CharacterNames { get; } =
#if false
        EnumHelper<Chara>.Enumerable.Where(static chara => chara is not Chara.Lunasa and not Chara.Merlin)
#else
        new[]
        {
            Chara.Reimu,
            Chara.Marisa,
            Chara.Sakuya,
            Chara.Youmu,
            Chara.Reisen,
            Chara.Cirno,
            Chara.Lyrica,
            Chara.Mystia,
            Chara.Tewi,
            Chara.Aya,
            Chara.Medicine,
            Chara.Yuuka,     // NOTE: The sort order does not match with the defined values in the Chara enum.
            Chara.Komachi,
            Chara.Eiki,
        }
#endif
        .ToDictionary(
            static chara => chara.ToPattern(),
            static chara => (chara.ToName(), chara.ToCharaName(), chara.ToCharaFullName()));

    public static IReadOnlyList<string> RankOrdinals { get; } =
    [
        "0th",  // unused
        "1st",
        "2nd",
        "3rd",
        "4th",
        "5th",
    ];
}
