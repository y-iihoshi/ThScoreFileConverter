using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th105;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050 and CS0703.
    internal sealed class Th105AllScoreDataWrapper<TParent, TChara, TLevel>
        where TParent : ThConverter
        where TChara : struct, Enum
        where TLevel : struct, Enum
    {
        private static readonly Type ParentType = typeof(TParent);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+AllScoreData";

        private readonly PrivateObject pobj = null;

        public static Th105AllScoreDataWrapper<TParent, TChara, TLevel> Create(byte[] array)
        {
            var allScoreData = new Th105AllScoreDataWrapper<TParent, TChara, TLevel>();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    allScoreData.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return allScoreData;
        }

        public Th105AllScoreDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;

        public IReadOnlyDictionary<TChara, byte> StoryClearCounts
            => this.pobj.GetProperty(nameof(this.StoryClearCounts)) as IReadOnlyDictionary<TChara, byte>;
        public IReadOnlyDictionary<int, ICardForDeck> SystemCards
            => this.pobj.GetProperty(nameof(this.SystemCards)) as IReadOnlyDictionary<int, ICardForDeck>;
        public IReadOnlyDictionary<TChara, IClearData<TChara, TLevel>> ClearData
            => this.pobj.GetProperty(nameof(this.ClearData)) as IReadOnlyDictionary<TChara, IClearData<TChara, TLevel>>;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
