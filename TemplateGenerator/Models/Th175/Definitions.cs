using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th175;

namespace TemplateGenerator.Models.Th175;

public class Definitions
{
    public static string Title { get; } = "東方剛欲異聞";

    public static IReadOnlyDictionary<string, string> LevelNames { get; } =
        EnumHelper<Level>.Enumerable.Where(level => level is Level.Normal or Level.Hard).ToDictionary(
            static level => level.ToShortName(),
            static level => (level.ToLongName().Length > 0) ? level.ToLongName() : level.ToString());

    public static IReadOnlyDictionary<string, string> CharacterNames { get; } = new[]
    {
        (Chara.Reimu,     "博麗 霊夢"),
        (Chara.Marisa,    "霧雨 魔理沙"),
        (Chara.Kanako,    "八坂 神奈子"),
        (Chara.Minamitsu, "村紗 水蜜"),
        (Chara.JoonShion, "依神 女苑 &amp; 紫苑"),
        (Chara.Flandre,   "フランドール・スカーレット"),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, string> CharacterWithTotalNames { get; } = new[]
    {
        (CharaWithTotal.Reimu,     "博麗 霊夢"),
        (CharaWithTotal.Marisa,    "霧雨 魔理沙"),
        (CharaWithTotal.Kanako,    "八坂 神奈子"),
        (CharaWithTotal.Minamitsu, "村紗 水蜜"),
        (CharaWithTotal.JoonShion, "依神 女苑 &amp; 紫苑"),
        (CharaWithTotal.Flandre,   "フランドール・スカーレット"),
        (CharaWithTotal.Total,     "全主人公合計"),
    }.ToStringKeyedDictionary();

    public static IEnumerable<string> CharacterKeysTotalFirst { get; } = CharacterWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> CharacterKeysTotalLast { get; } = CharacterWithTotalNames.Keys;
}
