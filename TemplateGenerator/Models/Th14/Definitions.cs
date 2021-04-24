using System.Collections.Generic;
using TemplateGenerator.Extensions;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th14;

namespace TemplateGenerator.Models.Th14
{
    public class Definitions : Models.Definitions
    {
        public static string Title { get; } = "東方輝針城";

        public static IReadOnlyDictionary<string, string> CharacterNames { get; } = new[]
        {
            (Chara.ReimuA,  "博麗 霊夢 (A)"),
            (Chara.ReimuB,  "博麗 霊夢 (B)"),
            (Chara.MarisaA, "霧雨 魔理沙 (A)"),
            (Chara.MarisaB, "霧雨 魔理沙 (B)"),
            (Chara.SakuyaA, "十六夜 咲夜 (A)"),
            (Chara.SakuyaB, "十六夜 咲夜 (B)"),
        }.ToStringKeyedDictionary();

        public static IReadOnlyDictionary<string, string> CharacterWithTotalNames { get; } = new[]
        {
            (CharaWithTotal.ReimuA,  "博麗 霊夢 (A)"),
            (CharaWithTotal.ReimuB,  "博麗 霊夢 (B)"),
            (CharaWithTotal.MarisaA, "霧雨 魔理沙 (A)"),
            (CharaWithTotal.MarisaB, "霧雨 魔理沙 (B)"),
            (CharaWithTotal.SakuyaA, "十六夜 咲夜 (A)"),
            (CharaWithTotal.SakuyaB, "十六夜 咲夜 (B)"),
            (CharaWithTotal.Total,   "全主人公合計"),
        }.ToStringKeyedDictionary();

        public static IEnumerable<string> CharacterKeysTotalFirst { get; } = CharacterWithTotalNames.Keys.RotateRight();

        public static IEnumerable<string> CharacterKeysTotalLast { get; } = CharacterWithTotalNames.Keys;

        public static IReadOnlyDictionary<string, int> NumCardsPerLevel { get; } = new[]
        {
            (Level.Easy,    26),
            (Level.Normal,  26),
            (Level.Hard,    28),
            (Level.Lunatic, 28),
            (Level.Extra,   12),
        }.ToStringKeyedDictionary();

        public static IReadOnlyDictionary<string, int> NumCardsPerStage { get; } = new[]
        {
            (Stage.One,   10),
            (Stage.Two,   16),
            (Stage.Three, 14),
            (Stage.Four,  24),
            (Stage.Five,  20),
            (Stage.Six,   24),
            (Stage.Extra, 12),
        }.ToStringKeyedDictionary();

        public static IReadOnlyDictionary<string, string> CareerKinds { get; } = new[]
        {
            ("S", "ゲーム本編"),
            ("P", "スペルプラクティス"),
        }.ToDictionary();
    }
}
