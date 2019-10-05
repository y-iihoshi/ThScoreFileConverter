//-----------------------------------------------------------------------
// <copyright file="Th075Converter.cs" company="None">
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
using System.Linq;
using ThScoreFileConverter.Models.Th075;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th075Converter : ThConverter
    {
        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get { return "1.11"; }
        }

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

            return this.allScoreData != null;
        }

        protected override IEnumerable<IStringReplaceable> CreateReplacers(
            bool hideUntriedCards, string outputFilePath)
        {
            return new List<IStringReplaceable>
            {
                new ScoreReplacer(this.allScoreData.ClearData),
                new CareerReplacer(this.allScoreData.ClearData),
                new CardReplacer(this.allScoreData.ClearData, hideUntriedCards),
                new CollectRateReplacer(this.allScoreData.ClearData),
                new CharaReplacer(this.allScoreData.ClearData),
            };
        }

        private static AllScoreData Read(Stream input)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var allScoreData = new AllScoreData();

                try
                {
                    allScoreData.ReadFrom(reader);
                }
                catch (EndOfStreamException)
                {
                }

                var numCharas = Enum.GetValues(typeof(CharaWithReserved)).Length;
                var numLevels = Enum.GetValues(typeof(Th075.Level)).Length;
                if ((allScoreData.ClearData.Count == numCharas * numLevels) &&
                    (allScoreData.Status != null))
                    return allScoreData;
                else
                    return null;
            }
        }

        private class AllScoreData : IBinaryReadable
        {
            public AllScoreData()
            {
                var numCharas = Enum.GetValues(typeof(CharaWithReserved)).Length;
                var numLevels = Enum.GetValues(typeof(Th075.Level)).Length;
                this.ClearData = new Dictionary<(CharaWithReserved, Th075.Level), ClearData>(numCharas * numLevels);
            }

            public IReadOnlyDictionary<(CharaWithReserved chara, Th075.Level level), ClearData> ClearData { get; private set; }

            public Status Status { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                var levels = Utils.GetEnumerator<Th075.Level>();

                this.ClearData = Utils.GetEnumerator<CharaWithReserved>()
                    .SelectMany(chara => levels.Select(level => (chara, level)))
                    .ToDictionary(pair => pair, pair =>
                    {
                        var clearData = new ClearData();
                        clearData.ReadFrom(reader);
                        return clearData;
                    });

                var status = new Status();
                status.ReadFrom(reader);
                this.Status = status;
            }
        }
    }
}
