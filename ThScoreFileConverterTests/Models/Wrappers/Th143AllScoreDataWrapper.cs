using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th095.Wrappers;

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

        public HeaderWrapper<Th143Converter> Header
        {
            get
            {
                var header = this.pobj.GetProperty(nameof(this.Header));
                return (header != null) ? new HeaderWrapper<Th143Converter>(header) : null;
            }
        }

        // NOTE: Th143Converter.Score is a private class.
        // public IReadOnlyList<IScore> Scores
        //     => this.pobj.GetProperty(nameof(this.Scores)) as IReadOnlyList<IScore>;
        public object Scores
            => this.pobj.GetProperty(nameof(this.Scores));
        public int? ScoresCount
            => this.Scores.GetType().GetProperty("Count").GetValue(this.Scores) as int?;
        public Th143ScoreWrapper ScoresItem(int index)
            => new Th143ScoreWrapper(
                this.Scores.GetType().GetProperty("Item").GetValue(this.Scores, new object[] { index }));

        // NOTE: Th143Converter.ItemStatus is a private class.
        // public IReadOnlyDictionary<ItemWithTotal, ItemStatus> ItemStatuses
        //     => this.pobj.GetProperty(nameof(this.ItemStatuses)) as Dictionary<ItemWithTotal, ItemStatus>;
        public object ItemStatuses
            => this.pobj.GetProperty(nameof(this.ItemStatuses));
        public int? ItemStatusesCount
            => this.ItemStatuses.GetType().GetProperty("Count").GetValue(this.ItemStatuses) as int?;
        public Th143ItemStatusWrapper ItemStatusesItem(Th143Converter.ItemWithTotal item)
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

        public void Set(HeaderWrapper<Th143Converter> header)
            => this.pobj.Invoke(nameof(Set), new object[] { header.Target }, CultureInfo.InvariantCulture);
        public void Set(Th143ScoreWrapper score)
            => this.pobj.Invoke(nameof(Set), new object[] { score.Target }, CultureInfo.InvariantCulture);
        public void Set(Th143ItemStatusWrapper item)
            => this.pobj.Invoke(nameof(Set), new object[] { item.Target }, CultureInfo.InvariantCulture);
        public void Set(Th143StatusWrapper status)
            => this.pobj.Invoke(nameof(Set), new object[] { status.Target }, CultureInfo.InvariantCulture);
    }
}
