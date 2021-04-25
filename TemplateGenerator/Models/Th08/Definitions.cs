using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th08;

namespace TemplateGenerator.Models.Th08
{
    public class Definitions : Models.Definitions
    {
        private static readonly IEnumerable<(Stage, string)> StageNamesImpl = new[]
        {
            (Stage.One,          "Stage 1"),
            (Stage.Two,          "Stage 2"),
            (Stage.Three,        "Stage 3"),
            (Stage.FourUncanny,  "Stage 4A"),
            (Stage.FourPowerful, "Stage 4B"),
            (Stage.Five,         "Stage 5"),
            (Stage.FinalA,       "Stage 6A"),
            (Stage.FinalB,       "Stage 6B"),
            (Stage.Extra,        "Extra"),
        };

        private static readonly IEnumerable<(LevelPractice, int)> NumCardsPerLevelImpl = new[]
        {
            (LevelPractice.Easy,     42),
            (LevelPractice.Normal,   49),
            (LevelPractice.Hard,     50),
            (LevelPractice.Lunatic,  50),
            (LevelPractice.Extra,    14),
            (LevelPractice.LastWord, 17),
        };

        public static string Title { get; } = "東方永夜抄";

        public static IReadOnlyDictionary<string, string> LevelSpellPracticeNames { get; } =
            EnumHelper<LevelPractice>.Enumerable.ToDictionary(
                static level => level.ToShortName(),
                static level => (level.ToLongName().Length > 0) ? level.ToLongName() : level.ToString());

        public static IReadOnlyDictionary<string, (string ShortName, string LongName)> CharacterNames { get; } = new[]
        {
            (Chara.ReimuYukari,   ("霊夢 &amp; 紫",       "霊夢 &amp; 紫")),
            (Chara.MarisaAlice,   ("魔理沙 &amp; アリス", "魔理沙 &amp; アリス")),
            (Chara.SakuyaRemilia, ("咲夜 &amp; レミリア", "咲夜 &amp; レミリア")),
            (Chara.YoumuYuyuko,   ("妖夢 &amp; 幽々子",   "妖夢 &amp; 幽々子")),
            (Chara.Reimu,         ("霊夢",                "博麗 霊夢")),
            (Chara.Yukari,        ("紫",                  "八雲 紫")),
            (Chara.Marisa,        ("魔理沙",              "霧雨 魔理沙")),
            (Chara.Alice,         ("アリス",              "アリス・マーガトロイド")),
            (Chara.Sakuya,        ("咲夜",                "十六夜 咲夜")),
            (Chara.Remilia,       ("レミリア",            "レミリア・スカーレット")),
            (Chara.Youmu,         ("妖夢",                "魂魄 妖夢")),
            (Chara.Yuyuko,        ("幽々子",              "西行寺 幽々子")),
        }.ToStringKeyedDictionary();

        public static IReadOnlyDictionary<string, (string ShortName, string LongName)> CharacterWithTotalNames { get; } = new[]
        {
            (CharaWithTotal.ReimuYukari,   ("霊夢 &amp; 紫",       "霊夢 &amp; 紫")),
            (CharaWithTotal.MarisaAlice,   ("魔理沙 &amp; アリス", "魔理沙 &amp; アリス")),
            (CharaWithTotal.SakuyaRemilia, ("咲夜 &amp; レミリア", "咲夜 &amp; レミリア")),
            (CharaWithTotal.YoumuYuyuko,   ("妖夢 &amp; 幽々子",   "妖夢 &amp; 幽々子")),
            (CharaWithTotal.Reimu,         ("霊夢",                "博麗 霊夢")),
            (CharaWithTotal.Yukari,        ("紫",                  "八雲 紫")),
            (CharaWithTotal.Marisa,        ("魔理沙",              "霧雨 魔理沙")),
            (CharaWithTotal.Alice,         ("アリス",              "アリス・マーガトロイド")),
            (CharaWithTotal.Sakuya,        ("咲夜",                "十六夜 咲夜")),
            (CharaWithTotal.Remilia,       ("レミリア",            "レミリア・スカーレット")),
            (CharaWithTotal.Youmu,         ("妖夢",                "魂魄 妖夢")),
            (CharaWithTotal.Yuyuko,        ("幽々子",              "西行寺 幽々子")),
            (CharaWithTotal.Total,         ("全主人公合計",        "全主人公合計")),
        }.ToStringKeyedDictionary();

        public static IEnumerable<string> CharacterKeysTotalFirst { get; } = CharacterWithTotalNames.Keys.RotateRight();

        public static IEnumerable<string> CharacterKeysTotalLast { get; } = CharacterWithTotalNames.Keys;

        public static new IReadOnlyDictionary<string, string> StageNames { get; } =
            StageNamesImpl.ToStringKeyedDictionary();

        public static new IReadOnlyDictionary<string, string> StagePracticeNames { get; } =
            StageNamesImpl.Where(static pair => CanPractice(pair.Item1)).ToStringKeyedDictionary();

        public static IReadOnlyDictionary<string, string> StageSpellPracticeNames { get; } = new[]
        {
            (StagePractice.One,          "Stage 1"),
            (StagePractice.Two,          "Stage 2"),
            (StagePractice.Three,        "Stage 3"),
            (StagePractice.FourUncanny,  "Stage 4A"),
            (StagePractice.FourPowerful, "Stage 4B"),
            (StagePractice.Five,         "Stage 5"),
            (StagePractice.FinalA,       "Stage 6A"),
            (StagePractice.FinalB,       "Stage 6B"),
            (StagePractice.Extra,        "Extra"),
            (StagePractice.LastWord,     "Last Word"),
        }.ToStringKeyedDictionary();

        public static new IReadOnlyDictionary<string, string> StageWithTotalNames { get; } = new[]
        {
            (StageWithTotal.One,          "Stage 1"),
            (StageWithTotal.Two,          "Stage 2"),
            (StageWithTotal.Three,        "Stage 3"),
            (StageWithTotal.FourUncanny,  "Stage 4A"),
            (StageWithTotal.FourPowerful, "Stage 4B"),
            (StageWithTotal.Five,         "Stage 5"),
            (StageWithTotal.FinalA,       "Stage 6A"),
            (StageWithTotal.FinalB,       "Stage 6B"),
            (StageWithTotal.Extra,        "Extra"),
            (StageWithTotal.Total,        "Total"),
        }.ToStringKeyedDictionary();

        public static new IEnumerable<string> StageKeysTotalFirst { get; } = StageWithTotalNames.Keys.RotateRight();

        public static new IEnumerable<string> StageKeysTotalLast { get; } = StageWithTotalNames.Keys;

        public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } =
            NumCardsPerLevelImpl.ToStringKeyedDictionary();

        public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } = new[]
        {
            (StagePractice.One,          13),
            (StagePractice.Two,          19),
            (StagePractice.Three,        22),
            (StagePractice.FourUncanny,  23),
            (StagePractice.FourPowerful, 23),
            (StagePractice.Five,         19),
            (StagePractice.FinalA,       28),
            (StagePractice.FinalB,       44),
            (StagePractice.Extra,        14),
            (StagePractice.LastWord,     17),
        }.ToStringKeyedDictionary();

        public static IReadOnlyDictionary<(string, string), int> NumCardsPerStage4Level { get; } = new[]
        {
            (StagePractice.FourUncanny,  LevelPractice.Easy,     5),
            (StagePractice.FourUncanny,  LevelPractice.Normal,   6),
            (StagePractice.FourUncanny,  LevelPractice.Hard,     6),
            (StagePractice.FourUncanny,  LevelPractice.Lunatic,  6),
            (StagePractice.FourUncanny,  LevelPractice.Extra,    0),
            (StagePractice.FourUncanny,  LevelPractice.LastWord, 0),
            (StagePractice.FourPowerful, LevelPractice.Easy,     5),
            (StagePractice.FourPowerful, LevelPractice.Normal,   6),
            (StagePractice.FourPowerful, LevelPractice.Hard,     6),
            (StagePractice.FourPowerful, LevelPractice.Lunatic,  6),
            (StagePractice.FourPowerful, LevelPractice.Extra,    0),
            (StagePractice.FourPowerful, LevelPractice.LastWord, 0),
        }.ToDictionary(
            static tuple => (tuple.Item1.ToShortName(), tuple.Item2.ToShortName()),
            static tuple => tuple.Item3);

        public static int NumCardsWithLastWord { get; } =
            NumCardsPerLevelImpl.Sum(static pair => pair.Item2);

        public static int NumCardsWithoutLastWord { get; } =
            NumCardsPerLevelImpl.Where(static pair => pair.Item1 != LevelPractice.LastWord).Sum(static pair => pair.Item2);

        public static IReadOnlyDictionary<string, string> UnreachableStagesPerCharacter { get; } = new[]
        {
            (Chara.ReimuYukari,   Stage.FourUncanny),
            (Chara.MarisaAlice,   Stage.FourPowerful),
            (Chara.SakuyaRemilia, Stage.FourPowerful),
            (Chara.YoumuYuyuko,   Stage.FourUncanny),
            (Chara.Reimu,         Stage.FourUncanny),
            (Chara.Yukari,        Stage.FourUncanny),
            (Chara.Marisa,        Stage.FourPowerful),
            (Chara.Alice,         Stage.FourPowerful),
            (Chara.Sakuya,        Stage.FourPowerful),
            (Chara.Remilia,       Stage.FourPowerful),
            (Chara.Youmu,         Stage.FourUncanny),
            (Chara.Yuyuko,        Stage.FourUncanny),
        }.ToDictionary(static pair => pair.Item1.ToShortName(), static pair => pair.Item2.ToShortName());

        public static bool CanPractice(Stage stage)
        {
            return stage != Stage.Extra;
        }
    }
}
