using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th10AllScoreDataWrapper<TParent, TCharaWithTotal, TStageProgress>
        where TParent : ThConverter
        where TCharaWithTotal : struct, Enum
        where TStageProgress : struct, Enum
    {
        private static readonly Type ParentType = typeof(TParent);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+AllScoreData";

        private readonly PrivateObject pobj = null;

        public Th10AllScoreDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public ThScoreFileConverter.Models.Th095.HeaderBase Header
            => this.pobj.GetProperty(nameof(this.Header)) as ThScoreFileConverter.Models.Th095.HeaderBase;
        public IReadOnlyDictionary<TCharaWithTotal, IClearData<TCharaWithTotal, TStageProgress>> ClearData
            => this.pobj.GetProperty(nameof(this.ClearData))
                as IReadOnlyDictionary<TCharaWithTotal, IClearData<TCharaWithTotal, TStageProgress>>;
        public IStatus Status
            => this.pobj.GetProperty(nameof(this.Status)) as IStatus;

        public void Set(ThScoreFileConverter.Models.Th095.HeaderBase header)
            => this.pobj.Invoke(nameof(Set), new object[] { header }, CultureInfo.InvariantCulture);
        public void Set(IClearData<TCharaWithTotal, TStageProgress> data)
            => this.pobj.Invoke(nameof(Set), new object[] { data }, CultureInfo.InvariantCulture);
        public void Set(IStatus status)
            => this.pobj.Invoke(nameof(Set), new object[] { status }, CultureInfo.InvariantCulture);
    }
}
