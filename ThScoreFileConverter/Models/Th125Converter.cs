//-----------------------------------------------------------------------
// <copyright file="Th125Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591
#pragma warning disable SA1600 // ElementsMustBeDocumented

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
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th125;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th125Converter : ThConverter
    {
        // Thanks to thwiki.info
        private static readonly Dictionary<(Th125.Level Level, int Scene), (Enemy Enemy, string Card)> SpellCards =
            new Dictionary<(Th125.Level, int), (Enemy, string)>()
            {
                { (Th125.Level.One,     1), (Enemy.Minoriko,  string.Empty) },
                { (Th125.Level.One,     2), (Enemy.Minoriko,  string.Empty) },
                { (Th125.Level.One,     3), (Enemy.Shizuha,   "秋符「フォーリンブラスト」") },
                { (Th125.Level.One,     4), (Enemy.Minoriko,  "実符「ウォームカラーハーヴェスト」") },
                { (Th125.Level.One,     5), (Enemy.Shizuha,   "枯道「ロストウィンドロウ」") },
                { (Th125.Level.One,     6), (Enemy.Minoriko,  "焼芋「スイートポテトルーム」") },
                { (Th125.Level.Two,     1), (Enemy.Parsee,    string.Empty) },
                { (Th125.Level.Two,     2), (Enemy.Hina,      string.Empty) },
                { (Th125.Level.Two,     3), (Enemy.Parsee,    "嫉妬「ジェラシーボンバー」") },
                { (Th125.Level.Two,     4), (Enemy.Hina,      "厄野「禊川の堆積」") },
                { (Th125.Level.Two,     5), (Enemy.Parsee,    "怨み念法「積怨返し」") },
                { (Th125.Level.Two,     6), (Enemy.Hina,      "災禍「呪いの雛人形」") },
                { (Th125.Level.Three,   1), (Enemy.Yamame,    string.Empty) },
                { (Th125.Level.Three,   2), (Enemy.Kogasa,    "傘符「一本足ピッチャー返し」") },
                { (Th125.Level.Three,   3), (Enemy.Kisume,    "釣瓶「飛んで井の中」") },
                { (Th125.Level.Three,   4), (Enemy.Yamame,    "細綱「カンダタロープ」") },
                { (Th125.Level.Three,   5), (Enemy.Kogasa,    "虹符「オーバー・ザ・レインボー」") },
                { (Th125.Level.Three,   6), (Enemy.Kisume,    "釣瓶「ウェルディストラクター」") },
                { (Th125.Level.Three,   7), (Enemy.Yamame,    "毒符「樺黄小町」") },
                { (Th125.Level.Three,   8), (Enemy.Kogasa,    "傘符「細雪の過客」") },
                { (Th125.Level.Four,    1), (Enemy.Nitori,    string.Empty) },
                { (Th125.Level.Four,    2), (Enemy.Momiji,    string.Empty) },
                { (Th125.Level.Four,    3), (Enemy.Nitori,    "水符「ウォーターカーペット」") },
                { (Th125.Level.Four,    4), (Enemy.Momiji,    "狗符「レイビーズバイト」") },
                { (Th125.Level.Four,    5), (Enemy.Nitori,    "河符「ディバイディングエッジ」") },
                { (Th125.Level.Four,    6), (Enemy.Momiji,    "山窩「エクスペリーズカナン」") },
                { (Th125.Level.Four,    7), (Enemy.Nitori,    "河童「乾燥尻子玉」") },
                { (Th125.Level.Five,    1), (Enemy.Ichirin,   string.Empty) },
                { (Th125.Level.Five,    2), (Enemy.Minamitsu, string.Empty) },
                { (Th125.Level.Five,    3), (Enemy.Ichirin,   "拳骨「天空鉄槌落とし」") },
                { (Th125.Level.Five,    4), (Enemy.Minamitsu, "錨符「幽霊船長期停泊」") },
                { (Th125.Level.Five,    5), (Enemy.Ichirin,   "稲妻「帯電入道」") },
                { (Th125.Level.Five,    6), (Enemy.Minamitsu, "浸水「船底のヴィーナス」") },
                { (Th125.Level.Five,    7), (Enemy.Ichirin,   "鉄拳「入道にょき」") },
                { (Th125.Level.Five,    8), (Enemy.Minamitsu, "「ディープシンカー」") },
                { (Th125.Level.Six,     1), (Enemy.Yuugi,     string.Empty) },
                { (Th125.Level.Six,     2), (Enemy.Suika,     string.Empty) },
                { (Th125.Level.Six,     3), (Enemy.Yuugi,     "光鬼「金剛螺旋」") },
                { (Th125.Level.Six,     4), (Enemy.Suika,     "鬼符「豆粒大の針地獄」") },
                { (Th125.Level.Six,     5), (Enemy.Yuugi,     "鬼符「鬼気狂瀾」") },
                { (Th125.Level.Six,     6), (Enemy.Suika,     "地獄「煉獄吐息」") },
                { (Th125.Level.Six,     7), (Enemy.Yuugi,     "鬼声「壊滅の咆哮」") },
                { (Th125.Level.Six,     8), (Enemy.Suika,     "鬼符「ミッシングパワー」") },
                { (Th125.Level.Seven,   1), (Enemy.Shou,      string.Empty) },
                { (Th125.Level.Seven,   2), (Enemy.Nazrin,    string.Empty) },
                { (Th125.Level.Seven,   3), (Enemy.Shou,      "寅符「ハングリータイガー」") },
                { (Th125.Level.Seven,   4), (Enemy.Nazrin,    "棒符「ナズーリンロッド」") },
                { (Th125.Level.Seven,   5), (Enemy.Shou,      "天符「焦土曼荼羅」") },
                { (Th125.Level.Seven,   6), (Enemy.Nazrin,    "財宝「ゴールドラッシュ」") },
                { (Th125.Level.Seven,   7), (Enemy.Shou,      "宝符「黄金の震眩」") },
                { (Th125.Level.Eight,   1), (Enemy.Rin,       string.Empty) },
                { (Th125.Level.Eight,   2), (Enemy.Utsuho,    "熔解「メルティングホワイト」") },
                { (Th125.Level.Eight,   3), (Enemy.Rin,       "死符「ゴーストタウン」") },
                { (Th125.Level.Eight,   4), (Enemy.Utsuho,    "巨星「レッドジャイアント」") },
                { (Th125.Level.Eight,   5), (Enemy.Rin,       "「死体繁華街」") },
                { (Th125.Level.Eight,   6), (Enemy.Utsuho,    "星符「巨星墜つ」") },
                { (Th125.Level.Eight,   7), (Enemy.Rin,       "酔歩「キャットランダムウォーク」") },
                { (Th125.Level.Eight,   8), (Enemy.Utsuho,    "七星「セプテントリオン」") },
                { (Th125.Level.Nine,    1), (Enemy.Satori,    string.Empty) },
                { (Th125.Level.Nine,    2), (Enemy.Koishi,    "心符「没我の愛」") },
                { (Th125.Level.Nine,    3), (Enemy.Satori,    "脳符「ブレインフィンガープリント」") },
                { (Th125.Level.Nine,    4), (Enemy.Koishi,    "記憶「ＤＮＡの瑕」") },
                { (Th125.Level.Nine,    5), (Enemy.Satori,    "心花「カメラシャイローズ」") },
                { (Th125.Level.Nine,    6), (Enemy.Koishi,    "「胎児の夢」") },
                { (Th125.Level.Nine,    7), (Enemy.Satori,    "想起「うろおぼえの金閣寺」") },
                { (Th125.Level.Nine,    8), (Enemy.Koishi,    "「ローズ地獄」") },
                { (Th125.Level.Ten,     1), (Enemy.Tenshi,    "気性「勇気凛々の剣」") },
                { (Th125.Level.Ten,     2), (Enemy.Iku,       "雷符「ライトニングフィッシュ」") },
                { (Th125.Level.Ten,     3), (Enemy.Tenshi,    "地震「避難険路」") },
                { (Th125.Level.Ten,     4), (Enemy.Iku,       "珠符「五爪龍の珠」") },
                { (Th125.Level.Ten,     5), (Enemy.Tenshi,    "要石「カナメファンネル」") },
                { (Th125.Level.Ten,     6), (Enemy.Iku,       "龍宮「タイヤヒラメダンス」") },
                { (Th125.Level.Ten,     7), (Enemy.Tenshi,    "「全人類の緋想天」") },
                { (Th125.Level.Ten,     8), (Enemy.Iku,       "龍魚「龍宮の使い遊泳弾」") },
                { (Th125.Level.Eleven,  1), (Enemy.Kanako,    string.Empty) },
                { (Th125.Level.Eleven,  2), (Enemy.Suwako,    "神桜「湛えの桜吹雪」") },
                { (Th125.Level.Eleven,  3), (Enemy.Kanako,    "蛇符「グラウンドサーペント」") },
                { (Th125.Level.Eleven,  4), (Enemy.Suwako,    "姫川「プリンセスジェイドグリーン」") },
                { (Th125.Level.Eleven,  5), (Enemy.Kanako,    "御柱「メテオリックオンバシラ」") },
                { (Th125.Level.Eleven,  6), (Enemy.Suwako,    "鉄輪「ミシカルリング」") },
                { (Th125.Level.Eleven,  7), (Enemy.Kanako,    "儚道「御神渡りクロス」") },
                { (Th125.Level.Eleven,  8), (Enemy.Suwako,    "土着神「御射軍神さま」") },
                { (Th125.Level.Twelve,  1), (Enemy.Byakuren,  string.Empty) },
                { (Th125.Level.Twelve,  2), (Enemy.Nue,       "正体不明「紫鏡」") },
                { (Th125.Level.Twelve,  3), (Enemy.Byakuren,  "「遊行聖」") },
                { (Th125.Level.Twelve,  4), (Enemy.Nue,       "正体不明「赤マント青マント」") },
                { (Th125.Level.Twelve,  5), (Enemy.Byakuren,  "習合「垂迹大日如来」") },
                { (Th125.Level.Twelve,  6), (Enemy.Nue,       "正体不明「厠の花子さん」") },
                { (Th125.Level.Twelve,  7), (Enemy.Byakuren,  "「スターソードの護法」") },
                { (Th125.Level.Twelve,  8), (Enemy.Nue,       "「遊星よりの弾幕Ｘ」") },
                { (Th125.Level.Extra,   1), (Enemy.Reimu,     "お札「新聞拡張団調伏」") },
                { (Th125.Level.Extra,   2), (Enemy.Marisa,    "星符「オールトクラウド」") },
                { (Th125.Level.Extra,   3), (Enemy.Sanae,     "奇跡「弘安の神風」") },
                { (Th125.Level.Extra,   4), (Enemy.Reimu,     "結界「パパラッチ撃退結界」") },
                { (Th125.Level.Extra,   5), (Enemy.Marisa,    "天儀「オーレリーズソーラーシステム」") },
                { (Th125.Level.Extra,   6), (Enemy.Sanae,     "蛙符「手管の蝦蟇」") },
                { (Th125.Level.Extra,   7), (Enemy.Reimu,     "夢符「夢想亜空穴」") },
                { (Th125.Level.Extra,   8), (Enemy.Marisa,    "彗星「ブレイジングスター」") },
                { (Th125.Level.Extra,   9), (Enemy.Sanae,     "妖怪退治「妖力スポイラー」") },
                { (Th125.Level.Spoiler, 1), (Enemy.Hatate,    string.Empty) },
                { (Th125.Level.Spoiler, 2), (Enemy.Hatate,    "取材「姫海棠はたての練習取材」") },
                { (Th125.Level.Spoiler, 3), (Enemy.Hatate,    "連写「ラピッドショット」") },
                { (Th125.Level.Spoiler, 4), (Enemy.Hatate,    "遠眼「天狗サイコグラフィ」") },
                { (Th125.Level.Spoiler, 5), (Enemy.Aya,       string.Empty) },
                { (Th125.Level.Spoiler, 6), (Enemy.Aya,       "取材「射命丸文の圧迫取材」") },
                { (Th125.Level.Spoiler, 7), (Enemy.Aya,       "望遠「キャンディッドショット」") },
                { (Th125.Level.Spoiler, 8), (Enemy.Aya,       "速写「ファストショット」") },
                { (Th125.Level.Spoiler, 9), (Enemy.Aya,       "「幻想風靡」") },
            };

        private static new readonly EnumShortNameParser<Th125.Level> LevelParser =
            new EnumShortNameParser<Th125.Level>();

        private static readonly EnumShortNameParser<Chara> CharaParser =
            new EnumShortNameParser<Chara>();

        private static readonly string LevelLongPattern =
            string.Join("|", Utils.GetEnumerator<Th125.Level>().Select(lv => lv.ToLongName()).ToArray());

        private AllScoreData allScoreData = null;

        private Dictionary<Chara, Dictionary<(Th125.Level Level, int Scene), (string Path, BestShotHeader Header)>> bestshots = null;

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
                        this.bestshots = new Dictionary<Chara, Dictionary<(Th125.Level, int), (string, BestShotHeader)>>(
                            Enum.GetValues(typeof(Chara)).Length);
                    }

                    if (!this.bestshots.ContainsKey(chara))
                    {
                        this.bestshots.Add(
                            chara, new Dictionary<(Th125.Level, int), (string, BestShotHeader)>(SpellCards.Count));
                    }

                    var key = (header.Level, header.Scene);
                    if (!this.bestshots[chara].ContainsKey(key))
                        this.bestshots[chara].Add(key, (outputFile.Name, header));

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

                    var score = parent.allScoreData.Scores.FirstOrDefault(elem =>
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

            private static readonly Func<IScore, Chara, int, bool> IsTargetImpl =
                (score, chara, method) =>
                {
                    if (score == null)
                        return false;

                    if (method == 1)
                    {
                        if (score.LevelScene.Level == Th125.Level.Spoiler)
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

                    Func<IScore, bool> isTarget = (score => IsTargetImpl(score, chara, method));
                    Func<IScore, bool> triedAndSucceeded =
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
                        var score = parent.allScoreData.Scores.FirstOrDefault(
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
                        IScore score;
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
                                score = parent.allScoreData.Scores.FirstOrDefault(elem =>
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

        private class AllScoreData
        {
            private readonly List<IScore> scores;

            public AllScoreData() => this.scores = new List<IScore>(SpellCards.Count);

            public Th095.HeaderBase Header { get; private set; }

            public IReadOnlyList<IScore> Scores => this.scores;

            public IStatus Status { get; private set; }

            public void Set(Th095.HeaderBase header) => this.Header = header;

            public void Set(IScore score) => this.scores.Add(score);

            public void Set(IStatus status) => this.Status = status;
        }

        private class Score : Th095.Chapter, IScore // per scene
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
                    this.LevelScene = (Utils.ToEnum<Th125.Level>(number / 10), (int)((number % 10) + 1));
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

            public (Th125.Level Level, int Scene) LevelScene { get; }

            public int HighScore { get; }

            public Chara Chara { get; }

            public int TrialCount { get; }

            public int FirstSuccess { get; }

            public uint DateTime { get; }   // UNIX time

            public int BestshotScore { get; }

            public static bool CanInitialize(Th095.Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class Status : Th095.Chapter, IStatus
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

            public IEnumerable<byte> LastName { get; }  // The last 2 bytes are always 0x00 ?

            public IEnumerable<byte> BgmFlags { get; }

            public int TotalPlayTime { get; }   // unit: 10ms

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

            public Th125.Level Level { get; private set; }

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

            public byte[] CardName { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader is null)
                    throw new ArgumentNullException(nameof(reader));

                this.Signature = Encoding.Default.GetString(reader.ReadExactBytes(SignatureSize));
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException();

                reader.ReadUInt16();    // always 0x0405?
                this.Level = Utils.ToEnum<Th125.Level>(reader.ReadInt16() - 1);
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

                public BonusFields(int data) => this.data = new BitVector32(data);

                [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
                public int Data => this.data.Data;

                public bool TwoShot => this.data[0x00000004];

                public bool NiceShot => this.data[0x00000008];

                public bool RiskBonus => this.data[0x00000010];

                public bool RedShot => this.data[0x00000040];

                public bool PurpleShot => this.data[0x00000080];

                public bool BlueShot => this.data[0x00000100];

                public bool CyanShot => this.data[0x00000200];

                public bool GreenShot => this.data[0x00000400];

                public bool YellowShot => this.data[0x00000800];

                public bool OrangeShot => this.data[0x00001000];

                public bool ColorfulShot => this.data[0x00002000];

                public bool RainbowShot => this.data[0x00004000];

                public bool SoloShot => this.data[0x00010000];

                public bool MacroBonus => this.data[0x00400000];

                public bool FrontShot => this.data[0x01000000];

                public bool BackShot => this.data[0x02000000];

                public bool SideShot => this.data[0x04000000];

                public bool ClearShot => this.data[0x08000000];

                public bool CatBonus => this.data[0x10000000];
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

            public bool Outputs { get; }

            public string Format { get; }

            public string Value { get; }
        }
    }
}
