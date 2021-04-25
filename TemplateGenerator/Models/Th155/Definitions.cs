using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th155;

namespace TemplateGenerator.Models.Th155
{
    public class Definitions
    {
        public static string Title { get; } = "東方憑依華";

        public static IReadOnlyDictionary<string, string> LevelNames { get; } =
            EnumHelper<Level>.Enumerable.ToDictionary(
                static level => level.ToShortName(),
                static level => (level.ToLongName().Length > 0) ? level.ToLongName() : level.ToString());

        public static IReadOnlyDictionary<string, string> StoryCharacterNames { get; } = new[]
        {
            (StoryChara.ReimuKasen,         "霊夢 &amp; 華扇"),
            (StoryChara.MarisaKoishi,       "魔理沙 &amp; こいし"),
            (StoryChara.NitoriKokoro,       "にとり &amp; こころ"),
            (StoryChara.MamizouMokou,       "マミゾウ &amp; 妹紅"),
            (StoryChara.MikoByakuren,       "神子 &amp; 白蓮"),
            (StoryChara.FutoIchirin,        "布都 &amp; 一輪"),
            (StoryChara.ReisenDoremy,       "鈴仙 &amp; ドレミー"),
            (StoryChara.SumirekoDoremy,     "菫子 &amp; ドレミー"),
            (StoryChara.TenshiShinmyoumaru, "天子 &amp; 針妙丸"),
            (StoryChara.YukariReimu,        "紫 &amp; 霊夢"),
            (StoryChara.JoonShion,          "女苑 &amp; 紫苑"),
        }.ToStringKeyedDictionary();
    }
}
