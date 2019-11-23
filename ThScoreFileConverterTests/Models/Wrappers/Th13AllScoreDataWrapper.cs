using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using HeaderBase = ThScoreFileConverter.Models.Th095.HeaderBase;
using IStatus = ThScoreFileConverter.Models.Th125.IStatus;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th13AllScoreDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>
        where TParent : ThConverter
        where TChWithT : struct, Enum       // TCharaWithTotal
        where TLv : struct, Enum            // TLevel
        where TLvPrac : struct, Enum        // TLevelPractice
        where TLvPracWithT : struct, Enum   // TLevelPracticeWithTotal
        where TStPrac : struct, Enum        // TStagePractice
    {
        private static readonly Type ParentType = typeof(TParent);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+AllScoreData";

        private readonly PrivateObject pobj = null;

        public Th13AllScoreDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public HeaderBase Header
            => this.pobj.GetProperty(nameof(this.Header)) as HeaderBase;
        public IReadOnlyDictionary<TChWithT, IClearData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>> ClearData
            => this.pobj.GetProperty(nameof(this.ClearData))
                as IReadOnlyDictionary<TChWithT, IClearData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>>;
        public IStatus Status
            => this.pobj.GetProperty(nameof(this.Status)) as IStatus;

        public void Set(HeaderBase header)
            => this.pobj.Invoke(nameof(Set), new object[] { header }, CultureInfo.InvariantCulture);
        public void Set(IClearData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac> data)
            => this.pobj.Invoke(nameof(Set), new object[] { data }, CultureInfo.InvariantCulture);
        public void Set(IStatus status)
            => this.pobj.Invoke(nameof(Set), new object[] { status }, CultureInfo.InvariantCulture);
    }
}
