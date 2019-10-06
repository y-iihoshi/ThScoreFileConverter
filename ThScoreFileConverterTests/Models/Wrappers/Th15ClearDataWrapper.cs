using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverterTests.Models.Th10.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th15ClearDataWrapper
    {
        private static readonly Type ParentType = typeof(Th15Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+ClearData";
        private static readonly PrivateType PrivateType = new PrivateType(AssemblyNameToTest, TypeNameToTest);

        private readonly PrivateObject pobj = null;

        public Th15ClearDataWrapper(ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th15ClearDataWrapper(object original)
            => this.pobj = new PrivateObject(original);

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
        public Th15Converter.CharaWithTotal? Chara
            => this.pobj.GetProperty(nameof(this.Chara)) as Th15Converter.CharaWithTotal?;
        // NOTE: Th15Converter.ClearDataPerGameMode is a private class.
        // public IReadOnlyDictionary<GameMode, ClearDataPerGameMode> Data1
        //     => this.pobj.GetProperty(nameof(this.Data1)) as Dictionary<GameMode, ClearDataPerGameMode>;
        public object Data1
            => this.pobj.GetProperty(nameof(this.Data1));
        public Th15ClearDataPerGameModeWrapper Data1Item(Th15Converter.GameMode mode)
            => new Th15ClearDataPerGameModeWrapper(
                this.Data1.GetType().GetProperty("Item").GetValue(this.Data1, new object[] { mode }));
        public IReadOnlyDictionary<(Level, Th15Converter.StagePractice), IPractice> Practices
            => this.pobj.GetProperty(nameof(this.Practices))
                as Dictionary<(Level, Th15Converter.StagePractice), IPractice>;

        public static bool CanInitialize(ChapterWrapper chapter)
            => (bool)PrivateType.InvokeStatic(
                nameof(CanInitialize), new object[] { chapter.Target }, CultureInfo.InvariantCulture);
    }
}
