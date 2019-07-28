//-----------------------------------------------------------------------
// <copyright file="Th125Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591
#pragma warning disable SA1600 // ElementsMustBeDocumented
#pragma warning disable SA1602 // EnumerationItemsMustBeDocumented

namespace ThScoreFileConverter.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
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

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th125Converter : ThConverter
    {
        // Thanks to thwiki.info
        private static readonly Dictionary<(Level Level, int Scene), (Enemy Enemy, string Card)> SpellCards =
            new Dictionary<(Level, int), (Enemy, string)>()
            {
                { (Level.Lv1,     1), (Enemy.Minoriko,  string.Empty) },
                { (Level.Lv1,     2), (Enemy.Minoriko,  string.Empty) },
                { (Level.Lv1,     3), (Enemy.Shizuha,   "秋符「フォーリンブラスト」") },
                { (Level.Lv1,     4), (Enemy.Minoriko,  "実符「ウォームカラーハーヴェスト」") },
                { (Level.Lv1,     5), (Enemy.Shizuha,   "枯道「ロストウィンドロウ」") },
                { (Level.Lv1,     6), (Enemy.Minoriko,  "焼芋「スイートポテトルーム」") },
                { (Level.Lv2,     1), (Enemy.Parsee,    string.Empty) },
                { (Level.Lv2,     2), (Enemy.Hina,      string.Empty) },
                { (Level.Lv2,     3), (Enemy.Parsee,    "嫉妬「ジェラシーボンバー」") },
                { (Level.Lv2,     4), (Enemy.Hina,      "厄野「禊川の堆積」") },
                { (Level.Lv2,     5), (Enemy.Parsee,    "怨み念法「積怨返し」") },
                { (Level.Lv2,     6), (Enemy.Hina,      "災禍「呪いの雛人形」") },
                { (Level.Lv3,     1), (Enemy.Yamame,    string.Empty) },
                { (Level.Lv3,     2), (Enemy.Kogasa,    "傘符「一本足ピッチャー返し」") },
                { (Level.Lv3,     3), (Enemy.Kisume,    "釣瓶「飛んで井の中」") },
                { (Level.Lv3,     4), (Enemy.Yamame,    "細綱「カンダタロープ」") },
                { (Level.Lv3,     5), (Enemy.Kogasa,    "虹符「オーバー・ザ・レインボー」") },
                { (Level.Lv3,     6), (Enemy.Kisume,    "釣瓶「ウェルディストラクター」") },
                { (Level.Lv3,     7), (Enemy.Yamame,    "毒符「樺黄小町」") },
                { (Level.Lv3,     8), (Enemy.Kogasa,    "傘符「細雪の過客」") },
                { (Level.Lv4,     1), (Enemy.Nitori,    string.Empty) },
                { (Level.Lv4,     2), (Enemy.Momiji,    string.Empty) },
                { (Level.Lv4,     3), (Enemy.Nitori,    "水符「ウォーターカーペット」") },
                { (Level.Lv4,     4), (Enemy.Momiji,    "狗符「レイビーズバイト」") },
                { (Level.Lv4,     5), (Enemy.Nitori,    "河符「ディバイディングエッジ」") },
                { (Level.Lv4,     6), (Enemy.Momiji,    "山窩「エクスペリーズカナン」") },
                { (Level.Lv4,     7), (Enemy.Nitori,    "河童「乾燥尻子玉」") },
                { (Level.Lv5,     1), (Enemy.Ichirin,   string.Empty) },
                { (Level.Lv5,     2), (Enemy.Minamitsu, string.Empty) },
                { (Level.Lv5,     3), (Enemy.Ichirin,   "拳骨「天空鉄槌落とし」") },
                { (Level.Lv5,     4), (Enemy.Minamitsu, "錨符「幽霊船長期停泊」") },
                { (Level.Lv5,     5), (Enemy.Ichirin,   "稲妻「帯電入道」") },
                { (Level.Lv5,     6), (Enemy.Minamitsu, "浸水「船底のヴィーナス」") },
                { (Level.Lv5,     7), (Enemy.Ichirin,   "鉄拳「入道にょき」") },
                { (Level.Lv5,     8), (Enemy.Minamitsu, "「ディープシンカー」") },
                { (Level.Lv6,     1), (Enemy.Yuugi,     string.Empty) },
                { (Level.Lv6,     2), (Enemy.Suika,     string.Empty) },
                { (Level.Lv6,     3), (Enemy.Yuugi,     "光鬼「金剛螺旋」") },
                { (Level.Lv6,     4), (Enemy.Suika,     "鬼符「豆粒大の針地獄」") },
                { (Level.Lv6,     5), (Enemy.Yuugi,     "鬼符「鬼気狂瀾」") },
                { (Level.Lv6,     6), (Enemy.Suika,     "地獄「煉獄吐息」") },
                { (Level.Lv6,     7), (Enemy.Yuugi,     "鬼声「壊滅の咆哮」") },
                { (Level.Lv6,     8), (Enemy.Suika,     "鬼符「ミッシングパワー」") },
                { (Level.Lv7,     1), (Enemy.Shou,      string.Empty) },
                { (Level.Lv7,     2), (Enemy.Nazrin,    string.Empty) },
                { (Level.Lv7,     3), (Enemy.Shou,      "寅符「ハングリータイガー」") },
                { (Level.Lv7,     4), (Enemy.Nazrin,    "棒符「ナズーリンロッド」") },
                { (Level.Lv7,     5), (Enemy.Shou,      "天符「焦土曼荼羅」") },
                { (Level.Lv7,     6), (Enemy.Nazrin,    "財宝「ゴールドラッシュ」") },
                { (Level.Lv7,     7), (Enemy.Shou,      "宝符「黄金の震眩」") },
                { (Level.Lv8,     1), (Enemy.Rin,       string.Empty) },
                { (Level.Lv8,     2), (Enemy.Utsuho,    "熔解「メルティングホワイト」") },
                { (Level.Lv8,     3), (Enemy.Rin,       "死符「ゴーストタウン」") },
                { (Level.Lv8,     4), (Enemy.Utsuho,    "巨星「レッドジャイアント」") },
                { (Level.Lv8,     5), (Enemy.Rin,       "「死体繁華街」") },
                { (Level.Lv8,     6), (Enemy.Utsuho,    "星符「巨星墜つ」") },
                { (Level.Lv8,     7), (Enemy.Rin,       "酔歩「キャットランダムウォーク」") },
                { (Level.Lv8,     8), (Enemy.Utsuho,    "七星「セプテントリオン」") },
                { (Level.Lv9,     1), (Enemy.Satori,    string.Empty) },
                { (Level.Lv9,     2), (Enemy.Koishi,    "心符「没我の愛」") },
                { (Level.Lv9,     3), (Enemy.Satori,    "脳符「ブレインフィンガープリント」") },
                { (Level.Lv9,     4), (Enemy.Koishi,    "記憶「ＤＮＡの瑕」") },
                { (Level.Lv9,     5), (Enemy.Satori,    "心花「カメラシャイローズ」") },
                { (Level.Lv9,     6), (Enemy.Koishi,    "「胎児の夢」") },
                { (Level.Lv9,     7), (Enemy.Satori,    "想起「うろおぼえの金閣寺」") },
                { (Level.Lv9,     8), (Enemy.Koishi,    "「ローズ地獄」") },
                { (Level.Lv10,    1), (Enemy.Tenshi,    "気性「勇気凛々の剣」") },
                { (Level.Lv10,    2), (Enemy.Iku,       "雷符「ライトニングフィッシュ」") },
                { (Level.Lv10,    3), (Enemy.Tenshi,    "地震「避難険路」") },
                { (Level.Lv10,    4), (Enemy.Iku,       "珠符「五爪龍の珠」") },
                { (Level.Lv10,    5), (Enemy.Tenshi,    "要石「カナメファンネル」") },
                { (Level.Lv10,    6), (Enemy.Iku,       "龍宮「タイヤヒラメダンス」") },
                { (Level.Lv10,    7), (Enemy.Tenshi,    "「全人類の緋想天」") },
                { (Level.Lv10,    8), (Enemy.Iku,       "龍魚「龍宮の使い遊泳弾」") },
                { (Level.Lv11,    1), (Enemy.Kanako,    string.Empty) },
                { (Level.Lv11,    2), (Enemy.Suwako,    "神桜「湛えの桜吹雪」") },
                { (Level.Lv11,    3), (Enemy.Kanako,    "蛇符「グラウンドサーペント」") },
                { (Level.Lv11,    4), (Enemy.Suwako,    "姫川「プリンセスジェイドグリーン」") },
                { (Level.Lv11,    5), (Enemy.Kanako,    "御柱「メテオリックオンバシラ」") },
                { (Level.Lv11,    6), (Enemy.Suwako,    "鉄輪「ミシカルリング」") },
                { (Level.Lv11,    7), (Enemy.Kanako,    "儚道「御神渡りクロス」") },
                { (Level.Lv11,    8), (Enemy.Suwako,    "土着神「御射軍神さま」") },
                { (Level.Lv12,    1), (Enemy.Byakuren,  string.Empty) },
                { (Level.Lv12,    2), (Enemy.Nue,       "正体不明「紫鏡」") },
                { (Level.Lv12,    3), (Enemy.Byakuren,  "「遊行聖」") },
                { (Level.Lv12,    4), (Enemy.Nue,       "正体不明「赤マント青マント」") },
                { (Level.Lv12,    5), (Enemy.Byakuren,  "習合「垂迹大日如来」") },
                { (Level.Lv12,    6), (Enemy.Nue,       "正体不明「厠の花子さん」") },
                { (Level.Lv12,    7), (Enemy.Byakuren,  "「スターソードの護法」") },
                { (Level.Lv12,    8), (Enemy.Nue,       "「遊星よりの弾幕Ｘ」") },
                { (Level.Extra,   1), (Enemy.Reimu,     "お札「新聞拡張団調伏」") },
                { (Level.Extra,   2), (Enemy.Marisa,    "星符「オールトクラウド」") },
                { (Level.Extra,   3), (Enemy.Sanae,     "奇跡「弘安の神風」") },
                { (Level.Extra,   4), (Enemy.Reimu,     "結界「パパラッチ撃退結界」") },
                { (Level.Extra,   5), (Enemy.Marisa,    "天儀「オーレリーズソーラーシステム」") },
                { (Level.Extra,   6), (Enemy.Sanae,     "蛙符「手管の蝦蟇」") },
                { (Level.Extra,   7), (Enemy.Reimu,     "夢符「夢想亜空穴」") },
                { (Level.Extra,   8), (Enemy.Marisa,    "彗星「ブレイジングスター」") },
                { (Level.Extra,   9), (Enemy.Sanae,     "妖怪退治「妖力スポイラー」") },
                { (Level.Spoiler, 1), (Enemy.Hatate,    string.Empty) },
                { (Level.Spoiler, 2), (Enemy.Hatate,    "取材「姫海棠はたての練習取材」") },
                { (Level.Spoiler, 3), (Enemy.Hatate,    "連写「ラピッドショット」") },
                { (Level.Spoiler, 4), (Enemy.Hatate,    "遠眼「天狗サイコグラフィ」") },
                { (Level.Spoiler, 5), (Enemy.Aya,       string.Empty) },
                { (Level.Spoiler, 6), (Enemy.Aya,       "取材「射命丸文の圧迫取材」") },
                { (Level.Spoiler, 7), (Enemy.Aya,       "望遠「キャンディッドショット」") },
                { (Level.Spoiler, 8), (Enemy.Aya,       "速写「ファストショット」") },
                { (Level.Spoiler, 9), (Enemy.Aya,       "「幻想風靡」") },
            };

        private static new readonly EnumShortNameParser<Level> LevelParser =
            new EnumShortNameParser<Level>();

        private static readonly EnumShortNameParser<Chara> CharaParser =
            new EnumShortNameParser<Chara>();

        private static readonly string LevelLongPattern =
            string.Join("|", Utils.GetEnumerator<Level>().Select(lv => lv.ToLongName()).ToArray());

        private AllScoreData allScoreData = null;

        private Dictionary<Chara, Dictionary<(Level Level, int Scene), BestShotPair>> bestshots = null;

        public new enum Level
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
            [EnumAltName("A", LongName = "10")] Lv10,
            [EnumAltName("B", LongName = "11")] Lv11,
            [EnumAltName("C", LongName = "12")] Lv12,
            [EnumAltName("X", LongName = "ex")] Extra,
            [EnumAltName("S", LongName = "sp")] Spoiler,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum Chara
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("A")] Aya,
            [EnumAltName("H")] Hatate,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum Enemy
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("静葉",        LongName = "秋 静葉")]          Shizuha,
            [EnumAltName("穣子",        LongName = "秋 穣子")]          Minoriko,
            [EnumAltName("パルスィ",    LongName = "水橋 パルスィ")]    Parsee,
            [EnumAltName("雛",          LongName = "鍵山 雛")]          Hina,
            [EnumAltName("小傘",        LongName = "多々良 小傘")]      Kogasa,
            [EnumAltName("キスメ",      LongName = "キスメ")]           Kisume,
            [EnumAltName("ヤマメ",      LongName = "黒谷 ヤマメ")]      Yamame,
            [EnumAltName("にとり",      LongName = "河城 にとり")]      Nitori,
            [EnumAltName("椛",          LongName = "犬走 椛")]          Momiji,
            [EnumAltName("一輪 & 雲山", LongName = "雲居 一輪 & 雲山")] Ichirin,
            [EnumAltName("水蜜",        LongName = "村紗 水蜜")]        Minamitsu,
            [EnumAltName("勇儀",        LongName = "星熊 勇儀")]        Yuugi,
            [EnumAltName("萃香",        LongName = "伊吹 萃香")]        Suika,
            [EnumAltName("星",          LongName = "寅丸 星")]          Shou,
            [EnumAltName("ナズーリン",  LongName = "ナズーリン")]       Nazrin,
            [EnumAltName("お空",        LongName = "霊烏路 空")]        Utsuho,
            [EnumAltName("お燐",        LongName = "火焔猫 燐")]        Rin,
            [EnumAltName("こいし",      LongName = "古明地 こいし")]    Koishi,
            [EnumAltName("さとり",      LongName = "古明地 さとり")]    Satori,
            [EnumAltName("天子",        LongName = "比那名居 天子")]    Tenshi,
            [EnumAltName("衣玖",        LongName = "永江 衣玖")]        Iku,
            [EnumAltName("諏訪子",      LongName = "洩矢 諏訪子")]      Suwako,
            [EnumAltName("神奈子",      LongName = "八坂 神奈子")]      Kanako,
            [EnumAltName("ぬえ",        LongName = "封獣 ぬえ")]        Nue,
            [EnumAltName("白蓮",        LongName = "聖 白蓮")]          Byakuren,
            [EnumAltName("霊夢",        LongName = "博麗 霊夢")]        Reimu,
            [EnumAltName("魔理沙",      LongName = "霧雨 魔理沙")]      Marisa,
            [EnumAltName("早苗",        LongName = "東風谷 早苗")]      Sanae,
            [EnumAltName("はたて",      LongName = "姫海棠 はたて")]    Hatate,
            [EnumAltName("文",          LongName = "射命丸 文")]        Aya,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public override string SupportedVersions
        {
            get { return "1.00a"; }
        }

        public override bool HasBestShotConverter
        {
            get { return true; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th125decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
                new TimeReplacer(this),
                new ShotReplacer(this, outputFilePath),
                new ShotExReplacer(this, outputFilePath),
            };
        }

        protected override string[] FilterBestShotFiles(string[] files)
        {
            var pattern = Utils.Format(@"bs2?_({0})_[1-9].dat", LevelLongPattern);

            return files.Where(file => Regex.IsMatch(
                Path.GetFileName(file), pattern, RegexOptions.IgnoreCase)).ToArray();
        }

        protected override void ConvertBestShot(Stream input, Stream output)
        {
            using (var decoded = new MemoryStream())
            {
                var outputFile = output as FileStream;
                var chara = Path.GetFileName(outputFile.Name)
                    .StartsWith("bs2_", StringComparison.CurrentCultureIgnoreCase)
                    ? Chara.Hatate : Chara.Aya;

                using (var reader = new BinaryReader(input, Encoding.UTF8, true))
                {
                    var header = new BestShotHeader();
                    header.ReadFrom(reader);

                    if (this.bestshots == null)
                    {
                        this.bestshots = new Dictionary<Chara, Dictionary<(Level, int), BestShotPair>>(
                            Enum.GetValues(typeof(Chara)).Length);
                    }

                    if (!this.bestshots.ContainsKey(chara))
                    {
                        this.bestshots.Add(
                            chara, new Dictionary<(Level, int), BestShotPair>(SpellCards.Count));
                    }

                    var key = (header.Level, header.Scene);
                    if (!this.bestshots[chara].ContainsKey(key))
                        this.bestshots[chara].Add(key, new BestShotPair(outputFile.Name, header));

                    Lzss.Extract(input, decoded);

                    decoded.Seek(0, SeekOrigin.Begin);
                    using (var bitmap = new Bitmap(header.Width, header.Height, PixelFormat.Format32bppArgb))
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
                            var destination = bitmapData.Scan0;
                            Marshal.Copy(source, 0, destination, source.Length);
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
                        if (dictionary.TryGetValue(chapter.Signature, out Action<AllScoreData, Th095.Chapter> setChapter))
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

        // %T125SCR[w][x][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T125SCR({0})({1})([1-9])([1-5])", CharaParser.Pattern, LevelParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th125Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var chara = CharaParser.Parse(match.Groups[1].Value);
                    var level = LevelParser.Parse(match.Groups[2].Value);
                    var scene = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    var key = (level, scene);
                    if (!SpellCards.ContainsKey(key))
                        return match.ToString();

                    var score = parent.allScoreData.Scores.Find(elem =>
                        (elem != null) && (elem.Chara == chara) && elem.LevelScene.Equals(key));

                    switch (type)
                    {
                        case 1:     // high score
                            return (score != null) ? Utils.ToNumberString(score.HighScore) : "0";
                        case 2:     // bestshot score
                            return (score != null) ? Utils.ToNumberString(score.BestshotScore) : "0";
                        case 3:     // num of shots
                            return (score != null) ? Utils.ToNumberString(score.TrialCount) : "0";
                        case 4:     // num of shots for the first success
                            return (score != null) ? Utils.ToNumberString(score.FirstSuccess) : "0";
                        case 5:     // date & time
                            return (score != null)
                                ? new DateTime(1970, 1, 1).AddSeconds(score.DateTime).ToLocalTime()
                                    .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture)
                                : "----/--/-- --:--:--";
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

        // %T125SCRTL[x][y][z]
        private class ScoreTotalReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T125SCRTL({0})([12])([1-5])", CharaParser.Pattern);

            private static readonly Func<Score, Chara, int, bool> IsTargetImpl =
                (score, chara, method) =>
                {
                    if (score == null)
                        return false;

                    if (method == 1)
                    {
                        if (score.LevelScene.Level == Level.Spoiler)
                        {
                            if (chara == Chara.Aya)
                            {
                                if (score.LevelScene.Scene <= 4)
                                    return score.Chara == Chara.Aya;
                                else
                                    return score.Chara == Chara.Hatate;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return score.Chara == chara;
                        }
                    }
                    else
                    {
                        return score.Chara == chara;
                    }
                };

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public ScoreTotalReplacer(Th125Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var chara = CharaParser.Parse(match.Groups[1].Value);
                    var method = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    Func<Score, bool> isTarget = (score => IsTargetImpl(score, chara, method));
                    Func<Score, bool> triedAndSucceeded =
                        (score => isTarget(score) && (score.TrialCount > 0) && (score.FirstSuccess > 0));

                    switch (type)
                    {
                        case 1:     // total score
                            return Utils.ToNumberString(
                                parent.allScoreData.Scores.Sum(
                                    score => triedAndSucceeded(score) ? (long)score.HighScore : 0L));
                        case 2:     // total of bestshot scores
                            return Utils.ToNumberString(
                                parent.allScoreData.Scores.Sum(
                                    score => isTarget(score) ? (long)score.BestshotScore : 0L));
                        case 3:     // total of num of shots
                            return Utils.ToNumberString(
                                parent.allScoreData.Scores.Sum(
                                    score => isTarget(score) ? score.TrialCount : 0));
                        case 4:     // total of num of shots for the first success
                            return Utils.ToNumberString(
                                parent.allScoreData.Scores.Sum(
                                    score => triedAndSucceeded(score) ? (long)score.FirstSuccess : 0L));
                        case 5:     // num of succeeded scenes
                            return parent.allScoreData.Scores
                                .Count(triedAndSucceeded)
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

        // %T125CARD[x][y][z]
        private class CardReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T125CARD({0})([1-9])([12])", LevelParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th125Converter parent, bool hideUntriedCards)
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

        // %T125TIMEPLY
        private class TimeReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T125TIMEPLY";

            private readonly MatchEvaluator evaluator;

            public TimeReplacer(Th125Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    return new Time(parent.allScoreData.Status.TotalPlayTime * 10, false).ToLongString();
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T125SHOT[x][y][z]
        private class ShotReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T125SHOT({0})({1})([1-9])", CharaParser.Pattern, LevelParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ShotReplacer(Th125Converter parent, string outputFilePath)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var chara = CharaParser.Parse(match.Groups[1].Value);
                    var level = LevelParser.Parse(match.Groups[2].Value);
                    var scene = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    var key = (level, scene);
                    if (!SpellCards.ContainsKey(key))
                        return match.ToString();

                    if (!string.IsNullOrEmpty(outputFilePath) &&
                        parent.bestshots.TryGetValue(chara, out var bestshots) &&
                        bestshots.TryGetValue(key, out var bestshot))
                    {
                        var relativePath = new Uri(outputFilePath)
                            .MakeRelativeUri(new Uri(bestshot.Path)).OriginalString;
                        var alternativeString = Utils.Format(
                            "ClearData: {0}{3}Slow: {1:F6}%{3}SpellName: {2}",
                            Utils.ToNumberString(bestshot.Header.ResultScore),
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

        // %T125SHOTEX[w][x][y][z]
        private class ShotExReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T125SHOTEX({0})({1})([1-9])([1-7])", CharaParser.Pattern, LevelParser.Pattern);

            private static readonly Func<BestShotHeader, List<Detail>> DetailList =
                header => new List<Detail>
                {
                    new Detail(true,                       "Base Point    {0,9}", Utils.ToNumberString(header.BasePoint)),
                    new Detail(header.Fields.ClearShot,    "Clear Shot!   {0,9}", Utils.Format("+ {0}", header.ClearShot)),
                    new Detail(header.Fields.SoloShot,     "Solo Shot     {0,9}", "+ 100"),
                    new Detail(header.Fields.RedShot,      "Red Shot      {0,9}", "+ 300"),
                    new Detail(header.Fields.PurpleShot,   "Purple Shot   {0,9}", "+ 300"),
                    new Detail(header.Fields.BlueShot,     "Blue Shot     {0,9}", "+ 300"),
                    new Detail(header.Fields.CyanShot,     "Cyan Shot     {0,9}", "+ 300"),
                    new Detail(header.Fields.GreenShot,    "Green Shot    {0,9}", "+ 300"),
                    new Detail(header.Fields.YellowShot,   "Yellow Shot   {0,9}", "+ 300"),
                    new Detail(header.Fields.OrangeShot,   "Orange Shot   {0,9}", "+ 300"),
                    new Detail(header.Fields.ColorfulShot, "Colorful Shot {0,9}", "+ 900"),
                    new Detail(header.Fields.RainbowShot,  "Rainbow Shot  {0,9}", Utils.Format("+ {0}", Utils.ToNumberString(2100))),
                    new Detail(header.Fields.RiskBonus,    "Risk Bonus    {0,9}", Utils.Format("+ {0}", Utils.ToNumberString(header.RiskBonus))),
                    new Detail(header.Fields.MacroBonus,   "Macro Bonus   {0,9}", Utils.Format("+ {0}", Utils.ToNumberString(header.MacroBonus))),
                    new Detail(header.Fields.FrontShot,    "Front Shot    {0,9}", Utils.Format("+ {0}", header.FrontSideBackShot)),
                    new Detail(header.Fields.SideShot,     "Side Shot     {0,9}", Utils.Format("+ {0}", header.FrontSideBackShot)),
                    new Detail(header.Fields.BackShot,     "Back Shot     {0,9}", Utils.Format("+ {0}", header.FrontSideBackShot)),
                    new Detail(header.Fields.CatBonus,     "Cat Bonus     {0,9}", "+ 666"),
                    new Detail(true,                       string.Empty,          string.Empty),
                    new Detail(true,                       "Boss Shot!    {0,9}", Utils.Format("* {0:F2}", header.BossShot)),
                    new Detail(header.Fields.TwoShot,      "Two Shot!     {0,9}", "* 1.50"),
                    new Detail(header.Fields.NiceShot,     "Nice Shot!    {0,9}", Utils.Format("* {0:F2}", header.NiceShot)),
                    new Detail(true,                       "Angle Bonus   {0,9}", Utils.Format("* {0:F2}", header.AngleBonus)),
                    new Detail(true,                       string.Empty,          string.Empty),
                    new Detail(true,                       "Result Score  {0,9}", Utils.ToNumberString(header.ResultScore)),
                };

            private readonly MatchEvaluator evaluator;

            public ShotExReplacer(Th125Converter parent, string outputFilePath)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var chara = CharaParser.Parse(match.Groups[1].Value);
                    var level = LevelParser.Parse(match.Groups[2].Value);
                    var scene = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    var key = (level, scene);
                    if (!SpellCards.ContainsKey(key))
                        return match.ToString();

                    if (!string.IsNullOrEmpty(outputFilePath) &&
                        parent.bestshots.TryGetValue(chara, out var bestshots) &&
                        bestshots.TryGetValue(key, out var bestshot))
                    {
                        Score score;
                        IEnumerable<string> detailStrings;
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
                                return Utils.ToNumberString(bestshot.Header.ResultScore);
                            case 5:     // slow rate
                                return Utils.Format("{0:F6}%", bestshot.Header.SlowRate);
                            case 6:     // date & time
                                score = parent.allScoreData.Scores.Find(elem =>
                                    (elem != null) &&
                                    (elem.Chara == chara) &&
                                    elem.LevelScene.Equals(key));
                                if (score == null)
                                    return "----/--/-- --:--:--";
                                return new DateTime(1970, 1, 1)
                                    .AddSeconds(score.DateTime).ToLocalTime()
                                    .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                            case 7:     // detail info
                                detailStrings = DetailList(bestshot.Header)
                                    .Where(detail => detail.Outputs)
                                    .Select(detail => Utils.Format(detail.Format, detail.Value));
                                return string.Join(Environment.NewLine, detailStrings.ToArray());
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
                            case 7: return string.Empty;
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
            public AllScoreData()
            {
                this.Scores = new List<Score>(SpellCards.Count);
            }

            public Header Header { get; private set; }

            public List<Score> Scores { get; private set; }

            public Status Status { get; private set; }

            public void Set(Header header)
            {
                this.Header = header;
            }

            public void Set(Score score)
            {
                this.Scores.Add(score);
            }

            public void Set(Status status)
            {
                this.Status = status;
            }
        }

        private class Header : Th095.Header
        {
            public const string ValidSignature = "T125";

            public override bool IsValid
                => base.IsValid && this.Signature.Equals(ValidSignature, StringComparison.Ordinal);
        }

        private class Score : Th095.Chapter   // per scene
        {
            public const string ValidSignature = "SC";
            public const ushort ValidVersion = 0x0000;
            public const int ValidSize = 0x00000048;

            public Score(Th095.Chapter chapter)
                : base(chapter, ValidSignature, ValidVersion, ValidSize)
            {
                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    var number = reader.ReadUInt32();
                    this.LevelScene = (Utils.ToEnum<Level>(number / 10), (int)((number % 10) + 1));
                    this.HighScore = reader.ReadInt32();
                    reader.ReadExactBytes(0x04);
                    this.Chara = Utils.ToEnum<Chara>(reader.ReadInt32());
                    reader.ReadExactBytes(0x04);
                    this.TrialCount = reader.ReadInt32();
                    this.FirstSuccess = reader.ReadInt32();
                    reader.ReadUInt32();    // always 0x00000000?
                    this.DateTime = reader.ReadUInt32();
                    reader.ReadUInt32();    // always 0x00000000?
                    reader.ReadUInt32();    // checksum of the bestshot file?
                    reader.ReadUInt32();    // always 0x00000001?
                    this.BestshotScore = reader.ReadInt32();
                    reader.ReadExactBytes(0x08);
                }
            }

            public (Level Level, int Scene) LevelScene { get; private set; }

            public int HighScore { get; private set; }

            public Chara Chara { get; private set; }        // size: 4Bytes

            public int TrialCount { get; private set; }

            public int FirstSuccess { get; private set; }

            public uint DateTime { get; private set; }      // UNIX time (unit: [s])

            public int BestshotScore { get; private set; }

            public static bool CanInitialize(Th095.Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class Status : Th095.Chapter
        {
            public const string ValidSignature = "ST";
            public const ushort ValidVersion = 0x0001;
            public const int ValidSize = 0x00000474;

            public Status(Th095.Chapter chapter)
                : base(chapter, ValidSignature, ValidVersion, ValidSize)
            {
                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    this.LastName = reader.ReadExactBytes(10);
                    reader.ReadExactBytes(2);
                    this.BgmFlags = reader.ReadExactBytes(6);
                    reader.ReadExactBytes(0x2E);
                    this.TotalPlayTime = reader.ReadInt32();
                    reader.ReadExactBytes(0x424);
                }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] LastName { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] BgmFlags { get; private set; }    // .Length = 6

            public int TotalPlayTime { get; private set; }  // unit: [0.01s]

            public static bool CanInitialize(Th095.Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class BestShotHeader : IBinaryReadable
        {
            public const string ValidSignature = "BST2";
            public const int SignatureSize = 4;

            public string Signature { get; private set; }

            public Level Level { get; private set; }

            public short Scene { get; private set; }        // 1-based

            public short Width { get; private set; }

            public short Height { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public short Width2 { get; private set; }       // ???

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public short Height2 { get; private set; }      // ???

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public short HalfWidth { get; private set; }    // ???

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public short HalfHeight { get; private set; }   // ???

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public uint DateTime { get; private set; }

            public float SlowRate { get; private set; }     // Really...?

            public BonusFields Fields { get; private set; }

            public int ResultScore { get; private set; }

            public int BasePoint { get; private set; }

            public int RiskBonus { get; private set; }

            public float BossShot { get; private set; }

            public float NiceShot { get; private set; }     // minimum = 1.20?

            public float AngleBonus { get; private set; }

            public int MacroBonus { get; private set; }

            public int FrontSideBackShot { get; private set; }  // Really...?

            public int ClearShot { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public float Angle { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int ResultScore2 { get; private set; }   // ???

            public byte[] CardName { get; private set; }    // .Length = 0x50

            public void ReadFrom(BinaryReader reader)
            {
                if (reader is null)
                    throw new ArgumentNullException(nameof(reader));

                this.Signature = Encoding.Default.GetString(reader.ReadExactBytes(SignatureSize));
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException();

                reader.ReadUInt16();    // always 0x0405?
                this.Level = Utils.ToEnum<Level>(reader.ReadInt16() - 1);
                this.Scene = reader.ReadInt16();
                reader.ReadUInt16();    // 0x0100 ... Version?
                this.Width = reader.ReadInt16();
                this.Height = reader.ReadInt16();
                reader.ReadUInt32();    // always 0x00000000?
                this.Width2 = reader.ReadInt16();
                this.Height2 = reader.ReadInt16();
                this.HalfWidth = reader.ReadInt16();
                this.HalfHeight = reader.ReadInt16();
                this.DateTime = reader.ReadUInt32();
                reader.ReadUInt32();    // always 0x00000000?
                this.SlowRate = reader.ReadSingle();
                this.Fields = new BonusFields(reader.ReadInt32());
                this.ResultScore = reader.ReadInt32();
                this.BasePoint = reader.ReadInt32();
                reader.ReadExactBytes(0x08);
                this.RiskBonus = reader.ReadInt32();
                this.BossShot = reader.ReadSingle();
                this.NiceShot = reader.ReadSingle();
                this.AngleBonus = reader.ReadSingle();
                this.MacroBonus = reader.ReadInt32();
                this.FrontSideBackShot = reader.ReadInt32();
                this.ClearShot = reader.ReadInt32();
                reader.ReadExactBytes(0x30);
                this.Angle = reader.ReadSingle();
                this.ResultScore2 = reader.ReadInt32();
                reader.ReadUInt32();
                this.CardName = reader.ReadExactBytes(0x50);
            }

            public struct BonusFields
            {
                private BitVector32 data;

                public BonusFields(int data)
                {
                    this.data = new BitVector32(data);
                }

                [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
                public int Data
                {
                    get { return this.data.Data; }
                }

                public bool TwoShot
                {
                    get { return this.data[0x00000004]; }
                }

                public bool NiceShot
                {
                    get { return this.data[0x00000008]; }
                }

                public bool RiskBonus
                {
                    get { return this.data[0x00000010]; }
                }

                public bool RedShot
                {
                    get { return this.data[0x00000040]; }
                }

                public bool PurpleShot
                {
                    get { return this.data[0x00000080]; }
                }

                public bool BlueShot
                {
                    get { return this.data[0x00000100]; }
                }

                public bool CyanShot
                {
                    get { return this.data[0x00000200]; }
                }

                public bool GreenShot
                {
                    get { return this.data[0x00000400]; }
                }

                public bool YellowShot
                {
                    get { return this.data[0x00000800]; }
                }

                public bool OrangeShot
                {
                    get { return this.data[0x00001000]; }
                }

                public bool ColorfulShot
                {
                    get { return this.data[0x00002000]; }
                }

                public bool RainbowShot
                {
                    get { return this.data[0x00004000]; }
                }

                public bool SoloShot
                {
                    get { return this.data[0x00010000]; }
                }

                public bool MacroBonus
                {
                    get { return this.data[0x00400000]; }
                }

                public bool FrontShot
                {
                    get { return this.data[0x01000000]; }
                }

                public bool BackShot
                {
                    get { return this.data[0x02000000]; }
                }

                public bool SideShot
                {
                    get { return this.data[0x04000000]; }
                }

                public bool ClearShot
                {
                    get { return this.data[0x08000000]; }
                }

                public bool CatBonus
                {
                    get { return this.data[0x10000000]; }
                }
            }
        }

        private class Detail
        {
            public Detail(bool outputs, string format, string value)
            {
                this.Outputs = outputs;
                this.Format = format;
                this.Value = value;
            }

            public bool Outputs { get; private set; }

            public string Format { get; private set; }

            public string Value { get; private set; }
        }
    }
}
