//-----------------------------------------------------------------------
// <copyright file="Th08Converter.cs" company="None">
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
    using System.IO;
    using System.Linq;
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
    public class Th08Converter : ThConverter
    {
        private enum Level               { Easy, Normal, Hard, Lunatic, Extra }
        private enum LevelWithTotal      { Easy, Normal, Hard, Lunatic, Extra, Total }
        private enum LevelPractice       { Easy, Normal, Hard, Lunatic, Extra, LastWord, Total }
        private enum LevelShort          { E, N, H, L, X }
        private enum LevelShortWithTotal { E, N, H, L, X, T }
        private enum LevelShortPractice  { E, N, H, L, X, W, T }

        private enum Chara
        {
            ReimuYukari, MarisaAlice, SakuyaRemilia, YoumuYuyuko,
            Reimu, Yukari, Marisa, Alice, Sakuya, Remilia, Youmu, Yuyuko
        }
        private enum CharaWithTotal
        {
            ReimuYukari, MarisaAlice, SakuyaRemilia, YoumuYuyuko,
            Reimu, Yukari, Marisa, Alice, Sakuya, Remilia, Youmu, Yuyuko, Total
        }
        private enum CharaShort          { RY, MA, SR, YY, RM, YK, MR, AL, SK, RL, YM, YU }
        private enum CharaShortWithTotal { RY, MA, SR, YY, RM, YK, MR, AL, SK, RL, YM, YU, TL }

        private enum Stage
        {
            Stage1, Stage2, Stage3, Stage4A, Stage4B, Stage5, Stage6A, Stage6B, Extra
        }
        private enum StageWithTotal
        {
            Stage1, Stage2, Stage3, Stage4A, Stage4B, Stage5, Stage6A, Stage6B, Extra, Total
        }
        private enum StagePractice
        {
            Stage1, Stage2, Stage3, Stage4A, Stage4B, Stage5, Stage6A, Stage6B, Extra, LastWord
        }

        private static readonly string[] StageArray =
        {
            "Stage 1", "Stage 2", "Stage 3", "Stage 4-uncanny", "Stage 4-powerful",
            "Stage 5", "Stage 6-Eirin", "Stage 6-Kaguya", "Extra Stage"
        };
        private static readonly string[] StageShortArray =
        {
            "1A", "2A", "3A", "4A", "4B", "5A", "6A", "6B"
        };
        private static readonly string[] StageShortWithTotalArray =
        {
            "00", "1A", "2A", "3A", "4A", "4B", "5A", "6A", "6B"
        };

        [Flags]
        private enum PlayableStageFlag
        {
            Stage1   = 0x0001,
            Stage2   = 0x0002,
            Stage3   = 0x0004,
            Stage4A  = 0x0008,
            Stage4B  = 0x0010,
            Stage5   = 0x0020,
            Stage6A  = 0x0040,
            Stage6B  = 0x0080,
            Unknown  = 0x4000,
            AllClear = 0x8000
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1008:OpeningParenthesisMustBeSpacedCorrectly",
            Justification = "Reviewed.")]
        private static readonly List<HighScore> InitialRanking = new List<HighScore>()
        {
            new HighScore(100000),
            new HighScore( 90000),
            new HighScore( 80000),
            new HighScore( 70000),
            new HighScore( 60000),
            new HighScore( 50000),
            new HighScore( 40000),
            new HighScore( 30000),
            new HighScore( 20000),
            new HighScore( 10000)
        };

        private const int NumCards = 222;

        // Thanks to thwiki.info
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1008:OpeningParenthesisMustBeSpacedCorrectly",
            Justification = "Reviewed.")]
        private static readonly Dictionary<StagePractice, Range<int>> StageCardTable =
            new Dictionary<StagePractice, Range<int>>()
            {
                { StagePractice.Stage1,   new Range<int>(  0,  12) },
                { StagePractice.Stage2,   new Range<int>( 13,  31) },
                { StagePractice.Stage3,   new Range<int>( 32,  53) },
                { StagePractice.Stage4A,  new Range<int>( 54,  76) },
                { StagePractice.Stage4B,  new Range<int>( 77,  99) },
                { StagePractice.Stage5,   new Range<int>(100, 118) },
                { StagePractice.Stage6A,  new Range<int>(119, 146) },
                { StagePractice.Stage6B,  new Range<int>(147, 190) },
                { StagePractice.Extra,    new Range<int>(191, 204) },
                { StagePractice.LastWord, new Range<int>(205, 221) }
            };

        private class CharaLevelPair : Pair<Chara, Level>
        {
            public Chara Chara { get { return this.First; } }
            public Level Level { get { return this.Second; } }

            public CharaLevelPair(Chara chara, Level level) : base(chara, level) { }
            public CharaLevelPair(CharaShort chara, LevelShort level) : base((Chara)chara, (Level)level) { }
        }

        private class StageLevelPair : Pair<Stage, Level>
        {
            public Stage Stage { get { return this.First; } }
            public Level Level { get { return this.Second; } }

            public StageLevelPair(Stage stage, Level level) : base(stage, level) { }
        }

        private class AllScoreData
        {
            public Header Header { get; set; }
            public Dictionary<CharaLevelPair, List<HighScore>> Rankings { get; set; }
            public Dictionary<CharaWithTotal, ClearData> ClearData { get; set; }
            public CardAttack[] CardAttacks { get; set; }
            public Dictionary<Chara, PracticeScore> PracticeScores { get; set; }
            public FLSP Flsp { get; set; }
            public PlayList PlayList { get; set; }
            public LastName LastName { get; set; }
            public VersionInfo VersionInfo { get; set; }

            public AllScoreData()
            {
                this.Rankings = new Dictionary<CharaLevelPair, List<HighScore>>();
                this.ClearData =
                    new Dictionary<CharaWithTotal, ClearData>(Enum.GetValues(typeof(CharaWithTotal)).Length);
                this.CardAttacks = new CardAttack[NumCards];
                this.PracticeScores =
                    new Dictionary<Chara, PracticeScore>(Enum.GetValues(typeof(Chara)).Length);
            }
        }

        private class Chapter : Utils.IBinaryReadable
        {
            public string Signature { get; private set; }   // .Length = 4
            public short Size1 { get; private set; }
            public short Size2 { get; private set; }        // always equal to size1?

            public Chapter() { }
            public Chapter(Chapter ch)
            {
                this.Signature = ch.Signature;
                this.Size1 = ch.Size1;
                this.Size2 = ch.Size2;
            }

            public virtual void ReadFrom(BinaryReader reader)
            {
                this.Signature = new string(reader.ReadChars(4));
                this.Size1 = reader.ReadInt16();
                this.Size2 = reader.ReadInt16();
            }
        }

        private class Header : Chapter
        {
            public uint Unknown { get; private set; }      // always 0x00000001?

            public Header(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "TH8K")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x000C)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x000C)
                //     throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.Unknown = reader.ReadUInt32();
            }
        }

        private class HighScore : Chapter   // per character, level, rank
        {
            public uint Unknown1 { get; private set; }      // always 0x00000004?
            public uint Score { get; private set; }         // * 10
            public float SlowRate { get; private set; }
            public Chara Chara { get; private set; }        // size: 1Byte
            public Level Level { get; private set; }        // size: 1Byte
            public byte StageProgress { get; private set; } // 0..7: Stage1..Stage6B, 99: all clear
            public byte[] Name { get; private set; }        // .Length = 9, null-terminated
            public byte[] Date { get; private set; }        // .Length = 6, "mm/dd\0"
            public ushort ContinueCount { get; private set; }
            public byte[] Unknown2 { get; private set; }    // .Length = 0x1C
            // 01 00 00 00 04 00 09 00 FF FF FF FF FF FF FF FF
            // 05 00 00 00 01 00 08 00 58 02 58 02
            public byte PlayerNum { get; private set; }     // 0-origin
            public byte[] Unknown3 { get; private set; }    // .Length = 0x1F
            // NN 03 00 01 01 LL 01 00 02 00 00 ** ** 00 00 00
            // 00 00 00 00 00 00 00 00 00 00 00 00 01 40 00 00
            // where NN: PlayerNum, LL: level, **: unknown (0x64 or 0x0A; 0x50 or 0x0A)
            public uint PlayTime { get; private set; }      // = seconds * 60fps
            public int PointItem { get; private set; }
            public uint Unknown4 { get; private set; }      // always 0x00000000?
            public int MissCount { get; private set; }
            public int BombCount { get; private set; }
            public int LastSpellCount { get; private set; }
            public int PauseCount { get; private set; }
            public int TimePoint { get; private set; }
            public int HumanRate { get; private set; }      // / 100
            public byte[] CardFlags { get; private set; }   // .Length = 222
            public byte[] Padding { get; private set; }     // .Length = 2

            public HighScore(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "HSCR")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0168)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x0168)
                //     throw new InvalidDataException("Size2");
            }

            public HighScore(uint score)    // for InitialRanking only
                : base()
            {
                this.Score = score;
                this.Name = Encoding.Default.GetBytes("--------\0");
                this.Date = Encoding.Default.GetBytes("--/--\0");
                this.CardFlags = new byte[NumCards];
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.Unknown1 = reader.ReadUInt32();
                this.Score = reader.ReadUInt32();
                this.SlowRate = reader.ReadSingle();
                this.Chara = (Chara)reader.ReadByte();
                this.Level = (Level)reader.ReadByte();
                this.StageProgress = reader.ReadByte();
                this.Name = reader.ReadBytes(9);
                this.Date = reader.ReadBytes(6);
                this.ContinueCount = reader.ReadUInt16();
                this.Unknown2 = reader.ReadBytes(0x1C);
                this.PlayerNum = reader.ReadByte();
                this.Unknown3 = reader.ReadBytes(0x1F);
                this.PlayTime = reader.ReadUInt32();
                this.PointItem = reader.ReadInt32();
                this.Unknown4 = reader.ReadUInt32();
                this.MissCount = reader.ReadInt32();
                this.BombCount = reader.ReadInt32();
                this.LastSpellCount = reader.ReadInt32();
                this.PauseCount = reader.ReadInt32();
                this.TimePoint = reader.ReadInt32();
                this.HumanRate = reader.ReadInt32();
                this.CardFlags = reader.ReadBytes(Th08Converter.NumCards);
                this.Padding = reader.ReadBytes(2);
            }
        }

        private class ClearData : Chapter   // per character-with-total
        {
            public uint Unknown1 { get; private set; }                      // always 0x00000004?
            public PlayableStageFlag[] StoryFlags { get; private set; }     // size: 2Bytes * levels; really...?
            public PlayableStageFlag[] PracticeFlags { get; private set; }  // size: 2Bytes * levels; really...?
            public byte Unknown2 { get; private set; }                      // always 0x00?
            public CharaWithTotal Chara { get; private set; }               // size: 1Byte
            public ushort Unknown3 { get; private set; }                    // always 0x0000?

            public ClearData(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "CLRD")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0024)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x0024)
                //     throw new InvalidDataException("Size2");

                var numLevels = Enum.GetValues(typeof(Level)).Length;
                this.StoryFlags = new PlayableStageFlag[numLevels];
                this.PracticeFlags = new PlayableStageFlag[numLevels];
            }

            public override void ReadFrom(BinaryReader reader)
            {
                var levels = Enum.GetValues(typeof(Level));
                this.Unknown1 = reader.ReadUInt32();
                foreach (int level in levels)
                    this.StoryFlags[level] = (PlayableStageFlag)reader.ReadUInt16();
                foreach (int level in levels)
                    this.PracticeFlags[level] = (PlayableStageFlag)reader.ReadUInt16();
                this.Unknown2 = reader.ReadByte();
                this.Chara = (CharaWithTotal)reader.ReadByte();
                this.Unknown3 = reader.ReadUInt16();
            }
        }

        private class CardAttack : Chapter      // per card
        {
            public uint Unknown1 { get; private set; }      // always 0x00000003?
            public short Number { get; private set; }       // 0-origin
            public byte Unknown2 { get; private set; }      // variable...
            public LevelPractice Level { get; private set; }
            public byte[] CardName { get; private set; }    // .Length = 0x30
            public byte[] EnemyName { get; private set; }   // .Length = 0x30
            public byte[] Comment { get; private set; }     // .Length = 0x80, should split by '\0'
            public CardAttackCareer StoryCareer { get; private set; }
            public CardAttackCareer PracticeCareer { get; private set; }
            public uint Unknown3 { get; private set; }      // always 0x00000000?

            public CardAttack(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "CATK")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x022C)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x022C)
                //     throw new InvalidDataException("Size2");

                this.StoryCareer = new CardAttackCareer();
                this.PracticeCareer = new CardAttackCareer();
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.Unknown1 = reader.ReadUInt32();
                this.Number = reader.ReadInt16();
                this.Unknown2 = reader.ReadByte();
                this.Level = (LevelPractice)reader.ReadByte();
                this.CardName = reader.ReadBytes(0x30);
                this.EnemyName = reader.ReadBytes(0x30);
                this.Comment = reader.ReadBytes(0x80);
                this.StoryCareer.ReadFrom(reader);
                this.PracticeCareer.ReadFrom(reader);
                this.Unknown3 = reader.ReadUInt32();
            }

            public bool HasTried()
            {
                return Encoding.Default.GetString(this.CardName).TrimEnd('\0').Length > 0;
            }
        }

        private class CardAttackCareer : Utils.IBinaryReadable      // per story or practice
        {
            public Dictionary<CharaWithTotal, uint> MaxBonuses { get; private set; }
            public Dictionary<CharaWithTotal, int> TrialCounts { get; private set; }
            public Dictionary<CharaWithTotal, int> ClearCounts { get; private set; }

            public CardAttackCareer()
            {
                var numCharas = Enum.GetValues(typeof(CharaWithTotal)).Length;
                this.MaxBonuses = new Dictionary<CharaWithTotal, uint>(numCharas);
                this.TrialCounts = new Dictionary<CharaWithTotal, int>(numCharas);
                this.ClearCounts = new Dictionary<CharaWithTotal, int>(numCharas);
            }

            public void ReadFrom(BinaryReader reader)
            {
                var charas = Enum.GetValues(typeof(CharaWithTotal));
                foreach (CharaWithTotal chara in charas)
                    this.MaxBonuses.Add(chara, reader.ReadUInt32());
                foreach (CharaWithTotal chara in charas)
                    this.TrialCounts.Add(chara, reader.ReadInt32());
                foreach (CharaWithTotal chara in charas)
                    this.ClearCounts.Add(chara, reader.ReadInt32());
            }
        }

        private class PracticeScore : Chapter   // per character
        {
            public uint Unknown1 { get; private set; }      // always 0x00000002?
            public Dictionary<StageLevelPair, int> PlayCounts { get; private set; }
            public Dictionary<StageLevelPair, int> HighScores { get; private set; }     // * 10
            public Chara Chara { get; private set; }        // size: 1Byte
            public byte[] Unknown2 { get; private set; }    // .Length = 3, always 0x000001?

            public PracticeScore(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "PSCR")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0178)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x0178)
                //     throw new InvalidDataException("Size2");

                this.PlayCounts = new Dictionary<StageLevelPair, int>();
                this.HighScores = new Dictionary<StageLevelPair, int>();
            }

            public override void ReadFrom(BinaryReader reader)
            {
                // The fields for Stage.Extra and Level.Extra actually exist...

                var stages = Enum.GetValues(typeof(Stage));
                var levels = Enum.GetValues(typeof(Level));
                this.Unknown1 = reader.ReadUInt32();
                foreach (Stage stage in stages)
                    foreach (Level level in levels)
                    {
                        var key = new StageLevelPair(stage, level);
                        if (!this.PlayCounts.ContainsKey(key))
                            this.PlayCounts.Add(key, 0);
                        this.PlayCounts[key] = reader.ReadInt32();
                    }
                foreach (Stage stage in stages)
                    foreach (Level level in levels)
                    {
                        var key = new StageLevelPair(stage, level);
                        if (!this.HighScores.ContainsKey(key))
                            this.HighScores.Add(key, 0);
                        this.HighScores[key] = reader.ReadInt32();
                    }
                this.Chara = (Chara)reader.ReadByte();
                this.Unknown2 = reader.ReadBytes(3);
            }
        }

        private class FLSP : Chapter    // FIXME
        {
            public byte[] Unknown { get; private set; }     // .Length = 0x18

            public FLSP(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "FLSP")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0020)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x0020)
                //     throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.Unknown = reader.ReadBytes(0x18);
            }
        }

        private class PlayList : Chapter
        {
            public uint Unknown1 { get; private set; }              // always 0x00000002?
            public Time TotalRunningTime { get; private set; }
            public Time TotalPlayTime { get; private set; }
            public Dictionary<Level, PlayCount> PlayCounts { get; private set; }
            public PlayCount Unknown2 { get; private set; }         // always all 0?
            public PlayCount TotalPlayCount { get; private set; }
            public byte[] BgmFlags { get; private set; }            // .Length = 21
            public byte[] Padding { get; private set; }             // .Length = 11

            public PlayList(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "PLST")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0228)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x0228)
                //     throw new InvalidDataException("Size2");

                this.PlayCounts = new Dictionary<Level, PlayCount>(Enum.GetValues(typeof(Level)).Length);
                this.Unknown2 = new PlayCount();
                this.TotalPlayCount = new PlayCount();
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.Unknown1 = reader.ReadUInt32();
                this.TotalRunningTime = new Time(
                    reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), false);
                this.TotalPlayTime = new Time(
                    reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), false);
                foreach (Level level in Enum.GetValues(typeof(Level)))
                {
                    var playCount = new PlayCount();
                    playCount.ReadFrom(reader);
                    if (!this.PlayCounts.ContainsKey(level))
                        this.PlayCounts.Add(level, playCount);
                }
                this.Unknown2.ReadFrom(reader);
                this.TotalPlayCount.ReadFrom(reader);
                this.BgmFlags = reader.ReadBytes(21);
                this.Padding = reader.ReadBytes(11);
            }
        }

        private class PlayCount : Utils.IBinaryReadable     // per level-with-total
        {
            public int TotalTrial { get; private set; }
            public Dictionary<Chara, int> Trials { get; private set; }
            public uint Unknown { get; private set; }       // always 0x00000000?
            public int TotalClear { get; private set; }
            public int TotalContinue { get; private set; }
            public int TotalPractice { get; private set; }

            public PlayCount()
            {
                this.Trials = new Dictionary<Chara, int>(Enum.GetValues(typeof(Chara)).Length);
            }

            public void ReadFrom(BinaryReader reader)
            {
                this.TotalTrial = reader.ReadInt32();
                foreach (Chara chara in Enum.GetValues(typeof(Chara)))
                    this.Trials.Add(chara, reader.ReadInt32());
                this.Unknown = reader.ReadUInt32();
                this.TotalClear = reader.ReadInt32();
                this.TotalContinue = reader.ReadInt32();
                this.TotalPractice = reader.ReadInt32();
            }
        }

        private class LastName : Chapter
        {
            public uint Unknown { get; private set; }   // always 0x00000001?
            public byte[] Name { get; private set; }    // .Length = 12, null-terminated

            public LastName(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "LSNM")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0018)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x0018)
                //     throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.Unknown = reader.ReadUInt32();
                this.Name = reader.ReadBytes(12);
            }
        }

        private class VersionInfo : Chapter
        {
            public uint Unknown1 { get; private set; }      // always 0x00000001?
            public byte[] Version { get; private set; }     // .Length = 6, null-terminated
            public byte[] Unknown2 { get; private set; }    // .Length = 3
            public byte[] Unknown3 { get; private set; }    // .Length = 3
            public uint Unknown4 { get; private set; }      // variable...

            public VersionInfo(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "VRSM")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x001C)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x001C)
                //     throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.Unknown1 = reader.ReadUInt32();
                this.Version = reader.ReadBytes(6);
                this.Unknown2 = reader.ReadBytes(3);
                this.Unknown3 = reader.ReadBytes(3);
                this.Unknown4 = reader.ReadUInt32();
            }
        }

        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get { return "1.00d"; }
        }

        public Th08Converter()
        {
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th08decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
            var size = (int)input.Length;
            ThCrypt.Decrypt(input, output, size, 0x59, 0x79, 0x0100, 0x0C00);

            var data = new byte[size];
            output.Seek(0, SeekOrigin.Begin);
            output.Read(data, 0, size);

            uint checksum = 0;
            byte temp = 0;
            for (var index = 2; index < size; index++)
            {
                temp += data[index - 1];
                temp = (byte)((temp >> 5) | (temp << 3));
                data[index] ^= temp;
                if (index > 3)
                    checksum += data[index];
            }

            output.Seek(0, SeekOrigin.Begin);
            output.Write(data, 0, size);

            return (ushort)checksum == BitConverter.ToUInt16(data, 2);
        }

        private bool Extract(Stream input, Stream output)
        {
            var reader = new BinaryReader(input);

            var unknown1 = reader.ReadUInt16();
            var checksum = reader.ReadUInt16();     // already checked by this.Decrypt()
            var version = reader.ReadInt16();
            if (version != 1)
                return false;

            var unknown2 = reader.ReadUInt16();
            var headerSize = reader.ReadInt32();
            if (headerSize != 0x1C)
                return false;

            var unknown3 = reader.ReadUInt32();
            var decodedSize = reader.ReadInt32();
            var decodedBodySize = reader.ReadInt32();
            var encodedBodySize = reader.ReadInt32();
            if (encodedBodySize != (input.Length - headerSize))
                return false;

            var header = new byte[headerSize];
            input.Seek(0, SeekOrigin.Begin);
            input.Read(header, 0, headerSize);
            output.Write(header, 0, headerSize);

            Lzss.Extract(input, output);
            output.Flush();
            output.SetLength(output.Position);

            return output.Position == decodedSize;
        }

        private bool Validate(Stream input)
        {
            var reader = new BinaryReader(input);
            var chapter = new Chapter();

            reader.ReadBytes(8);
            var headerSize = reader.ReadInt32();
            reader.ReadBytes(4);
            var decodedSize = reader.ReadInt32();
            var remainSize = decodedSize - headerSize;
            reader.ReadBytes(8);
            if (remainSize <= 0)
                return false;

            try
            {
                while (remainSize > 0)
                {
                    chapter.ReadFrom(reader);
                    if (chapter.Size1 == 0)
                        return false;

                    byte temp = 0;
                    switch (chapter.Signature)
                    {
                        case "TH8K":
                            temp = reader.ReadByte();
                            // 8 means the total size of Signature, Size1, and Size2.
                            reader.ReadBytes(chapter.Size1 - 8 - 1);
                            if (temp != 0x01)
                                return false;
                            break;
                        case "VRSM":
                            temp = reader.ReadByte();
                            reader.ReadBytes(chapter.Size1 - 8 - 1);    // 8 means the same above
                            if (temp != 0x01)
                                return false;
                            // th08.exe does something more things here...
                            break;
                        default:
                            reader.ReadBytes(chapter.Size1 - 8);        // 8 means the same above
                            break;
                    }
                    remainSize -= chapter.Size1;
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

            allScoreData.Rankings = new Dictionary<CharaLevelPair, List<HighScore>>();
            reader.ReadBytes(0x1C);

            try
            {
                while (true)
                {
                    chapter.ReadFrom(reader);
                    switch (chapter.Signature)
                    {
                        case "TH8K":
                            {
                                var header = new Header(chapter);
                                header.ReadFrom(reader);
                                allScoreData.Header = header;
                            }
                            break;

                        case "HSCR":
                            {
                                var score = new HighScore(chapter);
                                score.ReadFrom(reader);
                                var key = new CharaLevelPair(score.Chara, score.Level);
                                if (!allScoreData.Rankings.ContainsKey(key))
                                    allScoreData.Rankings.Add(key, new List<HighScore>(InitialRanking));
                                var ranking = allScoreData.Rankings[key];
                                ranking.Add(score);
                                ranking.Sort((lhs, rhs) => rhs.Score.CompareTo(lhs.Score));
                                ranking.RemoveAt(ranking.Count - 1);
                            }
                            break;

                        case "CLRD":
                            {
                                var clearData = new ClearData(chapter);
                                clearData.ReadFrom(reader);
                                if (!allScoreData.ClearData.ContainsKey(clearData.Chara))
                                    allScoreData.ClearData.Add(clearData.Chara, clearData);
                            }
                            break;

                        case "CATK":
                            {
                                var attack = new CardAttack(chapter);
                                attack.ReadFrom(reader);
                                allScoreData.CardAttacks[attack.Number] = attack;
                            }
                            break;

                        case "PSCR":
                            {
                                var score = new PracticeScore(chapter);
                                score.ReadFrom(reader);
                                if (!allScoreData.PracticeScores.ContainsKey(score.Chara))
                                    allScoreData.PracticeScores.Add(score.Chara, score);
                            }
                            break;

                        case "PLST":
                            {
                                var playList = new PlayList(chapter);
                                playList.ReadFrom(reader);
                                allScoreData.PlayList = playList;
                            }
                            break;

                        case "LSNM":
                            {
                                var lastName = new LastName(chapter);
                                lastName.ReadFrom(reader);
                                allScoreData.LastName = lastName;
                            }
                            break;

                        case "FLSP":
                            {
                                var flsp = new FLSP(chapter);
                                flsp.ReadFrom(reader);
                                allScoreData.Flsp = flsp;
                            }
                            break;

                        case "VRSM":
                            {
                                var versionInfo = new VersionInfo(chapter);
                                versionInfo.ReadFrom(reader);
                                allScoreData.VersionInfo = versionInfo;
                            }
                            break;

                        default:
                            // 8 means the total size of Signature, Size1, and Size2.
                            reader.ReadBytes(chapter.Size1 - 8);
                            break;
                    }
                }
            }
            catch (EndOfStreamException)
            {
                // It's OK, do nothing.
            }

            if ((allScoreData.Header != null) &&
                // (allScoreData.rankings.Count >= 0) &&
                (allScoreData.ClearData.Count == Enum.GetValues(typeof(CharaWithTotal)).Length) &&
                // (allScoreData.cardAttacks.Length == NumCards) &&
                // (allScoreData.practiceScores.Count >= 0) &&
                (allScoreData.Flsp != null) &&
                (allScoreData.PlayList != null) &&
                (allScoreData.LastName != null) &&
                (allScoreData.VersionInfo != null))
                return allScoreData;
            else
                return null;
        }

        protected override void Convert(Stream input, Stream output)
        {
            var reader = new StreamReader(input, Encoding.GetEncoding("shift_jis"));
            var writer = new StreamWriter(output, Encoding.GetEncoding("shift_jis"));

            var allLine = reader.ReadToEnd();
            allLine = this.ReplaceScore(allLine);
            allLine = this.ReplaceCareer(allLine);
            allLine = this.ReplaceCard(allLine);
            allLine = this.ReplaceCollectRate(allLine);
            allLine = this.ReplaceClear(allLine);
            allLine = this.ReplacePlay(allLine);
            allLine = this.ReplaceTime(allLine);
            allLine = this.ReplacePractice(allLine);
            writer.Write(allLine);

            writer.Flush();
            writer.BaseStream.SetLength(writer.BaseStream.Position);
        }

        // %T08SCR[w][xx][y][z]
        private string ReplaceScore(string input)
        {
            var pattern = string.Format(
                @"%T08SCR([{0}])({1})(\d)([\dA-G])",
                Utils.JoinEnumNames<LevelShort>(string.Empty),
                Utils.JoinEnumNames<CharaShort>("|"));
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                var level = Utils.ParseEnum<LevelShort>(match.Groups[1].Value, true);
                var chara = Utils.ParseEnum<CharaShort>(match.Groups[2].Value, true);
                var rank = (int.Parse(match.Groups[3].Value) + 9) % 10;     // 1..9,0 -> 0..9
                var type = match.Groups[4].Value.ToUpper();
                var key = new CharaLevelPair(chara, level);
                var score = this.allScoreData.Rankings.ContainsKey(key)
                    ? this.allScoreData.Rankings[key][rank] : InitialRanking[rank];

                switch (type)
                {
                    case "1":   // name
                        return Encoding.Default.GetString(score.Name).Split('\0')[0];
                    case "2":   // score
                        return this.ToNumberString((score.Score * 10) + score.ContinueCount);
                    case "3":   // stage
                        if ((level == LevelShort.X) &&
                            (Encoding.Default.GetString(score.Date).TrimEnd('\0') == "--/--"))
                            return StageArray[(int)Stage.Extra];
                        else if (score.StageProgress == 99)
                            return "All Clear";
                        else
                            return StageArray[score.StageProgress];
                    case "4":   // date
                        return Encoding.Default.GetString(score.Date).TrimEnd('\0');
                    case "5":   // slow rate
                        return score.SlowRate.ToString("F3") + "%";
                    case "6":   // play time
                        return new Time(score.PlayTime).ToString();
                    case "7":   // initial number of players
                        return (score.PlayerNum + 1).ToString();
                    case "8":   // point items
                        return this.ToNumberString(score.PointItem);
                    case "9":   // time point
                        return this.ToNumberString(score.TimePoint);
                    case "0":   // miss count
                        return score.MissCount.ToString();
                    case "A":   // bomb count
                        return score.BombCount.ToString();
                    case "B":   // last spell count
                        return score.LastSpellCount.ToString();
                    case "C":   // pause count
                        return this.ToNumberString(score.PauseCount);
                    case "D":   // continue count
                        return score.ContinueCount.ToString();
                    case "E":   // human rate
                        return string.Format("{0:F2}", score.HumanRate / 100.0) + "%";
                    case "F":   // got spell cards
                        var list = new List<string>();
                        for (var index = 0; index < NumCards; index++)
                        {
                            var attack = this.allScoreData.CardAttacks[index];
                            if (score.CardFlags[index] > 0)
                            {
                                var str = string.Format(
                                    "No.{0:D3} {1}",
                                    attack.Number + 1,
                                    Encoding.Default.GetString(attack.CardName).TrimEnd('\0'));
                                list.Add(str);
                            }
                        }
                        return string.Join("\n", list.ToArray());
                    case "G":   // number of got spell cards
                        return score.CardFlags.Count(flag => flag > 0).ToString();
                    default:    // unreachable
                        return match.ToString();
                }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T08C[w][xxx][yy][z]
        private string ReplaceCareer(string input)
        {
            var pattern = string.Format(
                @"%T08C([SP])(\d{{3}})({0})([1-3])",
                Utils.JoinEnumNames<CharaShortWithTotal>("|"));
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpper();
                var number = int.Parse(match.Groups[2].Value);
                var chara =
                    (CharaWithTotal)Utils.ParseEnum<CharaShortWithTotal>(match.Groups[3].Value, true);
                var type = int.Parse(match.Groups[4].Value);

                if (number == 0)
                    switch (type)
                    {
                        case 1:     // MaxBonus
                            if (kind == "S")
                                return this.ToNumberString(
                                    this.allScoreData.CardAttacks.Sum(
                                        attack => (attack != null)
                                            ? (long)attack.StoryCareer.MaxBonuses[chara] : 0L));
                            else
                                return this.ToNumberString(
                                    this.allScoreData.CardAttacks.Sum(
                                        attack => (attack != null)
                                            ? (long)attack.PracticeCareer.MaxBonuses[chara] : 0L));
                        case 2:     // clear count
                            if (kind == "S")
                                return this.ToNumberString(
                                    this.allScoreData.CardAttacks.Sum(
                                        attack => (attack != null)
                                            ? attack.StoryCareer.ClearCounts[chara] : 0));
                            else
                                return this.ToNumberString(
                                    this.allScoreData.CardAttacks.Sum(
                                        attack => (attack != null)
                                            ? attack.PracticeCareer.ClearCounts[chara] : 0));
                        case 3:     // trial count
                            if (kind == "S")
                                return this.ToNumberString(
                                    this.allScoreData.CardAttacks.Sum(
                                        attack => (attack != null)
                                            ? attack.StoryCareer.TrialCounts[chara] : 0));
                            else
                                return this.ToNumberString(
                                    this.allScoreData.CardAttacks.Sum(
                                        attack => (attack != null)
                                            ? attack.PracticeCareer.TrialCounts[chara] : 0));
                        default:    // unreachable
                            return match.ToString();
                    }
                else if (new Range<int>(1, NumCards).Contains(number))
                {
                    var attack = this.allScoreData.CardAttacks[number - 1];
                    if (attack != null)
                    {
                        var career = (kind == "S") ? attack.StoryCareer : attack.PracticeCareer;
                        switch (type)
                        {
                            case 1:     // MaxBonus
                                return this.ToNumberString(career.MaxBonuses[chara]);
                            case 2:     // clear count
                                return this.ToNumberString(career.ClearCounts[chara]);
                            case 3:     // trial count
                                return this.ToNumberString(career.TrialCounts[chara]);
                            default:    // unreachable
                                return match.ToString();
                        }
                    }
                    else
                        return "0";
                }
                else
                    return match.ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T08CARD[xxx][y]
        private string ReplaceCard(string input)
        {
            var pattern = @"%T08CARD(\d{3})([NR])";
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value);
                var type = match.Groups[2].Value.ToUpper();

                if (new Range<int>(1, NumCards).Contains(number))
                {
                    var attack = this.allScoreData.CardAttacks[number - 1];
                    if (type == "N")
                        return ((attack != null) && attack.HasTried())
                            ? Encoding.Default.GetString(attack.CardName).TrimEnd('\0') : "??????????";
                    else
                    {
                        if ((attack != null) && attack.HasTried())
                            return (attack.Level == LevelPractice.LastWord)
                                ? "Last Word" : attack.Level.ToString();
                        else
                            return "?????";
                    }
                }
                else
                    return match.ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T08CRG[v][w][xx][yy][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceCollectRate(string input)
        {
            var pattern = string.Format(
                @"%T08CRG([SP])([{0}])({1})({2})([12])",
                Utils.JoinEnumNames<LevelShortPractice>(string.Empty),
                Utils.JoinEnumNames<CharaShortWithTotal>("|"),
                string.Join("|", StageShortWithTotalArray));
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpper();
                var level = (LevelPractice)Utils.ParseEnum<LevelShortPractice>(
                    match.Groups[2].Value, true);
                var chara = (CharaWithTotal)Utils.ParseEnum<CharaShortWithTotal>(
                    match.Groups[3].Value, true);
                var stage = Array.IndexOf(StageShortWithTotalArray, match.Groups[4].Value.ToUpper());
                var type = int.Parse(match.Groups[5].Value);

                Func<CardAttack, bool> findCard = (attack => false);

                if (level == LevelPractice.Total)
                {
                    if (stage == 0)     // total
                    {
                        if (kind == "S")
                        {
                            if (type == 1)
                                findCard = (attack =>
                                    (attack != null) &&
                                    (attack.StoryCareer.ClearCounts[chara] > 0));
                            else
                                findCard = (attack =>
                                    (attack != null) &&
                                    (attack.StoryCareer.TrialCounts[chara] > 0));
                        }
                        else
                        {
                            if (type == 1)
                                findCard = (attack =>
                                    (attack != null) &&
                                    (attack.PracticeCareer.ClearCounts[chara] > 0));
                            else
                                findCard = (attack =>
                                    (attack != null) &&
                                    (attack.PracticeCareer.TrialCounts[chara] > 0));
                        }
                    }
                    else
                    {
                        var st = (StagePractice)(stage - 1);
                        if (kind == "S")
                        {
                            if (type == 1)
                                findCard = (attack =>
                                    (attack != null) &&
                                    StageCardTable[st].Contains(attack.Number) &&
                                    (attack.StoryCareer.ClearCounts[chara] > 0));
                            else
                                findCard = (attack =>
                                    (attack != null) &&
                                    StageCardTable[st].Contains(attack.Number) &&
                                    (attack.StoryCareer.TrialCounts[chara] > 0));
                        }
                        else
                        {
                            if (type == 1)
                                findCard = (attack =>
                                    (attack != null) &&
                                    StageCardTable[st].Contains(attack.Number) &&
                                    (attack.PracticeCareer.ClearCounts[chara] > 0));
                            else
                                findCard = (attack =>
                                    (attack != null) &&
                                    StageCardTable[st].Contains(attack.Number) &&
                                    (attack.PracticeCareer.TrialCounts[chara] > 0));
                        }
                    }
                }
                else if ((level == LevelPractice.Extra) || (level == LevelPractice.LastWord))
                {
                    var st = (level == LevelPractice.Extra)
                        ? StagePractice.Extra : StagePractice.LastWord;
                    if (kind == "S")
                    {
                        if (type == 1)
                            findCard = (attack =>
                                (attack != null) &&
                                StageCardTable[st].Contains(attack.Number) &&
                                (attack.StoryCareer.ClearCounts[chara] > 0));
                        else
                            findCard = (attack =>
                                (attack != null) &&
                                StageCardTable[st].Contains(attack.Number) &&
                                (attack.StoryCareer.TrialCounts[chara] > 0));
                    }
                    else
                    {
                        if (type == 1)
                            findCard = (attack =>
                                (attack != null) &&
                                StageCardTable[st].Contains(attack.Number) &&
                                (attack.PracticeCareer.ClearCounts[chara] > 0));
                        else
                            findCard = (attack =>
                                (attack != null) &&
                                StageCardTable[st].Contains(attack.Number) &&
                                (attack.PracticeCareer.TrialCounts[chara] > 0));
                    }
                }
                else
                {
                    if (stage == 0)     // total
                    {
                        if (kind == "S")
                        {
                            if (type == 1)
                                findCard = (attack =>
                                    (attack != null) &&
                                    (attack.Level == level) &&
                                    (attack.StoryCareer.ClearCounts[chara] > 0));
                            else
                                findCard = (attack =>
                                    (attack != null) &&
                                    (attack.Level == level) &&
                                    (attack.StoryCareer.TrialCounts[chara] > 0));
                        }
                        else
                        {
                            if (type == 1)
                                findCard = (attack =>
                                    (attack != null) &&
                                    (attack.Level == level) &&
                                    (attack.PracticeCareer.ClearCounts[chara] > 0));
                            else
                                findCard = (attack =>
                                    (attack != null) &&
                                    (attack.Level == level) &&
                                    (attack.PracticeCareer.TrialCounts[chara] > 0));
                        }
                    }
                    else
                    {
                        var st = (StagePractice)(stage - 1);
                        if (kind == "S")
                        {
                            if (type == 1)
                                findCard = (attack =>
                                    (attack != null) &&
                                    StageCardTable[st].Contains(attack.Number) &&
                                    (attack.Level == level) &&
                                    (attack.StoryCareer.ClearCounts[chara] > 0));
                            else
                                findCard = (attack =>
                                    (attack != null) &&
                                    StageCardTable[st].Contains(attack.Number) &&
                                    (attack.Level == level) &&
                                    (attack.StoryCareer.TrialCounts[chara] > 0));
                        }
                        else
                        {
                            if (type == 1)
                                findCard = (attack =>
                                    (attack != null) &&
                                    StageCardTable[st].Contains(attack.Number) &&
                                    (attack.Level == level) &&
                                    (attack.PracticeCareer.ClearCounts[chara] > 0));
                            else
                                findCard = (attack =>
                                    (attack != null) &&
                                    StageCardTable[st].Contains(attack.Number) &&
                                    (attack.Level == level) &&
                                    (attack.PracticeCareer.TrialCounts[chara] > 0));
                        }
                    }
                }

                return this.allScoreData.CardAttacks.Count(findCard).ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T08CLEAR[x][yy]
        private string ReplaceClear(string input)
        {
            var pattern = string.Format(
                @"%T08CLEAR([{0}])({1})",
                Utils.JoinEnumNames<LevelShort>(string.Empty),
                Utils.JoinEnumNames<CharaShort>("|"));
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                var level = Utils.ParseEnum<LevelShort>(match.Groups[1].Value, true);
                var chara = Utils.ParseEnum<CharaShort>(match.Groups[2].Value, true);

                var key = new CharaLevelPair(chara, level);
                if (this.allScoreData.Rankings.ContainsKey(key))
                {
                    var stageProgress = 0;
                    foreach (var rank in this.allScoreData.Rankings[key])
                        stageProgress = Math.Max(stageProgress, rank.StageProgress);
                    if ((stageProgress == (int)Stage.Stage4A) || (stageProgress == (int)Stage.Stage4B))
                        return "Stage 4";
                    else if (stageProgress < (int)Stage.Stage6B)
                        return StageArray[stageProgress];
                    else if (stageProgress == (int)Stage.Stage6B)
                        return "FinalA Clear";
                    else if (stageProgress == (int)Stage.Extra)
                        return "Not Clear";
                    else if (stageProgress == 99)
                        return "All Clear";
                    else
                        return "-------";
                }
                else
                    return "-------";
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T08PLAY[x][yy]
        private string ReplacePlay(string input)
        {
            var pattern = string.Format(
                @"%T08PLAY([{0}])({1}|CL|CN|PR)",
                Utils.JoinEnumNames<LevelShortWithTotal>(string.Empty),
                Utils.JoinEnumNames<CharaShortWithTotal>("|"));
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                var level = Utils.ParseEnum<LevelShortWithTotal>(match.Groups[1].Value, true);
                var charaAndMore = match.Groups[2].Value.ToUpper();

                var playCount = (level == LevelShortWithTotal.T)
                    ? this.allScoreData.PlayList.TotalPlayCount
                    : this.allScoreData.PlayList.PlayCounts[(Level)level];

                switch (charaAndMore)
                {
                    case "CL":  // clear count
                        return this.ToNumberString(playCount.TotalClear);
                    case "CN":  // continue count
                        return this.ToNumberString(playCount.TotalContinue);
                    case "PR":  // practice count
                        return this.ToNumberString(playCount.TotalPractice);
                    default:
                        var chara = Utils.ParseEnum<CharaShortWithTotal>(match.Groups[2].Value, true);
                        return this.ToNumberString((chara == CharaShortWithTotal.TL)
                            ? playCount.TotalTrial : playCount.Trials[(Chara)chara]);
                }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T08TIME(ALL|PLY)
        private string ReplaceTime(string input)
        {
            var pattern = @"%T08TIME(ALL|PLY)";
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpper();

                return (kind == "ALL")
                    ? this.allScoreData.PlayList.TotalRunningTime.ToLongString()
                    : this.allScoreData.PlayList.TotalPlayTime.ToLongString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T08PRAC[w][xx][yy][z]
        private string ReplacePractice(string input)
        {
            var pattern = string.Format(
                @"%T08PRAC([{0}])({1})({2})([12])",
                Utils.JoinEnumNames<LevelShort>(string.Empty),
                Utils.JoinEnumNames<CharaShort>("|"),
                string.Join("|", StageShortArray));
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                var level = (Level)Utils.ParseEnum<LevelShort>(match.Groups[1].Value, true);
                var chara = (Chara)Utils.ParseEnum<CharaShort>(match.Groups[2].Value, true);
                var stage = (Stage)Array.IndexOf(StageShortArray, match.Groups[3].Value.ToUpper());
                var type = int.Parse(match.Groups[4].Value);

                if (level == Level.Extra)
                    return match.ToString();

                if (this.allScoreData.PracticeScores.ContainsKey(chara))
                {
                    var scores = this.allScoreData.PracticeScores[chara];
                    var key = new StageLevelPair(stage, level);
                    if (type == 1)
                        return scores.HighScores.ContainsKey(key)
                            ? this.ToNumberString(scores.HighScores[key] * 10) : "0";
                    else
                        return scores.HighScores.ContainsKey(key)
                            ? this.ToNumberString(scores.PlayCounts[key]) : "0";
                }
                else
                    return "0";
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }
    }
}
