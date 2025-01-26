﻿//-----------------------------------------------------------------------
// <copyright file="Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th06;
using ThScoreFileConverter.Core.Resources;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th06;

#if !DEBUG
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Instantiated by ThConverterFactory.")]
#endif
internal sealed class Converter : ThConverter
{
    private AllScoreData? allScoreData;

    public override string SupportedVersions { get; } = "1.02h";

    protected override bool ReadScoreFile(Stream input)
    {
        using var decrypted = new MemoryStream();
        using var decoded = StreamHelper.Create("th06decoded.dat", FileMode.Create, FileAccess.ReadWrite);

        if (!Decrypt(input, decrypted))
            return false;

        _ = decrypted.Seek(0, SeekOrigin.Begin);
        if (!Extract(decrypted, decoded))
            return false;

        _ = decoded.Seek(0, SeekOrigin.Begin);
        if (!Validate(decoded))
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
            ThrowHelper.ThrowInvalidOperationException(
                StringHelper.Format(ExceptionMessages.InvalidOperationExceptionMustBeInvokedAfter, nameof(this.ReadScoreFile)));
        }

        return
        [
            new ScoreReplacer(this.allScoreData.Rankings, formatter),
            new CareerReplacer(this.allScoreData.CardAttacks, formatter),
            new CardReplacer(this.allScoreData.CardAttacks, hideUntriedCards),
            new CollectRateReplacer(this.allScoreData.CardAttacks, formatter),
            new ClearReplacer(this.allScoreData.Rankings),
            new PracticeReplacer(this.allScoreData.PracticeScores, formatter),
        ];
    }

    private static bool Decrypt(Stream input, Stream output)
    {
        var size = (int)input.Length;
        var data = new byte[size];
        _ = input.Read(data, 0, size);

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

    private static bool Extract(Stream input, Stream output)
    {
        using var reader = new BinaryReader(input, EncodingHelper.UTF8NoBOM, true);
        using var writer = new BinaryWriter(output, EncodingHelper.UTF8NoBOM, true);
        var header = new FileHeader();

        header.ReadFrom(reader);
        if (!header.IsValid)
            return false;
        if (header.DecodedAllSize != input.Length)
            return false;

        header.WriteTo(writer);

#if false
        Lzss.Extract(input, output);
#else
        var body = new byte[header.DecodedAllSize - header.Size];
        input.ReadExactly(body, 0, body.Length);
        output.Write(body, 0, body.Length);
#endif
        output.Flush();
        output.SetLength(output.Position);

        return output.Position == header.DecodedAllSize;
    }

    private static bool Validate(Stream input)
    {
        using var reader = new BinaryReader(input, EncodingHelper.UTF8NoBOM, true);
        var header = new FileHeader();
        var chapter = new Chapter();

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
                        if (chapter.FirstByteOfData != 0x10)
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
        var dictionary = new Dictionary<string, Action<AllScoreData, Chapter>>
        {
            { Header.ValidSignature,        (data, ch) => data.Set(new Header(ch))        },
            { HighScore.ValidSignature,     (data, ch) => data.Set(new HighScore(ch))     },
            { ClearData.ValidSignature,     (data, ch) => data.Set(new ClearData(ch))     },
            { CardAttack.ValidSignature,    (data, ch) => data.Set(new CardAttack(ch))    },
            { PracticeScore.ValidSignature, (data, ch) => data.Set(new PracticeScore(ch)) },
        };

        using var reader = new BinaryReader(input, EncodingHelper.UTF8NoBOM, true);
        var allScoreData = new AllScoreData();
        var chapter = new Chapter();

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
            //// (allScoreData.rankings.Count >= 0) &&
            //// (allScoreData.cardAttacks.Length == NumCards) &&
            //// (allScoreData.practiceScores.Count >= 0) &&
            (allScoreData.ClearData.Count == EnumHelper<Chara>.NumValues))
            return allScoreData;
        else
            return null;
    }
}
