using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    // NOTE: Setting the accessibility as public causes CS0703.
    internal sealed class Th10LevelStagePairWrapper<TParent, TLevel, TStage>
        where TParent : ThConverter
        where TLevel : struct, Enum
        where TStage : struct, Enum
    {
        private static Type ParentType = typeof(TParent);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+LevelStagePair";

        private readonly PrivateObject pobj = null;

        public Th10LevelStagePairWrapper(TLevel level, TStage stage)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { level, stage });

        public object Target
            => this.pobj.Target;
        public TLevel? Level
            => this.pobj.GetProperty(nameof(this.Level)) as TLevel?;
        public TStage? Stage
            => this.pobj.GetProperty(nameof(this.Stage)) as TStage?;
    }

    // NOTE: Setting the accessibility as public causes CS0703.
    internal sealed class Th10LevelStagePairWrapper<TParent, TLevel, TStage, TLevelArg, TStageArg>
        where TParent : ThConverter
        where TLevel : struct, Enum
        where TStage : struct, Enum
        where TLevelArg : struct, Enum
        where TStageArg : struct, Enum
    {
        private static Type ParentType = typeof(TParent);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+LevelStagePair";

        private readonly PrivateObject pobj = null;

        public Th10LevelStagePairWrapper(TLevel level, TStage stage)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { level, stage });
        public Th10LevelStagePairWrapper(TLevelArg level, TStageArg stage)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { level, stage });

        public object Target
            => this.pobj.Target;
        public TLevel? Level
            => this.pobj.GetProperty(nameof(this.Level)) as TLevel?;
        public TStage? Stage
            => this.pobj.GetProperty(nameof(this.Stage)) as TStage?;
    }
}
