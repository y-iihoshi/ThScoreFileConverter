using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th125;
using HeaderBase = ThScoreFileConverter.Models.Th095.HeaderBase;

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
        public HeaderBase Header
            => this.pobj.GetProperty(nameof(this.Header)) as HeaderBase;
        public IReadOnlyList<IScore> Scores
            => this.pobj.GetProperty(nameof(this.Scores)) as IReadOnlyList<IScore>;
        public IStatus Status
            => this.pobj.GetProperty(nameof(this.Status)) as IStatus;

        public void Set(HeaderBase header)
            => this.pobj.Invoke(nameof(Set), new object[] { header }, CultureInfo.InvariantCulture);
        public void Set(IScore score)
            => this.pobj.Invoke(nameof(Set), new object[] { score }, CultureInfo.InvariantCulture);
        public void Set(IStatus status)
            => this.pobj.Invoke(nameof(Set), new object[] { status }, CultureInfo.InvariantCulture);
    }
}
