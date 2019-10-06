//-----------------------------------------------------------------------
// <copyright file="Th13Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591
#pragma warning disable SA1600 // ElementsMustBeDocumented
#pragma warning disable SA1602 // EnumerationItemsMustBeDocumented

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Models.Th13;
using CardInfo = ThScoreFileConverter.Models.SpellCardInfo<
    ThScoreFileConverter.Models.Th13Converter.StagePractice, ThScoreFileConverter.Models.Th13Converter.LevelPractice>;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th13Converter : ThConverter
    {
        // Thanks to thwiki.info
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        private static readonly Dictionary<int, CardInfo> CardTable =
            new List<CardInfo>()
            {
                new CardInfo(  1, "符牒「死蝶の舞」",                     StagePractice.St1,       LevelPractice.Easy),
                new CardInfo(  2, "符牒「死蝶の舞」",                     StagePractice.St1,       LevelPractice.Normal),
                new CardInfo(  3, "符牒「死蝶の舞　- 桜花 -」",           StagePractice.St1,       LevelPractice.Hard),
                new CardInfo(  4, "符牒「死蝶の舞　- 桜花 -」",           StagePractice.St1,       LevelPractice.Lunatic),
                new CardInfo(  5, "幽蝶「ゴーストスポット」",             StagePractice.St1,       LevelPractice.Easy),
                new CardInfo(  6, "幽蝶「ゴーストスポット」",             StagePractice.St1,       LevelPractice.Normal),
                new CardInfo(  7, "幽蝶「ゴーストスポット　- 桜花 -」",   StagePractice.St1,       LevelPractice.Hard),
                new CardInfo(  8, "幽蝶「ゴーストスポット　- 桜花 -」",   StagePractice.St1,       LevelPractice.Lunatic),
                new CardInfo(  9, "冥符「常夜桜」",                       StagePractice.St1,       LevelPractice.Easy),
                new CardInfo( 10, "冥符「常夜桜」",                       StagePractice.St1,       LevelPractice.Normal),
                new CardInfo( 11, "冥符「常夜桜」",                       StagePractice.St1,       LevelPractice.Hard),
                new CardInfo( 12, "冥符「常夜桜」",                       StagePractice.St1,       LevelPractice.Lunatic),
                new CardInfo( 13, "桜符「西行桜吹雪」",                   StagePractice.St1,       LevelPractice.Hard),
                new CardInfo( 14, "桜符「西行桜吹雪」",                   StagePractice.St1,       LevelPractice.Lunatic),
                new CardInfo( 15, "響符「マウンテンエコー」",             StagePractice.St2,       LevelPractice.Easy),
                new CardInfo( 16, "響符「マウンテンエコー」",             StagePractice.St2,       LevelPractice.Normal),
                new CardInfo( 17, "響符「マウンテンエコースクランブル」", StagePractice.St2,       LevelPractice.Hard),
                new CardInfo( 18, "響符「マウンテンエコースクランブル」", StagePractice.St2,       LevelPractice.Lunatic),
                new CardInfo( 19, "響符「パワーレゾナンス」",             StagePractice.St2,       LevelPractice.Easy),
                new CardInfo( 20, "響符「パワーレゾナンス」",             StagePractice.St2,       LevelPractice.Normal),
                new CardInfo( 21, "響符「パワーレゾナンス」",             StagePractice.St2,       LevelPractice.Hard),
                new CardInfo( 22, "響符「パワーレゾナンス」",             StagePractice.St2,       LevelPractice.Lunatic),
                new CardInfo( 23, "山彦「ロングレンジエコー」",           StagePractice.St2,       LevelPractice.Easy),
                new CardInfo( 24, "山彦「ロングレンジエコー」",           StagePractice.St2,       LevelPractice.Normal),
                new CardInfo( 25, "山彦「アンプリファイエコー」",         StagePractice.St2,       LevelPractice.Hard),
                new CardInfo( 26, "山彦「アンプリファイエコー」",         StagePractice.St2,       LevelPractice.Lunatic),
                new CardInfo( 27, "大声「チャージドクライ」",             StagePractice.St2,       LevelPractice.Easy),
                new CardInfo( 28, "大声「チャージドクライ」",             StagePractice.St2,       LevelPractice.Normal),
                new CardInfo( 29, "大声「チャージドヤッホー」",           StagePractice.St2,       LevelPractice.Hard),
                new CardInfo( 30, "大声「チャージドヤッホー」",           StagePractice.St2,       LevelPractice.Lunatic),
                new CardInfo( 31, "虹符「アンブレラサイクロン」",         StagePractice.St3,       LevelPractice.Hard),
                new CardInfo( 32, "虹符「アンブレラサイクロン」",         StagePractice.St3,       LevelPractice.Lunatic),
                new CardInfo( 33, "回復「ヒールバイデザイア」",           StagePractice.St3,       LevelPractice.Easy),
                new CardInfo( 34, "回復「ヒールバイデザイア」",           StagePractice.St3,       LevelPractice.Normal),
                new CardInfo( 35, "回復「ヒールバイデザイア」",           StagePractice.St3,       LevelPractice.Hard),
                new CardInfo( 36, "回復「ヒールバイデザイア」",           StagePractice.St3,       LevelPractice.Lunatic),
                new CardInfo( 37, "毒爪「ポイズンレイズ」",               StagePractice.St3,       LevelPractice.Easy),
                new CardInfo( 38, "毒爪「ポイズンレイズ」",               StagePractice.St3,       LevelPractice.Normal),
                new CardInfo( 39, "毒爪「ポイズンマーダー」",             StagePractice.St3,       LevelPractice.Hard),
                new CardInfo( 40, "毒爪「ポイズンマーダー」",             StagePractice.St3,       LevelPractice.Lunatic),
                new CardInfo( 41, "欲符「稼欲霊招来」",                   StagePractice.St3,       LevelPractice.Easy),
                new CardInfo( 42, "欲符「稼欲霊招来」",                   StagePractice.St3,       LevelPractice.Normal),
                new CardInfo( 43, "欲霊「スコアデザイアイーター」",       StagePractice.St3,       LevelPractice.Hard),
                new CardInfo( 44, "欲霊「スコアデザイアイーター」",       StagePractice.St3,       LevelPractice.Lunatic),
                new CardInfo( 45, "邪符「ヤンシャオグイ」",               StagePractice.St4,       LevelPractice.Normal),
                new CardInfo( 46, "邪符「グーフンイエグイ」",             StagePractice.St4,       LevelPractice.Hard),
                new CardInfo( 47, "邪符「グーフンイエグイ」",             StagePractice.St4,       LevelPractice.Lunatic),
                new CardInfo( 48, "入魔「ゾウフォルゥモォ」",             StagePractice.St4,       LevelPractice.Easy),
                new CardInfo( 49, "入魔「ゾウフォルゥモォ」",             StagePractice.St4,       LevelPractice.Normal),
                new CardInfo( 50, "入魔「ゾウフォルゥモォ」",             StagePractice.St4,       LevelPractice.Hard),
                new CardInfo( 51, "入魔「ゾウフォルゥモォ」",             StagePractice.St4,       LevelPractice.Lunatic),
                new CardInfo( 52, "降霊「死人タンキー」",                 StagePractice.St4,       LevelPractice.Easy),
                new CardInfo( 53, "降霊「死人タンキー」",                 StagePractice.St4,       LevelPractice.Normal),
                new CardInfo( 54, "通霊「トンリン芳香」",                 StagePractice.St4,       LevelPractice.Hard),
                new CardInfo( 55, "通霊「トンリン芳香」",                 StagePractice.St4,       LevelPractice.Lunatic),
                new CardInfo( 56, "道符「タオ胎動」",                     StagePractice.St4,       LevelPractice.Easy),
                new CardInfo( 57, "道符「タオ胎動」",                     StagePractice.St4,       LevelPractice.Normal),
                new CardInfo( 58, "道符「タオ胎動」",                     StagePractice.St4,       LevelPractice.Hard),
                new CardInfo( 59, "道符「タオ胎動」",                     StagePractice.St4,       LevelPractice.Lunatic),
                new CardInfo( 60, "雷矢「ガゴウジサイクロン」",           StagePractice.St5,       LevelPractice.Normal),
                new CardInfo( 61, "雷矢「ガゴウジサイクロン」",           StagePractice.St5,       LevelPractice.Hard),
                new CardInfo( 62, "雷矢「ガゴウジトルネード」",           StagePractice.St5,       LevelPractice.Lunatic),
                new CardInfo( 63, "天符「雨の磐舟」",                     StagePractice.St5,       LevelPractice.Easy),
                new CardInfo( 64, "天符「雨の磐舟」",                     StagePractice.St5,       LevelPractice.Normal),
                new CardInfo( 65, "天符「天の磐舟よ天へ昇れ」",           StagePractice.St5,       LevelPractice.Hard),
                new CardInfo( 66, "天符「天の磐舟よ天へ昇れ」",           StagePractice.St5,       LevelPractice.Lunatic),
                new CardInfo( 67, "投皿「物部の八十平瓮」",               StagePractice.St5,       LevelPractice.Easy),
                new CardInfo( 68, "投皿「物部の八十平瓮」",               StagePractice.St5,       LevelPractice.Normal),
                new CardInfo( 69, "投皿「物部の八十平瓮」",               StagePractice.St5,       LevelPractice.Hard),
                new CardInfo( 70, "投皿「物部の八十平瓮」",               StagePractice.St5,       LevelPractice.Lunatic),
                new CardInfo( 71, "炎符「廃仏の炎風」",                   StagePractice.St5,       LevelPractice.Easy),
                new CardInfo( 72, "炎符「廃仏の炎風」",                   StagePractice.St5,       LevelPractice.Normal),
                new CardInfo( 73, "炎符「桜井寺炎上」",                   StagePractice.St5,       LevelPractice.Hard),
                new CardInfo( 74, "炎符「桜井寺炎上」",                   StagePractice.St5,       LevelPractice.Lunatic),
                new CardInfo( 75, "聖童女「大物忌正餐」",                 StagePractice.St5,       LevelPractice.Easy),
                new CardInfo( 76, "聖童女「大物忌正餐」",                 StagePractice.St5,       LevelPractice.Normal),
                new CardInfo( 77, "聖童女「大物忌正餐」",                 StagePractice.St5,       LevelPractice.Hard),
                new CardInfo( 78, "聖童女「大物忌正餐」",                 StagePractice.St5,       LevelPractice.Lunatic),
                new CardInfo( 79, "名誉「十二階の色彩」",                 StagePractice.St6,       LevelPractice.Easy),
                new CardInfo( 80, "名誉「十二階の色彩」",                 StagePractice.St6,       LevelPractice.Normal),
                new CardInfo( 81, "名誉「十二階の冠位」",                 StagePractice.St6,       LevelPractice.Hard),
                new CardInfo( 82, "名誉「十二階の冠位」",                 StagePractice.St6,       LevelPractice.Lunatic),
                new CardInfo( 83, "仙符「日出ずる処の道士」",             StagePractice.St6,       LevelPractice.Easy),
                new CardInfo( 84, "仙符「日出ずる処の道士」",             StagePractice.St6,       LevelPractice.Normal),
                new CardInfo( 85, "仙符「日出ずる処の天子」",             StagePractice.St6,       LevelPractice.Hard),
                new CardInfo( 86, "仙符「日出ずる処の天子」",             StagePractice.St6,       LevelPractice.Lunatic),
                new CardInfo( 87, "召喚「豪族乱舞」",                     StagePractice.St6,       LevelPractice.Easy),
                new CardInfo( 88, "召喚「豪族乱舞」",                     StagePractice.St6,       LevelPractice.Normal),
                new CardInfo( 89, "召喚「豪族乱舞」",                     StagePractice.St6,       LevelPractice.Hard),
                new CardInfo( 90, "召喚「豪族乱舞」",                     StagePractice.St6,       LevelPractice.Lunatic),
                new CardInfo( 91, "秘宝「斑鳩寺の天球儀」",               StagePractice.St6,       LevelPractice.Easy),
                new CardInfo( 92, "秘宝「斑鳩寺の天球儀」",               StagePractice.St6,       LevelPractice.Normal),
                new CardInfo( 93, "秘宝「斑鳩寺の天球儀」",               StagePractice.St6,       LevelPractice.Hard),
                new CardInfo( 94, "秘宝「聖徳太子のオーパーツ」",         StagePractice.St6,       LevelPractice.Lunatic),
                new CardInfo( 95, "光符「救世観音の光後光」",             StagePractice.St6,       LevelPractice.Easy),
                new CardInfo( 96, "光符「救世観音の光後光」",             StagePractice.St6,       LevelPractice.Normal),
                new CardInfo( 97, "光符「グセフラッシュ」",               StagePractice.St6,       LevelPractice.Hard),
                new CardInfo( 98, "光符「グセフラッシュ」",               StagePractice.St6,       LevelPractice.Lunatic),
                new CardInfo( 99, "眼光「十七条のレーザー」",             StagePractice.St6,       LevelPractice.Easy),
                new CardInfo(100, "眼光「十七条のレーザー」",             StagePractice.St6,       LevelPractice.Normal),
                new CardInfo(101, "神光「逆らう事なきを宗とせよ」",       StagePractice.St6,       LevelPractice.Hard),
                new CardInfo(102, "神光「逆らう事なきを宗とせよ」",       StagePractice.St6,       LevelPractice.Lunatic),
                new CardInfo(103, "「星降る神霊廟」",                     StagePractice.St6,       LevelPractice.Easy),
                new CardInfo(104, "「星降る神霊廟」",                     StagePractice.St6,       LevelPractice.Normal),
                new CardInfo(105, "「生まれたての神霊」",                 StagePractice.St6,       LevelPractice.Hard),
                new CardInfo(106, "「生まれたての神霊」",                 StagePractice.St6,       LevelPractice.Lunatic),
                new CardInfo(107, "アンノウン「軌道不明の鬼火」",         StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(108, "アンノウン「姿態不明の空魚」",         StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(109, "アンノウン「原理不明の妖怪玉」",       StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(110, "壱番勝負「霊長化弾幕変化」",           StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(111, "弐番勝負「肉食化弾幕変化」",           StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(112, "参番勝負「延羽化弾幕変化」",           StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(113, "四番勝負「両生化弾幕変化」",           StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(114, "伍番勝負「鳥獣戯画」",                 StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(115, "六番勝負「狸の化け学校」",             StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(116, "七番勝負「野生の離島」",               StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(117, "変化「まぬけ巫女の偽調伏」",           StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(118, "「マミゾウ化弾幕十変化」",             StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(119, "狢符「満月のポンポコリン」",           StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(120, "桜符「桜吹雪地獄」",                   StagePractice.OverDrive, LevelPractice.OverDrive),
                new CardInfo(121, "山彦「ヤマビコの本領発揮エコー」",     StagePractice.OverDrive, LevelPractice.OverDrive),
                new CardInfo(122, "毒爪「死なない殺人鬼」",               StagePractice.OverDrive, LevelPractice.OverDrive),
                new CardInfo(123, "道符「ＴＡＯ胎動　～道～」",           StagePractice.OverDrive, LevelPractice.OverDrive),
                new CardInfo(124, "怨霊「入鹿の雷」",                     StagePractice.OverDrive, LevelPractice.OverDrive),
                new CardInfo(125, "聖童女「太陽神の贄」",                 StagePractice.OverDrive, LevelPractice.OverDrive),
                new CardInfo(126, "「神霊大宇宙」",                       StagePractice.OverDrive, LevelPractice.OverDrive),
                new CardInfo(127, "「ワイルドカーペット」",               StagePractice.OverDrive, LevelPractice.OverDrive),
            }.ToDictionary(card => card.Id);

        private static readonly EnumShortNameParser<LevelPracticeWithTotal> LevelPracticeWithTotalParser =
            new EnumShortNameParser<LevelPracticeWithTotal>();

        private static readonly EnumShortNameParser<Chara> CharaParser =
            new EnumShortNameParser<Chara>();

        private static readonly EnumShortNameParser<CharaWithTotal> CharaWithTotalParser =
            new EnumShortNameParser<CharaWithTotal>();

        private AllScoreData allScoreData = null;

        public enum LevelPractice
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("D", LongName = "Over Drive")] OverDrive,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum LevelPracticeWithTotal
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("D", LongName = "Over Drive")] OverDrive,
            [EnumAltName("T")] Total,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum Chara
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RM")] Reimu,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("SN")] Sanae,
            [EnumAltName("YM")] Youmu,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum CharaWithTotal
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RM")] Reimu,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("SN")] Sanae,
            [EnumAltName("YM")] Youmu,
            [EnumAltName("TL")] Total,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum StagePractice
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("1")] St1,
            [EnumAltName("2")] St2,
            [EnumAltName("3")] St3,
            [EnumAltName("4")] St4,
            [EnumAltName("5")] St5,
            [EnumAltName("6")] St6,
            [EnumAltName("X")] Extra,
            [EnumAltName("D", LongName = "Over Drive")] OverDrive,
#pragma warning restore SA1134 // Attributes should not share line
        }

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
            [EnumAltName("Extra Clear")] ExtraClear,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public override string SupportedVersions
        {
            get { return "1.00c"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th13decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
                        if (dictionary.TryGetValue(chapter.Signature, out var setChapter))
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

        // %T13SCR[w][xx][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T13SCR({0})({1})(\d)([1-5])", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th13Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = (LevelPracticeWithTotal)LevelParser.Parse(match.Groups[1].Value);
                    var chara = (CharaWithTotal)CharaParser.Parse(match.Groups[2].Value);
                    var rank = Utils.ToZeroBased(
                        int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    var ranking = parent.allScoreData.ClearData[chara].Rankings[level][rank];
                    switch (type)
                    {
                        case 1:     // name
                            return Encoding.Default.GetString(ranking.Name.ToArray()).Split('\0')[0];
                        case 2:     // score
                            return Utils.ToNumberString((ranking.Score * 10) + ranking.ContinueCount);
                        case 3:     // stage
                            if (ranking.DateTime == 0)
                                return StageProgress.None.ToShortName();
                            if (ranking.StageProgress == StageProgress.Extra)
                                return "Not Clear";
                            if (ranking.StageProgress == StageProgress.ExtraClear)
                                return StageProgress.Clear.ToShortName();
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

        // %T13C[w][xxx][yy][z]
        private class CareerReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T13C([SP])(\d{{3}})({0})([12])", CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(Th13Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var kind = match.Groups[1].Value.ToUpperInvariant();
                    var number = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    var chara = CharaWithTotalParser.Parse(match.Groups[3].Value);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    Func<SpellCard, bool> isValidLevel;
                    Func<SpellCard, int> getCount;
                    if (kind == "S")
                    {
                        isValidLevel = (card => card.Level != LevelPractice.OverDrive);
                        if (type == 1)
                            getCount = (card => card.ClearCount);
                        else
                            getCount = (card => card.TrialCount);
                    }
                    else
                    {
                        isValidLevel = (card => true);
                        if (type == 1)
                            getCount = (card => card.PracticeClearCount);
                        else
                            getCount = (card => card.PracticeTrialCount);
                    }

                    var cards = parent.allScoreData.ClearData[chara].Cards;
                    if (number == 0)
                    {
                        return Utils.ToNumberString(cards.Values.Where(isValidLevel).Sum(getCount));
                    }
                    else if (CardTable.ContainsKey(number))
                    {
                        if (cards.TryGetValue(number, out var card))
                        {
                            return isValidLevel(card)
                                ? Utils.ToNumberString(getCount(card)) : match.ToString();
                        }
                        else
                        {
                            return "0";
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

        // %T13CARD[xxx][y]
        private class CardReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T13CARD(\d{3})([NR])";

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th13Converter parent, bool hideUntriedCards)
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
                                if (!cards.TryGetValue(number, out var card) || !card.HasTried)
                                    return "??????????";
                            }

                            return CardTable[number].Name;
                        }
                        else
                        {
                            var level = CardTable[number].Level;
                            var levelName = level.ToLongName();
                            return (levelName.Length > 0) ? levelName : level.ToString();
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

        // %T13CRG[v][w][xx][y][z]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T13CRG([SP])({0})({1})({2})([12])",
                LevelPracticeWithTotalParser.Pattern,
                CharaWithTotalParser.Pattern,
                StageWithTotalParser.Pattern);

            private static readonly Func<SpellCard, string, int, bool> FindByKindTypeImpl =
                (card, kind, type) =>
                {
                    if (kind == "S")
                    {
                        if (type == 1)
                            return (card.Level != LevelPractice.OverDrive) && (card.ClearCount > 0);
                        else
                            return (card.Level != LevelPractice.OverDrive) && (card.TrialCount > 0);
                    }
                    else
                    {
                        if (type == 1)
                            return card.PracticeClearCount > 0;
                        else
                            return card.PracticeTrialCount > 0;
                    }
                };

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(Th13Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var kind = match.Groups[1].Value.ToUpperInvariant();
                    var level = LevelPracticeWithTotalParser.Parse(match.Groups[2].Value);
                    var chara = CharaWithTotalParser.Parse(match.Groups[3].Value);
                    var stage = StageWithTotalParser.Parse(match.Groups[4].Value);
                    var type = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);

                    if (stage == StageWithTotal.Extra)
                        return match.ToString();
                    if ((kind == "S") && (level == LevelPracticeWithTotal.OverDrive))
                        return match.ToString();

                    Func<SpellCard, bool> findByKindType = (card => FindByKindTypeImpl(card, kind, type));

                    Func<SpellCard, bool> findByStage;
                    if (stage == StageWithTotal.Total)
                        findByStage = (card => true);
                    else
                        findByStage = (card => CardTable[card.Id].Stage == (StagePractice)stage);

                    Func<SpellCard, bool> findByLevel = (card => true);
                    switch (level)
                    {
                        case LevelPracticeWithTotal.Total:
                            // Do nothing
                            break;
                        case LevelPracticeWithTotal.Extra:
                            findByStage = (card => CardTable[card.Id].Stage == StagePractice.Extra);
                            break;
                        case LevelPracticeWithTotal.OverDrive:
                            findByStage = (card => CardTable[card.Id].Stage == StagePractice.OverDrive);
                            break;
                        default:
                            findByLevel = (card => card.Level == (LevelPractice)level);
                            break;
                    }

                    return parent.allScoreData.ClearData[chara].Cards.Values
                        .Count(Utils.MakeAndPredicate(findByKindType, findByLevel, findByStage))
                        .ToString(CultureInfo.CurrentCulture);
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T13CLEAR[x][yy]
        private class ClearReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T13CLEAR({0})({1})", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ClearReplacer(Th13Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = (LevelPracticeWithTotal)LevelParser.Parse(match.Groups[1].Value);
                    var chara = (CharaWithTotal)CharaParser.Parse(match.Groups[2].Value);

                    var rankings = parent.allScoreData.ClearData[chara].Rankings[level]
                        .Where(ranking => ranking.DateTime > 0);
                    var stageProgress = rankings.Any()
                        ? rankings.Max(ranking => ranking.StageProgress) : StageProgress.None;

                    if (stageProgress == StageProgress.Extra)
                        return "Not Clear";
                    else if (stageProgress == StageProgress.ExtraClear)
                        return StageProgress.Clear.ToShortName();
                    else
                        return stageProgress.ToShortName();
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T13CHARA[xx][y]
        private class CharaReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T13CHARA({0})([1-3])", CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CharaReplacer(Th13Converter parent)
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

        // %T13CHARAEX[x][yy][z]
        private class CharaExReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T13CHARAEX({0})({1})([1-3])", LevelWithTotalParser.Pattern, CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CharaExReplacer(Th13Converter parent)
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
                            getValueByType = (data => data.ClearCounts[(LevelPracticeWithTotal)level]);
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

        // %T13PRAC[x][yy][z]
        private class PracticeReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T13PRAC({0})({1})({2})", LevelParser.Pattern, CharaParser.Pattern, StageParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public PracticeReplacer(Th13Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = (LevelPractice)LevelParser.Parse(match.Groups[1].Value);
                    var chara = (CharaWithTotal)CharaParser.Parse(match.Groups[2].Value);
                    var stage = (StagePractice)StageParser.Parse(match.Groups[3].Value);

                    if (level == LevelPractice.Extra)
                        return match.ToString();
                    if (stage == StagePractice.Extra)
                        return match.ToString();

                    if (parent.allScoreData.ClearData.ContainsKey(chara))
                    {
                        var key = (level, stage);
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

        private class AllScoreData
        {
            public AllScoreData()
            {
                this.ClearData =
                    new Dictionary<CharaWithTotal, ClearData>(Enum.GetValues(typeof(CharaWithTotal)).Length);
            }

            public Header Header { get; private set; }

            public Dictionary<CharaWithTotal, ClearData> ClearData { get; private set; }

            public Th125.IStatus Status { get; private set; }

            public void Set(Header header) => this.Header = header;

            public void Set(ClearData data)
            {
                if (!this.ClearData.ContainsKey(data.Chara))
                    this.ClearData.Add(data.Chara, data);
            }

            public void Set(Th125.IStatus status) => this.Status = status;
        }

        private class Header : Th095.Header
        {
            public const string ValidSignature = "TH31";

            public override bool IsValid
                => base.IsValid && this.Signature.Equals(ValidSignature, StringComparison.Ordinal);
        }

        private class ClearData : Th10.Chapter   // per character
        {
            public const string ValidSignature = "CR";
            public const ushort ValidVersion = 0x0001;
            public const int ValidSize = 0x000056DC;

            public ClearData(Th10.Chapter chapter)
                : base(chapter, ValidSignature, ValidVersion, ValidSize)
            {
                var levelsWithTotal = Utils.GetEnumerator<LevelPracticeWithTotal>();
                var levels = Utils.GetEnumerator<LevelPractice>();
                var stages = Utils.GetEnumerator<StagePractice>();
                var numLevelsWithTotal = levelsWithTotal.Count();

                this.Rankings = new Dictionary<LevelPracticeWithTotal, ScoreData[]>(numLevelsWithTotal);
                this.ClearCounts = new Dictionary<LevelPracticeWithTotal, int>(numLevelsWithTotal);
                this.ClearFlags = new Dictionary<LevelPracticeWithTotal, int>(numLevelsWithTotal);
                this.Practices =
                    new Dictionary<(LevelPractice, StagePractice), IPractice>(levels.Count() * stages.Count());
                this.Cards = new Dictionary<int, SpellCard>(CardTable.Count);

                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    this.Chara = (CharaWithTotal)reader.ReadInt32();

                    foreach (var level in levelsWithTotal)
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

                    foreach (var level in levelsWithTotal)
                    {
                        var clearCount = reader.ReadInt32();
                        if (!this.ClearCounts.ContainsKey(level))
                            this.ClearCounts.Add(level, clearCount);
                    }

                    foreach (var level in levelsWithTotal)
                    {
                        var clearFlag = reader.ReadInt32();
                        if (!this.ClearFlags.ContainsKey(level))
                            this.ClearFlags.Add(level, clearFlag);
                    }

                    foreach (var level in levels)
                    {
                        foreach (var stage in stages)
                        {
                            var practice = new Practice();
                            practice.ReadFrom(reader);
                            var key = (level, stage);
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

            public CharaWithTotal Chara { get; }

            public Dictionary<LevelPracticeWithTotal, ScoreData[]> Rankings { get; }

            public int TotalPlayCount { get; }

            public int PlayTime { get; }    // = seconds * 60fps

            public Dictionary<LevelPracticeWithTotal, int> ClearCounts { get; }

            public Dictionary<LevelPracticeWithTotal, int> ClearFlags { get; }  // Really...?

            public Dictionary<(LevelPractice, StagePractice), IPractice> Practices { get; }

            public Dictionary<int, SpellCard> Cards { get; }

            public static bool CanInitialize(Th10.Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class Status : Th10.Chapter, Th125.IStatus
        {
            public const string ValidSignature = "ST";
            public const ushort ValidVersion = 0x0001;
            public const int ValidSize = 0x0000042C;

            public Status(Th10.Chapter chapter)
                : base(chapter, ValidSignature, ValidVersion, ValidSize)
            {
                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    this.LastName = reader.ReadExactBytes(10);
                    reader.ReadExactBytes(0x10);
                    this.BgmFlags = reader.ReadExactBytes(17);
                    reader.ReadExactBytes(0x11);
                    this.TotalPlayTime = reader.ReadInt32();
                    reader.ReadExactBytes(0x03E0);
                }
            }

            public IEnumerable<byte> LastName { get; }  // The last 2 bytes are always 0x00 ?

            public IEnumerable<byte> BgmFlags { get; }

            public int TotalPlayTime { get; }   // unit: 10ms

            public static bool CanInitialize(Th10.Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class ScoreData : Th10.ScoreData<StageProgress>
        {
            public new void ReadFrom(BinaryReader reader)
            {
                if (reader is null)
                    throw new ArgumentNullException(nameof(reader));

                base.ReadFrom(reader);
                reader.ReadUInt32();
            }
        }

        private class SpellCard : Th13.SpellCard<LevelPractice>
        {
        }
    }
}
