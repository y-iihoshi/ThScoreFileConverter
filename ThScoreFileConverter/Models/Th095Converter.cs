//-----------------------------------------------------------------------
// <copyright file="Th095Converter.cs" company="None">
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
using ThScoreFileConverter.Models.Th095;
using static ThScoreFileConverter.Models.Th095.Parsers;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th095Converter : ThConverter
    {
        private AllScoreData allScoreData = null;

        private Dictionary<(Th095.Level Level, int Scene), BestShotPair> bestshots = null;

        public override string SupportedVersions
        {
            get { return "1.02a"; }
        }

        public override bool HasBestShotConverter
        {
            get { return true; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th095decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
                new ShotReplacer(this, outputFilePath),
                new ShotExReplacer(this, outputFilePath),
            };
        }

        protected override string[] FilterBestShotFiles(string[] files)
        {
            var pattern = Utils.Format(@"bs_({0})_[1-9].dat", LevelLongPattern);

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

                    var key = (header.Level, header.Scene);
                    if (this.bestshots == null)
                        this.bestshots = new Dictionary<(Th095.Level, int), BestShotPair>(Definitions.SpellCards.Count);
                    if (!this.bestshots.ContainsKey(key))
                        this.bestshots.Add(key, new BestShotPair(outputFile.Name, header));

                    Lzss.Extract(input, decoded);

                    decoded.Seek(0, SeekOrigin.Begin);
                    using (var bitmap = new Bitmap(header.Width, header.Height, PixelFormat.Format24bppRgb))
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
                            var sourceStride = 3 * header.Width;    // "3" means 24bpp.
                            var destination = bitmapData.Scan0;
                            for (var sourceIndex = 0; sourceIndex < source.Length; sourceIndex += sourceStride)
                            {
                                Marshal.Copy(source, sourceIndex, destination, sourceStride);
                                destination += bitmapData.Stride;
                            }

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
                var chapter = new Th095.Chapter();

                try
                {
                    while (remainSize > 0)
                    {
                        chapter.ReadFrom(reader);
                        if (!chapter.IsValid)
                            return false;
                        if (!Score.CanInitialize(chapter) && !Status.CanInitialize(chapter))
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
            var dictionary = new Dictionary<string, Action<AllScoreData, Th095.Chapter>>
            {
                { Score.ValidSignature,  (data, ch) => data.Set(new Score(ch))  },
                { Status.ValidSignature, (data, ch) => data.Set(new Status(ch)) },
            };

            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var allScoreData = new AllScoreData();
                var chapter = new Th095.Chapter();

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

        // %T95SCR[x][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T95SCR({0})([1-9])([1-4])", Parsers.LevelParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th095Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                    var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    var key = (level, scene);
                    if (!Definitions.SpellCards.ContainsKey(key))
                        return match.ToString();

                    var score = parent.allScoreData.Scores.FirstOrDefault(
                        elem => (elem != null) && elem.LevelScene.Equals(key));

                    switch (type)
                    {
                        case 1:     // high score
                            return (score != null) ? Utils.ToNumberString(score.HighScore) : "0";
                        case 2:     // bestshot score
                            return (score != null) ? Utils.ToNumberString(score.BestshotScore) : "0";
                        case 3:     // num of shots
                            return (score != null) ? Utils.ToNumberString(score.TrialCount) : "0";
                        case 4:     // slow rate
                            return (score != null) ? Utils.Format("{0:F3}%", score.SlowRate2) : "-----%";
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

        // %T95SCRTL[x]
        private class ScoreTotalReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T95SCRTL([1-4])";

            private readonly MatchEvaluator evaluator;

            public ScoreTotalReplacer(Th095Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var type = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);

                    switch (type)
                    {
                        case 1:     // total score
                            return Utils.ToNumberString(
                                parent.allScoreData.Scores.Sum(
                                    score => (score != null) ? (long)score.HighScore : 0L));
                        case 2:     // total of bestshot scores
                            return Utils.ToNumberString(
                                parent.allScoreData.Scores.Sum(
                                    score => (score != null) ? (long)score.BestshotScore : 0L));
                        case 3:     // total of num of shots
                            return Utils.ToNumberString(
                                parent.allScoreData.Scores.Sum(
                                    score => (score != null) ? score.TrialCount : 0));
                        case 4:     // num of succeeded scenes
                            return parent.allScoreData.Scores
                                .Count(score => (score != null) && (score.HighScore > 0))
                                .ToString(CultureInfo.CurrentCulture);
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

        // %T95CARD[x][y][z]
        private class CardReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T95CARD({0})([1-9])([12])", Parsers.LevelParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th095Converter parent, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                    var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    var key = (level, scene);
                    if (!Definitions.SpellCards.ContainsKey(key))
                        return match.ToString();

                    if (hideUntriedCards)
                    {
                        var score = parent.allScoreData.Scores.FirstOrDefault(
                            elem => (elem != null) && elem.LevelScene.Equals(key));
                        if (score == null)
                            return "??????????";
                    }

                    return (type == 1)
                        ? Definitions.SpellCards[key].Enemy.ToLongName() : Definitions.SpellCards[key].Card;
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T95SHOT[x][y]
        private class ShotReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T95SHOT({0})([1-9])", Parsers.LevelParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ShotReplacer(Th095Converter parent, string outputFilePath)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                    var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                    var key = (level, scene);
                    if (!Definitions.SpellCards.ContainsKey(key))
                        return match.ToString();

                    if (!string.IsNullOrEmpty(outputFilePath) && parent.bestshots.TryGetValue(key, out var bestshot))
                    {
                        var relativePath = new Uri(outputFilePath)
                            .MakeRelativeUri(new Uri(bestshot.Path)).OriginalString;
                        var alternativeString = Utils.Format(
                            "ClearData: {0}{3}Slow: {1:F6}%{3}SpellName: {2}",
                            Utils.ToNumberString(bestshot.Header.Score),
                            bestshot.Header.SlowRate,
                            Encoding.Default.GetString(bestshot.Header.CardName).TrimEnd('\0'),
                            Environment.NewLine);
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

        // %T95SHOTEX[x][y][z]
        private class ShotExReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T95SHOTEX({0})([1-9])([1-6])", Parsers.LevelParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1513:ClosingCurlyBracketMustBeFollowedByBlankLine", Justification = "Reviewed.")]
            public ShotExReplacer(Th095Converter parent, string outputFilePath)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                    var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    var key = (level, scene);
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
                            case 4:     // score
                                return Utils.ToNumberString(bestshot.Header.Score);
                            case 5:     // slow rate
                                return Utils.Format("{0:F6}%", bestshot.Header.SlowRate);
                            case 6:     // date & time
                                {
                                    var score = parent.allScoreData.Scores.FirstOrDefault(
                                        elem => (elem != null) && elem.LevelScene.Equals(key));
                                    if (score != null)
                                    {
                                        return new DateTime(1970, 1, 1)
                                            .AddSeconds(score.DateTime).ToLocalTime()
                                            .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                                    }
                                    else
                                    {
                                        return "----/--/-- --:--:--";
                                    }
                                }
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
                            case 4: return "--------";
                            case 5: return "-----%";
                            case 6: return "----/--/-- --:--:--";
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

        private class BestShotPair : Tuple<string, BestShotHeader>
        {
            public BestShotPair(string path, BestShotHeader header)
                : base(path, header)
            {
            }

            public string Path => this.Item1;

            public BestShotHeader Header => this.Item2;

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public void Deconstruct(out string path, out BestShotHeader header)
            {
                path = this.Path;
                header = this.Header;
            }
        }

        private class AllScoreData
        {
            private readonly List<IScore> scores;

            public AllScoreData() => this.scores = new List<IScore>(Definitions.SpellCards.Count);

            public Header Header { get; private set; }

            public IReadOnlyList<IScore> Scores => this.scores;

            public IStatus Status { get; private set; }

            public void Set(Header header) => this.Header = header;

            public void Set(IScore score) => this.scores.Add(score);

            public void Set(IStatus status) => this.Status = status;
        }

        private class Header : Th095.Header
        {
            public const string ValidSignature = "TH95";

            public override bool IsValid
                => base.IsValid && this.Signature.Equals(ValidSignature, StringComparison.Ordinal);
        }

        private class Status : Chapter, IStatus
        {
            public const string ValidSignature = "ST";
            public const ushort ValidVersion = 0x0000;
            public const int ValidSize = 0x00000458;

            public Status(Chapter chapter)
                : base(chapter, ValidSignature, ValidVersion, ValidSize)
            {
                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    this.LastName = reader.ReadExactBytes(10);
                    reader.ReadExactBytes(0x0442);
                }
            }

            public IEnumerable<byte> LastName { get; }  // The last 2 bytes are always 0x00 ?

            public static bool CanInitialize(Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class BestShotHeader : IBinaryReadable
        {
            public const string ValidSignature = "BSTS";
            public const int SignatureSize = 4;

            public string Signature { get; private set; }

            public Th095.Level Level { get; private set; }

            public short Scene { get; private set; }    // 1-based

            public short Width { get; private set; }

            public short Height { get; private set; }

            public int Score { get; private set; }

            public float SlowRate { get; private set; }

            public byte[] CardName { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader is null)
                    throw new ArgumentNullException(nameof(reader));

                this.Signature = Encoding.Default.GetString(reader.ReadExactBytes(SignatureSize));
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException();

                reader.ReadUInt16();
                this.Level = Utils.ToEnum<Th095.Level>(reader.ReadInt16() - 1);
                this.Scene = reader.ReadInt16();
                reader.ReadUInt16();    // 0x0102 ... Version?
                this.Width = reader.ReadInt16();
                this.Height = reader.ReadInt16();
                this.Score = reader.ReadInt32();
                this.SlowRate = reader.ReadSingle();
                this.CardName = reader.ReadExactBytes(0x50);
            }
        }
    }
}
