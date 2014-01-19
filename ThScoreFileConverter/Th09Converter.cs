using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter
{
    public class Th09Converter : ThConverter
    {
        private enum Level      { Easy, Normal, Hard, Lunatic, Extra }
        private enum LevelShort { E, N, H, L, X }

        private enum Chara
        {
            Reimu, Marisa, Sakuya, Youmu, Reisen, Cirno, Lyrica, Mystia,
            Tewi, Yuuka, Aya, Medicine, Komachi, Shikieiki, Merlin, Lunasa
        }
        private enum CharaShort { RM, MR, SK, YM, RS, CI, LY, MY, TW, YU, AY, MD, KM, SI, ML, LN }

        private class CharaLevelPair : Pair<Chara, Level>
        {
            public Chara Chara { get { return this.First; } }
            public Level Level { get { return this.Second; } }

            public CharaLevelPair(Chara chara, Level level) : base(chara, level) { }
        }

        private class AllScoreData
        {
            public Header header;
            public Dictionary<CharaLevelPair, HighScore[]> rankings;
            public PlayList playList;
            public LastName lastName;
            public VersionInfo versionInfo;

            public AllScoreData()
            {
                this.rankings = new Dictionary<CharaLevelPair, HighScore[]>();
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
            public byte Unknown1 { get; private set; }      // always 0x01?
            public byte[] Unknown2 { get; private set; }    // .Length = 3

            public Header(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "TH9K")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x000C)
                    throw new InvalidDataException("Size1");
                //if (this.Size2 != 0x000C)
                //    throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.Unknown1 = reader.ReadByte();
                this.Unknown2 = reader.ReadBytes(3);
            }
        }

        private class HighScore : Chapter   // per character, level, rank
        {
            public uint Unknown1 { get; private set; }      // always 0x00000002?
            public uint Score { get; private set; }         // * 10
            public uint Unknown2 { get; private set; }      // always 0x00000000?
            public Chara Chara { get; private set; }        // size: 1Byte
            public Level Level { get; private set; }        // size: 1Byte
            public short Rank { get; private set; }         // 0-origin
            public byte[] Name { get; private set; }        // .Length = 9, null-terminated
            public byte[] Date { get; private set; }        // .Length = 9, "yy/mm/dd\0"
            public byte Unknown3 { get; private set; }      // always 0x00?
            public byte ContinueCount { get; private set; }

            public HighScore(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "HSCR")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x002C)
                    throw new InvalidDataException("Size1");
                //if (this.Size2 != 0x002C)
                //    throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.Unknown1 = reader.ReadUInt32();
                this.Score = reader.ReadUInt32();
                this.Unknown2 = reader.ReadUInt32();
                this.Chara = (Chara)reader.ReadByte();
                this.Level = (Level)reader.ReadByte();
                this.Rank = reader.ReadInt16();
                this.Name = reader.ReadBytes(9);
                this.Date = reader.ReadBytes(9);
                this.Unknown3 = reader.ReadByte();
                this.ContinueCount = reader.ReadByte();
            }
        }

        private class PlayList : Chapter
        {
            public uint Unknown { get; private set; }           // always 0x00000003?
            public Time TotalRunningTime { get; private set; }
            public Time TotalPlayTime { get; private set; }     // really...?
            public byte[] BgmFlags { get; private set; }        // .Length = 19
            public byte[] Padding { get; private set; }         // .Length = 13
            public Dictionary<Chara, byte> MatchFlags { get; private set; }
            public Dictionary<Chara, byte> StoryFlags { get; private set; }
            public Dictionary<Chara, byte> ExtraFlags { get; private set; }
            public Dictionary<Chara, ClearCount> ClearCounts { get; private set; }

            public PlayList(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "PLST")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x01FC)
                    throw new InvalidDataException("Size1");
                //if (this.Size2 != 0x01FC)
                //    throw new InvalidDataException("Size2");

                var numCharas = Enum.GetValues(typeof(Chara)).Length;
                this.MatchFlags = new Dictionary<Chara, byte>(numCharas);
                this.StoryFlags = new Dictionary<Chara, byte>(numCharas);
                this.ExtraFlags = new Dictionary<Chara, byte>(numCharas);
                this.ClearCounts = new Dictionary<Chara, ClearCount>(numCharas);
            }

            public override void ReadFrom(BinaryReader reader)
            {
                var charas = Enum.GetValues(typeof(Chara));
                this.Unknown = reader.ReadUInt32();
                this.TotalRunningTime = new Time(
                    reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), false);
                this.TotalPlayTime = new Time(
                    reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), false);
                this.BgmFlags = reader.ReadBytes(19);
                this.Padding = reader.ReadBytes(13);
                foreach (Chara chara in charas)
                    this.MatchFlags.Add(chara, reader.ReadByte());
                foreach (Chara chara in charas)
                    this.StoryFlags.Add(chara, reader.ReadByte());
                foreach (Chara chara in charas)
                    this.ExtraFlags.Add(chara, reader.ReadByte());
                foreach (Chara chara in charas)
                {
                    var clearCount = new ClearCount();
                    clearCount.ReadFrom(reader);
                    this.ClearCounts.Add(chara, clearCount);
                }
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
            public ushort Unknown2 { get; private set; }    // always 0x0018?
            public byte[] Version { get; private set; }     // .Length = 6, null-terminated
            public byte[] Unknown3 { get; private set; }    // .Length = 3, always 0x000049?
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

        private class ClearCount : Utils.IBinaryReadable
        {
            public Dictionary<Level, int> Counts { get; private set; }
            public uint Unknown { get; private set; }

            public ClearCount()
            {
                this.Counts = new Dictionary<Level, int>(Enum.GetValues(typeof(Level)).Length);
            }

            public void ReadFrom(BinaryReader reader)
            {
                foreach (Level level in Enum.GetValues(typeof(Level)))
                    this.Counts.Add(level, reader.ReadInt32());
                this.Unknown = reader.ReadUInt32();
            }
        }

        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get
            {
                return "1.50a";
            }
        }

        public Th09Converter() { }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th09decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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

        private bool Extract(Stream input, Stream output)
        {
            input.Seek(0, SeekOrigin.End);
            var endPos = input.Position;
            input.Seek(0, SeekOrigin.Begin);
            var encodedSize = (int)(endPos - input.Position);

            var reader = new BinaryReader(input);

            var unknown1 = reader.ReadUInt16();
            var checksum = reader.ReadUInt16();     // already checked by this.Decrypt()
            var version = reader.ReadInt16();
            if (version != 4)
                return false;

            var unknown2 = reader.ReadUInt16();
            var headerSize = reader.ReadInt32();
            if (headerSize != 0x18)
                return false;

            var decodedSize = reader.ReadInt32();
            var decodedBodySize = reader.ReadInt32();
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

        private bool Validate(Stream input)
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

        private AllScoreData Read(Stream input)
        {
            var reader = new BinaryReader(input);
            var allScoreData = new AllScoreData();
            var chapter = new Chapter();
            var charas = Enum.GetValues(typeof(Chara));
            var levels = Enum.GetValues(typeof(Level));

            reader.ReadBytes(0x18);

            try
            {
                while (true)
                {
                    chapter.ReadFrom(reader);
                    switch (chapter.Signature)
                    {
                        case "TH9K":
                            var header = new Header(chapter);
                            header.ReadFrom(reader);
                            allScoreData.header = header;
                            break;

                        case "HSCR":
                            var score = new HighScore(chapter);
                            score.ReadFrom(reader);
                            var key = new CharaLevelPair(score.Chara, score.Level);
                            if (!allScoreData.rankings.ContainsKey(key))
                                allScoreData.rankings.Add(key, new HighScore[5]);
                            allScoreData.rankings[key][score.Rank] = score;
                            break;

                        case "PLST":
                            var playList = new PlayList(chapter);
                            playList.ReadFrom(reader);
                            allScoreData.playList = playList;
                            break;

                        case "LSNM":
                            var lastName = new LastName(chapter);
                            lastName.ReadFrom(reader);
                            allScoreData.lastName = lastName;
                            break;

                        case "VRSM":
                            var versionInfo = new VersionInfo(chapter);
                            versionInfo.ReadFrom(reader);
                            allScoreData.versionInfo = versionInfo;
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

            if ((allScoreData.header != null) &&
                (allScoreData.rankings.Count == (charas.Length * levels.Length)) &&
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
            allLine = this.ReplaceTime(allLine);
            allLine = this.ReplaceClear(allLine);
            writer.Write(allLine);

            writer.Flush();
            writer.BaseStream.SetLength(writer.BaseStream.Position);
        }

        // %T09SCR[w][xx][y][z]
        private string ReplaceScore(string input)
        {
            var pattern = string.Format(
                @"%T09SCR([{0}])({1})([1-5])([1-3])",
                Utils.JoinEnumNames<LevelShort>(""),
                Utils.JoinEnumNames<CharaShort>("|"));
            return new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(input, Utils.ToNothrowEvaluator(match =>
                {
                    var level = (Level)Enum.Parse(typeof(LevelShort), match.Groups[1].Value, true);
                    var chara = (Chara)Enum.Parse(typeof(CharaShort), match.Groups[2].Value, true);
                    var rank = int.Parse(match.Groups[3].Value) - 1;
                    var type = int.Parse(match.Groups[4].Value);
                    var score = this.allScoreData.rankings[new CharaLevelPair(chara, level)][rank];

                    switch (type)
                    {
                        case 1:     // name
                            return Encoding.Default.GetString(score.Name).Split('\0')[0];
                        case 2:     // score
                            return this.ToNumberString(score.Score * 10 + score.ContinueCount);
                        case 3:     // date
                            var date = Encoding.Default.GetString(score.Date).Split('\0')[0];
                            return (date != "--/--") ? date : "--/--/--";
                        default:    // unreachable
                            return match.ToString();
                    }
                }));
        }

        // %T09TIMEALL
        private string ReplaceTime(string input)
        {
            return new Regex(@"%T09TIMEALL", RegexOptions.IgnoreCase)
                .Replace(input, Utils.ToNothrowEvaluator(match =>
                {
                    return this.allScoreData.playList.TotalRunningTime.ToLongString();
                }));
        }

        // %T09CLEAR[x][yy][z]
        private string ReplaceClear(string input)
        {
            var pattern = string.Format(
                @"%T09CLEAR([{0}])({1})([12])",
                Utils.JoinEnumNames<LevelShort>(""),
                Utils.JoinEnumNames<CharaShort>("|"));
            return new Regex(pattern, RegexOptions.IgnoreCase)
                .Replace(input, Utils.ToNothrowEvaluator(match =>
                {
                    var level = (Level)Enum.Parse(typeof(LevelShort), match.Groups[1].Value, true);
                    var chara = (Chara)Enum.Parse(typeof(CharaShort), match.Groups[2].Value, true);
                    var type = int.Parse(match.Groups[3].Value);
                    var count = this.allScoreData.playList.ClearCounts[chara].Counts[level];

                    switch (type)
                    {
                        case 1:     // clear count
                            return this.ToNumberString(count);
                        case 2:     // clear flag
                            if (count > 0)
                                return "Cleared";
                            else
                            {
                                var score = this.allScoreData.rankings[new CharaLevelPair(chara, level)][0];
                                var date = Encoding.Default.GetString(score.Date).TrimEnd('\0');
                                return (date != "--/--") ? "Not Cleared" : "-------";
                            }
                        default:    // unreachable
                            return match.ToString();
                    }
                }));
        }
    }
}
