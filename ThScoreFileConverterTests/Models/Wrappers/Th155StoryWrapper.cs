using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0053.
    internal sealed class Th155StoryWrapper
    {
        private static Type ConverterType = typeof(Th155Converter);
        private static string AssemblyNameToTest = ConverterType.Assembly.GetName().Name;
        private static string TypeNameToTest = ConverterType.FullName + "+AllScoreData+Story";

        private readonly PrivateObject pobj = null;

        public Th155StoryWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);
        public Th155StoryWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;

        public int? Stage
        {
            get { return this.pobj.GetField("stage") as int?; }
            set { this.pobj.SetField("stage", value.Value); }
        }

        public Th155Converter.LevelFlag? Ed
        {
            get { return this.pobj.GetField("ed") as Th155Converter.LevelFlag?; }
            set { this.pobj.SetField("ed", value.Value); }
        }

        public bool? Available
        {
            get { return this.pobj.GetField("available") as bool?; }
            set { this.pobj.SetField("available", value.Value); }
        }

        public int? OverDrive
        {
            get { return this.pobj.GetField("overDrive") as int?; }
            set { this.pobj.SetField("overDrive", value.Value); }
        }

        public int? StageOverDrive
        {
            get { return this.pobj.GetField("stageOverDrive") as int?; }
            set { this.pobj.SetField("stageOverDrive", value.Value); }
        }
    }
}
