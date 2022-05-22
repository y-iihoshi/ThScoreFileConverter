//-----------------------------------------------------------------------
// <copyright file="Th09Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // ElementsMustBeDocumented

using System;
using System.Collections.Generic;
using System.IO;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th09;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.Models;

#if !DEBUG
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Instantiated by ThConverterFactory.")]
#endif
internal class Th09Converter : ThConverter
{
    private AllScoreData? allScoreData;

    public override string SupportedVersions { get; } = "1.50a";

    public override bool HasCardReplacer { get; }

    protected override bool ReadScoreFile(Stream input)
    {
        using var decrypted = new MemoryStream();
#if DEBUG
        using var decoded = new FileStream("th09decoded.dat", FileMode.Create, FileAccess.ReadWrite);
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

    protected override IEnumerable<IStringReplaceable> CreateReplacers(
        INumberFormatter formatter, bool hideUntriedCards, string outputFilePath)
    {
        if ((this.allScoreData is null) || (this.allScoreData.PlayStatus is null))
        {
            throw new InvalidDataException(
                Utils.Format(Resources.InvalidOperationExceptionMustBeInvokedAfter, nameof(this.ReadScoreFile)));
        }

        return new List<IStringReplaceable>
        {
            new ScoreReplacer(this.allScoreData.Rankings, formatter),
            new TimeReplacer(this.allScoreData.PlayStatus),
            new ClearReplacer(this.allScoreData.Rankings, this.allScoreData.PlayStatus.ClearCounts, formatter),
        };
    }

    private static bool Decrypt(Stream input, Stream output)
    {
        var size = (int)input.Length;
        ThCrypt.Decrypt(input, output, size, 0x3A, 0xCD, 0x0100, 0x0C00);

        var data = new byte[size];
        _ = output.Seek(0, SeekOrigin.Begin);
        _ = output.Read(data, 0, size);

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

        _ = output.Seek(0, SeekOrigin.Begin);
        output.Write(data, 0, size);

        return (ushort)checksum == BitConverter.ToUInt16(data, 2);
    }

    private static bool Extract(Stream input, Stream output)
    {
        using var reader = new BinaryReader(input, EncodingHelper.UTF8NoBOM, true);
        using var writer = new BinaryWriter(output, EncodingHelper.UTF8NoBOM, true);
        var header = new FileHeader();

        header.ReadFrom(reader);
        if (!header.IsValid)
            return false;
        if (header.Size + header.EncodedBodySize != input.Length)
            return false;

        header.WriteTo(writer);

        Lzss.Decompress(input, output);
        output.Flush();
        output.SetLength(output.Position);

        return output.Position == header.DecodedAllSize;
    }

    private static bool Validate(Stream input)
    {
        using var reader = new BinaryReader(input, EncodingHelper.UTF8NoBOM, true);
        var header = new FileHeader();
        var chapter = new Th06.Chapter();

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

    private static AllScoreData? Read(Stream input)
    {
        var dictionary = new Dictionary<string, Action<AllScoreData, Th06.Chapter>>
        {
            { Header.ValidSignature,           (data, ch) => data.Set(new Header(ch))           },
            { HighScore.ValidSignature,        (data, ch) => data.Set(new HighScore(ch))        },
            { PlayStatus.ValidSignature,       (data, ch) => data.Set(new PlayStatus(ch))       },
            { Th07.LastName.ValidSignature,    (data, ch) => data.Set(new Th07.LastName(ch))    },
            { Th07.VersionInfo.ValidSignature, (data, ch) => data.Set(new Th07.VersionInfo(ch)) },
        };

        using var reader = new BinaryReader(input, EncodingHelper.UTF8NoBOM, true);
        var allScoreData = new AllScoreData();
        var chapter = new Th06.Chapter();

        _ = reader.ReadExactBytes(FileHeader.ValidSize);

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
            (allScoreData.Rankings.Count == EnumHelper<Chara>.NumValues * EnumHelper<Level>.NumValues) &&
            (allScoreData.PlayStatus is not null) &&
            (allScoreData.LastName is not null) &&
            (allScoreData.VersionInfo is not null))
            return allScoreData;
        else
            return null;
    }
}
