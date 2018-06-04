using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    // NOTE: Setting the accessibility as public causes CS0053.
    internal sealed class Th075StatusWrapper
    {
        private static Type ParentType = typeof(Th075Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+Status";

        private readonly PrivateObject pobj = null;

        public Th075StatusWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { });

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public string LastName
            => this.pobj.GetProperty(nameof(LastName)) as string;
        public IReadOnlyDictionary<Th075Converter.Chara, Dictionary<Th075Converter.Chara, int>> ArcadeScores
            => this.pobj.GetProperty(nameof(ArcadeScores))
                as Dictionary<Th075Converter.Chara, Dictionary<Th075Converter.Chara, int>>;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
