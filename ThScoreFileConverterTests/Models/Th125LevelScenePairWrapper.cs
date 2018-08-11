using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th125LevelScenePairWrapper
    {
        private static Type ParentType = typeof(Th125Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+LevelScenePair";

        private readonly PrivateObject pobj = null;

        public Th125LevelScenePairWrapper(Th125Converter.Level level, int scene)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { level, scene });
        public Th125LevelScenePairWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public Th125Converter.Level? Level
            => this.pobj.GetProperty(nameof(this.Level)) as Th125Converter.Level?;
        public int? Scene
            => this.pobj.GetProperty(nameof(this.Scene)) as int?;
    }
}
