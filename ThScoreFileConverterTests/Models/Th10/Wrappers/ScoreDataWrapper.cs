using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Th10.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0703.
    internal sealed class ScoreDataWrapper<TParent, TStageProgress>
        where TParent : ThConverter
        where TStageProgress : struct, Enum
    {
        private static readonly Type ParentType = typeof(TParent);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+ScoreData";

        private readonly PrivateObject pobj = null;

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static ScoreDataWrapper<TParent, TStageProgress> Create(byte[] array)
        {
            var scoreData = new ScoreDataWrapper<TParent, TStageProgress>();

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

        public ScoreDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);
        public ScoreDataWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        public object Target
            => this.pobj.Target;
        public uint? Score
            => this.pobj.GetProperty(nameof(Score)) as uint?;
        public TStageProgress? StageProgress
            => this.pobj.GetProperty(nameof(StageProgress)) as TStageProgress?;
        public byte? ContinueCount
            => this.pobj.GetProperty(nameof(ContinueCount)) as byte?;
        public IReadOnlyCollection<byte> Name
            => this.pobj.GetProperty(nameof(Name)) as byte[];
        public uint? DateTime
            => this.pobj.GetProperty(nameof(DateTime)) as uint?;
        public float? SlowRate
            => this.pobj.GetProperty(nameof(SlowRate)) as float?;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
