using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050 and CS0703.
    internal sealed class Th135AllScoreDataWrapper
    {
        private static Type ParentType = typeof(Th135Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+AllScoreData";
        private static readonly PrivateType PrivateType = new PrivateType(AssemblyNameToTest, TypeNameToTest);

        private readonly PrivateObject pobj = null;

        public static Th135AllScoreDataWrapper Create(byte[] array)
        {
            var allScoreData = new Th135AllScoreDataWrapper();

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

        public Th135AllScoreDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;

        public int? StoryProgress
            => this.pobj.GetProperty(nameof(StoryProgress)) as int?;
        public IReadOnlyDictionary<Th135Converter.Chara, Th135Converter.LevelFlag> StoryClearFlags
            => this.pobj.GetProperty(nameof(StoryClearFlags))
                as Dictionary<Th135Converter.Chara, Th135Converter.LevelFlag>;
        public int? EndingCount
            => this.pobj.GetProperty(nameof(EndingCount)) as int?;
        public int? Ending2Count
            => this.pobj.GetProperty(nameof(Ending2Count)) as int?;
        public bool? IsEnabledStageTanuki1
            => this.pobj.GetProperty(nameof(IsEnabledStageTanuki1)) as bool?;
        public bool? IsEnabledStageTanuki2
            => this.pobj.GetProperty(nameof(IsEnabledStageTanuki2)) as bool?;
        public bool? IsEnabledStageKokoro
            => this.pobj.GetProperty(nameof(IsEnabledStageKokoro)) as bool?;
        public bool? IsPlayableMamizou
            => this.pobj.GetProperty(nameof(IsPlayableMamizou)) as bool?;
        public bool? IsPlayableKokoro
            => this.pobj.GetProperty(nameof(IsPlayableKokoro)) as bool?;
        public IReadOnlyDictionary<int, bool> BgmFlags
            => this.pobj.GetProperty(nameof(BgmFlags)) as Dictionary<int, bool>;

        public static bool ReadObject(BinaryReader reader, out object obj)
        {
            var args = new object[] { reader, null };
            var result = (bool)PrivateType.InvokeStatic(nameof(ReadObject), args, CultureInfo.InvariantCulture);
            obj = args[1];
            return result;
        }

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
