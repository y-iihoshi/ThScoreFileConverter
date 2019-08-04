using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    public sealed class Th125DetailWrapper
    {
        private static readonly Type ParentType = typeof(Th125Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+Detail";

        private readonly PrivateObject pobj = null;

        public Th125DetailWrapper(bool outputs, string format, string value)
            => this.pobj = new PrivateObject(
                AssemblyNameToTest, TypeNameToTest, new object[] { outputs, format, value });

        public object Target
            => this.pobj.Target;
        public bool? Outputs
            => this.pobj.GetProperty(nameof(this.Outputs)) as bool?;
        public string Format
            => this.pobj.GetProperty(nameof(this.Format)) as string;
        public string Value
            => this.pobj.GetProperty(nameof(this.Value)) as string;
    }
}
