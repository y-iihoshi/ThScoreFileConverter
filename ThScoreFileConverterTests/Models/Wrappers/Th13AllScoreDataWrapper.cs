using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th095.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th13AllScoreDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>
        where TParent : ThConverter
        where TChWithT : struct, Enum       // TCharaWithTotal
        where TLv : struct, Enum            // TLevel
        where TLvPrac : struct, Enum        // TLevelPractice
        where TLvPracWithT : struct, Enum   // TLevelPracticeWithTotal
        where TStPrac : struct, Enum        // TStagePractice
        where TStProg : struct, Enum        // TStageProgress
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

        public HeaderWrapper<TParent> Header
        {
            get
            {
                var header = this.pobj.GetProperty(nameof(this.Header));
                return (header != null) ? new HeaderWrapper<TParent>(header) : null;
            }
        }

        // NOTE: Th13Converter.ClearData is a private class.
        // public IReadOnlyDictionary<CharaWithTotal, ClearData> ClearData
        //     => this.pobj.GetProperty(nameof(this.ClearData)) as Dictionary<CharaWithTotal, ClearData>;
        public object ClearData
            => this.pobj.GetProperty(nameof(this.ClearData));
        public int? ClearDataCount
            => this.ClearData.GetType().GetProperty("Count").GetValue(this.ClearData) as int?;
        public Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>
            ClearDataItem(TChWithT chara)
            => new Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg>(
                this.ClearData.GetType().GetProperty("Item").GetValue(this.ClearData, new object[] { chara }));

        public Th128StatusWrapper<TParent> Status
        {
            get
            {
                var status = this.pobj.GetProperty(nameof(this.Status));
                return (status != null) ? new Th128StatusWrapper<TParent>(status) : null;
            }
        }

        public void Set(HeaderWrapper<TParent> header)
            => this.pobj.Invoke(nameof(Set), new object[] { header.Target }, CultureInfo.InvariantCulture);
        public void Set(Th13ClearDataWrapper<TParent, TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac, TStProg> data)
            => this.pobj.Invoke(nameof(Set), new object[] { data.Target }, CultureInfo.InvariantCulture);
        public void Set(Th128StatusWrapper<TParent> status)
            => this.pobj.Invoke(nameof(Set), new object[] { status.Target }, CultureInfo.InvariantCulture);
    }
}
