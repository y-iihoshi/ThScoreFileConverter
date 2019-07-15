using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051.
    internal sealed class Th143ScoreWrapper
    {
        private static readonly Type ParentType = typeof(Th143Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+Score";
        private static readonly PrivateType PrivateType = new PrivateType(AssemblyNameToTest, TypeNameToTest);

        private readonly PrivateObject pobj = null;

        public Th143ScoreWrapper(Th10ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th143ScoreWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        public object Target
            => this.pobj.Target;
        public string Signature
            => this.pobj.GetProperty(nameof(this.Signature)) as string;
        public ushort? Version
            => this.pobj.GetProperty(nameof(this.Version)) as ushort?;
        public uint? Checksum
            => this.pobj.GetProperty(nameof(this.Checksum)) as uint?;
        public int? Size
            => this.pobj.GetProperty(nameof(this.Size)) as int?;
        public bool? IsValid
            => this.pobj.GetProperty(nameof(this.IsValid)) as bool?;
        public IReadOnlyCollection<byte> Data
            => this.pobj.GetProperty(nameof(this.Data)) as byte[];
        public int? Number
            => this.pobj.GetProperty(nameof(this.Number)) as int?;
        public IReadOnlyDictionary<Th143Converter.ItemWithTotal, int> ClearCounts
            => this.pobj.GetProperty(nameof(this.ClearCounts)) as Dictionary<Th143Converter.ItemWithTotal, int>;
        public IReadOnlyDictionary<Th143Converter.ItemWithTotal, int> ChallengeCounts
            => this.pobj.GetProperty(nameof(this.ChallengeCounts)) as Dictionary<Th143Converter.ItemWithTotal, int>;
        public int? HighScore
            => this.pobj.GetProperty(nameof(this.HighScore)) as int?;

        public static bool CanInitialize(Th10ChapterWrapper chapter)
            => (bool)PrivateType.InvokeStatic(
                nameof(CanInitialize), new object[] { chapter.Target }, CultureInfo.InvariantCulture);
    }
}
