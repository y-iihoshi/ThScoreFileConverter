using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th095LevelScenePairWrapper<TParent, TLevel>
        where TParent : ThConverter
        where TLevel : struct, Enum
    {
        private static Type ParentType = typeof(TParent);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+LevelScenePair";

        private readonly PrivateObject pobj = null;

        public Th095LevelScenePairWrapper(TLevel level, int scene)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { level, scene });
        public Th095LevelScenePairWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public TLevel? Level
            => this.pobj.GetProperty(nameof(this.Level)) as TLevel?;
        public int? Scene
            => this.pobj.GetProperty(nameof(this.Scene)) as int?;
    }
}
