//-----------------------------------------------------------------------
// <copyright file="Th09Converter.cs" company="None">
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.OrderingRules",
        "SA1201:ElementsMustAppearInTheCorrectOrder",
        Justification = "Reviewed.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.SpacingRules",
        "SA1025:CodeMustNotContainMultipleWhitespaceInARow",
        Justification = "Reviewed.")]
    internal class Th09Converter : ThConverter
    {
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

        private class CharaLevelPair : Pair<Chara, Level>
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public Chara Chara { get { return this.First; } }

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public Level Level { get { return this.Second; } }

            public CharaLevelPair(Chara chara, Level level) : base(chara, level) { }
        }

        private class AllScoreData
        {
            public Header Header { get; set; }
            public Dictionary<CharaLevelPair, HighScore[]> Rankings { get; set; }
            public PlayStatus PlayStatus { get; set; }
            public LastName LastName { get; set; }
            public VersionInfo VersionInfo { get; set; }

            public AllScoreData()
            {
                this.Rankings = new Dictionary<CharaLevelPair, HighScore[]>();
            }
        }

        private class Chapter : IBinaryReadable
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
            public Header(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "TH9K")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x000C)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x000C)
                //     throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                reader.ReadByte();      // always 0x01?
                reader.ReadBytes(3);
            }
        }

        private class HighScore : Chapter   // per character, level, rank
        {
            public uint Score { get; private set; }         // * 10
            public Chara Chara { get; private set; }        // size: 1Byte
            public Level Level { get; private set; }        // size: 1Byte
            public short Rank { get; private set; }         // 0-based
            public byte[] Name { get; private set; }        // .Length = 9, null-terminated
            public byte[] Date { get; private set; }        // .Length = 9, "yy/mm/dd\0"
            public byte ContinueCount { get; private set; }

            public HighScore(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "HSCR")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x002C)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x002C)
                //     throw new InvalidDataException("Size2");
            }

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
            public Time TotalRunningTime { get; private set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public Time TotalPlayTime { get; private set; }     // really...?

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte[] BgmFlags { get; private set; }        // .Length = 19

            public Dictionary<Chara, byte> MatchFlags { get; private set; }
            public Dictionary<Chara, byte> StoryFlags { get; private set; }
            public Dictionary<Chara, byte> ExtraFlags { get; private set; }
            public Dictionary<Chara, ClearCount> ClearCounts { get; private set; }

            public PlayStatus(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "PLST")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x01FC)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x01FC)
                //     throw new InvalidDataException("Size2");

                var numCharas = Enum.GetValues(typeof(Chara)).Length;
                this.MatchFlags = new Dictionary<Chara, byte>(numCharas);
                this.StoryFlags = new Dictionary<Chara, byte>(numCharas);
                this.ExtraFlags = new Dictionary<Chara, byte>(numCharas);
                this.ClearCounts = new Dictionary<Chara, ClearCount>(numCharas);
            }

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

        private class LastName : Chapter
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
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
                reader.ReadUInt32();    // always 0x00000001?
                this.Name = reader.ReadBytes(12);
            }
        }

        private class VersionInfo : Chapter
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte[] Version { get; private set; }     // .Length = 6, null-terminated

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
                reader.ReadUInt16();    // always 0x0001?
                reader.ReadUInt16();    // always 0x0018?
                this.Version = reader.ReadBytes(6);
                reader.ReadBytes(3);    // always 0x000049?
                reader.ReadBytes(3);
                reader.ReadUInt32();
            }
        }

        private class ClearCount : IBinaryReadable
        {
            public Dictionary<Level, int> Counts { get; private set; }

            public ClearCount()
            {
                this.Counts = new Dictionary<Level, int>(Enum.GetValues(typeof(Level)).Length);
            }

            public void ReadFrom(BinaryReader reader)
            {
                foreach (var level in Utils.GetEnumerator<Level>())
                    this.Counts.Add(level, reader.ReadInt32());
                reader.ReadUInt32();
            }
        }

        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get { return "1.50a"; }
        }

        private static readonly string LevelPattern;
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
        static Th09Converter()
        {
            var levels = Utils.GetEnumerator<Level>();
            var charas = Utils.GetEnumerator<Chara>();

            LevelPattern = string.Join(string.Empty, levels.Select(lv => lv.ToShortName()).ToArray());
            CharaPattern = string.Join("|", charas.Select(ch => ch.ToShortName()).ToArray());

            ToLevel = ((shortName, comparisonType) =>
                levels.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToChara = ((shortName, comparisonType) =>
                charas.First(ch => ch.ToShortName().Equals(shortName, comparisonType)));
        }

        public Th09Converter()
        {
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

        private static bool Decrypt(Stream input, Stream output)
        {
            input.Seek(0, SeekOrigin.End);
            var endPos = input.Position;
            input.Seek(0, SeekOrigin.Begin);
            var size = (int)(endPos - input.Position);
            ThCrypt.Decrypt(input, output, size, 0x3A, 0xCD, 0x0100, 0x0C00);

            byte[] data = new byte[size];
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
            output.Write(data, 0, (int)size);

            return (ushort)checksum == BitConverter.ToUInt16(data, 2);
        }

        private static bool Extract(Stream input, Stream output)
        {
            input.Seek(0, SeekOrigin.End);
            var endPos = input.Position;
            input.Seek(0, SeekOrigin.Begin);
            var encodedSize = (int)(endPos - input.Position);

            var reader = new BinaryReader(input);

            reader.ReadUInt16();    // Unknown1
            reader.ReadUInt16();    // Checksum; already checked by this.Decrypt()
            var version = reader.ReadInt16();
            if (version != 4)
                return false;

            reader.ReadUInt16();    // Unknown2
            var headerSize = reader.ReadInt32();
            if (headerSize != 0x18)
                return false;

            var decodedSize = reader.ReadInt32();
            reader.ReadInt32();     // DecodedBodySize
            var encodedBodySize = reader.ReadInt32();
            if (encodedBodySize != (encodedSize - headerSize))
                return false;

            byte[] header = new byte[headerSize];
            input.Seek(0, SeekOrigin.Begin);
            input.Read(header, 0, headerSize);
            output.Write(header, 0, headerSize);

            Lzss.Extract(input, output);
            output.Flush();
            output.SetLength(output.Position);

            return output.Position == decodedSize;
        }

        private static bool Validate(Stream input)
        {
            var reader = new BinaryReader(input);
            var chapter = new Chapter();

            reader.ReadBytes(8);
            var headerSize = reader.ReadInt32();
            var decodedSize = reader.ReadInt32();
            var remainSize = decodedSize - headerSize;
            reader.ReadBytes(8);
            if (remainSize <= 0)
                return false;

            while (remainSize > 0)
            {
                chapter.ReadFrom(reader);
                if (chapter.Size1 == 0)
                    return false;
                if (chapter.Signature == "TH9K")
                {
                    var temp = reader.ReadByte();
                    // 8 means the total size of Signature, Size1 and Size2.
                    reader.ReadBytes(chapter.Size1 - 8 - 1);
                    if (temp != 0x01)
                        return false;
                }
                else
                    reader.ReadBytes(chapter.Size1 - 8);    // 8 means the same above
                remainSize -= chapter.Size1;
            }

            return true;
        }

        private static AllScoreData Read(Stream input)
        {
            var reader = new BinaryReader(input);
            var allScoreData = new AllScoreData();
            var chapter = new Chapter();

            reader.ReadBytes(0x18);

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

        protected override void Convert(Stream input, Stream output)
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

        // %T09SCR[w][xx][y][z]
        private string ReplaceScore(string input)
        {
            var pattern = Utils.Format(@"%T09SCR([{0}])({1})([1-5])([1-3])", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
                var chara = ToChara(match.Groups[2].Value, StringComparison.OrdinalIgnoreCase);
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
        private string ReplaceClear(string input)
        {
            var pattern = Utils.Format(@"%T09CLEAR([{0}])({1})([12])", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
                var chara = ToChara(match.Groups[2].Value, StringComparison.OrdinalIgnoreCase);
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
    }
}
