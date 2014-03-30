//-----------------------------------------------------------------------
// <copyright file="Th07Converter.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed.")]

namespace ThScoreFileConverter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using CardInfo = SpellCardInfo<Th07Converter.Stage, Th07Converter.Level>;

    internal class Th07Converter : ThConverter
    {
        private static readonly Dictionary<int, CardInfo> CardTable;
        private static readonly List<HighScore> InitialRanking;

        private static readonly string LevelPattern;
        private static readonly string LevelWithTotalPattern;
        private static readonly string CharaPattern;
        private static readonly string CharaWithTotalPattern;
        private static readonly string StagePattern;
        private static readonly string StageWithTotalPattern;

        private static readonly Func<string, Level> ToLevel;
        private static readonly Func<string, LevelWithTotal> ToLevelWithTotal;
        private static readonly Func<string, Chara> ToChara;
        private static readonly Func<string, CharaWithTotal> ToCharaWithTotal;
        private static readonly Func<string, Stage> ToStage;
        private static readonly Func<string, StageWithTotal> ToStageWithTotal;

        private AllScoreData allScoreData = null;

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        static Th07Converter()
        {
            // Thanks to thwiki.info and www57.atwiki.jp/2touhoukouryaku
            var cardList = new List<CardInfo>()
            {
                new CardInfo(  1, "霜符「フロストコラムス」",                   Stage.St1,      Level.Hard),
                new CardInfo(  2, "霜符「フロストコラムス -Lunatic-」",         Stage.St1,      Level.Lunatic),
                new CardInfo(  3, "寒符「リンガリングコールド -Easy-」",        Stage.St1,      Level.Easy),
                new CardInfo(  4, "寒符「リンガリングコールド」",               Stage.St1,      Level.Normal),
                new CardInfo(  5, "寒符「リンガリングコールド -Hard-」",        Stage.St1,      Level.Hard),
                new CardInfo(  6, "寒符「リンガリングコールド -Lunatic-」",     Stage.St1,      Level.Lunatic),
                new CardInfo(  7, "冬符「フラワーウィザラウェイ -Easy-」",      Stage.St1,      Level.Easy),
                new CardInfo(  8, "冬符「フラワーウィザラウェイ」",             Stage.St1,      Level.Normal),
                new CardInfo(  9, "白符「アンデュレイションレイ」",             Stage.St1,      Level.Hard),
                new CardInfo( 10, "怪符「テーブルターニング」",                 Stage.St1,      Level.Lunatic),
                new CardInfo( 11, "仙符「鳳凰卵 -Easy-」",                      Stage.St2,      Level.Easy),
                new CardInfo( 12, "仙符「鳳凰卵」",                             Stage.St2,      Level.Normal),
                new CardInfo( 13, "仙符「鳳凰展翅」",                           Stage.St2,      Level.Hard),
                new CardInfo( 14, "仙符「鳳凰展翅 -Lunatic-」",                 Stage.St2,      Level.Lunatic),
                new CardInfo( 15, "式符「飛翔晴明 -Easy-」",                    Stage.St2,      Level.Easy),
                new CardInfo( 16, "式符「飛翔晴明」",                           Stage.St2,      Level.Normal),
                new CardInfo( 17, "陰陽「道満晴明」",                           Stage.St2,      Level.Hard),
                new CardInfo( 18, "陰陽「晴明大紋」",                           Stage.St2,      Level.Lunatic),
                new CardInfo( 19, "天符「天仙鳴動 -Easy-」",                    Stage.St2,      Level.Easy),
                new CardInfo( 20, "天符「天仙鳴動」",                           Stage.St2,      Level.Normal),
                new CardInfo( 21, "翔符「飛翔韋駄天」",                         Stage.St2,      Level.Hard),
                new CardInfo( 22, "童符「護法天童乱舞」",                       Stage.St2,      Level.Lunatic),
                new CardInfo( 23, "仙符「屍解永遠 -Easy-」",                    Stage.St2,      Level.Easy),
                new CardInfo( 24, "仙符「屍解永遠」",                           Stage.St2,      Level.Normal),
                new CardInfo( 25, "鬼符「鬼門金神」",                           Stage.St2,      Level.Hard),
                new CardInfo( 26, "方符「奇門遁甲」",                           Stage.St2,      Level.Lunatic),
                new CardInfo( 27, "操符「乙女文楽」",                           Stage.St3,      Level.Hard),
                new CardInfo( 28, "操符「乙女文楽 -Lunatic-」",                 Stage.St3,      Level.Lunatic),
                new CardInfo( 29, "蒼符「博愛の仏蘭西人形 -Easy-」",            Stage.St3,      Level.Easy),
                new CardInfo( 30, "蒼符「博愛の仏蘭西人形」",                   Stage.St3,      Level.Normal),
                new CardInfo( 31, "蒼符「博愛の仏蘭西人形 -Hard-」",            Stage.St3,      Level.Hard),
                new CardInfo( 32, "蒼符「博愛のオルレアン人形」",               Stage.St3,      Level.Lunatic),
                new CardInfo( 33, "紅符「紅毛の和蘭人形 -Easy-」",              Stage.St3,      Level.Easy),
                new CardInfo( 34, "紅符「紅毛の和蘭人形」",                     Stage.St3,      Level.Normal),
                new CardInfo( 35, "白符「白亜の露西亜人形」",                   Stage.St3,      Level.Hard),
                new CardInfo( 36, "白符「白亜の露西亜人形 -Lunatic-」",         Stage.St3,      Level.Lunatic),
                new CardInfo( 37, "闇符「霧の倫敦人形 -Easy-」",                Stage.St3,      Level.Easy),
                new CardInfo( 38, "闇符「霧の倫敦人形」",                       Stage.St3,      Level.Normal),
                new CardInfo( 39, "廻符「輪廻の西蔵人形」",                     Stage.St3,      Level.Hard),
                new CardInfo( 40, "雅符「春の京人形」",                         Stage.St3,      Level.Lunatic),
                new CardInfo( 41, "咒詛「魔彩光の上海人形 -Easy-」",            Stage.St3,      Level.Easy),
                new CardInfo( 42, "咒詛「魔彩光の上海人形」",                   Stage.St3,      Level.Normal),
                new CardInfo( 43, "咒詛「魔彩光の上海人形 -Hard-」",            Stage.St3,      Level.Hard),
                new CardInfo( 44, "咒詛「首吊り蓬莱人形」",                     Stage.St3,      Level.Lunatic),
                new CardInfo( 45, "騒符「ファントムディニング -Easy-」",        Stage.St4,      Level.Easy),
                new CardInfo( 46, "騒符「ファントムディニング」",               Stage.St4,      Level.Normal),
                new CardInfo( 47, "騒符「ライブポルターガイスト」",             Stage.St4,      Level.Hard),
                new CardInfo( 48, "騒符「ライブポルターガイスト -Lunatic-」",   Stage.St4,      Level.Lunatic),
                new CardInfo( 49, "弦奏「グァルネリ・デル・ジェス -Easy-」",    Stage.St4,      Level.Easy),
                new CardInfo( 50, "弦奏「グァルネリ・デル・ジェス」",           Stage.St4,      Level.Normal),
                new CardInfo( 51, "神弦「ストラディヴァリウス」",               Stage.St4,      Level.Hard),
                new CardInfo( 52, "偽弦「スードストラディヴァリウス」",         Stage.St4,      Level.Lunatic),
                new CardInfo( 53, "管霊「ヒノファンタズム -Easy-」",            Stage.St4,      Level.Easy),
                new CardInfo( 54, "管霊「ヒノファンタズム」",                   Stage.St4,      Level.Normal),
                new CardInfo( 55, "冥管「ゴーストクリフォード」",               Stage.St4,      Level.Hard),
                new CardInfo( 56, "管霊「ゴーストクリフォード -Lunatic-」",     Stage.St4,      Level.Lunatic),
                new CardInfo( 57, "冥鍵「ファツィオーリ冥奏 -Easy-」",          Stage.St4,      Level.Easy),
                new CardInfo( 58, "冥鍵「ファツィオーリ冥奏」",                 Stage.St4,      Level.Normal),
                new CardInfo( 59, "鍵霊「ベーゼンドルファー神奏」",             Stage.St4,      Level.Hard),
                new CardInfo( 60, "鍵霊「ベーゼンドルファー神奏 -Lunatic-」",   Stage.St4,      Level.Lunatic),
                new CardInfo( 61, "合葬「プリズムコンチェルト -Easy-」",        Stage.St4,      Level.Easy),
                new CardInfo( 62, "合葬「プリズムコンチェルト」",               Stage.St4,      Level.Normal),
                new CardInfo( 63, "騒葬「スティジャンリバーサイド」",           Stage.St4,      Level.Hard),
                new CardInfo( 64, "騒葬「スティジャンリバーサイド -Lunatic-」", Stage.St4,      Level.Lunatic),
                new CardInfo( 65, "大合葬「霊車コンチェルトグロッソ -Easy-」",  Stage.St4,      Level.Easy),
                new CardInfo( 66, "大合葬「霊車コンチェルトグロッソ」",         Stage.St4,      Level.Normal),
                new CardInfo( 67, "大合葬「霊車コンチェルトグロッソ改」",       Stage.St4,      Level.Hard),
                new CardInfo( 68, "大合葬「霊車コンチェルトグロッソ怪」",       Stage.St4,      Level.Lunatic),
                new CardInfo( 69, "幽鬼剣「妖童餓鬼の断食 -Easy-」",            Stage.St5,      Level.Easy),
                new CardInfo( 70, "幽鬼剣「妖童餓鬼の断食」",                   Stage.St5,      Level.Normal),
                new CardInfo( 71, "餓鬼剣「餓鬼道草紙」",                       Stage.St5,      Level.Hard),
                new CardInfo( 72, "餓王剣「餓鬼十王の報い」",                   Stage.St5,      Level.Lunatic),
                new CardInfo( 73, "獄界剣「二百由旬の一閃 -Easy-」",            Stage.St5,      Level.Easy),
                new CardInfo( 74, "獄界剣「二百由旬の一閃」",                   Stage.St5,      Level.Normal),
                new CardInfo( 75, "獄炎剣「業風閃影陣」",                       Stage.St5,      Level.Hard),
                new CardInfo( 76, "獄神剣「業風神閃斬」",                       Stage.St5,      Level.Lunatic),
                new CardInfo( 77, "畜趣剣「無為無策の冥罰 -Easy-」",            Stage.St5,      Level.Easy),
                new CardInfo( 78, "畜趣剣「無為無策の冥罰」",                   Stage.St5,      Level.Normal),
                new CardInfo( 79, "修羅剣「現世妄執」",                         Stage.St5,      Level.Hard),
                new CardInfo( 80, "修羅剣「現世妄執 -Lunatic-」",               Stage.St5,      Level.Lunatic),
                new CardInfo( 81, "人界剣「悟入幻想 -Easy-」",                  Stage.St5,      Level.Easy),
                new CardInfo( 82, "人界剣「悟入幻想」",                         Stage.St5,      Level.Normal),
                new CardInfo( 83, "人世剣「大悟顕晦」",                         Stage.St5,      Level.Hard),
                new CardInfo( 84, "人神剣「俗諦常住」",                         Stage.St5,      Level.Lunatic),
                new CardInfo( 85, "天上剣「天人の五衰 -Easy-」",                Stage.St5,      Level.Easy),
                new CardInfo( 86, "天上剣「天人の五衰」",                       Stage.St5,      Level.Normal),
                new CardInfo( 87, "天界剣「七魄忌諱」",                         Stage.St5,      Level.Hard),
                new CardInfo( 88, "天神剣「三魂七魄」",                         Stage.St5,      Level.Lunatic),
                new CardInfo( 89, "六道剣「一念無量劫 -Easy-」",                Stage.St6,      Level.Easy),
                new CardInfo( 90, "六道剣「一念無量劫」",                       Stage.St6,      Level.Normal),
                new CardInfo( 91, "六道剣「一念無量劫 -Hard-」",                Stage.St6,      Level.Hard),
                new CardInfo( 92, "六道剣「一念無量劫 -Lunatic-」",             Stage.St6,      Level.Lunatic),
                new CardInfo( 93, "亡郷「亡我郷 -さまよえる魂-」",              Stage.St6,      Level.Easy),
                new CardInfo( 94, "亡郷「亡我郷 -宿罪-」",                      Stage.St6,      Level.Normal),
                new CardInfo( 95, "亡郷「亡我郷 -道無き道-」",                  Stage.St6,      Level.Hard),
                new CardInfo( 96, "亡郷「亡我郷 -自尽-」",                      Stage.St6,      Level.Lunatic),
                new CardInfo( 97, "亡舞「生者必滅の理 -眩惑-」",                Stage.St6,      Level.Easy),
                new CardInfo( 98, "亡舞「生者必滅の理 -死蝶-」",                Stage.St6,      Level.Normal),
                new CardInfo( 99, "亡舞「生者必滅の理 -毒蛾-」",                Stage.St6,      Level.Hard),
                new CardInfo(100, "亡舞「生者必滅の理 -魔境-」",                Stage.St6,      Level.Lunatic),
                new CardInfo(101, "華霊「ゴーストバタフライ」",                 Stage.St6,      Level.Easy),
                new CardInfo(102, "華霊「スワローテイルバタフライ」",           Stage.St6,      Level.Normal),
                new CardInfo(103, "華霊「ディープルーティドバタフライ」",       Stage.St6,      Level.Hard),
                new CardInfo(104, "華霊「バタフライディルージョン」",           Stage.St6,      Level.Lunatic),
                new CardInfo(105, "幽曲「リポジトリ・オブ・ヒロカワ -偽霊-」",  Stage.St6,      Level.Easy),
                new CardInfo(106, "幽曲「リポジトリ・オブ・ヒロカワ -亡霊-」",  Stage.St6,      Level.Normal),
                new CardInfo(107, "幽曲「リポジトリ・オブ・ヒロカワ -幻霊-」",  Stage.St6,      Level.Hard),
                new CardInfo(108, "幽曲「リポジトリ・オブ・ヒロカワ -神霊-」",  Stage.St6,      Level.Lunatic),
                new CardInfo(109, "桜符「完全なる墨染の桜 -封印-」",            Stage.St6,      Level.Easy),
                new CardInfo(110, "桜符「完全なる墨染の桜 -亡我-」",            Stage.St6,      Level.Normal),
                new CardInfo(111, "桜符「完全なる墨染の桜 -春眠-」",            Stage.St6,      Level.Hard),
                new CardInfo(112, "桜符「完全なる墨染の桜 -開花-」",            Stage.St6,      Level.Lunatic),
                new CardInfo(113, "「反魂蝶 -一分咲-」",                        Stage.St6,      Level.Easy),
                new CardInfo(114, "「反魂蝶 -参分咲-」",                        Stage.St6,      Level.Normal),
                new CardInfo(115, "「反魂蝶 -伍分咲-」",                        Stage.St6,      Level.Hard),
                new CardInfo(116, "「反魂蝶 -八分咲-」",                        Stage.St6,      Level.Lunatic),
                new CardInfo(117, "鬼符「青鬼赤鬼」",                           Stage.Extra,    Level.Extra),
                new CardInfo(118, "鬼神「飛翔毘沙門天」",                       Stage.Extra,    Level.Extra),
                new CardInfo(119, "式神「仙狐思念」",                           Stage.Extra,    Level.Extra),
                new CardInfo(120, "式神「十二神将の宴」",                       Stage.Extra,    Level.Extra),
                new CardInfo(121, "式輝「狐狸妖怪レーザー」",                   Stage.Extra,    Level.Extra),
                new CardInfo(122, "式輝「四面楚歌チャーミング」",               Stage.Extra,    Level.Extra),
                new CardInfo(123, "式輝「プリンセス天狐 -Illusion-」",          Stage.Extra,    Level.Extra),
                new CardInfo(124, "式弾「アルティメットブディスト」",           Stage.Extra,    Level.Extra),
                new CardInfo(125, "式弾「ユーニラタルコンタクト」",             Stage.Extra,    Level.Extra),
                new CardInfo(126, "式神「橙」",                                 Stage.Extra,    Level.Extra),
                new CardInfo(127, "「狐狗狸さんの契約」",                       Stage.Extra,    Level.Extra),
                new CardInfo(128, "幻神「飯綱権現降臨」",                       Stage.Extra,    Level.Extra),
                new CardInfo(129, "式神「前鬼後鬼の守護」",                     Stage.Phantasm, Level.Phantasm),
                new CardInfo(130, "式神「憑依荼吉尼天」",                       Stage.Phantasm, Level.Phantasm),
                new CardInfo(131, "結界「夢と現の呪」",                         Stage.Phantasm, Level.Phantasm),
                new CardInfo(132, "結界「動と静の均衡」",                       Stage.Phantasm, Level.Phantasm),
                new CardInfo(133, "結界「光と闇の網目」",                       Stage.Phantasm, Level.Phantasm),
                new CardInfo(134, "罔両「ストレートとカーブの夢郷」",           Stage.Phantasm, Level.Phantasm),
                new CardInfo(135, "罔両「八雲紫の神隠し」",                     Stage.Phantasm, Level.Phantasm),
                new CardInfo(136, "罔両「禅寺に棲む妖蝶」",                     Stage.Phantasm, Level.Phantasm),
                new CardInfo(137, "魍魎「二重黒死蝶」",                         Stage.Phantasm, Level.Phantasm),
                new CardInfo(138, "式神「八雲藍」",                             Stage.Phantasm, Level.Phantasm),
                new CardInfo(139, "「人間と妖怪の境界」",                       Stage.Phantasm, Level.Phantasm),
                new CardInfo(140, "結界「生と死の境界」",                       Stage.Phantasm, Level.Phantasm),
                new CardInfo(141, "紫奥義「弾幕結界」",                         Stage.Phantasm, Level.Phantasm)
            };
            CardTable = cardList.ToDictionary(card => card.Number);

            InitialRanking = new List<HighScore>()
            {
                new HighScore(100000),
                new HighScore( 90000),
                new HighScore( 80000),
                new HighScore( 70000),
                new HighScore( 60000),
                new HighScore( 50000),
                new HighScore( 40000),
                new HighScore( 30000),
                new HighScore( 20000),
                new HighScore( 10000)
            };

            var levels = Utils.GetEnumerator<Level>();
            var levelsWithTotal = Utils.GetEnumerator<LevelWithTotal>();
            var charas = Utils.GetEnumerator<Chara>();
            var charasWithTotal = Utils.GetEnumerator<CharaWithTotal>();
            var stages = Utils.GetEnumerator<Stage>();
            var stagesWithTotal = Utils.GetEnumerator<StageWithTotal>();

            LevelPattern = string.Join(
                string.Empty, levels.Select(lv => lv.ToShortName()).ToArray());
            LevelWithTotalPattern = string.Join(
                string.Empty, levelsWithTotal.Select(lv => lv.ToShortName()).ToArray());
            CharaPattern = string.Join(
                "|", charas.Select(ch => ch.ToShortName()).ToArray());
            CharaWithTotalPattern = string.Join(
                "|", charasWithTotal.Select(ch => ch.ToShortName()).ToArray());
            StagePattern = string.Join(
                string.Empty, stages.Select(st => st.ToShortName()).ToArray());
            StageWithTotalPattern = string.Join(
                string.Empty, stagesWithTotal.Select(st => st.ToShortName()).ToArray());

            var comparisonType = StringComparison.OrdinalIgnoreCase;

            ToLevel = (shortName =>
                levels.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToLevelWithTotal = (shortName =>
                levelsWithTotal.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToChara = (shortName =>
                charas.First(ch => ch.ToShortName().Equals(shortName, comparisonType)));
            ToCharaWithTotal = (shortName =>
                charasWithTotal.First(ch => ch.ToShortName().Equals(shortName, comparisonType)));
            ToStage = (shortName =>
                stages.First(st => st.ToShortName().Equals(shortName, comparisonType)));
            ToStageWithTotal = (shortName =>
                stagesWithTotal.First(st => st.ToShortName().Equals(shortName, comparisonType)));
        }

        public Th07Converter()
        {
        }

        public enum Level
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("P")] Phantasm
        }

        public enum LevelWithTotal
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("P")] Phantasm,
            [EnumAltName("T")] Total
        }

        public enum Chara
        {
            [EnumAltName("RA")] ReimuA,
            [EnumAltName("RB")] ReimuB,
            [EnumAltName("MA")] MarisaA,
            [EnumAltName("MB")] MarisaB,
            [EnumAltName("SA")] SakuyaA,
            [EnumAltName("SB")] SakuyaB
        }

        public enum CharaWithTotal
        {
            [EnumAltName("RA")] ReimuA,
            [EnumAltName("RB")] ReimuB,
            [EnumAltName("MA")] MarisaA,
            [EnumAltName("MB")] MarisaB,
            [EnumAltName("SA")] SakuyaA,
            [EnumAltName("SB")] SakuyaB,
            [EnumAltName("TL")] Total
        }

        public enum Stage
        {
            [EnumAltName("1")] St1,
            [EnumAltName("2")] St2,
            [EnumAltName("3")] St3,
            [EnumAltName("4")] St4,
            [EnumAltName("5")] St5,
            [EnumAltName("6")] St6,
            [EnumAltName("X")] Extra,
            [EnumAltName("P")] Phantasm
        }

        public enum StageWithTotal
        {
            [EnumAltName("1")] St1,
            [EnumAltName("2")] St2,
            [EnumAltName("3")] St3,
            [EnumAltName("4")] St4,
            [EnumAltName("5")] St5,
            [EnumAltName("6")] St6,
            [EnumAltName("X")] Extra,
            [EnumAltName("P")] Phantasm,
            [EnumAltName("0")] Total
        }

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        public enum StageProgress
        {
            [EnumAltName("-------")]        None,
            [EnumAltName("Stage 1")]        St1,
            [EnumAltName("Stage 2")]        St2,
            [EnumAltName("Stage 3")]        St3,
            [EnumAltName("Stage 4")]        St4,
            [EnumAltName("Stage 5")]        St5,
            [EnumAltName("Stage 6")]        St6,
            [EnumAltName("Extra Stage")]    Extra,
            [EnumAltName("Phantasm Stage")] Phantasm,
            [EnumAltName("All Clear")]      Clear = 99
        }

        public override string SupportedVersions
        {
            get { return "1.00b"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th07decoded.dat", FileMode.Create, FileAccess.ReadWrite))
#else
            using (var decoded = new MemoryStream())
#endif
            {
                if (!Decrypt(input, decrypted))
                    return false;

                decrypted.Seek(0, SeekOrigin.Begin);
                if (!Extract(decrypted, decoded))
                    return false;

                decoded.Seek(0, SeekOrigin.Begin);
                if (!Validate(decoded))
                    return false;

                decoded.Seek(0, SeekOrigin.Begin);
                this.allScoreData = Read(decoded);

                return this.allScoreData != null;
            }
        }

        protected override void Convert(Stream input, Stream output, bool hideUntriedCards)
        {
            var reader = new StreamReader(input, Encoding.GetEncoding("shift_jis"));
            var writer = new StreamWriter(output, Encoding.GetEncoding("shift_jis"));

            var allLine = reader.ReadToEnd();
            allLine = this.ReplaceScore(allLine);
            allLine = this.ReplaceCareer(allLine);
            allLine = this.ReplaceCard(allLine, hideUntriedCards);
            allLine = this.ReplaceCollectRate(allLine);
            allLine = this.ReplaceClear(allLine);
            allLine = this.ReplacePlay(allLine);
            allLine = this.ReplaceTime(allLine);
            allLine = this.ReplacePractice(allLine);

            writer.Write(allLine);
            writer.Flush();
            writer.BaseStream.SetLength(writer.BaseStream.Position);
        }

        private static bool Decrypt(Stream input, Stream output)
        {
            var size = (int)input.Length;
            var data = new byte[size];
            input.Read(data, 0, size);

            uint checksum = 0;
            byte temp = 0;
            for (var index = 2; index < size; index++)
            {
                temp += data[index - 1];
                temp = (byte)((temp >> 5) | (temp << 3));
                data[index] ^= temp;
                if (index > 3)
                    checksum += data[index];
            }

            output.Write(data, 0, size);

            return (ushort)checksum == BitConverter.ToUInt16(data, 2);
        }

        private static bool Extract(Stream input, Stream output)
        {
            var reader = new BinaryReader(input);
            var writer = new BinaryWriter(output);
            var header = new FileHeader();

            header.ReadFrom(reader);
            if (!header.IsValid)
                return false;
            if (header.Size + header.EncodedBodySize != input.Length)
                return false;

            header.WriteTo(writer);

            Lzss.Extract(input, output);
            output.Flush();
            output.SetLength(output.Position);

            return output.Position == header.DecodedAllSize;
        }

        private static bool Validate(Stream input)
        {
            var reader = new BinaryReader(input);
            var header = new FileHeader();
            var chapter = new Chapter();

            header.ReadFrom(reader);
            var remainSize = header.DecodedAllSize - header.Size;
            if (remainSize <= 0)
                return false;

            try
            {
                while (remainSize > 0)
                {
                    chapter.ReadFrom(reader);
                    if (chapter.Size1 == 0)
                        return false;

                    byte temp = 0;
                    switch (chapter.Signature)
                    {
                        case "TH7K":
                            temp = reader.ReadByte();
                            //// 8 means the total size of Signature, Size1, and Size2.
                            reader.ReadBytes(chapter.Size1 - 8 - 1);
                            if (temp != 0x01)
                                return false;
                            break;
                        case "VRSM":
                            temp = reader.ReadByte();
                            reader.ReadBytes(chapter.Size1 - 8 - 1);    // 8 means the same above
                            if (temp != 0x01)
                                return false;
                            //// th07.exe does something more things here...
                            break;
                        default:
                            reader.ReadBytes(chapter.Size1 - 8);        // 8 means the same above
                            break;
                    }

                    remainSize -= chapter.Size1;
                }
            }
            catch (EndOfStreamException)
            {
                // It's OK, do nothing.
            }

            return remainSize == 0;
        }

        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1513:ClosingCurlyBracketMustBeFollowedByBlankLine", Justification = "Reviewed.")]
        private static AllScoreData Read(Stream input)
        {
            var reader = new BinaryReader(input);
            var allScoreData = new AllScoreData();
            var chapter = new Chapter();

            reader.ReadBytes(FileHeader.ValidSize);

            try
            {
                while (true)
                {
                    chapter.ReadFrom(reader);
                    switch (chapter.Signature)
                    {
                        case "TH7K":
                            {
                                var header = new Header(chapter);
                                header.ReadFrom(reader);
                                allScoreData.Header = header;
                            }
                            break;

                        case "HSCR":
                            {
                                var score = new HighScore(chapter);
                                score.ReadFrom(reader);
                                var key = new CharaLevelPair(score.Chara, score.Level);
                                if (!allScoreData.Rankings.ContainsKey(key))
                                    allScoreData.Rankings.Add(key, new List<HighScore>(InitialRanking));
                                var ranking = allScoreData.Rankings[key];
                                ranking.Add(score);
                                ranking.Sort((lhs, rhs) => rhs.Score.CompareTo(lhs.Score));
                                ranking.RemoveAt(ranking.Count - 1);
                            }
                            break;

                        case "CLRD":
                            {
                                var clearData = new ClearData(chapter);
                                clearData.ReadFrom(reader);
                                if (!allScoreData.ClearData.ContainsKey(clearData.Chara))
                                    allScoreData.ClearData.Add(clearData.Chara, clearData);
                            }
                            break;

                        case "CATK":
                            {
                                var attack = new CardAttack(chapter);
                                attack.ReadFrom(reader);
                                if (!allScoreData.CardAttacks.ContainsKey(attack.Number))
                                    allScoreData.CardAttacks.Add(attack.Number, attack);
                            }
                            break;

                        case "PSCR":
                            {
                                var score = new PracticeScore(chapter);
                                score.ReadFrom(reader);
                                if ((score.Level != Level.Extra) && (score.Level != Level.Phantasm) &&
                                    (score.Stage != Stage.Extra) && (score.Stage != Stage.Phantasm))
                                {
                                    var key = new CharaLevelPair(score.Chara, score.Level);
                                    if (!allScoreData.PracticeScores.ContainsKey(key))
                                    {
                                        var numStages = Utils.GetEnumerator<Stage>()
                                            .Where(st => (st != Stage.Extra) && (st != Stage.Phantasm))
                                            .Count();
                                        allScoreData.PracticeScores.Add(
                                            key, new Dictionary<Stage, PracticeScore>(numStages));
                                    }

                                    var scores = allScoreData.PracticeScores[key];
                                    if (!scores.ContainsKey(score.Stage))
                                        scores.Add(score.Stage, score);
                                }
                            }
                            break;

                        case "PLST":
                            {
                                var status = new PlayStatus(chapter);
                                status.ReadFrom(reader);
                                allScoreData.PlayStatus = status;
                            }
                            break;

                        case "LSNM":
                            {
                                var lastName = new LastName(chapter);
                                lastName.ReadFrom(reader);
                                allScoreData.LastName = lastName;
                            }
                            break;

                        case "VRSM":
                            {
                                var versionInfo = new VersionInfo(chapter);
                                versionInfo.ReadFrom(reader);
                                allScoreData.VersionInfo = versionInfo;
                            }
                            break;

                        default:
                            // 8 means the total size of Signature, Size1, and Size2.
                            reader.ReadBytes(chapter.Size1 - 8);
                            break;
                    }
                }
            }
            catch (EndOfStreamException)
            {
                // It's OK, do nothing.
            }

            if ((allScoreData.Header != null) &&
                //// (allScoreData.rankings.Count >= 0) &&
                (allScoreData.ClearData.Count == Enum.GetValues(typeof(Chara)).Length) &&
                //// (allScoreData.cardAttacks.Length == NumCards) &&
                //// (allScoreData.practiceScores.Count >= 0) &&
                (allScoreData.PlayStatus != null) &&
                (allScoreData.LastName != null) &&
                (allScoreData.VersionInfo != null))
                return allScoreData;
            else
                return null;
        }

        // %T07SCR[w][xx][y][z]
        private string ReplaceScore(string input)
        {
            var pattern = Utils.Format(@"%T07SCR([{0}])({1})(\d)([1-5])", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value);
                var chara = ToChara(match.Groups[2].Value);
                var rank = Utils.ToZeroBased(int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                var key = new CharaLevelPair(chara, level);
                var score = this.allScoreData.Rankings.ContainsKey(key)
                    ? this.allScoreData.Rankings[key][rank] : InitialRanking[rank];

                switch (type)
                {
                    case 1:     // name
                        return Encoding.Default.GetString(score.Name).Split('\0')[0];
                    case 2:     // score
                        return this.ToNumberString((score.Score * 10) + score.ContinueCount);
                    case 3:     // stage
                        return score.StageProgress.ToShortName();
                    case 4:     // date
                        return Encoding.Default.GetString(score.Date).TrimEnd('\0');
                    case 5:     // slow rate
                        return Utils.Format("{0:F3}%", score.SlowRate);
                    default:    // unreachable
                        return match.ToString();
                }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T07C[xxx][yy][z]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
        private string ReplaceCareer(string input)
        {
            var pattern = Utils.Format(@"%T07C(\d{{3}})({0})([1-3])", CharaWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var chara = ToCharaWithTotal(match.Groups[2].Value);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                Func<CardAttack, long> getValue = (attack => 0L);
                if (type == 1)
                    getValue = (attack => attack.MaxBonuses[chara]);
                else if (type == 2)
                    getValue = (attack => attack.ClearCounts[chara]);
                else
                    getValue = (attack => attack.TrialCounts[chara]);

                if (number == 0)
                    return this.ToNumberString(this.allScoreData.CardAttacks.Values.Sum(getValue));
                else if (CardTable.ContainsKey(number))
                {
                    CardAttack attack;
                    if (this.allScoreData.CardAttacks.TryGetValue(number, out attack))
                        return this.ToNumberString(getValue(attack));
                    else
                        return "0";
                }
                else
                    return match.ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T07CARD[xxx][y]
        private string ReplaceCard(string input, bool hideUntriedCards)
        {
            var pattern = @"%T07CARD(\d{3})([NR])";
            var evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var type = match.Groups[2].Value.ToUpperInvariant();

                if (CardTable.ContainsKey(number))
                {
                    if (hideUntriedCards)
                    {
                        CardAttack attack;
                        if (!this.allScoreData.CardAttacks.TryGetValue(number, out attack) ||
                            !attack.HasTried())
                            return (type == "N") ? "??????????" : "?????";
                    }

                    return (type == "N") ? CardTable[number].Name : CardTable[number].Level.ToString();
                }
                else
                    return match.ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T07CRG[w][xx][yy][z]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
        private string ReplaceCollectRate(string input)
        {
            var pattern = Utils.Format(
                @"%T07CRG([{0}])({1})([{2}])([12])",
                LevelWithTotalPattern,
                CharaWithTotalPattern,
                StageWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevelWithTotal(match.Groups[1].Value);
                var chara = ToCharaWithTotal(match.Groups[2].Value);
                var stage = ToStageWithTotal(match.Groups[3].Value);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                if ((stage == StageWithTotal.Extra) || (stage == StageWithTotal.Phantasm))
                    return match.ToString();

                Func<CardAttack, bool> findByLevel = (attack => true);
                Func<CardAttack, bool> findByStage = (attack => true);
                Func<CardAttack, bool> findByType = (attack => true);

                if (stage == StageWithTotal.Total)
                {
                    // Do nothing
                }
                else
                    findByStage = (attack => CardTable[attack.Number].Stage == (Stage)stage);

                switch (level)
                {
                    case LevelWithTotal.Total:
                        // Do nothing
                        break;
                    case LevelWithTotal.Extra:
                        findByStage = (attack => CardTable[attack.Number].Stage == Stage.Extra);
                        break;
                    case LevelWithTotal.Phantasm:
                        findByStage = (attack => CardTable[attack.Number].Stage == Stage.Phantasm);
                        break;
                    default:
                        findByLevel = (attack => CardTable[attack.Number].Level == (Level)level);
                        break;
                }

                if (type == 1)
                    findByType = (attack => attack.ClearCounts[chara] > 0);
                else
                    findByType = (attack => attack.TrialCounts[chara] > 0);

                var and = Utils.MakeAndPredicate(findByLevel, findByStage, findByType);
                return this.allScoreData.CardAttacks.Values.Count(and).ToString(CultureInfo.CurrentCulture);
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T07CLEAR[x][yy]
        private string ReplaceClear(string input)
        {
            var pattern = Utils.Format(@"%T07CLEAR([{0}])({1})", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value);
                var chara = ToChara(match.Groups[2].Value);

                var key = new CharaLevelPair(chara, level);
                if (this.allScoreData.Rankings.ContainsKey(key))
                {
                    var stageProgress = this.allScoreData.Rankings[key].Max(rank => rank.StageProgress);
                    if ((stageProgress == StageProgress.Extra) || (stageProgress == StageProgress.Phantasm))
                        return "Not Clear";
                    else
                        return stageProgress.ToShortName();
                }
                else
                    return StageProgress.None.ToShortName();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T07PLAY[x][yy]
        private string ReplacePlay(string input)
        {
            var pattern = Utils.Format(
                @"%T07PLAY([{0}])({1}|CL|CN|PR|RT)", LevelWithTotalPattern, CharaWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevelWithTotal(match.Groups[1].Value);
                var charaAndMore = match.Groups[2].Value.ToUpperInvariant();

                var playCount = this.allScoreData.PlayStatus.PlayCounts[(LevelWithTotal)level];
                switch (charaAndMore)
                {
                    case "CL":  // clear count
                        return this.ToNumberString(playCount.TotalClear);
                    case "CN":  // continue count
                        return this.ToNumberString(playCount.TotalContinue);
                    case "PR":  // practice count
                        return this.ToNumberString(playCount.TotalPractice);
                    case "RT":  // retry count
                        return this.ToNumberString(playCount.TotalRetry);
                    default:
                        {
                            var chara = ToCharaWithTotal(match.Groups[2].Value);
                            return this.ToNumberString((chara == CharaWithTotal.Total)
                                ? playCount.TotalTrial : playCount.Trials[(Chara)chara]);
                        }
                }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T07TIME(ALL|PLY)
        private string ReplaceTime(string input)
        {
            var pattern = @"%T07TIME(ALL|PLY)";
            var evaluator = new MatchEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();

                return (kind == "ALL")
                    ? this.allScoreData.PlayStatus.TotalRunningTime.ToLongString()
                    : this.allScoreData.PlayStatus.TotalPlayTime.ToLongString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T07PRAC[w][xx][y][z]
        private string ReplacePractice(string input)
        {
            var pattern = Utils.Format(
                @"%T07PRAC([{0}])({1})([{2}])([12])", LevelPattern, CharaPattern, StagePattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value);
                var chara = ToChara(match.Groups[2].Value);
                var stage = ToStage(match.Groups[3].Value);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                if ((level == Level.Extra) || (level == Level.Phantasm))
                    return match.ToString();
                if ((stage == Stage.Extra) || (stage == Stage.Phantasm))
                    return match.ToString();

                var key = new CharaLevelPair(chara, level);
                if (this.allScoreData.PracticeScores.ContainsKey(key))
                {
                    var scores = this.allScoreData.PracticeScores[key];
                    if (type == 1)
                        return scores.ContainsKey(stage)
                            ? this.ToNumberString(scores[stage].HighScore * 10) : "0";
                    else
                        return scores.ContainsKey(stage)
                            ? this.ToNumberString(scores[stage].TrialCount) : "0";
                }
                else
                    return "0";
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        private class CharaLevelPair : Pair<Chara, Level>
        {
            public CharaLevelPair(Chara chara, Level level)
                : base(chara, level)
            {
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Chara Chara
            {
                get { return this.First; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Level Level
            {
                get { return this.Second; }
            }
        }

        private class FileHeader : IBinaryReadable, IBinaryWritable
        {
            public const short ValidVersion = 0x000B;
            public const int ValidSize = 0x0000001C;

            private ushort unknown1;
            private ushort unknown2;
            private uint unknown3;

            public FileHeader()
            {
            }

            public ushort Checksum { get; private set; }

            public short Version { get; private set; }

            public int Size { get; private set; }

            public int DecodedAllSize { get; private set; }

            public int DecodedBodySize { get; private set; }

            public int EncodedBodySize { get; private set; }

            public bool IsValid
            {
                get
                {
                    return (this.Version == ValidVersion)
                        && (this.Size == ValidSize)
                        && (this.DecodedAllSize == this.Size + this.DecodedBodySize);
                }
            }

            public void ReadFrom(BinaryReader reader)
            {
                this.unknown1 = reader.ReadUInt16();
                this.Checksum = reader.ReadUInt16();
                this.Version = reader.ReadInt16();
                this.unknown2 = reader.ReadUInt16();
                this.Size = reader.ReadInt32();
                this.unknown3 = reader.ReadUInt32();
                this.DecodedAllSize = reader.ReadInt32();
                this.DecodedBodySize = reader.ReadInt32();
                this.EncodedBodySize = reader.ReadInt32();
            }

            public void WriteTo(BinaryWriter writer)
            {
                writer.Write(this.unknown1);
                writer.Write(this.Checksum);
                writer.Write(this.Version);
                writer.Write(this.unknown2);
                writer.Write(this.Size);
                writer.Write(this.unknown3);
                writer.Write(this.DecodedAllSize);
                writer.Write(this.DecodedBodySize);
                writer.Write(this.EncodedBodySize);
            }
        }

        private class AllScoreData
        {
            public AllScoreData()
            {
                var numCharas = Enum.GetValues(typeof(Chara)).Length;
                var numPairs = numCharas * Enum.GetValues(typeof(Level)).Length;
                this.Rankings = new Dictionary<CharaLevelPair, List<HighScore>>(numPairs);
                this.ClearData = new Dictionary<Chara, ClearData>(numCharas);
                this.CardAttacks = new Dictionary<int, CardAttack>(CardTable.Count);
                this.PracticeScores =
                    new Dictionary<CharaLevelPair, Dictionary<Stage, PracticeScore>>(numPairs);
            }

            public Header Header { get; set; }

            public Dictionary<CharaLevelPair, List<HighScore>> Rankings { get; set; }

            public Dictionary<Chara, ClearData> ClearData { get; set; }

            public Dictionary<int, CardAttack> CardAttacks { get; set; }

            public Dictionary<CharaLevelPair, Dictionary<Stage, PracticeScore>> PracticeScores { get; set; }

            public PlayStatus PlayStatus { get; set; }

            public LastName LastName { get; set; }

            public VersionInfo VersionInfo { get; set; }
        }

        private class Chapter : IBinaryReadable
        {
            public Chapter()
            {
            }

            public Chapter(Chapter ch)
            {
                this.Signature = ch.Signature;
                this.Size1 = ch.Size1;
                this.Size2 = ch.Size2;
            }

            public string Signature { get; private set; }   // .Length = 4

            public short Size1 { get; private set; }

            public short Size2 { get; private set; }        // always equal to size1?

            public virtual void ReadFrom(BinaryReader reader)
            {
                this.Signature = Encoding.Default.GetString(reader.ReadBytes(4));
                this.Size1 = reader.ReadInt16();
                this.Size2 = reader.ReadInt16();
            }
        }

        private class Header : Chapter
        {
            public Header(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "TH7K")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x000C)
                    throw new InvalidDataException("Size1");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadUInt32();    // always 0x00000001?
            }
        }

        private class HighScore : Chapter   // per character, level, rank
        {
            public HighScore(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "HSCR")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0028)
                    throw new InvalidDataException("Size1");
            }

            public HighScore(uint score)    // for InitialRanking only
                : base()
            {
                this.Score = score;
                this.Name = Encoding.Default.GetBytes("--------\0");
                this.Date = Encoding.Default.GetBytes("--/--\0");
            }

            public uint Score { get; private set; }                     // * 10

            public float SlowRate { get; private set; }

            public Chara Chara { get; private set; }                    // size: 1Byte

            public Level Level { get; private set; }                    // size: 1Byte

            public StageProgress StageProgress { get; private set; }    // size: 1Byte

            public byte[] Name { get; private set; }                    // .Length = 9, null-terminated

            public byte[] Date { get; private set; }                    // .Length = 6, "mm/dd\0"

            public ushort ContinueCount { get; private set; }

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadUInt32();    // always 0x00000001?
                this.Score = reader.ReadUInt32();
                this.SlowRate = reader.ReadSingle();
                this.Chara = (Chara)reader.ReadByte();
                this.Level = (Level)reader.ReadByte();
                this.StageProgress = (StageProgress)reader.ReadByte();
                this.Name = reader.ReadBytes(9);
                this.Date = reader.ReadBytes(6);
                this.ContinueCount = reader.ReadUInt16();
            }
        }

        private class ClearData : Chapter   // per character
        {
            public ClearData(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "CLRD")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x001C)
                    throw new InvalidDataException("Size1");

                var numLevels = Enum.GetValues(typeof(Level)).Length;
                this.StoryFlags = new Dictionary<Level, byte>(numLevels);
                this.PracticeFlags = new Dictionary<Level, byte>(numLevels);
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Dictionary<Level, byte> StoryFlags { get; private set; }     // really...?

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Dictionary<Level, byte> PracticeFlags { get; private set; }  // really...?

            public Chara Chara { get; private set; }            // size: 4Bytes

            public override void ReadFrom(BinaryReader reader)
            {
                var levels = Utils.GetEnumerator<Level>();

                reader.ReadUInt32();    // always 0x00000001?
                foreach (var level in levels)
                    this.StoryFlags.Add(level, reader.ReadByte());
                foreach (var level in levels)
                    this.PracticeFlags.Add(level, reader.ReadByte());
                this.Chara = (Chara)reader.ReadInt32();
            }
        }

        private class CardAttack : Chapter      // per card
        {
            public CardAttack(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "CATK")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0078)
                    throw new InvalidDataException("Size1");

                var numCharas = Enum.GetValues(typeof(CharaWithTotal)).Length;
                this.MaxBonuses = new Dictionary<CharaWithTotal, uint>(numCharas);
                this.TrialCounts = new Dictionary<CharaWithTotal, ushort>(numCharas);
                this.ClearCounts = new Dictionary<CharaWithTotal, ushort>(numCharas);
            }

            public Dictionary<CharaWithTotal, uint> MaxBonuses { get; private set; }

            public short Number { get; private set; }       // 1-based

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] CardName { get; private set; }    // .Length = 0x30

            public Dictionary<CharaWithTotal, ushort> TrialCounts { get; private set; }

            public Dictionary<CharaWithTotal, ushort> ClearCounts { get; private set; }

            public override void ReadFrom(BinaryReader reader)
            {
                var charas = Utils.GetEnumerator<CharaWithTotal>();
                reader.ReadUInt32();    // always 0x00000001?
                foreach (var chara in charas)
                    this.MaxBonuses.Add(chara, reader.ReadUInt32());
                this.Number = (short)(reader.ReadInt16() + 1);
                reader.ReadByte();
                this.CardName = reader.ReadBytes(0x30);
                reader.ReadByte();      // always 0x00?
                foreach (var chara in charas)
                    this.TrialCounts.Add(chara, reader.ReadUInt16());
                foreach (var chara in charas)
                    this.ClearCounts.Add(chara, reader.ReadUInt16());
            }

            public bool HasTried()
            {
                return this.TrialCounts[CharaWithTotal.Total] > 0;
            }
        }

        private class PracticeScore : Chapter   // per character, level, stage
        {
            public PracticeScore(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "PSCR")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0018)
                    throw new InvalidDataException("Size1");
            }

            public int TrialCount { get; private set; }     // really...?

            public int HighScore { get; private set; }

            public Chara Chara { get; private set; }        // size: 1Byte

            public Level Level { get; private set; }        // size: 1Byte

            public Stage Stage { get; private set; }        // size: 1Byte

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadUInt32();    // always 0x00000001?
                this.TrialCount = reader.ReadInt32();
                this.HighScore = reader.ReadInt32();
                this.Chara = (Chara)reader.ReadByte();
                this.Level = (Level)reader.ReadByte();
                this.Stage = (Stage)reader.ReadByte();
                reader.ReadByte();      // always 0x00?
            }
        }

        private class PlayStatus : Chapter
        {
            public PlayStatus(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "PLST")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0160)
                    throw new InvalidDataException("Size1");

                this.PlayCounts =
                    new Dictionary<LevelWithTotal, PlayCount>(Enum.GetValues(typeof(LevelWithTotal)).Length);
            }

            public Time TotalRunningTime { get; private set; }

            public Time TotalPlayTime { get; private set; }

            public Dictionary<LevelWithTotal, PlayCount> PlayCounts { get; private set; }

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadUInt32();    // always 0x00000001?
                this.TotalRunningTime = new Time(
                    reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), false);
                this.TotalPlayTime = new Time(
                    reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), false);
                foreach (var level in Utils.GetEnumerator<LevelWithTotal>())
                {
                    var playCount = new PlayCount();
                    playCount.ReadFrom(reader);
                    if (!this.PlayCounts.ContainsKey(level))
                        this.PlayCounts.Add(level, playCount);
                }
            }
        }

        private class PlayCount : IBinaryReadable     // per level-with-total
        {
            public PlayCount()
            {
                this.Trials = new Dictionary<Chara, int>(Enum.GetValues(typeof(Chara)).Length);
            }

            public int TotalTrial { get; private set; }

            public Dictionary<Chara, int> Trials { get; private set; }

            public int TotalRetry { get; private set; }

            public int TotalClear { get; private set; }

            public int TotalContinue { get; private set; }

            public int TotalPractice { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.TotalTrial = reader.ReadInt32();
                foreach (var chara in Utils.GetEnumerator<Chara>())
                    this.Trials.Add(chara, reader.ReadInt32());
                this.TotalRetry = reader.ReadInt32();
                this.TotalClear = reader.ReadInt32();
                this.TotalContinue = reader.ReadInt32();
                this.TotalPractice = reader.ReadInt32();
            }
        }

        private class LastName : Chapter
        {
            public LastName(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "LSNM")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0018)
                    throw new InvalidDataException("Size1");
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] Name { get; private set; }    // .Length = 12, null-terminated

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadUInt32();    // always 0x00000001?
                this.Name = reader.ReadBytes(12);
            }
        }

        private class VersionInfo : Chapter
        {
            public VersionInfo(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "VRSM")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x001C)
                    throw new InvalidDataException("Size1");
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] Version { get; private set; }     // .Length = 6, null-terminated

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadUInt16();    // always 0x0001?
                reader.ReadUInt16();
                this.Version = reader.ReadBytes(6);
                reader.ReadBytes(3);
                reader.ReadBytes(3);
                reader.ReadUInt32();
            }
        }
    }
}
