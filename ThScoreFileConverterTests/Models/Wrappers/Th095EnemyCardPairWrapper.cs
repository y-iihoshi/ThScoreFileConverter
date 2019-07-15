using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0703.
    internal sealed class Th095EnemyCardPairWrapper<TParent, TEnemy>
        where TParent : ThConverter
        where TEnemy : struct, Enum
    {
        private static readonly Type ParentType = typeof(TParent);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+EnemyCardPair";

        private readonly PrivateObject pobj = null;

        public Th095EnemyCardPairWrapper(TEnemy enemy, string card)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { enemy, card });
        public Th095EnemyCardPairWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public TEnemy? Enemy
            => this.pobj.GetProperty(nameof(this.Enemy)) as TEnemy?;
        public string Card
            => this.pobj.GetProperty(nameof(this.Card)) as string;
    }
}
