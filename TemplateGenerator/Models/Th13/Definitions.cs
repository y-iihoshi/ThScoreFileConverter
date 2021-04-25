using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th13;

namespace TemplateGenerator.Models.Th13
{
    public class Definitions : Models.Definitions
    {
        private static readonly IEnumerable<(LevelPractice, int)> NumCardsPerLevelImpl = new[]
        {
            (LevelPractice.Easy,      24),
            (LevelPractice.Normal,    26),
            (LevelPractice.Hard,      28),
            (LevelPractice.Lunatic,   28),
            (LevelPractice.Extra,     13),
            (LevelPractice.OverDrive,  8),
        };

        public static string Title { get; } = "東方神霊廟";

        public static IReadOnlyDictionary<string, string> LevelSpellPracticeNames { get; } =
            EnumHelper<LevelPractice>.Enumerable.ToDictionary(
                level => level.ToShortName(),
                level => (level.ToLongName().Length > 0) ? level.ToLongName() : level.ToString());

        public static IReadOnlyDictionary<string, string> CharacterNames { get; } = new[]
        {
            (Chara.Reimu,  "博麗 霊夢"),
            (Chara.Marisa, "霧雨 魔理沙"),
            (Chara.Sanae,  "東風谷 早苗"),
            (Chara.Youmu,  "魂魄 妖夢"),
        }.ToStringKeyedDictionary();

        public static IReadOnlyDictionary<string, string> CharacterWithTotalNames { get; } = new[]
        {
            (CharaWithTotal.Reimu,  "博麗 霊夢"),
            (CharaWithTotal.Marisa, "霧雨 魔理沙"),
            (CharaWithTotal.Sanae,  "東風谷 早苗"),
            (CharaWithTotal.Youmu,  "魂魄 妖夢"),
            (CharaWithTotal.Total,  "全主人公合計"),
        }.ToStringKeyedDictionary();

        public static IEnumerable<string> CharacterKeysTotalFirst { get; } = CharacterWithTotalNames.Keys.RotateRight();

        public static IEnumerable<string> CharacterKeysTotalLast { get; } = CharacterWithTotalNames.Keys;

        public static IReadOnlyDictionary<string, string> StageSpellPracticeNames { get; } = new[]
        {
            (StagePractice.One,       "Stage 1"),
            (StagePractice.Two,       "Stage 2"),
            (StagePractice.Three,     "Stage 3"),
            (StagePractice.Four,      "Stage 4"),
            (StagePractice.Five,      "Stage 5"),
            (StagePractice.Six,       "Stage 6"),
            (StagePractice.Extra,     "Extra"),
            (StagePractice.OverDrive, "Over Drive"),
        }.ToStringKeyedDictionary();

        public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } =
            NumCardsPerLevelImpl.ToStringKeyedDictionary();

        public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } = new[]
        {
            (StagePractice.One,       14),
            (StagePractice.Two,       16),
            (StagePractice.Three,     14),
            (StagePractice.Four,      15),
            (StagePractice.Five,      19),
            (StagePractice.Six,       28),
            (StagePractice.Extra,     13),
            (StagePractice.OverDrive,  8),
        }.ToStringKeyedDictionary();

        public static int NumCardsWithOverDrive { get; } =
            NumCardsPerLevelImpl.Sum(pair => pair.Item2);

        public static int NumCardsWithoutOverDrive { get; } =
            NumCardsPerLevelImpl.Where(pair => pair.Item1 != LevelPractice.OverDrive).Sum(pair => pair.Item2);
    }
}
