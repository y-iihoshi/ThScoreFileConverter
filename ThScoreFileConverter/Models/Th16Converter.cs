//-----------------------------------------------------------------------
// <copyright file="Th16Converter.cs" company="None">
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
using CardInfo = ThScoreFileConverter.Models.SpellCardInfo<
    ThScoreFileConverter.Models.ThConverter.Stage, ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th16Converter : ThConverter
    {
        // Thanks to thwiki.info
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        private static readonly Dictionary<int, CardInfo> CardTable =
            new List<CardInfo>()
            {
                new CardInfo(  1, "蝶符「ミニットスケールス」",           Stage.St1,   Level.Easy),
                new CardInfo(  2, "蝶符「ミニットスケールス」",           Stage.St1,   Level.Normal),
                new CardInfo(  3, "蝶符「アゲハの鱗粉」",                 Stage.St1,   Level.Hard),
                new CardInfo(  4, "蝶符「アゲハの鱗粉」",                 Stage.St1,   Level.Lunatic),
                new CardInfo(  5, "蝶符「フラッタリングサマー」",         Stage.St1,   Level.Easy),
                new CardInfo(  6, "蝶符「フラッタリングサマー」",         Stage.St1,   Level.Normal),
                new CardInfo(  7, "蝶符「真夏の羽ばたき」",               Stage.St1,   Level.Hard),
                new CardInfo(  8, "蝶符「真夏の羽ばたき」",               Stage.St1,   Level.Lunatic),
                new CardInfo(  9, "雨符「囚われの秋雨」",                 Stage.St2,   Level.Easy),
                new CardInfo( 10, "雨符「囚われの秋雨」",                 Stage.St2,   Level.Normal),
                new CardInfo( 11, "雨符「呪われた柴榑雨」",               Stage.St2,   Level.Hard),
                new CardInfo( 12, "雨符「呪われた柴榑雨」",               Stage.St2,   Level.Lunatic),
                new CardInfo( 13, "刃符「山姥の包丁研ぎ」",               Stage.St2,   Level.Easy),
                new CardInfo( 14, "刃符「山姥の包丁研ぎ」",               Stage.St2,   Level.Normal),
                new CardInfo( 15, "刃符「山姥の鬼包丁研ぎ」",             Stage.St2,   Level.Hard),
                new CardInfo( 16, "刃符「山姥の鬼包丁研ぎ」",             Stage.St2,   Level.Lunatic),
                new CardInfo( 17, "尽符「マウンテンマーダー」",           Stage.St2,   Level.Easy),
                new CardInfo( 18, "尽符「マウンテンマーダー」",           Stage.St2,   Level.Normal),
                new CardInfo( 19, "尽符「ブラッディマウンテンマーダー」", Stage.St2,   Level.Hard),
                new CardInfo( 20, "尽符「ブラッディマウンテンマーダー」", Stage.St2,   Level.Lunatic),
                new CardInfo( 21, "春符「サプライズスプリング」",         Stage.St3,   Level.Hard),
                new CardInfo( 22, "春符「サプライズスプリング」",         Stage.St3,   Level.Lunatic),
                new CardInfo( 23, "犬符「野良犬の散歩」",                 Stage.St3,   Level.Easy),
                new CardInfo( 24, "犬符「野良犬の散歩」",                 Stage.St3,   Level.Normal),
                new CardInfo( 25, "狗符「山狗の散歩」",                   Stage.St3,   Level.Hard),
                new CardInfo( 26, "狗符「山狗の散歩」",                   Stage.St3,   Level.Lunatic),
                new CardInfo( 27, "独楽「コマ犬回し」",                   Stage.St3,   Level.Easy),
                new CardInfo( 28, "独楽「コマ犬回し」",                   Stage.St3,   Level.Normal),
                new CardInfo( 29, "独楽「コマ犬回し」",                   Stage.St3,   Level.Hard),
                new CardInfo( 30, "独楽「カールアップアンドダイ」",       Stage.St3,   Level.Lunatic),
                new CardInfo( 31, "狗符「独り阿吽の呼吸」",               Stage.St3,   Level.Easy),
                new CardInfo( 32, "狗符「独り阿吽の呼吸」",               Stage.St3,   Level.Normal),
                new CardInfo( 33, "狗符「独り阿吽の呼吸」",               Stage.St3,   Level.Hard),
                new CardInfo( 34, "狗符「独り阿吽の呼吸」",               Stage.St3,   Level.Lunatic),
                new CardInfo( 35, "魔符「インスタントボーディ」",         Stage.St4,   Level.Easy),
                new CardInfo( 36, "魔符「インスタントボーディ」",         Stage.St4,   Level.Normal),
                new CardInfo( 37, "魔符「即席菩提」",                     Stage.St4,   Level.Hard),
                new CardInfo( 38, "魔符「即席菩提」",                     Stage.St4,   Level.Lunatic),
                new CardInfo( 39, "魔符「バレットゴーレム」",             Stage.St4,   Level.Easy),
                new CardInfo( 40, "魔符「バレットゴーレム」",             Stage.St4,   Level.Normal),
                new CardInfo( 41, "魔符「ペットの巨大弾生命体」",         Stage.St4,   Level.Hard),
                new CardInfo( 42, "魔符「ペットの巨大弾生命体」",         Stage.St4,   Level.Lunatic),
                new CardInfo( 43, "地蔵「クリミナルサルヴェイション」",   Stage.St4,   Level.Easy),
                new CardInfo( 44, "地蔵「クリミナルサルヴェイション」",   Stage.St4,   Level.Normal),
                new CardInfo( 45, "地蔵「業火救済」",                     Stage.St4,   Level.Hard),
                new CardInfo( 46, "地蔵「業火救済」",                     Stage.St4,   Level.Lunatic),
                new CardInfo( 47, "竹符「バンブースピアダンス」",         Stage.St5,   Level.Easy),
                new CardInfo( 48, "竹符「バンブースピアダンス」",         Stage.St5,   Level.Normal),
                new CardInfo( 49, "竹符「バンブークレイジーダンス」",     Stage.St5,   Level.Hard),
                new CardInfo( 50, "竹符「バンブークレイジーダンス」",     Stage.St5,   Level.Lunatic),
                new CardInfo( 51, "茗荷「フォゲットユアネーム」",         Stage.St5,   Level.Easy),
                new CardInfo( 52, "茗荷「フォゲットユアネーム」",         Stage.St5,   Level.Normal),
                new CardInfo( 53, "茗荷「フォゲットユアネーム」",         Stage.St5,   Level.Hard),
                new CardInfo( 54, "茗荷「フォゲットユアネーム」",         Stage.St5,   Level.Lunatic),
                new CardInfo( 55, "笹符「タナバタスターフェスティバル」", Stage.St5,   Level.Easy),
                new CardInfo( 56, "笹符「タナバタスターフェスティバル」", Stage.St5,   Level.Normal),
                new CardInfo( 57, "笹符「タナバタスターフェスティバル」", Stage.St5,   Level.Hard),
                new CardInfo( 58, "笹符「タナバタスターフェスティバル」", Stage.St5,   Level.Lunatic),
                new CardInfo( 59, "冥加「ビハインドユー」",               Stage.St5,   Level.Easy),
                new CardInfo( 60, "冥加「ビハインドユー」",               Stage.St5,   Level.Normal),
                new CardInfo( 61, "冥加「ビハインドユー」",               Stage.St5,   Level.Hard),
                new CardInfo( 62, "冥加「ビハインドユー」",               Stage.St5,   Level.Lunatic),
                new CardInfo( 63, "舞符「ビハインドフェスティバル」",     Stage.St5,   Level.Easy),
                new CardInfo( 64, "舞符「ビハインドフェスティバル」",     Stage.St5,   Level.Normal),
                new CardInfo( 65, "舞符「ビハインドフェスティバル」",     Stage.St5,   Level.Hard),
                new CardInfo( 66, "舞符「ビハインドフェスティバル」",     Stage.St5,   Level.Lunatic),
                new CardInfo( 67, "狂舞「テングオドシ」",                 Stage.St5,   Level.Easy),
                new CardInfo( 68, "狂舞「テングオドシ」",                 Stage.St5,   Level.Normal),
                new CardInfo( 69, "狂舞「狂乱天狗怖し」",                 Stage.St5,   Level.Hard),
                new CardInfo( 70, "狂舞「狂乱天狗怖し」",                 Stage.St5,   Level.Lunatic),
                new CardInfo( 71, "後符「秘神の後光」",                   Stage.St6,   Level.Easy),
                new CardInfo( 72, "後符「秘神の後光」",                   Stage.St6,   Level.Normal),
                new CardInfo( 73, "後符「秘神の後光」",                   Stage.St6,   Level.Hard),
                new CardInfo( 74, "後符「絶対秘神の後光」",               Stage.St6,   Level.Lunatic),
                new CardInfo( 75, "裏夏「スコーチ・バイ・ホットサマー」", Stage.St6,   Level.Easy),
                new CardInfo( 76, "裏夏「スコーチ・バイ・ホットサマー」", Stage.St6,   Level.Normal),
                new CardInfo( 77, "裏夏「異常猛暑の焦土」",               Stage.St6,   Level.Hard),
                new CardInfo( 78, "裏夏「異常猛暑の焦土」",               Stage.St6,   Level.Lunatic),
                new CardInfo( 79, "裏秋「ダイ・オブ・ファミン」",         Stage.St6,   Level.Easy),
                new CardInfo( 80, "裏秋「ダイ・オブ・ファミン」",         Stage.St6,   Level.Normal),
                new CardInfo( 81, "裏秋「異常枯死の餓鬼」",               Stage.St6,   Level.Hard),
                new CardInfo( 82, "裏秋「異常枯死の餓鬼」",               Stage.St6,   Level.Lunatic),
                new CardInfo( 83, "裏冬「ブラックスノーマン」",           Stage.St6,   Level.Easy),
                new CardInfo( 84, "裏冬「ブラックスノーマン」",           Stage.St6,   Level.Normal),
                new CardInfo( 85, "裏冬「異常降雪の雪だるま」",           Stage.St6,   Level.Hard),
                new CardInfo( 86, "裏冬「異常降雪の雪だるま」",           Stage.St6,   Level.Lunatic),
                new CardInfo( 87, "裏春「エイプリルウィザード」",         Stage.St6,   Level.Easy),
                new CardInfo( 88, "裏春「エイプリルウィザード」",         Stage.St6,   Level.Normal),
                new CardInfo( 89, "裏春「異常落花の魔術使い」",           Stage.St6,   Level.Hard),
                new CardInfo( 90, "裏春「異常落花の魔術使い」",           Stage.St6,   Level.Lunatic),
                new CardInfo( 91, "「裏・ブリージーチェリーブロッサム」", Stage.St6,   Level.Easy),
                new CardInfo( 92, "「裏・ブリージーチェリーブロッサム」", Stage.St6,   Level.Normal),
                new CardInfo( 93, "「裏・ブリージーチェリーブロッサム」", Stage.St6,   Level.Hard),
                new CardInfo( 94, "「裏・ブリージーチェリーブロッサム」", Stage.St6,   Level.Lunatic),
                new CardInfo( 95, "「裏・パーフェクトサマーアイス」",     Stage.St6,   Level.Easy),
                new CardInfo( 96, "「裏・パーフェクトサマーアイス」",     Stage.St6,   Level.Normal),
                new CardInfo( 97, "「裏・パーフェクトサマーアイス」",     Stage.St6,   Level.Hard),
                new CardInfo( 98, "「裏・パーフェクトサマーアイス」",     Stage.St6,   Level.Lunatic),
                new CardInfo( 99, "「裏・クレイジーフォールウィンド」",   Stage.St6,   Level.Easy),
                new CardInfo(100, "「裏・クレイジーフォールウィンド」",   Stage.St6,   Level.Normal),
                new CardInfo(101, "「裏・クレイジーフォールウィンド」",   Stage.St6,   Level.Hard),
                new CardInfo(102, "「裏・クレイジーフォールウィンド」",   Stage.St6,   Level.Lunatic),
                new CardInfo(103, "「裏・エクストリームウィンター」",     Stage.St6,   Level.Easy),
                new CardInfo(104, "「裏・エクストリームウィンター」",     Stage.St6,   Level.Normal),
                new CardInfo(105, "「裏・エクストリームウィンター」",     Stage.St6,   Level.Hard),
                new CardInfo(106, "「裏・エクストリームウィンター」",     Stage.St6,   Level.Lunatic),
                new CardInfo(107, "鼓舞「パワフルチアーズ」",             Stage.Extra, Level.Extra),
                new CardInfo(108, "狂舞「クレイジーバックダンス」",       Stage.Extra, Level.Extra),
                new CardInfo(109, "弾舞「二つ目の台風」",                 Stage.Extra, Level.Extra),
                new CardInfo(110, "秘儀「リバースインヴォーカー」",       Stage.Extra, Level.Extra),
                new CardInfo(111, "秘儀「裏切りの後方射撃」",             Stage.Extra, Level.Extra),
                new CardInfo(112, "秘儀「弾幕の玉繭」",                   Stage.Extra, Level.Extra),
                new CardInfo(113, "秘儀「穢那の火」",                     Stage.Extra, Level.Extra),
                new CardInfo(114, "秘儀「後戸の狂言」",                   Stage.Extra, Level.Extra),
                new CardInfo(115, "秘儀「マターラドゥッカ」",             Stage.Extra, Level.Extra),
                new CardInfo(116, "秘儀「七星の剣」",                     Stage.Extra, Level.Extra),
                new CardInfo(117, "秘儀「無縁の芸能者」",                 Stage.Extra, Level.Extra),
                new CardInfo(118, "「背面の暗黒猿楽」",                   Stage.Extra, Level.Extra),
                new CardInfo(119, "「アナーキーバレットヘル」",           Stage.Extra, Level.Extra),
            }.ToDictionary(card => card.Id);

        private static readonly EnumShortNameParser<Chara> CharaParser =
            new EnumShortNameParser<Chara>();

        private static readonly EnumShortNameParser<CharaWithTotal> CharaWithTotalParser =
            new EnumShortNameParser<CharaWithTotal>();

        private AllScoreData allScoreData = null;

        public enum Chara
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RM")] Reimu,
            [EnumAltName("CI")] Cirno,
            [EnumAltName("AY")] Aya,
            [EnumAltName("MR")] Marisa,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum CharaWithTotal
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RM")] Reimu,
            [EnumAltName("CI")] Cirno,
            [EnumAltName("AY")] Aya,
            [EnumAltName("MR")] Marisa,
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
            [EnumAltName("-")] NotUsed,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum Season
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("春")]   Spring,
            [EnumAltName("夏")]   Summer,
            [EnumAltName("秋")]   Autumn,
            [EnumAltName("冬")]   Winter,
            [EnumAltName("土用")] Full,
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
            get { return "1.00a"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th16decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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

        // %T16SCR[w][xx][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T16SCR({0})({1})(\d)([1-6])", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th16Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = (LevelWithTotal)LevelParser.Parse(match.Groups[1].Value);
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
                        case 6:     // season
                            if (ranking.DateTime == 0)
                                return "-----";
                            return ranking.Season.ToShortName();
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

        // %T16C[w][xxx][yy][z]
        private class CareerReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T16C([SP])(\d{{3}})({0})([12])", CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(Th16Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var kind = match.Groups[1].Value.ToUpperInvariant();
                    var number = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    var chara = CharaWithTotalParser.Parse(match.Groups[3].Value);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    Func<SpellCard, int> getCount;
                    if (kind == "S")
                    {
                        if (type == 1)
                            getCount = (card => card.ClearCount);
                        else
                            getCount = (card => card.TrialCount);
                    }
                    else
                    {
                        if (type == 1)
                            getCount = (card => card.PracticeClearCount);
                        else
                            getCount = (card => card.PracticeTrialCount);
                    }

                    var cards = parent.allScoreData.ClearData[chara].Cards;
                    if (number == 0)
                    {
                        return Utils.ToNumberString(cards.Values.Sum(getCount));
                    }
                    else if (CardTable.ContainsKey(number))
                    {
                        if (cards.TryGetValue(number, out var card))
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

        // %T16CARD[xxx][y]
        private class CardReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T16CARD(\d{3})([NR])";

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th16Converter parent, bool hideUntriedCards)
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

        // %T16CRG[v][w][xx][y][z]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T16CRG([SP])({0})({1})({2})([12])",
                LevelWithTotalParser.Pattern,
                CharaWithTotalParser.Pattern,
                StageWithTotalParser.Pattern);

            private static readonly Func<SpellCard, string, int, bool> FindByKindTypeImpl =
                (card, kind, type) =>
                {
                    if (kind == "S")
                    {
                        if (type == 1)
                            return card.ClearCount > 0;
                        else
                            return card.TrialCount > 0;
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
            public CollectRateReplacer(Th16Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var kind = match.Groups[1].Value.ToUpperInvariant();
                    var level = LevelWithTotalParser.Parse(match.Groups[2].Value);
                    var chara = CharaWithTotalParser.Parse(match.Groups[3].Value);
                    var stage = StageWithTotalParser.Parse(match.Groups[4].Value);
                    var type = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);

                    if (stage == StageWithTotal.Extra)
                        return match.ToString();

                    Func<SpellCard, bool> findByKindType = (card => FindByKindTypeImpl(card, kind, type));

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

        // %T16CLEAR[x][yy]
        private class ClearReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T16CLEAR({0})({1})", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ClearReplacer(Th16Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = (LevelWithTotal)LevelParser.Parse(match.Groups[1].Value);
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

        // %T16CHARA[xx][y]
        private class CharaReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T16CHARA({0})([1-3])", CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CharaReplacer(Th16Converter parent)
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
                        toString = (value => new Time(value * 10, false).ToString());
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

        // %T16CHARAEX[x][yy][z]
        private class CharaExReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T16CHARAEX({0})({1})([1-3])", LevelWithTotalParser.Pattern, CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CharaExReplacer(Th16Converter parent)
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
                        toString = (value => new Time(value * 10, false).ToString());
                    }
                    else
                    {
                        if (level == LevelWithTotal.Total)
                            getValueByType = (data => data.ClearCounts.Values.Sum());
                        else
                            getValueByType = (data => data.ClearCounts[level]);
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

        // %T16PRAC[x][yy][z]
        private class PracticeReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T16PRAC({0})({1})({2})", LevelParser.Pattern, CharaParser.Pattern, StageParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public PracticeReplacer(Th16Converter parent)
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
                        var key = (level, (StagePractice)stage);
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

            public Status Status { get; private set; }

            public void Set(Header header) => this.Header = header;

            public void Set(ClearData data)
            {
                if (!this.ClearData.ContainsKey(data.Chara))
                    this.ClearData.Add(data.Chara, data);
            }

            public void Set(Status status) => this.Status = status;
        }

        private class Header : Th095.Header
        {
            public const string ValidSignature = "TH61";

            public override bool IsValid
                => base.IsValid && this.Signature.Equals(ValidSignature, StringComparison.Ordinal);
        }

        private class ClearData : Th10.Chapter   // per character
        {
            public const string ValidSignature = "CR";
            public const ushort ValidVersion = 0x0001;
            public const int ValidSize = 0x00005318;

            public ClearData(Th10.Chapter chapter)
                : base(chapter, ValidSignature, ValidVersion, ValidSize)
            {
                var levelsWithTotal = Utils.GetEnumerator<LevelWithTotal>();
                var levels = Utils.GetEnumerator<Level>();
                var stages = Utils.GetEnumerator<StagePractice>();
                var numLevelsWithTotal = levelsWithTotal.Count();

                this.Rankings = new Dictionary<LevelWithTotal, ScoreData[]>(numLevelsWithTotal);
                this.ClearCounts = new Dictionary<LevelWithTotal, int>(numLevelsWithTotal);
                this.ClearFlags = new Dictionary<LevelWithTotal, int>(numLevelsWithTotal);
                this.Practices = new Dictionary<(Level, StagePractice), Th13.Practice>(levels.Count() * stages.Count());
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

                    reader.ReadExactBytes(0x140);

                    for (var number = 0; number < CardTable.Count; number++)
                    {
                        var card = new SpellCard();
                        card.ReadFrom(reader);
                        if (!this.Cards.ContainsKey(card.Id))
                            this.Cards.Add(card.Id, card);
                    }

                    this.TotalPlayCount = reader.ReadInt32();
                    this.PlayTime = reader.ReadInt32();

                    reader.ReadUInt32();
                    foreach (var level in levelsWithTotal)
                    {
                        var clearCount = reader.ReadInt32();
                        if (!this.ClearCounts.ContainsKey(level))
                            this.ClearCounts.Add(level, clearCount);
                    }

                    reader.ReadUInt32();
                    foreach (var level in levelsWithTotal)
                    {
                        var clearFlag = reader.ReadInt32();
                        if (!this.ClearFlags.ContainsKey(level))
                            this.ClearFlags.Add(level, clearFlag);
                    }

                    reader.ReadUInt32();

                    foreach (var level in levels)
                    {
                        foreach (var stage in stages)
                        {
                            var practice = new Th13.Practice();
                            practice.ReadFrom(reader);
                            var key = (level, stage);
                            if (!this.Practices.ContainsKey(key))
                                this.Practices.Add(key, practice);
                        }
                    }
                }
            }

            public CharaWithTotal Chara { get; }

            public Dictionary<LevelWithTotal, ScoreData[]> Rankings { get; }

            public int TotalPlayCount { get; }

            public int PlayTime { get; }    // unit: 10ms

            public Dictionary<LevelWithTotal, int> ClearCounts { get; }

            public Dictionary<LevelWithTotal, int> ClearFlags { get; }  // Really...?

            public Dictionary<(Level, StagePractice), Th13.Practice> Practices { get; }

            public Dictionary<int, SpellCard> Cards { get; }

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

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] LastName { get; }     // The last 2 bytes are always 0x00 ?

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] BgmFlags { get; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int TotalPlayTime { get; }   // unit: 10ms

            public static bool CanInitialize(Th10.Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class ScoreData : IBinaryReadable
        {
            public uint Score { get; private set; }     // Divided by 10

            public StageProgress StageProgress { get; private set; }

            public byte ContinueCount { get; private set; }

            public byte[] Name { get; private set; }    // The last 2 bytes are always 0x00 ?

            public uint DateTime { get; private set; }  // UNIX time

            public float SlowRate { get; private set; }

            public Season Season { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader is null)
                    throw new ArgumentNullException(nameof(reader));

                this.Score = reader.ReadUInt32();
                this.StageProgress = Utils.ToEnum<StageProgress>(reader.ReadByte());
                this.ContinueCount = reader.ReadByte();
                this.Name = reader.ReadExactBytes(10);
                this.DateTime = reader.ReadUInt32();
                reader.ReadUInt32();
                this.SlowRate = reader.ReadSingle();
                this.Season = Utils.ToEnum<Season>(reader.ReadInt32());
            }
        }

        private class SpellCard : Th13.SpellCard<Level>
        {
        }
    }
}
