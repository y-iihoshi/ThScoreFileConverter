//-----------------------------------------------------------------------
// <copyright file="Th125Converter.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.LayoutRules", "*", Justification = "Reviewed.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.LayoutRules",
    "SA1503:CurlyBracketsMustNotBeOmitted",
    Justification = "Reviewed.")]

namespace ThScoreFileConverter
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.OrderingRules",
        "SA1201:ElementsMustAppearInTheCorrectOrder",
        Justification = "Reviewed.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.SpacingRules",
        "SA1025:CodeMustNotContainMultipleWhitespaceInARow",
        Justification = "Reviewed.")]
    public class Th125Converter : ThConverter
    {
        private enum Level
        {
            [EnumAltName("1", LongName = "01")] Level1,
            [EnumAltName("2", LongName = "02")] Level2,
            [EnumAltName("3", LongName = "03")] Level3,
            [EnumAltName("4", LongName = "04")] Level4,
            [EnumAltName("5", LongName = "05")] Level5,
            [EnumAltName("6", LongName = "06")] Level6,
            [EnumAltName("7", LongName = "07")] Level7,
            [EnumAltName("8", LongName = "08")] Level8,
            [EnumAltName("9", LongName = "09")] Level9,
            [EnumAltName("A", LongName = "10")] Level10,
            [EnumAltName("B", LongName = "11")] Level11,
            [EnumAltName("C", LongName = "12")] Level12,
            [EnumAltName("X", LongName = "ex")] Extra,
            [EnumAltName("S", LongName = "sp")] Spoiler
        }

        private enum Chara
        {
            [EnumAltName("A")] Aya,
            [EnumAltName("H")] Hatate
        }

        private enum Enemy
        {
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
            [EnumAltName("文",          LongName = "射命丸 文")]        Aya
        }

        private class LevelScenePair : Pair<Level, int>
        {
            public Level Level { get { return this.First; } }
            public int Scene { get { return this.Second; } }    // 1-based

            public LevelScenePair(Level level, int scene) : base(level, scene) { }
        }

        private class EnemyCardPair : Pair<Enemy, string>
        {
            public Enemy Enemy { get { return this.First; } }
            public string Card { get { return this.Second; } }

            public EnemyCardPair(Enemy enemy, string card) : base(enemy, card) { }
        }

        private class BestShotPair : Pair<string, BestShotHeader>
        {
            public string Path { get { return this.First; } }
            public BestShotHeader Header { get { return this.Second; } }

            public BestShotPair(string name, BestShotHeader header) : base(name, header) { }
        }

        // Thanks to thwiki.info
        private static readonly Dictionary<LevelScenePair, EnemyCardPair> SpellCards =
            new Dictionary<LevelScenePair, EnemyCardPair>()
            {
                { new LevelScenePair(Level.Level1, 1), new EnemyCardPair(Enemy.Minoriko, string.Empty) },
                { new LevelScenePair(Level.Level1, 2), new EnemyCardPair(Enemy.Minoriko, string.Empty) },
                { new LevelScenePair(Level.Level1, 3), new EnemyCardPair(Enemy.Shizuha,  "秋符「フォーリンブラスト」") },
                { new LevelScenePair(Level.Level1, 4), new EnemyCardPair(Enemy.Minoriko, "実符「ウォームカラーハーヴェスト」") },
                { new LevelScenePair(Level.Level1, 5), new EnemyCardPair(Enemy.Shizuha,  "枯道「ロストウィンドロウ」") },
                { new LevelScenePair(Level.Level1, 6), new EnemyCardPair(Enemy.Minoriko, "焼芋「スイートポテトルーム」") },

                { new LevelScenePair(Level.Level2, 1), new EnemyCardPair(Enemy.Parsee, string.Empty) },
                { new LevelScenePair(Level.Level2, 2), new EnemyCardPair(Enemy.Hina,   string.Empty) },
                { new LevelScenePair(Level.Level2, 3), new EnemyCardPair(Enemy.Parsee, "嫉妬「ジェラシーボンバー」") },
                { new LevelScenePair(Level.Level2, 4), new EnemyCardPair(Enemy.Hina,   "厄野「禊川の堆積」") },
                { new LevelScenePair(Level.Level2, 5), new EnemyCardPair(Enemy.Parsee, "怨み念法「積怨返し」") },
                { new LevelScenePair(Level.Level2, 6), new EnemyCardPair(Enemy.Hina,   "災禍「呪いの雛人形」") },

                { new LevelScenePair(Level.Level3, 1), new EnemyCardPair(Enemy.Yamame, string.Empty) },
                { new LevelScenePair(Level.Level3, 2), new EnemyCardPair(Enemy.Kogasa, "傘符「一本足ピッチャー返し」") },
                { new LevelScenePair(Level.Level3, 3), new EnemyCardPair(Enemy.Kisume, "釣瓶「飛んで井の中」") },
                { new LevelScenePair(Level.Level3, 4), new EnemyCardPair(Enemy.Yamame, "細綱「カンダタロープ」") },
                { new LevelScenePair(Level.Level3, 5), new EnemyCardPair(Enemy.Kogasa, "虹符「オーバー・ザ・レインボー」") },
                { new LevelScenePair(Level.Level3, 6), new EnemyCardPair(Enemy.Kisume, "釣瓶「ウェルディストラクター」") },
                { new LevelScenePair(Level.Level3, 7), new EnemyCardPair(Enemy.Yamame, "毒符「樺黄小町」") },
                { new LevelScenePair(Level.Level3, 8), new EnemyCardPair(Enemy.Kogasa, "傘符「細雪の過客」") },

                { new LevelScenePair(Level.Level4, 1), new EnemyCardPair(Enemy.Nitori, string.Empty) },
                { new LevelScenePair(Level.Level4, 2), new EnemyCardPair(Enemy.Momiji, string.Empty) },
                { new LevelScenePair(Level.Level4, 3), new EnemyCardPair(Enemy.Nitori, "水符「ウォーターカーペット」") },
                { new LevelScenePair(Level.Level4, 4), new EnemyCardPair(Enemy.Momiji, "狗符「レイビーズバイト」") },
                { new LevelScenePair(Level.Level4, 5), new EnemyCardPair(Enemy.Nitori, "河符「ディバイディングエッジ」") },
                { new LevelScenePair(Level.Level4, 6), new EnemyCardPair(Enemy.Momiji, "山窩「エクスペリーズカナン」") },
                { new LevelScenePair(Level.Level4, 7), new EnemyCardPair(Enemy.Nitori, "河童「乾燥尻子玉」") },

                { new LevelScenePair(Level.Level5, 1), new EnemyCardPair(Enemy.Ichirin,   string.Empty) },
                { new LevelScenePair(Level.Level5, 2), new EnemyCardPair(Enemy.Minamitsu, string.Empty) },
                { new LevelScenePair(Level.Level5, 3), new EnemyCardPair(Enemy.Ichirin,   "拳骨「天空鉄槌落とし」") },
                { new LevelScenePair(Level.Level5, 4), new EnemyCardPair(Enemy.Minamitsu, "錨符「幽霊船長期停泊」") },
                { new LevelScenePair(Level.Level5, 5), new EnemyCardPair(Enemy.Ichirin,   "稲妻「帯電入道」") },
                { new LevelScenePair(Level.Level5, 6), new EnemyCardPair(Enemy.Minamitsu, "浸水「船底のヴィーナス」") },
                { new LevelScenePair(Level.Level5, 7), new EnemyCardPair(Enemy.Ichirin,   "鉄拳「入道にょき」") },
                { new LevelScenePair(Level.Level5, 8), new EnemyCardPair(Enemy.Minamitsu, "「ディープシンカー」") },

                { new LevelScenePair(Level.Level6, 1), new EnemyCardPair(Enemy.Yuugi, string.Empty) },
                { new LevelScenePair(Level.Level6, 2), new EnemyCardPair(Enemy.Suika, string.Empty) },
                { new LevelScenePair(Level.Level6, 3), new EnemyCardPair(Enemy.Yuugi, "光鬼「金剛螺旋」") },
                { new LevelScenePair(Level.Level6, 4), new EnemyCardPair(Enemy.Suika, "鬼符「豆粒大の針地獄」") },
                { new LevelScenePair(Level.Level6, 5), new EnemyCardPair(Enemy.Yuugi, "鬼符「鬼気狂瀾」") },
                { new LevelScenePair(Level.Level6, 6), new EnemyCardPair(Enemy.Suika, "地獄「煉獄吐息」") },
                { new LevelScenePair(Level.Level6, 7), new EnemyCardPair(Enemy.Yuugi, "鬼声「壊滅の咆哮」") },
                { new LevelScenePair(Level.Level6, 8), new EnemyCardPair(Enemy.Suika, "鬼符「ミッシングパワー」") },

                { new LevelScenePair(Level.Level7, 1), new EnemyCardPair(Enemy.Shou,   string.Empty) },
                { new LevelScenePair(Level.Level7, 2), new EnemyCardPair(Enemy.Nazrin, string.Empty) },
                { new LevelScenePair(Level.Level7, 3), new EnemyCardPair(Enemy.Shou,   "寅符「ハングリータイガー」") },
                { new LevelScenePair(Level.Level7, 4), new EnemyCardPair(Enemy.Nazrin, "棒符「ナズーリンロッド」") },
                { new LevelScenePair(Level.Level7, 5), new EnemyCardPair(Enemy.Shou,   "天符「焦土曼荼羅」") },
                { new LevelScenePair(Level.Level7, 6), new EnemyCardPair(Enemy.Nazrin, "財宝「ゴールドラッシュ」") },
                { new LevelScenePair(Level.Level7, 7), new EnemyCardPair(Enemy.Shou,   "宝符「黄金の震眩」") },

                { new LevelScenePair(Level.Level8, 1), new EnemyCardPair(Enemy.Rin,    string.Empty) },
                { new LevelScenePair(Level.Level8, 2), new EnemyCardPair(Enemy.Utsuho, "熔解「メルティングホワイト」") },
                { new LevelScenePair(Level.Level8, 3), new EnemyCardPair(Enemy.Rin,    "死符「ゴーストタウン」") },
                { new LevelScenePair(Level.Level8, 4), new EnemyCardPair(Enemy.Utsuho, "巨星「レッドジャイアント」") },
                { new LevelScenePair(Level.Level8, 5), new EnemyCardPair(Enemy.Rin,    "「死体繁華街」") },
                { new LevelScenePair(Level.Level8, 6), new EnemyCardPair(Enemy.Utsuho, "星符「巨星墜つ」") },
                { new LevelScenePair(Level.Level8, 7), new EnemyCardPair(Enemy.Rin,    "酔歩「キャットランダムウォーク」") },
                { new LevelScenePair(Level.Level8, 8), new EnemyCardPair(Enemy.Utsuho, "七星「セプテントリオン」") },

                { new LevelScenePair(Level.Level9, 1), new EnemyCardPair(Enemy.Satori, string.Empty) },
                { new LevelScenePair(Level.Level9, 2), new EnemyCardPair(Enemy.Koishi, "心符「没我の愛」") },
                { new LevelScenePair(Level.Level9, 3), new EnemyCardPair(Enemy.Satori, "脳符「ブレインフィンガープリント」") },
                { new LevelScenePair(Level.Level9, 4), new EnemyCardPair(Enemy.Koishi, "記憶「ＤＮＡの瑕」") },
                { new LevelScenePair(Level.Level9, 5), new EnemyCardPair(Enemy.Satori, "心花「カメラシャイローズ」") },
                { new LevelScenePair(Level.Level9, 6), new EnemyCardPair(Enemy.Koishi, "「胎児の夢」") },
                { new LevelScenePair(Level.Level9, 7), new EnemyCardPair(Enemy.Satori, "想起「うろおぼえの金閣寺」") },
                { new LevelScenePair(Level.Level9, 8), new EnemyCardPair(Enemy.Koishi, "「ローズ地獄」") },

                { new LevelScenePair(Level.Level10, 1), new EnemyCardPair(Enemy.Tenshi, "気性「勇気凛々の剣」") },
                { new LevelScenePair(Level.Level10, 2), new EnemyCardPair(Enemy.Iku,    "雷符「ライトニングフィッシュ」") },
                { new LevelScenePair(Level.Level10, 3), new EnemyCardPair(Enemy.Tenshi, "地震「避難険路」") },
                { new LevelScenePair(Level.Level10, 4), new EnemyCardPair(Enemy.Iku,    "珠符「五爪龍の珠」") },
                { new LevelScenePair(Level.Level10, 5), new EnemyCardPair(Enemy.Tenshi, "要石「カナメファンネル」") },
                { new LevelScenePair(Level.Level10, 6), new EnemyCardPair(Enemy.Iku,    "龍宮「タイヤヒラメダンス」") },
                { new LevelScenePair(Level.Level10, 7), new EnemyCardPair(Enemy.Tenshi, "「全人類の緋想天」") },
                { new LevelScenePair(Level.Level10, 8), new EnemyCardPair(Enemy.Iku,    "龍魚「龍宮の使い遊泳弾」") },

                { new LevelScenePair(Level.Level11, 1), new EnemyCardPair(Enemy.Kanako, string.Empty) },
                { new LevelScenePair(Level.Level11, 2), new EnemyCardPair(Enemy.Suwako, "神桜「湛えの桜吹雪」") },
                { new LevelScenePair(Level.Level11, 3), new EnemyCardPair(Enemy.Kanako, "蛇符「グラウンドサーペント」") },
                { new LevelScenePair(Level.Level11, 4), new EnemyCardPair(Enemy.Suwako, "姫川「プリンセスジェイドグリーン」") },
                { new LevelScenePair(Level.Level11, 5), new EnemyCardPair(Enemy.Kanako, "御柱「メテオリックオンバシラ」") },
                { new LevelScenePair(Level.Level11, 6), new EnemyCardPair(Enemy.Suwako, "鉄輪「ミシカルリング」") },
                { new LevelScenePair(Level.Level11, 7), new EnemyCardPair(Enemy.Kanako, "儚道「御神渡りクロス」") },
                { new LevelScenePair(Level.Level11, 8), new EnemyCardPair(Enemy.Suwako, "土着神「御射軍神さま」") },

                { new LevelScenePair(Level.Level12, 1), new EnemyCardPair(Enemy.Byakuren, string.Empty) },
                { new LevelScenePair(Level.Level12, 2), new EnemyCardPair(Enemy.Nue,      "正体不明「紫鏡」") },
                { new LevelScenePair(Level.Level12, 3), new EnemyCardPair(Enemy.Byakuren, "「遊行聖」") },
                { new LevelScenePair(Level.Level12, 4), new EnemyCardPair(Enemy.Nue,      "正体不明「赤マント青マント」") },
                { new LevelScenePair(Level.Level12, 5), new EnemyCardPair(Enemy.Byakuren, "習合「垂迹大日如来」") },
                { new LevelScenePair(Level.Level12, 6), new EnemyCardPair(Enemy.Nue,      "正体不明「厠の花子さん」") },
                { new LevelScenePair(Level.Level12, 7), new EnemyCardPair(Enemy.Byakuren, "「スターソードの護法」") },
                { new LevelScenePair(Level.Level12, 8), new EnemyCardPair(Enemy.Nue,      "「遊星よりの弾幕Ｘ」") },

                { new LevelScenePair(Level.Extra, 1), new EnemyCardPair(Enemy.Reimu,  "お札「新聞拡張団調伏」") },
                { new LevelScenePair(Level.Extra, 2), new EnemyCardPair(Enemy.Marisa, "星符「オールトクラウド」") },
                { new LevelScenePair(Level.Extra, 3), new EnemyCardPair(Enemy.Sanae,  "奇跡「弘安の神風」") },
                { new LevelScenePair(Level.Extra, 4), new EnemyCardPair(Enemy.Reimu,  "結界「パパラッチ撃退結界」") },
                { new LevelScenePair(Level.Extra, 5), new EnemyCardPair(Enemy.Marisa, "天儀「オーレリーズソーラーシステム」") },
                { new LevelScenePair(Level.Extra, 6), new EnemyCardPair(Enemy.Sanae,  "蛙符「手管の蝦蟇」") },
                { new LevelScenePair(Level.Extra, 7), new EnemyCardPair(Enemy.Reimu,  "夢符「夢想亜空穴」") },
                { new LevelScenePair(Level.Extra, 8), new EnemyCardPair(Enemy.Marisa, "彗星「ブレイジングスター」") },
                { new LevelScenePair(Level.Extra, 9), new EnemyCardPair(Enemy.Sanae,  "妖怪退治「妖力スポイラー」") },

                { new LevelScenePair(Level.Spoiler, 1), new EnemyCardPair(Enemy.Hatate, string.Empty) },
                { new LevelScenePair(Level.Spoiler, 2), new EnemyCardPair(Enemy.Hatate, "取材「姫海棠はたての練習取材」") },
                { new LevelScenePair(Level.Spoiler, 3), new EnemyCardPair(Enemy.Hatate, "連写「ラピッドショット」") },
                { new LevelScenePair(Level.Spoiler, 4), new EnemyCardPair(Enemy.Hatate, "遠眼「天狗サイコグラフィ」") },
                { new LevelScenePair(Level.Spoiler, 5), new EnemyCardPair(Enemy.Aya,    string.Empty) },
                { new LevelScenePair(Level.Spoiler, 6), new EnemyCardPair(Enemy.Aya,    "取材「射命丸文の圧迫取材」") },
                { new LevelScenePair(Level.Spoiler, 7), new EnemyCardPair(Enemy.Aya,    "望遠「キャンディッドショット」") },
                { new LevelScenePair(Level.Spoiler, 8), new EnemyCardPair(Enemy.Aya,    "速写「ファストショット」") },
                { new LevelScenePair(Level.Spoiler, 9), new EnemyCardPair(Enemy.Aya,    "「幻想風靡」") }
            };

        private class AllScoreData
        {
            public Header Header { get; set; }
            public List<Score> Scores { get; set; }
            public Status Status { get; set; }
        }

        private class Header : IBinaryReadable
        {
            private uint unknown1;
            private uint unknown2;

            public string Signature { get; private set; }
            public int EncodedAllSize { get; private set; }
            public int EncodedBodySize { get; private set; }
            public int DecodedBodySize { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.Signature = Encoding.Default.GetString(reader.ReadBytes(4));
                this.EncodedAllSize = reader.ReadInt32();
                this.unknown1 = reader.ReadUInt32();
                this.unknown2 = reader.ReadUInt32();
                this.EncodedBodySize = reader.ReadInt32();
                this.DecodedBodySize = reader.ReadInt32();
            }

            public void WriteTo(BinaryWriter writer)
            {
                writer.Write(this.Signature.ToCharArray());
                writer.Write(this.EncodedAllSize);
                writer.Write(this.unknown1);
                writer.Write(this.unknown2);
                writer.Write(this.EncodedBodySize);
                writer.Write(this.DecodedBodySize);
            }
        }

        private class Chapter : IBinaryReadable
        {
            public string Signature { get; private set; }
            public ushort Version { get; private set; }
            public int Size { get; private set; }
            public uint Checksum { get; private set; }

            public Chapter() { }
            public Chapter(Chapter ch)
            {
                this.Signature = ch.Signature;
                this.Version = ch.Version;
                this.Size = ch.Size;
                this.Checksum = ch.Checksum;
            }

            public virtual void ReadFrom(BinaryReader reader)
            {
                this.Signature = Encoding.Default.GetString(reader.ReadBytes(2));
                this.Version = reader.ReadUInt16();
                this.Size = reader.ReadInt32();
                this.Checksum = reader.ReadUInt32();
            }
        }

        private class Score : Chapter   // per scene
        {
            public LevelScenePair LevelScene { get; private set; }
            public int HighScore { get; private set; }
            public Chara Chara { get; private set; }        // size: 4Bytes
            public int TrialCount { get; private set; }
            public int FirstSuccess { get; private set; }
            public uint DateTime { get; private set; }      // UNIX time (unit: [s])
            public int BestshotScore { get; private set; }

            public Score(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "SC")
                    throw new InvalidDataException("Signature");
                if (this.Version != 0x0000)
                    throw new InvalidDataException("Version");
                if (this.Size != 0x00000048)
                    throw new InvalidDataException("Size");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                var number = reader.ReadInt32();
                this.LevelScene = new LevelScenePair((Level)(number / 10), (number % 10) + 1);
                this.HighScore = reader.ReadInt32();
                reader.ReadBytes(0x04);
                this.Chara = (Chara)reader.ReadInt32();
                reader.ReadBytes(0x04);
                this.TrialCount = reader.ReadInt32();
                this.FirstSuccess = reader.ReadInt32();
                reader.ReadUInt32();    // always 0x00000000?
                this.DateTime = reader.ReadUInt32();
                reader.ReadUInt32();    // always 0x00000000?
                reader.ReadUInt32();    // checksum of the bestshot file?
                reader.ReadUInt32();    // always 0x00000001?
                this.BestshotScore = reader.ReadInt32();
                reader.ReadBytes(0x08);
            }
        }

        private class Status : Chapter
        {
            public byte[] LastName { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)
            public byte[] BgmFlags { get; private set; }    // .Length = 6
            public int TotalPlayTime { get; private set; }  // unit: [0.01s]

            public Status(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "ST")
                    throw new InvalidDataException("Signature");
                if (this.Version != 0x0001)
                    throw new InvalidDataException("Version");
                if (this.Size != 0x00000474)
                    throw new InvalidDataException("Size");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.LastName = reader.ReadBytes(10);
                reader.ReadBytes(2);
                this.BgmFlags = reader.ReadBytes(6);
                reader.ReadBytes(0x2E);
                this.TotalPlayTime = reader.ReadInt32();
                reader.ReadBytes(0x424);
            }
        }

        private class BestShotHeader : IBinaryReadable
        {
            public struct BonusFields
            {
                private BitVector32 data;

                public BonusFields(int data) { this.data = new BitVector32(data); }

                public int Data { get { return this.data.Data; } }

                public bool Bit00        { get { return this.data[0x00000001]; } }
                public bool Bit01        { get { return this.data[0x00000002]; } }
                public bool TwoShot      { get { return this.data[0x00000004]; } }
                public bool NiceShot     { get { return this.data[0x00000008]; } }
                public bool RiskBonus    { get { return this.data[0x00000010]; } }
                public bool Bit05        { get { return this.data[0x00000020]; } }
                public bool RedShot      { get { return this.data[0x00000040]; } }
                public bool PurpleShot   { get { return this.data[0x00000080]; } }

                public bool BlueShot     { get { return this.data[0x00000100]; } }
                public bool CyanShot     { get { return this.data[0x00000200]; } }
                public bool GreenShot    { get { return this.data[0x00000400]; } }
                public bool YellowShot   { get { return this.data[0x00000800]; } }
                public bool OrangeShot   { get { return this.data[0x00001000]; } }
                public bool ColorfulShot { get { return this.data[0x00002000]; } }
                public bool RainbowShot  { get { return this.data[0x00004000]; } }
                public bool Bit15        { get { return this.data[0x00008000]; } }

                public bool SoloShot     { get { return this.data[0x00010000]; } }
                public bool Bit17        { get { return this.data[0x00020000]; } }
                public bool Bit18        { get { return this.data[0x00040000]; } }
                public bool Bit19        { get { return this.data[0x00080000]; } }
                public bool Bit20        { get { return this.data[0x00100000]; } }
                public bool Bit21        { get { return this.data[0x00200000]; } }
                public bool MacroBonus   { get { return this.data[0x00400000]; } }
                public bool Bit23        { get { return this.data[0x00800000]; } }

                public bool FrontShot    { get { return this.data[0x01000000]; } }
                public bool BackShot     { get { return this.data[0x02000000]; } }
                public bool SideShot     { get { return this.data[0x04000000]; } }
                public bool ClearShot    { get { return this.data[0x08000000]; } }
                public bool CatBonus     { get { return this.data[0x10000000]; } }
                public bool Bit29        { get { return this.data[0x20000000]; } }
                public bool Bit30        { get { return this.data[0x40000000]; } }
                // public bool Bit31 { get { return this.data[0x80000000]; } }
            }

            public string Signature { get; private set; }   // "BST2"
            public Level Level { get; private set; }
            public short Scene { get; private set; }        // 1-based
            public short Width { get; private set; }
            public short Height { get; private set; }
            public short Width2 { get; private set; }       // ???
            public short Height2 { get; private set; }      // ???
            public short HalfWidth { get; private set; }    // ???
            public short HalfHeight { get; private set; }   // ???
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
            public float Angle { get; private set; }
            public int ResultScore2 { get; private set; }   // ???
            public byte[] CardName { get; private set; }    // .Length = 0x50

            public void ReadFrom(BinaryReader reader)
            {
                this.Signature = new string(reader.ReadChars(4));
                if (this.Signature == "BST2")
                {
                    reader.ReadUInt16();    // always 0x0405?
                    this.Level = (Level)(reader.ReadInt16() - 1);
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
                    reader.ReadBytes(0x08);
                    this.RiskBonus = reader.ReadInt32();
                    this.BossShot = reader.ReadSingle();
                    this.NiceShot = reader.ReadSingle();
                    this.AngleBonus = reader.ReadSingle();
                    this.MacroBonus = reader.ReadInt32();
                    this.FrontSideBackShot = reader.ReadInt32();
                    this.ClearShot = reader.ReadInt32();
                    reader.ReadBytes(0x30);
                    this.Angle = reader.ReadSingle();
                    this.ResultScore2 = reader.ReadInt32();
                    reader.ReadUInt32();
                    this.CardName = reader.ReadBytes(0x50);
                }
            }
        }

        private AllScoreData allScoreData = null;
        private Dictionary<Chara, Dictionary<LevelScenePair, BestShotPair>> bestshots = null;

        public override string SupportedVersions
        {
            get { return "1.00a"; }
        }

        public override bool HasBestShotConverter
        {
            get { return true; }
        }

        private static readonly string LevelPattern;
        private static readonly string LevelLongPattern;
        private static readonly string CharaPattern;

        private static readonly Func<string, StringComparison, Level> ToLevel;
        private static readonly Func<string, StringComparison, Chara> ToChara;

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Performance",
            "CA1810:InitializeReferenceTypeStaticFieldsInline",
            Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        static Th125Converter()
        {
            var levels = Utils.GetEnumerator<Level>();
            var charas = Utils.GetEnumerator<Chara>();

            LevelPattern = string.Join(string.Empty, levels.Select(lv => lv.ToShortName()).ToArray());
            LevelLongPattern = string.Join("|", levels.Select(lv => lv.ToLongName()).ToArray());
            CharaPattern = string.Join("|", charas.Select(ch => ch.ToShortName()).ToArray());

            ToLevel = ((shortName, comparisonType) =>
                levels.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToChara = ((shortName, comparisonType) =>
                charas.First(ch => ch.ToShortName().Equals(shortName, comparisonType)));
        }

        public Th125Converter()
        {
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

        private static bool Decrypt(Stream input, Stream output)
        {
            var reader = new BinaryReader(input);
            var writer = new BinaryWriter(output);

            var header = new Header();
            header.ReadFrom(reader);

            if (header.Signature != "T125")
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
                while (true)
                {
                    var signature = reader.ReadUInt32();
                    reader.BaseStream.Seek(-4, SeekOrigin.Current);

                    chapter.ReadFrom(reader);
                    if (!((chapter.Signature == "SC") && (chapter.Version == 0x0000)) &&
                        !((chapter.Signature == "ST") && (chapter.Version == 0x0001)))
                        return false;

                    long sum = signature + chapter.Size;
                    // 3 means Signature, Checksum, and Size.
                    for (var count = 3; count < chapter.Size / sizeof(uint); count++)
                        sum += reader.ReadUInt32();
                    if ((uint)sum != chapter.Checksum)
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

        private static AllScoreData Read(Stream input)
        {
            var reader = new BinaryReader(input);
            var allScoreData = new AllScoreData();
            var chapter = new Chapter();

            allScoreData.Scores = new List<Score>(SpellCards.Count);
            allScoreData.Header = new Header();
            allScoreData.Header.ReadFrom(reader);

            try
            {
                while (true)
                {
                    chapter.ReadFrom(reader);
                    switch (chapter.Signature)
                    {
                        case "SC":
                            {
                                var score = new Score(chapter);
                                score.ReadFrom(reader);
                                allScoreData.Scores.Add(score);
                            }
                            break;
                        case "ST":
                            {
                                var status = new Status(chapter);
                                status.ReadFrom(reader);
                                allScoreData.Status = status;
                            }
                            break;
                        default:
                            // 12 means the total size of Signature, Version, Size and Checksum.
                            reader.ReadBytes(chapter.Size - 12);
                            break;
                    }
                }
            }
            catch (EndOfStreamException)
            {
                // It's OK, do nothing.
            }

            if ((allScoreData.Header != null) &&
                // (allScoreData.scores.Count >= 0) &&
                (allScoreData.Status != null))
                return allScoreData;
            else
                return null;
        }

        protected override void Convert(Stream input, Stream output)
        {
            var reader = new StreamReader(input, Encoding.GetEncoding("shift_jis"));
            var writer = new StreamWriter(output, Encoding.GetEncoding("shift_jis"));
            var outputFile = output as FileStream;

            var allLine = reader.ReadToEnd();
            allLine = this.ReplaceScore(allLine);
            allLine = this.ReplaceScoreTotal(allLine);
            allLine = this.ReplaceCard(allLine);
            allLine = this.ReplaceTime(allLine);
            if (outputFile != null)
            {
                allLine = this.ReplaceShot(allLine, outputFile.Name);
                allLine = this.ReplaceShotEx(allLine, outputFile.Name);
            }
            writer.Write(allLine);

            writer.Flush();
            writer.BaseStream.SetLength(writer.BaseStream.Position);
        }

        // %T125SCR[w][x][y][z]
        private string ReplaceScore(string input)
        {
            var pattern = Utils.Format(
                @"%T125SCR([{0}])([{1}])([1-9])([1-5])", CharaPattern, LevelPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var chara = ToChara(match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
                var level = ToLevel(match.Groups[2].Value, StringComparison.OrdinalIgnoreCase);
                var scene = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                var key = new LevelScenePair(level, scene);
                var score = this.allScoreData.Scores.Find(elem =>
                    (elem != null) && (elem.Chara == chara) && elem.LevelScene.Equals(key));

                switch (type)
                {
                    case 1:     // high score
                        return (score != null) ? this.ToNumberString(score.HighScore) : "0";
                    case 2:     // bestshot score
                        return (score != null) ? this.ToNumberString(score.BestshotScore) : "0";
                    case 3:     // num of shots
                        return (score != null) ? this.ToNumberString(score.TrialCount) : "0";
                    case 4:     // num of shots for the first success
                        return (score != null) ? this.ToNumberString(score.FirstSuccess) : "0";
                    case 5:     // date & time
                        if (score != null)
                            return new DateTime(1970, 1, 1).AddSeconds(score.DateTime)
                                .ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                        else
                            return "----/--/-- --:--:--";
                    default:    // unreachable
                        return match.ToString();
                }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T125SCRTL[x][y][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceScoreTotal(string input)
        {
            var pattern = Utils.Format(@"%T125SCRTL([{0}])([12])([1-5])", CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var chara = ToChara(match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
                var method = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                Func<Score, bool> triedAndSucceeded = (score =>
                    (score.TrialCount > 0) && (score.FirstSuccess > 0));
                Func<Score, bool> isTarget = (score =>
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
                                return false;
                        }
                        else
                            return score.Chara == chara;
                    }
                    else
                        return score.Chara == chara;
                });

                switch (type)
                {
                    case 1:     // total score
                        return this.ToNumberString(
                            this.allScoreData.Scores.Sum(
                                score => (isTarget(score) && triedAndSucceeded(score))
                                    ? (long)score.HighScore : 0L));
                    case 2:     // total of bestshot scores
                        return this.ToNumberString(
                            this.allScoreData.Scores.Sum(
                                score => isTarget(score) ? (long)score.BestshotScore : 0L));
                    case 3:     // total of num of shots
                        return this.ToNumberString(
                            this.allScoreData.Scores.Sum(
                                score => isTarget(score) ? score.TrialCount : 0));
                    case 4:     // total of num of shots for the first success
                        return this.ToNumberString(
                            this.allScoreData.Scores.Sum(
                                score => (isTarget(score) && triedAndSucceeded(score))
                                    ? (long)score.FirstSuccess : 0L));
                    case 5:     // num of succeeded scenes
                        return this.allScoreData.Scores
                            .Count(score => isTarget(score) && triedAndSucceeded(score))
                            .ToString(CultureInfo.CurrentCulture);
                    default:    // unreachable
                        return match.ToString();
                }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T125CARD[x][y][z]
        private string ReplaceCard(string input)
        {
            var pattern = Utils.Format(@"%T125CARD([{0}])([1-9])([12])", LevelPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
                var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                var key = new LevelScenePair(level, scene);
                var score = this.allScoreData.Scores.Find(
                    elem => (elem != null) && elem.LevelScene.Equals(key));

                switch (type)
                {
                    case 1:     // target Name
                        return (score != null) ? SpellCards[key].Enemy.ToLongName() : "??????????";
                    case 2:     // spell card Name
                        return (score != null) ? SpellCards[key].Card : "??????????";
                    default:    // unreachable
                        return match.ToString();
                }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T125TIMEPLY
        private string ReplaceTime(string input)
        {
            var pattern = @"%T125TIMEPLY";
            var evaluator = new MatchEvaluator(match =>
            {
                return new Time(this.allScoreData.Status.TotalPlayTime * 10, false).ToLongString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T125SHOT[x][y][z]
        private string ReplaceShot(string input, string outputFilePath)
        {
            var pattern = Utils.Format(@"%T125SHOT([{0}])([{1}])([1-9])", CharaPattern, LevelPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var chara = ToChara(match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
                var level = ToLevel(match.Groups[2].Value, StringComparison.OrdinalIgnoreCase);
                var scene = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                var bestshots = this.bestshots.ContainsKey(chara) ? this.bestshots[chara] : null;
                var key = new LevelScenePair(level, scene);

                if ((bestshots != null) && bestshots.ContainsKey(key))
                {
                    var relativePath = new Uri(outputFilePath)
                        .MakeRelativeUri(new Uri(bestshots[key].Path)).OriginalString;
                    var alternativeString = Utils.Format(
                        "ClearData: {0}\nSlow: {1:F6}%\nSpellName: {2}",
                        this.ToNumberString(bestshots[key].Header.ResultScore),
                        bestshots[key].Header.SlowRate,
                        Encoding.Default.GetString(bestshots[key].Header.CardName).TrimEnd('\0'));
                    return Utils.Format(
                        "<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" border=0>",
                        relativePath,
                        alternativeString);
                }
                else
                    return string.Empty;
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
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

        // %T125SHOTEX[w][x][y][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceShotEx(string input, string outputFilePath)
        {
            Func<BestShotHeader, List<Detail>> detailList = (header => new List<Detail>
            {
                new Detail(true,                       "Base Point    {0,9}", this.ToNumberString(header.BasePoint)),
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
                new Detail(header.Fields.RainbowShot,  "Rainbow Shot  {0,9}", Utils.Format("+ {0}", this.ToNumberString(2100))),
                new Detail(header.Fields.RiskBonus,    "Risk Bonus    {0,9}", Utils.Format("+ {0}", this.ToNumberString(header.RiskBonus))),
                new Detail(header.Fields.MacroBonus,   "Macro Bonus   {0,9}", Utils.Format("+ {0}", this.ToNumberString(header.MacroBonus))),
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
                new Detail(true,                       "Result Score  {0,9}", this.ToNumberString(header.ResultScore))
            });
            var pattern = Utils.Format(
                @"%T125SHOTEX([{0}])([{1}])([1-9])([1-7])", CharaPattern, LevelPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var chara = ToChara(match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
                var level = ToLevel(match.Groups[2].Value, StringComparison.OrdinalIgnoreCase);
                var scene = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                var bestshots = this.bestshots.ContainsKey(chara) ? this.bestshots[chara] : null;
                var key = new LevelScenePair(level, scene);

                if ((bestshots != null) && bestshots.ContainsKey(key))
                    switch (type)
                    {
                        case 1:     // relative path to the bestshot file
                            return new Uri(outputFilePath)
                                .MakeRelativeUri(new Uri(bestshots[key].Path)).OriginalString;
                        case 2:     // width
                            return bestshots[key].Header.Width.ToString(CultureInfo.InvariantCulture);
                        case 3:     // height
                            return bestshots[key].Header.Height.ToString(CultureInfo.InvariantCulture);
                        case 4:     // score
                            return this.ToNumberString(bestshots[key].Header.ResultScore);
                        case 5:     // slow rate
                            return Utils.Format("{0:F6}%", bestshots[key].Header.SlowRate);
                        case 6:     // date & time
                            {
                                var score = this.allScoreData.Scores.Find(elem =>
                                    (elem != null) && (elem.Chara == chara) && elem.LevelScene.Equals(key));
                                if (score != null)
                                    return new DateTime(1970, 1, 1).AddSeconds(score.DateTime).ToLocalTime()
                                        .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                                else
                                    return "----/--/-- --:--:--";
                            }
                        case 7:     // detail info
                            {
                                var detailStrings = detailList(bestshots[key].Header)
                                    .Where(detail => detail.Outputs)
                                    .Select(detail => Utils.Format(detail.Format, detail.Value));
                                return string.Join("\r\n", detailStrings.ToArray());
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
                        case 7: return string.Empty;
                        default: return match.ToString();
                    }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        protected override string[] FilterBestShotFiles(string[] files)
        {
            var pattern = Utils.Format(@"bs2?_({0})_[1-9].dat", LevelLongPattern);
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return files.Where(file => regex.IsMatch(Path.GetFileName(file))).ToArray();
        }

        protected override void ConvertBestShot(Stream input, Stream output)
        {
            using (var decoded = new MemoryStream())
            {
                var outputFile = output as FileStream;
                var chara = Path.GetFileName(outputFile.Name)
                    .StartsWith("bs2_", StringComparison.CurrentCultureIgnoreCase)
                    ? Chara.Hatate : Chara.Aya;

                var reader = new BinaryReader(input);
                var header = new BestShotHeader();
                header.ReadFrom(reader);

                if (this.bestshots == null)
                    this.bestshots = new Dictionary<Chara, Dictionary<LevelScenePair, BestShotPair>>(
                        Enum.GetValues(typeof(Chara)).Length);
                if (!this.bestshots.ContainsKey(chara))
                    this.bestshots.Add(
                        chara, new Dictionary<LevelScenePair, BestShotPair>(SpellCards.Count));

                var key = new LevelScenePair(header.Level, header.Scene);
                if (!this.bestshots[chara].ContainsKey(key))
                    this.bestshots[chara].Add(key, new BestShotPair(outputFile.Name, header));

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
    }
}
