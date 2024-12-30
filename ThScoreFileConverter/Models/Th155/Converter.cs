//-----------------------------------------------------------------------
// <copyright file="Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // ElementsMustBeDocumented

using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Resources;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th155;

#if !DEBUG
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Instantiated by ThConverterFactory.")]
#endif
internal sealed class Converter : ThConverter
{
    private AllScoreData? allScoreData;

    public override string SupportedVersions { get; } = "1.10c";

    public override bool HasCardReplacer { get; }

    protected override bool ReadScoreFile(Stream input)
    {
        using var decoded = StreamHelper.Create("th155decoded.dat", FileMode.Create, FileAccess.ReadWrite);

        if (!Extract(input, decoded))
            return false;

        _ = decoded.Seek(0, SeekOrigin.Begin);
        this.allScoreData = Read(decoded);

        return this.allScoreData is not null;
    }

    protected override IEnumerable<IStringReplaceable> CreateReplacers(
        INumberFormatter formatter, bool hideUntriedCards, string outputFilePath)
    {
        if (this.allScoreData is null)
        {
            ThrowHelper.ThrowInvalidDataException(
                StringHelper.Format(ExceptionMessages.InvalidOperationExceptionMustBeInvokedAfter, nameof(this.ReadScoreFile)));
        }

        return
        [
            new ClearRankReplacer(this.allScoreData.StoryDictionary),
        ];
    }

    private static bool Extract(Stream input, Stream output)
    {
        var succeeded = false;

        // See section 2.2 of RFC 1950
        var validHeader = new byte[] { 0x78, 0x9C };

        if (input.Length >= sizeof(int) + validHeader.Length)
        {
            var size = new byte[sizeof(int)];
            var header = new byte[validHeader.Length];

            _ = input.Seek(0, SeekOrigin.Begin);
            _ = input.Read(size, 0, size.Length);
            _ = input.Read(header, 0, header.Length);

            if (Enumerable.SequenceEqual(header, validHeader))
            {
                var extracted = new byte[0x80000];
                var extractedSize = 0;

                using (var deflate = new DeflateStream(input, CompressionMode.Decompress, true))
                    extractedSize = deflate.Read(extracted, 0, extracted.Length);

                _ = output.Seek(0, SeekOrigin.Begin);
                output.Write(extracted, 0, extractedSize);

                succeeded = true;
            }
            else
            {
                // Invalid header
            }
        }
        else
        {
            // The input stream is too short
        }

        return succeeded;
    }

    private static AllScoreData? Read(Stream input)
    {
        using var reader = new BinaryReader(input, EncodingHelper.UTF8NoBOM, true);
        var allScoreData = new AllScoreData();

        try
        {
            allScoreData.ReadFrom(reader);
        }
        catch (EndOfStreamException)
        {
        }

        return allScoreData;
    }
}
