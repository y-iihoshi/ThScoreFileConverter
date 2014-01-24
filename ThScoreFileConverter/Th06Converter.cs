//-----------------------------------------------------------------------
// <copyright file="Th06Converter.cs" company="None">
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
    public class Th06Converter : ThConverter
    {
        private enum Level               { Easy, Normal, Hard, Lunatic, Extra }
        private enum LevelWithTotal      { Easy, Normal, Hard, Lunatic, Extra, Total }
        private enum LevelShort          { E, N, H, L, X }
        private enum LevelShortWithTotal { E, N, H, L, X, T }

        private enum Chara               { ReimuA, ReimuB, MarisaA, MarisaB }
        private enum CharaWithTotal      { ReimuA, ReimuB, MarisaA, MarisaB, Total }
        private enum CharaShort          { RA, RB, MA, MB }
        private enum CharaShortWithTotal { RA, RB, MA, MB, TL }

        private enum Stage          { Stage1, Stage2, Stage3, Stage4, Stage5, Stage6, Extra }
        private enum StageWithTotal { Stage1, Stage2, Stage3, Stage4, Stage5, Stage6, Extra, Total }

        private static readonly string[] StageProgressArray =
        {
            "-------", "Stage 1", "Stage 2", "Stage 3", "Stage 4", "Stage 5", "Stage 6", "Extra Stage"
        };

        private static readonly List<HighScore> InitialRanking = new List<HighScore>()
        {
            new HighScore(1000000),
            new HighScore(900000),
            new HighScore(800000),
            new HighScore(700000),
            new HighScore(600000),
            new HighScore(500000),
            new HighScore(400000),
            new HighScore(300000),
            new HighScore(200000),
            new HighScore(100000)
        };

        private const int NumCards = 64;

        // Thanks to thwiki.info
        private static readonly Dictionary<Stage, Range<int>> StageCardTable =
            new Dictionary<Stage, Range<int>>()
            {
                { Stage.Stage1, new Range<int> { Min = 0,  Max = 2  } },
                { Stage.Stage2, new Range<int> { Min = 3,  Max = 6  } },
                { Stage.Stage3, new Range<int> { Min = 7,  Max = 13 } },
                { Stage.Stage4, new Range<int> { Min = 14, Max = 31 } },
                { Stage.Stage5, new Range<int> { Min = 32, Max = 39 } },
                { Stage.Stage6, new Range<int> { Min = 40, Max = 50 } },
                { Stage.Extra,  new Range<int> { Min = 51, Max = 63 } }
            };

        // Thanks to thwiki.info
        private static readonly Level[][] CardLevelTable =
        {
            new Level[] { Level.Hard, Level.Lunatic },
            new Level[] { Level.Easy, Level.Normal, Level.Hard, Level.Lunatic },
            new Level[] { Level.Easy, Level.Normal, Level.Hard, Level.Lunatic },
            new Level[] { Level.Easy, Level.Normal },
            new Level[] { Level.Hard, Level.Lunatic },
            new Level[] { Level.Easy, Level.Normal, Level.Hard, Level.Lunatic },
            new Level[] { Level.Normal, Level.Hard, Level.Lunatic },
            new Level[] { Level.Easy, Level.Normal },
            new Level[] { Level.Hard, Level.Lunatic },
            new Level[] { Level.Easy, Level.Normal, Level.Hard, Level.Lunatic },

            new Level[] { Level.Hard, Level.Lunatic },
            new Level[] { Level.Easy, Level.Normal },
            new Level[] { Level.Hard, Level.Lunatic },
            new Level[] { Level.Normal, Level.Hard, Level.Lunatic },
            new Level[] { Level.Easy, Level.Normal },
            new Level[] { Level.Easy, Level.Normal },
            new Level[] { Level.Easy, Level.Normal },
            new Level[] { Level.Easy, Level.Normal },
            new Level[] { Level.Normal },
            new Level[] { Level.Normal, Level.Hard, Level.Lunatic },

            new Level[] { Level.Normal, Level.Hard, Level.Lunatic },
            new Level[] { Level.Normal, Level.Hard, Level.Lunatic },
            new Level[] { Level.Hard, Level.Lunatic },
            new Level[] { Level.Hard, Level.Lunatic },
            new Level[] { Level.Hard, Level.Lunatic },
            new Level[] { Level.Hard, Level.Lunatic },
            new Level[] { Level.Hard, Level.Lunatic },
            new Level[] { Level.Easy, Level.Normal, Level.Hard, Level.Lunatic },
            new Level[] { Level.Easy, Level.Normal, Level.Hard, Level.Lunatic },
            new Level[] { Level.Easy, Level.Normal, Level.Hard, Level.Lunatic },

            new Level[] { Level.Normal, Level.Hard, Level.Lunatic },
            new Level[] { Level.Easy, Level.Normal, Level.Hard, Level.Lunatic },
            new Level[] { Level.Easy, Level.Normal },
            new Level[] { Level.Hard, Level.Lunatic },
            new Level[] { Level.Easy, Level.Normal },
            new Level[] { Level.Easy, Level.Normal },
            new Level[] { Level.Easy, Level.Normal },
            new Level[] { Level.Hard, Level.Lunatic },
            new Level[] { Level.Hard, Level.Lunatic },
            new Level[] { Level.Hard, Level.Lunatic },

            new Level[] { Level.Normal, Level.Hard, Level.Lunatic },
            new Level[] { Level.Normal },
            new Level[] { Level.Normal },
            new Level[] { Level.Normal },
            new Level[] { Level.Normal },
            new Level[] { Level.Normal },
            new Level[] { Level.Hard, Level.Lunatic },
            new Level[] { Level.Hard, Level.Lunatic },
            new Level[] { Level.Hard, Level.Lunatic },
            new Level[] { Level.Hard, Level.Lunatic },

            new Level[] { Level.Hard, Level.Lunatic },
            new Level[] { Level.Extra },
            new Level[] { Level.Extra },
            new Level[] { Level.Extra },
            new Level[] { Level.Extra },
            new Level[] { Level.Extra },
            new Level[] { Level.Extra },
            new Level[] { Level.Extra },
            new Level[] { Level.Extra },
            new Level[] { Level.Extra },

            new Level[] { Level.Extra },
            new Level[] { Level.Extra },
            new Level[] { Level.Extra },
            new Level[] { Level.Extra }
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
            public Header Header { get; set; }
            public Dictionary<CharaLevelPair, List<HighScore>> Rankings { get; set; }
            public Dictionary<Chara, ClearData> ClearData { get; set; }
            public CardAttack[] CardAttacks { get; set; }
            public Dictionary<CharaLevelPair, Dictionary<Stage, PracticeScore>> PracticeScores { get; set; }

            public AllScoreData()
            {
                var numCharas = Enum.GetValues(typeof(Chara)).Length;
                this.Rankings = new Dictionary<CharaLevelPair, List<HighScore>>();
                this.ClearData = new Dictionary<Chara, ClearData>(numCharas);
                this.CardAttacks = new CardAttack[NumCards];
                this.PracticeScores = new Dictionary<CharaLevelPair, Dictionary<Stage, PracticeScore>>();
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
            public uint Unknown { get; private set; }   // always 0x00000010?

            public Header(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "TH6K")
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
            public uint Unknown1 { get; private set; }      // always 0x00000001?
            public uint Score { get; private set; }
            public Chara Chara { get; private set; }        // size: 1Byte
            public Level Level { get; private set; }        // size: 1Byte
            public byte StageProgress { get; private set; } // 1..7: Stage1..Extra, 99: all clear
            public byte[] Name { get; private set; }        // .Length = 9, null-terminated

            public HighScore(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "HSCR")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x001C)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x001C)
                //     throw new InvalidDataException("Size2");
            }

            public HighScore(uint score)    // for InitialRanking only
                : base()
            {
                this.Score = score;
                this.Name = Encoding.Default.GetBytes("Nanashi\0\0");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.Unknown1 = reader.ReadUInt32();
                this.Score = reader.ReadUInt32();
                this.Chara = (Chara)reader.ReadByte();
                this.Level = (Level)reader.ReadByte();
                this.StageProgress = reader.ReadByte();
                this.Name = reader.ReadBytes(9);
            }
        }

        private class ClearData : Chapter   // per character
        {
            public uint Unknown1 { get; private set; }          // always 0x00000010?
            public byte[] StoryFlags { get; private set; }      // [level]; really...?
            public byte[] PracticeFlags { get; private set; }   // [level]; really...?
            public Chara Chara { get; private set; }            // size: 2Bytes

            public ClearData(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "CLRD")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0018)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x0018)
                //     throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                var numLevels = Enum.GetValues(typeof(Level)).Length;
                this.Unknown1 = reader.ReadUInt32();
                this.StoryFlags = reader.ReadBytes(numLevels);
                this.PracticeFlags = reader.ReadBytes(numLevels);
                this.Chara = (Chara)reader.ReadInt16();
            }
        }

        private class CardAttack : Chapter      // per card
        {
            public byte[] Unknown1 { get; private set; }    // .Length = 8
            public short Number { get; private set; }       // 0-origin
            public byte[] Unknown2 { get; private set; }    // .Length = 6
            public byte[] CardName { get; private set; }    // .Length = 0x24, null-terminated
            public ushort TrialCount { get; private set; }
            public ushort ClearCount { get; private set; }

            public CardAttack(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "CATK")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0040)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x0040)
                //     throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.Unknown1 = reader.ReadBytes(8);
                this.Number = reader.ReadInt16();
                this.Unknown2 = reader.ReadBytes(6);
                this.CardName = reader.ReadBytes(0x24);
                this.TrialCount = reader.ReadUInt16();
                this.ClearCount = reader.ReadUInt16();
            }

            public bool HasTried()
            {
                return this.TrialCount > 0;
            }
        }

        private class PracticeScore : Chapter   // per character, level, stage
        {
            public uint Unknown1 { get; private set; }      // always 0x00000010?
            public int HighScore { get; private set; }
            public Chara Chara { get; private set; }        // size: 1Byte
            public Level Level { get; private set; }        // size: 1Byte
            public Stage Stage { get; private set; }        // size: 1Byte
            public byte Unknown2 { get; private set; }

            public PracticeScore(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "PSCR")
                    throw new InvalidDataException("Signature");
                if (this.Size1 != 0x0014)
                    throw new InvalidDataException("Size1");
                // if (this.Size2 != 0x0014)
                //     throw new InvalidDataException("Size2");
            }

            public override void ReadFrom(BinaryReader reader)
            {
                this.Unknown1 = reader.ReadUInt32();
                this.HighScore = reader.ReadInt32();
                this.Chara = (Chara)reader.ReadByte();
                this.Level = (Level)reader.ReadByte();
                this.Stage = (Stage)reader.ReadByte();
                this.Unknown2 = reader.ReadByte();
            }
        }

        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get
            {
                return "1.02h";
            }
        }

        public Th06Converter() { }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th06decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
            if (version != 0x10)
                return false;

            var unknown2 = reader.ReadUInt16();
            var headerSize = reader.ReadInt32();
            if (headerSize != 0x14)
                return false;

            var unknown3 = reader.ReadUInt32();
            var size = reader.ReadInt32();
            if (size != input.Length)
                return false;

            var header = new byte[headerSize];
            input.Seek(0, SeekOrigin.Begin);
            input.Read(header, 0, headerSize);
            output.Write(header, 0, headerSize);

            // Lzss.Extract(input, output);
            var body = new byte[size - headerSize];
            input.Read(body, 0, body.Length);
            output.Write(body, 0, body.Length);
            output.Flush();
            output.SetLength(output.Position);

            return true;
        }

        private bool Validate(Stream input)
        {
            var reader = new BinaryReader(input);
            var chapter = new Chapter();

            reader.ReadBytes(8);
            var headerSize = reader.ReadInt32();
            var remainSize = input.Length - headerSize;
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
                        case "TH6K":
                            temp = reader.ReadByte();
                            // 8 means the total size of Signature, Size1, and Size2.
                            reader.ReadBytes(chapter.Size1 - 8 - 1);
                            if (temp != 0x10)
                                return false;
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
            reader.ReadBytes(0x14);

            try
            {
                while (true)
                {
                    chapter.ReadFrom(reader);
                    switch (chapter.Signature)
                    {
                        case "TH6K":
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
                                var cardAttack = new CardAttack(chapter);
                                cardAttack.ReadFrom(reader);
                                allScoreData.CardAttacks[cardAttack.Number] = cardAttack;
                            }
                            break;

                        case "PSCR":
                            {
                                var score = new PracticeScore(chapter);
                                score.ReadFrom(reader);
                                if ((score.Level != Level.Extra) && (score.Stage != Stage.Extra) &&
                                    !((score.Level == Level.Easy) && (score.Stage == Stage.Stage6)))
                                {
                                    var key = new CharaLevelPair(score.Chara, score.Level);
                                    if (!allScoreData.PracticeScores.ContainsKey(key))
                                        allScoreData.PracticeScores.Add(
                                            key, new Dictionary<Stage, PracticeScore>());
                                    var scores = allScoreData.PracticeScores[key];
                                    if (!scores.ContainsKey(score.Stage))
                                        scores.Add(score.Stage, score);
                                }
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
                // (allScoreData.cardAttacks.Length == NumCards) &&
                // (allScoreData.practiceScores.Count >= 0) &&
                (allScoreData.ClearData.Count == Enum.GetValues(typeof(Chara)).Length))
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
            allLine = this.ReplacePractice(allLine);
            writer.Write(allLine);

            writer.Flush();
            writer.BaseStream.SetLength(writer.BaseStream.Position);
        }

        // %T06SCR[w][xx][y][z]
        private string ReplaceScore(string input)
        {
            var pattern = string.Format(
                @"%T06SCR([{0}])({1})(\d)([1-3])",
                Utils.JoinEnumNames<LevelShort>(string.Empty),
                Utils.JoinEnumNames<CharaShort>("|"));
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                var level = Utils.ParseEnum<LevelShort>(match.Groups[1].Value, true);
                var chara = Utils.ParseEnum<CharaShort>(match.Groups[2].Value, true);
                var rank = (int.Parse(match.Groups[3].Value) + 9) % 10;     // 1..9,0 -> 0..9
                var type = int.Parse(match.Groups[4].Value);

                var key = new CharaLevelPair(chara, level);
                var score = this.allScoreData.Rankings.ContainsKey(key)
                    ? this.allScoreData.Rankings[key][rank] : InitialRanking[rank];

                switch (type)
                {
                    case 1:     // name
                        return Encoding.Default.GetString(score.Name).Split('\0')[0];
                    case 2:     // score
                        return this.ToNumberString(score.Score);
                    case 3:     // stage
                        if (score.StageProgress == 99)
                            return "All Clear";
                        else if (score.StageProgress < StageProgressArray.Length)
                            return StageProgressArray[score.StageProgress];
                        else
                            return StageProgressArray[0];
                    default:    // unreachable
                        return match.ToString();
                }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T06C[xx][y]
        private string ReplaceCareer(string input)
        {
            var pattern = @"%T06C(\d{2})([12])";
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value);
                var type = int.Parse(match.Groups[2].Value);

                if (number == 0)
                    switch (type)
                    {
                        case 1:     // clear count
                            return this.ToNumberString(
                                this.allScoreData.CardAttacks.Sum(
                                    attack => (attack != null) ? attack.ClearCount : 0));
                        case 2:     // trial count
                            return this.ToNumberString(
                                this.allScoreData.CardAttacks.Sum(
                                    attack => (attack != null) ? attack.TrialCount : 0));
                        default:    // unreachable
                            return match.ToString();
                    }
                else if ((0 < number) && (number <= NumCards))
                {
                    var attack = this.allScoreData.CardAttacks[number - 1];
                    if (attack != null)
                        switch (type)
                        {
                            case 1:     // clear count
                                return this.ToNumberString(attack.ClearCount);
                            case 2:     // trial count
                                return this.ToNumberString(attack.TrialCount);
                            default:    // unreachable
                                return match.ToString();
                        }
                    else
                        return "0";
                }
                else
                    return match.ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T06CARD[xx][y]
        private string ReplaceCard(string input)
        {
            var pattern = @"%T06CARD(\d{2})([NR])";
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value);
                var type = match.Groups[2].Value.ToUpper();

                if ((0 < number) && (number <= NumCards))
                {
                    var attack = this.allScoreData.CardAttacks[number - 1];
                    if (type == "N")
                        return ((attack != null) && attack.HasTried())
                            ? Encoding.Default.GetString(attack.CardName).Split('\0')[0]
                            : "??????????";
                    else
                    {
                        if ((attack != null) && attack.HasTried())
                        {
                            var levelStrings = Array.ConvertAll<Level, string>(
                                CardLevelTable[attack.Number], elem => elem.ToString());
                            return string.Join(", ", levelStrings);
                        }
                        else
                            return "?????";
                    }
                }
                else
                    return match.ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T06CRG[x][y]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceCollectRate(string input)
        {
            string[] stageShortWithTotalArray = { "0", "1", "2", "3", "4", "5", "6", "X" };
            var pattern = @"%T06CRG([0-6X])([12])";
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                var stage = Array.IndexOf(stageShortWithTotalArray, match.Groups[1].Value.ToUpper());
                var type = int.Parse(match.Groups[2].Value);

                Func<CardAttack, bool> findCard = (attack => false);

                if (stage == 0)     // total
                {
                    if (type == 1)
                        findCard = (attack => (attack != null) && (attack.ClearCount > 0));
                    else
                        findCard = (attack => (attack != null) && (attack.TrialCount > 0));
                }
                else
                {
                    var st = (Stage)(stage - 1);
                    if (type == 1)
                        findCard = (attack =>
                            (attack != null) &&
                            StageCardTable[st].Contains(attack.Number) && (attack.ClearCount > 0));
                    else
                        findCard = (attack =>
                            (attack != null) &&
                            StageCardTable[st].Contains(attack.Number) && (attack.TrialCount > 0));
                }

                return this.allScoreData.CardAttacks.Count(findCard).ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T06CLEAR[x][yy]
        private string ReplaceClear(string input)
        {
            var pattern = string.Format(
                @"%T06CLEAR([{0}])({1})",
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
                    if (stageProgress == (int)Stage.Extra + 1)
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
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T06PRAC[x][yy][z]
        private string ReplacePractice(string input)
        {
            var pattern = string.Format(
                @"%T06PRAC([{0}])({1})([1-6])",
                Utils.JoinEnumNames<LevelShort>(string.Empty),
                Utils.JoinEnumNames<CharaShort>("|"));
            var evaluator = Utils.ToNothrowEvaluator(match =>
            {
                var level = Utils.ParseEnum<LevelShort>(match.Groups[1].Value, true);
                var chara = Utils.ParseEnum<CharaShort>(match.Groups[2].Value, true);
                var stage = (Stage)(int.Parse(match.Groups[3].Value) - 1);

                if (level == LevelShort.X)
                    return match.ToString();

                var key = new CharaLevelPair(chara, level);
                if (this.allScoreData.PracticeScores.ContainsKey(key))
                {
                    var scores = this.allScoreData.PracticeScores[key];
                    return this.ToNumberString(
                        scores.ContainsKey(stage) ? scores[stage].HighScore : 0);
                }
                else
                    return "0";
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }
    }
}
