//-----------------------------------------------------------------------
// <copyright file="Th105Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // ElementsMustBeDocumented

using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th105;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th105;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.Models;

#if !DEBUG
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Instantiated by ThConverterFactory.")]
#endif
internal class Th105Converter : ThConverter
{
    private AllScoreData? allScoreData;

    public override string SupportedVersions { get; } = "1.06a";

    protected override bool ReadScoreFile(Stream input)
    {
        using var decrypted = new MemoryStream();
#if DEBUG
        using var decoded = new FileStream("th105decoded.dat", FileMode.Create, FileAccess.ReadWrite);
#else
        using var decoded = new MemoryStream();
#endif

        if (!Decrypt(input, decrypted))
            return false;

        decrypted.Seek(0, SeekOrigin.Begin);
        if (!Extract(decrypted, decoded))
            return false;

        decoded.Seek(0, SeekOrigin.Begin);
        this.allScoreData = Read(decoded);

        return this.allScoreData is not null;
    }

    protected override IEnumerable<IStringReplaceable> CreateReplacers(
        INumberFormatter formatter, bool hideUntriedCards, string outputFilePath)
    {
        if (this.allScoreData is null)
        {
            throw new InvalidDataException(
                Utils.Format(Resources.InvalidOperationExceptionMustBeInvokedAfter, nameof(this.ReadScoreFile)));
        }

        return new List<IStringReplaceable>
        {
            new CareerReplacer(this.allScoreData.ClearData, formatter),
            new CardReplacer(this.allScoreData.ClearData, hideUntriedCards),
            new CollectRateReplacer(this.allScoreData.ClearData, formatter),
            new CardForDeckReplacer(this.allScoreData.SystemCards, this.allScoreData.ClearData, formatter, hideUntriedCards),
        };
    }

    private static bool Decrypt(Stream input, Stream output)
    {
        var size = (int)input.Length;
        var inData = new byte[size];
        var outData = new byte[size];

        _ = input.Seek(0, SeekOrigin.Begin);
        _ = input.Read(inData, 0, size);

        for (var index = 0; index < size; index++)
            outData[index] = (byte)((index * 7) ^ inData[size - index - 1]);

        _ = output.Seek(0, SeekOrigin.Begin);
        output.Write(outData, 0, size);

        // See section 2.2 of RFC 1950
        return (outData[0] == 0x78) && (outData[1] == 0x9C);
    }

    private static bool Extract(Stream input, Stream output)
    {
        var extracted = new byte[0x80000];
        var extractedSize = 0;

        // Skip the header bytes of a zlib stream
        _ = input.Seek(2, SeekOrigin.Begin);

        using (var deflate = new DeflateStream(input, CompressionMode.Decompress, true))
            extractedSize = deflate.Read(extracted, 0, extracted.Length);

        _ = output.Seek(0, SeekOrigin.Begin);
        output.Write(extracted, 0, extractedSize);

        return true;
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

        if (allScoreData.ClearData.Count == EnumHelper<Chara>.NumValues)
            return allScoreData;
        else
            return null;
    }
}
