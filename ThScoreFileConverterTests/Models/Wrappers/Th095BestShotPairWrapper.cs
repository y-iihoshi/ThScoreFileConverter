using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th095BestShotPairWrapper
    {
        private static readonly Type ParentType = typeof(Th095Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+BestShotPair";

        private readonly PrivateObject pobj = null;

        public Th095BestShotPairWrapper(string path, Th095BestShotHeaderWrapper header)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { path, header.Target });

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public string Path
            => this.pobj.GetProperty(nameof(this.Path)) as string;
        public Th095BestShotHeaderWrapper Header
            => new Th095BestShotHeaderWrapper(this.pobj.GetProperty(nameof(this.Header)));

        public void Deconstruct(out string path, out Th095BestShotHeaderWrapper header)
        {
            var args = new object[] { null, null };
            this.pobj.Invoke(nameof(Deconstruct), args, CultureInfo.InvariantCulture);
            path = args[0] as string;
            header = new Th095BestShotHeaderWrapper(args[1]);
        }
    }
}
