using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0053.
    internal sealed class Th155StoryWrapper
    {
        private static readonly Type ConverterType = typeof(Th155Converter);
        private static readonly string AssemblyNameToTest = ConverterType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ConverterType.FullName + "+AllScoreData+Story";

        private readonly PrivateObject pobj = null;

        public Th155StoryWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);
        public Th155StoryWrapper(object original)
            => this.pobj = new PrivateObject(original);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;

        public int? Stage
        {
            get => this.pobj.GetField(nameof(this.Stage)) as int?;
            set => this.pobj.SetField(nameof(this.Stage), value.Value);
        }

        public Th155Converter.LevelFlag? Ed
        {
            get => this.pobj.GetField(nameof(this.Ed)) as Th155Converter.LevelFlag?;
            set => this.pobj.SetField(nameof(this.Ed), value.Value);
        }

        public bool? Available
        {
            get => this.pobj.GetField(nameof(this.Available)) as bool?;
            set => this.pobj.SetField(nameof(this.Available), value.Value);
        }

        public int? OverDrive
        {
            get => this.pobj.GetField(nameof(this.OverDrive)) as int?;
            set => this.pobj.SetField(nameof(this.OverDrive), value.Value);
        }

        public int? StageOverDrive
        {
            get => this.pobj.GetField(nameof(this.StageOverDrive)) as int?;
            set => this.pobj.SetField(nameof(this.StageOverDrive), value.Value);
        }
    }
}
