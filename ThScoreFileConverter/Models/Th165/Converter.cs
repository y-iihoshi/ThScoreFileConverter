//-----------------------------------------------------------------------
// <copyright file="Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Models.Th165;
using ThScoreFileConverter.Core.Resources;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th165;

#if !DEBUG
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Instantiated by ThConverterFactory.")]
#endif
internal sealed class Converter : ThConverter
{
    private readonly Dictionary<(Day Day, int Scene), (string Path, IBestShotHeader Header)> bestshots =
        new(Definitions.SpellCards.Count);

    private AllScoreData? allScoreData;

    public override string SupportedVersions => "1.00a";

    public override bool HasBestShotConverter => true;

    protected override bool ReadScoreFile(Stream input)
    {
        using var decrypted = new MemoryStream();
        using var decoded = StreamHelper.Create("th165decoded.dat", FileMode.Create, FileAccess.ReadWrite);

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
        if ((this.allScoreData is null) || (this.allScoreData.Status is null))
        {
            ThrowHelper.ThrowInvalidDataException(
                StringHelper.Format(ExceptionMessages.InvalidOperationExceptionMustBeInvokedAfter, nameof(this.ReadScoreFile)));
        }

        return
        [
            new ScoreReplacer(this.allScoreData.Scores, formatter),
            new ScoreTotalReplacer(this.allScoreData.Scores, this.allScoreData.Status, formatter),
            new CardReplacer(this.allScoreData.Scores, hideUntriedCards),
            new NicknameReplacer(this.allScoreData.Status),
            new TimeReplacer(this.allScoreData.Status),
            new ShotReplacer(this.bestshots, outputFilePath),
            new ShotExReplacer(this.bestshots, formatter, outputFilePath),
        ];
    }

    protected override string[] FilterBestShotFiles(string[] files)
    {
        var pairs = new[]
        {
            (Day.Sunday,             "01"),
            (Day.Monday,             "02"),
            (Day.Tuesday,            "03"),
            (Day.Wednesday,          "04"),
            (Day.Thursday,           "05"),
            (Day.Friday,             "06"),
            (Day.Saturday,           "07"),
            (Day.WrongSunday,        "08"),
            (Day.WrongMonday,        "09"),
            (Day.WrongTuesday,       "10"),
            (Day.WrongWednesday,     "11"),
            (Day.WrongThursday,      "12"),
            (Day.WrongFriday,        "13"),
            (Day.WrongSaturday,      "14"),
            (Day.NightmareSunday,    "15"),
            (Day.NightmareMonday,    "16"),
            (Day.NightmareTuesday,   "17"),
            (Day.NightmareWednesday, "18"),
            (Day.NightmareThursday,  "19"),
            (Day.NightmareFriday,    "20"),
            (Day.NightmareSaturday,  "21"),
            (Day.NightmareDiary,     "22"),
        };
        var dayPattern = string.Join("|", pairs.Select(pair => pair.Item2));
        var fileNamePattern = StringHelper.Create($@"bs({dayPattern})_\d{{2}}.dat");

        return files.Where(file => Regex.IsMatch(Path.GetFileName(file), fileNamePattern, RegexOptions.IgnoreCase)).ToArray();
    }

    protected override void ConvertBestShot(Stream input, Stream output)
    {
        using var decoded = new MemoryStream();

        Guard.IsTrue(output is FileStream, nameof(output), ExceptionMessages.ArgumentExceptionWrongType);
        var outputFile = (FileStream)output;

        var header = BestShotDeveloper.Develop<BestShotHeader>(input, output, PixelFormat.Format32bppArgb);

        var key = (header.Weekday, header.Dream);
        _ = this.bestshots.TryAdd(key, (outputFile.Name, header));
    }

    private static bool Decrypt(Stream input, Stream output)
    {
        using var reader = new BinaryReader(input, EncodingHelper.UTF8NoBOM, true);
        using var writer = new BinaryWriter(output, EncodingHelper.UTF8NoBOM, true);
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
        using var reader = new BinaryReader(input, EncodingHelper.UTF8NoBOM, true);
        using var writer = new BinaryWriter(output, EncodingHelper.UTF8NoBOM, true);

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
        using var reader = new BinaryReader(input, EncodingHelper.UTF8NoBOM, true);

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

    private static AllScoreData? Read(Stream input)
    {
        var dictionary = new Dictionary<string, Action<AllScoreData, Th10.Chapter>>
        {
            { Score.ValidSignature,  (data, ch) => data.Set(new Score(ch))  },
            { Status.ValidSignature, (data, ch) => data.Set(new Status(ch)) },
        };

        using var reader = new BinaryReader(input, EncodingHelper.UTF8NoBOM, true);
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
            //// (allScoreData.scores.Count >= 0) &&
            (allScoreData.Status is not null))
            return allScoreData;
        else
            return null;
    }
}
