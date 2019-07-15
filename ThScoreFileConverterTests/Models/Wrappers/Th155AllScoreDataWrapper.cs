using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050 and CS0051.
    internal sealed class Th155AllScoreDataWrapper
    {
        private static readonly Type ParentType = typeof(Th155Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+AllScoreData";
        private static readonly PrivateType PrivateType = new PrivateType(AssemblyNameToTest, TypeNameToTest);

        private readonly PrivateObject pobj = null;

        public static Th155AllScoreDataWrapper Create(byte[] array)
        {
            var allScoreData = new Th155AllScoreDataWrapper();

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

        public Th155AllScoreDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;

        // NOTE: Th155Converter.Story is a private struct.
        // public IReadOnlyDictionary<StoryChara, Story> StoryDictionary
        //     => this.pobj.GetProperty(nameof(StoryDictionary)) as Dictionary<StoryChara, Story>;
        public object StoryDictionary
            => this.pobj.GetProperty(nameof(StoryDictionary));
        public int? StoryDictionaryCount
            => this.StoryDictionary.GetType().GetProperty("Count").GetValue(this.StoryDictionary) as int?;
        public Th155StoryWrapper StoryDictionaryItem(Th155Converter.StoryChara chara)
            => new Th155StoryWrapper(this.StoryDictionary.GetType().GetProperty("Item").GetValue(
                this.StoryDictionary, new object[] { chara }));

        public IReadOnlyDictionary<string, int> CharacterDictionary
            => this.pobj.GetProperty(nameof(CharacterDictionary)) as Dictionary<string, int>;
        public IReadOnlyDictionary<int, bool> BgmDictionary
            => this.pobj.GetProperty(nameof(BgmDictionary)) as Dictionary<int, bool>;
        public IReadOnlyDictionary<string, int> EndingDictionary
            => this.pobj.GetProperty(nameof(EndingDictionary)) as Dictionary<string, int>;
        public IReadOnlyDictionary<int, int> StageDictionary
            => this.pobj.GetProperty(nameof(StageDictionary)) as Dictionary<int, int>;
        public int? Version
            => this.pobj.GetProperty(nameof(Version)) as int?;

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
