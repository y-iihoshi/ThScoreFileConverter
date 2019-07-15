using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0053.
    internal sealed class Th16ScoreDataWrapper
    {
        private static readonly Type ParentType = typeof(Th16Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+ScoreData";

        private readonly PrivateObject pobj = null;

        public static Th16ScoreDataWrapper Create(byte[] array)
        {
            var scoreData = new Th16ScoreDataWrapper();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    scoreData.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return scoreData;
        }

        public Th16ScoreDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);
        public Th16ScoreDataWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public uint? Score
            => this.pobj.GetProperty(nameof(Score)) as uint?;
        public Th16Converter.StageProgress? StageProgress
            => this.pobj.GetProperty(nameof(StageProgress)) as Th16Converter.StageProgress?;
        public byte? ContinueCount
            => this.pobj.GetProperty(nameof(ContinueCount)) as byte?;
        public IReadOnlyCollection<byte> Name
            => this.pobj.GetProperty(nameof(Name)) as byte[];
        public uint? DateTime
            => this.pobj.GetProperty(nameof(DateTime)) as uint?;
        public float? SlowRate
            => this.pobj.GetProperty(nameof(SlowRate)) as float?;
        public Th16Converter.Season? Season
            => this.pobj.GetProperty(nameof(Season)) as Th16Converter.Season?;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
