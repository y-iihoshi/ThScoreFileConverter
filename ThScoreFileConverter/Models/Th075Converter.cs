//-----------------------------------------------------------------------
// <copyright file="Th075Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // ElementsMustBeDocumented

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th075;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.Models
{
#if !DEBUG
    [SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Instantiated by ThConverterFactory.")]
#endif
    internal class Th075Converter : ThConverter
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
                input.Read(data, 0, size);
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
                throw new InvalidDataException(
                    Utils.Format(Resources.InvalidOperationExceptionMustBeInvokedAfter, nameof(this.ReadScoreFile)));
            }

            return new List<IStringReplaceable>
            {
                new ScoreReplacer(this.allScoreData.ClearData, formatter),
                new CareerReplacer(this.allScoreData.ClearData, formatter),
                new CardReplacer(this.allScoreData.ClearData, hideUntriedCards),
                new CollectRateReplacer(this.allScoreData.ClearData, formatter),
                new CharaReplacer(this.allScoreData.ClearData, formatter),
            };
        }

        private static AllScoreData? Read(Stream input)
        {
            using var reader = new BinaryReader(input, Encoding.UTF8, true);
            var allScoreData = new AllScoreData();

            try
            {
                allScoreData.ReadFrom(reader);
            }
            catch (EndOfStreamException)
            {
            }

            var numCharas = EnumHelper<CharaWithReserved>.NumValues;
            var numLevels = EnumHelper<Th075.Level>.NumValues;
            if ((allScoreData.ClearData.Count == numCharas * numLevels) &&
                (allScoreData.Status is not null))
                return allScoreData;
            else
                return null;
        }
    }
}
