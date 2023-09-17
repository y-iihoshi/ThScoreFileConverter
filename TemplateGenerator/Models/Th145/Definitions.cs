using System.Collections.Generic;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th145;
using ThScoreFileConverter.Core.Resources;

namespace TemplateGenerator.Models.Th145;

public static class Definitions
{
    public static string Title { get; } = StringResources.TH145;

    public static IReadOnlyDictionary<string, string> LevelNames { get; } =
        EnumHelper<Level>.Enumerable.ToStringDictionary();

    public static IReadOnlyDictionary<string, string> LevelWithTotalNames { get; } =
        EnumHelper<LevelWithTotal>.Enumerable.ToStringDictionary();

    public static IEnumerable<string> LevelKeysTotalFirst { get; } = LevelWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> LevelKeysTotalLast { get; } = LevelWithTotalNames.Keys;

    public static IReadOnlyDictionary<string, string> CharacterNames { get; } = new[]
    {
        (Chara.ReimuA,       "博麗 霊夢（序）"),
        (Chara.Marisa,       "霧雨 魔理沙"),
        (Chara.IchirinUnzan, "雲居 一輪 &amp; 雲山"),
        (Chara.Byakuren,     "聖 白蓮"),
        (Chara.Futo,         "物部 布都"),
        (Chara.Miko,         "豊聡耳 神子"),
        (Chara.Nitori,       "河城 にとり"),
        (Chara.Koishi,       "古明地 こいし"),
        (Chara.Mamizou,      "二ッ岩 マミゾウ"),
        (Chara.Kokoro,       "秦 こころ"),
        (Chara.Kasen,        "茨木 華扇"),
        (Chara.Mokou,        "藤原 妹紅"),
        (Chara.Shinmyoumaru, "少名 針妙丸"),
        (Chara.Sumireko,     "宇佐見 菫子"),
        (Chara.ReimuB,       "博麗 霊夢（終）"),
    }.ToStringKeyedDictionary();

    public static IReadOnlyDictionary<string, string> CharacterWithTotalNames { get; } = new[]
    {
        (CharaWithTotal.ReimuA,       "博麗 霊夢（序）"),
        (CharaWithTotal.Marisa,       "霧雨 魔理沙"),
        (CharaWithTotal.IchirinUnzan, "雲居 一輪 &amp; 雲山"),
        (CharaWithTotal.Byakuren,     "聖 白蓮"),
        (CharaWithTotal.Futo,         "物部 布都"),
        (CharaWithTotal.Miko,         "豊聡耳 神子"),
        (CharaWithTotal.Nitori,       "河城 にとり"),
        (CharaWithTotal.Koishi,       "古明地 こいし"),
        (CharaWithTotal.Mamizou,      "二ッ岩 マミゾウ"),
        (CharaWithTotal.Kokoro,       "秦 こころ"),
        (CharaWithTotal.Kasen,        "茨木 華扇"),
        (CharaWithTotal.Mokou,        "藤原 妹紅"),
        (CharaWithTotal.Shinmyoumaru, "少名 針妙丸"),
        (CharaWithTotal.Sumireko,     "宇佐見 菫子"),
        (CharaWithTotal.ReimuB,       "博麗 霊夢（終）"),
        (CharaWithTotal.Total,        "全キャラ合計"),
    }.ToStringKeyedDictionary();

    public static IEnumerable<string> CharacterKeysTotalFirst { get; } = CharacterWithTotalNames.Keys.RotateRight();

    public static IEnumerable<string> CharacterKeysTotalLast { get; } = CharacterWithTotalNames.Keys;
}
