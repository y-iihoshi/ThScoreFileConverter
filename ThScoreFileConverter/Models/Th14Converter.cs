//-----------------------------------------------------------------------
// <copyright file="Th14Converter.cs" company="None">
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
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th14;
using CardInfo = ThScoreFileConverter.Models.SpellCardInfo<
    ThScoreFileConverter.Models.Stage, ThScoreFileConverter.Models.Level>;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th14Converter.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th14Converter.LevelPractice,
    ThScoreFileConverter.Models.Th14Converter.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th14Converter.StagePractice,
    ThScoreFileConverter.Models.Th14Converter.StageProgress>;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th14Converter : ThConverter
    {
        // Thanks to thwiki.info
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        private static readonly Dictionary<int, CardInfo> CardTable =
            new List<CardInfo>()
            {
                new CardInfo(  1, "氷符「アルティメットブリザード」",   Stage.One,   Level.Hard),
                new CardInfo(  2, "氷符「アルティメットブリザード」",   Stage.One,   Level.Lunatic),
                new CardInfo(  3, "水符「テイルフィンスラップ」",       Stage.One,   Level.Easy),
                new CardInfo(  4, "水符「テイルフィンスラップ」",       Stage.One,   Level.Normal),
                new CardInfo(  5, "水符「テイルフィンスラップ」",       Stage.One,   Level.Hard),
                new CardInfo(  6, "水符「テイルフィンスラップ」",       Stage.One,   Level.Lunatic),
                new CardInfo(  7, "鱗符「スケールウェイブ」",           Stage.One,   Level.Easy),
                new CardInfo(  8, "鱗符「スケールウェイブ」",           Stage.One,   Level.Normal),
                new CardInfo(  9, "鱗符「逆鱗の荒波」",                 Stage.One,   Level.Hard),
                new CardInfo( 10, "鱗符「逆鱗の大荒波」",               Stage.One,   Level.Lunatic),
                new CardInfo( 11, "飛符「フライングヘッド」",           Stage.Two,   Level.Easy),
                new CardInfo( 12, "飛符「フライングヘッド」",           Stage.Two,   Level.Normal),
                new CardInfo( 13, "飛符「フライングヘッド」",           Stage.Two,   Level.Hard),
                new CardInfo( 14, "飛符「フライングヘッド」",           Stage.Two,   Level.Lunatic),
                new CardInfo( 15, "首符「クローズアイショット」",       Stage.Two,   Level.Easy),
                new CardInfo( 16, "首符「クローズアイショット」",       Stage.Two,   Level.Normal),
                new CardInfo( 17, "首符「ろくろ首飛来」",               Stage.Two,   Level.Hard),
                new CardInfo( 18, "首符「ろくろ首飛来」",               Stage.Two,   Level.Lunatic),
                new CardInfo( 19, "飛頭「マルチプリケイティブヘッド」", Stage.Two,   Level.Easy),
                new CardInfo( 20, "飛頭「マルチプリケイティブヘッド」", Stage.Two,   Level.Normal),
                new CardInfo( 21, "飛頭「セブンズヘッド」",             Stage.Two,   Level.Hard),
                new CardInfo( 22, "飛頭「ナインズヘッド」",             Stage.Two,   Level.Lunatic),
                new CardInfo( 23, "飛頭「デュラハンナイト」",           Stage.Two,   Level.Easy),
                new CardInfo( 24, "飛頭「デュラハンナイト」",           Stage.Two,   Level.Normal),
                new CardInfo( 25, "飛頭「デュラハンナイト」",           Stage.Two,   Level.Hard),
                new CardInfo( 26, "飛頭「デュラハンナイト」",           Stage.Two,   Level.Lunatic),
                new CardInfo( 27, "牙符「月下の犬歯」",                 Stage.Three, Level.Hard),
                new CardInfo( 28, "牙符「月下の犬歯」",                 Stage.Three, Level.Lunatic),
                new CardInfo( 29, "変身「トライアングルファング」",     Stage.Three, Level.Easy),
                new CardInfo( 30, "変身「トライアングルファング」",     Stage.Three, Level.Normal),
                new CardInfo( 31, "変身「スターファング」",             Stage.Three, Level.Hard),
                new CardInfo( 32, "変身「スターファング」",             Stage.Three, Level.Lunatic),
                new CardInfo( 33, "咆哮「ストレンジロア」",             Stage.Three, Level.Easy),
                new CardInfo( 34, "咆哮「ストレンジロア」",             Stage.Three, Level.Normal),
                new CardInfo( 35, "咆哮「満月の遠吠え」",               Stage.Three, Level.Hard),
                new CardInfo( 36, "咆哮「満月の遠吠え」",               Stage.Three, Level.Lunatic),
                new CardInfo( 37, "狼符「スターリングパウンス」",       Stage.Three, Level.Easy),
                new CardInfo( 38, "狼符「スターリングパウンス」",       Stage.Three, Level.Normal),
                new CardInfo( 39, "天狼「ハイスピードパウンス」",       Stage.Three, Level.Hard),
                new CardInfo( 40, "天狼「ハイスピードパウンス」",       Stage.Three, Level.Lunatic),
                new CardInfo( 41, "平曲「祇園精舎の鐘の音」",           Stage.Four,  Level.Easy),
                new CardInfo( 42, "平曲「祇園精舎の鐘の音」",           Stage.Four,  Level.Normal),
                new CardInfo( 43, "平曲「祇園精舎の鐘の音」",           Stage.Four,  Level.Hard),
                new CardInfo( 44, "平曲「祇園精舎の鐘の音」",           Stage.Four,  Level.Lunatic),
                new CardInfo( 45, "怨霊「耳無し芳一」",                 Stage.Four,  Level.Easy),
                new CardInfo( 46, "怨霊「耳無し芳一」",                 Stage.Four,  Level.Normal),
                new CardInfo( 47, "怨霊「平家の大怨霊」",               Stage.Four,  Level.Hard),
                new CardInfo( 48, "怨霊「平家の大怨霊」",               Stage.Four,  Level.Lunatic),
                new CardInfo( 49, "楽符「邪悪な五線譜」",               Stage.Four,  Level.Easy),
                new CardInfo( 50, "楽符「邪悪な五線譜」",               Stage.Four,  Level.Normal),
                new CardInfo( 51, "楽符「凶悪な五線譜」",               Stage.Four,  Level.Hard),
                new CardInfo( 52, "楽符「ダブルスコア」",               Stage.Four,  Level.Lunatic),
                new CardInfo( 53, "琴符「諸行無常の琴の音」",           Stage.Four,  Level.Easy),
                new CardInfo( 54, "琴符「諸行無常の琴の音」",           Stage.Four,  Level.Normal),
                new CardInfo( 55, "琴符「諸行無常の琴の音」",           Stage.Four,  Level.Hard),
                new CardInfo( 56, "琴符「諸行無常の琴の音」",           Stage.Four,  Level.Lunatic),
                new CardInfo( 57, "響符「平安の残響」",                 Stage.Four,  Level.Easy),
                new CardInfo( 58, "響符「平安の残響」",                 Stage.Four,  Level.Normal),
                new CardInfo( 59, "響符「エコーチェンバー」",           Stage.Four,  Level.Hard),
                new CardInfo( 60, "響符「エコーチェンバー」",           Stage.Four,  Level.Lunatic),
                new CardInfo( 61, "箏曲「下克上送箏曲」",               Stage.Four,  Level.Easy),
                new CardInfo( 62, "箏曲「下克上送箏曲」",               Stage.Four,  Level.Normal),
                new CardInfo( 63, "筝曲「下克上レクイエム」",           Stage.Four,  Level.Hard),
                new CardInfo( 64, "筝曲「下克上レクイエム」",           Stage.Four,  Level.Lunatic),
                new CardInfo( 65, "欺符「逆針撃」",                     Stage.Five,  Level.Easy),
                new CardInfo( 66, "欺符「逆針撃」",                     Stage.Five,  Level.Normal),
                new CardInfo( 67, "欺符「逆針撃」",                     Stage.Five,  Level.Hard),
                new CardInfo( 68, "欺符「逆針撃」",                     Stage.Five,  Level.Lunatic),
                new CardInfo( 69, "逆符「鏡の国の弾幕」",               Stage.Five,  Level.Easy),
                new CardInfo( 70, "逆符「鏡の国の弾幕」",               Stage.Five,  Level.Normal),
                new CardInfo( 71, "逆符「イビルインザミラー」",         Stage.Five,  Level.Hard),
                new CardInfo( 72, "逆符「イビルインザミラー」",         Stage.Five,  Level.Lunatic),
                new CardInfo( 73, "逆符「天地有用」",                   Stage.Five,  Level.Easy),
                new CardInfo( 74, "逆符「天地有用」",                   Stage.Five,  Level.Normal),
                new CardInfo( 75, "逆符「天下転覆」",                   Stage.Five,  Level.Hard),
                new CardInfo( 76, "逆符「天下転覆」",                   Stage.Five,  Level.Lunatic),
                new CardInfo( 77, "逆弓「天壌夢弓」",                   Stage.Five,  Level.Easy),
                new CardInfo( 78, "逆弓「天壌夢弓」",                   Stage.Five,  Level.Normal),
                new CardInfo( 79, "逆弓「天壌夢弓の詔勅」",             Stage.Five,  Level.Hard),
                new CardInfo( 80, "逆弓「天壌夢弓の詔勅」",             Stage.Five,  Level.Lunatic),
                new CardInfo( 81, "逆転「リバースヒエラルキー」",       Stage.Five,  Level.Easy),
                new CardInfo( 82, "逆転「リバースヒエラルキー」",       Stage.Five,  Level.Normal),
                new CardInfo( 83, "逆転「チェンジエアブレイブ」",       Stage.Five,  Level.Hard),
                new CardInfo( 84, "逆転「チェンジエアブレイブ」",       Stage.Five,  Level.Lunatic),
                new CardInfo( 85, "小弾「小人の道」",                   Stage.Six,   Level.Easy),
                new CardInfo( 86, "小弾「小人の道」",                   Stage.Six,   Level.Normal),
                new CardInfo( 87, "小弾「小人の茨道」",                 Stage.Six,   Level.Hard),
                new CardInfo( 88, "小弾「小人の茨道」",                 Stage.Six,   Level.Lunatic),
                new CardInfo( 89, "小槌「大きくなあれ」",               Stage.Six,   Level.Easy),
                new CardInfo( 90, "小槌「大きくなあれ」",               Stage.Six,   Level.Normal),
                new CardInfo( 91, "小槌「もっと大きくなあれ」",         Stage.Six,   Level.Hard),
                new CardInfo( 92, "小槌「もっと大きくなあれ」",         Stage.Six,   Level.Lunatic),
                new CardInfo( 93, "妖剣「輝針剣」",                     Stage.Six,   Level.Easy),
                new CardInfo( 94, "妖剣「輝針剣」",                     Stage.Six,   Level.Normal),
                new CardInfo( 95, "妖剣「輝針剣」",                     Stage.Six,   Level.Hard),
                new CardInfo( 96, "妖剣「輝針剣」",                     Stage.Six,   Level.Lunatic),
                new CardInfo( 97, "小槌「お前が大きくなあれ」",         Stage.Six,   Level.Easy),
                new CardInfo( 98, "小槌「お前が大きくなあれ」",         Stage.Six,   Level.Normal),
                new CardInfo( 99, "小槌「お前が大きくなあれ」",         Stage.Six,   Level.Hard),
                new CardInfo(100, "小槌「お前が大きくなあれ」",         Stage.Six,   Level.Lunatic),
                new CardInfo(101, "「進撃の小人」",                     Stage.Six,   Level.Easy),
                new CardInfo(102, "「進撃の小人」",                     Stage.Six,   Level.Normal),
                new CardInfo(103, "「ウォールオブイッスン」",           Stage.Six,   Level.Hard),
                new CardInfo(104, "「ウォールオブイッスン」",           Stage.Six,   Level.Lunatic),
                new CardInfo(105, "「ホップオマイサムセブン」",         Stage.Six,   Level.Easy),
                new CardInfo(106, "「ホップオマイサムセブン」",         Stage.Six,   Level.Normal),
                new CardInfo(107, "「七人の一寸法師」",                 Stage.Six,   Level.Hard),
                new CardInfo(108, "「七人の一寸法師」",                 Stage.Six,   Level.Lunatic),
                new CardInfo(109, "弦楽「嵐のアンサンブル」",           Stage.Extra, Level.Extra),
                new CardInfo(110, "弦楽「浄瑠璃世界」",                 Stage.Extra, Level.Extra),
                new CardInfo(111, "一鼓「暴れ宮太鼓」",                 Stage.Extra, Level.Extra),
                new CardInfo(112, "二鼓「怨霊アヤノツヅミ」",           Stage.Extra, Level.Extra),
                new CardInfo(113, "三鼓「午前零時のスリーストライク」", Stage.Extra, Level.Extra),
                new CardInfo(114, "死鼓「ランドパーカス」",             Stage.Extra, Level.Extra),
                new CardInfo(115, "五鼓「デンデン太鼓」",               Stage.Extra, Level.Extra),
                new CardInfo(116, "六鼓「オルタネイトスティッキング」", Stage.Extra, Level.Extra),
                new CardInfo(117, "七鼓「高速和太鼓ロケット」",         Stage.Extra, Level.Extra),
                new CardInfo(118, "八鼓「雷神の怒り」",                 Stage.Extra, Level.Extra),
                new CardInfo(119, "「ブルーレディショー」",             Stage.Extra, Level.Extra),
                new CardInfo(120, "「プリスティンビート」",             Stage.Extra, Level.Extra),
            }.ToDictionary(card => card.Id);

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
            [EnumAltName("-")] NotUsed,
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
            [EnumAltName("-")] NotUsed,
            [EnumAltName("T")] Total,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum Chara
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RA")] ReimuA,
            [EnumAltName("RB")] ReimuB,
            [EnumAltName("MA")] MarisaA,
            [EnumAltName("MB")] MarisaB,
            [EnumAltName("SA")] SakuyaA,
            [EnumAltName("SB")] SakuyaB,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum CharaWithTotal
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RA")] ReimuA,
            [EnumAltName("RB")] ReimuB,
            [EnumAltName("MA")] MarisaA,
            [EnumAltName("MB")] MarisaB,
            [EnumAltName("SA")] SakuyaA,
            [EnumAltName("SB")] SakuyaB,
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
            get { return "1.00b"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th14decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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

        // %T14SCR[w][xx][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T14SCR({0})({1})(\d)([1-5])", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th14Converter parent)
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

        // %T14C[w][xxx][yy][z]
        private class CareerReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T14C([SP])(\d{{3}})({0})([12])", CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(Th14Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var kind = match.Groups[1].Value.ToUpperInvariant();
                    var number = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    var chara = CharaWithTotalParser.Parse(match.Groups[3].Value);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    Func<Th13.ISpellCard<Level>, int> getCount;
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

        // %T14CARD[xxx][y]
        private class CardReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T14CARD(\d{3})([NR])";

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th14Converter parent, bool hideUntriedCards)
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

        // %T14CRG[v][w][xx][y][z]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T14CRG([SP])({0})({1})({2})([12])",
                LevelWithTotalParser.Pattern,
                CharaWithTotalParser.Pattern,
                StageWithTotalParser.Pattern);

            private static readonly Func<Th13.ISpellCard<Level>, string, int, bool> FindByKindTypeImpl =
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
            public CollectRateReplacer(Th14Converter parent)
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

                    Func<Th13.ISpellCard<Level>, bool> findByKindType = (card => FindByKindTypeImpl(card, kind, type));

                    Func<Th13.ISpellCard<Level>, bool> findByStage;
                    if (stage == StageWithTotal.Total)
                        findByStage = (card => true);
                    else
                        findByStage = (card => CardTable[card.Id].Stage == (Stage)stage);

                    Func<Th13.ISpellCard<Level>, bool> findByLevel = (card => true);
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

        // %T14CLEAR[x][yy]
        private class ClearReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T14CLEAR({0})({1})", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ClearReplacer(Th14Converter parent)
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

        // %T14CHARA[xx][y]
        private class CharaReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T14CHARA({0})([1-3])", CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CharaReplacer(Th14Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var chara = CharaWithTotalParser.Parse(match.Groups[1].Value);
                    var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                    Func<IClearData, long> getValueByType;
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

        // %T14CHARAEX[x][yy][z]
        private class CharaExReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T14CHARAEX({0})({1})([1-3])", LevelWithTotalParser.Pattern, CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CharaExReplacer(Th14Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var chara = CharaWithTotalParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    Func<IClearData, long> getValueByType;
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

        // %T14PRAC[x][yy][z]
        private class PracticeReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T14PRAC({0})({1})({2})", LevelParser.Pattern, CharaParser.Pattern, StageParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public PracticeReplacer(Th14Converter parent)
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
                        var key = ((LevelPractice)level, (StagePractice)stage);
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
            private readonly Dictionary<CharaWithTotal, IClearData> clearData;

            public AllScoreData()
            {
                this.clearData =
                    new Dictionary<CharaWithTotal, IClearData>(Enum.GetValues(typeof(CharaWithTotal)).Length);
            }

            public Th095.HeaderBase Header { get; private set; }

            public IReadOnlyDictionary<CharaWithTotal, IClearData> ClearData => this.clearData;

            public Th125.IStatus Status { get; private set; }

            public void Set(Th095.HeaderBase header) => this.Header = header;

            public void Set(IClearData data)
            {
                if (!this.clearData.ContainsKey(data.Chara))
                    this.clearData.Add(data.Chara, data);
            }

            public void Set(Th125.IStatus status) => this.Status = status;
        }

        private class ClearData : Th10.Chapter, IClearData  // per character
        {
            public const string ValidSignature = "CR";
            public const ushort ValidVersion = 0x0001;
            public const int ValidSize = 0x00005298;

            public ClearData(Th10.Chapter chapter)
                : base(chapter, ValidSignature, ValidVersion, ValidSize)
            {
                var levelsWithTotal = Utils.GetEnumerator<LevelPracticeWithTotal>();
                var levels = Utils.GetEnumerator<LevelPractice>();
                var stages = Utils.GetEnumerator<StagePractice>();

                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    this.Chara = (CharaWithTotal)reader.ReadInt32();

                    this.Rankings = levelsWithTotal.ToDictionary(
                        level => level,
                        _ => Enumerable.Range(0, 10).Select(rank =>
                        {
                            var score = new ScoreData();
                            score.ReadFrom(reader);
                            return score;
                        }).ToList() as IReadOnlyList<Th10.IScoreData<StageProgress>>);

                    this.TotalPlayCount = reader.ReadInt32();
                    this.PlayTime = reader.ReadInt32();
                    this.ClearCounts = levelsWithTotal.ToDictionary(level => level, _ => reader.ReadInt32());
                    this.ClearFlags = levelsWithTotal.ToDictionary(level => level, _ => reader.ReadInt32());

                    this.Practices = levels
                        .SelectMany(level => stages.Select(stage => (level, stage)))
                        .ToDictionary(pair => pair, _ =>
                        {
                            var practice = new Th13.Practice();
                            practice.ReadFrom(reader);
                            return practice as Th13.IPractice;
                        });

                    this.Cards = Enumerable.Range(0, CardTable.Count).Select(_ =>
                    {
                        var card = new SpellCard();
                        card.ReadFrom(reader);
                        return card as Th13.ISpellCard<Level>;
                    }).ToDictionary(card => card.Id);
                }
            }

            public CharaWithTotal Chara { get; }

            public IReadOnlyDictionary<LevelPracticeWithTotal, IReadOnlyList<Th10.IScoreData<StageProgress>>> Rankings { get; }

            public int TotalPlayCount { get; }

            public int PlayTime { get; }    // = seconds * 60fps

            public IReadOnlyDictionary<LevelPracticeWithTotal, int> ClearCounts { get; }

            public IReadOnlyDictionary<LevelPracticeWithTotal, int> ClearFlags { get; }  // Really...?

            public IReadOnlyDictionary<(LevelPractice, StagePractice), Th13.IPractice> Practices { get; }

            public IReadOnlyDictionary<int, Th13.ISpellCard<Level>> Cards { get; }

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

        private class ScoreData : Th10.ScoreDataBase<StageProgress>
        {
            public new void ReadFrom(BinaryReader reader)
            {
                if (reader is null)
                    throw new ArgumentNullException(nameof(reader));

                base.ReadFrom(reader);
                reader.ReadUInt32();
            }
        }

        private class SpellCard : Th13.SpellCard<Level>
        {
        }
    }
}
