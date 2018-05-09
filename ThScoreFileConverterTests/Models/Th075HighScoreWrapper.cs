using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.IO;

namespace ThScoreFileConverter.Models.Tests
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th075HighScoreWrapper
    {
        private static Type ParentType = typeof(Th075Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+HighScore";

        private readonly PrivateObject pobj = null;

        public Th075HighScoreWrapper()
            => this.pobj = new PrivateObject(
                AssemblyNameToTest,
                TypeNameToTest,
                new object[] { });
        public Th075HighScoreWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        // NOTE: Enabling the following causes CA1811.
        // public object Target => this.pobj.Target;

        public string Name
            => this.pobj.GetProperty(nameof(Name)) as string;
        public byte? Month
            => this.pobj.GetProperty(nameof(Month)) as byte?;
        public byte? Day
            => this.pobj.GetProperty(nameof(Day)) as byte?;
        public int? Score
            => this.pobj.GetProperty(nameof(Score)) as int?;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(
                nameof(ReadFrom),
                new object[] { reader },
                CultureInfo.InvariantCulture);
    }
}
