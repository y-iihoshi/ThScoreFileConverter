using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th143DayScenePairWrapper
    {
        private static readonly Type ParentType = typeof(Th143Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+DayScenePair";

        private readonly PrivateObject pobj = null;

        public Th143DayScenePairWrapper(Th143Converter.Day day, int scene)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { day, scene });

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public Th143Converter.Day? Day
            => this.pobj.GetProperty(nameof(this.Day)) as Th143Converter.Day?;
        public int? Scene
            => this.pobj.GetProperty(nameof(this.Scene)) as int?;
    }
}
