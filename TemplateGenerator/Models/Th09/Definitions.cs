using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th09;

namespace TemplateGenerator.Models.Th09
{
    public class Definitions : Models.Definitions
    {
        public static string Title { get; } = "東方花映塚";

        public static IReadOnlyDictionary<string, (string Id, string ShortName, string LongName)> CharacterNames { get; } = new[]
        {
            (Chara.Reimu,     "霊夢",       "博麗 霊夢"),
            (Chara.Marisa,    "魔理沙",     "霧雨 魔理沙"),
            (Chara.Sakuya,    "咲夜",       "十六夜 咲夜"),
            (Chara.Youmu,     "妖夢",       "魂魄 妖夢"),
            (Chara.Reisen,    "鈴仙",       "鈴仙・優曇華院・イナバ"),
            (Chara.Cirno,     "チルノ",     "チルノ"),
            (Chara.Lyrica,    "リリカ",     "リリカ・プリズムリバー"),
            (Chara.Mystia,    "ミスティア", "ミスティア・ローレライ"),
            (Chara.Tewi,      "てゐ",       "因幡 てゐ"),
            (Chara.Aya,       "文",         "射命丸 文"),
            (Chara.Medicine,  "メディスン", "メディスン・メランコリー"),
            (Chara.Yuuka,     "幽香",       "風見 幽香"),
            (Chara.Komachi,   "小町",       "小野塚 小町"),
            (Chara.Shikieiki, "四季映姫",   "四季映姫・ヤマザナドゥ"),
        }.ToDictionary(
            static tuple => tuple.Item1.ToShortName(),
            static tuple => (tuple.Item1.ToString(), tuple.Item2, tuple.Item3));

        public static IReadOnlyList<string> RankOrdinals { get; } = new[]
        {
            "0th",  // unused
            "1st",
            "2nd",
            "3rd",
            "4th",
            "5th",
        };
    }
}
