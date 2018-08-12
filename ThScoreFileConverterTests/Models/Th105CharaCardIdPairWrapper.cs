using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    // NOTE: Setting the accessibility as public causes CS0703.
    internal sealed class Th105CharaCardIdPairWrapper<TParent, TChara>
        where TParent : ThConverter
        where TChara : struct, Enum
    {
        private static Type ParentType = typeof(TParent);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+CharaCardIdPair";

        private readonly PrivateObject pobj = null;

        public Th105CharaCardIdPairWrapper(TChara chara, int cardId)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chara, cardId });

        public object Target
            => this.pobj.Target;
        public TChara? Chara
            => this.pobj.GetProperty(nameof(this.Chara)) as TChara?;
        public int? CardId
            => this.pobj.GetProperty(nameof(this.CardId)) as int?;
    }
}
