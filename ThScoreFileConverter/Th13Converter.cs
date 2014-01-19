using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter
{
    public class Th13Converter : ThConverter
    {
        private enum Level                          { Easy, Normal, Hard, Lunatic, Extra }
        private enum LevelWithTotal                 { Easy, Normal, Hard, Lunatic, Extra, Total }
        private enum LevelPractice                  { Easy, Normal, Hard, Lunatic, Extra, OverDrive }
        private enum LevelPracticeWithTotal         { Easy, Normal, Hard, Lunatic, Extra, OverDrive, Total }
        private enum LevelShort                     { E, N, H, L, X }
        private enum LevelShortWithTotal            { E, N, H, L, X, T }
        private enum LevelShortPractice             { E, N, H, L, X, D }
        private enum LevelShortPracticeWithTotal    { E, N, H, L, X, D, T }

        private enum Chara
        {
            Reimu, Marisa, Sanae, Youmu
        }
        private enum CharaWithTotal
        {
            Reimu, Marisa, Sanae, Youmu, Total
        }
        private enum CharaShort             { RM, MR, SN, YM }
        private enum CharaShortWithTotal    { RM, MR, SN, YM, TL }

        private enum Stage          { Stage1, Stage2, Stage3, Stage4, Stage5, Stage6, Extra }
        private enum StagePractice  { Stage1, Stage2, Stage3, Stage4, Stage5, Stage6, Extra, OverDrive }

        private static readonly string[] StageProgressArray =
        {
            "-------", "Stage 1", "Stage 2", "Stage 3", "Stage 4", "Stage 5", "Stage 6",
            "Not Clear", "All Clear"
        };

        private const int NumCards = 127;

        // Thanks to thwiki.info
        private static readonly Dictionary<StagePractice, Range<int>> StageCardTable =
            new Dictionary<StagePractice, Range<int>>()
            {
                { StagePractice.Stage1,    new Range<int>{ Min = 0,   Max = 13  } },
                { StagePractice.Stage2,    new Range<int>{ Min = 14,  Max = 29  } },
                { StagePractice.Stage3,    new Range<int>{ Min = 30,  Max = 43  } },
                { StagePractice.Stage4,    new Range<int>{ Min = 44,  Max = 58  } },
                { StagePractice.Stage5,    new Range<int>{ Min = 59,  Max = 77  } },
                { StagePractice.Stage6,    new Range<int>{ Min = 78,  Max = 105 } },
                { StagePractice.Extra,     new Range<int>{ Min = 106, Max = 118 } },
                { StagePractice.OverDrive, new Range<int>{ Min = 119, Max = 126 } }
            };

        private class LevelStagePair : Pair<LevelPractice, StagePractice>
        {
            public LevelPractice Level { get { return this.First; } }
            public StagePractice Stage { get { return this.Second; } }

            public LevelStagePair(LevelPractice level, StagePractice stage) : base(level, stage) { }
        }

        private class AllScoreData
        {
            public Header header;
            public Dictionary<CharaWithTotal, ClearData> clearData;
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
                this.Signature = new string(reader.ReadChars(4));
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
            public uint Checksum { get; private set; }
            public int Size { get; private set; }

            public Chapter() { }
            public Chapter(Chapter ch)
            {
                this.Signature = ch.Signature;
                this.Unknown = ch.Unknown;
                this.Checksum = ch.Checksum;
                this.Size = ch.Size;
            }

            public virtual void ReadFrom(BinaryReader reader)
            {
                this.Signature = Encoding.Default.GetString(reader.ReadBytes(2));
                this.Unknown = reader.ReadUInt16();
                this.Checksum = reader.ReadUInt32();
                this.Size = reader.ReadInt32();
            }
        }

        private class ClearData : Chapter   // per character
        {
            public CharaWithTotal Chara { get; private set; }   // size: 4Bytes
            public Dictionary<LevelPractice, ScoreData[]> Rankings { get; private set; }
            public byte[] Unknown1 { get; private set; }        // .Length = 0x118
            public int TotalPlayCount { get; private set; }
            public int PlayTime { get; private set; }           // = seconds * 60fps
            public Dictionary<LevelPractice, int> ClearCounts { get; private set; }
            public byte[] Unknown2 { get; private set; }        // .Length = 0x04
            public Dictionary<Level, int> ClearFlags { get; private set; }  // Really...?
            public byte[] Unknown3 { get; private set; }        // .Length = 0x08
            public Dictionary<LevelStagePair, Practice> Practices { get; private set; }
            public SpellCard[] Cards { get; private set; }      // [0..126]

            public ClearData(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "CR")
                    throw new InvalidDataException("Signature");
                if (this.Unknown != 0x0001)
                    throw new InvalidDataException("Unknown");
                if (this.Size != 0x000056DC)
                    throw new InvalidDataException("Size");

                var numLevels = Enum.GetValues(typeof(Level)).Length;
                var numLevelsPractice = Enum.GetValues(typeof(LevelPractice)).Length;
                this.Rankings = new Dictionary<LevelPractice, ScoreData[]>(numLevelsPractice);
                this.ClearCounts = new Dictionary<LevelPractice, int>(numLevelsPractice);
                this.ClearFlags = new Dictionary<Level, int>(numLevels);
                this.Practices = new Dictionary<LevelStagePair, Practice>();
                this.Cards = new SpellCard[NumCards];
            }

            public override void ReadFrom(BinaryReader reader)
            {
                var levels = Enum.GetValues(typeof(LevelPractice));
                this.Chara = (CharaWithTotal)reader.ReadInt32();
                foreach (LevelPractice level in levels)
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
                this.Unknown1 = reader.ReadBytes(0x118);
                this.TotalPlayCount = reader.ReadInt32();
                this.PlayTime = reader.ReadInt32();
                foreach (LevelPractice level in levels)
                {
                    var clearCount = reader.ReadInt32();
                    if (!this.ClearCounts.ContainsKey(level))
                        this.ClearCounts.Add(level, clearCount);
                }
                this.Unknown2 = reader.ReadBytes(0x04);
                foreach (Level level in Enum.GetValues(typeof(Level)))
                {
                    var clearFlag = reader.ReadInt32();
                    if (!this.ClearFlags.ContainsKey(level))
                        this.ClearFlags.Add(level, clearFlag);
                }
                this.Unknown3 = reader.ReadBytes(0x08);
                foreach (LevelPractice level in levels)
                    foreach (StagePractice stage in Enum.GetValues(typeof(StagePractice)))
                    {
                        var practice = new Practice();
                        practice.ReadFrom(reader);
                        var key = new LevelStagePair(level, stage);
                        if (!this.Practices.ContainsKey(key))
                            this.Practices.Add(key, practice);
                    }
                for (var number = 0; number < NumCards; number++)
                {
                    var card = new SpellCard();
                    card.ReadFrom(reader);
                    this.Cards[number] = card;
                }
            }
        }

        private class Status : Chapter
        {
            public byte[] LastName { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)
            public byte[] Unknown1 { get; private set; }    // .Length = 0x10
            public byte[] BgmFlags { get; private set; }    // .Length = 17
            public byte[] Unknown2 { get; private set; }    // .Length = 0x11
            public int TotalPlayTime;                       // unit: 10ms
            public byte[] Unknown3 { get; private set; }    // .Length = 0x03E0

            public Status(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "ST")
                    throw new InvalidDataException("Signature");
                if (this.Unknown != 0x0001)
                    throw new InvalidDataException("Unknown");
                if (this.Size != 0x0000042C)
                    throw new InvalidDataException("Size");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.LastName = reader.ReadBytes(10);
                this.Unknown1 = reader.ReadBytes(0x10);
                this.BgmFlags = reader.ReadBytes(17);
                this.Unknown2 = reader.ReadBytes(0x11);
                this.TotalPlayTime = reader.ReadInt32();
                this.Unknown3 = reader.ReadBytes(0x03E0);
            }
        }

        private class ScoreData : Utils.IBinaryReadable
        {
            public uint Score { get; private set; }         // * 10
            public byte StageProgress { get; private set; }
            public byte ContinueCount { get; private set; }
            public byte[] Name { get; private set; }        // .Length = 10 (The last 2 bytes are always 0x00 ?)
            public uint DateTime { get; private set; }      // UNIX time (unit: [s])
            public float SlowRate { get; private set; }     // really...?
            public uint Unknown1 { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.Score = reader.ReadUInt32();
                this.StageProgress = reader.ReadByte();
                this.ContinueCount = reader.ReadByte();
                this.Name = reader.ReadBytes(10);
                this.DateTime = reader.ReadUInt32();
                this.SlowRate = reader.ReadSingle();
                this.Unknown1 = reader.ReadUInt32();
            }
        }

        private class SpellCard : Utils.IBinaryReadable
        {
            public byte[] Name { get; private set; }            // .Length = 0x80
            public int ClearCount { get; private set; }
            public int PracticeClearCount { get; private set; }
            public int TrialCount { get; private set; }
            public int PracticeTrialCount { get; private set; }
            public int Number { get; private set; }             // 0-origin
            public LevelPractice Level { get; private set; }    // Easy .. OverDrive
            public int PracticeScore { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.Name = reader.ReadBytes(0x80);
                this.ClearCount = reader.ReadInt32();
                this.PracticeClearCount = reader.ReadInt32();
                this.TrialCount = reader.ReadInt32();
                this.PracticeTrialCount = reader.ReadInt32();
                this.Number = reader.ReadInt32();
                this.Level = (LevelPractice)reader.ReadInt32();
                this.PracticeScore = reader.ReadInt32();
            }
        }

        private class Practice : Utils.IBinaryReadable
        {
            public uint Score { get; private set; }         // * 10
            public byte ClearFlag { get; private set; }     // 0x00: Not clear, 0x01: Cleared
            public byte EnableFlag { get; private set; }    // 0x00: Disable, 0x01: Enable
            public ushort Unknown1 { get; private set; }    // always 0x0000?

            public void ReadFrom(BinaryReader reader)
            {
                this.Score = reader.ReadUInt32();
                this.ClearFlag = reader.ReadByte();
                this.EnableFlag = reader.ReadByte();
                this.Unknown1 = reader.ReadUInt16();
            }
        }

        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get
            {
                return "1.00c";
            }
        }

        public Th13Converter() { }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th13decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
            if (header.Signature != "TH31")
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
                    chapter.ReadFrom(reader);

                    if (!((chapter.Signature == "CR") && (chapter.Unknown == 0x0001)) &&
                        !((chapter.Signature == "ST") && (chapter.Unknown == 0x0001)))
                        return false;

                    // -4 means the size of Size.
                    reader.BaseStream.Seek(-4, SeekOrigin.Current);
                    // 8 means the total size of Signature, Unknown, and Checksum.
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

        private AllScoreData Read(Stream input)
        {
            var reader = new BinaryReader(input);
            var allScoreData = new AllScoreData();
            var chapter = new Chapter();
            var numCharas = Enum.GetValues(typeof(CharaWithTotal)).Length;

            allScoreData.clearData = new Dictionary<CharaWithTotal, ClearData>(numCharas);
            allScoreData.header = new Header();
            allScoreData.header.ReadFrom(reader);

            try
            {
                while (true)
                {
                    chapter.ReadFrom(reader);
                    switch (chapter.Signature)
                    {
                        case "CR":
                            var clearData = new ClearData(chapter);
                            clearData.ReadFrom(reader);
                            if (!allScoreData.clearData.ContainsKey(clearData.Chara))
                                allScoreData.clearData.Add(clearData.Chara, clearData);
                            break;
                        case "ST":
                            var status = new Status(chapter);
                            status.ReadFrom(reader);
                            allScoreData.status = status;
                            break;
                        default:
                            // 12 means the total size of Signature, Unknown, Checksum, and Size.
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
                (allScoreData.clearData.Count == numCharas) &&
                (allScoreData.status != null))
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
            var pattern = string.Format(
                @"%T13SCR([{0}])({1})(\d)([1-5])",
                Utils.JoinEnumNames<LevelShort>(""),
                Utils.JoinEnumNames<CharaShort>("|"));
            return new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(input, Utils.ToNothrowEvaluator(match =>
                {
                    var level = (LevelPractice)Utils.ParseEnum<LevelShort>(match.Groups[1].Value, true);
                    var chara = (CharaWithTotal)Utils.ParseEnum<CharaShort>(match.Groups[2].Value, true);
                    var rank = (int.Parse(match.Groups[3].Value) + 9) % 10;     // from [1..9, 0] to [0..9]
                    var type = int.Parse(match.Groups[4].Value);

                    var ranking = this.allScoreData.clearData[chara].Rankings[level][rank];
                    switch (type)
                    {
                        case 1:     // name
                            return Encoding.Default.GetString(ranking.Name).Split('\0')[0];
                        case 2:     // score
                            return this.ToNumberString(ranking.Score * 10 + ranking.ContinueCount);
                        case 3:     // stage
                            if (ranking.DateTime > 0)
                                return (ranking.StageProgress < StageProgressArray.Length)
                                    ? StageProgressArray[ranking.StageProgress]
                                    : StageProgressArray[0];
                            else
                                return StageProgressArray[0];
                        case 4:     // date & time
                            if (ranking.DateTime > 0)
                                return new DateTime(1970, 1, 1).AddSeconds(ranking.DateTime)
                                    .ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
                            else
                                return "----/--/-- --:--:--";
                        case 5:     // slow
                            if (ranking.DateTime > 0)
                                return ranking.SlowRate.ToString("F3") + "%";
                            else
                                return "-----%";
                        default:    // unreachable
                            return match.ToString();
                    }
                }));
        }

        // %T13C[w][xxx][yy][z]
        private string ReplaceCareer(string input)
        {
            var pattern = string.Format(
                @"%T13C([SP])(\d\d\d)({0})([12])",
                Utils.JoinEnumNames<CharaShortWithTotal>("|"));
            return new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(input, Utils.ToNothrowEvaluator(match =>
                {
                    var kind = match.Groups[1].Value.ToUpper();
                    var number = int.Parse(match.Groups[2].Value);
                    var chara = Utils.ParseEnum<CharaShortWithTotal>(match.Groups[3].Value, true);
                    var type = int.Parse(match.Groups[4].Value);

                    var cards = this.allScoreData.clearData[(CharaWithTotal)chara].Cards;
                    if (number == 0)
                    {
                        if (type == 1)
                            return this.ToNumberString((kind == "S")
                                ? cards.Sum(card => card.ClearCount)
                                : cards.Sum(card => card.PracticeClearCount));
                        else
                            return this.ToNumberString((kind == "S")
                                ? cards.Sum(card => card.TrialCount)
                                : cards.Sum(card => card.PracticeTrialCount));
                    }
                    else if ((0 < number) && (number <= NumCards))
                    {
                        if (type == 1)
                            return this.ToNumberString((kind == "S")
                                ? cards[number - 1].ClearCount : cards[number - 1].PracticeClearCount);
                        else
                            return this.ToNumberString((kind == "S")
                                ? cards[number - 1].TrialCount : cards[number - 1].PracticeTrialCount);
                    }
                    else
                        return match.ToString();
                }));
        }

        // %T13CARD[xxx][y]
        private string ReplaceCard(string input)
        {
            return new Regex(@"%T13CARD(\d{3})([NR])", RegexOptions.IgnoreCase)
                .Replace(input, Utils.ToNothrowEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value);
                    var type = match.Groups[2].Value.ToUpper();

                    if ((0 < number) && (number <= NumCards))
                    {
                        var card = this.allScoreData.clearData[CharaWithTotal.Total].Cards[number - 1];
                        if (type == "N")
                        {
                            var name = Encoding.Default.GetString(card.Name).TrimEnd('\0');
                            return (name.Length > 0) ? name : "??????????";
                        }
                        else
                            return (card.Level == LevelPractice.OverDrive)
                                ? "Over Drive" : card.Level.ToString();
                    }
                    else
                        return match.ToString();
                }));
        }

        // %T13CRG[v][w][xx][y][z]
        private string ReplaceCollectRate(string input)
        {
            var pattern = string.Format(
                @"%T13CRG([SP])([{0}])({1})([0-6])([12])",
                Utils.JoinEnumNames<LevelShortPracticeWithTotal>(""),
                Utils.JoinEnumNames<CharaShortWithTotal>("|"));
            return new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(input, Utils.ToNothrowEvaluator(match =>
                {
                    var kind = match.Groups[1].Value.ToUpper();
                    var level = Utils.ParseEnum<LevelShortPracticeWithTotal>(match.Groups[2].Value, true);
                    var chara = Utils.ParseEnum<CharaShortWithTotal>(match.Groups[3].Value, true);
                    var stage = int.Parse(match.Groups[4].Value);   // 0: total of all stages
                    var type = int.Parse(match.Groups[5].Value);

                    Func<SpellCard, bool> findByKindType = (card => true);
                    Func<SpellCard, bool> findByLevel = (card => true);
                    Func<SpellCard, bool> findByStage = (card => true);

                    if (kind == "S")
                    {
                        if (type == 1)
                            findByKindType = (card => (card.ClearCount > 0));
                        else
                            findByKindType = (card => (card.TrialCount > 0));
                    }
                    else
                    {
                        if (type == 1)
                            findByKindType = (card => (card.PracticeClearCount > 0));
                        else
                            findByKindType = (card => (card.PracticeTrialCount > 0));
                    }

                    if (stage == 0)
                    {
                        // Do nothing
                    }
                    else
                        findByStage = (card =>
                            StageCardTable[(StagePractice)(stage - 1)].Contains(card.Number));

                    switch (level)
                    {
                        case LevelShortPracticeWithTotal.T:
                            // Do nothing
                            break;
                        case LevelShortPracticeWithTotal.X:
                            findByStage = (card =>
                                StageCardTable[StagePractice.Extra].Contains(card.Number));
                            break;
                        case LevelShortPracticeWithTotal.D:
                            findByStage = (card =>
                                StageCardTable[StagePractice.OverDrive].Contains(card.Number));
                            break;
                        default:
                            findByLevel = (card => (card.Level == (LevelPractice)level));
                            break;
                    }

                    return this.allScoreData.clearData[(CharaWithTotal)chara].Cards.Count(
                        new Utils.And<SpellCard>(findByKindType, findByLevel, findByStage)).ToString();
                }));
        }

        // %T13CLEAR[x][yy]
        private string ReplaceClear(string input)
        {
            var pattern = string.Format(
                @"%T13CLEAR([{0}])({1})",
                Utils.JoinEnumNames<LevelShort>(""),
                Utils.JoinEnumNames<CharaShort>("|"));
            return new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(input, Utils.ToNothrowEvaluator(match =>
                {
                    var level = (LevelPractice)Utils.ParseEnum<LevelShort>(match.Groups[1].Value, true);
                    var chara = (CharaWithTotal)Utils.ParseEnum<CharaShort>(match.Groups[2].Value, true);

                    var stageProgress = 0;
                    for (var rank = 0; rank < 10; rank++)
                    {
                        var ranking = this.allScoreData.clearData[chara].Rankings[level][rank];
                        if (ranking.DateTime > 0)
                            stageProgress = Math.Max(stageProgress, ranking.StageProgress);
                    }

                    if (stageProgress < StageProgressArray.Length)
                    {
                        if (level != LevelPractice.Extra)
                            return (stageProgress != 7)
                                ? StageProgressArray[stageProgress] : StageProgressArray[0];
                        else
                            return (stageProgress >= 7)
                                ? StageProgressArray[stageProgress] : StageProgressArray[0];
                    }
                    else
                        return StageProgressArray[0];
                }));
        }

        // %T13CHARA[xx][y]
        private string ReplaceChara(string input)
        {
            var pattern = string.Format(
                @"%T13CHARA({0})([1-3])",
                Utils.JoinEnumNames<CharaShortWithTotal>("|"));
            return new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(input, Utils.ToNothrowEvaluator(match =>
                {
                    var chara =
                        (CharaWithTotal)Utils.ParseEnum<CharaShortWithTotal>(match.Groups[1].Value, true);
                    var type = int.Parse(match.Groups[2].Value);

                    switch (type)
                    {
                        case 1:     // total play count
                            if (chara == CharaWithTotal.Total)
                                return this.ToNumberString(
                                    this.allScoreData.clearData.Values.Sum(
                                        data => (data.Chara != chara) ? data.TotalPlayCount : 0));
                            else
                                return this.ToNumberString(
                                    this.allScoreData.clearData[chara].TotalPlayCount);
                        case 2:     // play times
                            {
                                var frames = (chara == CharaWithTotal.Total)
                                    ? this.allScoreData.clearData.Values.Sum(
                                        data => (data.Chara != chara) ? (long)data.PlayTime : 0L)
                                    : (long)this.allScoreData.clearData[chara].PlayTime;
                                return new Time(frames).ToString();
                            }
                        case 3:     // clear count
                            if (chara == CharaWithTotal.Total)
                                return this.ToNumberString(
                                    this.allScoreData.clearData.Values.Sum(
                                        data => (data.Chara != chara)
                                            ? data.ClearCounts.Values.Sum(count => count) : 0));
                            else
                                return this.ToNumberString(
                                    this.allScoreData.clearData[chara].ClearCounts.Values.Sum(count => count));
                        default:    // unreachable
                            return match.ToString();
                    }
                }));
        }

        // %T13CHARAEX[x][yy][z]
        private string ReplaceCharaEx(string input)
        {
            var pattern = string.Format(
                @"%T13CHARAEX([{0}])({1})([1-3])",
                Utils.JoinEnumNames<LevelShortWithTotal>(""),
                Utils.JoinEnumNames<CharaShortWithTotal>("|"));
            return new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(input, Utils.ToNothrowEvaluator(match =>
                {
                    var level =
                        (LevelWithTotal)Utils.ParseEnum<LevelShortWithTotal>(match.Groups[1].Value, true);
                    var chara =
                        (CharaWithTotal)Utils.ParseEnum<CharaShortWithTotal>(match.Groups[2].Value, true);
                    var type = int.Parse(match.Groups[3].Value);

                    switch (type)
                    {
                        case 1:     // total play count
                            if (chara == CharaWithTotal.Total)
                                return this.ToNumberString(
                                    this.allScoreData.clearData.Values.Sum(
                                        data => (data.Chara != chara) ? data.TotalPlayCount : 0));
                            else
                                return this.allScoreData.clearData[chara].TotalPlayCount.ToString();
                        case 2:     // play times
                            {
                                var frames = (chara == CharaWithTotal.Total)
                                    ? this.allScoreData.clearData.Values.Sum(
                                        data => (data.Chara != chara) ? (long)data.PlayTime : 0L)
                                    : (long)this.allScoreData.clearData[chara].PlayTime;
                                return new Time(frames).ToString();
                            }
                        case 3:     // clear count
                            if (chara == CharaWithTotal.Total)
                            {
                                if (level == LevelWithTotal.Total)
                                    return this.ToNumberString(
                                        this.allScoreData.clearData.Values.Sum(
                                            data => (data.Chara != chara)
                                                ? data.ClearCounts.Values.Sum(count => count) : 0));
                                else
                                    return this.ToNumberString(
                                        this.allScoreData.clearData.Values.Sum(
                                            data => (data.Chara != chara)
                                                ? data.ClearCounts[(LevelPractice)level] : 0));
                            }
                            else
                            {
                                if (level == LevelWithTotal.Total)
                                    return this.ToNumberString(
                                        this.allScoreData.clearData[chara].ClearCounts.Values.Sum(count => count));
                                else
                                    return this.ToNumberString(this.allScoreData.
                                        clearData[chara].ClearCounts[(LevelPractice)level]);
                            }
                        default:    // unreachable
                            return match.ToString();
                    }
                }));
        }

        // %T13PRAC[x][yy][z]
        private string ReplacePractice(string input)
        {
            var pattern = string.Format(
                @"%T13PRAC([{0}])({1})([1-6])",
                Utils.JoinEnumNames<LevelShort>(""),
                Utils.JoinEnumNames<CharaShort>("|"));
            return new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(input, Utils.ToNothrowEvaluator(match =>
                {
                    var level = (LevelPractice)Utils.ParseEnum<LevelShort>(match.Groups[1].Value, true);
                    var chara = (CharaWithTotal)Utils.ParseEnum<CharaShort>(match.Groups[2].Value, true);
                    var stage = (StagePractice)(int.Parse(match.Groups[3].Value) - 1);

                    if (level == LevelPractice.Extra)
                        return match.ToString();

                    if (this.allScoreData.clearData.ContainsKey(chara))
                    {
                        var key = new LevelStagePair(level, stage);
                        var practices = this.allScoreData.clearData[chara].Practices;
                        return this.ToNumberString(
                            practices.ContainsKey(key) ? (practices[key].Score * 10) : 0);
                    }
                    else
                        return "0";
                }));
        }
    }
}
