//-----------------------------------------------------------------------
// <copyright file="Th09Converter.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed.")]

namespace ThScoreFileConverter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class Th09Converter : ThConverter
    {
        private static readonly string LevelPattern;
        private static readonly string CharaPattern;

        private static readonly Func<string, Level> ToLevel;
        private static readonly Func<string, Chara> ToChara;

        private AllScoreData allScoreData = null;

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
        static Th09Converter()
        {
            var levels = Utils.GetEnumerator<Level>();
            var charas = Utils.GetEnumerator<Chara>();

            LevelPattern = string.Join(string.Empty, levels.Select(lv => lv.ToShortName()).ToArray());
            CharaPattern = string.Join("|", charas.Select(ch => ch.ToShortName()).ToArray());

            var comparisonType = StringComparison.OrdinalIgnoreCase;

            ToLevel = (shortName => levels.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToChara = (shortName => charas.First(ch => ch.ToShortName().Equals(shortName, comparisonType)));
        }

        public Th09Converter()
        {
        }

        public enum Level
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra
        }

        public enum Chara
        {
            [EnumAltName("RM")] Reimu,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("SK")] Sakuya,
            [EnumAltName("YM")] Youmu,
            [EnumAltName("RS")] Reisen,
            [EnumAltName("CI")] Cirno,
            [EnumAltName("LY")] Lyrica,
            [EnumAltName("MY")] Mystia,
            [EnumAltName("TW")] Tewi,
            [EnumAltName("YU")] Yuuka,
            [EnumAltName("AY")] Aya,
            [EnumAltName("MD")] Medicine,
            [EnumAltName("KM")] Komachi,
            [EnumAltName("SI")] Shikieiki,
            [EnumAltName("ML")] Merlin,
            [EnumAltName("LN")] Lunasa
        }

        public override string SupportedVersions
        {
            get { return "1.50a"; }
        }

        public override bool HasCardReplacer
        {
            get { return false; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th09decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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

        protected override void Convert(Stream input, Stream output, bool hideUntriedCards)
        {
            var reader = new StreamReader(input, Encoding.GetEncoding("shift_jis"));
            var writer = new StreamWriter(output, Encoding.GetEncoding("shift_jis"));

            var allLine = reader.ReadToEnd();
            allLine = this.ReplaceScore(allLine);
            allLine = this.ReplaceTime(allLine);
            allLine = this.ReplaceClear(allLine);

            writer.Write(allLine);
            writer.Flush();
            writer.BaseStream.SetLength(writer.BaseStream.Position);
        }

        private static bool Decrypt(Stream input, Stream output)
        {
            var size = (int)input.Length;
            ThCrypt.Decrypt(input, output, size, 0x3A, 0xCD, 0x0100, 0x0C00);

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

        private static bool Extract(Stream input, Stream output)
        {
            var reader = new BinaryReader(input);
            var writer = new BinaryWriter(output);
            var header = new FileHeader();

            header.ReadFrom(reader);
            if (!header.IsValid)
                return false;
            if (header.Size + header.EncodedBodySize != input.Length)
                return false;

            header.WriteTo(writer);

            Lzss.Extract(input, output);
            output.Flush();
            output.SetLength(output.Position);

            return output.Position == header.DecodedAllSize;
        }

        private static bool Validate(Stream input)
        {
            var reader = new BinaryReader(input);
            var header = new FileHeader();
            var chapter = new Chapter();

            header.ReadFrom(reader);
            var remainSize = header.DecodedAllSize - header.Size;
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
                        case "TH9K":
                            temp = reader.ReadByte();
                            //// 8 means the total size of Signature, Size1 and Size2.
                            reader.ReadBytes(chapter.Size1 - 8 - 1);
                            if (temp != 0x01)
                                return false;
                            break;
                        default:
                            reader.ReadBytes(chapter.Size1 - 8);    // 8 means the same above
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

        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1513:ClosingCurlyBracketMustBeFollowedByBlankLine", Justification = "Reviewed.")]
        private static AllScoreData Read(Stream input)
        {
            var reader = new BinaryReader(input);
            var allScoreData = new AllScoreData();
            var chapter = new Chapter();

            reader.ReadBytes(FileHeader.ValidSize);

            try
            {
                while (true)
                {
                    chapter.ReadFrom(reader);
                    switch (chapter.Signature)
                    {
                        case "TH9K":
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
                                    allScoreData.Rankings.Add(key, new HighScore[5]);
                                if ((0 <= score.Rank) && (score.Rank < 5))
                                    allScoreData.Rankings[key][score.Rank] = score;
                            }
                            break;

                        case "PLST":
                            {
                                var status = new PlayStatus(chapter);
                                status.ReadFrom(reader);
                                allScoreData.PlayStatus = status;
                            }
                            break;

                        case "LSNM":
                            {
                                var lastName = new LastName(chapter);
                                lastName.ReadFrom(reader);
                                allScoreData.LastName = lastName;
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
                            // 8 means the total size of Signature, Size1 and Size2.
                            reader.ReadBytes(chapter.Size1 - 8);
                            break;
                    }
                }
            }
            catch (EndOfStreamException)
            {
                // It's OK, do nothing.
            }

            var numCharas = Enum.GetValues(typeof(Chara)).Length;
            var numLevels = Enum.GetValues(typeof(Level)).Length;
            if ((allScoreData.Header != null) &&
                (allScoreData.Rankings.Count == numCharas * numLevels) &&
                (allScoreData.PlayStatus != null) &&
                (allScoreData.LastName != null) &&
                (allScoreData.VersionInfo != null))
                return allScoreData;
            else
                return null;
        }

        // %T09SCR[w][xx][y][z]
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1513:ClosingCurlyBracketMustBeFollowedByBlankLine", Justification = "Reviewed.")]
        private string ReplaceScore(string input)
        {
            var pattern = Utils.Format(@"%T09SCR([{0}])({1})([1-5])([1-3])", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value);
                var chara = ToChara(match.Groups[2].Value);
                var rank = Utils.ToZeroBased(int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                var score = this.allScoreData.Rankings[new CharaLevelPair(chara, level)][rank];

                switch (type)
                {
                    case 1:     // name
                        return Encoding.Default.GetString(score.Name).Split('\0')[0];
                    case 2:     // score
                        return this.ToNumberString((score.Score * 10) + score.ContinueCount);
                    case 3:     // date
                        {
                            var date = Encoding.Default.GetString(score.Date).Split('\0')[0];
                            return (date != "--/--") ? date : "--/--/--";
                        }
                    default:    // unreachable
                        return match.ToString();
                }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T09TIMEALL
        private string ReplaceTime(string input)
        {
            var pattern = @"%T09TIMEALL";
            var evaluator = new MatchEvaluator(match =>
            {
                return this.allScoreData.PlayStatus.TotalRunningTime.ToLongString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T09CLEAR[x][yy][z]
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1513:ClosingCurlyBracketMustBeFollowedByBlankLine", Justification = "Reviewed.")]
        private string ReplaceClear(string input)
        {
            var pattern = Utils.Format(@"%T09CLEAR([{0}])({1})([12])", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value);
                var chara = ToChara(match.Groups[2].Value);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                var count = this.allScoreData.PlayStatus.ClearCounts[chara].Counts[level];

                switch (type)
                {
                    case 1:     // clear count
                        return this.ToNumberString(count);
                    case 2:     // clear flag
                        if (count > 0)
                            return "Cleared";
                        else
                        {
                            var score = this.allScoreData.Rankings[new CharaLevelPair(chara, level)][0];
                            var date = Encoding.Default.GetString(score.Date).TrimEnd('\0');
                            return (date != "--/--") ? "Not Cleared" : "-------";
                        }
                    default:    // unreachable
                        return match.ToString();
                }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        private class CharaLevelPair : Pair<Chara, Level>
        {
            public CharaLevelPair(Chara chara, Level level)
                : base(chara, level)
            {
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Chara Chara
            {
                get { return this.First; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Level Level
            {
                get { return this.Second; }
            }
        }

        private class FileHeader
        {
            public const short ValidVersion = 0x0004;
            public const int ValidSize = 0x00000018;

            private ushort unknown1;
            private ushort unknown2;

            public FileHeader()
            {
            }

            public ushort Checksum { get; private set; }

            public short Version { get; private set; }

            public int Size { get; private set; }

            public int DecodedAllSize { get; private set; }

            public int DecodedBodySize { get; private set; }

            public int EncodedBodySize { get; private set; }

            public bool IsValid
            {
                get
                {
                    return (this.Version == ValidVersion)
                        && (this.Size == ValidSize)
                        && (this.DecodedAllSize == this.Size + this.DecodedBodySize);
                }
            }

            public void ReadFrom(BinaryReader reader)
            {
                this.unknown1 = reader.ReadUInt16();
                this.Checksum = reader.ReadUInt16();
                this.Version = reader.ReadInt16();
                this.unknown2 = reader.ReadUInt16();
                this.Size = reader.ReadInt32();
                this.DecodedAllSize = reader.ReadInt32();
                this.DecodedBodySize = reader.ReadInt32();
                this.EncodedBodySize = reader.ReadInt32();
            }

            public void WriteTo(BinaryWriter writer)
            {
                writer.Write(this.unknown1);
                writer.Write(this.Checksum);
                writer.Write(this.Version);
                writer.Write(this.unknown2);
                writer.Write(this.Size);
                writer.Write(this.DecodedAllSize);
                writer.Write(this.DecodedBodySize);
                writer.Write(this.EncodedBodySize);
            }
        }

        private class AllScoreData
        {
            public AllScoreData()
            {
                var numPairs = Enum.GetValues(typeof(Chara)).Length * Enum.GetValues(typeof(Level)).Length;
                this.Rankings = new Dictionary<CharaLevelPair, HighScore[]>(numPairs);
            }

            public Header Header { get; set; }

            public Dictionary<CharaLevelPair, HighScore[]> Rankings { get; set; }

            public PlayStatus PlayStatus { get; set; }

            public LastName LastName { get; set; }

            public VersionInfo VersionInfo { get; set; }
        }

        private class Chapter : IBinaryReadable
        {
            public Chapter()
            {
            }

            public Chapter(Chapter ch)
            {
                this.Signature = ch.Signature;
                this.Size1 = ch.Size1;
                this.Size2 = ch.Size2;
            }

            public string Signature { get; private set; }   // .Length = 4

            public short Size1 { get; private set; }

            public short Size2 { get; private set; }        // always equal to size1?

            public virtual void ReadFrom(BinaryReader reader)
            {
                this.Signature = new string(reader.ReadChars(4));
                this.Size1 = reader.ReadInt16();
                this.Size2 = reader.ReadInt16();
            }
        }

        private class Header : Chapter
        {
            public Header(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "TH9K")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x000C)
                    throw new InvalidDataException("Size1");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadByte();      // always 0x01?
                reader.ReadBytes(3);
            }
        }

        private class HighScore : Chapter   // per character, level, rank
        {
            public HighScore(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "HSCR")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x002C)
                    throw new InvalidDataException("Size1");
            }

            public uint Score { get; private set; }         // * 10

            public Chara Chara { get; private set; }        // size: 1Byte

            public Level Level { get; private set; }        // size: 1Byte

            public short Rank { get; private set; }         // 0-based

            public byte[] Name { get; private set; }        // .Length = 9, null-terminated

            public byte[] Date { get; private set; }        // .Length = 9, "yy/mm/dd\0"

            public byte ContinueCount { get; private set; }

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadUInt32();    // always 0x00000002?
                this.Score = reader.ReadUInt32();
                reader.ReadUInt32();    // always 0x00000000?
                this.Chara = (Chara)reader.ReadByte();
                this.Level = (Level)reader.ReadByte();
                this.Rank = reader.ReadInt16();
                this.Name = reader.ReadBytes(9);
                this.Date = reader.ReadBytes(9);
                reader.ReadByte();      // always 0x00?
                this.ContinueCount = reader.ReadByte();
            }
        }

        private class PlayStatus : Chapter
        {
            public PlayStatus(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "PLST")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x01FC)
                    throw new InvalidDataException("Size1");

                var numCharas = Enum.GetValues(typeof(Chara)).Length;
                this.MatchFlags = new Dictionary<Chara, byte>(numCharas);
                this.StoryFlags = new Dictionary<Chara, byte>(numCharas);
                this.ExtraFlags = new Dictionary<Chara, byte>(numCharas);
                this.ClearCounts = new Dictionary<Chara, ClearCount>(numCharas);
            }

            public Time TotalRunningTime { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Time TotalPlayTime { get; private set; }     // really...?

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] BgmFlags { get; private set; }        // .Length = 19

            public Dictionary<Chara, byte> MatchFlags { get; private set; }

            public Dictionary<Chara, byte> StoryFlags { get; private set; }

            public Dictionary<Chara, byte> ExtraFlags { get; private set; }

            public Dictionary<Chara, ClearCount> ClearCounts { get; private set; }

            public override void ReadFrom(BinaryReader reader)
            {
                var charas = Utils.GetEnumerator<Chara>();
                reader.ReadUInt32();    // always 0x00000003?
                this.TotalRunningTime = new Time(
                    reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), false);
                this.TotalPlayTime = new Time(
                    reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), false);
                this.BgmFlags = reader.ReadBytes(19);
                reader.ReadBytes(13);
                foreach (var chara in charas)
                    this.MatchFlags.Add(chara, reader.ReadByte());
                foreach (var chara in charas)
                    this.StoryFlags.Add(chara, reader.ReadByte());
                foreach (var chara in charas)
                    this.ExtraFlags.Add(chara, reader.ReadByte());
                foreach (var chara in charas)
                {
                    var clearCount = new ClearCount();
                    clearCount.ReadFrom(reader);
                    this.ClearCounts.Add(chara, clearCount);
                }
            }
        }

        private class ClearCount : IBinaryReadable
        {
            public ClearCount()
            {
                this.Counts = new Dictionary<Level, int>(Enum.GetValues(typeof(Level)).Length);
            }

            public Dictionary<Level, int> Counts { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                foreach (var level in Utils.GetEnumerator<Level>())
                    this.Counts.Add(level, reader.ReadInt32());
                reader.ReadUInt32();
            }
        }

        private class LastName : Chapter
        {
            public LastName(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "LSNM")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0018)
                    throw new InvalidDataException("Size1");
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] Name { get; private set; }    // .Length = 12, null-terminated

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadUInt32();    // always 0x00000001?
                this.Name = reader.ReadBytes(12);
            }
        }

        private class VersionInfo : Chapter
        {
            public VersionInfo(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "VRSM")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x001C)
                    throw new InvalidDataException("Size1");
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] Version { get; private set; }     // .Length = 6, null-terminated

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadUInt16();    // always 0x0001?
                reader.ReadUInt16();    // always 0x0018?
                this.Version = reader.ReadBytes(6);
                reader.ReadBytes(3);    // always 0x000049?
                reader.ReadBytes(3);
                reader.ReadUInt32();
            }
        }
    }
}
