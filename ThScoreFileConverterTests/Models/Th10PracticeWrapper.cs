using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;

namespace ThScoreFileConverter.Models.Tests
{
    public sealed class Th10PracticeWrapper<TParent>
    {
        private static Type ParentType = typeof(TParent);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+Practice";

        private readonly PrivateObject pobj = null;

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Th10PracticeWrapper<TParent> Create(byte[] array)
        {
            var practice = new Th10PracticeWrapper<TParent>();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    practice.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return practice;
        }

        public Th10PracticeWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        // NOTE: Enabling the following causes CA1811.
        // public object Target => this.pobj.Target;

        public uint? Score
            => this.pobj.GetProperty(nameof(Score)) as uint?;
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flag")]
        public uint? StageFlag
            => this.pobj.GetProperty(nameof(StageFlag)) as uint?;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
