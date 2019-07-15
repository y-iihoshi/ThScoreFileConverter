using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th128AllScoreDataWrapper
    {
        private static readonly Type ParentType = typeof(Th128Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+AllScoreData";

        private readonly PrivateObject pobj = null;

        public Th128AllScoreDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;

        public Th095HeaderWrapper<Th128Converter> Header
        {
            get
            {
                var header = this.pobj.GetProperty(nameof(Header));
                return (header != null) ? new Th095HeaderWrapper<Th128Converter>(header) : null;
            }
        }

        // NOTE: Th128Converter.ClearData is a private class.
        // public IReadOnlyDictionary<RouteWithTotal, ClearData> ClearData
        //     => this.pobj.GetProperty(nameof(ClearData)) as Dictionary<RouteWithTotal, ClearData>;
        public object ClearData
            => this.pobj.GetProperty(nameof(ClearData));
        public int? ClearDataCount
            => this.ClearData.GetType().GetProperty("Count").GetValue(this.ClearData) as int?;
        public Th128ClearDataWrapper ClearDataItem(Th128Converter.RouteWithTotal route)
            => new Th128ClearDataWrapper(
                this.ClearData.GetType().GetProperty("Item").GetValue(this.ClearData, new object[] { route }));

        public Th128CardDataWrapper CardData
        {
            get
            {
                var status = this.pobj.GetProperty(nameof(CardData));
                return (status != null) ? new Th128CardDataWrapper(status) : null;
            }
        }

        public Th128StatusWrapper<Th128Converter> Status
        {
            get
            {
                var status = this.pobj.GetProperty(nameof(Status));
                return (status != null) ? new Th128StatusWrapper<Th128Converter>(status) : null;
            }
        }

        public void Set(Th095HeaderWrapper<Th128Converter> header)
            => this.pobj.Invoke(nameof(Set), new object[] { header.Target }, CultureInfo.InvariantCulture);
        public void Set(Th128ClearDataWrapper data)
            => this.pobj.Invoke(nameof(Set), new object[] { data.Target }, CultureInfo.InvariantCulture);
        public void Set(Th128CardDataWrapper data)
            => this.pobj.Invoke(nameof(Set), new object[] { data.Target }, CultureInfo.InvariantCulture);
        public void Set(Th128StatusWrapper<Th128Converter> status)
            => this.pobj.Invoke(nameof(Set), new object[] { status.Target }, CultureInfo.InvariantCulture);
    }
}
