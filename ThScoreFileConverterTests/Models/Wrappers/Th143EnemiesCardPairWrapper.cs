using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th143EnemiesCardPairWrapper
    {
        private static Type ParentType = typeof(Th143Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+EnemiesCardPair";

        private readonly PrivateObject pobj = null;

        public Th143EnemiesCardPairWrapper(Th143Converter.Enemy enemy, string card)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { enemy, card });
        public Th143EnemiesCardPairWrapper(Th143Converter.Enemy enemy1, Th143Converter.Enemy enemy2, string card)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { enemy1, enemy2, card });

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public Th143Converter.Enemy? Enemy
            => this.pobj.GetProperty(nameof(this.Enemy)) as Th143Converter.Enemy?;
        public IReadOnlyCollection<Th143Converter.Enemy> Enemies
            => this.pobj.GetProperty(nameof(this.Enemies)) as Th143Converter.Enemy[];
        public string Card
            => this.pobj.GetProperty(nameof(this.Card)) as string;
    }
}
