using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter
{
    public class Th07Converter : ThConverter
    {
        private enum Level               { Easy, Normal, Hard, Lunatic, Extra, Phantasm }
        private enum LevelWithTotal      { Easy, Normal, Hard, Lunatic, Extra, Phantasm, Total }
        private enum LevelShort          { E, N, H, L, X, P }
        private enum LevelShortWithTotal { E, N, H, L, X, P, T }

        private enum Chara               { ReimuA, ReimuB, MarisaA, MarisaB, SakuyaA, SakuyaB }
        private enum CharaWithTotal      { ReimuA, ReimuB, MarisaA, MarisaB, SakuyaA, SakuyaB, Total }
        private enum CharaShort          { RA, RB, MA, MB, SA, SB }
        private enum CharaShortWithTotal { RA, RB, MA, MB, SA, SB, TL }

        private enum Stage          { Stage1, Stage2, Stage3, Stage4, Stage5, Stage6, Extra, Phantasm }
        private enum StageWithTotal { Stage1, Stage2, Stage3, Stage4, Stage5, Stage6, Extra, Phantasm, Total }

        private static readonly string[] StageProgressArray =
        {
            "-------", "Stage 1", "Stage 2", "Stage 3", "Stage 4", "Stage 5", "Stage 6",
            "Extra Stage", "Phantasm Stage"
        };

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

        private const int NumCards = 141;

        // Thanks to thwiki.info
        private static readonly Dictionary<Stage, Range<int>> StageCardTable =
            new Dictionary<Stage, Range<int>>()
            {
                { Stage.Stage1,   new Range<int>{ Min = 0,   Max = 9   } },
                { Stage.Stage2,   new Range<int>{ Min = 10,  Max = 25  } },
                { Stage.Stage3,   new Range<int>{ Min = 26,  Max = 43  } },
                { Stage.Stage4,   new Range<int>{ Min = 44,  Max = 67  } },
                { Stage.Stage5,   new Range<int>{ Min = 68,  Max = 87  } },
                { Stage.Stage6,   new Range<int>{ Min = 88,  Max = 115 } },
                { Stage.Extra,    new Range<int>{ Min = 116, Max = 127 } },
                { Stage.Phantasm, new Range<int>{ Min = 128, Max = 140 } }
            };

        // Thanks to www57.atwiki.jp/2touhoukouryaku
        private static readonly Level[] CardLevelTable =
        {
            Level.Hard, Level.Lunatic, Level.Easy, Level.Normal, Level.Hard,
            Level.Lunatic, Level.Easy, Level.Normal, Level.Hard, Level.Lunatic,
            Level.Easy, Level.Normal, Level.Hard, Level.Lunatic, Level.Easy,
            Level.Normal, Level.Hard, Level.Lunatic, Level.Easy, Level.Normal,
            Level.Hard, Level.Lunatic, Level.Easy, Level.Normal, Level.Hard,
            Level.Lunatic, Level.Hard, Level.Lunatic, Level.Easy, Level.Normal,
            Level.Hard, Level.Lunatic, Level.Easy, Level.Normal, Level.Hard,
            Level.Lunatic, Level.Easy, Level.Normal, Level.Hard, Level.Lunatic,
            Level.Easy, Level.Normal, Level.Hard, Level.Lunatic, Level.Easy,
            Level.Normal, Level.Hard, Level.Lunatic, Level.Easy, Level.Normal,

            Level.Hard, Level.Lunatic, Level.Easy, Level.Normal, Level.Hard,
            Level.Lunatic, Level.Easy, Level.Normal, Level.Hard, Level.Lunatic,
            Level.Easy, Level.Normal, Level.Hard, Level.Lunatic, Level.Easy,
            Level.Normal, Level.Hard, Level.Lunatic, Level.Easy, Level.Normal,
            Level.Hard, Level.Lunatic, Level.Easy, Level.Normal, Level.Hard,
            Level.Lunatic, Level.Easy, Level.Normal, Level.Hard, Level.Lunatic,
            Level.Easy, Level.Normal, Level.Hard, Level.Lunatic, Level.Easy,
            Level.Normal, Level.Hard, Level.Lunatic, Level.Easy, Level.Normal,
            Level.Hard, Level.Lunatic, Level.Easy, Level.Normal, Level.Hard,
            Level.Lunatic, Level.Easy, Level.Normal, Level.Hard, Level.Lunatic,

            Level.Easy, Level.Normal, Level.Hard, Level.Lunatic, Level.Easy,
            Level.Normal, Level.Hard, Level.Lunatic, Level.Easy, Level.Normal,
            Level.Hard, Level.Lunatic, Level.Easy, Level.Normal, Level.Hard,
            Level.Lunatic, Level.Extra, Level.Extra, Level.Extra, Level.Extra,
            Level.Extra, Level.Extra, Level.Extra, Level.Extra, Level.Extra,
            Level.Extra, Level.Extra, Level.Extra, Level.Phantasm, Level.Phantasm,
            Level.Phantasm, Level.Phantasm, Level.Phantasm, Level.Phantasm, Level.Phantasm,
            Level.Phantasm, Level.Phantasm, Level.Phantasm, Level.Phantasm, Level.Phantasm,
            Level.Phantasm
        };

        private class CharaLevelPair : Pair<Chara, Level>
        {
            public Chara Chara { get { return this.First; } }
            public Level Level { get { return this.Second; } }

            public CharaLevelPair(Chara chara, Level level) : base(chara, level) { }
            public CharaLevelPair(CharaShort chara, LevelShort level) : base((Chara)chara, (Level)level) { }
        }

        private class AllScoreData
        {
            public Header header;
            public Dictionary<CharaLevelPair, List<HighScore>> rankings;
            public Dictionary<Chara, ClearData> clearData;
            public CardAttack[] cardAttacks;
            public Dictionary<CharaLevelPair, Dictionary<Stage, PracticeScore>> practiceScores;
            public PlayList playList;
            public LastName lastName;
            public VersionInfo versionInfo;

            public AllScoreData()
            {
                var numCharas = Enum.GetValues(typeof(Chara)).Length;
                this.rankings = new Dictionary<CharaLevelPair, List<HighScore>>();
                this.clearData = new Dictionary<Chara, ClearData>(numCharas);
                this.cardAttacks = new CardAttack[NumCards];
                this.practiceScores = new Dictionary<CharaLevelPair, Dictionary<Stage, PracticeScore>>();
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
            public uint Unknown { get; private set; }   // always 0x00000001?

            public Header(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "TH7K")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x000C)
                    throw new InvalidDataException("Size1");
                //if (this.Size2 != 0x000C)
                //    throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.Unknown = reader.ReadUInt32();
            }
        }

        private class HighScore : Chapter   // per character, level, rank
        {
            public uint Unknown1 { get; private set; }      // always 0x00000001?
            public uint Score { get; private set; }         // * 10
            public float SlowRate { get; private set; }
            public Chara Chara { get; private set; }        // size: 1Byte
            public Level Level { get; private set; }        // size: 1Byte
            public byte StageProgress { get; private set; } // 1..8: Stage1..Phantasm, 99: all clear
            public byte[] Name { get; private set; }        // .Length = 9, null-terminated
            public byte[] Date { get; private set; }        // .Length = 6, "mm/dd\0"
            public ushort ContinueCount { get; private set; }

            public HighScore(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "HSCR")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0028)
                    throw new InvalidDataException("Size1");
                //if (this.Size2 != 0x0028)
                //    throw new InvalidDataException("Size2");
            }

            public HighScore(uint score)    // for InitialRanking only
                : base()
            {
                this.Score = score;
                this.Name = Encoding.Default.GetBytes("--------\0");
                this.Date = Encoding.Default.GetBytes("--/--\0");
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
            }
        }

        private class ClearData : Chapter   // per character
        {
            public uint Unknown1 { get; private set; }          // always 0x00000001?
            public byte[] StoryFlags { get; private set; }      // [level]; really...?
            public byte[] PracticeFlags { get; private set; }   // [level]; really...?
            public Chara Chara { get; private set; }            // size: 4Bytes

            public ClearData(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "CLRD")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x001C)
                    throw new InvalidDataException("Size1");
                //if (this.Size2 != 0x001C)
                //    throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                var numLevels = Enum.GetValues(typeof(Level)).Length;
                this.Unknown1 = reader.ReadUInt32();
                this.StoryFlags = reader.ReadBytes(numLevels);
                this.PracticeFlags = reader.ReadBytes(numLevels);
                this.Chara = (Chara)reader.ReadInt32();
            }
        }

        private class CardAttack : Chapter      // per card
        {
            public uint Unknown1 { get; private set; }      // always 0x00000001?
            public Dictionary<CharaWithTotal, uint> MaxBonuses { get; private set; }
            public short Number { get; private set; }       // 0-origin
            public byte Unknown2 { get; private set; }      // variable...
            public byte[] CardName { get; private set; }    // .Length = 0x30
            public byte Unknown3 { get; private set; }      // always 0x00?
            public Dictionary<CharaWithTotal, ushort> TrialCounts { get; private set; }
            public Dictionary<CharaWithTotal, ushort> ClearCounts { get; private set; }

            public CardAttack(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "CATK")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0078)
                    throw new InvalidDataException("Size1");
                //if (this.Size2 != 0x0078)
                //    throw new InvalidDataException("Size2");

                var numCharas = Enum.GetValues(typeof(CharaWithTotal)).Length;
                this.MaxBonuses = new Dictionary<CharaWithTotal, uint>(numCharas);
                this.TrialCounts = new Dictionary<CharaWithTotal, ushort>(numCharas);
                this.ClearCounts = new Dictionary<CharaWithTotal, ushort>(numCharas);
            }

            public override void ReadFrom(BinaryReader reader)
            {
                var charas = Enum.GetValues(typeof(CharaWithTotal));
                this.Unknown1 = reader.ReadUInt32();
                foreach (CharaWithTotal chara in charas)
                    this.MaxBonuses.Add(chara, reader.ReadUInt32());
                this.Number = reader.ReadInt16();
                this.Unknown2 = reader.ReadByte();
                this.CardName = reader.ReadBytes(0x30);
                this.Unknown3 = reader.ReadByte();
                foreach (CharaWithTotal chara in charas)
                    this.TrialCounts.Add(chara, reader.ReadUInt16());
                foreach (CharaWithTotal chara in charas)
                    this.ClearCounts.Add(chara, reader.ReadUInt16());
            }

            public bool hasTried()
            {
                return Encoding.Default.GetString(this.CardName).TrimEnd('\0').Length > 0;
            }
        }

        private class PracticeScore : Chapter   // per character, level, stage
        {
            public uint Unknown1 { get; private set; }      // always 0x00000001?
            public int TrialCount { get; private set; }     // really...?
            public int HighScore { get; private set; }
            public Chara Chara { get; private set; }        // size: 1Byte
            public Level Level { get; private set; }        // size: 1Byte
            public Stage Stage { get; private set; }        // size: 1Byte
            public byte Unknown2 { get; private set; }      // always 0x00?

            public PracticeScore(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "PSCR")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0018)
                    throw new InvalidDataException("Size1");
                //if (this.Size2 != 0x0018)
                //    throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.Unknown1 = reader.ReadUInt32();
                this.TrialCount = reader.ReadInt32();
                this.HighScore = reader.ReadInt32();
                this.Chara = (Chara)reader.ReadByte();
                this.Level = (Level)reader.ReadByte();
                this.Stage = (Stage)reader.ReadByte();
                this.Unknown2 = reader.ReadByte();
            }
        }

        private class PlayList : Chapter
        {
            public uint Unknown { get; private set; }               // always 0x00000001?
            public Time TotalRunningTime { get; private set; }
            public Time TotalPlayTime { get; private set; }
            public Dictionary<LevelWithTotal, PlayCount> PlayCounts { get; private set; }

            public PlayList(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "PLST")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0160)
                    throw new InvalidDataException("Size1");
                //if (this.Size2 != 0x0160)
                //    throw new InvalidDataException("Size2");

                this.PlayCounts =
                    new Dictionary<LevelWithTotal, PlayCount>(Enum.GetValues(typeof(LevelWithTotal)).Length);
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.Unknown = reader.ReadUInt32();
                this.TotalRunningTime = new Time(
                    reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), false);
                this.TotalPlayTime = new Time(
                    reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), false);
                foreach (LevelWithTotal level in Enum.GetValues(typeof(LevelWithTotal)))
                {
                    var playCount = new PlayCount();
                    playCount.ReadFrom(reader);
                    if (!this.PlayCounts.ContainsKey(level))
                        this.PlayCounts.Add(level, playCount);
                }
            }
        }

        private class PlayCount : Utils.IBinaryReadable     // per level-with-total
        {
            public int TotalTrial { get; private set; }
            public Dictionary<Chara, int> Trials { get; private set; }
            public int TotalRetry { get; private set; }
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
                this.TotalRetry = reader.ReadInt32();
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
                //if (this.Size2 != 0x0018)
                //    throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.Unknown = reader.ReadUInt32();
                this.Name = reader.ReadBytes(12);
            }
        }

        private class VersionInfo : Chapter
        {
            public ushort Unknown1 { get; private set; }    // always 0x0001?
            public ushort Unknown2 { get; private set; }    // variable...
            public byte[] Version { get; private set; }     // .Length = 6, null-terminated
            public byte[] Unknown3 { get; private set; }    // .Length = 3
            public byte[] Unknown4 { get; private set; }    // .Length = 3
            public uint Unknown5 { get; private set; }      // variable...

            public VersionInfo(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "VRSM")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x001C)
                    throw new InvalidDataException("Size1");
                //if (this.Size2 != 0x001C)
                //    throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.Unknown1 = reader.ReadUInt16();
                this.Unknown2 = reader.ReadUInt16();
                this.Version = reader.ReadBytes(6);
                this.Unknown3 = reader.ReadBytes(3);
                this.Unknown4 = reader.ReadBytes(3);
                this.Unknown5 = reader.ReadUInt32();
            }
        }

        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get
            {
                return "1.00b";
            }
        }

        public Th07Converter() { }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th07decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
            var data = new byte[size];
            input.Read(data, 0, size);

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

            output.Write(data, 0, size);

            return (ushort)checksum == BitConverter.ToUInt16(data, 2);
        }

        private bool Extract(Stream input, Stream output)
        {
            var reader = new BinaryReader(input);

            var unknown1 = reader.ReadUInt16();
            var checksum = reader.ReadUInt16();     // already checked by this.Decrypt()
            var version = reader.ReadInt16();
            if (version != 0x0B)
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
                        case "TH7K":
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
                            // th07.exe does something more things here...
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

            allScoreData.rankings = new Dictionary<CharaLevelPair, List<HighScore>>();
            reader.ReadBytes(0x1C);

            try
            {
                while (true)
                {
                    chapter.ReadFrom(reader);
                    switch (chapter.Signature)
                    {
                        case "TH7K":
                            {
                                var header = new Header(chapter);
                                header.ReadFrom(reader);
                                allScoreData.header = header;
                            }
                            break;

                        case "HSCR":
                            {
                                var score = new HighScore(chapter);
                                score.ReadFrom(reader);
                                var key = new CharaLevelPair(score.Chara, score.Level);
                                if (!allScoreData.rankings.ContainsKey(key))
                                    allScoreData.rankings.Add(key, new List<HighScore>(InitialRanking));
                                var ranking = allScoreData.rankings[key];
                                ranking.Add(score);
                                ranking.Sort(
                                    new Comparison<HighScore>((lhs, rhs) => rhs.Score.CompareTo(lhs.Score)));
                                ranking.RemoveAt(ranking.Count - 1);
                            }
                            break;

                        case "CLRD":
                            {
                                var clearData = new ClearData(chapter);
                                clearData.ReadFrom(reader);
                                if (!allScoreData.clearData.ContainsKey(clearData.Chara))
                                    allScoreData.clearData.Add(clearData.Chara, clearData);
                            }
                            break;

                        case "CATK":
                            {
                                var attack = new CardAttack(chapter);
                                attack.ReadFrom(reader);
                                allScoreData.cardAttacks[attack.Number] = attack;
                            }
                            break;

                        case "PSCR":
                            {
                                var score = new PracticeScore(chapter);
                                score.ReadFrom(reader);
                                if ((score.Level != Level.Extra) && (score.Level != Level.Phantasm) &&
                                    (score.Stage != Stage.Extra) && (score.Stage != Stage.Phantasm))
                                {
                                    var key = new CharaLevelPair(score.Chara, score.Level);
                                    if (!allScoreData.practiceScores.ContainsKey(key))
                                        allScoreData.practiceScores.Add(
                                            key, new Dictionary<Stage, PracticeScore>());
                                    var scores = allScoreData.practiceScores[key];
                                    if (!scores.ContainsKey(score.Stage))
                                        scores.Add(score.Stage, score);
                                }
                            }
                            break;

                        case "PLST":
                            {
                                var playList = new PlayList(chapter);
                                playList.ReadFrom(reader);
                                allScoreData.playList = playList;
                            }
                            break;

                        case "LSNM":
                            {
                                var lastName = new LastName(chapter);
                                lastName.ReadFrom(reader);
                                allScoreData.lastName = lastName;
                            }
                            break;

                        case "VRSM":
                            {
                                var versionInfo = new VersionInfo(chapter);
                                versionInfo.ReadFrom(reader);
                                allScoreData.versionInfo = versionInfo;
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

            if ((allScoreData.header != null) &&
                //(allScoreData.rankings.Count >= 0) &&
                (allScoreData.clearData.Count == Enum.GetValues(typeof(Chara)).Length) &&
                //(allScoreData.cardAttacks.Length == NumCards) &&
                //(allScoreData.practiceScores.Count >= 0) &&
                (allScoreData.playList != null) &&
                (allScoreData.lastName != null) &&
                (allScoreData.versionInfo != null))
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

        // %T07SCR[w][xx][y][z]
        private string ReplaceScore(string input)
        {
            var pattern = string.Format(
                @"%T07SCR([{0}])({1})(\d)([1-5])",
                Utils.JoinEnumNames<LevelShort>(""),
                Utils.JoinEnumNames<CharaShort>("|"));
            return new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(input, new MatchEvaluator(match =>
                {
                    try
                    {
                        var level = Utils.ParseEnum<LevelShort>(match.Groups[1].Value, true);
                        var chara = Utils.ParseEnum<CharaShort>(match.Groups[2].Value, true);
                        var rank = (int.Parse(match.Groups[3].Value) + 9) % 10;     // 1..9,0 -> 0..9
                        var type = int.Parse(match.Groups[4].Value);

                        var key = new CharaLevelPair(chara, level);
                        var score = this.allScoreData.rankings.ContainsKey(key)
                            ? this.allScoreData.rankings[key][rank] : InitialRanking[rank];

                        switch (type)
                        {
                            case 1:     // name
                                return Encoding.Default.GetString(score.Name).Split('\0')[0];
                            case 2:     // score
                                return (score.Score * 10 + score.ContinueCount).ToString();
                            case 3:     // stage
                                if (score.StageProgress == 99)
                                    return "All Clear";
                                else if (score.StageProgress < StageProgressArray.Length)
                                    return StageProgressArray[score.StageProgress];
                                else
                                    return StageProgressArray[0];
                            case 4:     // date
                                return Encoding.Default.GetString(score.Date);
                            case 5:     // slow rate
                                return score.SlowRate.ToString("F3") + "%";
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

        // %T07C[xxx][yy][z]
        private string ReplaceCareer(string input)
        {
            var pattern = string.Format(
                @"%T07C(\d\d\d)({0})([1-3])",
                Utils.JoinEnumNames<CharaShortWithTotal>("|"));
            return new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(input, new MatchEvaluator(match =>
                {
                    try
                    {
                        var number = int.Parse(match.Groups[1].Value);
                        var chara =
                            (CharaWithTotal)Utils.ParseEnum<CharaShortWithTotal>(match.Groups[2].Value, true);
                        var type = int.Parse(match.Groups[3].Value);

                        if (number == 0)
                            switch (type)
                            {
                                case 1:     // MaxBonus
                                    return Utils.Accumulate<CardAttack>(
                                        this.allScoreData.cardAttacks, new Converter<CardAttack, uint>(
                                            attack => ((attack != null) ? attack.MaxBonuses[chara] : 0)))
                                            .ToString();
                                case 2:     // clear count
                                    return Utils.Accumulate<CardAttack>(
                                        this.allScoreData.cardAttacks, new Converter<CardAttack, int>(
                                            attack => ((attack != null) ? attack.ClearCounts[chara] : 0)))
                                            .ToString();
                                case 3:     // trial count
                                    return Utils.Accumulate<CardAttack>(
                                        this.allScoreData.cardAttacks, new Converter<CardAttack, int>(
                                            attack => ((attack != null) ? attack.TrialCounts[chara] : 0)))
                                            .ToString();
                                default:    // unreachable
                                    return match.ToString();
                            }
                        else if ((0 < number) && (number <= NumCards))
                        {
                            var attack = this.allScoreData.cardAttacks[number - 1];
                            if (attack != null)
                                switch (type)
                                {
                                    case 1:     // MaxBonus
                                        return attack.MaxBonuses[chara].ToString();
                                    case 2:     // clear count
                                        return attack.ClearCounts[chara].ToString();
                                    case 3:     // trial count
                                        return attack.TrialCounts[chara].ToString();
                                    default:    // unreachable
                                        return match.ToString();
                                }
                            else
                                return "0";
                        }
                        else
                            return match.ToString();
                    }
                    catch
                    {
                        return match.ToString();
                    }
                }));
        }

        // %T07CARD[xxx][y]
        private string ReplaceCard(string input)
        {
            return new Regex(@"%T07CARD(\d{3})([NR])", RegexOptions.IgnoreCase)
                .Replace(input, new MatchEvaluator(match =>
                {
                    try
                    {
                        var number = int.Parse(match.Groups[1].Value);
                        var type = match.Groups[2].Value.ToUpper();

                        if ((0 < number) && (number <= NumCards))
                        {
                            var attack = this.allScoreData.cardAttacks[number - 1];
                            if (type == "N")
                                return ((attack != null) && attack.hasTried())
                                    ? Encoding.Default.GetString(attack.CardName) : "??????????";
                            else
                                return ((attack != null) && attack.hasTried())
                                    ? CardLevelTable[attack.Number].ToString() : "?????";
                        }
                        else
                            return match.ToString();
                    }
                    catch
                    {
                        return match.ToString();
                    }
                }));
        }

        // %T07CRG[w][xx][yy][z]
        private string ReplaceCollectRate(string input)
        {
            var pattern = string.Format(
                @"%T07CRG([{0}])({1})([0-6])([12])",
                Utils.JoinEnumNames<LevelShortWithTotal>(""),
                Utils.JoinEnumNames<CharaShortWithTotal>("|"));
            return new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(input, new MatchEvaluator(match =>
                {
                    try
                    {
                        var level =
                            (LevelWithTotal)Utils.ParseEnum<LevelShortWithTotal>(match.Groups[1].Value, true);
                        var chara =
                            (CharaWithTotal)Utils.ParseEnum<CharaShortWithTotal>(match.Groups[2].Value, true);
                        var stage = int.Parse(match.Groups[3].Value);
                        var type = int.Parse(match.Groups[4].Value);

                        Predicate<CardAttack> findCard = (attack => false);

                        if (level == LevelWithTotal.Total)
                        {
                            if (stage == 0)     // total
                            {
                                if (type == 1)
                                    findCard = (attack =>
                                        ((attack != null) && (attack.ClearCounts[chara] > 0)));
                                else
                                    findCard = (attack =>
                                        ((attack != null) && (attack.TrialCounts[chara] > 0)));
                            }
                            else
                            {
                                var st = (Stage)(stage - 1);
                                if (type == 1)
                                    findCard = (attack =>
                                        ((attack != null) &&
                                        StageCardTable[st].Contains(attack.Number) &&
                                        (attack.ClearCounts[chara] > 0)));
                                else
                                    findCard = (attack =>
                                        ((attack != null) &&
                                        StageCardTable[st].Contains(attack.Number) &&
                                        (attack.TrialCounts[chara] > 0)));
                            }
                        }
                        else if ((level == LevelWithTotal.Extra) || (level == LevelWithTotal.Phantasm))
                        {
                            var st = (level == LevelWithTotal.Extra) ? Stage.Extra : Stage.Phantasm;
                            if (type == 1)
                                findCard = (attack =>
                                    ((attack != null) &&
                                    StageCardTable[st].Contains(attack.Number) &&
                                    (attack.ClearCounts[chara] > 0)));
                            else
                                findCard = (attack =>
                                    ((attack != null) &&
                                    StageCardTable[st].Contains(attack.Number) &&
                                    (attack.TrialCounts[chara] > 0)));
                        }
                        else
                        {
                            var lv = (Level)level;
                            if (stage > 0)
                            {
                                if (type == 1)
                                    findCard = (attack =>
                                        ((attack != null) &&
                                        (CardLevelTable[attack.Number] == lv) &&
                                        (attack.ClearCounts[chara] > 0)));
                                else
                                    findCard = (attack =>
                                        ((attack != null) &&
                                        (CardLevelTable[attack.Number] == lv) &&
                                        (attack.TrialCounts[chara] > 0)));
                            }
                            else
                            {
                                if (type == 1)
                                    findCard = (attack =>
                                        ((attack != null) &&
                                        (CardLevelTable[attack.Number] == lv) &&
                                        (attack.ClearCounts[chara] > 0)));
                                else
                                    findCard = (attack =>
                                        ((attack != null) &&
                                        (CardLevelTable[attack.Number] == lv) &&
                                        (attack.TrialCounts[chara] > 0)));
                            }
                        }

                        return Utils.CountIf<CardAttack>(this.allScoreData.cardAttacks, findCard).ToString();
                    }
                    catch
                    {
                        return match.ToString();
                    }
                }));
        }

        // %T07CLEAR[x][yy]
        private string ReplaceClear(string input)
        {
            var pattern = string.Format(
                @"%T07CLEAR([{0}])({1})",
                Utils.JoinEnumNames<LevelShort>(""),
                Utils.JoinEnumNames<CharaShort>("|"));
            return new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(input, new MatchEvaluator(match =>
                {
                    try
                    {
                        var level = Utils.ParseEnum<LevelShort>(match.Groups[1].Value, true);
                        var chara = Utils.ParseEnum<CharaShort>(match.Groups[2].Value, true);

                        var key = new CharaLevelPair(chara, level);
                        if (this.allScoreData.rankings.ContainsKey(key))
                        {
                            var stageProgress = 0;
                            foreach (var rank in this.allScoreData.rankings[key])
                                stageProgress = Math.Max(stageProgress, rank.StageProgress);
                            if ((stageProgress == (int)Stage.Extra + 1) ||
                                (stageProgress == (int)Stage.Phantasm + 1))
                                return "Not Clear";
                            else if (stageProgress < StageProgressArray.Length)
                                return StageProgressArray[stageProgress];
                            else if (stageProgress == 99)
                                return "All Clear";
                            else
                                return "-------";
                        }
                        else
                            return "-------";
                    }
                    catch
                    {
                        return match.ToString();
                    }
                }));
        }

        // %T07PLAY[x][yy]
        private string ReplacePlay(string input)
        {
            var pattern = string.Format(
                @"%T07PLAY([{0}])({1}|CL|CN|PR|RT)",
                Utils.JoinEnumNames<LevelShortWithTotal>(""),
                Utils.JoinEnumNames<CharaShortWithTotal>("|"));
            return new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(input, new MatchEvaluator(match =>
                {
                    try
                    {
                        var level = Utils.ParseEnum<LevelShortWithTotal>(match.Groups[1].Value, true);
                        var charaAndMore = match.Groups[2].Value.ToUpper();

                        var playCount = this.allScoreData.playList.PlayCounts[(LevelWithTotal)level];
                        switch (charaAndMore)
                        {
                            case "CL":  // clear count
                                return playCount.TotalClear.ToString();
                            case "CN":  // continue count
                                return playCount.TotalContinue.ToString();
                            case "PR":  // practice count
                                return playCount.TotalPractice.ToString();
                            case "RT":  // retry count
                                return playCount.TotalRetry.ToString();
                            default:
                                var chara = Utils.ParseEnum<CharaShortWithTotal>(match.Groups[2].Value, true);
                                return (chara == CharaShortWithTotal.TL)
                                    ? playCount.TotalTrial.ToString()
                                    : playCount.Trials[(Chara)chara].ToString();
                        }
                    }
                    catch
                    {
                        return match.ToString();
                    }
                }));
        }

        // %T07TIME(ALL|PLY)
        private string ReplaceTime(string input)
        {
            return new Regex(@"%T07TIME(ALL|PLY)", RegexOptions.IgnoreCase)
                .Replace(input, new MatchEvaluator(match =>
                {
                    var kind = match.Groups[1].Value.ToUpper();

                    return (kind == "ALL")
                        ? this.allScoreData.playList.TotalRunningTime.ToLongString()
                        : this.allScoreData.playList.TotalPlayTime.ToLongString();
                }));
        }

        // %T07PRAC[w][xx][y][z]
        private string ReplacePractice(string input)
        {
            var pattern = string.Format(
                @"%T07PRAC([{0}])({1})([1-6])([12])",
                Utils.JoinEnumNames<LevelShort>(""),
                Utils.JoinEnumNames<CharaShort>("|"));
            return new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(input, new MatchEvaluator(match =>
                {
                    try
                    {
                        var level = Utils.ParseEnum<LevelShort>(match.Groups[1].Value, true);
                        var chara = Utils.ParseEnum<CharaShort>(match.Groups[2].Value, true);
                        var stage = (Stage)(int.Parse(match.Groups[3].Value) - 1);
                        var type = int.Parse(match.Groups[4].Value);

                        if ((level == LevelShort.X) || (level == LevelShort.P))
                            return match.ToString();

                        var key = new CharaLevelPair(chara, level);
                        if (this.allScoreData.practiceScores.ContainsKey(key))
                        {
                            var scores = this.allScoreData.practiceScores[key];
                            if (type == 1)
                                return (scores.ContainsKey(stage)
                                    ? (scores[stage].HighScore * 10) : 0).ToString();
                            else
                                return (scores.ContainsKey(stage) ? scores[stage].TrialCount : 0).ToString();
                        }
                        else
                            return "0";
                    }
                    catch
                    {
                        return match.ToString();
                    }
                }));
        }
    }
}
