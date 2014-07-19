//-----------------------------------------------------------------------
// <copyright file="Th143Converter.cs" company="None">
//     (c) 2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed.")]

namespace ThScoreFileConverter
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
    using System.Text;
    using System.Text.RegularExpressions;

    internal class Th143Converter : ThConverter
    {
        // Thanks to thwiki.info
        private static readonly Dictionary<DayScenePair, EnemiesCardPair> SpellCards =
            new Dictionary<DayScenePair, EnemiesCardPair>()
            {
                { new DayScenePair(Day.Day1,  1), new EnemiesCardPair(Enemy.Yatsuhashi,   string.Empty) },
                { new DayScenePair(Day.Day1,  2), new EnemiesCardPair(Enemy.Wakasagihime, "水符「ルナティックレッドスラップ」") },
                { new DayScenePair(Day.Day1,  3), new EnemiesCardPair(Enemy.Cirno,        "氷符「パーフェクトグレーシェリスト」") },
                { new DayScenePair(Day.Day1,  4), new EnemiesCardPair(Enemy.Wakasagihime, "潮符「湖のタイダルウェイブ」") },
                { new DayScenePair(Day.Day1,  5), new EnemiesCardPair(Enemy.Cirno,        "氷王「フロストキング」") },
                { new DayScenePair(Day.Day1,  6), new EnemiesCardPair(Enemy.Wakasagihime, "魚符「スクールオブフィッシュ」") },
                { new DayScenePair(Day.Day2,  1), new EnemiesCardPair(Enemy.Kyouko,       "叫喚「プライマルスクリーム」") },
                { new DayScenePair(Day.Day2,  2), new EnemiesCardPair(Enemy.Sekibanki,    "飛首「エクストリームロングネック」") },
                { new DayScenePair(Day.Day2,  3), new EnemiesCardPair(Enemy.Kyouko,       "劈音「ピアッシングサークル」") },
                { new DayScenePair(Day.Day2,  4), new EnemiesCardPair(Enemy.Sekibanki,    "眼光「ヘルズレイ」") },
                { new DayScenePair(Day.Day2,  5), new EnemiesCardPair(Enemy.Kyouko,       "御経「無限念仏」") },
                { new DayScenePair(Day.Day2,  6), new EnemiesCardPair(Enemy.Sekibanki,    "飛首「ツインロクロヘッド」") },
                { new DayScenePair(Day.Day3,  1), new EnemiesCardPair(Enemy.Kagerou,      string.Empty) },
                { new DayScenePair(Day.Day3,  2), new EnemiesCardPair(Enemy.Kagerou,      "満月「フルムーンロア」") },
                { new DayScenePair(Day.Day3,  3), new EnemiesCardPair(Enemy.Keine,        "「２０ＸＸ年　死後の旅」") },
                { new DayScenePair(Day.Day3,  4), new EnemiesCardPair(Enemy.Mokou,        "惜命「不死身の捨て身」") },
                { new DayScenePair(Day.Day3,  5), new EnemiesCardPair(Enemy.Kagerou,      "狼牙「血に餓えたウルフファング」") },
                { new DayScenePair(Day.Day3,  6), new EnemiesCardPair(Enemy.Keine,        "大火「江戸のフラワー」") },
                { new DayScenePair(Day.Day3,  7), new EnemiesCardPair(Enemy.Mokou,        "「火の鳥 ―不死伝説―」") },
                { new DayScenePair(Day.Day4,  1), new EnemiesCardPair(Enemy.Yuyuko,       string.Empty) },
                { new DayScenePair(Day.Day4,  2), new EnemiesCardPair(Enemy.Seiga, Enemy.Yoshika, "入魔「過剰ゾウフォルゥモォ」") },
                { new DayScenePair(Day.Day4,  3), new EnemiesCardPair(Enemy.Yuyuko,       "蝶符「花蝶風月」") },
                { new DayScenePair(Day.Day4,  4), new EnemiesCardPair(Enemy.Yoshika,      "毒爪「ゾンビクロー」") },
                { new DayScenePair(Day.Day4,  5), new EnemiesCardPair(Enemy.Seiga,        "仙術「ウォールランナー」") },
                { new DayScenePair(Day.Day4,  6), new EnemiesCardPair(Enemy.Yuyuko,       "桜花「桜吹雪花小町」") },
                { new DayScenePair(Day.Day4,  7), new EnemiesCardPair(Enemy.Seiga,        "仙術「壁抜けワームホール」") },
                { new DayScenePair(Day.Day5,  1), new EnemiesCardPair(Enemy.Raiko,        string.Empty) },
                { new DayScenePair(Day.Day5,  2), new EnemiesCardPair(Enemy.Yatsuhashi,   "琴符「天の詔琴」") },
                { new DayScenePair(Day.Day5,  3), new EnemiesCardPair(Enemy.Benben,       "音符「大熱唱琵琶」") },
                { new DayScenePair(Day.Day5,  4), new EnemiesCardPair(Enemy.Raiko,        "雷符「怒りのデンデン太鼓」") },
                { new DayScenePair(Day.Day5,  5), new EnemiesCardPair(Enemy.Yatsuhashi,   "哀歌「人琴ともに亡ぶ」") },
                { new DayScenePair(Day.Day5,  6), new EnemiesCardPair(Enemy.Benben,       "楽譜「スコアウェブ」") },
                { new DayScenePair(Day.Day5,  7), new EnemiesCardPair(Enemy.Raiko,        "太鼓「ファンタジックウーファー」") },
                { new DayScenePair(Day.Day5,  8), new EnemiesCardPair(Enemy.Benben, Enemy.Yatsuhashi, "両吟「星降る唄」") },
                { new DayScenePair(Day.Day6,  1), new EnemiesCardPair(Enemy.Mamizou,      string.Empty) },
                { new DayScenePair(Day.Day6,  2), new EnemiesCardPair(Enemy.Aya,          "写真「激撮テングスクープ」") },
                { new DayScenePair(Day.Day6,  3), new EnemiesCardPair(Enemy.Hatate,       "写真「フルパノラマショット」") },
                { new DayScenePair(Day.Day6,  4), new EnemiesCardPair(Enemy.Nitori,       "瀑符「シライトフォール」") },
                { new DayScenePair(Day.Day6,  5), new EnemiesCardPair(Enemy.Momiji,       "牙符「咀嚼玩味」") },
                { new DayScenePair(Day.Day6,  6), new EnemiesCardPair(Enemy.Nitori,       "瀑符「ケゴンガン」") },
                { new DayScenePair(Day.Day6,  7), new EnemiesCardPair(Enemy.Hatate,       "写真「籠もりパパラッチ」") },
                { new DayScenePair(Day.Day6,  8), new EnemiesCardPair(Enemy.Aya,          "「瞬撮ジャーナリスト」") },
                { new DayScenePair(Day.Day7,  1), new EnemiesCardPair(Enemy.Marisa,       "恋符「ワイドマスター」") },
                { new DayScenePair(Day.Day7,  2), new EnemiesCardPair(Enemy.Sakuya,       "時符「タイムストッパー咲夜」") },
                { new DayScenePair(Day.Day7,  3), new EnemiesCardPair(Enemy.Youmu,        "光符「冥府光芒一閃」") },
                { new DayScenePair(Day.Day7,  4), new EnemiesCardPair(Enemy.Sanae,        "蛇符「バインドスネークカモン」") },
                { new DayScenePair(Day.Day7,  5), new EnemiesCardPair(Enemy.Marisa,       "恋符「マシンガンスパーク」") },
                { new DayScenePair(Day.Day7,  6), new EnemiesCardPair(Enemy.Sakuya,       "時符「チェンジリングマジック」") },
                { new DayScenePair(Day.Day7,  7), new EnemiesCardPair(Enemy.Youmu,        "彼岸剣「地獄極楽滅多斬り」") },
                { new DayScenePair(Day.Day7,  8), new EnemiesCardPair(Enemy.Sanae,        "蛇符「グリーンスネークカモン」") },
                { new DayScenePair(Day.Day8,  1), new EnemiesCardPair(Enemy.Shinmyoumaru, string.Empty) },
                { new DayScenePair(Day.Day8,  2), new EnemiesCardPair(Enemy.Reimu,        "神籤「反則結界」") },
                { new DayScenePair(Day.Day8,  3), new EnemiesCardPair(Enemy.Mamizou,      "「鳴かぬなら泣くまで待とう時鳥」") },
                { new DayScenePair(Day.Day8,  4), new EnemiesCardPair(Enemy.Shinmyoumaru, "「小人の地獄」") },
                { new DayScenePair(Day.Day8,  5), new EnemiesCardPair(Enemy.Reimu,        "「パスウェイジョンニードル」") },
                { new DayScenePair(Day.Day8,  6), new EnemiesCardPair(Enemy.Mamizou,      "「にんげんって良いな」") },
                { new DayScenePair(Day.Day8,  7), new EnemiesCardPair(Enemy.Shinmyoumaru, "輝針「鬼ごろし両目突きの針」") },
                { new DayScenePair(Day.Day9,  1), new EnemiesCardPair(Enemy.Kanako,       "御柱「ライジングオンバシラ」") },
                { new DayScenePair(Day.Day9,  2), new EnemiesCardPair(Enemy.Suwako,       "緑石「ジェイドブレイク」") },
                { new DayScenePair(Day.Day9,  3), new EnemiesCardPair(Enemy.Futo,         "古舟「エンシェントシップ」") },
                { new DayScenePair(Day.Day9,  4), new EnemiesCardPair(Enemy.Suika,        "鬼群「インプスウォーム」") },
                { new DayScenePair(Day.Day9,  5), new EnemiesCardPair(Enemy.Kanako,       "「神の御威光」") },
                { new DayScenePair(Day.Day9,  6), new EnemiesCardPair(Enemy.Suwako,       "蛙符「血塗られた赤蛙塚」") },
                { new DayScenePair(Day.Day9,  7), new EnemiesCardPair(Enemy.Futo,         "熱龍「火焔龍脈」") },
                { new DayScenePair(Day.Day9,  8), new EnemiesCardPair(Enemy.Suika,        "鬼群「百鬼禿童」") },
                { new DayScenePair(Day.Last,  1), new EnemiesCardPair(Enemy.Byakuren,     "「ハリの制縛」") },
                { new DayScenePair(Day.Last,  2), new EnemiesCardPair(Enemy.Miko,         "「我こそが天道なり」") },
                { new DayScenePair(Day.Last,  3), new EnemiesCardPair(Enemy.Tenshi,       "「全妖怪の緋想天」") },
                { new DayScenePair(Day.Last,  4), new EnemiesCardPair(Enemy.Remilia,      "「フィットフルナイトメア」") },
                { new DayScenePair(Day.Last,  5), new EnemiesCardPair(Enemy.Yukari,       "「不可能弾幕結界」") },
                { new DayScenePair(Day.Last,  6), new EnemiesCardPair(Enemy.Byakuren,     "「ブラフマーの瞳」") },
                { new DayScenePair(Day.Last,  7), new EnemiesCardPair(Enemy.Miko,         "「十七条の憲法爆弾」") },
                { new DayScenePair(Day.Last,  8), new EnemiesCardPair(Enemy.Tenshi,       "「鹿島鎮護」") },
                { new DayScenePair(Day.Last,  9), new EnemiesCardPair(Enemy.Remilia,      "「きゅうけつ鬼ごっこ」") },
                { new DayScenePair(Day.Last, 10), new EnemiesCardPair(Enemy.Yukari,       "「運鈍根の捕物帖」") }
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
                "究極反則生命体"
            };

        private static readonly EnumShortNameParser<Day> DayParser =
            new EnumShortNameParser<Day>();

        private static readonly EnumShortNameParser<ItemWithTotal> ItemWithTotalParser =
            new EnumShortNameParser<ItemWithTotal>();

        private static readonly string DayLongPattern =
            string.Join("|", Utils.GetEnumerator<Day>().Select(day => day.ToLongName()).ToArray());

        private AllScoreData allScoreData = null;
        private Dictionary<DayScenePair, BestShotPair> bestshots = null;

        public Th143Converter()
        {
        }

        public enum Day
        {
            [EnumAltName("1", LongName = "01")] Day1,
            [EnumAltName("2", LongName = "02")] Day2,
            [EnumAltName("3", LongName = "03")] Day3,
            [EnumAltName("4", LongName = "04")] Day4,
            [EnumAltName("5", LongName = "05")] Day5,
            [EnumAltName("6", LongName = "06")] Day6,
            [EnumAltName("7", LongName = "07")] Day7,
            [EnumAltName("8", LongName = "08")] Day8,
            [EnumAltName("9", LongName = "09")] Day9,
            [EnumAltName("L", LongName = "10")] Last
        }

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        public enum Enemy
        {
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
            [EnumAltName("紫",          LongName = "八雲 紫")]                Yukari
        }

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        public enum ItemWithTotal
        {
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
            [EnumAltName("T", LongName = "合計")]                     Total
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
                new ShotExReplacer(this, outputFilePath)
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

                var reader = new BinaryReader(input);
                var header = new BestShotHeader();
                header.ReadFrom(reader);

                if (this.bestshots == null)
                    this.bestshots = new Dictionary<DayScenePair, BestShotPair>(SpellCards.Count);

                var key = new DayScenePair(header.Day, header.Scene);
                if (!this.bestshots.ContainsKey(key))
                    this.bestshots.Add(key, new BestShotPair(outputFile.Name, header));

                Lzss.Extract(input, decoded);

                decoded.Seek(0, SeekOrigin.Begin);
                var bitmap = new Bitmap(header.Width, header.Height, PixelFormat.Format32bppArgb);
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

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        private static AllScoreData Read(Stream input)
        {
            var dictionary = new Dictionary<string, Action<AllScoreData, Chapter>>
            {
                { Score.ValidSignature,      (data, ch) => data.Set(new Score(ch))      },
                { ItemStatus.ValidSignature, (data, ch) => data.Set(new ItemStatus(ch)) },
                { Status.ValidSignature,     (data, ch) => data.Set(new Status(ch))     }
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

                    var key = new DayScenePair(day, scene);
                    var score = parent.allScoreData.Scores.Find(elem =>
                        (elem != null) && ((0 < elem.Number) && (elem.Number <= SpellCards.Count)) &&
                        SpellCards.ElementAt(elem.Number - 1).Key.Equals(key));

                    switch (type)
                    {
                        case 1:     // high score
                            return (score != null) ? Utils.ToNumberString(score.HighScore * 10) : "0";
                        case 2:     // challenge count
                            if (item == ItemWithTotal.NoItem)
                                return "-";
                            else
                                return (score != null)
                                    ? Utils.ToNumberString(score.ChallengeCounts[item]) : "0";
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

                    var key = new DayScenePair(day, scene);

                    if (hideUntriedCards)
                    {
                        var score = parent.allScoreData.Scores.Find(elem =>
                            (elem != null) && ((0 < elem.Number) && (elem.Number <= SpellCards.Count)) &&
                            SpellCards.ElementAt(elem.Number - 1).Key.Equals(key));
                        if ((score == null) || (score.ChallengeCounts[ItemWithTotal.Total] <= 0))
                            return "??????????";
                    }

                    if (type == 1)
                    {
                        var enemies = SpellCards[key].Enemies;
                        if (enemies.Length == 1)
                            return SpellCards[key].Enemy.ToLongName();
                        else
                            return string.Join(
                                " &amp; ", enemies.Select(enemy => enemy.ToLongName()).ToArray());
                    }
                    else
                        return SpellCards[key].Card;
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

                    if ((0 < number) && (number <= Nicknames.Count))
                        return (parent.allScoreData.Status.NicknameFlags[number] > 0)
                            ? Nicknames[number - 1] : "??????????";
                    else
                        return match.ToString();
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

                    var key = new DayScenePair(day, scene);

                    if (!string.IsNullOrEmpty(outputFilePath) &&
                        (parent.bestshots != null) &&
                        parent.bestshots.ContainsKey(key))
                    {
                        var relativePath = new Uri(outputFilePath)
                            .MakeRelativeUri(new Uri(parent.bestshots[key].Path)).OriginalString;
                        var alternativeString = Utils.Format("SpellName: {0}", SpellCards[key].Card);
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

                    var key = new DayScenePair(day, scene);

                    if (!string.IsNullOrEmpty(outputFilePath) &&
                        (parent.bestshots != null) &&
                        parent.bestshots.ContainsKey(key))
                        switch (type)
                        {
                            case 1:     // relative path to the bestshot file
                                return new Uri(outputFilePath)
                                    .MakeRelativeUri(new Uri(parent.bestshots[key].Path)).OriginalString;
                            case 2:     // width
                                return parent.bestshots[key]
                                    .Header.Width.ToString(CultureInfo.InvariantCulture);
                            case 3:     // height
                                return parent.bestshots[key]
                                    .Header.Height.ToString(CultureInfo.InvariantCulture);
                            case 4:     // date & time
                                return new DateTime(1970, 1, 1)
                                    .AddSeconds(parent.bestshots[key].Header.DateTime).ToLocalTime()
                                    .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                            default:    // unreachable
                                return match.ToString();
                        }
                    else
                        switch (type)
                        {
                            case 1: return string.Empty;
                            case 2: return "0";
                            case 3: return "0";
                            case 4: return "----/--/-- --:--:--";
                            default: return match.ToString();
                        }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        private class DayScenePair : Pair<Day, int>
        {
            public DayScenePair(Day day, int scene)
                : base(day, scene)
            {
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Day Day
            {
                get { return this.First; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int Scene
            {
                get { return this.Second; }     // 1-based
            }
        }

        private class EnemiesCardPair : Pair<Enemy[], string>
        {
            public EnemiesCardPair(Enemy enemy, string card)
                : base(new Enemy[] { enemy }, card)
            {
            }

            public EnemiesCardPair(Enemy enemy1, Enemy enemy2, string card)
                : base(new Enemy[] { enemy1, enemy2 }, card)
            {
            }

            public Enemy Enemy
            {
                get { return this.First[0]; }
            }

            public Enemy[] Enemies
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

        private class Header : IBinaryReadable, IBinaryWritable
        {
            public const string ValidSignature = "T341";
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
                this.Signature = Encoding.Default.GetString(reader.ReadBytes(SignatureSize));
                this.EncodedAllSize = reader.ReadInt32();
                this.unknown1 = reader.ReadUInt32();
                this.unknown2 = reader.ReadUInt32();
                this.EncodedBodySize = reader.ReadInt32();
                this.DecodedBodySize = reader.ReadInt32();
            }

            public void WriteTo(BinaryWriter writer)
            {
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
                this.Checksum = chapter.Checksum;
                this.Size = chapter.Size;
                this.Data = new byte[chapter.Data.Length];
                chapter.Data.CopyTo(this.Data, 0);
            }

            public string Signature { get; private set; }

            public ushort Version { get; private set; }

            public uint Checksum { get; private set; }

            public int Size { get; private set; }

            public bool IsValid
            {
                get
                {
                    int sum = BitConverter.GetBytes(this.Size).Sum(elem => (int)elem);
                    sum += this.Data.Sum(elem => (int)elem);
                    return (uint)sum == this.Checksum;
                }
            }

            protected byte[] Data { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.Signature = Encoding.Default.GetString(reader.ReadBytes(SignatureSize));
                this.Version = reader.ReadUInt16();
                this.Checksum = reader.ReadUInt32();
                this.Size = reader.ReadInt32();
                this.Data = reader.ReadBytes(
                    this.Size - SignatureSize - sizeof(ushort) - sizeof(uint) - sizeof(int));
            }
        }

        private class Score : Chapter   // per scene
        {
            public const string ValidSignature = "SN";
            public const ushort ValidVersion = 0x0001;
            public const int ValidSize = 0x00000314;

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
                using (var reader = new BinaryReader(stream))
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
                    reader.ReadBytes(0x548);    // always all 0x00?
                }
            }

            public int Number { get; private set; }

            public Dictionary<ItemWithTotal, int> ClearCounts { get; private set; }

            public Dictionary<ItemWithTotal, int> ChallengeCounts { get; private set; }

            public int HighScore { get; private set; }  // * 10

            public static bool CanInitialize(Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class ItemStatus : Chapter
        {
            public const string ValidSignature = "TI";
            public const ushort ValidVersion = 0x0001;
            public const int ValidSize = 0x00000034;

            public ItemStatus(Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Version != ValidVersion)
                    throw new InvalidDataException("Version");
                if (this.Size != ValidSize)
                    throw new InvalidDataException("Size");

                using (var stream = new MemoryStream(this.Data, false))
                using (var reader = new BinaryReader(stream))
                {
                    this.Item = (ItemWithTotal)reader.ReadInt32();
                    this.UseCount = reader.ReadInt32();
                    this.ClearedCount = reader.ReadInt32();
                    this.ClearedScenes = reader.ReadInt32();
                    this.ItemLevel = reader.ReadInt32();
                    reader.ReadInt32();
                    this.AvaliableCount = reader.ReadInt32();
                    this.FramesOrRanges = reader.ReadInt32();
                    reader.ReadInt32();                         // always 0?
                    reader.ReadInt32();                         // always 0?
                }
            }

            public ItemWithTotal Item { get; private set; }

            public int UseCount { get; private set; }

            public int ClearedCount { get; private set; }

            public int ClearedScenes { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int ItemLevel { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int AvaliableCount { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int FramesOrRanges { get; private set; }

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
            public const ushort ValidVersion = 0x0001;
            public const int ValidSize = 0x00000224;

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
                using (var reader = new BinaryReader(stream))
                {
                    this.LastName = reader.ReadBytes(14);
                    reader.ReadBytes(0x12);
                    this.BgmFlags = reader.ReadBytes(9);
                    reader.ReadBytes(0x17);
                    this.TotalPlayTime = reader.ReadInt32();
                    reader.ReadInt32();
                    this.LastMainItem = (ItemWithTotal)reader.ReadInt32();
                    this.LastSubItem = (ItemWithTotal)reader.ReadInt32();
                    reader.ReadBytes(0x54);
                    this.NicknameFlags = reader.ReadBytes(71);
                    reader.ReadBytes(0x12D);
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

            public static bool CanInitialize(Chapter chapter)
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
                this.Signature = Encoding.Default.GetString(reader.ReadBytes(SignatureSize));
                if (this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                {
                    reader.ReadUInt16();    // always 0xDF01?
                    this.Day = (Day)reader.ReadInt16();
                    this.Scene = (short)(reader.ReadInt16() + 1);
                    reader.ReadUInt16();    // 0x0100 ... Version?
                    this.Width = reader.ReadInt16();
                    this.Height = reader.ReadInt16();
                    reader.ReadUInt32();    // always 0x0005E800?
                    this.DateTime = reader.ReadUInt32();
                    this.SlowRate = reader.ReadSingle();    // really...?
                    reader.ReadBytes(0x58);
                }
            }
        }
    }
}
