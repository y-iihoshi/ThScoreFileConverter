//-----------------------------------------------------------------------
// <copyright file="Th15Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591
#pragma warning disable SA1600 // ElementsMustBeDocumented

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th15Converter : ThConverter
    {
        private AllScoreData? allScoreData;

        public override string SupportedVersions { get; } = "1.00b";

        protected override bool ReadScoreFile(Stream input)
        {
            using var decrypted = new MemoryStream();
#if DEBUG
            using var decoded = new FileStream("th15decoded.dat", FileMode.Create, FileAccess.ReadWrite);
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
            if (this.allScoreData is null)
            {
                throw new InvalidDataException(
                    Utils.Format(Resources.InvalidOperationExceptionMustBeInvokedAfter, nameof(this.ReadScoreFile)));
            }

            return new List<IStringReplaceable>
            {
                new ScoreReplacer(this.allScoreData.ClearData),
                new CareerReplacer(this.allScoreData.ClearData),
                new CardReplacer(this.allScoreData.ClearData, hideUntriedCards),
                new CollectRateReplacer(this.allScoreData.ClearData),
                new ClearReplacer(this.allScoreData.ClearData),
                new CharaReplacer(this.allScoreData.ClearData),
                new CharaExReplacer(this.allScoreData.ClearData),
                new PracticeReplacer(this.allScoreData.ClearData),
            };
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
            var chapter = new Th10.Chapter();

            try
            {
                while (remainSize > 0)
                {
                    chapter.ReadFrom(reader);
                    if (!chapter.IsValid)
                        return false;
                    if (!ClearData.CanInitialize(chapter) && !Th13.Status.CanInitialize(chapter))
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
            var dictionary = new Dictionary<string, Action<AllScoreData, Th10.Chapter>>
            {
                { ClearData.ValidSignature,   (data, ch) => data.Set(new ClearData(ch))   },
                { Th13.Status.ValidSignature, (data, ch) => data.Set(new Th13.Status(ch)) },
            };

            using var reader = new BinaryReader(input, Encoding.UTF8, true);
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

            if ((allScoreData.Header is not null) &&
                (allScoreData.ClearData.Count == Enum.GetValues(typeof(CharaWithTotal)).Length) &&
                (allScoreData.Status is not null))
                return allScoreData;
            else
                return null;
        }
    }
}
