using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th075SpellCardInfoWrapper
    {
        private static readonly Type ParentType = typeof(Th075Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+SpellCardInfo";

        private readonly PrivateObject pobj = null;

        public Th075SpellCardInfoWrapper(string name, Th075Converter.Chara enemy, Th075Converter.Level level)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { name, enemy, level });

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public string Name
            => this.pobj.GetProperty(nameof(Name)) as string;
        public Th075Converter.Chara? Enemy
            => this.pobj.GetProperty(nameof(Enemy)) as Th075Converter.Chara?;
        public Th075Converter.Level? Level
            => this.pobj.GetProperty(nameof(Level)) as Th075Converter.Level?;
    }
}
