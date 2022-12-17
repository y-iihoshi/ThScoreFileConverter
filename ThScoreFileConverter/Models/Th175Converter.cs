//-----------------------------------------------------------------------
// <copyright file="Th175Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // ElementsMustBeDocumented

using System.Collections.Generic;
using System.IO;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Resources;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th175;

namespace ThScoreFileConverter.Models;

#if !DEBUG
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Instantiated by ThConverterFactory.")]
#endif
internal class Th175Converter : ThConverter
{
    private AllScoreData? allScoreData;

    public override string SupportedVersions { get; } = "1.04";

    public override bool HasCardReplacer { get; }

    protected override bool ReadScoreFile(Stream input)
    {
#if DEBUG
        using var decoded = new FileStream("th175decoded.dat", FileMode.Create, FileAccess.ReadWrite);
#else
        using var decoded = new MemoryStream();
#endif

        if (!Extract(input, decoded))
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
            ThrowHelper.ThrowInvalidDataException(
                Utils.Format(ExceptionMessages.InvalidOperationExceptionMustBeInvokedAfter, nameof(this.ReadScoreFile)));
        }

        return new List<IStringReplaceable>
        {
            new ScoreReplacer(
                this.allScoreData.SaveDataDictionary[0].ScoreDictionary,
                this.allScoreData.SaveDataDictionary[0].TimeDictionary,
                formatter),
            new CharaReplacer(
                this.allScoreData.SaveDataDictionary[0].UseCountDictionary,
                this.allScoreData.SaveDataDictionary[0].RetireCountDictionary,
                this.allScoreData.SaveDataDictionary[0].ClearCountDictionary,
                this.allScoreData.SaveDataDictionary[0].PerfectClearCountDictionary,
                formatter),
        };
    }

    private static bool Extract(Stream input, Stream output)
    {
        using var reader = new BinaryReader(input, EncodingHelper.UTF8NoBOM, true);
        using var writer = new BinaryWriter(output, EncodingHelper.UTF8NoBOM, true);

        var initialKey = reader.ReadInt32();
        var key = 0;

        var size = input.Length - input.Position;
        for (var index = 0; index < size; index += sizeof(int))
        {
            var value = RandomHelper.ParkMillerRNG(index ^ initialKey);
            var decoded = reader.ReadInt32() ^ value;
            writer.Write(decoded);

            key ^= decoded;
        }

        return key == initialKey;
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
