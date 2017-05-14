//-----------------------------------------------------------------------
// <copyright file="Th095Converter.cs" company="None">
//     (c) 2013-2015 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed.")]

namespace ThScoreFileConverter.Models
{
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
    using System.Text;
    using System.Text.RegularExpressions;

    internal class Th095Converter : ThConverter
    {
        // Thanks to thwiki.info
        private static readonly Dictionary<LevelScenePair, EnemyCardPair> SpellCards =
            new Dictionary<LevelScenePair, EnemyCardPair>()
            {
                { new LevelScenePair(Level.Lv1,   1), new EnemyCardPair(Enemy.Wriggle,   string.Empty) },
                { new LevelScenePair(Level.Lv1,   2), new EnemyCardPair(Enemy.Rumia,     string.Empty) },
                { new LevelScenePair(Level.Lv1,   3), new EnemyCardPair(Enemy.Wriggle,   "蛍符「地上の恒星」") },
                { new LevelScenePair(Level.Lv1,   4), new EnemyCardPair(Enemy.Rumia,     "闇符「ダークサイドオブザムーン」") },
                { new LevelScenePair(Level.Lv1,   5), new EnemyCardPair(Enemy.Wriggle,   "蝶符「バタフライストーム」") },
                { new LevelScenePair(Level.Lv1,   6), new EnemyCardPair(Enemy.Rumia,     "夜符「ミッドナイトバード」") },
                { new LevelScenePair(Level.Lv2,   1), new EnemyCardPair(Enemy.Cirno,     string.Empty) },
                { new LevelScenePair(Level.Lv2,   2), new EnemyCardPair(Enemy.Letty,     string.Empty) },
                { new LevelScenePair(Level.Lv2,   3), new EnemyCardPair(Enemy.Cirno,     "雪符「ダイアモンドブリザード」") },
                { new LevelScenePair(Level.Lv2,   4), new EnemyCardPair(Enemy.Letty,     "寒符「コールドスナップ」") },
                { new LevelScenePair(Level.Lv2,   5), new EnemyCardPair(Enemy.Cirno,     "凍符「マイナスＫ」") },
                { new LevelScenePair(Level.Lv2,   6), new EnemyCardPair(Enemy.Letty,     "冬符「ノーザンウイナー」") },
                { new LevelScenePair(Level.Lv3,   1), new EnemyCardPair(Enemy.Alice,     string.Empty) },
                { new LevelScenePair(Level.Lv3,   2), new EnemyCardPair(Enemy.Keine,     "光符「アマテラス」") },
                { new LevelScenePair(Level.Lv3,   3), new EnemyCardPair(Enemy.Alice,     "操符「ドールズインシー」") },
                { new LevelScenePair(Level.Lv3,   4), new EnemyCardPair(Enemy.Keine,     "包符「昭和の雨」") },
                { new LevelScenePair(Level.Lv3,   5), new EnemyCardPair(Enemy.Alice,     "呪符「ストロードールカミカゼ」") },
                { new LevelScenePair(Level.Lv3,   6), new EnemyCardPair(Enemy.Keine,     "葵符「水戸の光圀」") },
                { new LevelScenePair(Level.Lv3,   7), new EnemyCardPair(Enemy.Alice,     "赤符「ドールミラセティ」") },
                { new LevelScenePair(Level.Lv3,   8), new EnemyCardPair(Enemy.Keine,     "倭符「邪馬台の国」") },
                { new LevelScenePair(Level.Lv4,   1), new EnemyCardPair(Enemy.Reisen,    string.Empty) },
                { new LevelScenePair(Level.Lv4,   2), new EnemyCardPair(Enemy.Medicine,  "霧符「ガシングガーデン」") },
                { new LevelScenePair(Level.Lv4,   3), new EnemyCardPair(Enemy.Tewi,      "脱兎「フラスターエスケープ」") },
                { new LevelScenePair(Level.Lv4,   4), new EnemyCardPair(Enemy.Reisen,    "散符「朧月花栞（ロケット・イン・ミスト）」") },
                { new LevelScenePair(Level.Lv4,   5), new EnemyCardPair(Enemy.Medicine,  "毒符「ポイズンブレス」") },
                { new LevelScenePair(Level.Lv4,   6), new EnemyCardPair(Enemy.Reisen,    "波符「幻の月（インビジブルハーフムーン）」") },
                { new LevelScenePair(Level.Lv4,   7), new EnemyCardPair(Enemy.Medicine,  "譫妄「イントゥデリリウム」") },
                { new LevelScenePair(Level.Lv4,   8), new EnemyCardPair(Enemy.Tewi,      "借符「大穴牟遅様の薬」") },
                { new LevelScenePair(Level.Lv4,   9), new EnemyCardPair(Enemy.Reisen,    "狂夢「風狂の夢（ドリームワールド）」") },
                { new LevelScenePair(Level.Lv5,   1), new EnemyCardPair(Enemy.Meirin,    string.Empty) },
                { new LevelScenePair(Level.Lv5,   2), new EnemyCardPair(Enemy.Patchouli, "日＆水符「ハイドロジェナスプロミネンス」") },
                { new LevelScenePair(Level.Lv5,   3), new EnemyCardPair(Enemy.Meirin,    "華符「彩光蓮華掌」") },
                { new LevelScenePair(Level.Lv5,   4), new EnemyCardPair(Enemy.Patchouli, "水＆火符「フロギスティックレイン」") },
                { new LevelScenePair(Level.Lv5,   5), new EnemyCardPair(Enemy.Meirin,    "彩翔「飛花落葉」") },
                { new LevelScenePair(Level.Lv5,   6), new EnemyCardPair(Enemy.Patchouli, "月＆木符「サテライトヒマワリ」") },
                { new LevelScenePair(Level.Lv5,   7), new EnemyCardPair(Enemy.Meirin,    "彩華「虹色太極拳」") },
                { new LevelScenePair(Level.Lv5,   8), new EnemyCardPair(Enemy.Patchouli, "日＆月符「ロイヤルダイアモンドリング」") },
                { new LevelScenePair(Level.Lv6,   1), new EnemyCardPair(Enemy.Chen,      string.Empty) },
                { new LevelScenePair(Level.Lv6,   2), new EnemyCardPair(Enemy.Youmu,     "人智剣「天女返し」") },
                { new LevelScenePair(Level.Lv6,   3), new EnemyCardPair(Enemy.Chen,      "星符「飛び重ね鱗」") },
                { new LevelScenePair(Level.Lv6,   4), new EnemyCardPair(Enemy.Youmu,     "妄執剣「修羅の血」") },
                { new LevelScenePair(Level.Lv6,   5), new EnemyCardPair(Enemy.Chen,      "鬼神「鳴動持国天」") },
                { new LevelScenePair(Level.Lv6,   6), new EnemyCardPair(Enemy.Youmu,     "天星剣「涅槃寂静の如し」") },
                { new LevelScenePair(Level.Lv6,   7), new EnemyCardPair(Enemy.Chen,      "化猫「橙」") },
                { new LevelScenePair(Level.Lv6,   8), new EnemyCardPair(Enemy.Youmu,     "四生剣「衆生無情の響き」") },
                { new LevelScenePair(Level.Lv7,   1), new EnemyCardPair(Enemy.Sakuya,    string.Empty) },
                { new LevelScenePair(Level.Lv7,   2), new EnemyCardPair(Enemy.Remilia,   "魔符「全世界ナイトメア」") },
                { new LevelScenePair(Level.Lv7,   3), new EnemyCardPair(Enemy.Sakuya,    "時符「トンネルエフェクト」") },
                { new LevelScenePair(Level.Lv7,   4), new EnemyCardPair(Enemy.Remilia,   "紅符「ブラッディマジックスクウェア」") },
                { new LevelScenePair(Level.Lv7,   5), new EnemyCardPair(Enemy.Sakuya,    "空虚「インフレーションスクウェア」") },
                { new LevelScenePair(Level.Lv7,   6), new EnemyCardPair(Enemy.Remilia,   "紅蝙蝠「ヴァンピリッシュナイト」") },
                { new LevelScenePair(Level.Lv7,   7), new EnemyCardPair(Enemy.Sakuya,    "銀符「パーフェクトメイド」") },
                { new LevelScenePair(Level.Lv7,   8), new EnemyCardPair(Enemy.Remilia,   "神鬼「レミリアストーカー」") },
                { new LevelScenePair(Level.Lv8,   1), new EnemyCardPair(Enemy.Ran,       string.Empty) },
                { new LevelScenePair(Level.Lv8,   2), new EnemyCardPair(Enemy.Yuyuko,    "幽雅「死出の誘蛾灯」") },
                { new LevelScenePair(Level.Lv8,   3), new EnemyCardPair(Enemy.Ran,       "密符「御大師様の秘鍵」") },
                { new LevelScenePair(Level.Lv8,   4), new EnemyCardPair(Enemy.Yuyuko,    "蝶符「鳳蝶紋の死槍」") },
                { new LevelScenePair(Level.Lv8,   5), new EnemyCardPair(Enemy.Ran,       "行符「八千万枚護摩」") },
                { new LevelScenePair(Level.Lv8,   6), new EnemyCardPair(Enemy.Yuyuko,    "死符「酔人の生、死の夢幻」") },
                { new LevelScenePair(Level.Lv8,   7), new EnemyCardPair(Enemy.Ran,       "超人「飛翔役小角」") },
                { new LevelScenePair(Level.Lv8,   8), new EnemyCardPair(Enemy.Yuyuko,    "「死蝶浮月」") },
                { new LevelScenePair(Level.Lv9,   1), new EnemyCardPair(Enemy.Eirin,     string.Empty) },
                { new LevelScenePair(Level.Lv9,   2), new EnemyCardPair(Enemy.Kaguya,    "新難題「月のイルメナイト」") },
                { new LevelScenePair(Level.Lv9,   3), new EnemyCardPair(Enemy.Eirin,     "薬符「胡蝶夢丸ナイトメア」") },
                { new LevelScenePair(Level.Lv9,   4), new EnemyCardPair(Enemy.Kaguya,    "新難題「エイジャの赤石」") },
                { new LevelScenePair(Level.Lv9,   5), new EnemyCardPair(Enemy.Eirin,     "錬丹「水銀の海」") },
                { new LevelScenePair(Level.Lv9,   6), new EnemyCardPair(Enemy.Kaguya,    "新難題「金閣寺の一枚天井」") },
                { new LevelScenePair(Level.Lv9,   7), new EnemyCardPair(Enemy.Eirin,     "秘薬「仙香玉兎」") },
                { new LevelScenePair(Level.Lv9,   8), new EnemyCardPair(Enemy.Kaguya,    "新難題「ミステリウム」") },
                { new LevelScenePair(Level.Lv10,  1), new EnemyCardPair(Enemy.Komachi,   string.Empty) },
                { new LevelScenePair(Level.Lv10,  2), new EnemyCardPair(Enemy.Shikieiki, "嘘言「タン・オブ・ウルフ」") },
                { new LevelScenePair(Level.Lv10,  3), new EnemyCardPair(Enemy.Komachi,   "死歌「八重霧の渡し」") },
                { new LevelScenePair(Level.Lv10,  4), new EnemyCardPair(Enemy.Shikieiki, "審判「十王裁判」") },
                { new LevelScenePair(Level.Lv10,  5), new EnemyCardPair(Enemy.Komachi,   "古雨「黄泉中有の旅の雨」") },
                { new LevelScenePair(Level.Lv10,  6), new EnemyCardPair(Enemy.Shikieiki, "審判「ギルティ・オワ・ノットギルティ」") },
                { new LevelScenePair(Level.Lv10,  7), new EnemyCardPair(Enemy.Komachi,   "死価「プライス・オブ・ライフ」") },
                { new LevelScenePair(Level.Lv10,  8), new EnemyCardPair(Enemy.Shikieiki, "審判「浄頗梨審判 -射命丸文-」") },
                { new LevelScenePair(Level.Extra, 1), new EnemyCardPair(Enemy.Flandre,   "禁忌「フォービドゥンフルーツ」") },
                { new LevelScenePair(Level.Extra, 2), new EnemyCardPair(Enemy.Flandre,   "禁忌「禁じられた遊び」") },
                { new LevelScenePair(Level.Extra, 3), new EnemyCardPair(Enemy.Yukari,    "境符「色と空の境界」") },
                { new LevelScenePair(Level.Extra, 4), new EnemyCardPair(Enemy.Yukari,    "境符「波と粒の境界」") },
                { new LevelScenePair(Level.Extra, 5), new EnemyCardPair(Enemy.Mokou,     "貴人「サンジェルマンの忠告」") },
                { new LevelScenePair(Level.Extra, 6), new EnemyCardPair(Enemy.Mokou,     "蓬莱「瑞江浦嶋子と五色の瑞亀」") },
                { new LevelScenePair(Level.Extra, 7), new EnemyCardPair(Enemy.Suika,     "鬼気「濛々迷霧」") },
                { new LevelScenePair(Level.Extra, 8), new EnemyCardPair(Enemy.Suika,     "「百万鬼夜行」") }
            };

        private static readonly new EnumShortNameParser<Level> LevelParser =
            new EnumShortNameParser<Level>();

        private static readonly string LevelLongPattern =
            string.Join("|", Utils.GetEnumerator<Level>().Select(lv => lv.ToLongName()).ToArray());

        private AllScoreData allScoreData = null;

        private Dictionary<LevelScenePair, BestShotPair> bestshots = null;

        public new enum Level
        {
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
            [EnumAltName("X", LongName = "ex")] Extra
        }

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        public enum Enemy
        {
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
            [EnumAltName("萃香",       LongName = "伊吹 萃香")]                  Suika
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
                new ShotExReplacer(this, outputFilePath)
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

                var reader = new BinaryReader(input);
                var header = new BestShotHeader();
                header.ReadFrom(reader);

                var key = new LevelScenePair(header.Level, header.Scene);
                if (this.bestshots == null)
                    this.bestshots = new Dictionary<LevelScenePair, BestShotPair>(SpellCards.Count);
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
                            destination = destination + bitmapData.Stride;
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

        private static bool Decrypt(Stream input, Stream output)
        {
            var reader = new BinaryReader(input);
            var writer = new BinaryWriter(output);

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

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        private static AllScoreData Read(Stream input)
        {
            var dictionary = new Dictionary<string, Action<AllScoreData, Chapter>>
            {
                { Score.ValidSignature,  (data, ch) => data.Set(new Score(ch))  },
                { Status.ValidSignature, (data, ch) => data.Set(new Status(ch)) }
            };

            var reader = new BinaryReader(input);
            var allScoreData = new AllScoreData();
            var chapter = new Chapter();

            var header = new Header();
            header.ReadFrom(reader);
            allScoreData.Set(header);

            try
            {
                Action<AllScoreData, Chapter> setChapter;
                while (true)
                {
                    chapter.ReadFrom(reader);
                    if (dictionary.TryGetValue(chapter.Signature, out setChapter))
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

                    var key = new LevelScenePair(level, scene);
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

                    var key = new LevelScenePair(level, scene);
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

                    var key = new LevelScenePair(level, scene);
                    if (!SpellCards.ContainsKey(key))
                        return match.ToString();

                    BestShotPair bestshot;
                    if (!string.IsNullOrEmpty(outputFilePath) &&
                        parent.bestshots.TryGetValue(key, out bestshot))
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
                        return string.Empty;
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

                    var key = new LevelScenePair(level, scene);
                    if (!SpellCards.ContainsKey(key))
                        return match.ToString();

                    BestShotPair bestshot;
                    if (!string.IsNullOrEmpty(outputFilePath) &&
                        parent.bestshots.TryGetValue(key, out bestshot))
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
                                        return new DateTime(1970, 1, 1)
                                            .AddSeconds(score.DateTime).ToLocalTime()
                                            .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                                    else
                                        return "----/--/-- --:--:--";
                                }
                            default:    // unreachable
                                return match.ToString();
                        }
                    else
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
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        private class LevelScenePair : Pair<Level, int>
        {
            public LevelScenePair(Level level, int scene)
                : base(level, scene)
            {
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Level Level
            {
                get { return this.First; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int Scene
            {
                get { return this.Second; }     // 1-based
            }
        }

        private class EnemyCardPair : Pair<Enemy, string>
        {
            public EnemyCardPair(Enemy enemy, string card)
                : base(enemy, card)
            {
            }

            public Enemy Enemy
            {
                get { return this.First; }
            }

            public string Card
            {
                get { return this.Second; }
            }
        }

        private class BestShotPair : Pair<string, BestShotHeader>
        {
            public BestShotPair(string name, BestShotHeader header)
                : base(name, header)
            {
            }

            public string Path
            {
                get { return this.First; }
            }

            public BestShotHeader Header
            {
                get { return this.Second; }
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

        private class Header : IBinaryReadable, IBinaryWritable
        {
            public const string ValidSignature = "TH95";
            public const int SignatureSize = 4;
            public const int Size = SignatureSize + (sizeof(int) * 3) + (sizeof(uint) * 2);

            private uint unknown1;
            private uint unknown2;

            public string Signature { get; private set; }

            public int EncodedAllSize { get; private set; }

            public int EncodedBodySize { get; private set; }

            public int DecodedBodySize { get; private set; }

            public bool IsValid
            {
                get
                {
                    return this.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                        && (this.EncodedAllSize - this.EncodedBodySize == Size);
                }
            }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException("reader");

                this.Signature = Encoding.Default.GetString(reader.ReadBytes(SignatureSize));
                this.EncodedAllSize = reader.ReadInt32();
                this.unknown1 = reader.ReadUInt32();
                this.unknown2 = reader.ReadUInt32();
                this.EncodedBodySize = reader.ReadInt32();
                this.DecodedBodySize = reader.ReadInt32();
            }

            public void WriteTo(BinaryWriter writer)
            {
                if (writer == null)
                    throw new ArgumentNullException("writer");

                writer.Write(Encoding.Default.GetBytes(this.Signature));
                writer.Write(this.EncodedAllSize);
                writer.Write(this.unknown1);
                writer.Write(this.unknown2);
                writer.Write(this.EncodedBodySize);
                writer.Write(this.DecodedBodySize);
            }
        }

        private class Chapter : IBinaryReadable
        {
            public const int SignatureSize = 2;

            public Chapter()
            {
                this.Signature = string.Empty;
                this.Version = 0;
                this.Size = 0;
                this.Checksum = 0;
                this.Data = new byte[] { };
            }

            protected Chapter(Chapter chapter)
            {
                this.Signature = chapter.Signature;
                this.Version = chapter.Version;
                this.Size = chapter.Size;
                this.Checksum = chapter.Checksum;
                this.Data = new byte[chapter.Data.Length];
                chapter.Data.CopyTo(this.Data, 0);
            }

            public string Signature { get; private set; }

            public ushort Version { get; private set; }

            public int Size { get; private set; }

            public uint Checksum { get; private set; }

            public bool IsValid
            {
                get
                {
                    var sigVer = Encoding.Default.GetBytes(this.Signature)
                        .Concat(BitConverter.GetBytes(this.Version))
                        .ToArray();
                    long sum = BitConverter.ToUInt32(sigVer, 0) + this.Size;
                    for (var index = 0; index < this.Data.Length; index += sizeof(uint))
                        sum += BitConverter.ToUInt32(this.Data, index);
                    return (uint)sum == this.Checksum;
                }
            }

            protected byte[] Data { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException("reader");

                this.Signature = Encoding.Default.GetString(reader.ReadBytes(SignatureSize));
                this.Version = reader.ReadUInt16();
                this.Size = reader.ReadInt32();
                this.Checksum = reader.ReadUInt32();
                this.Data = reader.ReadBytes(
                    this.Size - SignatureSize - sizeof(ushort) - sizeof(int) - sizeof(uint));
            }
        }

        private class Score : Chapter   // per scene
        {
            public const string ValidSignature = "SC";
            public const ushort ValidVersion = 0x0001;
            public const int ValidSize = 0x00000060;

            public Score(Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Version != ValidVersion)
                    throw new InvalidDataException("Version");
                if (this.Size != ValidSize)
                    throw new InvalidDataException("Size");

                using (var stream = new MemoryStream(this.Data, false))
                {
                    var reader = new BinaryReader(stream);

                    var number = reader.ReadInt32();
                    this.LevelScene = new LevelScenePair((Level)(number / 10), (number % 10) + 1);
                    this.HighScore = reader.ReadInt32();
                    reader.ReadUInt32();    // always 0x00000000?
                    this.BestshotScore = reader.ReadInt32();
                    reader.ReadBytes(0x20);
                    this.DateTime = reader.ReadUInt32();
                    reader.ReadUInt32();    // checksum of the bestshot file?
                    this.TrialCount = reader.ReadInt32();
                    this.SlowRate1 = reader.ReadSingle();
                    this.SlowRate2 = reader.ReadSingle();
                    reader.ReadBytes(0x10);
                }
            }

            public LevelScenePair LevelScene { get; private set; }

            public int HighScore { get; private set; }

            public int BestshotScore { get; private set; }

            public uint DateTime { get; private set; }      // UNIX time (unit: [s])

            public int TrialCount { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public float SlowRate1 { get; private set; }    // ??

            public float SlowRate2 { get; private set; }    // ??

            public static bool CanInitialize(Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class Status : Chapter
        {
            public const string ValidSignature = "ST";
            public const ushort ValidVersion = 0x0000;
            public const int ValidSize = 0x00000458;

            public Status(Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Version != ValidVersion)
                    throw new InvalidDataException("Version");
                if (this.Size != ValidSize)
                    throw new InvalidDataException("Size");

                using (var stream = new MemoryStream(this.Data, false))
                {
                    var reader = new BinaryReader(stream);

                    this.LastName = reader.ReadBytes(10);
                    reader.ReadBytes(0x0442);
                }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] LastName { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)

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

            public short Scene { get; private set; }        // 1-based

            public short Width { get; private set; }

            public short Height { get; private set; }

            public int Score { get; private set; }

            public float SlowRate { get; private set; }

            public byte[] CardName { get; private set; }    // .Length = 0x50

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException("reader");

                this.Signature = Encoding.Default.GetString(reader.ReadBytes(SignatureSize));
                if (this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                {
                    reader.ReadUInt16();
                    this.Level = (Level)(reader.ReadInt16() - 1);
                    this.Scene = reader.ReadInt16();
                    reader.ReadUInt16();    // 0x0102 ... Version?
                    this.Width = reader.ReadInt16();
                    this.Height = reader.ReadInt16();
                    this.Score = reader.ReadInt32();
                    this.SlowRate = reader.ReadSingle();
                    this.CardName = reader.ReadBytes(0x50);
                }
            }
        }
    }
}
