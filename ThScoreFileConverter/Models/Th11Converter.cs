//-----------------------------------------------------------------------
// <copyright file="Th11Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591
#pragma warning disable SA1600 // ElementsMustBeDocumented
#pragma warning disable SA1602 // EnumerationItemsMustBeDocumented

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed.")]

namespace ThScoreFileConverter.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using ThScoreFileConverter.Properties;
    using CardInfo = SpellCardInfo<ThConverter.Stage, ThConverter.Level>;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th11Converter : ThConverter
    {
        // Thanks to thwiki.info
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        private static readonly Dictionary<int, CardInfo> CardTable =
            new List<CardInfo>()
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
                new CardInfo(175, "「サブタレイニアンローズ」",           Stage.Extra, Level.Extra),
            }.ToDictionary(card => card.Id);

        private static readonly EnumShortNameParser<Chara> CharaParser =
            new EnumShortNameParser<Chara>();

        private static readonly EnumShortNameParser<CharaWithTotal> CharaWithTotalParser =
            new EnumShortNameParser<CharaWithTotal>();

        private AllScoreData allScoreData = null;

        public enum Chara
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RY")] ReimuYukari,
            [EnumAltName("RS")] ReimuSuika,
            [EnumAltName("RA")] ReimuAya,
            [EnumAltName("MA")] MarisaAlice,
            [EnumAltName("MP")] MarisaPatchouli,
            [EnumAltName("MN")] MarisaNitori,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum CharaWithTotal
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RY")] ReimuYukari,
            [EnumAltName("RS")] ReimuSuika,
            [EnumAltName("RA")] ReimuAya,
            [EnumAltName("MA")] MarisaAlice,
            [EnumAltName("MP")] MarisaPatchouli,
            [EnumAltName("MN")] MarisaNitori,
            [EnumAltName("TL")] Total,
#pragma warning restore SA1134 // Attributes should not share line
        }

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        public enum StageProgress
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("-------")]     None,
            [EnumAltName("Stage 1")]     St1,
            [EnumAltName("Stage 2")]     St2,
            [EnumAltName("Stage 3")]     St3,
            [EnumAltName("Stage 4")]     St4,
            [EnumAltName("Stage 5")]     St5,
            [EnumAltName("Stage 6")]     St6,
            [EnumAltName("Extra Stage")] Extra,
            [EnumAltName("All Clear")]   Clear,
#pragma warning restore SA1134 // Attributes should not share line
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

        protected override IEnumerable<IStringReplaceable> CreateReplacers(
            bool hideUntriedCards, string outputFilePath)
        {
            return new List<IStringReplaceable>
            {
                new ScoreReplacer(this),
                new CareerReplacer(this),
                new CardReplacer(this, hideUntriedCards),
                new CollectRateReplacer(this),
                new ClearReplacer(this),
                new CharaReplacer(this),
                new CharaExReplacer(this),
                new PracticeReplacer(this),
            };
        }

        private static bool Decrypt(Stream input, Stream output)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            using (var writer = new BinaryWriter(output, Encoding.UTF8, true))
            {
                var header = new Header();
                header.ReadFrom(reader);
                if (!header.IsValid)
                    return false;
                if (header.EncodedAllSize != reader.BaseStream.Length)
                    return false;

                header.WriteTo(writer);
                ThCrypt.Decrypt(input, output, header.EncodedBodySize, 0xAC, 0x35, 0x10, header.EncodedBodySize);

                return true;
            }
        }

        private static bool Extract(Stream input, Stream output)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            using (var writer = new BinaryWriter(output, Encoding.UTF8, true))
            {
                var header = new Header();
                header.ReadFrom(reader);
                header.WriteTo(writer);

                var bodyBeginPos = output.Position;
                Lzss.Extract(input, output);
                output.Flush();
                output.SetLength(output.Position);

                return header.DecodedBodySize == (output.Position - bodyBeginPos);
            }
        }

        private static bool Validate(Stream input)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var header = new Header();
                header.ReadFrom(reader);
                var remainSize = header.DecodedBodySize;
                var chapter = new Th10.Chapter();

                try
                {
                    while (remainSize > 0)
                    {
                        chapter.ReadFrom(reader);
                        if (!chapter.IsValid)
                            return false;
                        if (!ClearData.CanInitialize(chapter) && !Status.CanInitialize(chapter))
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
        }

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        private static AllScoreData Read(Stream input)
        {
            var dictionary = new Dictionary<string, Action<AllScoreData, Th10.Chapter>>
            {
                { ClearData.ValidSignature, (data, ch) => data.Set(new ClearData(ch)) },
                { Status.ValidSignature,    (data, ch) => data.Set(new Status(ch))    },
            };

            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var allScoreData = new AllScoreData();
                var chapter = new Th10.Chapter();

                var header = new Header();
                header.ReadFrom(reader);
                allScoreData.Set(header);

                try
                {
                    while (true)
                    {
                        chapter.ReadFrom(reader);
                        if (dictionary.TryGetValue(chapter.Signature, out Action<AllScoreData, Th10.Chapter> setChapter))
                            setChapter(allScoreData, chapter);
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
        }

        // %T11SCR[w][xx][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T11SCR({0})({1})(\d)([1-5])", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th11Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = (CharaWithTotal)CharaParser.Parse(match.Groups[2].Value);
                    var rank = Utils.ToZeroBased(
                        int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    var ranking = parent.allScoreData.ClearData[chara].Rankings[level][rank];
                    switch (type)
                    {
                        case 1:     // name
                            return Encoding.Default.GetString(ranking.Name).Split('\0')[0];
                        case 2:     // score
                            return Utils.ToNumberString((ranking.Score * 10) + ranking.ContinueCount);
                        case 3:     // stage
                            if (ranking.DateTime == 0)
                                return StageProgress.None.ToShortName();
                            if (ranking.StageProgress == StageProgress.Extra)
                                return "Not Clear";
                            return ranking.StageProgress.ToShortName();
                        case 4:     // date & time
                            if (ranking.DateTime == 0)
                                return "----/--/-- --:--:--";
                            return new DateTime(1970, 1, 1).AddSeconds(ranking.DateTime).ToLocalTime()
                                .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                        case 5:     // slow
                            if (ranking.DateTime == 0)
                                return "-----%";
                            return Utils.Format("{0:F3}%", ranking.SlowRate);
                        default:    // unreachable
                            return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T11C[xxx][yy][z]
        private class CareerReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T11C(\d{{3}})({0})([12])", CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(Th11Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var chara = CharaWithTotalParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    Func<SpellCard, int> getCount;
                    if (type == 1)
                        getCount = (card => card.ClearCount);
                    else
                        getCount = (card => card.TrialCount);

                    var cards = parent.allScoreData.ClearData[chara].Cards;
                    if (number == 0)
                    {
                        return Utils.ToNumberString(cards.Values.Sum(getCount));
                    }
                    else if (CardTable.ContainsKey(number))
                    {
                        if (cards.TryGetValue(number, out SpellCard card))
                            return Utils.ToNumberString(getCount(card));
                        else
                            return "0";
                    }
                    else
                    {
                        return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T11CARD[xxx][y]
        private class CardReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T11CARD(\d{3})([NR])";

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th11Converter parent, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var type = match.Groups[2].Value.ToUpperInvariant();

                    if (CardTable.ContainsKey(number))
                    {
                        if (type == "N")
                        {
                            if (hideUntriedCards)
                            {
                                var cards = parent.allScoreData.ClearData[CharaWithTotal.Total].Cards;
                                if (!cards.TryGetValue(number, out SpellCard card) || !card.HasTried())
                                    return "??????????";
                            }

                            return CardTable[number].Name;
                        }
                        else
                        {
                            return CardTable[number].Level.ToString();
                        }
                    }
                    else
                    {
                        return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T11CRG[w][xx][y][z]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T11CRG({0})({1})({2})([12])",
                LevelWithTotalParser.Pattern,
                CharaWithTotalParser.Pattern,
                StageWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(Th11Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var chara = CharaWithTotalParser.Parse(match.Groups[2].Value);
                    var stage = StageWithTotalParser.Parse(match.Groups[3].Value);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    if (stage == StageWithTotal.Extra)
                        return match.ToString();

                    Func<SpellCard, bool> findByStage;
                    if (stage == StageWithTotal.Total)
                        findByStage = (card => true);
                    else
                        findByStage = (card => CardTable[card.Id].Stage == (Stage)stage);

                    Func<SpellCard, bool> findByLevel = (card => true);
                    switch (level)
                    {
                        case LevelWithTotal.Total:
                            // Do nothing
                            break;
                        case LevelWithTotal.Extra:
                            findByStage = (card => CardTable[card.Id].Stage == Stage.Extra);
                            break;
                        default:
                            findByLevel = (card => card.Level == (Level)level);
                            break;
                    }

                    Func<SpellCard, bool> findByType;
                    if (type == 1)
                        findByType = (card => card.ClearCount > 0);
                    else
                        findByType = (card => card.TrialCount > 0);

                    return parent.allScoreData.ClearData[chara].Cards.Values
                        .Count(Utils.MakeAndPredicate(findByLevel, findByStage, findByType))
                        .ToString(CultureInfo.CurrentCulture);
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T11CLEAR[x][yy]
        private class ClearReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T11CLEAR({0})({1})", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ClearReplacer(Th11Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = (CharaWithTotal)CharaParser.Parse(match.Groups[2].Value);

                    var rankings = parent.allScoreData.ClearData[chara].Rankings[level]
                        .Where(ranking => ranking.DateTime > 0);
                    var stageProgress = (rankings.Count() > 0)
                        ? rankings.Max(ranking => ranking.StageProgress) : StageProgress.None;

                    return (stageProgress == StageProgress.Extra)
                        ? "Not Clear" : stageProgress.ToShortName();
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T11CHARA[xx][y]
        private class CharaReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T11CHARA({0})([1-3])", CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CharaReplacer(Th11Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var chara = CharaWithTotalParser.Parse(match.Groups[1].Value);
                    var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                    Func<ClearData, long> getValueByType;
                    Func<long, string> toString;
                    if (type == 1)
                    {
                        getValueByType = (data => data.TotalPlayCount);
                        toString = Utils.ToNumberString;
                    }
                    else if (type == 2)
                    {
                        getValueByType = (data => data.PlayTime);
                        toString = (value => new Time(value).ToString());
                    }
                    else
                    {
                        getValueByType = (data => data.ClearCounts.Values.Sum());
                        toString = Utils.ToNumberString;
                    }

                    Func<AllScoreData, long> getValueByChara;
                    if (chara == CharaWithTotal.Total)
                    {
                        getValueByChara = (allData => allData.ClearData.Values
                            .Where(data => data.Chara != chara).Sum(getValueByType));
                    }
                    else
                    {
                        getValueByChara = (allData => getValueByType(allData.ClearData[chara]));
                    }

                    return toString(getValueByChara(parent.allScoreData));
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T11CHARAEX[x][yy][z]
        private class CharaExReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T11CHARAEX({0})({1})([1-3])", LevelWithTotalParser.Pattern, CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CharaExReplacer(Th11Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var chara = CharaWithTotalParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    Func<ClearData, long> getValueByType;
                    Func<long, string> toString;
                    if (type == 1)
                    {
                        getValueByType = (data => data.TotalPlayCount);
                        toString = Utils.ToNumberString;
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
                        toString = Utils.ToNumberString;
                    }

                    Func<AllScoreData, long> getValueByChara;
                    if (chara == CharaWithTotal.Total)
                    {
                        getValueByChara = (allData => allData.ClearData.Values
                            .Where(data => data.Chara != chara).Sum(getValueByType));
                    }
                    else
                    {
                        getValueByChara = (allData => getValueByType(allData.ClearData[chara]));
                    }

                    return toString(getValueByChara(parent.allScoreData));
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T11PRAC[x][yy][z]
        private class PracticeReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T11PRAC({0})({1})({2})", LevelParser.Pattern, CharaParser.Pattern, StageParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public PracticeReplacer(Th11Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = (CharaWithTotal)CharaParser.Parse(match.Groups[2].Value);
                    var stage = StageParser.Parse(match.Groups[3].Value);

                    if (level == Level.Extra)
                        return match.ToString();
                    if (stage == Stage.Extra)
                        return match.ToString();

                    if (parent.allScoreData.ClearData.ContainsKey(chara))
                    {
                        var key = new LevelStagePair(level, stage);
                        var practices = parent.allScoreData.ClearData[chara].Practices;
                        return practices.ContainsKey(key)
                            ? Utils.ToNumberString(practices[key].Score * 10) : "0";
                    }
                    else
                    {
                        return "0";
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
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

            public Header Header { get; private set; }

            public Dictionary<CharaWithTotal, ClearData> ClearData { get; private set; }

            public Status Status { get; private set; }

            public void Set(Header header)
            {
                this.Header = header;
            }

            public void Set(ClearData data)
            {
                if (!this.ClearData.ContainsKey(data.Chara))
                    this.ClearData.Add(data.Chara, data);
            }

            public void Set(Status status)
            {
                this.Status = status;
            }
        }

        private class Header : Th095.Header
        {
            public const string ValidSignature = "TH11";

            public override bool IsValid
                => base.IsValid && this.Signature.Equals(ValidSignature, StringComparison.Ordinal);
        }

        private class ClearData : Th10.Chapter   // per character
        {
            public const string ValidSignature = "CR";
            public const ushort ValidVersion = 0x0000;
            public const int ValidSize = 0x000068D4;

            public ClearData(Th10.Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                {
                    throw new InvalidDataException(
                        Utils.Format(Resources.InvalidDataExceptionPropertyIsInvalid, nameof(this.Signature)));
                }

                if (this.Version != ValidVersion)
                {
                    throw new InvalidDataException(
                        Utils.Format(Resources.InvalidDataExceptionPropertyIsInvalid, nameof(this.Version)));
                }

                if (this.Size != ValidSize)
                {
                    throw new InvalidDataException(
                        Utils.Format(Resources.InvalidDataExceptionPropertyIsInvalid, nameof(this.Size)));
                }

                var levels = Utils.GetEnumerator<Level>();
                var levelsExceptExtra = levels.Where(lv => lv != Level.Extra);
                var stages = Utils.GetEnumerator<Stage>();
                var stagesExceptExtra = stages.Where(st => st != Stage.Extra);
                var numLevels = levels.Count();
                var numPairs = levelsExceptExtra.Count() * stagesExceptExtra.Count();

                this.Rankings = new Dictionary<Level, ScoreData[]>(numLevels);
                this.ClearCounts = new Dictionary<Level, int>(numLevels);
                this.Practices = new Dictionary<LevelStagePair, Practice>(numPairs);
                this.Cards = new Dictionary<int, SpellCard>(CardTable.Count);

                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
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

                    foreach (var level in levelsExceptExtra)
                    {
                        foreach (var stage in stagesExceptExtra)
                        {
                            var practice = new Practice();
                            practice.ReadFrom(reader);
                            var key = new LevelStagePair(level, stage);
                            if (!this.Practices.ContainsKey(key))
                                this.Practices.Add(key, practice);
                        }
                    }

                    for (var number = 0; number < CardTable.Count; number++)
                    {
                        var card = new SpellCard();
                        card.ReadFrom(reader);
                        if (!this.Cards.ContainsKey(card.Id))
                            this.Cards.Add(card.Id, card);
                    }
                }
            }

            public CharaWithTotal Chara { get; private set; }   // size: 4Bytes

            public Dictionary<Level, ScoreData[]> Rankings { get; private set; }

            public int TotalPlayCount { get; private set; }

            public int PlayTime { get; private set; }           // = seconds * 60fps

            public Dictionary<Level, int> ClearCounts { get; private set; }

            public Dictionary<LevelStagePair, Practice> Practices { get; private set; }

            public Dictionary<int, SpellCard> Cards { get; private set; }

            public static bool CanInitialize(Th10.Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class Status : Th10.Chapter
        {
            public const string ValidSignature = "ST";
            public const ushort ValidVersion = 0x0000;
            public const int ValidSize = 0x00000448;

            public Status(Th10.Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                {
                    throw new InvalidDataException(
                        Utils.Format(Resources.InvalidDataExceptionPropertyIsInvalid, nameof(this.Signature)));
                }

                if (this.Version != ValidVersion)
                {
                    throw new InvalidDataException(
                        Utils.Format(Resources.InvalidDataExceptionPropertyIsInvalid, nameof(this.Version)));
                }

                if (this.Size != ValidSize)
                {
                    throw new InvalidDataException(
                        Utils.Format(Resources.InvalidDataExceptionPropertyIsInvalid, nameof(this.Size)));
                }

                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    this.LastName = reader.ReadExactBytes(10);
                    reader.ReadExactBytes(0x10);
                    this.BgmFlags = reader.ReadExactBytes(17);
                    reader.ReadExactBytes(0x0411);
                }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] LastName { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] BgmFlags { get; private set; }    // .Length = 17

            public static bool CanInitialize(Th10.Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
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
                if (reader is null)
                    throw new ArgumentNullException(nameof(reader));

                this.Score = reader.ReadUInt32();
                this.StageProgress = Utils.ToEnum<StageProgress>(reader.ReadByte());
                this.ContinueCount = reader.ReadByte();
                this.Name = reader.ReadExactBytes(10);
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

            public int Id { get; private set; }         // 1-based

            public Level Level { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader is null)
                    throw new ArgumentNullException(nameof(reader));

                this.Name = reader.ReadExactBytes(0x80);
                this.ClearCount = reader.ReadInt32();
                this.TrialCount = reader.ReadInt32();
                this.Id = reader.ReadInt32() + 1;
                this.Level = Utils.ToEnum<Level>(reader.ReadInt32());
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
                if (reader == null)
                    throw new ArgumentNullException(nameof(reader));

                this.Score = reader.ReadUInt32();
                this.StageFlag = reader.ReadUInt32();
            }
        }
    }
}
