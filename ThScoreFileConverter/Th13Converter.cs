//-----------------------------------------------------------------------
// <copyright file="Th13Converter.cs" company="None">
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
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using CardInfo = SpellCardInfo<Th13Converter.StagePractice, Th13Converter.LevelPractice>;

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.OrderingRules",
        "SA1201:ElementsMustAppearInTheCorrectOrder",
        Justification = "Reviewed.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.SpacingRules",
        "SA1025:CodeMustNotContainMultipleWhitespaceInARow",
        Justification = "Reviewed.")]
    internal class Th13Converter : ThConverter
    {
        public enum Level
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra
        }
        public enum LevelWithTotal
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("T")] Total
        }
        public enum LevelPractice
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("D", LongName = "Over Drive")] OverDrive
        }
        public enum LevelPracticeWithTotal
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("D", LongName = "Over Drive")] OverDrive,
            [EnumAltName("T")] Total
        }

        public enum Chara
        {
            [EnumAltName("RM")] Reimu,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("SN")] Sanae,
            [EnumAltName("YM")] Youmu
        }
        public enum CharaWithTotal
        {
            [EnumAltName("RM")] Reimu,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("SN")] Sanae,
            [EnumAltName("YM")] Youmu,
            [EnumAltName("TL")] Total
        }

        public enum Stage
        {
            [EnumAltName("1")] St1,
            [EnumAltName("2")] St2,
            [EnumAltName("3")] St3,
            [EnumAltName("4")] St4,
            [EnumAltName("5")] St5,
            [EnumAltName("6")] St6,
            [EnumAltName("X")] Extra
        }
        public enum StageWithTotal
        {
            [EnumAltName("1")] St1,
            [EnumAltName("2")] St2,
            [EnumAltName("3")] St3,
            [EnumAltName("4")] St4,
            [EnumAltName("5")] St5,
            [EnumAltName("6")] St6,
            [EnumAltName("X")] Extra,
            [EnumAltName("0")] Total
        }
        public enum StagePractice
        {
            [EnumAltName("1")] St1,
            [EnumAltName("2")] St2,
            [EnumAltName("3")] St3,
            [EnumAltName("4")] St4,
            [EnumAltName("5")] St5,
            [EnumAltName("6")] St6,
            [EnumAltName("X")] Extra,
            [EnumAltName("D", LongName = "Over Drive")] OverDrive
        }

        public enum StageProgress
        {
            [EnumAltName("-------")]     None,
            [EnumAltName("Stage 1")]     St1,
            [EnumAltName("Stage 2")]     St2,
            [EnumAltName("Stage 3")]     St3,
            [EnumAltName("Stage 4")]     St4,
            [EnumAltName("Stage 5")]     St5,
            [EnumAltName("Stage 6")]     St6,
            [EnumAltName("Extra Stage")] Extra,
            [EnumAltName("All Clear")]   Clear,
            [EnumAltName("Extra Clear")] ExtraClear
        }

        private class LevelStagePair : Pair<LevelPractice, StagePractice>
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public LevelPractice Level { get { return this.First; } }

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public StagePractice Stage { get { return this.Second; } }

            public LevelStagePair(LevelPractice level, StagePractice stage) : base(level, stage) { }
        }

        private class AllScoreData
        {
            public Header Header { get; set; }
            public Dictionary<CharaWithTotal, ClearData> ClearData { get; set; }
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
                this.Signature = new string(reader.ReadChars(4));
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
            public uint Checksum { get; private set; }
            public int Size { get; private set; }

            public Chapter() { }
            public Chapter(Chapter ch)
            {
                this.Signature = ch.Signature;
                this.Version = ch.Version;
                this.Checksum = ch.Checksum;
                this.Size = ch.Size;
            }

            public virtual void ReadFrom(BinaryReader reader)
            {
                this.Signature = Encoding.Default.GetString(reader.ReadBytes(2));
                this.Version = reader.ReadUInt16();
                this.Checksum = reader.ReadUInt32();
                this.Size = reader.ReadInt32();
            }
        }

        private class ClearData : Chapter   // per character
        {
            public CharaWithTotal Chara { get; private set; }   // size: 4Bytes
            public Dictionary<LevelPracticeWithTotal, ScoreData[]> Rankings { get; private set; }
            public int TotalPlayCount { get; private set; }
            public int PlayTime { get; private set; }           // = seconds * 60fps
            public Dictionary<LevelPracticeWithTotal, int> ClearCounts { get; private set; }
            public Dictionary<LevelPracticeWithTotal, int> ClearFlags { get; private set; }     // Really...?
            public Dictionary<LevelStagePair, Practice> Practices { get; private set; }
            public Dictionary<int, SpellCard> Cards { get; private set; }

            public ClearData(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "CR")
                    throw new InvalidDataException("Signature");
                if (this.Version != 0x0001)
                    throw new InvalidDataException("Version");
                if (this.Size != 0x000056DC)
                    throw new InvalidDataException("Size");

                var numLevels = Enum.GetValues(typeof(LevelPracticeWithTotal)).Length;
                this.Rankings = new Dictionary<LevelPracticeWithTotal, ScoreData[]>(numLevels);
                this.ClearCounts = new Dictionary<LevelPracticeWithTotal, int>(numLevels);
                this.ClearFlags = new Dictionary<LevelPracticeWithTotal, int>(numLevels);
                this.Practices = new Dictionary<LevelStagePair, Practice>();
                this.Cards = new Dictionary<int, SpellCard>(CardTable.Count);
            }

            public override void ReadFrom(BinaryReader reader)
            {
                var levelsPracticeWithTotal = Utils.GetEnumerator<LevelPracticeWithTotal>();
                var stages = Utils.GetEnumerator<StagePractice>();
                this.Chara = (CharaWithTotal)reader.ReadInt32();
                foreach (var level in levelsPracticeWithTotal)
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
                this.TotalPlayCount = reader.ReadInt32();
                this.PlayTime = reader.ReadInt32();
                foreach (var level in levelsPracticeWithTotal)
                {
                    var clearCount = reader.ReadInt32();
                    if (!this.ClearCounts.ContainsKey(level))
                        this.ClearCounts.Add(level, clearCount);
                }
                foreach (var level in levelsPracticeWithTotal)
                {
                    var clearFlag = reader.ReadInt32();
                    if (!this.ClearFlags.ContainsKey(level))
                        this.ClearFlags.Add(level, clearFlag);
                }
                foreach (var level in Utils.GetEnumerator<LevelPractice>())
                    foreach (var stage in stages)
                    {
                        var practice = new Practice();
                        practice.ReadFrom(reader);
                        var key = new LevelStagePair(level, stage);
                        if (!this.Practices.ContainsKey(key))
                            this.Practices.Add(key, practice);
                    }
                for (var number = 0; number < CardTable.Count; number++)
                {
                    var card = new SpellCard();
                    card.ReadFrom(reader);
                    if (!this.Cards.ContainsKey(card.Number))
                        this.Cards.Add(card.Number, card);
                }
            }
        }

        private class Status : Chapter
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte[] LastName { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte[] BgmFlags { get; private set; }    // .Length = 17

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public int TotalPlayTime { get; private set; }  // unit: 10ms

            public Status(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "ST")
                    throw new InvalidDataException("Signature");
                if (this.Version != 0x0001)
                    throw new InvalidDataException("Version");
                if (this.Size != 0x0000042C)
                    throw new InvalidDataException("Size");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.LastName = reader.ReadBytes(10);
                reader.ReadBytes(0x10);
                this.BgmFlags = reader.ReadBytes(17);
                reader.ReadBytes(0x11);
                this.TotalPlayTime = reader.ReadInt32();
                reader.ReadBytes(0x03E0);
            }
        }

        private class ScoreData : IBinaryReadable
        {
            public uint Score { get; private set; }         // * 10
            public StageProgress StageProgress { get; private set; }    // size: 1Byte
            public byte ContinueCount { get; private set; }
            public byte[] Name { get; private set; }        // .Length = 10 (The last 2 bytes are always 0x00 ?)
            public uint DateTime { get; private set; }      // UNIX time (unit: [s])
            public float SlowRate { get; private set; }     // really...?

            public void ReadFrom(BinaryReader reader)
            {
                this.Score = reader.ReadUInt32();
                this.StageProgress = (StageProgress)reader.ReadByte();
                this.ContinueCount = reader.ReadByte();
                this.Name = reader.ReadBytes(10);
                this.DateTime = reader.ReadUInt32();
                this.SlowRate = reader.ReadSingle();
                reader.ReadUInt32();
            }
        }

        private class SpellCard : IBinaryReadable
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte[] Name { get; private set; }            // .Length = 0x80

            public int ClearCount { get; private set; }
            public int PracticeClearCount { get; private set; }
            public int TrialCount { get; private set; }
            public int PracticeTrialCount { get; private set; }
            public int Number { get; private set; }             // 1-based
            public LevelPractice Level { get; private set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public int PracticeScore { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.Name = reader.ReadBytes(0x80);
                this.ClearCount = reader.ReadInt32();
                this.PracticeClearCount = reader.ReadInt32();
                this.TrialCount = reader.ReadInt32();
                this.PracticeTrialCount = reader.ReadInt32();
                this.Number = reader.ReadInt32() + 1;
                this.Level = (LevelPractice)reader.ReadInt32();
                this.PracticeScore = reader.ReadInt32();
            }

            public bool HasTried()
            {
                return (this.TrialCount > 0) || (this.PracticeTrialCount > 0);
            }
        }

        private class Practice : IBinaryReadable
        {
            public uint Score { get; private set; }         // * 10

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte ClearFlag { get; private set; }     // 0x00: Not clear, 0x01: Cleared

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte EnableFlag { get; private set; }    // 0x00: Disable, 0x01: Enable

            public void ReadFrom(BinaryReader reader)
            {
                this.Score = reader.ReadUInt32();
                this.ClearFlag = reader.ReadByte();
                this.EnableFlag = reader.ReadByte();
                reader.ReadUInt16();    // always 0x0000?
            }
        }

        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get { return "1.00c"; }
        }

        private static readonly Dictionary<int, CardInfo> CardTable;

        private static readonly string LevelPattern;
        private static readonly string LevelWithTotalPattern;
        private static readonly string LevelPracticeWithTotalPattern;
        private static readonly string CharaPattern;
        private static readonly string CharaWithTotalPattern;
        private static readonly string StagePattern;
        private static readonly string StageWithTotalPattern;

        private static readonly Func<string, Level> ToLevel;
        private static readonly Func<string, LevelWithTotal> ToLevelWithTotal;
        private static readonly Func<string, LevelPracticeWithTotal> ToLevelPracticeWithTotal;
        private static readonly Func<string, Chara> ToChara;
        private static readonly Func<string, CharaWithTotal> ToCharaWithTotal;
        private static readonly Func<string, Stage> ToStage;
        private static readonly Func<string, StageWithTotal> ToStageWithTotal;

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Performance",
            "CA1810:InitializeReferenceTypeStaticFieldsInline",
            Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1008:OpeningParenthesisMustBeSpacedCorrectly",
            Justification = "Reviewed.")]
        static Th13Converter()
        {
            var cardList = new List<CardInfo>()
            {
                new CardInfo(  1, "符牒「死蝶の舞」",                     StagePractice.St1,       LevelPractice.Easy),
                new CardInfo(  2, "符牒「死蝶の舞」",                     StagePractice.St1,       LevelPractice.Normal),
                new CardInfo(  3, "符牒「死蝶の舞　- 桜花 -」",           StagePractice.St1,       LevelPractice.Hard),
                new CardInfo(  4, "符牒「死蝶の舞　- 桜花 -」",           StagePractice.St1,       LevelPractice.Lunatic),
                new CardInfo(  5, "幽蝶「ゴーストスポット」",             StagePractice.St1,       LevelPractice.Easy),
                new CardInfo(  6, "幽蝶「ゴーストスポット」",             StagePractice.St1,       LevelPractice.Normal),
                new CardInfo(  7, "幽蝶「ゴーストスポット　- 桜花 -」",   StagePractice.St1,       LevelPractice.Hard),
                new CardInfo(  8, "幽蝶「ゴーストスポット　- 桜花 -」",   StagePractice.St1,       LevelPractice.Lunatic),
                new CardInfo(  9, "冥符「常夜桜」",                       StagePractice.St1,       LevelPractice.Easy),
                new CardInfo( 10, "冥符「常夜桜」",                       StagePractice.St1,       LevelPractice.Normal),
                new CardInfo( 11, "冥符「常夜桜」",                       StagePractice.St1,       LevelPractice.Hard),
                new CardInfo( 12, "冥符「常夜桜」",                       StagePractice.St1,       LevelPractice.Lunatic),
                new CardInfo( 13, "桜符「西行桜吹雪」",                   StagePractice.St1,       LevelPractice.Hard),
                new CardInfo( 14, "桜符「西行桜吹雪」",                   StagePractice.St1,       LevelPractice.Lunatic),
                new CardInfo( 15, "響符「マウンテンエコー」",             StagePractice.St2,       LevelPractice.Easy),
                new CardInfo( 16, "響符「マウンテンエコー」",             StagePractice.St2,       LevelPractice.Normal),
                new CardInfo( 17, "響符「マウンテンエコースクランブル」", StagePractice.St2,       LevelPractice.Hard),
                new CardInfo( 18, "響符「マウンテンエコースクランブル」", StagePractice.St2,       LevelPractice.Lunatic),
                new CardInfo( 19, "響符「パワーレゾナンス」",             StagePractice.St2,       LevelPractice.Easy),
                new CardInfo( 20, "響符「パワーレゾナンス」",             StagePractice.St2,       LevelPractice.Normal),
                new CardInfo( 21, "響符「パワーレゾナンス」",             StagePractice.St2,       LevelPractice.Hard),
                new CardInfo( 22, "響符「パワーレゾナンス」",             StagePractice.St2,       LevelPractice.Lunatic),
                new CardInfo( 23, "山彦「ロングレンジエコー」",           StagePractice.St2,       LevelPractice.Easy),
                new CardInfo( 24, "山彦「ロングレンジエコー」",           StagePractice.St2,       LevelPractice.Normal),
                new CardInfo( 25, "山彦「アンプリファイエコー」",         StagePractice.St2,       LevelPractice.Hard),
                new CardInfo( 26, "山彦「アンプリファイエコー」",         StagePractice.St2,       LevelPractice.Lunatic),
                new CardInfo( 27, "大声「チャージドクライ」",             StagePractice.St2,       LevelPractice.Easy),
                new CardInfo( 28, "大声「チャージドクライ」",             StagePractice.St2,       LevelPractice.Normal),
                new CardInfo( 29, "大声「チャージドヤッホー」",           StagePractice.St2,       LevelPractice.Hard),
                new CardInfo( 30, "大声「チャージドヤッホー」",           StagePractice.St2,       LevelPractice.Lunatic),
                new CardInfo( 31, "虹符「アンブレラサイクロン」",         StagePractice.St3,       LevelPractice.Hard),
                new CardInfo( 32, "虹符「アンブレラサイクロン」",         StagePractice.St3,       LevelPractice.Lunatic),
                new CardInfo( 33, "回復「ヒールバイデザイア」",           StagePractice.St3,       LevelPractice.Easy),
                new CardInfo( 34, "回復「ヒールバイデザイア」",           StagePractice.St3,       LevelPractice.Normal),
                new CardInfo( 35, "回復「ヒールバイデザイア」",           StagePractice.St3,       LevelPractice.Hard),
                new CardInfo( 36, "回復「ヒールバイデザイア」",           StagePractice.St3,       LevelPractice.Lunatic),
                new CardInfo( 37, "毒爪「ポイズンレイズ」",               StagePractice.St3,       LevelPractice.Easy),
                new CardInfo( 38, "毒爪「ポイズンレイズ」",               StagePractice.St3,       LevelPractice.Normal),
                new CardInfo( 39, "毒爪「ポイズンマーダー」",             StagePractice.St3,       LevelPractice.Hard),
                new CardInfo( 40, "毒爪「ポイズンマーダー」",             StagePractice.St3,       LevelPractice.Lunatic),
                new CardInfo( 41, "欲符「稼欲霊招来」",                   StagePractice.St3,       LevelPractice.Easy),
                new CardInfo( 42, "欲符「稼欲霊招来」",                   StagePractice.St3,       LevelPractice.Normal),
                new CardInfo( 43, "欲霊「スコアデザイアイーター」",       StagePractice.St3,       LevelPractice.Hard),
                new CardInfo( 44, "欲霊「スコアデザイアイーター」",       StagePractice.St3,       LevelPractice.Lunatic),
                new CardInfo( 45, "邪符「ヤンシャオグイ」",               StagePractice.St4,       LevelPractice.Normal),
                new CardInfo( 46, "邪符「グーフンイエグイ」",             StagePractice.St4,       LevelPractice.Hard),
                new CardInfo( 47, "邪符「グーフンイエグイ」",             StagePractice.St4,       LevelPractice.Lunatic),
                new CardInfo( 48, "入魔「ゾウフォルゥモォ」",             StagePractice.St4,       LevelPractice.Easy),
                new CardInfo( 49, "入魔「ゾウフォルゥモォ」",             StagePractice.St4,       LevelPractice.Normal),
                new CardInfo( 50, "入魔「ゾウフォルゥモォ」",             StagePractice.St4,       LevelPractice.Hard),
                new CardInfo( 51, "入魔「ゾウフォルゥモォ」",             StagePractice.St4,       LevelPractice.Lunatic),
                new CardInfo( 52, "降霊「死人タンキー」",                 StagePractice.St4,       LevelPractice.Easy),
                new CardInfo( 53, "降霊「死人タンキー」",                 StagePractice.St4,       LevelPractice.Normal),
                new CardInfo( 54, "通霊「トンリン芳香」",                 StagePractice.St4,       LevelPractice.Hard),
                new CardInfo( 55, "通霊「トンリン芳香」",                 StagePractice.St4,       LevelPractice.Lunatic),
                new CardInfo( 56, "道符「タオ胎動」",                     StagePractice.St4,       LevelPractice.Easy),
                new CardInfo( 57, "道符「タオ胎動」",                     StagePractice.St4,       LevelPractice.Normal),
                new CardInfo( 58, "道符「タオ胎動」",                     StagePractice.St4,       LevelPractice.Hard),
                new CardInfo( 59, "道符「タオ胎動」",                     StagePractice.St4,       LevelPractice.Lunatic),
                new CardInfo( 60, "雷矢「ガゴウジサイクロン」",           StagePractice.St5,       LevelPractice.Normal),
                new CardInfo( 61, "雷矢「ガゴウジサイクロン」",           StagePractice.St5,       LevelPractice.Hard),
                new CardInfo( 62, "雷矢「ガゴウジトルネード」",           StagePractice.St5,       LevelPractice.Lunatic),
                new CardInfo( 63, "天符「雨の磐舟」",                     StagePractice.St5,       LevelPractice.Easy),
                new CardInfo( 64, "天符「雨の磐舟」",                     StagePractice.St5,       LevelPractice.Normal),
                new CardInfo( 65, "天符「天の磐舟よ天へ昇れ」",           StagePractice.St5,       LevelPractice.Hard),
                new CardInfo( 66, "天符「天の磐舟よ天へ昇れ」",           StagePractice.St5,       LevelPractice.Lunatic),
                new CardInfo( 67, "投皿「物部の八十平瓮」",               StagePractice.St5,       LevelPractice.Easy),
                new CardInfo( 68, "投皿「物部の八十平瓮」",               StagePractice.St5,       LevelPractice.Normal),
                new CardInfo( 69, "投皿「物部の八十平瓮」",               StagePractice.St5,       LevelPractice.Hard),
                new CardInfo( 70, "投皿「物部の八十平瓮」",               StagePractice.St5,       LevelPractice.Lunatic),
                new CardInfo( 71, "炎符「廃仏の炎風」",                   StagePractice.St5,       LevelPractice.Easy),
                new CardInfo( 72, "炎符「廃仏の炎風」",                   StagePractice.St5,       LevelPractice.Normal),
                new CardInfo( 73, "炎符「桜井寺炎上」",                   StagePractice.St5,       LevelPractice.Hard),
                new CardInfo( 74, "炎符「桜井寺炎上」",                   StagePractice.St5,       LevelPractice.Lunatic),
                new CardInfo( 75, "聖童女「大物忌正餐」",                 StagePractice.St5,       LevelPractice.Easy),
                new CardInfo( 76, "聖童女「大物忌正餐」",                 StagePractice.St5,       LevelPractice.Normal),
                new CardInfo( 77, "聖童女「大物忌正餐」",                 StagePractice.St5,       LevelPractice.Hard),
                new CardInfo( 78, "聖童女「大物忌正餐」",                 StagePractice.St5,       LevelPractice.Lunatic),
                new CardInfo( 79, "名誉「十二階の色彩」",                 StagePractice.St6,       LevelPractice.Easy),
                new CardInfo( 80, "名誉「十二階の色彩」",                 StagePractice.St6,       LevelPractice.Normal),
                new CardInfo( 81, "名誉「十二階の冠位」",                 StagePractice.St6,       LevelPractice.Hard),
                new CardInfo( 82, "名誉「十二階の冠位」",                 StagePractice.St6,       LevelPractice.Lunatic),
                new CardInfo( 83, "仙符「日出ずる処の道士」",             StagePractice.St6,       LevelPractice.Easy),
                new CardInfo( 84, "仙符「日出ずる処の道士」",             StagePractice.St6,       LevelPractice.Normal),
                new CardInfo( 85, "仙符「日出ずる処の天子」",             StagePractice.St6,       LevelPractice.Hard),
                new CardInfo( 86, "仙符「日出ずる処の天子」",             StagePractice.St6,       LevelPractice.Lunatic),
                new CardInfo( 87, "召喚「豪族乱舞」",                     StagePractice.St6,       LevelPractice.Easy),
                new CardInfo( 88, "召喚「豪族乱舞」",                     StagePractice.St6,       LevelPractice.Normal),
                new CardInfo( 89, "召喚「豪族乱舞」",                     StagePractice.St6,       LevelPractice.Hard),
                new CardInfo( 90, "召喚「豪族乱舞」",                     StagePractice.St6,       LevelPractice.Lunatic),
                new CardInfo( 91, "秘宝「斑鳩寺の天球儀」",               StagePractice.St6,       LevelPractice.Easy),
                new CardInfo( 92, "秘宝「斑鳩寺の天球儀」",               StagePractice.St6,       LevelPractice.Normal),
                new CardInfo( 93, "秘宝「斑鳩寺の天球儀」",               StagePractice.St6,       LevelPractice.Hard),
                new CardInfo( 94, "秘宝「聖徳太子のオーパーツ」",         StagePractice.St6,       LevelPractice.Lunatic),
                new CardInfo( 95, "光符「救世観音の光後光」",             StagePractice.St6,       LevelPractice.Easy),
                new CardInfo( 96, "光符「救世観音の光後光」",             StagePractice.St6,       LevelPractice.Normal),
                new CardInfo( 97, "光符「グセフラッシュ」",               StagePractice.St6,       LevelPractice.Hard),
                new CardInfo( 98, "光符「グセフラッシュ」",               StagePractice.St6,       LevelPractice.Lunatic),
                new CardInfo( 99, "眼光「十七条のレーザー」",             StagePractice.St6,       LevelPractice.Easy),
                new CardInfo(100, "眼光「十七条のレーザー」",             StagePractice.St6,       LevelPractice.Normal),
                new CardInfo(101, "神光「逆らう事なきを宗とせよ」",       StagePractice.St6,       LevelPractice.Hard),
                new CardInfo(102, "神光「逆らう事なきを宗とせよ」",       StagePractice.St6,       LevelPractice.Lunatic),
                new CardInfo(103, "「星降る神霊廟」",                     StagePractice.St6,       LevelPractice.Easy),
                new CardInfo(104, "「星降る神霊廟」",                     StagePractice.St6,       LevelPractice.Normal),
                new CardInfo(105, "「生まれたての神霊」",                 StagePractice.St6,       LevelPractice.Hard),
                new CardInfo(106, "「生まれたての神霊」",                 StagePractice.St6,       LevelPractice.Lunatic),
                new CardInfo(107, "アンノウン「軌道不明の鬼火」",         StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(108, "アンノウン「姿態不明の空魚」",         StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(109, "アンノウン「原理不明の妖怪玉」",       StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(110, "壱番勝負「霊長化弾幕変化」",           StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(111, "弐番勝負「肉食化弾幕変化」",           StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(112, "参番勝負「延羽化弾幕変化」",           StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(113, "四番勝負「両生化弾幕変化」",           StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(114, "伍番勝負「鳥獣戯画」",                 StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(115, "六番勝負「狸の化け学校」",             StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(116, "七番勝負「野生の離島」",               StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(117, "変化「まぬけ巫女の偽調伏」",           StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(118, "「マミゾウ化弾幕十変化」",             StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(119, "狢符「満月のポンポコリン」",           StagePractice.Extra,     LevelPractice.Extra),
                new CardInfo(120, "桜符「桜吹雪地獄」",                   StagePractice.OverDrive, LevelPractice.OverDrive),
                new CardInfo(121, "山彦「ヤマビコの本領発揮エコー」",     StagePractice.OverDrive, LevelPractice.OverDrive),
                new CardInfo(122, "毒爪「死なない殺人鬼」",               StagePractice.OverDrive, LevelPractice.OverDrive),
                new CardInfo(123, "道符「ＴＡＯ胎動　～道～」",           StagePractice.OverDrive, LevelPractice.OverDrive),
                new CardInfo(124, "怨霊「入鹿の雷」",                     StagePractice.OverDrive, LevelPractice.OverDrive),
                new CardInfo(125, "聖童女「太陽神の贄」",                 StagePractice.OverDrive, LevelPractice.OverDrive),
                new CardInfo(126, "「神霊大宇宙」",                       StagePractice.OverDrive, LevelPractice.OverDrive),
                new CardInfo(127, "「ワイルドカーペット」",               StagePractice.OverDrive, LevelPractice.OverDrive)
            };
            CardTable = cardList.ToDictionary(card => card.Number);

            var levels = Utils.GetEnumerator<Level>();
            var levelsWithTotal = Utils.GetEnumerator<LevelWithTotal>();
            var levelsPracticeWithTotal = Utils.GetEnumerator<LevelPracticeWithTotal>();
            var charas = Utils.GetEnumerator<Chara>();
            var charasWithTotal = Utils.GetEnumerator<CharaWithTotal>();
            var stages = Utils.GetEnumerator<Stage>();
            var stagesWithTotal = Utils.GetEnumerator<StageWithTotal>();

            LevelPattern = string.Join(
                string.Empty, levels.Select(lv => lv.ToShortName()).ToArray());
            LevelWithTotalPattern = string.Join(
                string.Empty, levelsWithTotal.Select(lv => lv.ToShortName()).ToArray());
            LevelPracticeWithTotalPattern = string.Join(
                string.Empty, levelsPracticeWithTotal.Select(lv => lv.ToShortName()).ToArray());
            CharaPattern = string.Join(
                "|", charas.Select(ch => ch.ToShortName()).ToArray());
            CharaWithTotalPattern = string.Join(
                "|", charasWithTotal.Select(ch => ch.ToShortName()).ToArray());
            StagePattern = string.Join(
                string.Empty, stages.Select(st => st.ToShortName()).ToArray());
            StageWithTotalPattern = string.Join(
                string.Empty, stagesWithTotal.Select(st => st.ToShortName()).ToArray());

            var comparisonType = StringComparison.OrdinalIgnoreCase;

            ToLevel = (shortName =>
                levels.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToLevelWithTotal = (shortName =>
                levelsWithTotal.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToLevelPracticeWithTotal = (shortName =>
                levelsPracticeWithTotal.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToChara = (shortName =>
                charas.First(ch => ch.ToShortName().Equals(shortName, comparisonType)));
            ToCharaWithTotal = (shortName =>
                charasWithTotal.First(ch => ch.ToShortName().Equals(shortName, comparisonType)));
            ToStage = (shortName =>
                stages.First(st => st.ToShortName().Equals(shortName, comparisonType)));
            ToStageWithTotal = (shortName =>
                stagesWithTotal.First(st => st.ToShortName().Equals(shortName, comparisonType)));
        }

        public Th13Converter()
        {
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th13decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
            if (header.Signature != "TH31")
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
                    chapter.ReadFrom(reader);

                    if (!((chapter.Signature == "CR") && (chapter.Version == 0x0001)) &&
                        !((chapter.Signature == "ST") && (chapter.Version == 0x0001)))
                        return false;

                    // -4 means the size of Size.
                    reader.BaseStream.Seek(-4, SeekOrigin.Current);
                    // 8 means the total size of Signature, Version, and Checksum.
                    var body = reader.ReadBytes(chapter.Size - 8);
                    var sum = body.Sum(elem => (int)elem);
                    if (sum != chapter.Checksum)
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
            var numCharas = Enum.GetValues(typeof(CharaWithTotal)).Length;

            allScoreData.ClearData = new Dictionary<CharaWithTotal, ClearData>(numCharas);
            allScoreData.Header = new Header();
            allScoreData.Header.ReadFrom(reader);

            try
            {
                while (true)
                {
                    chapter.ReadFrom(reader);
                    switch (chapter.Signature)
                    {
                        case "CR":
                            {
                                var clearData = new ClearData(chapter);
                                clearData.ReadFrom(reader);
                                if (!allScoreData.ClearData.ContainsKey(clearData.Chara))
                                    allScoreData.ClearData.Add(clearData.Chara, clearData);
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
                            // 12 means the total size of Signature, Version, Checksum, and Size.
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
                (allScoreData.ClearData.Count == numCharas) &&
                (allScoreData.Status != null))
                return allScoreData;
            else
                return null;
        }

        protected override void Convert(Stream input, Stream output)
        {
            var reader = new StreamReader(input, Encoding.GetEncoding("shift_jis"));
            var writer = new StreamWriter(output, Encoding.GetEncoding("shift_jis"));

            var allLines = reader.ReadToEnd();
            allLines = this.ReplaceScore(allLines);
            allLines = this.ReplaceCareer(allLines);
            allLines = this.ReplaceCard(allLines);
            allLines = this.ReplaceCollectRate(allLines);
            allLines = this.ReplaceClear(allLines);
            allLines = this.ReplaceChara(allLines);
            allLines = this.ReplaceCharaEx(allLines);
            allLines = this.ReplacePractice(allLines);
            writer.Write(allLines);

            writer.Flush();
            writer.BaseStream.SetLength(writer.BaseStream.Position);
        }

        // %T13SCR[w][xx][y][z]
        private string ReplaceScore(string input)
        {
            var pattern = Utils.Format(@"%T13SCR([{0}])({1})(\d)([1-5])", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = (LevelPracticeWithTotal)ToLevel(match.Groups[1].Value);
                var chara = (CharaWithTotal)ToChara(match.Groups[2].Value);
                var rank = Utils.ToZeroBased(int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                var ranking = this.allScoreData.ClearData[chara].Rankings[level][rank];
                switch (type)
                {
                    case 1:     // name
                        return Encoding.Default.GetString(ranking.Name).Split('\0')[0];
                    case 2:     // score
                        return this.ToNumberString((ranking.Score * 10) + ranking.ContinueCount);
                    case 3:     // stage
                        if (ranking.DateTime > 0)
                        {
                            if (ranking.StageProgress == StageProgress.Extra)
                                return "Not Clear";
                            else if (ranking.StageProgress == StageProgress.ExtraClear)
                                return StageProgress.Clear.ToShortName();
                            else
                                return ranking.StageProgress.ToShortName();
                        }
                        else
                            return StageProgress.None.ToShortName();
                    case 4:     // date & time
                        if (ranking.DateTime > 0)
                            return new DateTime(1970, 1, 1).AddSeconds(ranking.DateTime)
                                .ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                        else
                            return "----/--/-- --:--:--";
                    case 5:     // slow
                        if (ranking.DateTime > 0)
                            return Utils.Format("{0:F3}%", ranking.SlowRate);
                        else
                            return "-----%";
                    default:    // unreachable
                        return match.ToString();
                }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T13C[w][xxx][yy][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceCareer(string input)
        {
            var pattern = Utils.Format(@"%T13C([SP])(\d{{3}})({0})([12])", CharaWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();
                var number = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                var chara = ToCharaWithTotal(match.Groups[3].Value);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                Func<SpellCard, int> getCount = (card => 0);
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

                var cards = this.allScoreData.ClearData[chara].Cards;
                if (number == 0)
                    return this.ToNumberString(cards.Values.Sum(getCount));
                else if (CardTable.ContainsKey(number))
                {
                    SpellCard card;
                    if (cards.TryGetValue(number, out card))
                        return this.ToNumberString(getCount(card));
                    else
                        return "0";
                }
                else
                    return match.ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T13CARD[xxx][y]
        private string ReplaceCard(string input)
        {
            var pattern = @"%T13CARD(\d{3})([NR])";
            var evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var type = match.Groups[2].Value.ToUpperInvariant();

                if (CardTable.ContainsKey(number))
                {
                    if (type == "N")
                    {
                        var cards = this.allScoreData.ClearData[CharaWithTotal.Total].Cards;
                        SpellCard card;
                        if (cards.TryGetValue(number, out card) && card.HasTried())
                            return CardTable[number].Name;
                        else
                            return "??????????";
                    }
                    else
                    {
                        var level = CardTable[number].Level;
                        var levelName = level.ToLongName();
                        return (levelName.Length > 0) ? levelName : level.ToString();
                    }
                }
                else
                    return match.ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T13CRG[v][w][xx][y][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceCollectRate(string input)
        {
            var pattern = Utils.Format(
                @"%T13CRG([SP])([{0}])({1})([{2}])([12])",
                LevelPracticeWithTotalPattern,
                CharaWithTotalPattern,
                StageWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();
                var level = ToLevelPracticeWithTotal(match.Groups[2].Value);
                var chara = ToCharaWithTotal(match.Groups[3].Value);
                var stage = ToStageWithTotal(match.Groups[4].Value);
                var type = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);

                if (stage == StageWithTotal.Extra)
                    return match.ToString();

                Func<SpellCard, bool> findByKindType = (card => true);
                Func<SpellCard, bool> findByLevel = (card => true);
                Func<SpellCard, bool> findByStage = (card => true);

                if (kind == "S")
                {
                    if (type == 1)
                        findByKindType = (card => card.ClearCount > 0);
                    else
                        findByKindType = (card => card.TrialCount > 0);
                }
                else
                {
                    if (type == 1)
                        findByKindType = (card => card.PracticeClearCount > 0);
                    else
                        findByKindType = (card => card.PracticeTrialCount > 0);
                }

                if (stage == StageWithTotal.Total)
                {
                    // Do nothing
                }
                else
                    findByStage = (card => CardTable[card.Number].Stage == (StagePractice)stage);

                switch (level)
                {
                    case LevelPracticeWithTotal.Total:
                        // Do nothing
                        break;
                    case LevelPracticeWithTotal.Extra:
                        findByStage = (card => CardTable[card.Number].Stage == StagePractice.Extra);
                        break;
                    case LevelPracticeWithTotal.OverDrive:
                        findByStage = (card => CardTable[card.Number].Stage == StagePractice.OverDrive);
                        break;
                    default:
                        findByLevel = (card => card.Level == (LevelPractice)level);
                        break;
                }

                var and = Utils.MakeAndPredicate(findByKindType, findByLevel, findByStage);
                return this.allScoreData.ClearData[chara].Cards.Values.Count(and)
                    .ToString(CultureInfo.CurrentCulture);
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T13CLEAR[x][yy]
        private string ReplaceClear(string input)
        {
            var pattern = Utils.Format(@"%T13CLEAR([{0}])({1})", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = (LevelPracticeWithTotal)ToLevel(match.Groups[1].Value);
                var chara = (CharaWithTotal)ToChara(match.Groups[2].Value);

                var rankings = this.allScoreData.ClearData[chara].Rankings[level]
                    .Where(ranking => ranking.DateTime > 0);
                var stageProgress = (rankings.Count() > 0)
                    ? rankings.Max(ranking => ranking.StageProgress) : StageProgress.None;

                if (stageProgress == StageProgress.Extra)
                    return "Not Clear";
                else if (stageProgress == StageProgress.ExtraClear)
                    return StageProgress.Clear.ToShortName();
                else
                    return stageProgress.ToShortName();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T13CHARA[xx][y]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceChara(string input)
        {
            var pattern = Utils.Format(@"%T13CHARA({0})([1-3])", CharaWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var chara = ToCharaWithTotal(match.Groups[1].Value);
                var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                Func<ClearData, long> getValueByType = (data => 0L);
                Func<long, string> toString = (value => string.Empty);
                if (type == 1)
                {
                    getValueByType = (data => data.TotalPlayCount);
                    toString = (value => this.ToNumberString(value));
                }
                else if (type == 2)
                {
                    getValueByType = (data => data.PlayTime);
                    toString = (value => new Time(value).ToString());
                }
                else
                {
                    getValueByType = (data => data.ClearCounts.Values.Sum());
                    toString = (value => this.ToNumberString(value));
                }

                Func<AllScoreData, long> getValueByChara = (allData => 0L);
                if (chara == CharaWithTotal.Total)
                    getValueByChara = (allData => allData.ClearData.Values.Sum(
                        data => (data.Chara != chara) ? getValueByType(data) : 0L));
                else
                    getValueByChara = (allData => getValueByType(allData.ClearData[chara]));

                return toString(getValueByChara(this.allScoreData));
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T13CHARAEX[x][yy][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceCharaEx(string input)
        {
            var pattern = Utils.Format(
                @"%T13CHARAEX([{0}])({1})([1-3])", LevelWithTotalPattern, CharaWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevelWithTotal(match.Groups[1].Value);
                var chara = ToCharaWithTotal(match.Groups[2].Value);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                Func<ClearData, long> getValueByType = (data => 0L);
                Func<long, string> toString = (value => string.Empty);
                if (type == 1)
                {
                    getValueByType = (data => data.TotalPlayCount);
                    toString = (value => this.ToNumberString(value));
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
                    toString = (value => this.ToNumberString(value));
                }

                Func<AllScoreData, long> getValueByChara = (allData => 0L);
                if (chara == CharaWithTotal.Total)
                    getValueByChara = (allData => allData.ClearData.Values.Sum(
                        data => (data.Chara != chara) ? getValueByType(data) : 0L));
                else
                    getValueByChara = (allData => getValueByType(allData.ClearData[chara]));

                return toString(getValueByChara(this.allScoreData));
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T13PRAC[x][yy][z]
        private string ReplacePractice(string input)
        {
            var pattern = Utils.Format(
                @"%T13PRAC([{0}])({1})([{2}])", LevelPattern, CharaPattern, StagePattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = (LevelPractice)ToLevel(match.Groups[1].Value);
                var chara = (CharaWithTotal)ToChara(match.Groups[2].Value);
                var stage = (StagePractice)ToStage(match.Groups[3].Value);

                if (level == LevelPractice.Extra)
                    return match.ToString();
                if (stage == StagePractice.Extra)
                    return match.ToString();

                if (this.allScoreData.ClearData.ContainsKey(chara))
                {
                    var key = new LevelStagePair(level, stage);
                    var practices = this.allScoreData.ClearData[chara].Practices;
                    return practices.ContainsKey(key)
                        ? this.ToNumberString(practices[key].Score * 10) : "0";
                }
                else
                    return "0";
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }
    }
}
