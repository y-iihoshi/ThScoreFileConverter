using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th06CharaLevelPairWrapper
    {
        private static Type ParentType = typeof(Th06Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+CharaLevelPair";

        private readonly PrivateObject pobj = null;

        public Th06CharaLevelPairWrapper(Th06Converter.Chara chara, ThConverter.Level level)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chara, level });

        public object Target
            => this.pobj.Target;
        public Th06Converter.Chara? Chara
            => this.pobj.GetProperty(nameof(Chara)) as Th06Converter.Chara?;
        public ThConverter.Level? Level
            => this.pobj.GetProperty(nameof(Level)) as ThConverter.Level?;
    }
}
