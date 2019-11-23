using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th143;
using HeaderBase = ThScoreFileConverter.Models.Th095.HeaderBase;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th143AllScoreDataWrapper
    {
        private static readonly Type ParentType = typeof(Th143Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+AllScoreData";

        private readonly PrivateObject pobj = null;

        public Th143AllScoreDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public HeaderBase Header
            => this.pobj.GetProperty(nameof(this.Header)) as HeaderBase;
        public IReadOnlyList<IScore> Scores
            => this.pobj.GetProperty(nameof(this.Scores)) as IReadOnlyList<IScore>;

        // NOTE: Th143Converter.ItemStatus is a private class.
        // public IReadOnlyDictionary<ItemWithTotal, ItemStatus> ItemStatuses
        //     => this.pobj.GetProperty(nameof(this.ItemStatuses)) as Dictionary<ItemWithTotal, ItemStatus>;
        public object ItemStatuses
            => this.pobj.GetProperty(nameof(this.ItemStatuses));
        public int? ItemStatusesCount
            => this.ItemStatuses.GetType().GetProperty("Count").GetValue(this.ItemStatuses) as int?;
        public Th143ItemStatusWrapper ItemStatusesItem(ItemWithTotal item)
            => new Th143ItemStatusWrapper(
                this.ItemStatuses.GetType().GetProperty("Item").GetValue(this.ItemStatuses, new object[] { item }));

        public Th143StatusWrapper Status
        {
            get
            {
                var status = this.pobj.GetProperty(nameof(this.Status));
                return (status != null) ? new Th143StatusWrapper(status) : null;
            }
        }

        public void Set(HeaderBase header)
            => this.pobj.Invoke(nameof(Set), new object[] { header }, CultureInfo.InvariantCulture);
        public void Set(IScore score)
            => this.pobj.Invoke(nameof(Set), new object[] { score }, CultureInfo.InvariantCulture);
        public void Set(Th143ItemStatusWrapper item)
            => this.pobj.Invoke(nameof(Set), new object[] { item.Target }, CultureInfo.InvariantCulture);
        public void Set(Th143StatusWrapper status)
            => this.pobj.Invoke(nameof(Set), new object[] { status.Target }, CultureInfo.InvariantCulture);
    }
}
