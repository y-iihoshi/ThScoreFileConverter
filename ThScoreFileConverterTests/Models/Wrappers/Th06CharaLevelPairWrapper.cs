using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0703.
    internal sealed class Th06CharaLevelPairWrapper<TParent, TChara, TLevel>
        where TParent : ThConverter
        where TChara : struct, Enum
        where TLevel : struct, Enum
    {
        private static readonly Type ParentType = typeof(TParent);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+CharaLevelPair";

        private readonly PrivateObject pobj = null;

        public Th06CharaLevelPairWrapper(TChara chara, TLevel level)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chara, level });

        public object Target
            => this.pobj.Target;
        public TChara? Chara
            => this.pobj.GetProperty(nameof(Chara)) as TChara?;
        public TLevel? Level
            => this.pobj.GetProperty(nameof(Level)) as TLevel?;
    }
}
