//-----------------------------------------------------------------------
// <copyright file="Th095Converter.cs" company="None">
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
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th095Converter : ThConverter
    {
        // Thanks to thwiki.info
        private static readonly Dictionary<(Level Level, int Scene), (Enemy Enemy, string Card)> SpellCards =
            new Dictionary<(Level, int), (Enemy, string)>()
            {
                { (Level.Lv1,   1), (Enemy.Wriggle,   string.Empty) },
                { (Level.Lv1,   2), (Enemy.Rumia,     string.Empty) },
                { (Level.Lv1,   3), (Enemy.Wriggle,   "蛍符「地上の恒星」") },
                { (Level.Lv1,   4), (Enemy.Rumia,     "闇符「ダークサイドオブザムーン」") },
                { (Level.Lv1,   5), (Enemy.Wriggle,   "蝶符「バタフライストーム」") },
                { (Level.Lv1,   6), (Enemy.Rumia,     "夜符「ミッドナイトバード」") },
                { (Level.Lv2,   1), (Enemy.Cirno,     string.Empty) },
                { (Level.Lv2,   2), (Enemy.Letty,     string.Empty) },
                { (Level.Lv2,   3), (Enemy.Cirno,     "雪符「ダイアモンドブリザード」") },
                { (Level.Lv2,   4), (Enemy.Letty,     "寒符「コールドスナップ」") },
                { (Level.Lv2,   5), (Enemy.Cirno,     "凍符「マイナスＫ」") },
                { (Level.Lv2,   6), (Enemy.Letty,     "冬符「ノーザンウイナー」") },
                { (Level.Lv3,   1), (Enemy.Alice,     string.Empty) },
                { (Level.Lv3,   2), (Enemy.Keine,     "光符「アマテラス」") },
                { (Level.Lv3,   3), (Enemy.Alice,     "操符「ドールズインシー」") },
                { (Level.Lv3,   4), (Enemy.Keine,     "包符「昭和の雨」") },
                { (Level.Lv3,   5), (Enemy.Alice,     "呪符「ストロードールカミカゼ」") },
                { (Level.Lv3,   6), (Enemy.Keine,     "葵符「水戸の光圀」") },
                { (Level.Lv3,   7), (Enemy.Alice,     "赤符「ドールミラセティ」") },
                { (Level.Lv3,   8), (Enemy.Keine,     "倭符「邪馬台の国」") },
                { (Level.Lv4,   1), (Enemy.Reisen,    string.Empty) },
                { (Level.Lv4,   2), (Enemy.Medicine,  "霧符「ガシングガーデン」") },
                { (Level.Lv4,   3), (Enemy.Tewi,      "脱兎「フラスターエスケープ」") },
                { (Level.Lv4,   4), (Enemy.Reisen,    "散符「朧月花栞（ロケット・イン・ミスト）」") },
                { (Level.Lv4,   5), (Enemy.Medicine,  "毒符「ポイズンブレス」") },
                { (Level.Lv4,   6), (Enemy.Reisen,    "波符「幻の月（インビジブルハーフムーン）」") },
                { (Level.Lv4,   7), (Enemy.Medicine,  "譫妄「イントゥデリリウム」") },
                { (Level.Lv4,   8), (Enemy.Tewi,      "借符「大穴牟遅様の薬」") },
                { (Level.Lv4,   9), (Enemy.Reisen,    "狂夢「風狂の夢（ドリームワールド）」") },
                { (Level.Lv5,   1), (Enemy.Meirin,    string.Empty) },
                { (Level.Lv5,   2), (Enemy.Patchouli, "日＆水符「ハイドロジェナスプロミネンス」") },
                { (Level.Lv5,   3), (Enemy.Meirin,    "華符「彩光蓮華掌」") },
                { (Level.Lv5,   4), (Enemy.Patchouli, "水＆火符「フロギスティックレイン」") },
                { (Level.Lv5,   5), (Enemy.Meirin,    "彩翔「飛花落葉」") },
                { (Level.Lv5,   6), (Enemy.Patchouli, "月＆木符「サテライトヒマワリ」") },
                { (Level.Lv5,   7), (Enemy.Meirin,    "彩華「虹色太極拳」") },
                { (Level.Lv5,   8), (Enemy.Patchouli, "日＆月符「ロイヤルダイアモンドリング」") },
                { (Level.Lv6,   1), (Enemy.Chen,      string.Empty) },
                { (Level.Lv6,   2), (Enemy.Youmu,     "人智剣「天女返し」") },
                { (Level.Lv6,   3), (Enemy.Chen,      "星符「飛び重ね鱗」") },
                { (Level.Lv6,   4), (Enemy.Youmu,     "妄執剣「修羅の血」") },
                { (Level.Lv6,   5), (Enemy.Chen,      "鬼神「鳴動持国天」") },
                { (Level.Lv6,   6), (Enemy.Youmu,     "天星剣「涅槃寂静の如し」") },
                { (Level.Lv6,   7), (Enemy.Chen,      "化猫「橙」") },
                { (Level.Lv6,   8), (Enemy.Youmu,     "四生剣「衆生無情の響き」") },
                { (Level.Lv7,   1), (Enemy.Sakuya,    string.Empty) },
                { (Level.Lv7,   2), (Enemy.Remilia,   "魔符「全世界ナイトメア」") },
                { (Level.Lv7,   3), (Enemy.Sakuya,    "時符「トンネルエフェクト」") },
                { (Level.Lv7,   4), (Enemy.Remilia,   "紅符「ブラッディマジックスクウェア」") },
                { (Level.Lv7,   5), (Enemy.Sakuya,    "空虚「インフレーションスクウェア」") },
                { (Level.Lv7,   6), (Enemy.Remilia,   "紅蝙蝠「ヴァンピリッシュナイト」") },
                { (Level.Lv7,   7), (Enemy.Sakuya,    "銀符「パーフェクトメイド」") },
                { (Level.Lv7,   8), (Enemy.Remilia,   "神鬼「レミリアストーカー」") },
                { (Level.Lv8,   1), (Enemy.Ran,       string.Empty) },
                { (Level.Lv8,   2), (Enemy.Yuyuko,    "幽雅「死出の誘蛾灯」") },
                { (Level.Lv8,   3), (Enemy.Ran,       "密符「御大師様の秘鍵」") },
                { (Level.Lv8,   4), (Enemy.Yuyuko,    "蝶符「鳳蝶紋の死槍」") },
                { (Level.Lv8,   5), (Enemy.Ran,       "行符「八千万枚護摩」") },
                { (Level.Lv8,   6), (Enemy.Yuyuko,    "死符「酔人の生、死の夢幻」") },
                { (Level.Lv8,   7), (Enemy.Ran,       "超人「飛翔役小角」") },
                { (Level.Lv8,   8), (Enemy.Yuyuko,    "「死蝶浮月」") },
                { (Level.Lv9,   1), (Enemy.Eirin,     string.Empty) },
                { (Level.Lv9,   2), (Enemy.Kaguya,    "新難題「月のイルメナイト」") },
                { (Level.Lv9,   3), (Enemy.Eirin,     "薬符「胡蝶夢丸ナイトメア」") },
                { (Level.Lv9,   4), (Enemy.Kaguya,    "新難題「エイジャの赤石」") },
                { (Level.Lv9,   5), (Enemy.Eirin,     "錬丹「水銀の海」") },
                { (Level.Lv9,   6), (Enemy.Kaguya,    "新難題「金閣寺の一枚天井」") },
                { (Level.Lv9,   7), (Enemy.Eirin,     "秘薬「仙香玉兎」") },
                { (Level.Lv9,   8), (Enemy.Kaguya,    "新難題「ミステリウム」") },
                { (Level.Lv10,  1), (Enemy.Komachi,   string.Empty) },
                { (Level.Lv10,  2), (Enemy.Shikieiki, "嘘言「タン・オブ・ウルフ」") },
                { (Level.Lv10,  3), (Enemy.Komachi,   "死歌「八重霧の渡し」") },
                { (Level.Lv10,  4), (Enemy.Shikieiki, "審判「十王裁判」") },
                { (Level.Lv10,  5), (Enemy.Komachi,   "古雨「黄泉中有の旅の雨」") },
                { (Level.Lv10,  6), (Enemy.Shikieiki, "審判「ギルティ・オワ・ノットギルティ」") },
                { (Level.Lv10,  7), (Enemy.Komachi,   "死価「プライス・オブ・ライフ」") },
                { (Level.Lv10,  8), (Enemy.Shikieiki, "審判「浄頗梨審判 -射命丸文-」") },
                { (Level.Extra, 1), (Enemy.Flandre,   "禁忌「フォービドゥンフルーツ」") },
                { (Level.Extra, 2), (Enemy.Flandre,   "禁忌「禁じられた遊び」") },
                { (Level.Extra, 3), (Enemy.Yukari,    "境符「色と空の境界」") },
                { (Level.Extra, 4), (Enemy.Yukari,    "境符「波と粒の境界」") },
                { (Level.Extra, 5), (Enemy.Mokou,     "貴人「サンジェルマンの忠告」") },
                { (Level.Extra, 6), (Enemy.Mokou,     "蓬莱「瑞江浦嶋子と五色の瑞亀」") },
                { (Level.Extra, 7), (Enemy.Suika,     "鬼気「濛々迷霧」") },
                { (Level.Extra, 8), (Enemy.Suika,     "「百万鬼夜行」") },
            };

        private static new readonly EnumShortNameParser<Level> LevelParser =
            new EnumShortNameParser<Level>();

        private static readonly string LevelLongPattern =
            string.Join("|", Utils.GetEnumerator<Level>().Select(lv => lv.ToLongName()).ToArray());

        private AllScoreData allScoreData = null;

        private Dictionary<(Level Level, int Scene), BestShotPair> bestshots = null;

        public enum Level
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("1", LongName = "01")] Lv1,
            [EnumAltName("2", LongName = "02")] Lv2,
            [EnumAltName("3", LongName = "03")] Lv3,
            [EnumAltName("4", LongName = "04")] Lv4,
            [EnumAltName("5", LongName = "05")] Lv5,
            [EnumAltName("6", LongName = "06")] Lv6,
            [EnumAltName("7", LongName = "07")] Lv7,
            [EnumAltName("8", LongName = "08")] Lv8,
            [EnumAltName("9", LongName = "09")] Lv9,
            [EnumAltName("0", LongName = "10")] Lv10,
            [EnumAltName("X", LongName = "ex")] Extra,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum Enemy
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("リグル",     LongName = "リグル・ナイトバグ")]         Wriggle,
            [EnumAltName("ルーミア",   LongName = "ルーミア")]                   Rumia,
            [EnumAltName("チルノ",     LongName = "チルノ")]                     Cirno,
            [EnumAltName("レティ",     LongName = "レティ・ホワイトロック")]     Letty,
            [EnumAltName("アリス",     LongName = "アリス・マーガトロイド")]     Alice,
            [EnumAltName("慧音",       LongName = "上白沢 慧音")]                Keine,
            [EnumAltName("メディスン", LongName = "メディスン・メランコリー")]   Medicine,
            [EnumAltName("てゐ",       LongName = "因幡 てゐ")]                  Tewi,
            [EnumAltName("鈴仙",       LongName = "鈴仙・優曇華院・イナバ")]     Reisen,
            [EnumAltName("美鈴",       LongName = "紅 美鈴")]                    Meirin,
            [EnumAltName("パチュリー", LongName = "パチュリー・ノーレッジ")]     Patchouli,
            [EnumAltName("橙",         LongName = "橙")]                         Chen,
            [EnumAltName("妖夢",       LongName = "魂魄 妖夢")]                  Youmu,
            [EnumAltName("咲夜",       LongName = "十六夜 咲夜")]                Sakuya,
            [EnumAltName("レミリア",   LongName = "レミリア・スカーレット")]     Remilia,
            [EnumAltName("藍",         LongName = "八雲 藍")]                    Ran,
            [EnumAltName("幽々子",     LongName = "西行寺 幽々子")]              Yuyuko,
            [EnumAltName("永琳",       LongName = "八意 永琳")]                  Eirin,
            [EnumAltName("輝夜",       LongName = "蓬莱山 輝夜")]                Kaguya,
            [EnumAltName("小町",       LongName = "小野塚 小町")]                Komachi,
            [EnumAltName("映姫",       LongName = "四季映姫・ヤマザナドゥ")]     Shikieiki,
            [EnumAltName("フラン",     LongName = "フランドール・スカーレット")] Flandre,
            [EnumAltName("紫",         LongName = "八雲 紫")]                    Yukari,
            [EnumAltName("妹紅",       LongName = "藤原 妹紅")]                  Mokou,
            [EnumAltName("萃香",       LongName = "伊吹 萃香")]                  Suika,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public override string SupportedVersions
        {
            get { return "1.02a"; }
        }

        public override bool HasBestShotConverter
        {
            get { return true; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th095decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
                new ScoreTotalReplacer(this),
                new CardReplacer(this, hideUntriedCards),
                new ShotReplacer(this, outputFilePath),
                new ShotExReplacer(this, outputFilePath),
            };
        }

        protected override string[] FilterBestShotFiles(string[] files)
        {
            var pattern = Utils.Format(@"bs_({0})_[1-9].dat", LevelLongPattern);

            return files.Where(file => Regex.IsMatch(
                Path.GetFileName(file), pattern, RegexOptions.IgnoreCase)).ToArray();
        }

        protected override void ConvertBestShot(Stream input, Stream output)
        {
            using (var decoded = new MemoryStream())
            {
                var outputFile = output as FileStream;

                using (var reader = new BinaryReader(input, Encoding.UTF8, true))
                {
                    var header = new BestShotHeader();
                    header.ReadFrom(reader);

                    var key = (header.Level, header.Scene);
                    if (this.bestshots == null)
                        this.bestshots = new Dictionary<(Level, int), BestShotPair>(SpellCards.Count);
                    if (!this.bestshots.ContainsKey(key))
                        this.bestshots.Add(key, new BestShotPair(outputFile.Name, header));

                    Lzss.Extract(input, decoded);

                    decoded.Seek(0, SeekOrigin.Begin);
                    using (var bitmap = new Bitmap(header.Width, header.Height, PixelFormat.Format24bppRgb))
                    {
                        try
                        {
                            var permission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
                            permission.Demand();

                            var bitmapData = bitmap.LockBits(
                                new Rectangle(0, 0, header.Width, header.Height),
                                ImageLockMode.WriteOnly,
                                bitmap.PixelFormat);

                            var source = decoded.ToArray();
                            var sourceStride = 3 * header.Width;    // "3" means 24bpp.
                            var destination = bitmapData.Scan0;
                            for (var sourceIndex = 0; sourceIndex < source.Length; sourceIndex += sourceStride)
                            {
                                Marshal.Copy(source, sourceIndex, destination, sourceStride);
                                destination += bitmapData.Stride;
                            }

                            bitmap.UnlockBits(bitmapData);
                        }
                        catch (SecurityException e)
                        {
                            Console.WriteLine(e.ToString());
                        }

                        bitmap.Save(output, ImageFormat.Png);
                        output.Flush();
                        output.SetLength(output.Position);
                    }
                }
            }
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
                var chapter = new Th095.Chapter();

                try
                {
                    while (remainSize > 0)
                    {
                        chapter.ReadFrom(reader);
                        if (!chapter.IsValid)
                            return false;
                        if (!Score.CanInitialize(chapter) && !Status.CanInitialize(chapter))
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
            var dictionary = new Dictionary<string, Action<AllScoreData, Th095.Chapter>>
            {
                { Score.ValidSignature,  (data, ch) => data.Set(new Score(ch))  },
                { Status.ValidSignature, (data, ch) => data.Set(new Status(ch)) },
            };

            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var allScoreData = new AllScoreData();
                var chapter = new Th095.Chapter();

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
                    //// (allScoreData.scores.Count >= 0) &&
                    (allScoreData.Status != null))
                    return allScoreData;
                else
                    return null;
            }
        }

        // %T95SCR[x][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T95SCR({0})([1-9])([1-4])", LevelParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th095Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    var key = (level, scene);
                    if (!SpellCards.ContainsKey(key))
                        return match.ToString();

                    var score = parent.allScoreData.Scores.Find(
                        elem => (elem != null) && elem.LevelScene.Equals(key));

                    switch (type)
                    {
                        case 1:     // high score
                            return (score != null) ? Utils.ToNumberString(score.HighScore) : "0";
                        case 2:     // bestshot score
                            return (score != null) ? Utils.ToNumberString(score.BestshotScore) : "0";
                        case 3:     // num of shots
                            return (score != null) ? Utils.ToNumberString(score.TrialCount) : "0";
                        case 4:     // slow rate
                            return (score != null) ? Utils.Format("{0:F3}%", score.SlowRate2) : "-----%";
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

        // %T95SCRTL[x]
        private class ScoreTotalReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T95SCRTL([1-4])";

            private readonly MatchEvaluator evaluator;

            public ScoreTotalReplacer(Th095Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var type = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);

                    switch (type)
                    {
                        case 1:     // total score
                            return Utils.ToNumberString(
                                parent.allScoreData.Scores.Sum(
                                    score => (score != null) ? (long)score.HighScore : 0L));
                        case 2:     // total of bestshot scores
                            return Utils.ToNumberString(
                                parent.allScoreData.Scores.Sum(
                                    score => (score != null) ? (long)score.BestshotScore : 0L));
                        case 3:     // total of num of shots
                            return Utils.ToNumberString(
                                parent.allScoreData.Scores.Sum(
                                    score => (score != null) ? score.TrialCount : 0));
                        case 4:     // num of succeeded scenes
                            return parent.allScoreData.Scores
                                .Count(score => (score != null) && (score.HighScore > 0))
                                .ToString(CultureInfo.CurrentCulture);
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

        // %T95CARD[x][y][z]
        private class CardReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T95CARD({0})([1-9])([12])", LevelParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th095Converter parent, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    var key = (level, scene);
                    if (!SpellCards.ContainsKey(key))
                        return match.ToString();

                    if (hideUntriedCards)
                    {
                        var score = parent.allScoreData.Scores.Find(
                            elem => (elem != null) && elem.LevelScene.Equals(key));
                        if (score == null)
                            return "??????????";
                    }

                    return (type == 1) ? SpellCards[key].Enemy.ToLongName() : SpellCards[key].Card;
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T95SHOT[x][y]
        private class ShotReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T95SHOT({0})([1-9])", LevelParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ShotReplacer(Th095Converter parent, string outputFilePath)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                    var key = (level, scene);
                    if (!SpellCards.ContainsKey(key))
                        return match.ToString();

                    if (!string.IsNullOrEmpty(outputFilePath) && parent.bestshots.TryGetValue(key, out var bestshot))
                    {
                        var relativePath = new Uri(outputFilePath)
                            .MakeRelativeUri(new Uri(bestshot.Path)).OriginalString;
                        var alternativeString = Utils.Format(
                            "ClearData: {0}{3}Slow: {1:F6}%{3}SpellName: {2}",
                            Utils.ToNumberString(bestshot.Header.Score),
                            bestshot.Header.SlowRate,
                            Encoding.Default.GetString(bestshot.Header.CardName).TrimEnd('\0'),
                            Environment.NewLine);
                        return Utils.Format(
                            "<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" border=0>",
                            relativePath,
                            alternativeString);
                    }
                    else
                    {
                        return string.Empty;
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T95SHOTEX[x][y][z]
        private class ShotExReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T95SHOTEX({0})([1-9])([1-6])", LevelParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1513:ClosingCurlyBracketMustBeFollowedByBlankLine", Justification = "Reviewed.")]
            public ShotExReplacer(Th095Converter parent, string outputFilePath)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    var key = (level, scene);
                    if (!SpellCards.ContainsKey(key))
                        return match.ToString();

                    if (!string.IsNullOrEmpty(outputFilePath) && parent.bestshots.TryGetValue(key, out var bestshot))
                    {
                        switch (type)
                        {
                            case 1:     // relative path to the bestshot file
                                return new Uri(outputFilePath)
                                    .MakeRelativeUri(new Uri(bestshot.Path)).OriginalString;
                            case 2:     // width
                                return bestshot.Header.Width.ToString(CultureInfo.InvariantCulture);
                            case 3:     // height
                                return bestshot.Header.Height.ToString(CultureInfo.InvariantCulture);
                            case 4:     // score
                                return Utils.ToNumberString(bestshot.Header.Score);
                            case 5:     // slow rate
                                return Utils.Format("{0:F6}%", bestshot.Header.SlowRate);
                            case 6:     // date & time
                                {
                                    var score = parent.allScoreData.Scores.Find(
                                        elem => (elem != null) && elem.LevelScene.Equals(key));
                                    if (score != null)
                                    {
                                        return new DateTime(1970, 1, 1)
                                            .AddSeconds(score.DateTime).ToLocalTime()
                                            .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                                    }
                                    else
                                    {
                                        return "----/--/-- --:--:--";
                                    }
                                }
                            default:    // unreachable
                                return match.ToString();
                        }
                    }
                    else
                    {
                        switch (type)
                        {
                            case 1: return string.Empty;
                            case 2: return "0";
                            case 3: return "0";
                            case 4: return "--------";
                            case 5: return "-----%";
                            case 6: return "----/--/-- --:--:--";
                            default: return match.ToString();
                        }
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        private class BestShotPair : Tuple<string, BestShotHeader>
        {
            public BestShotPair(string path, BestShotHeader header)
                : base(path, header)
            {
            }

            public string Path => this.Item1;

            public BestShotHeader Header => this.Item2;

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public void Deconstruct(out string path, out BestShotHeader header)
            {
                path = this.Path;
                header = this.Header;
            }
        }

        private class AllScoreData
        {
            public AllScoreData() => this.Scores = new List<Score>(SpellCards.Count);

            public Header Header { get; private set; }

            public List<Score> Scores { get; private set; }

            public IStatus Status { get; private set; }

            public void Set(Header header) => this.Header = header;

            public void Set(Score score) => this.Scores.Add(score);

            public void Set(IStatus status) => this.Status = status;
        }

        private class Header : Th095.Header
        {
            public const string ValidSignature = "TH95";

            public override bool IsValid
                => base.IsValid && this.Signature.Equals(ValidSignature, StringComparison.Ordinal);
        }

        private class Score : Th095.Chapter   // per scene
        {
            public const string ValidSignature = "SC";
            public const ushort ValidVersion = 0x0001;
            public const int ValidSize = 0x00000060;

            public Score(Th095.Chapter chapter)
                : base(chapter, ValidSignature, ValidVersion, ValidSize)
            {
                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    var number = reader.ReadUInt32();
                    this.LevelScene = (Utils.ToEnum<Level>(number / 10), (int)((number % 10) + 1));
                    this.HighScore = reader.ReadInt32();
                    reader.ReadUInt32();    // always 0x00000000?
                    this.BestshotScore = reader.ReadInt32();
                    reader.ReadExactBytes(0x20);
                    this.DateTime = reader.ReadUInt32();
                    reader.ReadUInt32();    // checksum of the bestshot file?
                    this.TrialCount = reader.ReadInt32();
                    this.SlowRate1 = reader.ReadSingle();
                    this.SlowRate2 = reader.ReadSingle();
                    reader.ReadExactBytes(0x10);
                }
            }

            public (Level Level, int Scene) LevelScene { get; }

            public int HighScore { get; }

            public int BestshotScore { get; }

            public uint DateTime { get; }   // UNIX time

            public int TrialCount { get; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public float SlowRate1 { get; } // ??

            public float SlowRate2 { get; } // ??

            public static bool CanInitialize(Th095.Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class Status : Chapter, IStatus
        {
            public const string ValidSignature = "ST";
            public const ushort ValidVersion = 0x0000;
            public const int ValidSize = 0x00000458;

            public Status(Chapter chapter)
                : base(chapter, ValidSignature, ValidVersion, ValidSize)
            {
                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    this.LastName = reader.ReadExactBytes(10);
                    reader.ReadExactBytes(0x0442);
                }
            }

            public IEnumerable<byte> LastName { get; }  // The last 2 bytes are always 0x00 ?

            public static bool CanInitialize(Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class BestShotHeader : IBinaryReadable
        {
            public const string ValidSignature = "BSTS";
            public const int SignatureSize = 4;

            public string Signature { get; private set; }

            public Level Level { get; private set; }

            public short Scene { get; private set; }    // 1-based

            public short Width { get; private set; }

            public short Height { get; private set; }

            public int Score { get; private set; }

            public float SlowRate { get; private set; }

            public byte[] CardName { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader is null)
                    throw new ArgumentNullException(nameof(reader));

                this.Signature = Encoding.Default.GetString(reader.ReadExactBytes(SignatureSize));
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException();

                reader.ReadUInt16();
                this.Level = Utils.ToEnum<Level>(reader.ReadInt16() - 1);
                this.Scene = reader.ReadInt16();
                reader.ReadUInt16();    // 0x0102 ... Version?
                this.Width = reader.ReadInt16();
                this.Height = reader.ReadInt16();
                this.Score = reader.ReadInt32();
                this.SlowRate = reader.ReadSingle();
                this.CardName = reader.ReadExactBytes(0x50);
            }
        }
    }
}
