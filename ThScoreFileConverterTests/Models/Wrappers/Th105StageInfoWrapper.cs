using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th105StageInfoWrapper<TParent, TStage, TChara>
        where TParent : ThConverter
        where TStage : struct, Enum
        where TChara : struct, Enum
    {
        private static readonly Type ParentType = typeof(TParent);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+StageInfo";

        private readonly PrivateObject pobj = null;

        public Th105StageInfoWrapper(TStage stage, TChara enemy, IEnumerable<int> cardIds)
            => this.pobj = new PrivateObject(
                AssemblyNameToTest, TypeNameToTest, new object[] { stage, enemy, cardIds });

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public TStage? Stage
            => this.pobj.GetProperty(nameof(Stage)) as TStage?;
        public TChara? Enemy
            => this.pobj.GetProperty(nameof(Enemy)) as TChara?;
        public IReadOnlyCollection<int> CardIds
            => this.pobj.GetProperty(nameof(CardIds)) as List<int>;
    }
}
