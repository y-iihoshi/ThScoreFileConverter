using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ThScoreFileConverter.Models.Tests
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th07CardAttackWrapper
    {
        private static Type ParentType = typeof(Th07Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+CardAttack";

        private readonly PrivateObject pobj = null;

        public Th07CardAttackWrapper(Th06ChapterWrapper<Th07Converter> chapter)
        {
            if (chapter == null)
            {
                var ch = new Th06ChapterWrapper<Th07Converter>();
                this.pobj = new PrivateObject(
                    AssemblyNameToTest,
                    TypeNameToTest,
                    new Type[] { ch.Target.GetType() },
                    new object[] { null });
            }
            else
            {
                this.pobj = new PrivateObject(
                    AssemblyNameToTest,
                    TypeNameToTest,
                    new Type[] { chapter.Target.GetType() },
                    new object[] { chapter.Target });
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public string Signature
            => this.pobj.GetProperty(nameof(Signature)) as string;
        public short? Size1
            => this.pobj.GetProperty(nameof(Size1)) as short?;
        public short? Size2
            => this.pobj.GetProperty(nameof(Size2)) as short?;
        public byte? FirstByteOfData
            => this.pobj.GetProperty(nameof(FirstByteOfData)) as byte?;
        public IReadOnlyCollection<byte> Data
            => this.pobj.GetProperty(nameof(Data)) as byte[];
        public IReadOnlyDictionary<Th07Converter.CharaWithTotal, uint> MaxBonuses
            => this.pobj.GetProperty(nameof(MaxBonuses)) as Dictionary<Th07Converter.CharaWithTotal, uint>;
        public short? CardId
            => this.pobj.GetProperty(nameof(CardId)) as short?;
        public IReadOnlyCollection<byte> CardName
            => this.pobj.GetProperty(nameof(CardName)) as byte[];
        public IReadOnlyDictionary<Th07Converter.CharaWithTotal, ushort> TrialCounts
            => this.pobj.GetProperty(nameof(TrialCounts)) as Dictionary<Th07Converter.CharaWithTotal, ushort>;
        public IReadOnlyDictionary<Th07Converter.CharaWithTotal, ushort> ClearCounts
            => this.pobj.GetProperty(nameof(ClearCounts)) as Dictionary<Th07Converter.CharaWithTotal, ushort>;

        public bool? HasTried()
            => this.pobj.Invoke(nameof(HasTried), new object[] { }, CultureInfo.InvariantCulture) as bool?;
    }
}
