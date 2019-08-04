using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th095.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th165AllScoreDataWrapper
    {
        private static readonly Type ParentType = typeof(Th165Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+AllScoreData";

        private readonly PrivateObject pobj = null;

        public Th165AllScoreDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;

        public HeaderWrapper<Th165Converter> Header
        {
            get
            {
                var header = this.pobj.GetProperty(nameof(this.Header));
                return (header != null) ? new HeaderWrapper<Th165Converter>(header) : null;
            }
        }

        // NOTE: Th165Converter.Score is a private class.
        // public IReadOnlyList<Score> Scores
        //     => this.pobj.GetProperty(nameof(this.Scores)) as List<Score>;
        public object Scores
            => this.pobj.GetProperty(nameof(this.Scores));
        public int? ScoresCount
            => this.Scores.GetType().GetProperty("Count").GetValue(this.Scores) as int?;
        public Th165ScoreWrapper ScoresItem(int index)
            => new Th165ScoreWrapper(
                this.Scores.GetType().GetProperty("Item").GetValue(this.Scores, new object[] { index }));

        public Th165StatusWrapper Status
        {
            get
            {
                var status = this.pobj.GetProperty(nameof(this.Status));
                return (status != null) ? new Th165StatusWrapper(status) : null;
            }
        }

        public void Set(HeaderWrapper<Th165Converter> header)
            => this.pobj.Invoke(nameof(Set), new object[] { header.Target }, CultureInfo.InvariantCulture);
        public void Set(Th165ScoreWrapper data)
            => this.pobj.Invoke(nameof(Set), new object[] { data.Target }, CultureInfo.InvariantCulture);
        public void Set(Th165StatusWrapper status)
            => this.pobj.Invoke(nameof(Set), new object[] { status.Target }, CultureInfo.InvariantCulture);
    }
}
