//-----------------------------------------------------------------------
// <copyright file="Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th075;
using ThScoreFileConverter.Core.Resources;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th075;

#if !DEBUG
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Instantiated by ThConverterFactory.")]
#endif
internal sealed class Converter : ThConverter
{
    private AllScoreData? allScoreData;

    public override string SupportedVersions { get; } = "1.11";

    protected override bool ReadScoreFile(Stream input)
    {
#if DEBUG
        using (var decoded = new FileStream("th075decoded.dat", FileMode.Create, FileAccess.ReadWrite))
        {
            var size = (int)input.Length;
            var data = new byte[size];
            input.ReadExactly(data, 0, size);
            decoded.Write(data, 0, size);
            decoded.Flush();
            decoded.SetLength(decoded.Position);
        }
#endif

        input.Seek(0, SeekOrigin.Begin);
        this.allScoreData = Read(input);

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
            new ScoreReplacer(this.allScoreData.ClearData, formatter),
            new CareerReplacer(this.allScoreData.ClearData, formatter),
            new CardReplacer(this.allScoreData.ClearData, hideUntriedCards),
            new CollectRateReplacer(this.allScoreData.ClearData, formatter),
            new CharaReplacer(this.allScoreData.ClearData, formatter),
        ];
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

        var numCharas = EnumHelper<CharaWithReserved>.NumValues;

        var numLevels = EnumHelper<Level>.NumValues;
        if ((allScoreData.ClearData.Count == numCharas * numLevels) &&
            (allScoreData.Status is not null))
            return allScoreData;
        else
            return null;
    }
}
