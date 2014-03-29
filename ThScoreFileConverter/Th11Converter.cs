//-----------------------------------------------------------------------
// <copyright file="Th11Converter.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:CurlyBracketsMustNotBeOmitted", Justification = "Reviewed.")]

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
    using CardInfo = SpellCardInfo<Th11Converter.Stage, Th11Converter.Level>;

    internal class Th11Converter : ThConverter
    {
        private static readonly Dictionary<int, CardInfo> CardTable;

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
        static Th11Converter()
        {
            var cardList = new List<CardInfo>()
            {
                new CardInfo(  1, "怪奇「釣瓶落としの怪」",               Stage.St1,   Level.Hard),
                new CardInfo(  2, "怪奇「釣瓶落としの怪」",               Stage.St1,   Level.Lunatic),
                new CardInfo(  3, "罠符「キャプチャーウェブ」",           Stage.St1,   Level.Easy),
                new CardInfo(  4, "罠符「キャプチャーウェブ」",           Stage.St1,   Level.Normal),
                new CardInfo(  5, "蜘蛛「石窟の蜘蛛の巣」",               Stage.St1,   Level.Hard),
                new CardInfo(  6, "蜘蛛「石窟の蜘蛛の巣」",               Stage.St1,   Level.Lunatic),
                new CardInfo(  7, "瘴符「フィルドミアズマ」",             Stage.St1,   Level.Easy),
                new CardInfo(  8, "瘴符「フィルドミアズマ」",             Stage.St1,   Level.Normal),
                new CardInfo(  9, "瘴気「原因不明の熱病」",               Stage.St1,   Level.Hard),
                new CardInfo( 10, "瘴気「原因不明の熱病」",               Stage.St1,   Level.Lunatic),
                new CardInfo( 11, "妬符「グリーンアイドモンスター」",     Stage.St2,   Level.Easy),
                new CardInfo( 12, "妬符「グリーンアイドモンスター」",     Stage.St2,   Level.Normal),
                new CardInfo( 13, "嫉妬「緑色の目をした見えない怪物」",   Stage.St2,   Level.Hard),
                new CardInfo( 14, "嫉妬「緑色の目をした見えない怪物」",   Stage.St2,   Level.Lunatic),
                new CardInfo( 15, "花咲爺「華やかなる仁者への嫉妬」",     Stage.St2,   Level.Easy),
                new CardInfo( 16, "花咲爺「華やかなる仁者への嫉妬」",     Stage.St2,   Level.Normal),
                new CardInfo( 17, "花咲爺「シロの灰」",                   Stage.St2,   Level.Hard),
                new CardInfo( 18, "花咲爺「シロの灰」",                   Stage.St2,   Level.Lunatic),
                new CardInfo( 19, "舌切雀「謙虚なる富者への片恨」",       Stage.St2,   Level.Easy),
                new CardInfo( 20, "舌切雀「謙虚なる富者への片恨」",       Stage.St2,   Level.Normal),
                new CardInfo( 21, "舌切雀「大きな葛籠と小さな葛籠」",     Stage.St2,   Level.Hard),
                new CardInfo( 22, "舌切雀「大きな葛籠と小さな葛籠」",     Stage.St2,   Level.Lunatic),
                new CardInfo( 23, "恨符「丑の刻参り」",                   Stage.St2,   Level.Easy),
                new CardInfo( 24, "恨符「丑の刻参り」",                   Stage.St2,   Level.Normal),
                new CardInfo( 25, "恨符「丑の刻参り七日目」",             Stage.St2,   Level.Hard),
                new CardInfo( 26, "恨符「丑の刻参り七日目」",             Stage.St2,   Level.Lunatic),
                new CardInfo( 27, "鬼符「怪力乱神」",                     Stage.St3,   Level.Easy),
                new CardInfo( 28, "鬼符「怪力乱神」",                     Stage.St3,   Level.Normal),
                new CardInfo( 29, "鬼符「怪力乱神」",                     Stage.St3,   Level.Hard),
                new CardInfo( 30, "鬼符「怪力乱神」",                     Stage.St3,   Level.Lunatic),
                new CardInfo( 31, "怪輪「地獄の苦輪」",                   Stage.St3,   Level.Easy),
                new CardInfo( 32, "怪輪「地獄の苦輪」",                   Stage.St3,   Level.Normal),
                new CardInfo( 33, "枷符「咎人の外さぬ枷」",               Stage.St3,   Level.Hard),
                new CardInfo( 34, "枷符「咎人の外さぬ枷」",               Stage.St3,   Level.Lunatic),
                new CardInfo( 35, "力業「大江山嵐」",                     Stage.St3,   Level.Easy),
                new CardInfo( 36, "力業「大江山嵐」",                     Stage.St3,   Level.Normal),
                new CardInfo( 37, "力業「大江山颪」",                     Stage.St3,   Level.Hard),
                new CardInfo( 38, "力業「大江山颪」",                     Stage.St3,   Level.Lunatic),
                new CardInfo( 39, "四天王奥義「三歩必殺」",               Stage.St3,   Level.Easy),
                new CardInfo( 40, "四天王奥義「三歩必殺」",               Stage.St3,   Level.Normal),
                new CardInfo( 41, "四天王奥義「三歩必殺」",               Stage.St3,   Level.Hard),
                new CardInfo( 42, "四天王奥義「三歩必殺」",               Stage.St3,   Level.Lunatic),
                new CardInfo( 43, "想起「テリブルスーヴニール」",         Stage.St4,   Level.Easy),
                new CardInfo( 44, "想起「テリブルスーヴニール」",         Stage.St4,   Level.Normal),
                new CardInfo( 45, "想起「恐怖催眠術」",                   Stage.St4,   Level.Hard),
                new CardInfo( 46, "想起「恐怖催眠術」",                   Stage.St4,   Level.Lunatic),
                new CardInfo( 47, "想起「二重黒死蝶」",                   Stage.St4,   Level.Easy),
                new CardInfo( 48, "想起「二重黒死蝶」",                   Stage.St4,   Level.Normal),
                new CardInfo( 49, "想起「二重黒死蝶」",                   Stage.St4,   Level.Hard),
                new CardInfo( 50, "想起「二重黒死蝶」",                   Stage.St4,   Level.Lunatic),
                new CardInfo( 51, "想起「飛行虫ネスト」",                 Stage.St4,   Level.Easy),
                new CardInfo( 52, "想起「飛行虫ネスト」",                 Stage.St4,   Level.Normal),
                new CardInfo( 53, "想起「飛行虫ネスト」",                 Stage.St4,   Level.Hard),
                new CardInfo( 54, "想起「飛行虫ネスト」",                 Stage.St4,   Level.Lunatic),
                new CardInfo( 55, "想起「波と粒の境界」",                 Stage.St4,   Level.Easy),
                new CardInfo( 56, "想起「波と粒の境界」",                 Stage.St4,   Level.Normal),
                new CardInfo( 57, "想起「波と粒の境界」",                 Stage.St4,   Level.Hard),
                new CardInfo( 58, "想起「波と粒の境界」",                 Stage.St4,   Level.Lunatic),
                new CardInfo( 59, "想起「戸隠山投げ」",                   Stage.St4,   Level.Easy),
                new CardInfo( 60, "想起「戸隠山投げ」",                   Stage.St4,   Level.Normal),
                new CardInfo( 61, "想起「戸隠山投げ」",                   Stage.St4,   Level.Hard),
                new CardInfo( 62, "想起「戸隠山投げ」",                   Stage.St4,   Level.Lunatic),
                new CardInfo( 63, "想起「百万鬼夜行」",                   Stage.St4,   Level.Easy),
                new CardInfo( 64, "想起「百万鬼夜行」",                   Stage.St4,   Level.Normal),
                new CardInfo( 65, "想起「百万鬼夜行」",                   Stage.St4,   Level.Hard),
                new CardInfo( 66, "想起「百万鬼夜行」",                   Stage.St4,   Level.Lunatic),
                new CardInfo( 67, "想起「濛々迷霧」",                     Stage.St4,   Level.Easy),
                new CardInfo( 68, "想起「濛々迷霧」",                     Stage.St4,   Level.Normal),
                new CardInfo( 69, "想起「濛々迷霧」",                     Stage.St4,   Level.Hard),
                new CardInfo( 70, "想起「濛々迷霧」",                     Stage.St4,   Level.Lunatic),
                new CardInfo( 71, "想起「風神木の葉隠れ」",               Stage.St4,   Level.Easy),
                new CardInfo( 72, "想起「風神木の葉隠れ」",               Stage.St4,   Level.Normal),
                new CardInfo( 73, "想起「風神木の葉隠れ」",               Stage.St4,   Level.Hard),
                new CardInfo( 74, "想起「風神木の葉隠れ」",               Stage.St4,   Level.Lunatic),
                new CardInfo( 75, "想起「天狗のマクロバースト」",         Stage.St4,   Level.Easy),
                new CardInfo( 76, "想起「天狗のマクロバースト」",         Stage.St4,   Level.Normal),
                new CardInfo( 77, "想起「天狗のマクロバースト」",         Stage.St4,   Level.Hard),
                new CardInfo( 78, "想起「天狗のマクロバースト」",         Stage.St4,   Level.Lunatic),
                new CardInfo( 79, "想起「鳥居つむじ風」",                 Stage.St4,   Level.Easy),
                new CardInfo( 80, "想起「鳥居つむじ風」",                 Stage.St4,   Level.Normal),
                new CardInfo( 81, "想起「鳥居つむじ風」",                 Stage.St4,   Level.Hard),
                new CardInfo( 82, "想起「鳥居つむじ風」",                 Stage.St4,   Level.Lunatic),
                new CardInfo( 83, "想起「春の京人形」",                   Stage.St4,   Level.Easy),
                new CardInfo( 84, "想起「春の京人形」",                   Stage.St4,   Level.Normal),
                new CardInfo( 85, "想起「春の京人形」",                   Stage.St4,   Level.Hard),
                new CardInfo( 86, "想起「春の京人形」",                   Stage.St4,   Level.Lunatic),
                new CardInfo( 87, "想起「ストロードールカミカゼ」",       Stage.St4,   Level.Easy),
                new CardInfo( 88, "想起「ストロードールカミカゼ」",       Stage.St4,   Level.Normal),
                new CardInfo( 89, "想起「ストロードールカミカゼ」",       Stage.St4,   Level.Hard),
                new CardInfo( 90, "想起「ストロードールカミカゼ」",       Stage.St4,   Level.Lunatic),
                new CardInfo( 91, "想起「リターンイナニメトネス」",       Stage.St4,   Level.Easy),
                new CardInfo( 92, "想起「リターンイナニメトネス」",       Stage.St4,   Level.Normal),
                new CardInfo( 93, "想起「リターンイナニメトネス」",       Stage.St4,   Level.Hard),
                new CardInfo( 94, "想起「リターンイナニメトネス」",       Stage.St4,   Level.Lunatic),
                new CardInfo( 95, "想起「マーキュリポイズン」",           Stage.St4,   Level.Easy),
                new CardInfo( 96, "想起「マーキュリポイズン」",           Stage.St4,   Level.Normal),
                new CardInfo( 97, "想起「マーキュリポイズン」",           Stage.St4,   Level.Hard),
                new CardInfo( 98, "想起「マーキュリポイズン」",           Stage.St4,   Level.Lunatic),
                new CardInfo( 99, "想起「プリンセスウンディネ」",         Stage.St4,   Level.Easy),
                new CardInfo(100, "想起「プリンセスウンディネ」",         Stage.St4,   Level.Normal),
                new CardInfo(101, "想起「プリンセスウンディネ」",         Stage.St4,   Level.Hard),
                new CardInfo(102, "想起「プリンセスウンディネ」",         Stage.St4,   Level.Lunatic),
                new CardInfo(103, "想起「賢者の石」",                     Stage.St4,   Level.Easy),
                new CardInfo(104, "想起「賢者の石」",                     Stage.St4,   Level.Normal),
                new CardInfo(105, "想起「賢者の石」",                     Stage.St4,   Level.Hard),
                new CardInfo(106, "想起「賢者の石」",                     Stage.St4,   Level.Lunatic),
                new CardInfo(107, "想起「のびーるアーム」",               Stage.St4,   Level.Easy),
                new CardInfo(108, "想起「のびーるアーム」",               Stage.St4,   Level.Normal),
                new CardInfo(109, "想起「のびーるアーム」",               Stage.St4,   Level.Hard),
                new CardInfo(110, "想起「のびーるアーム」",               Stage.St4,   Level.Lunatic),
                new CardInfo(111, "想起「河童のポロロッカ」",             Stage.St4,   Level.Easy),
                new CardInfo(112, "想起「河童のポロロッカ」",             Stage.St4,   Level.Normal),
                new CardInfo(113, "想起「河童のポロロッカ」",             Stage.St4,   Level.Hard),
                new CardInfo(114, "想起「河童のポロロッカ」",             Stage.St4,   Level.Lunatic),
                new CardInfo(115, "想起「光り輝く水底のトラウマ」",       Stage.St4,   Level.Easy),
                new CardInfo(116, "想起「光り輝く水底のトラウマ」",       Stage.St4,   Level.Normal),
                new CardInfo(117, "想起「光り輝く水底のトラウマ」",       Stage.St4,   Level.Hard),
                new CardInfo(118, "想起「光り輝く水底のトラウマ」",       Stage.St4,   Level.Lunatic),
                new CardInfo(119, "猫符「キャッツウォーク」",             Stage.St5,   Level.Easy),
                new CardInfo(120, "猫符「キャッツウォーク」",             Stage.St5,   Level.Normal),
                new CardInfo(121, "猫符「怨霊猫乱歩」",                   Stage.St5,   Level.Hard),
                new CardInfo(122, "猫符「怨霊猫乱歩」",                   Stage.St5,   Level.Lunatic),
                new CardInfo(123, "呪精「ゾンビフェアリー」",             Stage.St5,   Level.Easy),
                new CardInfo(124, "呪精「ゾンビフェアリー」",             Stage.St5,   Level.Normal),
                new CardInfo(125, "呪精「怨霊憑依妖精」",                 Stage.St5,   Level.Hard),
                new CardInfo(126, "呪精「怨霊憑依妖精」",                 Stage.St5,   Level.Lunatic),
                new CardInfo(127, "恨霊「スプリーンイーター」",           Stage.St5,   Level.Easy),
                new CardInfo(128, "恨霊「スプリーンイーター」",           Stage.St5,   Level.Normal),
                new CardInfo(129, "屍霊「食人怨霊」",                     Stage.St5,   Level.Hard),
                new CardInfo(130, "屍霊「食人怨霊」",                     Stage.St5,   Level.Lunatic),
                new CardInfo(131, "贖罪「旧地獄の針山」",                 Stage.St5,   Level.Easy),
                new CardInfo(132, "贖罪「旧地獄の針山」",                 Stage.St5,   Level.Normal),
                new CardInfo(133, "贖罪「昔時の針と痛がる怨霊」",         Stage.St5,   Level.Hard),
                new CardInfo(134, "贖罪「昔時の針と痛がる怨霊」",         Stage.St5,   Level.Lunatic),
                new CardInfo(135, "「死灰復燃」",                         Stage.St5,   Level.Easy),
                new CardInfo(136, "「死灰復燃」",                         Stage.St5,   Level.Normal),
                new CardInfo(137, "「小悪霊復活せし」",                   Stage.St5,   Level.Hard),
                new CardInfo(138, "「小悪霊復活せし」",                   Stage.St5,   Level.Lunatic),
                new CardInfo(139, "妖怪「火焔の車輪」",                   Stage.St6,   Level.Easy),
                new CardInfo(140, "妖怪「火焔の車輪」",                   Stage.St6,   Level.Normal),
                new CardInfo(141, "妖怪「火焔の車輪」",                   Stage.St6,   Level.Hard),
                new CardInfo(142, "妖怪「火焔の車輪」",                   Stage.St6,   Level.Lunatic),
                new CardInfo(143, "核熱「ニュークリアフュージョン」",     Stage.St6,   Level.Easy),
                new CardInfo(144, "核熱「ニュークリアフュージョン」",     Stage.St6,   Level.Normal),
                new CardInfo(145, "核熱「ニュークリアエクスカーション」", Stage.St6,   Level.Hard),
                new CardInfo(146, "核熱「核反応制御不能」",               Stage.St6,   Level.Lunatic),
                new CardInfo(147, "爆符「プチフレア」",                   Stage.St6,   Level.Easy),
                new CardInfo(148, "爆符「メガフレア」",                   Stage.St6,   Level.Normal),
                new CardInfo(149, "爆符「ギガフレア」",                   Stage.St6,   Level.Hard),
                new CardInfo(150, "爆符「ペタフレア」",                   Stage.St6,   Level.Lunatic),
                new CardInfo(151, "焔星「フィクストスター」",             Stage.St6,   Level.Easy),
                new CardInfo(152, "焔星「フィクストスター」",             Stage.St6,   Level.Normal),
                new CardInfo(153, "焔星「プラネタリーレボリューション」", Stage.St6,   Level.Hard),
                new CardInfo(154, "焔星「十凶星」",                       Stage.St6,   Level.Lunatic),
                new CardInfo(155, "「地獄極楽メルトダウン」",             Stage.St6,   Level.Easy),
                new CardInfo(156, "「地獄極楽メルトダウン」",             Stage.St6,   Level.Normal),
                new CardInfo(157, "「ヘルズトカマク」",                   Stage.St6,   Level.Hard),
                new CardInfo(158, "「ヘルズトカマク」",                   Stage.St6,   Level.Lunatic),
                new CardInfo(159, "「地獄の人工太陽」",                   Stage.St6,   Level.Easy),
                new CardInfo(160, "「地獄の人工太陽」",                   Stage.St6,   Level.Normal),
                new CardInfo(161, "「サブタレイニアンサン」",             Stage.St6,   Level.Hard),
                new CardInfo(162, "「サブタレイニアンサン」",             Stage.St6,   Level.Lunatic),
                new CardInfo(163, "秘法「九字刺し」",                     Stage.Extra, Level.Extra),
                new CardInfo(164, "奇跡「ミラクルフルーツ」",             Stage.Extra, Level.Extra),
                new CardInfo(165, "神徳「五穀豊穣ライスシャワー」",       Stage.Extra, Level.Extra),
                new CardInfo(166, "表象「夢枕にご先祖総立ち」",           Stage.Extra, Level.Extra),
                new CardInfo(167, "表象「弾幕パラノイア」",               Stage.Extra, Level.Extra),
                new CardInfo(168, "本能「イドの解放」",                   Stage.Extra, Level.Extra),
                new CardInfo(169, "抑制「スーパーエゴ」",                 Stage.Extra, Level.Extra),
                new CardInfo(170, "反応「妖怪ポリグラフ」",               Stage.Extra, Level.Extra),
                new CardInfo(171, "無意識「弾幕のロールシャッハ」",       Stage.Extra, Level.Extra),
                new CardInfo(172, "復燃「恋の埋火」",                     Stage.Extra, Level.Extra),
                new CardInfo(173, "深層「無意識の遺伝子」",               Stage.Extra, Level.Extra),
                new CardInfo(174, "「嫌われ者のフィロソフィ」",           Stage.Extra, Level.Extra),
                new CardInfo(175, "「サブタレイニアンローズ」",           Stage.Extra, Level.Extra)
            };
            CardTable = cardList.ToDictionary(card => card.Number);

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

        public Th11Converter()
        {
        }

        public enum Level
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra
        }

        public enum LevelWithTotal
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("T")] Total
        }

        public enum Chara
        {
            [EnumAltName("RY")] ReimuYukari,
            [EnumAltName("RS")] ReimuSuika,
            [EnumAltName("RA")] ReimuAya,
            [EnumAltName("MA")] MarisaAlice,
            [EnumAltName("MP")] MarisaPatchouli,
            [EnumAltName("MN")] MarisaNitori
        }

        public enum CharaWithTotal
        {
            [EnumAltName("RY")] ReimuYukari,
            [EnumAltName("RS")] ReimuSuika,
            [EnumAltName("RA")] ReimuAya,
            [EnumAltName("MA")] MarisaAlice,
            [EnumAltName("MP")] MarisaPatchouli,
            [EnumAltName("MN")] MarisaNitori,
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
            [EnumAltName("X")] Extra
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
            [EnumAltName("0")] Total
        }

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        public enum StageProgress
        {
            [EnumAltName("-------")]     None,
            [EnumAltName("Stage 1")]     St1,
            [EnumAltName("Stage 2")]     St2,
            [EnumAltName("Stage 3")]     St3,
            [EnumAltName("Stage 4")]     St4,
            [EnumAltName("Stage 5")]     St5,
            [EnumAltName("Stage 6")]     St6,
            [EnumAltName("Extra Stage")] Extra,
            [EnumAltName("All Clear")]   Clear
        }

        public override string SupportedVersions
        {
            get { return "1.00a"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th11decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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

            var allLines = reader.ReadToEnd();
            allLines = this.ReplaceScore(allLines);
            allLines = this.ReplaceCareer(allLines);
            allLines = this.ReplaceCard(allLines, hideUntriedCards);
            allLines = this.ReplaceCollectRate(allLines);
            allLines = this.ReplaceClear(allLines);
            allLines = this.ReplaceChara(allLines);
            allLines = this.ReplaceCharaEx(allLines);
            allLines = this.ReplacePractice(allLines);

            writer.Write(allLines);
            writer.Flush();
            writer.BaseStream.SetLength(writer.BaseStream.Position);
        }

        private static bool Decrypt(Stream input, Stream output)
        {
            var reader = new BinaryReader(input);
            var writer = new BinaryWriter(output);

            var header = new Header();
            header.ReadFrom(reader);
            if (header.Signature != "TH11")
                return false;
            if (header.EncodedAllSize != reader.BaseStream.Length)
                return false;

            header.WriteTo(writer);
            ThCrypt.Decrypt(input, output, header.EncodedBodySize, 0xAC, 0x35, 0x10, header.EncodedBodySize);

            return true;
        }

        private static bool Extract(Stream input, Stream output)
        {
            var reader = new BinaryReader(input);
            var writer = new BinaryWriter(output);

            var header = new Header();
            header.ReadFrom(reader);
            header.WriteTo(writer);

            var bodyBeginPos = output.Position;
            Lzss.Extract(input, output);
            output.Flush();
            output.SetLength(output.Position);

            return header.DecodedBodySize == (output.Position - bodyBeginPos);
        }

        private static bool Validate(Stream input)
        {
            var reader = new BinaryReader(input);

            var header = new Header();
            header.ReadFrom(reader);
            var remainSize = header.DecodedBodySize;
            var chapter = new Chapter();

            try
            {
                while (true)
                {
                    chapter.ReadFrom(reader);

                    if (!((chapter.Signature == "CR") && (chapter.Version == 0x0000)) &&
                        !((chapter.Signature == "ST") && (chapter.Version == 0x0000)))
                        return false;

                    //// -4 means the size of Size.
                    reader.BaseStream.Seek(-4, SeekOrigin.Current);
                    //// 8 means the total size of Signature, Version, and Checksum.
                    var body = reader.ReadBytes(chapter.Size - 8);
                    var sum = body.Sum(elem => (int)elem);
                    if (sum != chapter.Checksum)
                        return false;

                    remainSize -= chapter.Size;
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

            var header = new Header();
            header.ReadFrom(reader);
            allScoreData.Header = header;

            try
            {
                while (true)
                {
                    chapter.ReadFrom(reader);
                    switch (chapter.Signature)
                    {
                        case "CR":
                            {
                                var clearData = new ClearData(chapter);
                                clearData.ReadFrom(reader);
                                if (!allScoreData.ClearData.ContainsKey(clearData.Chara))
                                    allScoreData.ClearData.Add(clearData.Chara, clearData);
                            }
                            break;
                        case "ST":
                            {
                                var status = new Status(chapter);
                                status.ReadFrom(reader);
                                allScoreData.Status = status;
                            }
                            break;
                        default:
                            // 12 means the total size of Signature, Version, Checksum, and Size.
                            reader.ReadBytes(chapter.Size - 12);
                            break;
                    }
                }
            }
            catch (EndOfStreamException)
            {
                // It's OK, do nothing.
            }

            if ((allScoreData.Header != null) &&
                (allScoreData.ClearData.Count == Enum.GetValues(typeof(CharaWithTotal)).Length) &&
                (allScoreData.Status != null))
                return allScoreData;
            else
                return null;
        }

        // %T11SCR[w][xx][y][z]
        private string ReplaceScore(string input)
        {
            var pattern = Utils.Format(@"%T11SCR([{0}])({1})(\d)([1-5])", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value);
                var chara = (CharaWithTotal)ToChara(match.Groups[2].Value);
                var rank = Utils.ToZeroBased(int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                var ranking = this.allScoreData.ClearData[chara].Rankings[level][rank];
                switch (type)
                {
                    case 1:     // name
                        return Encoding.Default.GetString(ranking.Name).Split('\0')[0];
                    case 2:     // score
                        return this.ToNumberString((ranking.Score * 10) + ranking.ContinueCount);
                    case 3:     // stage
                        if (ranking.DateTime > 0)
                            return (ranking.StageProgress == StageProgress.Extra)
                                ? "Not Clear" : ranking.StageProgress.ToShortName();
                        else
                            return StageProgress.None.ToShortName();
                    case 4:     // date & time
                        if (ranking.DateTime > 0)
                            return new DateTime(1970, 1, 1).AddSeconds(ranking.DateTime)
                                .ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                        else
                            return "----/--/-- --:--:--";
                    case 5:     // slow
                        if (ranking.DateTime > 0)
                            return Utils.Format("{0:F3}%", ranking.SlowRate);
                        else
                            return "-----%";
                    default:    // unreachable
                        return match.ToString();
                }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T11C[xxx][yy][z]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
        private string ReplaceCareer(string input)
        {
            var pattern = Utils.Format(@"%T11C(\d{{3}})({0})([12])", CharaWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var chara = ToCharaWithTotal(match.Groups[2].Value);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                Func<SpellCard, int> getCount = (card => 0);
                if (type == 1)
                    getCount = (card => card.ClearCount);
                else
                    getCount = (card => card.TrialCount);

                var cards = this.allScoreData.ClearData[chara].Cards;
                if (number == 0)
                    return this.ToNumberString(cards.Values.Sum(getCount));
                else if (CardTable.ContainsKey(number))
                {
                    SpellCard card;
                    if (cards.TryGetValue(number, out card))
                        return this.ToNumberString(getCount(card));
                    else
                        return "0";
                }
                else
                    return match.ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T11CARD[xxx][y]
        private string ReplaceCard(string input, bool hideUntriedCards)
        {
            var pattern = @"%T11CARD(\d{3})([NR])";
            var evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var type = match.Groups[2].Value.ToUpperInvariant();

                if (CardTable.ContainsKey(number))
                {
                    if (type == "N")
                    {
                        if (hideUntriedCards)
                        {
                            var cards = this.allScoreData.ClearData[CharaWithTotal.Total].Cards;
                            SpellCard card;
                            if (!cards.TryGetValue(number, out card) || !card.HasTried())
                                return "??????????";
                        }

                        return CardTable[number].Name;
                    }
                    else
                        return CardTable[number].Level.ToString();
                }
                else
                    return match.ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T11CRG[w][xx][y][z]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
        private string ReplaceCollectRate(string input)
        {
            var pattern = Utils.Format(
                @"%T11CRG([{0}])({1})([{2}])([12])",
                LevelWithTotalPattern,
                CharaWithTotalPattern,
                StageWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevelWithTotal(match.Groups[1].Value);
                var chara = ToCharaWithTotal(match.Groups[2].Value);
                var stage = ToStageWithTotal(match.Groups[3].Value);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                if (stage == StageWithTotal.Extra)
                    return match.ToString();

                Func<SpellCard, bool> findByLevel = (card => true);
                Func<SpellCard, bool> findByStage = (card => true);
                Func<SpellCard, bool> findByType = (card => true);

                if (stage == StageWithTotal.Total)
                {
                    // Do nothing
                }
                else
                    findByStage = (card => CardTable[card.Number].Stage == (Stage)stage);

                switch (level)
                {
                    case LevelWithTotal.Total:
                        // Do nothing
                        break;
                    case LevelWithTotal.Extra:
                        findByStage = (card => CardTable[card.Number].Stage == Stage.Extra);
                        break;
                    default:
                        findByLevel = (card => card.Level == (Level)level);
                        break;
                }

                if (type == 1)
                    findByType = (card => card.ClearCount > 0);
                else
                    findByType = (card => card.TrialCount > 0);

                var and = Utils.MakeAndPredicate(findByLevel, findByStage, findByType);
                return this.allScoreData.ClearData[chara].Cards.Values.Count(and)
                    .ToString(CultureInfo.CurrentCulture);
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T11CLEAR[x][yy]
        private string ReplaceClear(string input)
        {
            var pattern = Utils.Format(@"%T11CLEAR([{0}])({1})", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value);
                var chara = (CharaWithTotal)ToChara(match.Groups[2].Value);

                var rankings = this.allScoreData.ClearData[chara].Rankings[level]
                    .Where(ranking => ranking.DateTime > 0);
                var stageProgress = (rankings.Count() > 0)
                    ? rankings.Max(ranking => ranking.StageProgress) : StageProgress.None;

                return (stageProgress == StageProgress.Extra) ? "Not Clear" : stageProgress.ToShortName();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T11CHARA[xx][y]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
        private string ReplaceChara(string input)
        {
            var pattern = Utils.Format(@"%T11CHARA({0})([1-3])", CharaWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var chara = ToCharaWithTotal(match.Groups[1].Value);
                var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                Func<ClearData, long> getValueByType = (data => 0L);
                Func<long, string> toString = (value => string.Empty);
                if (type == 1)
                {
                    getValueByType = (data => data.TotalPlayCount);
                    toString = (value => this.ToNumberString(value));
                }
                else if (type == 2)
                {
                    getValueByType = (data => data.PlayTime);
                    toString = (value => new Time(value).ToString());
                }
                else
                {
                    getValueByType = (data => data.ClearCounts.Values.Sum());
                    toString = (value => this.ToNumberString(value));
                }

                Func<AllScoreData, long> getValueByChara = (allData => 0L);
                if (chara == CharaWithTotal.Total)
                    getValueByChara = (allData => allData.ClearData.Values.Sum(
                        data => (data.Chara != chara) ? getValueByType(data) : 0L));
                else
                    getValueByChara = (allData => getValueByType(allData.ClearData[chara]));

                return toString(getValueByChara(this.allScoreData));
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T11CHARAEX[x][yy][z]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
        private string ReplaceCharaEx(string input)
        {
            var pattern = Utils.Format(
                @"%T11CHARAEX([{0}])({1})([1-3])", LevelWithTotalPattern, CharaWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevelWithTotal(match.Groups[1].Value);
                var chara = ToCharaWithTotal(match.Groups[2].Value);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                Func<ClearData, long> getValueByType = (data => 0L);
                Func<long, string> toString = (value => string.Empty);
                if (type == 1)
                {
                    getValueByType = (data => data.TotalPlayCount);
                    toString = (value => this.ToNumberString(value));
                }
                else if (type == 2)
                {
                    getValueByType = (data => data.PlayTime);
                    toString = (value => new Time(value).ToString());
                }
                else
                {
                    if (level == LevelWithTotal.Total)
                        getValueByType = (data => data.ClearCounts.Values.Sum());
                    else
                        getValueByType = (data => data.ClearCounts[(Level)level]);
                    toString = (value => this.ToNumberString(value));
                }

                Func<AllScoreData, long> getValueByChara = (allData => 0L);
                if (chara == CharaWithTotal.Total)
                    getValueByChara = (allData => allData.ClearData.Values.Sum(
                        data => (data.Chara != chara) ? getValueByType(data) : 0L));
                else
                    getValueByChara = (allData => getValueByType(allData.ClearData[chara]));

                return toString(getValueByChara(this.allScoreData));
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T11PRAC[x][yy][z]
        private string ReplacePractice(string input)
        {
            var pattern = Utils.Format(
                @"%T11PRAC([{0}])({1})([{2}])", LevelPattern, CharaPattern, StagePattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value);
                var chara = (CharaWithTotal)ToChara(match.Groups[2].Value);
                var stage = ToStage(match.Groups[3].Value);

                if (level == Level.Extra)
                    return match.ToString();
                if (stage == Stage.Extra)
                    return match.ToString();

                if (this.allScoreData.ClearData.ContainsKey(chara))
                {
                    var key = new LevelStagePair(level, stage);
                    var practices = this.allScoreData.ClearData[chara].Practices;
                    return practices.ContainsKey(key)
                        ? this.ToNumberString(practices[key].Score * 10) : "0";
                }
                else
                    return "0";
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        private class LevelStagePair : Pair<Level, Stage>
        {
            public LevelStagePair(Level level, Stage stage)
                : base(level, stage)
            {
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Level Level
            {
                get { return this.First; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Stage Stage
            {
                get { return this.Second; }
            }
        }

        private class AllScoreData
        {
            public AllScoreData()
            {
                this.ClearData =
                    new Dictionary<CharaWithTotal, ClearData>(Enum.GetValues(typeof(CharaWithTotal)).Length);
            }

            public Header Header { get; set; }

            public Dictionary<CharaWithTotal, ClearData> ClearData { get; set; }

            public Status Status { get; set; }
        }

        private class Header : IBinaryReadable
        {
            private uint unknown1;
            private uint unknown2;

            public string Signature { get; private set; }

            public int EncodedAllSize { get; private set; }

            public int EncodedBodySize { get; private set; }

            public int DecodedBodySize { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.Signature = new string(reader.ReadChars(4));
                this.EncodedAllSize = reader.ReadInt32();
                this.unknown1 = reader.ReadUInt32();
                this.unknown2 = reader.ReadUInt32();
                this.EncodedBodySize = reader.ReadInt32();
                this.DecodedBodySize = reader.ReadInt32();
            }

            public void WriteTo(BinaryWriter writer)
            {
                writer.Write(this.Signature.ToCharArray());
                writer.Write(this.EncodedAllSize);
                writer.Write(this.unknown1);
                writer.Write(this.unknown2);
                writer.Write(this.EncodedBodySize);
                writer.Write(this.DecodedBodySize);
            }
        }

        private class Chapter : IBinaryReadable
        {
            public Chapter()
            {
            }

            public Chapter(Chapter ch)
            {
                this.Signature = ch.Signature;
                this.Version = ch.Version;
                this.Checksum = ch.Checksum;
                this.Size = ch.Size;
            }

            public string Signature { get; private set; }

            public ushort Version { get; private set; }

            public uint Checksum { get; private set; }

            public int Size { get; private set; }

            public virtual void ReadFrom(BinaryReader reader)
            {
                this.Signature = Encoding.Default.GetString(reader.ReadBytes(2));
                this.Version = reader.ReadUInt16();
                this.Checksum = reader.ReadUInt32();
                this.Size = reader.ReadInt32();
            }
        }

        private class ClearData : Chapter   // per character
        {
            public ClearData(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "CR")
                    throw new InvalidDataException("Signature");
                if (this.Version != 0x0000)
                    throw new InvalidDataException("Version");
                if (this.Size != 0x000068D4)
                    throw new InvalidDataException("Size");

                var numLevels = Enum.GetValues(typeof(Level)).Length;
                var numPairs = Utils.GetEnumerator<Level>().Where(lv => lv != Level.Extra).Count() *
                    Utils.GetEnumerator<Stage>().Where(st => st != Stage.Extra).Count();
                this.Rankings = new Dictionary<Level, ScoreData[]>(numLevels);
                this.ClearCounts = new Dictionary<Level, int>(numLevels);
                this.Practices = new Dictionary<LevelStagePair, Practice>(numPairs);
                this.Cards = new Dictionary<int, SpellCard>(CardTable.Count);
            }

            public CharaWithTotal Chara { get; private set; }   // size: 4Bytes

            public Dictionary<Level, ScoreData[]> Rankings { get; private set; }

            public int TotalPlayCount { get; private set; }

            public int PlayTime { get; private set; }           // = seconds * 60fps

            public Dictionary<Level, int> ClearCounts { get; private set; }

            public Dictionary<LevelStagePair, Practice> Practices { get; private set; }

            public Dictionary<int, SpellCard> Cards { get; private set; }

            public override void ReadFrom(BinaryReader reader)
            {
                var levels = Utils.GetEnumerator<Level>();
                var stages = Utils.GetEnumerator<Stage>();

                this.Chara = (CharaWithTotal)reader.ReadInt32();

                foreach (var level in levels)
                {
                    if (!this.Rankings.ContainsKey(level))
                        this.Rankings.Add(level, new ScoreData[10]);
                    for (var rank = 0; rank < 10; rank++)
                    {
                        var score = new ScoreData();
                        score.ReadFrom(reader);
                        this.Rankings[level][rank] = score;
                    }
                }

                this.TotalPlayCount = reader.ReadInt32();
                this.PlayTime = reader.ReadInt32();

                foreach (var level in levels)
                {
                    var clearCount = reader.ReadInt32();
                    if (!this.ClearCounts.ContainsKey(level))
                        this.ClearCounts.Add(level, clearCount);
                }

                foreach (var level in levels.Where(lv => lv != Level.Extra))
                    foreach (var stage in stages.Where(st => st != Stage.Extra))
                    {
                        var practice = new Practice();
                        practice.ReadFrom(reader);
                        var key = new LevelStagePair(level, stage);
                        if (!this.Practices.ContainsKey(key))
                            this.Practices.Add(key, practice);
                    }

                for (var number = 0; number < CardTable.Count; number++)
                {
                    var card = new SpellCard();
                    card.ReadFrom(reader);
                    if (!this.Cards.ContainsKey(card.Number))
                        this.Cards.Add(card.Number, card);
                }
            }
        }

        private class Status : Chapter
        {
            public Status(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "ST")
                    throw new InvalidDataException("Signature");
                if (this.Version != 0x0000)
                    throw new InvalidDataException("Version");
                if (this.Size != 0x00000448)
                    throw new InvalidDataException("Size");
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] LastName { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] BgmFlags { get; private set; }    // .Length = 17

            public override void ReadFrom(BinaryReader reader)
            {
                this.LastName = reader.ReadBytes(10);
                reader.ReadBytes(0x10);
                this.BgmFlags = reader.ReadBytes(17);
                reader.ReadBytes(0x0411);
            }
        }

        private class ScoreData : IBinaryReadable
        {
            public uint Score { get; private set; }     // * 10

            public StageProgress StageProgress { get; private set; }    // size: 1Byte

            public byte ContinueCount { get; private set; }

            public byte[] Name { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)

            public uint DateTime { get; private set; }  // UNIX time (unit: [s])

            public float SlowRate { get; private set; } // really...?

            public void ReadFrom(BinaryReader reader)
            {
                this.Score = reader.ReadUInt32();
                this.StageProgress = (StageProgress)reader.ReadByte();
                this.ContinueCount = reader.ReadByte();
                this.Name = reader.ReadBytes(10);
                this.DateTime = reader.ReadUInt32();
                this.SlowRate = reader.ReadSingle();
                reader.ReadUInt32();
            }
        }

        private class SpellCard : IBinaryReadable
        {
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] Name { get; private set; }    // .Length = 0x80

            public int ClearCount { get; private set; }

            public int TrialCount { get; private set; }

            public int Number { get; private set; }     // 1-based

            public Level Level { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.Name = reader.ReadBytes(0x80);
                this.ClearCount = reader.ReadInt32();
                this.TrialCount = reader.ReadInt32();
                this.Number = reader.ReadInt32() + 1;
                this.Level = (Level)reader.ReadInt32();
            }

            public bool HasTried()
            {
                return this.TrialCount > 0;
            }
        }

        private class Practice : IBinaryReadable
        {
            public uint Score { get; private set; }     // * 10

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public uint StageFlag { get; private set; } // 0x00000000: disable, 0x00000101: enable ?

            public void ReadFrom(BinaryReader reader)
            {
                this.Score = reader.ReadUInt32();
                this.StageFlag = reader.ReadUInt32();
            }
        }
    }
}
