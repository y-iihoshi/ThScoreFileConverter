using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th095.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th125AllScoreDataWrapper
    {
        private static readonly Type ParentType = typeof(Th125Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+AllScoreData";

        private readonly PrivateObject pobj = null;

        public Th125AllScoreDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;

        public HeaderWrapper<Th125Converter> Header
        {
            get
            {
                var header = this.pobj.GetProperty(nameof(this.Header));
                return (header != null) ? new HeaderWrapper<Th125Converter>(header) : null;
            }
        }

        // NOTE: Th125Converter.Score is a private class.
        // public IReadOnlyList<IScore> Scores
        //     => this.pobj.GetProperty(nameof(this.Scores)) as IReadOnlyList<IScore>;
        public object Scores
            => this.pobj.GetProperty(nameof(this.Scores));
        public int? ScoresCount
            => this.Scores.GetType().GetProperty("Count").GetValue(this.Scores) as int?;
        public Th125ScoreWrapper ScoresItem(int index)
            => new Th125ScoreWrapper(
                this.Scores.GetType().GetProperty("Item").GetValue(this.Scores, new object[] { index }));

        public Th125StatusWrapper Status
        {
            get
            {
                var status = this.pobj.GetProperty(nameof(this.Status));
                return (status != null) ? new Th125StatusWrapper(status) : null;
            }
        }

        public void Set(HeaderWrapper<Th125Converter> header)
            => this.pobj.Invoke(nameof(Set), new object[] { header.Target }, CultureInfo.InvariantCulture);
        public void Set(Th125ScoreWrapper data)
            => this.pobj.Invoke(nameof(Set), new object[] { data.Target }, CultureInfo.InvariantCulture);
        public void Set(Th125StatusWrapper status)
            => this.pobj.Invoke(nameof(Set), new object[] { status.Target }, CultureInfo.InvariantCulture);
    }
}
