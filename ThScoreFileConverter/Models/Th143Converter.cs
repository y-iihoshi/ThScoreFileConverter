//-----------------------------------------------------------------------
// <copyright file="Th143Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591
#pragma warning disable SA1600 // ElementsMustBeDocumented

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th143Converter : ThConverter
    {
        private AllScoreData allScoreData = null;

        private Dictionary<(Day Day, int Scene), (string Path, BestShotHeader Header)> bestshots = null;

        public override string SupportedVersions
        {
            get { return "1.00a"; }
        }

        public override bool HasBestShotConverter
        {
            get { return true; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th143decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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

        protected override IEnumerable<IStringReplaceable> CreateReplacers(
            bool hideUntriedCards, string outputFilePath)
        {
            return new List<IStringReplaceable>
            {
                new ScoreReplacer(this),
                new ScoreTotalReplacer(this),
                new CardReplacer(this, hideUntriedCards),
                new NicknameReplacer(this),
                new TimeReplacer(this),
                new ShotReplacer(this, outputFilePath),
                new ShotExReplacer(this, outputFilePath),
            };
        }

        protected override string[] FilterBestShotFiles(string[] files)
        {
            var pattern = Utils.Format(@"sc({0})_\d{{2}}.dat", Parsers.DayLongPattern);

            return files.Where(file => Regex.IsMatch(
                Path.GetFileName(file), pattern, RegexOptions.IgnoreCase)).ToArray();
        }

        protected override void ConvertBestShot(Stream input, Stream output)
        {
            using (var decoded = new MemoryStream())
            {
                var outputFile = output as FileStream;

                using (var reader = new BinaryReader(input, Encoding.UTF8, true))
                {
                    var header = new BestShotHeader();
                    header.ReadFrom(reader);

                    if (this.bestshots == null)
                    {
                        this.bestshots =
                            new Dictionary<(Day, int), (string, BestShotHeader)>(Definitions.SpellCards.Count);
                    }

                    var key = (header.Day, header.Scene);
                    if (!this.bestshots.ContainsKey(key))
                        this.bestshots.Add(key, (outputFile.Name, header));

                    Lzss.Extract(input, decoded);

                    decoded.Seek(0, SeekOrigin.Begin);
                    using (var bitmap = new Bitmap(header.Width, header.Height, PixelFormat.Format32bppArgb))
                    {
                        try
                        {
                            var permission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
                            permission.Demand();

                            var bitmapData = bitmap.LockBits(
                                new Rectangle(0, 0, header.Width, header.Height),
                                ImageLockMode.WriteOnly,
                                bitmap.PixelFormat);
                            var source = decoded.ToArray();
                            var destination = bitmapData.Scan0;
                            Marshal.Copy(source, 0, destination, source.Length);
                            bitmap.UnlockBits(bitmapData);
                        }
                        catch (SecurityException e)
                        {
                            Console.WriteLine(e.ToString());
                        }

                        bitmap.Save(output, ImageFormat.Png);
                        output.Flush();
                        output.SetLength(output.Position);
                    }
                }
            }
        }

        private static bool Decrypt(Stream input, Stream output)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            using (var writer = new BinaryWriter(output, Encoding.UTF8, true))
            {
                var header = new Header();
                header.ReadFrom(reader);
                if (!header.IsValid)
                    return false;
                if (header.EncodedAllSize != reader.BaseStream.Length)
                    return false;

                header.WriteTo(writer);
                ThCrypt.Decrypt(input, output, header.EncodedBodySize, 0xAC, 0x35, 0x10, header.EncodedBodySize);

                return true;
            }
        }

        private static bool Extract(Stream input, Stream output)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            using (var writer = new BinaryWriter(output, Encoding.UTF8, true))
            {
                var header = new Header();
                header.ReadFrom(reader);
                header.WriteTo(writer);

                var bodyBeginPos = output.Position;
                Lzss.Extract(input, output);
                output.Flush();
                output.SetLength(output.Position);

                return header.DecodedBodySize == (output.Position - bodyBeginPos);
            }
        }

        private static bool Validate(Stream input)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var header = new Header();
                header.ReadFrom(reader);
                var remainSize = header.DecodedBodySize;
                var chapter = new Th10.Chapter();

                try
                {
                    while (remainSize > 0)
                    {
                        chapter.ReadFrom(reader);
                        if (!chapter.IsValid)
                            return false;
                        if (!Score.CanInitialize(chapter) &&
                            !ItemStatus.CanInitialize(chapter) &&
                            !Status.CanInitialize(chapter))
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
        }

        private static AllScoreData Read(Stream input)
        {
            var dictionary = new Dictionary<string, Action<AllScoreData, Th10.Chapter>>
            {
                { Score.ValidSignature,      (data, ch) => data.Set(new Score(ch))      },
                { ItemStatus.ValidSignature, (data, ch) => data.Set(new ItemStatus(ch)) },
                { Status.ValidSignature,     (data, ch) => data.Set(new Status(ch))     },
            };

            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var allScoreData = new AllScoreData();
                var chapter = new Th10.Chapter();

                var header = new Header();
                header.ReadFrom(reader);
                allScoreData.Set(header);

                try
                {
                    while (true)
                    {
                        chapter.ReadFrom(reader);
                        if (dictionary.TryGetValue(chapter.Signature, out var setChapter))
                            setChapter(allScoreData, chapter);
                    }
                }
                catch (EndOfStreamException)
                {
                    // It's OK, do nothing.
                }

                if ((allScoreData.Header != null) &&
                    //// (allScoreData.scores.Count >= 0) &&
                    (allScoreData.Status != null))
                    return allScoreData;
                else
                    return null;
            }
        }

        // %T143SCR[w][x][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T143SCR({0})([0-9])({1})([1-3])", Parsers.DayParser.Pattern, Parsers.ItemWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th143Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var day = Parsers.DayParser.Parse(match.Groups[1].Value);
                    var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    scene = (scene == 0) ? 10 : scene;
                    var item = Parsers.ItemWithTotalParser.Parse(match.Groups[3].Value);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    var key = (day, scene);
                    if (!Definitions.SpellCards.ContainsKey(key))
                        return match.ToString();

                    var score = parent.allScoreData.Scores.FirstOrDefault(elem =>
                        (elem != null) && ((elem.Number > 0) && (elem.Number <= Definitions.SpellCards.Count)) &&
                        Definitions.SpellCards.ElementAt(elem.Number - 1).Key.Equals(key));

                    switch (type)
                    {
                        case 1:     // high score
                            return (score != null) ? Utils.ToNumberString(score.HighScore * 10) : "0";
                        case 2:     // challenge count
                            if (item == ItemWithTotal.NoItem)
                            {
                                return "-";
                            }
                            else
                            {
                                return (score != null)
                                    ? Utils.ToNumberString(score.ChallengeCounts[item]) : "0";
                            }

                        case 3:     // cleared count
                            return (score != null) ? Utils.ToNumberString(score.ClearCounts[item]) : "0";
                        default:    // unreachable
                            return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T143SCRTL[x][y]
        private class ScoreTotalReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T143SCRTL({0})([1-4])", Parsers.ItemWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public ScoreTotalReplacer(Th143Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var item = Parsers.ItemWithTotalParser.Parse(match.Groups[1].Value);
                    var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                    switch (type)
                    {
                        case 1:     // total score
                            return Utils.ToNumberString(
                                parent.allScoreData.Scores.Sum(score => score.HighScore * 10L));
                        case 2:     // total of challenge counts
                            if (item == ItemWithTotal.NoItem)
                                return "-";
                            else
                                return Utils.ToNumberString(parent.allScoreData.ItemStatuses[item].UseCount);
                        case 3:     // total of cleared counts
                            return Utils.ToNumberString(
                                parent.allScoreData.ItemStatuses[item].ClearedCount);
                        case 4:     // num of cleared scenes
                            return Utils.ToNumberString(
                                parent.allScoreData.ItemStatuses[item].ClearedScenes);
                        default:    // unreachable
                            return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T143CARD[x][y][z]
        private class CardReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T143CARD({0})([0-9])([12])", Parsers.DayParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th143Converter parent, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var day = Parsers.DayParser.Parse(match.Groups[1].Value);
                    var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    scene = (scene == 0) ? 10 : scene;
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    var key = (day, scene);
                    if (!Definitions.SpellCards.ContainsKey(key))
                        return match.ToString();

                    if (hideUntriedCards)
                    {
                        var score = parent.allScoreData.Scores.FirstOrDefault(elem =>
                            (elem != null) && ((elem.Number > 0) && (elem.Number <= Definitions.SpellCards.Count)) &&
                            Definitions.SpellCards.ElementAt(elem.Number - 1).Key.Equals(key));
                        if ((score == null) || (score.ChallengeCounts[ItemWithTotal.Total] <= 0))
                            return "??????????";
                    }

                    if (type == 1)
                    {
                        return string.Join(
                            " &amp; ",
                            Definitions.SpellCards[key].Enemies.Select(enemy => enemy.ToLongName()).ToArray());
                    }
                    else
                    {
                        return Definitions.SpellCards[key].Card;
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T143NICK[xx]
        private class NicknameReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T143NICK(\d{2})";

            private readonly MatchEvaluator evaluator;

            public NicknameReplacer(Th143Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);

                    if ((number > 0) && (number <= Definitions.Nicknames.Count))
                    {
                        return (parent.allScoreData.Status.NicknameFlags.ElementAt(number) > 0)
                            ? Definitions.Nicknames[number - 1] : "??????????";
                    }
                    else
                    {
                        return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T143TIMEPLY
        private class TimeReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T143TIMEPLY";

            private readonly MatchEvaluator evaluator;

            public TimeReplacer(Th143Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    return new Time(parent.allScoreData.Status.TotalPlayTime * 10, false).ToLongString();
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T143SHOT[x][y]
        private class ShotReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T143SHOT({0})([0-9])", Parsers.DayParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ShotReplacer(Th143Converter parent, string outputFilePath)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var day = Parsers.DayParser.Parse(match.Groups[1].Value);
                    var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    scene = (scene == 0) ? 10 : scene;

                    var key = (day, scene);
                    if (!Definitions.SpellCards.ContainsKey(key))
                        return match.ToString();

                    if (!string.IsNullOrEmpty(outputFilePath) && parent.bestshots.TryGetValue(key, out var bestshot))
                    {
                        var relativePath = new Uri(outputFilePath)
                            .MakeRelativeUri(new Uri(bestshot.Path)).OriginalString;
                        var alternativeString = Utils.Format("SpellName: {0}", Definitions.SpellCards[key].Card);
                        return Utils.Format(
                            "<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" border=0>",
                            relativePath,
                            alternativeString);
                    }
                    else
                    {
                        return string.Empty;
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T143SHOTEX[w][x][y]
        private class ShotExReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T143SHOTEX({0})([0-9])([1-4])", Parsers.DayParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public ShotExReplacer(Th143Converter parent, string outputFilePath)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var day = Parsers.DayParser.Parse(match.Groups[1].Value);
                    var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    scene = (scene == 0) ? 10 : scene;
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    var key = (day, scene);
                    if (!Definitions.SpellCards.ContainsKey(key))
                        return match.ToString();

                    if (!string.IsNullOrEmpty(outputFilePath) && parent.bestshots.TryGetValue(key, out var bestshot))
                    {
                        switch (type)
                        {
                            case 1:     // relative path to the bestshot file
                                return new Uri(outputFilePath)
                                    .MakeRelativeUri(new Uri(bestshot.Path)).OriginalString;
                            case 2:     // width
                                return bestshot.Header.Width.ToString(CultureInfo.InvariantCulture);
                            case 3:     // height
                                return bestshot.Header.Height.ToString(CultureInfo.InvariantCulture);
                            case 4:     // date & time
                                return new DateTime(1970, 1, 1)
                                    .AddSeconds(bestshot.Header.DateTime).ToLocalTime()
                                    .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                            default:    // unreachable
                                return match.ToString();
                        }
                    }
                    else
                    {
                        switch (type)
                        {
                            case 1: return string.Empty;
                            case 2: return "0";
                            case 3: return "0";
                            case 4: return "----/--/-- --:--:--";
                            default: return match.ToString();
                        }
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        private class AllScoreData
        {
            private readonly List<IScore> scores;
            private readonly Dictionary<ItemWithTotal, IItemStatus> itemStatuses;

            public AllScoreData()
            {
                this.scores = new List<IScore>(Definitions.SpellCards.Count);
                this.itemStatuses = new Dictionary<ItemWithTotal, IItemStatus>(
                    Enum.GetValues(typeof(ItemWithTotal)).Length);
            }

            public Th095.HeaderBase Header { get; private set; }

            public IReadOnlyList<IScore> Scores => this.scores;

            public IReadOnlyDictionary<ItemWithTotal, IItemStatus> ItemStatuses => this.itemStatuses;

            public IStatus Status { get; private set; }

            public void Set(Th095.HeaderBase header) => this.Header = header;

            public void Set(IScore score) => this.scores.Add(score);

            public void Set(IItemStatus status)
            {
                if (!this.itemStatuses.ContainsKey(status.Item))
                    this.itemStatuses.Add(status.Item, status);
            }

            public void Set(IStatus status) => this.Status = status;
        }

        private class Status : Th10.Chapter, IStatus
        {
            public const string ValidSignature = "ST";
            public const ushort ValidVersion = 0x0001;
            public const int ValidSize = 0x00000224;

            public Status(Th10.Chapter chapter)
                : base(chapter, ValidSignature, ValidVersion, ValidSize)
            {
                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    this.LastName = reader.ReadExactBytes(14);
                    reader.ReadExactBytes(0x12);
                    this.BgmFlags = reader.ReadExactBytes(9);
                    reader.ReadExactBytes(0x17);
                    this.TotalPlayTime = reader.ReadInt32();
                    reader.ReadInt32();
                    this.LastMainItem = Utils.ToEnum<ItemWithTotal>(reader.ReadInt32());
                    this.LastSubItem = Utils.ToEnum<ItemWithTotal>(reader.ReadInt32());
                    reader.ReadExactBytes(0x54);
                    this.NicknameFlags = reader.ReadExactBytes(71);
                    reader.ReadExactBytes(0x12D);
                }
            }

            public IEnumerable<byte> LastName { get; }  // The last 2 bytes are always 0x00 ?

            public IEnumerable<byte> BgmFlags { get; }

            public int TotalPlayTime { get; }   // unit: 10ms

            public ItemWithTotal LastMainItem { get; }

            public ItemWithTotal LastSubItem { get; }

            public IEnumerable<byte> NicknameFlags { get; }

            public static bool CanInitialize(Th10.Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class BestShotHeader : IBinaryReadable
        {
            public const string ValidSignature = "BST3";
            public const int SignatureSize = 4;

            public string Signature { get; private set; }

            public Day Day { get; private set; }

            public short Scene { get; private set; }    // 1-based

            public short Width { get; private set; }

            public short Height { get; private set; }

            public uint DateTime { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public float SlowRate { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader is null)
                    throw new ArgumentNullException(nameof(reader));

                this.Signature = Encoding.Default.GetString(reader.ReadExactBytes(SignatureSize));
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException();

                reader.ReadUInt16();    // always 0xDF01?
                this.Day = Utils.ToEnum<Day>(reader.ReadInt16());
                this.Scene = (short)(reader.ReadInt16() + 1);
                reader.ReadUInt16();    // 0x0100 ... Version?
                this.Width = reader.ReadInt16();
                this.Height = reader.ReadInt16();
                reader.ReadUInt32();    // always 0x0005E800?
                this.DateTime = reader.ReadUInt32();
                this.SlowRate = reader.ReadSingle();    // really...?
                reader.ReadExactBytes(0x58);
            }
        }
    }
}
