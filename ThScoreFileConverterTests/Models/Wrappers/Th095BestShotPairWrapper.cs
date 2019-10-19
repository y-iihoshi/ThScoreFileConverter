using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th095BestShotPairWrapper
    {
        private static readonly Type ParentType = typeof(Th095Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+BestShotPair";

        private readonly PrivateObject pobj = null;

        public Th095BestShotPairWrapper(string path, IBestShotHeader header)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { path, header });

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public string Path
            => this.pobj.GetProperty(nameof(this.Path)) as string;
        public IBestShotHeader Header
            => this.pobj.GetProperty(nameof(this.Header)) as IBestShotHeader;

        public void Deconstruct(out string path, out IBestShotHeader header)
        {
            var args = new object[] { null, null };
            this.pobj.Invoke(nameof(Deconstruct), args, CultureInfo.InvariantCulture);
            path = args[0] as string;
            header = args[1] as IBestShotHeader;
        }
    }
}
