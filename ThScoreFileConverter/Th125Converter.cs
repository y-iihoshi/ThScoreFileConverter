using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter
{
    public class Th125Converter : ThConverter
    {
        private static readonly string[] LevelArray =
        {
            "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "ex", "sp"
        };
        private static readonly string[] LevelShortArray =
        {
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "X", "S"
        };

        private enum Chara      { Aya, Hatate };
        private enum CharaShort { A, H };

        private enum Enemy
        {
            Shizuha, Minoriko, Parsee, Hina, Kogasa, Kisume, Yamame, Nitori, Momiji, Ichirin,
            Minamitsu, Yuugi, Suika, Shou, Nazrin, Utsuho, Rin, Koishi, Satori, Tenshi,
            Iku, Suwako, Kanako, Nue, Byakuren, Reimu, Marisa, Sanae, Hatate, Aya
        }

        private static readonly Dictionary<Enemy, string> EnemyNames = new Dictionary<Enemy, string>()
        {
            { Enemy.Shizuha,    "秋 静葉"          },
            { Enemy.Minoriko,   "秋 穣子"          },
            { Enemy.Parsee,     "水橋 パルスィ"    },
            { Enemy.Hina,       "鍵山 雛"          },
            { Enemy.Kogasa,     "多々良 小傘"      },
            { Enemy.Kisume,     "キスメ"           },
            { Enemy.Yamame,     "黒谷 ヤマメ"      },
            { Enemy.Nitori,     "河城 にとり"      },
            { Enemy.Momiji,     "犬走 椛"          },
            { Enemy.Ichirin,    "雲居 一輪 & 雲山" },
            { Enemy.Minamitsu,  "村紗 水蜜"        },
            { Enemy.Yuugi,      "星熊 勇儀"        },
            { Enemy.Suika,      "伊吹 萃香"        },
            { Enemy.Shou,       "寅丸 星"          },
            { Enemy.Nazrin,     "ナズーリン"       },
            { Enemy.Utsuho,     "霊烏路 空"        },
            { Enemy.Rin,        "火焔猫 燐"        },
            { Enemy.Koishi,     "古明地 こいし"    },
            { Enemy.Satori,     "古明地 さとり"    },
            { Enemy.Tenshi,     "比那名居 天子"    },
            { Enemy.Iku,        "永江 衣玖"        },
            { Enemy.Suwako,     "洩矢 諏訪子"      },
            { Enemy.Kanako,     "八坂 神奈子"      },
            { Enemy.Nue,        "封獣 ぬえ"        },
            { Enemy.Byakuren,   "聖 白蓮"          },
            { Enemy.Reimu,      "博麗 霊夢"        },
            { Enemy.Marisa,     "霧雨 魔理沙"      },
            { Enemy.Sanae,      "東風谷 早苗"      },
            { Enemy.Hatate,     "姫海棠 はたて"    },
            { Enemy.Aya,        "射命丸 文"        }
        };

        private class LevelScenePair : Pair<int, int>
        {
            public int Level { get { return this.First; } }     // 1-origin
            public int Scene { get { return this.Second; } }    // 1-origin

            public LevelScenePair(int level, int scene) : base(level, scene) { }
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
                { new LevelScenePair(1, 1), new EnemyCardPair(Enemy.Minoriko, "") },
                { new LevelScenePair(1, 2), new EnemyCardPair(Enemy.Minoriko, "") },
                { new LevelScenePair(1, 3), new EnemyCardPair(Enemy.Shizuha,  "秋符「フォーリンブラスト」") },
                { new LevelScenePair(1, 4), new EnemyCardPair(Enemy.Minoriko, "実符「ウォームカラーハーヴェスト」") },
                { new LevelScenePair(1, 5), new EnemyCardPair(Enemy.Shizuha,  "枯道「ロストウィンドロウ」") },
                { new LevelScenePair(1, 6), new EnemyCardPair(Enemy.Minoriko, "焼芋「スイートポテトルーム」") },

                { new LevelScenePair(2, 1), new EnemyCardPair(Enemy.Parsee, "") },
                { new LevelScenePair(2, 2), new EnemyCardPair(Enemy.Hina,   "") },
                { new LevelScenePair(2, 3), new EnemyCardPair(Enemy.Parsee, "嫉妬「ジェラシーボンバー」") },
                { new LevelScenePair(2, 4), new EnemyCardPair(Enemy.Hina,   "厄野「禊川の堆積」") },
                { new LevelScenePair(2, 5), new EnemyCardPair(Enemy.Parsee, "怨み念法「積怨返し」") },
                { new LevelScenePair(2, 6), new EnemyCardPair(Enemy.Hina,   "災禍「呪いの雛人形」") },

                { new LevelScenePair(3, 1), new EnemyCardPair(Enemy.Yamame, "") },
                { new LevelScenePair(3, 2), new EnemyCardPair(Enemy.Kogasa, "傘符「一本足ピッチャー返し」") },
                { new LevelScenePair(3, 3), new EnemyCardPair(Enemy.Kisume, "釣瓶「飛んで井の中」") },
                { new LevelScenePair(3, 4), new EnemyCardPair(Enemy.Yamame, "細綱「カンダタロープ」") },
                { new LevelScenePair(3, 5), new EnemyCardPair(Enemy.Kogasa, "虹符「オーバー・ザ・レインボー」") },
                { new LevelScenePair(3, 6), new EnemyCardPair(Enemy.Kisume, "釣瓶「ウェルディストラクター」") },
                { new LevelScenePair(3, 7), new EnemyCardPair(Enemy.Yamame, "毒符「樺黄小町」") },
                { new LevelScenePair(3, 8), new EnemyCardPair(Enemy.Kogasa, "傘符「細雪の過客」") },

                { new LevelScenePair(4, 1), new EnemyCardPair(Enemy.Nitori, "") },
                { new LevelScenePair(4, 2), new EnemyCardPair(Enemy.Momiji, "") },
                { new LevelScenePair(4, 3), new EnemyCardPair(Enemy.Nitori, "水符「ウォーターカーペット」") },
                { new LevelScenePair(4, 4), new EnemyCardPair(Enemy.Momiji, "狗符「レイビーズバイト」") },
                { new LevelScenePair(4, 5), new EnemyCardPair(Enemy.Nitori, "河符「ディバイディングエッジ」") },
                { new LevelScenePair(4, 6), new EnemyCardPair(Enemy.Momiji, "山窩「エクスペリーズカナン」") },
                { new LevelScenePair(4, 7), new EnemyCardPair(Enemy.Nitori, "河童「乾燥尻子玉」") },

                { new LevelScenePair(5, 1), new EnemyCardPair(Enemy.Ichirin,   "") },
                { new LevelScenePair(5, 2), new EnemyCardPair(Enemy.Minamitsu, "") },
                { new LevelScenePair(5, 3), new EnemyCardPair(Enemy.Ichirin,   "拳骨「天空鉄槌落とし」") },
                { new LevelScenePair(5, 4), new EnemyCardPair(Enemy.Minamitsu, "錨符「幽霊船長期停泊」") },
                { new LevelScenePair(5, 5), new EnemyCardPair(Enemy.Ichirin,   "稲妻「帯電入道」") },
                { new LevelScenePair(5, 6), new EnemyCardPair(Enemy.Minamitsu, "浸水「船底のヴィーナス」") },
                { new LevelScenePair(5, 7), new EnemyCardPair(Enemy.Ichirin,   "鉄拳「入道にょき」") },
                { new LevelScenePair(5, 8), new EnemyCardPair(Enemy.Minamitsu, "「ディープシンカー」") },

                { new LevelScenePair(6, 1), new EnemyCardPair(Enemy.Yuugi, "") },
                { new LevelScenePair(6, 2), new EnemyCardPair(Enemy.Suika, "") },
                { new LevelScenePair(6, 3), new EnemyCardPair(Enemy.Yuugi, "光鬼「金剛螺旋」") },
                { new LevelScenePair(6, 4), new EnemyCardPair(Enemy.Suika, "鬼符「豆粒大の針地獄」") },
                { new LevelScenePair(6, 5), new EnemyCardPair(Enemy.Yuugi, "鬼符「鬼気狂瀾」") },
                { new LevelScenePair(6, 6), new EnemyCardPair(Enemy.Suika, "地獄「煉獄吐息」") },
                { new LevelScenePair(6, 7), new EnemyCardPair(Enemy.Yuugi, "鬼声「壊滅の咆哮」") },
                { new LevelScenePair(6, 8), new EnemyCardPair(Enemy.Suika, "鬼符「ミッシングパワー」") },

                { new LevelScenePair(7, 1), new EnemyCardPair(Enemy.Shou,   "") },
                { new LevelScenePair(7, 2), new EnemyCardPair(Enemy.Nazrin, "") },
                { new LevelScenePair(7, 3), new EnemyCardPair(Enemy.Shou,   "寅符「ハングリータイガー」") },
                { new LevelScenePair(7, 4), new EnemyCardPair(Enemy.Nazrin, "棒符「ナズーリンロッド」") },
                { new LevelScenePair(7, 5), new EnemyCardPair(Enemy.Shou,   "天符「焦土曼荼羅」") },
                { new LevelScenePair(7, 6), new EnemyCardPair(Enemy.Nazrin, "財宝「ゴールドラッシュ」") },
                { new LevelScenePair(7, 7), new EnemyCardPair(Enemy.Shou,   "宝符「黄金の震眩」") },

                { new LevelScenePair(8, 1), new EnemyCardPair(Enemy.Rin,    "") },
                { new LevelScenePair(8, 2), new EnemyCardPair(Enemy.Utsuho, "熔解「メルティングホワイト」") },
                { new LevelScenePair(8, 3), new EnemyCardPair(Enemy.Rin,    "死符「ゴーストタウン」") },
                { new LevelScenePair(8, 4), new EnemyCardPair(Enemy.Utsuho, "巨星「レッドジャイアント」") },
                { new LevelScenePair(8, 5), new EnemyCardPair(Enemy.Rin,    "「死体繁華街」") },
                { new LevelScenePair(8, 6), new EnemyCardPair(Enemy.Utsuho, "星符「巨星墜つ」") },
                { new LevelScenePair(8, 7), new EnemyCardPair(Enemy.Rin,    "酔歩「キャットランダムウォーク」") },
                { new LevelScenePair(8, 8), new EnemyCardPair(Enemy.Utsuho, "七星「セプテントリオン」") },

                { new LevelScenePair(9, 1), new EnemyCardPair(Enemy.Satori, "") },
                { new LevelScenePair(9, 2), new EnemyCardPair(Enemy.Koishi, "心符「没我の愛」") },
                { new LevelScenePair(9, 3), new EnemyCardPair(Enemy.Satori, "脳符「ブレインフィンガープリント」") },
                { new LevelScenePair(9, 4), new EnemyCardPair(Enemy.Koishi, "記憶「ＤＮＡの瑕」") },
                { new LevelScenePair(9, 5), new EnemyCardPair(Enemy.Satori, "心花「カメラシャイローズ」") },
                { new LevelScenePair(9, 6), new EnemyCardPair(Enemy.Koishi, "「胎児の夢」") },
                { new LevelScenePair(9, 7), new EnemyCardPair(Enemy.Satori, "想起「うろおぼえの金閣寺」") },
                { new LevelScenePair(9, 8), new EnemyCardPair(Enemy.Koishi, "「ローズ地獄」") },

                { new LevelScenePair(10, 1), new EnemyCardPair(Enemy.Tenshi, "気性「勇気凛々の剣」") },
                { new LevelScenePair(10, 2), new EnemyCardPair(Enemy.Iku,    "雷符「ライトニングフィッシュ」") },
                { new LevelScenePair(10, 3), new EnemyCardPair(Enemy.Tenshi, "地震「避難険路」") },
                { new LevelScenePair(10, 4), new EnemyCardPair(Enemy.Iku,    "珠符「五爪龍の珠」") },
                { new LevelScenePair(10, 5), new EnemyCardPair(Enemy.Tenshi, "要石「カナメファンネル」") },
                { new LevelScenePair(10, 6), new EnemyCardPair(Enemy.Iku,    "龍宮「タイヤヒラメダンス」") },
                { new LevelScenePair(10, 7), new EnemyCardPair(Enemy.Tenshi, "「全人類の緋想天」") },
                { new LevelScenePair(10, 8), new EnemyCardPair(Enemy.Iku,    "龍魚「龍宮の使い遊泳弾」") },

                { new LevelScenePair(11, 1), new EnemyCardPair(Enemy.Kanako, "") },
                { new LevelScenePair(11, 2), new EnemyCardPair(Enemy.Suwako, "神桜「湛えの桜吹雪」") },
                { new LevelScenePair(11, 3), new EnemyCardPair(Enemy.Kanako, "蛇符「グラウンドサーペント」") },
                { new LevelScenePair(11, 4), new EnemyCardPair(Enemy.Suwako, "姫川「プリンセスジェイドグリーン」") },
                { new LevelScenePair(11, 5), new EnemyCardPair(Enemy.Kanako, "御柱「メテオリックオンバシラ」") },
                { new LevelScenePair(11, 6), new EnemyCardPair(Enemy.Suwako, "鉄輪「ミシカルリング」") },
                { new LevelScenePair(11, 7), new EnemyCardPair(Enemy.Kanako, "儚道「御神渡りクロス」") },
                { new LevelScenePair(11, 8), new EnemyCardPair(Enemy.Suwako, "土着神「御射軍神さま」") },

                { new LevelScenePair(12, 1), new EnemyCardPair(Enemy.Byakuren, "") },
                { new LevelScenePair(12, 2), new EnemyCardPair(Enemy.Nue,      "正体不明「紫鏡」") },
                { new LevelScenePair(12, 3), new EnemyCardPair(Enemy.Byakuren, "「遊行聖」") },
                { new LevelScenePair(12, 4), new EnemyCardPair(Enemy.Nue,      "正体不明「赤マント青マント」") },
                { new LevelScenePair(12, 5), new EnemyCardPair(Enemy.Byakuren, "習合「垂迹大日如来」") },
                { new LevelScenePair(12, 6), new EnemyCardPair(Enemy.Nue,      "正体不明「厠の花子さん」") },
                { new LevelScenePair(12, 7), new EnemyCardPair(Enemy.Byakuren, "「スターソードの護法」") },
                { new LevelScenePair(12, 8), new EnemyCardPair(Enemy.Nue,      "「遊星よりの弾幕Ｘ」") },

                { new LevelScenePair(13, 1), new EnemyCardPair(Enemy.Reimu,  "お札「新聞拡張団調伏」") },
                { new LevelScenePair(13, 2), new EnemyCardPair(Enemy.Marisa, "星符「オールトクラウド」") },
                { new LevelScenePair(13, 3), new EnemyCardPair(Enemy.Sanae,  "奇跡「弘安の神風」") },
                { new LevelScenePair(13, 4), new EnemyCardPair(Enemy.Reimu,  "結界「パパラッチ撃退結界」") },
                { new LevelScenePair(13, 5), new EnemyCardPair(Enemy.Marisa, "天儀「オーレリーズソーラーシステム」") },
                { new LevelScenePair(13, 6), new EnemyCardPair(Enemy.Sanae,  "蛙符「手管の蝦蟇」") },
                { new LevelScenePair(13, 7), new EnemyCardPair(Enemy.Reimu,  "夢符「夢想亜空穴」") },
                { new LevelScenePair(13, 8), new EnemyCardPair(Enemy.Marisa, "彗星「ブレイジングスター」") },
                { new LevelScenePair(13, 9), new EnemyCardPair(Enemy.Sanae,  "妖怪退治「妖力スポイラー」") },

                { new LevelScenePair(14, 1), new EnemyCardPair(Enemy.Hatate, "") },
                { new LevelScenePair(14, 2), new EnemyCardPair(Enemy.Hatate, "取材「姫海棠はたての練習取材」") },
                { new LevelScenePair(14, 3), new EnemyCardPair(Enemy.Hatate, "連写「ラピッドショット」") },
                { new LevelScenePair(14, 4), new EnemyCardPair(Enemy.Hatate, "遠眼「天狗サイコグラフィ」") },
                { new LevelScenePair(14, 5), new EnemyCardPair(Enemy.Aya,    "") },
                { new LevelScenePair(14, 6), new EnemyCardPair(Enemy.Aya,    "取材「射命丸文の圧迫取材」") },
                { new LevelScenePair(14, 7), new EnemyCardPair(Enemy.Aya,    "望遠「キャンディッドショット」") },
                { new LevelScenePair(14, 8), new EnemyCardPair(Enemy.Aya,    "速写「ファストショット」") },
                { new LevelScenePair(14, 9), new EnemyCardPair(Enemy.Aya,    "「幻想風靡」") }
            };

        private class AllScoreData
        {
            public Header header;
            public List<Score> scores;
            public Status status;
        }

        private class Header : Utils.IBinaryReadable
        {
            public string Signature { get; private set; }
            public int EncodedAllSize { get; private set; }
            public uint Unknown1 { get; private set; }
            public uint Unknown2 { get; private set; }
            public int EncodedBodySize { get; private set; }
            public int DecodedBodySize { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.Signature = Encoding.Default.GetString(reader.ReadBytes(4));
                this.EncodedAllSize = reader.ReadInt32();
                this.Unknown1 = reader.ReadUInt32();
                this.Unknown2 = reader.ReadUInt32();
                this.EncodedBodySize = reader.ReadInt32();
                this.DecodedBodySize = reader.ReadInt32();
            }

            public void WriteTo(BinaryWriter writer)
            {
                writer.Write(this.Signature.ToCharArray());
                writer.Write(this.EncodedAllSize);
                writer.Write(this.Unknown1);
                writer.Write(this.Unknown2);
                writer.Write(this.EncodedBodySize);
                writer.Write(this.DecodedBodySize);
            }
        }

        private class Chapter : Utils.IBinaryReadable
        {
            public string Signature { get; private set; }
            public ushort Unknown { get; private set; }
            public int Size { get; private set; }
            public uint Checksum { get; private set; }

            public Chapter() { }
            public Chapter(Chapter ch)
            {
                this.Signature = ch.Signature;
                this.Unknown = ch.Unknown;
                this.Size = ch.Size;
                this.Checksum = ch.Checksum;
            }

            public virtual void ReadFrom(BinaryReader reader)
            {
                this.Signature = Encoding.Default.GetString(reader.ReadBytes(2));
                this.Unknown = reader.ReadUInt16();
                this.Size = reader.ReadInt32();
                this.Checksum = reader.ReadUInt32();
            }
        }

        private class Score : Chapter   // per scene
        {
            public LevelScenePair LevelScene { get; private set; }
            public int HighScore { get; private set; }
            public byte[] Unknown1 { get; private set; }    // .Length = 0x04
            public Chara Chara { get; private set; }        // size: 4Bytes
            public byte[] Unknown2 { get; private set; }    // .Length = 0x04
            public int TrialCount { get; private set; }
            public int FirstSuccess { get; private set; }
            public uint Unknown3 { get; private set; }      // always 0x00000000?
            public uint DateTime { get; private set; }      // UNIX time (unit: [s])
            public uint Unknown4 { get; private set; }      // always 0x00000000?
            public uint Unknown5 { get; private set; }      // checksum of the bestshot file?
            public uint Unknown6 { get; private set; }      // always 0x00000001?
            public int BestshotScore { get; private set; }
            public byte[] Unknown7 { get; private set; }    // .Length = 0x08

            public Score(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "SC")
                    throw new InvalidDataException("Signature");
                if (this.Unknown != 0x0000)
                    throw new InvalidDataException("Unknown");
                if (this.Size != 0x00000048)
                    throw new InvalidDataException("Size");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                var number = reader.ReadInt32();
                this.LevelScene = new LevelScenePair((number / 10) + 1, (number % 10) + 1);
                this.HighScore = reader.ReadInt32();
                this.Unknown1 = reader.ReadBytes(0x04);
                this.Chara = (Chara)reader.ReadInt32();
                this.Unknown2 = reader.ReadBytes(0x04);
                this.TrialCount = reader.ReadInt32();
                this.FirstSuccess = reader.ReadInt32();
                this.Unknown3 = reader.ReadUInt32();
                this.DateTime = reader.ReadUInt32();
                this.Unknown4 = reader.ReadUInt32();
                this.Unknown5 = reader.ReadUInt32();
                this.Unknown6 = reader.ReadUInt32();
                this.BestshotScore = reader.ReadInt32();
                this.Unknown7 = reader.ReadBytes(0x08);
            }
        }

        private class Status : Chapter
        {
            public byte[] LastName { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)
            public byte[] Unknown1 { get; private set; }    // .Length = 2
            public byte[] BgmFlags { get; private set; }    // .Length = 6
            public byte[] Unknown2 { get; private set; }    // .Length = 0x2E
            public int TotalPlayTime { get; private set; }  // unit: [0.01s]
            public byte[] Unknown3 { get; private set; }    // .Length = 0x0424

            public Status(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "ST")
                    throw new InvalidDataException("Signature");
                if (this.Unknown != 0x0001)
                    throw new InvalidDataException("Unknown");
                if (this.Size != 0x00000474)
                    throw new InvalidDataException("Size");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.LastName = reader.ReadBytes(10);
                this.Unknown1 = reader.ReadBytes(2);
                this.BgmFlags = reader.ReadBytes(6);
                this.Unknown2 = reader.ReadBytes(0x2E);
                this.TotalPlayTime = reader.ReadInt32();
                this.Unknown3 = reader.ReadBytes(0x424);
            }
        }

        private class BestShotHeader : Utils.IBinaryReadable
        {
            public struct BonusFields
            {
                private BitVector32 data;

                public BonusFields(int data) { this.data = new BitVector32(data); }

                public int Data { get { return data.Data; } }

                public bool Bit00        { get { return data[0x00000001]; } }
                public bool Bit01        { get { return data[0x00000002]; } }
                public bool TwoShot      { get { return data[0x00000004]; } }
                public bool NiceShot     { get { return data[0x00000008]; } }
                public bool RiskBonus    { get { return data[0x00000010]; } }
                public bool Bit05        { get { return data[0x00000020]; } }
                public bool RedShot      { get { return data[0x00000040]; } }
                public bool PurpleShot   { get { return data[0x00000080]; } }

                public bool BlueShot     { get { return data[0x00000100]; } }
                public bool CyanShot     { get { return data[0x00000200]; } }
                public bool GreenShot    { get { return data[0x00000400]; } }
                public bool YellowShot   { get { return data[0x00000800]; } }
                public bool OrangeShot   { get { return data[0x00001000]; } }
                public bool ColorfulShot { get { return data[0x00002000]; } }
                public bool RainbowShot  { get { return data[0x00004000]; } }
                public bool Bit15        { get { return data[0x00008000]; } }

                public bool SoloShot     { get { return data[0x00010000]; } }
                public bool Bit17        { get { return data[0x00020000]; } }
                public bool Bit18        { get { return data[0x00040000]; } }
                public bool Bit19        { get { return data[0x00080000]; } }
                public bool Bit20        { get { return data[0x00100000]; } }
                public bool Bit21        { get { return data[0x00200000]; } }
                public bool MacroBonus   { get { return data[0x00400000]; } }
                public bool Bit23        { get { return data[0x00800000]; } }

                public bool FrontShot    { get { return data[0x01000000]; } }
                public bool BackShot     { get { return data[0x02000000]; } }
                public bool SideShot     { get { return data[0x04000000]; } }
                public bool ClearShot    { get { return data[0x08000000]; } }
                public bool CatBonus     { get { return data[0x10000000]; } }
                public bool Bit29        { get { return data[0x20000000]; } }
                public bool Bit30        { get { return data[0x40000000]; } }
                //public bool Bit31 { get { return data[0x80000000]; } }
            }

            public string Signature { get; private set; }   // "BST2"
            public ushort Unknown1 { get; private set; }    // always 0x0405?
            public short Level { get; private set; }        // 1-origin
            public short Scene { get; private set; }        // 1-origin
            public ushort Unknown2 { get; private set; }    // 0x0100 ... Version?
            public short Width { get; private set; }
            public short Height { get; private set; }
            public uint Unknown3 { get; private set; }      // always 0x00000000?
            public short Width2 { get; private set; }       // ???
            public short Height2 { get; private set; }      // ???
            public short HalfWidth { get; private set; }    // ???
            public short HalfHeight { get; private set; }   // ???
            public uint DateTime { get; private set; }
            public uint Unknown4 { get; private set; }      // always 0x00000000?
            public float SlowRate { get; private set; }     // Really...?
            public BonusFields Fields { get; private set; }
            public int ResultScore { get; private set; }
            public int BasePoint { get; private set; }
            public byte[] Unknown7 { get; private set; }    // .Length = 0x08
            public int RiskBonus { get; private set; }
            public float BossShot { get; private set; }
            public float NiceShot { get; private set; }     // minimum = 1.20?
            public float AngleBonus { get; private set; }
            public int MacroBonus { get; private set; }
            public int FrontSideBackShot { get; private set; }  // Really...?
            public int ClearShot { get; private set; }
            public byte[] Unknown10 { get; private set; }   // .Length = 0x30
            public float Angle { get; private set; }
            public int ResultScore2 { get; private set; }   // ???
            public uint Unknown12 { get; private set; }
            public byte[] CardName { get; private set; }    // .Length = 0x50

            public void ReadFrom(BinaryReader reader)
            {
                this.Signature = new string(reader.ReadChars(4));
                if (this.Signature == "BST2")
                {
                    this.Unknown1 = reader.ReadUInt16();
                    this.Level = reader.ReadInt16();
                    this.Scene = reader.ReadInt16();
                    this.Unknown2 = reader.ReadUInt16();
                    this.Width = reader.ReadInt16();
                    this.Height = reader.ReadInt16();
                    this.Unknown3 = reader.ReadUInt32();
                    this.Width2 = reader.ReadInt16();
                    this.Height2 = reader.ReadInt16();
                    this.HalfWidth = reader.ReadInt16();
                    this.HalfHeight = reader.ReadInt16();
                    this.DateTime = reader.ReadUInt32();
                    this.Unknown4 = reader.ReadUInt32();
                    this.SlowRate = reader.ReadSingle();
                    this.Fields = new BonusFields(reader.ReadInt32());
                    this.ResultScore = reader.ReadInt32();
                    this.BasePoint = reader.ReadInt32();
                    this.Unknown7 = reader.ReadBytes(0x08);
                    this.RiskBonus = reader.ReadInt32();
                    this.BossShot = reader.ReadSingle();
                    this.NiceShot = reader.ReadSingle();
                    this.AngleBonus = reader.ReadSingle();
                    this.MacroBonus = reader.ReadInt32();
                    this.FrontSideBackShot = reader.ReadInt32();
                    this.ClearShot = reader.ReadInt32();
                    this.Unknown10 = reader.ReadBytes(0x30);
                    this.Angle = reader.ReadSingle();
                    this.ResultScore2 = reader.ReadInt32();
                    this.Unknown12 = reader.ReadUInt32();
                    this.CardName = reader.ReadBytes(0x50);
                }
            }
        }

        private AllScoreData allScoreData = null;
        private Dictionary<Chara, Dictionary<LevelScenePair, BestShotPair>> bestshots = null;

        public override string SupportedVersions
        {
            get
            {
                return "1.00a";
            }
        }

        public override bool HasBestShotConverter
        {
            get
            {
                return true;
            }
        }

        public Th125Converter() { }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th125decoded.dat", FileMode.Create, FileAccess.ReadWrite))
#else
            using (var decoded = new MemoryStream())
#endif
            {
                if (!this.Decrypt(input, decrypted))
                    return false;

                decrypted.Seek(0, SeekOrigin.Begin);
                if (!this.Extract(decrypted, decoded))
                    return false;

                decoded.Seek(0, SeekOrigin.Begin);
                if (!this.Validate(decoded))
                    return false;

                decoded.Seek(0, SeekOrigin.Begin);
                this.allScoreData = this.Read(decoded);

                return this.allScoreData != null;
            }
        }

        private bool Decrypt(Stream input, Stream output)
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

        private bool Extract(Stream input, Stream output)
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

        private bool Validate(Stream input)
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
                    if (!((chapter.Signature == "SC") && (chapter.Unknown == 0x0000)) &&
                        !((chapter.Signature == "ST") && (chapter.Unknown == 0x0001)))
                        return false;

                    Int64 sum = signature + chapter.Size;
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

        private AllScoreData Read(Stream input)
        {
            var reader = new BinaryReader(input);
            var allScoreData = new AllScoreData();
            var chapter = new Chapter();

            allScoreData.scores = new List<Score>(SpellCards.Count);
            allScoreData.header = new Header();
            allScoreData.header.ReadFrom(reader);

            try
            {
                while (true)
                {
                    chapter.ReadFrom(reader);
                    switch (chapter.Signature)
                    {
                        case "SC":
                            var score = new Score(chapter);
                            score.ReadFrom(reader);
                            allScoreData.scores.Add(score);
                            break;
                        case "ST":
                            var status = new Status(chapter);
                            status.ReadFrom(reader);
                            allScoreData.status = status;
                            break;
                        default:
                            // 12 means the total size of Signature, Unknown, Size and Checksum.
                            reader.ReadBytes(chapter.Size - 12);
                            break;
                    }
                }
            }
            catch (EndOfStreamException)
            {
                // It's OK, do nothing.
            }

            if ((allScoreData.header != null) &&
                // (allScoreData.scores.Count >= 0) &&
                (allScoreData.status != null))
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
            var pattern = string.Format(
                @"%T125SCR([{0}])([{1}])([1-9])([1-5])",
                Utils.JoinEnumNames<CharaShort>(""),
                string.Join("", LevelShortArray));
            return new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(input, new MatchEvaluator(match =>
                {
                    try
                    {
                        var chara = (Chara)Utils.ParseEnum<CharaShort>(match.Groups[1].Value, true);
                        var level = match.Groups[2].Value.ToUpper();
                        var scene = int.Parse(match.Groups[3].Value);
                        var type = int.Parse(match.Groups[4].Value);

                        var levelIndex = Array.FindIndex<string>(
                            LevelShortArray, new Predicate<string>(elem => (elem == level)));
                        var levelScene = new LevelScenePair(levelIndex + 1, scene);
                        var score = this.allScoreData.scores.Find(new Predicate<Score>(elem => (
                            (elem != null) && (elem.Chara == chara) && elem.LevelScene.Equals(levelScene))));

                        switch (type)
                        {
                            case 1:     // high score
                                return this.ToNumberString((score != null) ? score.HighScore : 0);
                            case 2:     // bestshot score
                                return this.ToNumberString((score != null) ? score.BestshotScore : 0);
                            case 3:     // num of shots
                                return this.ToNumberString((score != null) ? score.TrialCount : 0);
                            case 4:     // num of shots for the first success
                                return this.ToNumberString((score != null) ? score.FirstSuccess : 0);
                            case 5:     // date & time
                                if (score != null)
                                    return new DateTime(1970, 1, 1).AddSeconds(score.DateTime)
                                        .ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
                                else
                                    return "----/--/-- --:--:--";
                            default:    // unreachable
                                return match.ToString();
                        }
                    }
                    catch
                    {
                        return match.ToString();
                    }
                }));
        }

        // %T125SCRTL[x][y][z]
        private string ReplaceScoreTotal(string input)
        {
            var pattern = string.Format(@"%T125SCRTL([{0}])([12])([1-5])", Utils.JoinEnumNames<CharaShort>(""));
            return new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(input, new MatchEvaluator(match =>
                {
                    try
                    {
                        var chara = (Chara)Utils.ParseEnum<CharaShort>(match.Groups[1].Value, true);
                        var method = int.Parse(match.Groups[2].Value);
                        var type = int.Parse(match.Groups[3].Value);

                        var triedAndSucceeded = new Predicate<Score>(
                            score => (score.TrialCount > 0) && (score.FirstSuccess > 0));
                        var isTarget = new Predicate<Score>(score =>
                        {
                            if (score == null)
                                return false;

                            if (method == 1)
                            {
                                if (LevelArray[score.LevelScene.Level - 1] == "sp")
                                {
                                    if (chara == Chara.Aya)
                                    {
                                        if (score.LevelScene.Scene <= 4)
                                            return (score.Chara == Chara.Aya);
                                        else
                                            return (score.Chara == Chara.Hatate);
                                    }
                                    else
                                        return false;
                                }
                                else
                                    return (score.Chara == chara);
                            }
                            else
                                return (score.Chara == chara);
                        });

                        switch (type)
                        {
                            case 1:     // total score
                                return this.ToNumberString(Utils.Accumulate<Score>(
                                    this.allScoreData.scores, new Converter<Score, int>(
                                        score => (isTarget(score) && triedAndSucceeded(score))
                                            ? score.HighScore : 0)));
                            case 2:     // total of bestshot scores
                                return this.ToNumberString(Utils.Accumulate<Score>(
                                    this.allScoreData.scores, new Converter<Score, int>(
                                        score => isTarget(score) ? score.BestshotScore : 0)));
                            case 3:     // total of num of shots
                                return this.ToNumberString(Utils.Accumulate<Score>(
                                    this.allScoreData.scores, new Converter<Score, int>(
                                        score => isTarget(score) ? score.TrialCount : 0)));
                            case 4:     // total of num of shots for the first success
                                return this.ToNumberString(Utils.Accumulate<Score>(
                                    this.allScoreData.scores, new Converter<Score, int>(
                                        score => (isTarget(score) && triedAndSucceeded(score))
                                            ? score.FirstSuccess : 0)));
                            case 5:     // num of succeeded scenes
                                return Utils.CountIf<Score>(
                                    this.allScoreData.scores, new Predicate<Score>(
                                        score => (isTarget(score) && triedAndSucceeded(score)))).ToString();
                            default:    // unreachable
                                return match.ToString();
                        }
                    }
                    catch
                    {
                        return match.ToString();
                    }
                }));
        }

        // %T125CARD[x][y][z]
        private string ReplaceCard(string input)
        {
            var pattern = string.Format(@"%T125CARD([{0}])([1-9])([12])", string.Join("", LevelShortArray));
            return new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(input, new MatchEvaluator(match =>
                {
                    try
                    {
                        var level = match.Groups[1].Value.ToUpper();
                        var scene = int.Parse(match.Groups[2].Value);
                        var type = int.Parse(match.Groups[3].Value);

                        var levelIndex = Array.FindIndex<string>(
                            LevelShortArray, new Predicate<string>(elem => (elem == level)));
                        var key = new LevelScenePair(levelIndex + 1, scene);
                        var score = this.allScoreData.scores.Find(
                            new Predicate<Score>(elem => ((elem != null) && elem.LevelScene.Equals(key))));

                        switch (type)
                        {
                            case 1:     // target Name
                                return (score != null) ? EnemyNames[SpellCards[key].Enemy] : "??????????";
                            case 2:     // spell card Name
                                return (score != null) ? SpellCards[key].Card : "??????????";
                            default:    // unreachable
                                return match.ToString();
                        }
                    }
                    catch
                    {
                        return match.ToString();
                    }
                }));
        }

        // %T125TIMEPLY
        private string ReplaceTime(string input)
        {
            return new Regex(@"%T125TIMEPLY", RegexOptions.IgnoreCase)
                .Replace(input, new MatchEvaluator(match =>
                {
                    return new Time(this.allScoreData.status.TotalPlayTime * 10, false).ToLongString();
                }));
        }

        // %T125SHOT[x][y][z]
        private string ReplaceShot(string input, string outputFilePath)
        {
            var pattern = string.Format(@"%T125SHOT([{0}])([{1}])([1-9])",
                Utils.JoinEnumNames<CharaShort>(""),
                string.Join("", LevelShortArray));
            return new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(input, new MatchEvaluator(match =>
                {
                    try
                    {
                        var chara = (Chara)Utils.ParseEnum<CharaShort>(match.Groups[1].Value, true);
                        var level = match.Groups[2].Value.ToUpper();
                        var scene = int.Parse(match.Groups[3].Value);

                        var bestshots = this.bestshots.ContainsKey(chara) ? this.bestshots[chara] : null;
                        var levelIndex = Array.FindIndex<string>(
                            LevelShortArray, new Predicate<string>(elem => (elem == level)));
                        var key = new LevelScenePair(levelIndex + 1, scene);

                        if ((bestshots != null) && bestshots.ContainsKey(key))
                            return string.Format("<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" border=0>",
                                new Uri(outputFilePath)
                                    .MakeRelativeUri(new Uri(bestshots[key].Path)).OriginalString,
                                string.Format("ClearData: {0}\nSlow: {1:F6}%\nSpellName: {2}",
                                    this.ToNumberString(bestshots[key].Header.ResultScore),
                                    bestshots[key].Header.SlowRate,
                                    Encoding.Default.GetString(bestshots[key].Header.CardName).TrimEnd('\0')));
                        else
                            return "";
                    }
                    catch
                    {
                        return "";
                    }
                }));
        }

        // %T125SHOTEX[w][x][y][z]
        private string ReplaceShotEx(string input, string outputFilePath)
        {
            var pattern = string.Format(@"%T125SHOTEX([{0}])([{1}])([1-9])([1-7])",
                Utils.JoinEnumNames<CharaShort>(""),
                string.Join("", LevelShortArray));
            return new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(input, new MatchEvaluator(match =>
                {
                    try
                    {
                        var chara = (Chara)Utils.ParseEnum<CharaShort>(match.Groups[1].Value, true);
                        var level = match.Groups[2].Value.ToUpper();
                        var scene = int.Parse(match.Groups[3].Value);
                        var type = int.Parse(match.Groups[4].Value);

                        var bestshots = this.bestshots.ContainsKey(chara) ? this.bestshots[chara] : null;
                        var levelIndex = Array.FindIndex<string>(
                            LevelShortArray, new Predicate<string>(elem => (elem == level)));
                        var key = new LevelScenePair(levelIndex + 1, scene);

                        if ((bestshots != null) && bestshots.ContainsKey(key))
                            switch (type)
                            {
                                case 1:     // relative path to the bestshot file
                                    return new Uri(outputFilePath)
                                        .MakeRelativeUri(new Uri(bestshots[key].Path)).OriginalString;
                                case 2:     // width
                                    return bestshots[key].Header.Width.ToString();
                                case 3:     // height
                                    return bestshots[key].Header.Height.ToString();
                                case 4:     // score
                                    return this.ToNumberString(bestshots[key].Header.ResultScore);
                                case 5:     // slow rate
                                    return bestshots[key].Header.SlowRate.ToString("F6") + "%";
                                case 6:     // date & time
                                    {
                                        var score = this.allScoreData.scores.Find(new Predicate<Score>(
                                            elem => ((elem != null) && (elem.Chara == chara) && elem.LevelScene.Equals(key))));
                                        if (score != null)
                                            return new DateTime(1970, 1, 1).AddSeconds(score.DateTime)
                                                .ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
                                        else
                                            return "----/--/-- --:--:--";
                                    }
                                case 7:     // detail info
                                    {
                                        var header = bestshots[key].Header;
                                        var info = new List<string>();
                                        info.Add(string.Format("Base Point    {0,9}",
                                            this.ToNumberString<int>(header.BasePoint)));
                                        if (header.Fields.ClearShot)
                                            info.Add(string.Format("Clear Shot!   {0,9}",
                                                string.Format("+ {0}", header.ClearShot)));
                                        if (header.Fields.SoloShot)
                                            info.Add(string.Format("Solo Shot     {0,9}", "+ 100"));
                                        if (header.Fields.RedShot)
                                            info.Add(string.Format("Red Shot      {0,9}", "+ 300"));
                                        if (header.Fields.PurpleShot)
                                            info.Add(string.Format("Purple Shot   {0,9}", "+ 300"));
                                        if (header.Fields.BlueShot)
                                            info.Add(string.Format("Blue Shot     {0,9}", "+ 300"));
                                        if (header.Fields.CyanShot)
                                            info.Add(string.Format("Cyan Shot     {0,9}", "+ 300"));
                                        if (header.Fields.GreenShot)
                                            info.Add(string.Format("Green Shot    {0,9}", "+ 300"));
                                        if (header.Fields.YellowShot)
                                            info.Add(string.Format("Yellow Shot   {0,9}", "+ 300"));
                                        if (header.Fields.OrangeShot)
                                            info.Add(string.Format("Orange Shot   {0,9}", "+ 300"));
                                        if (header.Fields.ColorfulShot)
                                            info.Add(string.Format("Colorful Shot {0,9}", "+ 900"));
                                        if (header.Fields.RainbowShot)
                                            info.Add(string.Format("Rainbow Shot  {0,9}",
                                                string.Format("+ {0}", this.ToNumberString<int>(2100))));
                                        if (header.Fields.RiskBonus)
                                            info.Add(string.Format("Risk Bonus    {0,9}",
                                                string.Format("+ {0}",
                                                    this.ToNumberString<int>(header.RiskBonus))));
                                        if (header.Fields.MacroBonus)
                                            info.Add(string.Format("Macro Bonus   {0,9}",
                                                string.Format("+ {0}",
                                                    this.ToNumberString<int>(header.MacroBonus))));
                                        if (header.Fields.FrontShot)
                                            info.Add(string.Format("Front Shot    {0,9}",
                                                string.Format("+ {0}", header.FrontSideBackShot)));
                                        if (header.Fields.SideShot)
                                            info.Add(string.Format("Side Shot     {0,9}",
                                                string.Format("+ {0}", header.FrontSideBackShot)));
                                        if (header.Fields.BackShot)
                                            info.Add(string.Format("Back Shot     {0,9}",
                                                string.Format("+ {0}", header.FrontSideBackShot)));
                                        if (header.Fields.CatBonus)
                                            info.Add(string.Format("Cat Bonus     {0,9}", "+ 666"));
                                        info.Add("");
                                        info.Add(string.Format("Boss Shot!    {0,9}",
                                            string.Format("* {0:F2}", header.BossShot)));
                                        if (header.Fields.TwoShot)
                                            info.Add(string.Format("Two Shot!     {0,9}", "* 1.50"));
                                        if (header.Fields.NiceShot)
                                            info.Add(string.Format("Nice Shot!    {0,9}",
                                                string.Format("* {0:F2}", header.NiceShot)));
                                        info.Add(string.Format("Angle Bonus   {0,9}",
                                            string.Format("* {0:F2}", header.AngleBonus)));
                                        info.Add("");
                                        info.Add(string.Format("Result Score  {0,9}",
                                            this.ToNumberString<int>(header.ResultScore)));
                                        return string.Join("\r\n", info.ToArray());
                                    }
                                default:    // unreachable
                                    return match.ToString();
                            }
                        else
                            switch (type)
                            {
                                case 1: return "";
                                case 2: return "0";
                                case 3: return "0";
                                case 4: return "--------";
                                case 5: return "-----%";
                                case 6: return "----/--/-- --:--:--";
                                case 7: return "";
                                default: return match.ToString();
                            }
                    }
                    catch
                    {
                        return match.ToString();
                    }
                }));
        }

        protected override string[] FilterBestShotFiles(string[] files)
        {
            var bestshots = new List<string>();
            var pattern = string.Format(@"bs2?_({0})_[1-9].dat", string.Join("|", LevelArray));
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            foreach (var file in files)
                if (regex.IsMatch(Path.GetFileName(file)))
                    bestshots.Add(file);

            return bestshots.ToArray();
        }

        protected override void ConvertBestShot(Stream input, Stream output)
        {
            using (var decoded = new MemoryStream())
            {
                var outputFile = output as FileStream;
                var chara = Path.GetFileName(outputFile.Name).StartsWith("bs2_") ? Chara.Hatate : Chara.Aya;

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
                var source = decoded.ToArray();
                var bitmap = new Bitmap(header.Width, header.Height, PixelFormat.Format32bppArgb);
                var bitmapData = bitmap.LockBits(
                    new Rectangle(0, 0, header.Width, header.Height),
                    ImageLockMode.WriteOnly, bitmap.PixelFormat);
                var destination = bitmapData.Scan0;
                Marshal.Copy(source, 0, destination, source.Length);
                bitmap.UnlockBits(bitmapData);

                bitmap.Save(output, ImageFormat.Png);
                output.Flush();
                output.SetLength(output.Position);
            }
        }
    }
}
