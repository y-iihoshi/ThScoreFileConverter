using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th08StageLevelPairWrapper
    {
        private static Type ParentType = typeof(Th08Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+StageLevelPair";

        private readonly PrivateObject pobj = null;

        public Th08StageLevelPairWrapper(Th08Converter.Stage stage, ThConverter.Level level)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { stage, level });

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public Th08Converter.Stage? Stage
            => this.pobj.GetProperty(nameof(Stage)) as Th08Converter.Stage?;
        public ThConverter.Level? Level
            => this.pobj.GetProperty(nameof(Level)) as ThConverter.Level?;
    }
}
