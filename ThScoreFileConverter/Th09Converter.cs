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

            var allLines = reader.ReadToEnd();
            allLines = this.ReplaceScore(allLines);
            allLines = this.ReplaceTime(allLines);
            allLines = this.ReplaceClear(allLines);

            writer.Write(allLines);
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

                    switch (chapter.Signature)
                    {
                        case Header.ValidSignature:
                            if (chapter.FirstByteOfData != 0x01)
                                return false;
                            break;
                        default:
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

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        private static AllScoreData Read(Stream input)
        {
            var dictionary = new Dictionary<string, Action<AllScoreData, Chapter>>
            {
                { Header.ValidSignature,        (data, ch) => data.Set(new Header(ch))        },
                { HighScore.ValidSignature,     (data, ch) => data.Set(new HighScore(ch))     },
                { PlayStatus.ValidSignature,    (data, ch) => data.Set(new PlayStatus(ch))    },
                { LastName.ValidSignature,      (data, ch) => data.Set(new LastName(ch))      },
                { VersionInfo.ValidSignature,   (data, ch) => data.Set(new VersionInfo(ch))   }
            };

            var reader = new BinaryReader(input);
            var allScoreData = new AllScoreData();
            var chapter = new Chapter();

            reader.ReadBytes(FileHeader.ValidSize);

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

        private class FileHeader : IBinaryReadable, IBinaryWritable
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

            public Header Header { get; private set; }

            public Dictionary<CharaLevelPair, HighScore[]> Rankings { get; private set; }

            public PlayStatus PlayStatus { get; private set; }

            public LastName LastName { get; private set; }

            public VersionInfo VersionInfo { get; private set; }

            public void Set(Header header)
            {
                this.Header = header;
            }

            public void Set(HighScore score)
            {
                var key = new CharaLevelPair(score.Chara, score.Level);
                if (!this.Rankings.ContainsKey(key))
                    this.Rankings.Add(key, new HighScore[5]);
                if ((0 <= score.Rank) && (score.Rank < 5))
                    this.Rankings[key][score.Rank] = score;
            }

            public void Set(PlayStatus status)
            {
                this.PlayStatus = status;
            }

            public void Set(LastName name)
            {
                this.LastName = name;
            }

            public void Set(VersionInfo info)
            {
                this.VersionInfo = info;
            }
        }

        private class Chapter : IBinaryReadable
        {
            public Chapter()
            {
                this.Signature = string.Empty;
                this.Size1 = 0;
                this.Size2 = 0;
                this.Data = new byte[] { };
            }

            protected Chapter(Chapter chapter)
            {
                this.Signature = chapter.Signature;
                this.Size1 = chapter.Size1;
                this.Size2 = chapter.Size2;
                this.Data = new byte[chapter.Data.Length];
                chapter.Data.CopyTo(this.Data, 0);
            }

            public string Signature { get; private set; }   // .Length = 4

            public short Size1 { get; private set; }

            public short Size2 { get; private set; }        // always equal to size1?

            public byte FirstByteOfData
            {
                get { return this.Data[0]; }
            }

            protected byte[] Data { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.Signature = Encoding.Default.GetString(reader.ReadBytes(4));
                this.Size1 = reader.ReadInt16();
                this.Size2 = reader.ReadInt16();
                this.Data = reader.ReadBytes(this.Size1 - this.Signature.Length - (sizeof(short) * 2));
            }
        }

        private class Header : Chapter
        {
            public const string ValidSignature = "TH9K";
            public const short ValidSize = 0x000C;

            public Header(Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Size1 != ValidSize)
                    throw new InvalidDataException("Size1");

                using (var stream = new MemoryStream(this.Data, false))
                using (var reader = new BinaryReader(stream))
                {
                    reader.ReadByte();      // always 0x01?
                    reader.ReadBytes(3);
                }
            }
        }

        private class HighScore : Chapter   // per character, level, rank
        {
            public const string ValidSignature = "HSCR";
            public const short ValidSize = 0x002C;

            public HighScore(Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Size1 != ValidSize)
                    throw new InvalidDataException("Size1");

                using (var stream = new MemoryStream(this.Data, false))
                using (var reader = new BinaryReader(stream))
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

            public uint Score { get; private set; }         // * 10

            public Chara Chara { get; private set; }        // size: 1Byte

            public Level Level { get; private set; }        // size: 1Byte

            public short Rank { get; private set; }         // 0-based

            public byte[] Name { get; private set; }        // .Length = 9, null-terminated

            public byte[] Date { get; private set; }        // .Length = 9, "yy/mm/dd\0"

            public byte ContinueCount { get; private set; }
        }

        private class PlayStatus : Chapter
        {
            public const string ValidSignature = "PLST";
            public const short ValidSize = 0x01FC;

            public PlayStatus(Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Size1 != ValidSize)
                    throw new InvalidDataException("Size1");

                var charas = Utils.GetEnumerator<Chara>();
                var numCharas = charas.Count();
                this.MatchFlags = new Dictionary<Chara, byte>(numCharas);
                this.StoryFlags = new Dictionary<Chara, byte>(numCharas);
                this.ExtraFlags = new Dictionary<Chara, byte>(numCharas);
                this.ClearCounts = new Dictionary<Chara, ClearCount>(numCharas);

                using (var stream = new MemoryStream(this.Data, false))
                using (var reader = new BinaryReader(stream))
                {
                    reader.ReadUInt32();    // always 0x00000003?
                    var hours = reader.ReadInt32();
                    var minutes = reader.ReadInt32();
                    var seconds = reader.ReadInt32();
                    var milliseconds = reader.ReadInt32();
                    this.TotalRunningTime = new Time(hours, minutes, seconds, milliseconds, false);
                    hours = reader.ReadInt32();
                    minutes = reader.ReadInt32();
                    seconds = reader.ReadInt32();
                    milliseconds = reader.ReadInt32();
                    this.TotalPlayTime = new Time(hours, minutes, seconds, milliseconds, false);
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

            public Time TotalRunningTime { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Time TotalPlayTime { get; private set; }     // really...?

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] BgmFlags { get; private set; }        // .Length = 19

            public Dictionary<Chara, byte> MatchFlags { get; private set; }

            public Dictionary<Chara, byte> StoryFlags { get; private set; }

            public Dictionary<Chara, byte> ExtraFlags { get; private set; }

            public Dictionary<Chara, ClearCount> ClearCounts { get; private set; }
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
            public const string ValidSignature = "LSNM";
            public const short ValidSize = 0x0018;

            public LastName(Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Size1 != ValidSize)
                    throw new InvalidDataException("Size1");

                using (var stream = new MemoryStream(this.Data, false))
                using (var reader = new BinaryReader(stream))
                {
                    reader.ReadUInt32();    // always 0x00000001?
                    this.Name = reader.ReadBytes(12);
                }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] Name { get; private set; }    // .Length = 12, null-terminated
        }

        private class VersionInfo : Chapter
        {
            public const string ValidSignature = "VRSM";
            public const short ValidSize = 0x001C;

            public VersionInfo(Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Size1 != ValidSize)
                    throw new InvalidDataException("Size1");

                using (var stream = new MemoryStream(this.Data, false))
                using (var reader = new BinaryReader(stream))
                {
                    reader.ReadUInt16();    // always 0x0001?
                    reader.ReadUInt16();    // always 0x0018?
                    this.Version = reader.ReadBytes(6);
                    reader.ReadBytes(3);    // always 0x000049?
                    reader.ReadBytes(3);
                    reader.ReadUInt32();
                }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] Version { get; private set; }     // .Length = 6, null-terminated
        }
    }
}
