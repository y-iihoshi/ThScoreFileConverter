//-----------------------------------------------------------------------
// <copyright file="Th143Converter.cs" company="None">
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
    internal class Th143Converter : ThConverter
    {
        // Thanks to thwiki.info
        private static readonly Dictionary<(Day Day, int Scene), (Enemy[] Enemies, string Card)> SpellCards =
            new Dictionary<(Day, int), (Enemy[], string)>()
            {
                { (Day.First,   1), (new[] { Enemy.Yatsuhashi },   string.Empty) },
                { (Day.First,   2), (new[] { Enemy.Wakasagihime }, "水符「ルナティックレッドスラップ」") },
                { (Day.First,   3), (new[] { Enemy.Cirno },        "氷符「パーフェクトグレーシェリスト」") },
                { (Day.First,   4), (new[] { Enemy.Wakasagihime }, "潮符「湖のタイダルウェイブ」") },
                { (Day.First,   5), (new[] { Enemy.Cirno },        "氷王「フロストキング」") },
                { (Day.First,   6), (new[] { Enemy.Wakasagihime }, "魚符「スクールオブフィッシュ」") },
                { (Day.Second,  1), (new[] { Enemy.Kyouko },       "叫喚「プライマルスクリーム」") },
                { (Day.Second,  2), (new[] { Enemy.Sekibanki },    "飛首「エクストリームロングネック」") },
                { (Day.Second,  3), (new[] { Enemy.Kyouko },       "劈音「ピアッシングサークル」") },
                { (Day.Second,  4), (new[] { Enemy.Sekibanki },    "眼光「ヘルズレイ」") },
                { (Day.Second,  5), (new[] { Enemy.Kyouko },       "御経「無限念仏」") },
                { (Day.Second,  6), (new[] { Enemy.Sekibanki },    "飛首「ツインロクロヘッド」") },
                { (Day.Third,   1), (new[] { Enemy.Kagerou },      string.Empty) },
                { (Day.Third,   2), (new[] { Enemy.Kagerou },      "満月「フルムーンロア」") },
                { (Day.Third,   3), (new[] { Enemy.Keine },        "「２０ＸＸ年　死後の旅」") },
                { (Day.Third,   4), (new[] { Enemy.Mokou },        "惜命「不死身の捨て身」") },
                { (Day.Third,   5), (new[] { Enemy.Kagerou },      "狼牙「血に餓えたウルフファング」") },
                { (Day.Third,   6), (new[] { Enemy.Keine },        "大火「江戸のフラワー」") },
                { (Day.Third,   7), (new[] { Enemy.Mokou },        "「火の鳥 ―不死伝説―」") },
                { (Day.Fourth,  1), (new[] { Enemy.Yuyuko },       string.Empty) },
                { (Day.Fourth,  2), (new[] { Enemy.Seiga, Enemy.Yoshika }, "入魔「過剰ゾウフォルゥモォ」") },
                { (Day.Fourth,  3), (new[] { Enemy.Yuyuko },       "蝶符「花蝶風月」") },
                { (Day.Fourth,  4), (new[] { Enemy.Yoshika },      "毒爪「ゾンビクロー」") },
                { (Day.Fourth,  5), (new[] { Enemy.Seiga },        "仙術「ウォールランナー」") },
                { (Day.Fourth,  6), (new[] { Enemy.Yuyuko },       "桜花「桜吹雪花小町」") },
                { (Day.Fourth,  7), (new[] { Enemy.Seiga },        "仙術「壁抜けワームホール」") },
                { (Day.Fifth,   1), (new[] { Enemy.Raiko },        string.Empty) },
                { (Day.Fifth,   2), (new[] { Enemy.Yatsuhashi },   "琴符「天の詔琴」") },
                { (Day.Fifth,   3), (new[] { Enemy.Benben },       "音符「大熱唱琵琶」") },
                { (Day.Fifth,   4), (new[] { Enemy.Raiko },        "雷符「怒りのデンデン太鼓」") },
                { (Day.Fifth,   5), (new[] { Enemy.Yatsuhashi },   "哀歌「人琴ともに亡ぶ」") },
                { (Day.Fifth,   6), (new[] { Enemy.Benben },       "楽譜「スコアウェブ」") },
                { (Day.Fifth,   7), (new[] { Enemy.Raiko },        "太鼓「ファンタジックウーファー」") },
                { (Day.Fifth,   8), (new[] { Enemy.Benben, Enemy.Yatsuhashi }, "両吟「星降る唄」") },
                { (Day.Sixth,   1), (new[] { Enemy.Mamizou },      string.Empty) },
                { (Day.Sixth,   2), (new[] { Enemy.Aya },          "写真「激撮テングスクープ」") },
                { (Day.Sixth,   3), (new[] { Enemy.Hatate },       "写真「フルパノラマショット」") },
                { (Day.Sixth,   4), (new[] { Enemy.Nitori },       "瀑符「シライトフォール」") },
                { (Day.Sixth,   5), (new[] { Enemy.Momiji },       "牙符「咀嚼玩味」") },
                { (Day.Sixth,   6), (new[] { Enemy.Nitori },       "瀑符「ケゴンガン」") },
                { (Day.Sixth,   7), (new[] { Enemy.Hatate },       "写真「籠もりパパラッチ」") },
                { (Day.Sixth,   8), (new[] { Enemy.Aya },          "「瞬撮ジャーナリスト」") },
                { (Day.Seventh, 1), (new[] { Enemy.Marisa },       "恋符「ワイドマスター」") },
                { (Day.Seventh, 2), (new[] { Enemy.Sakuya },       "時符「タイムストッパー咲夜」") },
                { (Day.Seventh, 3), (new[] { Enemy.Youmu },        "光符「冥府光芒一閃」") },
                { (Day.Seventh, 4), (new[] { Enemy.Sanae },        "蛇符「バインドスネークカモン」") },
                { (Day.Seventh, 5), (new[] { Enemy.Marisa },       "恋符「マシンガンスパーク」") },
                { (Day.Seventh, 6), (new[] { Enemy.Sakuya },       "時符「チェンジリングマジック」") },
                { (Day.Seventh, 7), (new[] { Enemy.Youmu },        "彼岸剣「地獄極楽滅多斬り」") },
                { (Day.Seventh, 8), (new[] { Enemy.Sanae },        "蛇符「グリーンスネークカモン」") },
                { (Day.Eighth,  1), (new[] { Enemy.Shinmyoumaru }, string.Empty) },
                { (Day.Eighth,  2), (new[] { Enemy.Reimu },        "神籤「反則結界」") },
                { (Day.Eighth,  3), (new[] { Enemy.Mamizou },      "「鳴かぬなら泣くまで待とう時鳥」") },
                { (Day.Eighth,  4), (new[] { Enemy.Shinmyoumaru }, "「小人の地獄」") },
                { (Day.Eighth,  5), (new[] { Enemy.Reimu },        "「パスウェイジョンニードル」") },
                { (Day.Eighth,  6), (new[] { Enemy.Mamizou },      "「にんげんって良いな」") },
                { (Day.Eighth,  7), (new[] { Enemy.Shinmyoumaru }, "輝針「鬼ごろし両目突きの針」") },
                { (Day.Ninth,   1), (new[] { Enemy.Kanako },       "御柱「ライジングオンバシラ」") },
                { (Day.Ninth,   2), (new[] { Enemy.Suwako },       "緑石「ジェイドブレイク」") },
                { (Day.Ninth,   3), (new[] { Enemy.Futo },         "古舟「エンシェントシップ」") },
                { (Day.Ninth,   4), (new[] { Enemy.Suika },        "鬼群「インプスウォーム」") },
                { (Day.Ninth,   5), (new[] { Enemy.Kanako },       "「神の御威光」") },
                { (Day.Ninth,   6), (new[] { Enemy.Suwako },       "蛙符「血塗られた赤蛙塚」") },
                { (Day.Ninth,   7), (new[] { Enemy.Futo },         "熱龍「火焔龍脈」") },
                { (Day.Ninth,   8), (new[] { Enemy.Suika },        "鬼群「百鬼禿童」") },
                { (Day.Last,    1), (new[] { Enemy.Byakuren },     "「ハリの制縛」") },
                { (Day.Last,    2), (new[] { Enemy.Miko },         "「我こそが天道なり」") },
                { (Day.Last,    3), (new[] { Enemy.Tenshi },       "「全妖怪の緋想天」") },
                { (Day.Last,    4), (new[] { Enemy.Remilia },      "「フィットフルナイトメア」") },
                { (Day.Last,    5), (new[] { Enemy.Yukari },       "「不可能弾幕結界」") },
                { (Day.Last,    6), (new[] { Enemy.Byakuren },     "「ブラフマーの瞳」") },
                { (Day.Last,    7), (new[] { Enemy.Miko },         "「十七条の憲法爆弾」") },
                { (Day.Last,    8), (new[] { Enemy.Tenshi },       "「鹿島鎮護」") },
                { (Day.Last,    9), (new[] { Enemy.Remilia },      "「きゅうけつ鬼ごっこ」") },
                { (Day.Last,   10), (new[] { Enemy.Yukari },       "「運鈍根の捕物帖」") },
            };

        private static readonly List<string> Nicknames =
            new List<string>
            {
                "弾幕アマノジャク",
                "ひよっこアマノジャク",
                "慣れてきたアマノジャク",
                "一人前アマノジャク",
                "無敵のアマノジャク",
                "不滅のアマノジャク",
                "伝説のアマノジャク",
                "神話のアマノジャク",
                "全てを敵に回した天邪鬼",
                "逃げ切ったアマノジャク",
                "はじめてのアマノジャク",
                "新たなアイテム使い",
                "ミラクル不思議道具使い",
                "おや、片手が空いていた",
                "そろそろお茶でも",
                "ドライアイにご注意",
                "悟りでも開けるよ",
                "もう痛みを感じない",
                "もしかして、快感？",
                "彼女の屍を超えてゆけ",
                "初日マスター",
                "２日目マスター",
                "３日目マスター",
                "４日目マスター",
                "５日目マスター",
                "６日目マスター",
                "７日目マスター",
                "８日目マスター",
                "９日目マスター",
                "最終日マスター",
                "おひらりさん",
                "カメラ小僧",
                "仕舞いっぱなしの傘",
                "とおりすがりの亡霊さん",
                "たま使い",
                "手持ち花火",
                "お地蔵さん",
                "お人形屋さん",
                "物理で殴れ",
                "反則嫌い",
                "ひらりスター",
                "カメラ大人",
                "お気に入りの傘",
                "もしかして生霊さん？",
                "たま職人",
                "スターマインさん",
                "地蔵菩薩",
                "人形蒐集家",
                "ピコピコハンマー",
                "正々堂々屋さん",
                "ひらりマスター",
                "カメラ紳士",
                "傘ハウス",
                "りっぱな霊体",
                "たま仙人",
                "クレイジーボマー",
                "まさに地蔵の様な人",
                "人形原型師",
                "小槌でスマッシュ！",
                "モッタイナイ精神",
                "ひらり宇宙神",
                "カメラ魔人",
                "傘のパラダイス",
                "生まれながらの亡霊",
                "たまたまデスター",
                "花火曼荼羅",
                "世界は地蔵で廻っている",
                "呪われ人形メイク",
                "脳みそ金時",
                "究極反則生命体",
            };

        private static readonly EnumShortNameParser<Day> DayParser =
            new EnumShortNameParser<Day>();

        private static readonly EnumShortNameParser<ItemWithTotal> ItemWithTotalParser =
            new EnumShortNameParser<ItemWithTotal>();

        private static readonly string DayLongPattern =
            string.Join("|", Utils.GetEnumerator<Day>().Select(day => day.ToLongName()).ToArray());

        private AllScoreData allScoreData = null;

        private Dictionary<(Day Day, int Scene), BestShotPair> bestshots = null;

        public enum Day
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("1", LongName = "01")] First,
            [EnumAltName("2", LongName = "02")] Second,
            [EnumAltName("3", LongName = "03")] Third,
            [EnumAltName("4", LongName = "04")] Fourth,
            [EnumAltName("5", LongName = "05")] Fifth,
            [EnumAltName("6", LongName = "06")] Sixth,
            [EnumAltName("7", LongName = "07")] Seventh,
            [EnumAltName("8", LongName = "08")] Eighth,
            [EnumAltName("9", LongName = "09")] Ninth,
            [EnumAltName("L", LongName = "10")] Last,
#pragma warning restore SA1134 // Attributes should not share line
        }

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        public enum Enemy
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("わかさぎ姫",  LongName = "わかさぎ姫")]             Wakasagihime,
            [EnumAltName("チルノ",      LongName = "チルノ")]                 Cirno,
            [EnumAltName("響子",        LongName = "幽谷 響子")]              Kyouko,
            [EnumAltName("赤蛮奇",      LongName = "赤蛮奇")]                 Sekibanki,
            [EnumAltName("影狼",        LongName = "今泉 影狼")]              Kagerou,
            [EnumAltName("慧音",        LongName = "上白沢 慧音")]            Keine,
            [EnumAltName("妹紅",        LongName = "藤原 妹紅")]              Mokou,
            [EnumAltName("芳香",        LongName = "宮古 芳香")]              Yoshika,
            [EnumAltName("青娥",        LongName = "霍 青娥")]                Seiga,
            [EnumAltName("幽々子",      LongName = "西行寺 幽々子")]          Yuyuko,
            [EnumAltName("八橋",        LongName = "九十九 八橋")]            Yatsuhashi,
            [EnumAltName("弁々",        LongName = "九十九 弁々")]            Benben,
            [EnumAltName("雷鼓",        LongName = "堀川 雷鼓")]              Raiko,
            [EnumAltName("文",          LongName = "射命丸 文")]              Aya,
            [EnumAltName("はたて",      LongName = "姫海棠 はたて")]          Hatate,
            [EnumAltName("にとり",      LongName = "河城 にとり")]            Nitori,
            [EnumAltName("椛",          LongName = "犬走 椛")]                Momiji,
            [EnumAltName("魔理沙",      LongName = "霧雨 魔理沙")]            Marisa,
            [EnumAltName("咲夜",        LongName = "十六夜 咲夜")]            Sakuya,
            [EnumAltName("妖夢",        LongName = "魂魄 妖夢")]              Youmu,
            [EnumAltName("早苗",        LongName = "東風谷 早苗")]            Sanae,
            [EnumAltName("霊夢",        LongName = "博麗 霊夢")]              Reimu,
            [EnumAltName("マミゾウ",    LongName = "二ッ岩 マミゾウ")]        Mamizou,
            [EnumAltName("針妙丸",      LongName = "少名 針妙丸")]            Shinmyoumaru,
            [EnumAltName("神奈子",      LongName = "八坂 神奈子")]            Kanako,
            [EnumAltName("諏訪子",      LongName = "洩矢 諏訪子")]            Suwako,
            [EnumAltName("布都",        LongName = "物部 布都")]              Futo,
            [EnumAltName("萃香",        LongName = "伊吹 萃香")]              Suika,
            [EnumAltName("白蓮",        LongName = "聖 白蓮")]                Byakuren,
            [EnumAltName("神子",        LongName = "豊聡耳 神子")]            Miko,
            [EnumAltName("天子",        LongName = "比那名居 天子")]          Tenshi,
            [EnumAltName("レミリア",    LongName = "レミリア・スカーレット")] Remilia,
            [EnumAltName("紫",          LongName = "八雲 紫")]                Yukari,
#pragma warning restore SA1134 // Attributes should not share line
        }

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        public enum ItemWithTotal
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("1", LongName = "ひらり布")]                 Fablic,
            [EnumAltName("2", LongName = "天狗のトイカメラ")]         Camera,
            [EnumAltName("3", LongName = "隙間の折りたたみ傘")]       Umbrella,
            [EnumAltName("4", LongName = "亡霊の送り提灯")]           Lantern,
            [EnumAltName("5", LongName = "血に飢えた陰陽玉")]         Orb,
            [EnumAltName("6", LongName = "四尺マジックボム")]         Bomb,
            [EnumAltName("7", LongName = "身代わり地蔵")]             Jizou,
            [EnumAltName("8", LongName = "呪いのデコイ人形")]         Doll,
            [EnumAltName("9", LongName = "打ち出の小槌（レプリカ）")] Mallet,
            [EnumAltName("0", LongName = "ノーアイテム")]             NoItem,
            [EnumAltName("T", LongName = "合計")]                     Total,
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
            using (var decoded = new FileStream("th143decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
                new NicknameReplacer(this),
                new TimeReplacer(this),
                new ShotReplacer(this, outputFilePath),
                new ShotExReplacer(this, outputFilePath),
            };
        }

        protected override string[] FilterBestShotFiles(string[] files)
        {
            var pattern = Utils.Format(@"sc({0})_\d{{2}}.dat", DayLongPattern);

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

                    if (this.bestshots == null)
                        this.bestshots = new Dictionary<(Day, int), BestShotPair>(SpellCards.Count);

                    var key = (header.Day, header.Scene);
                    if (!this.bestshots.ContainsKey(key))
                        this.bestshots.Add(key, new BestShotPair(outputFile.Name, header));

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
                var chapter = new Th10.Chapter();

                try
                {
                    while (remainSize > 0)
                    {
                        chapter.ReadFrom(reader);
                        if (!chapter.IsValid)
                            return false;
                        if (!Score.CanInitialize(chapter) &&
                            !ItemStatus.CanInitialize(chapter) &&
                            !Status.CanInitialize(chapter))
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
                { Score.ValidSignature,      (data, ch) => data.Set(new Score(ch))      },
                { ItemStatus.ValidSignature, (data, ch) => data.Set(new ItemStatus(ch)) },
                { Status.ValidSignature,     (data, ch) => data.Set(new Status(ch))     },
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
                    //// (allScoreData.scores.Count >= 0) &&
                    (allScoreData.Status != null))
                    return allScoreData;
                else
                    return null;
            }
        }

        // %T143SCR[w][x][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T143SCR({0})([0-9])({1})([1-3])", DayParser.Pattern, ItemWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th143Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var day = DayParser.Parse(match.Groups[1].Value);
                    var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    scene = (scene == 0) ? 10 : scene;
                    var item = ItemWithTotalParser.Parse(match.Groups[3].Value);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    var key = (day, scene);
                    if (!SpellCards.ContainsKey(key))
                        return match.ToString();

                    var score = parent.allScoreData.Scores.Find(elem =>
                        (elem != null) && ((elem.Number > 0) && (elem.Number <= SpellCards.Count)) &&
                        SpellCards.ElementAt(elem.Number - 1).Key.Equals(key));

                    switch (type)
                    {
                        case 1:     // high score
                            return (score != null) ? Utils.ToNumberString(score.HighScore * 10) : "0";
                        case 2:     // challenge count
                            if (item == ItemWithTotal.NoItem)
                            {
                                return "-";
                            }
                            else
                            {
                                return (score != null)
                                    ? Utils.ToNumberString(score.ChallengeCounts[item]) : "0";
                            }

                        case 3:     // cleared count
                            return (score != null) ? Utils.ToNumberString(score.ClearCounts[item]) : "0";
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

        // %T143SCRTL[x][y]
        private class ScoreTotalReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T143SCRTL({0})([1-4])", ItemWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public ScoreTotalReplacer(Th143Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var item = ItemWithTotalParser.Parse(match.Groups[1].Value);
                    var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                    switch (type)
                    {
                        case 1:     // total score
                            return Utils.ToNumberString(
                                parent.allScoreData.Scores.Sum(score => score.HighScore * 10L));
                        case 2:     // total of challenge counts
                            if (item == ItemWithTotal.NoItem)
                                return "-";
                            else
                                return Utils.ToNumberString(parent.allScoreData.ItemStatuses[item].UseCount);
                        case 3:     // total of cleared counts
                            return Utils.ToNumberString(
                                parent.allScoreData.ItemStatuses[item].ClearedCount);
                        case 4:     // num of cleared scenes
                            return Utils.ToNumberString(
                                parent.allScoreData.ItemStatuses[item].ClearedScenes);
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

        // %T143CARD[x][y][z]
        private class CardReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T143CARD({0})([0-9])([12])", DayParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th143Converter parent, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var day = DayParser.Parse(match.Groups[1].Value);
                    var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    scene = (scene == 0) ? 10 : scene;
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    var key = (day, scene);
                    if (!SpellCards.ContainsKey(key))
                        return match.ToString();

                    if (hideUntriedCards)
                    {
                        var score = parent.allScoreData.Scores.Find(elem =>
                            (elem != null) && ((elem.Number > 0) && (elem.Number <= SpellCards.Count)) &&
                            SpellCards.ElementAt(elem.Number - 1).Key.Equals(key));
                        if ((score == null) || (score.ChallengeCounts[ItemWithTotal.Total] <= 0))
                            return "??????????";
                    }

                    if (type == 1)
                    {
                        return string.Join(
                            " &amp; ", SpellCards[key].Enemies.Select(enemy => enemy.ToLongName()).ToArray());
                    }
                    else
                    {
                        return SpellCards[key].Card;
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T143NICK[xx]
        private class NicknameReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T143NICK(\d{2})";

            private readonly MatchEvaluator evaluator;

            public NicknameReplacer(Th143Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);

                    if ((number > 0) && (number <= Nicknames.Count))
                    {
                        return (parent.allScoreData.Status.NicknameFlags[number] > 0)
                            ? Nicknames[number - 1] : "??????????";
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

        // %T143TIMEPLY
        private class TimeReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T143TIMEPLY";

            private readonly MatchEvaluator evaluator;

            public TimeReplacer(Th143Converter parent)
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

        // %T143SHOT[x][y]
        private class ShotReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T143SHOT({0})([0-9])", DayParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ShotReplacer(Th143Converter parent, string outputFilePath)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var day = DayParser.Parse(match.Groups[1].Value);
                    var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    scene = (scene == 0) ? 10 : scene;

                    var key = (day, scene);
                    if (!SpellCards.ContainsKey(key))
                        return match.ToString();

                    if (!string.IsNullOrEmpty(outputFilePath) && parent.bestshots.TryGetValue(key, out var bestshot))
                    {
                        var relativePath = new Uri(outputFilePath)
                            .MakeRelativeUri(new Uri(bestshot.Path)).OriginalString;
                        var alternativeString = Utils.Format("SpellName: {0}", SpellCards[key].Card);
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

        // %T143SHOTEX[w][x][y]
        private class ShotExReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T143SHOTEX({0})([0-9])([1-4])", DayParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public ShotExReplacer(Th143Converter parent, string outputFilePath)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var day = DayParser.Parse(match.Groups[1].Value);
                    var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    scene = (scene == 0) ? 10 : scene;
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    var key = (day, scene);
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
                            case 4:     // date & time
                                return new DateTime(1970, 1, 1)
                                    .AddSeconds(bestshot.Header.DateTime).ToLocalTime()
                                    .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
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
                            case 4: return "----/--/-- --:--:--";
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
                this.ItemStatuses = new Dictionary<ItemWithTotal, ItemStatus>(
                    Enum.GetValues(typeof(ItemWithTotal)).Length);
            }

            public Header Header { get; private set; }

            public List<Score> Scores { get; private set; }

            public Dictionary<ItemWithTotal, ItemStatus> ItemStatuses { get; private set; }

            public Status Status { get; private set; }

            public void Set(Header header)
            {
                this.Header = header;
            }

            public void Set(Score score)
            {
                this.Scores.Add(score);
            }

            public void Set(ItemStatus status)
            {
                if (!this.ItemStatuses.ContainsKey(status.Item))
                    this.ItemStatuses.Add(status.Item, status);
            }

            public void Set(Status status)
            {
                this.Status = status;
            }
        }

        private class Header : Th095.Header
        {
            public const string ValidSignature = "T341";

            public override bool IsValid
                => base.IsValid && this.Signature.Equals(ValidSignature, StringComparison.Ordinal);
        }

        private class Score : Th10.Chapter   // per scene
        {
            public const string ValidSignature = "SN";
            public const ushort ValidVersion = 0x0001;
            public const int ValidSize = 0x00000314;

            public Score(Th10.Chapter chapter)
                : base(chapter, ValidSignature, ValidVersion, ValidSize)
            {
                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    var items = Utils.GetEnumerator<ItemWithTotal>();

                    this.Number = reader.ReadInt32();

                    this.ClearCounts = new Dictionary<ItemWithTotal, int>(items.Count());
                    foreach (var item in items)
                        this.ClearCounts.Add(item, reader.ReadInt32());

                    this.ChallengeCounts = new Dictionary<ItemWithTotal, int>(items.Count());
                    foreach (var item in items)
                        this.ChallengeCounts.Add(item, reader.ReadInt32());

                    this.HighScore = reader.ReadInt32();
                    reader.ReadExactBytes(0x2A8);   // always all 0x00?
                }
            }

            public int Number { get; private set; }

            public Dictionary<ItemWithTotal, int> ClearCounts { get; private set; }

            public Dictionary<ItemWithTotal, int> ChallengeCounts { get; private set; }

            public int HighScore { get; private set; }  // * 10

            public static bool CanInitialize(Th10.Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class ItemStatus : Th10.Chapter
        {
            public const string ValidSignature = "TI";
            public const ushort ValidVersion = 0x0001;
            public const int ValidSize = 0x00000034;

            public ItemStatus(Th10.Chapter chapter)
                : base(chapter, ValidSignature, ValidVersion, ValidSize)
            {
                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    this.Item = Utils.ToEnum<ItemWithTotal>(reader.ReadInt32());
                    this.UseCount = reader.ReadInt32();
                    this.ClearedCount = reader.ReadInt32();
                    this.ClearedScenes = reader.ReadInt32();
                    this.ItemLevel = reader.ReadInt32();
                    reader.ReadInt32();
                    this.AvailableCount = reader.ReadInt32();
                    this.FramesOrRanges = reader.ReadInt32();
                    reader.ReadInt32(); // always 0?
                    reader.ReadInt32(); // always 0?
                }
            }

            public ItemWithTotal Item { get; private set; }

            public int UseCount { get; private set; }

            public int ClearedCount { get; private set; }

            public int ClearedScenes { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int ItemLevel { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int AvailableCount { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int FramesOrRanges { get; private set; }

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
            public const int ValidSize = 0x00000224;

            public Status(Th10.Chapter chapter)
                : base(chapter, ValidSignature, ValidVersion, ValidSize)
            {
                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    this.LastName = reader.ReadExactBytes(14);
                    reader.ReadExactBytes(0x12);
                    this.BgmFlags = reader.ReadExactBytes(9);
                    reader.ReadExactBytes(0x17);
                    this.TotalPlayTime = reader.ReadInt32();
                    reader.ReadInt32();
                    this.LastMainItem = Utils.ToEnum<ItemWithTotal>(reader.ReadInt32());
                    this.LastSubItem = Utils.ToEnum<ItemWithTotal>(reader.ReadInt32());
                    reader.ReadExactBytes(0x54);
                    this.NicknameFlags = reader.ReadExactBytes(71);
                    reader.ReadExactBytes(0x12D);
                }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] LastName { get; private set; }    // .Length = 14 (The last 2 bytes are always 0x00 ?)

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] BgmFlags { get; private set; }    // .Length = 9

            public int TotalPlayTime { get; private set; }  // unit: [0.01s]

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public ItemWithTotal LastMainItem { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public ItemWithTotal LastSubItem { get; private set; }

            public byte[] NicknameFlags { get; private set; }   // .Length = 71?

            public static bool CanInitialize(Th10.Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class BestShotHeader : IBinaryReadable
        {
            public const string ValidSignature = "BST3";
            public const int SignatureSize = 4;

            public string Signature { get; private set; }

            public Day Day { get; private set; }

            public short Scene { get; private set; }    // 1-based

            public short Width { get; private set; }

            public short Height { get; private set; }

            public uint DateTime { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public float SlowRate { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader is null)
                    throw new ArgumentNullException(nameof(reader));

                this.Signature = Encoding.Default.GetString(reader.ReadExactBytes(SignatureSize));
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException();

                reader.ReadUInt16();    // always 0xDF01?
                this.Day = Utils.ToEnum<Day>(reader.ReadInt16());
                this.Scene = (short)(reader.ReadInt16() + 1);
                reader.ReadUInt16();    // 0x0100 ... Version?
                this.Width = reader.ReadInt16();
                this.Height = reader.ReadInt16();
                reader.ReadUInt32();    // always 0x0005E800?
                this.DateTime = reader.ReadUInt32();
                this.SlowRate = reader.ReadSingle();    // really...?
                reader.ReadExactBytes(0x58);
            }
        }
    }
}
