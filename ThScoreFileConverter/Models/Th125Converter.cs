//-----------------------------------------------------------------------
// <copyright file="Th125Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // ElementsMustBeDocumented

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th125;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.Models
{
    internal class Th125Converter : ThConverter
    {
        private readonly Dictionary<
            (Chara, Th125.Level Level, int Scene), (string Path, IBestShotHeader Header)> bestshots =
            new Dictionary<(Chara, Th125.Level, int), (string, IBestShotHeader)>(
                Enum.GetValues(typeof(Chara)).Length * Definitions.SpellCards.Count);

        private AllScoreData? allScoreData;

        public override string SupportedVersions { get; } = "1.00a";

        public override bool HasBestShotConverter { get; } = true;

        protected override bool ReadScoreFile(Stream input)
        {
            using var decrypted = new MemoryStream();
#if DEBUG
            using var decoded = new FileStream("th125decoded.dat", FileMode.Create, FileAccess.ReadWrite);
#else
            using var decoded = new MemoryStream();
#endif

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

            return this.allScoreData is not null;
        }

        protected override IEnumerable<IStringReplaceable> CreateReplacers(bool hideUntriedCards, string outputFilePath)
        {
            if ((this.allScoreData is null) || (this.allScoreData.Status is null))
            {
                throw new InvalidDataException(
                    Utils.Format(Resources.InvalidOperationExceptionMustBeInvokedAfter, nameof(this.ReadScoreFile)));
            }

            return new List<IStringReplaceable>
            {
                new ScoreReplacer(this.allScoreData.Scores),
                new ScoreTotalReplacer(this.allScoreData.Scores),
                new CardReplacer(this.allScoreData.Scores, hideUntriedCards),
                new TimeReplacer(this.allScoreData.Status),
                new ShotReplacer(this.bestshots, outputFilePath),
                new ShotExReplacer(this.bestshots, this.allScoreData.Scores, outputFilePath),
            };
        }

        protected override string[] FilterBestShotFiles(string[] files)
        {
            var pattern = Utils.Format(@"bs2?_({0})_[1-9].dat", Parsers.LevelLongPattern);

            return files.Where(file => Regex.IsMatch(
                Path.GetFileName(file), pattern, RegexOptions.IgnoreCase)).ToArray();
        }

        protected override void ConvertBestShot(Stream input, Stream output)
        {
            using var decoded = new MemoryStream();

            if (output is not FileStream outputFile)
                throw new ArgumentException(Resources.ArgumentExceptionWrongType, nameof(output));
            var chara = Path.GetFileName(outputFile.Name)
                .StartsWith("bs2_", StringComparison.CurrentCultureIgnoreCase)
                ? Chara.Hatate : Chara.Aya;

            using var reader = new BinaryReader(input, Encoding.UTF8, true);
            var header = new BestShotHeader();
            header.ReadFrom(reader);

            var key = (chara, header.Level, header.Scene);
            _ = this.bestshots.TryAdd(key, (outputFile.Name, header));

            Lzss.Decompress(input, decoded);

            _ = decoded.Seek(0, SeekOrigin.Begin);
            using var bitmap = new Bitmap(header.Width, header.Height, PixelFormat.Format32bppArgb);

            try
            {
#if !NET5_0
                var permission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
                permission.Demand();
#endif

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

        private static bool Decrypt(Stream input, Stream output)
        {
            using var reader = new BinaryReader(input, Encoding.UTF8, true);
            using var writer = new BinaryWriter(output, Encoding.UTF8, true);
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

        private static bool Extract(Stream input, Stream output)
        {
            using var reader = new BinaryReader(input, Encoding.UTF8, true);
            using var writer = new BinaryWriter(output, Encoding.UTF8, true);

            var header = new Header();
            header.ReadFrom(reader);
            header.WriteTo(writer);

            var bodyBeginPos = output.Position;
            Lzss.Decompress(input, output);
            output.Flush();
            output.SetLength(output.Position);

            return header.DecodedBodySize == (output.Position - bodyBeginPos);
        }

        private static bool Validate(Stream input)
        {
            using var reader = new BinaryReader(input, Encoding.UTF8, true);

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

        private static AllScoreData? Read(Stream input)
        {
            var dictionary = new Dictionary<string, Action<AllScoreData, Th095.Chapter>>
            {
                { Score.ValidSignature,  (data, ch) => data.Set(new Score(ch))  },
                { Status.ValidSignature, (data, ch) => data.Set(new Status(ch)) },
            };

            using var reader = new BinaryReader(input, Encoding.UTF8, true);
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

            if ((allScoreData.Header is not null) &&
                //// (allScoreData.scores.Count >= 0) &&
                (allScoreData.Status is not null))
                return allScoreData;
            else
                return null;
        }
    }
}
