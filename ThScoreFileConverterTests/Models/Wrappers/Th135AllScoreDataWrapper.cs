using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th135;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0053.
    internal sealed class Th135AllScoreDataWrapper
    {
        private static readonly Type ParentType = typeof(Th135Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+AllScoreData";

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
            => this.pobj.GetProperty(nameof(this.StoryProgress)) as int?;
        public IReadOnlyDictionary<Chara, LevelFlags> StoryClearFlags
            => this.pobj.GetProperty(nameof(this.StoryClearFlags)) as Dictionary<Chara, LevelFlags>;
        public int? EndingCount
            => this.pobj.GetProperty(nameof(this.EndingCount)) as int?;
        public int? Ending2Count
            => this.pobj.GetProperty(nameof(this.Ending2Count)) as int?;
        public bool? IsEnabledStageTanuki1
            => this.pobj.GetProperty(nameof(this.IsEnabledStageTanuki1)) as bool?;
        public bool? IsEnabledStageTanuki2
            => this.pobj.GetProperty(nameof(this.IsEnabledStageTanuki2)) as bool?;
        public bool? IsEnabledStageKokoro
            => this.pobj.GetProperty(nameof(this.IsEnabledStageKokoro)) as bool?;
        public bool? IsPlayableMamizou
            => this.pobj.GetProperty(nameof(this.IsPlayableMamizou)) as bool?;
        public bool? IsPlayableKokoro
            => this.pobj.GetProperty(nameof(this.IsPlayableKokoro)) as bool?;
        public IReadOnlyDictionary<int, bool> BgmFlags
            => this.pobj.GetProperty(nameof(this.BgmFlags)) as Dictionary<int, bool>;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
