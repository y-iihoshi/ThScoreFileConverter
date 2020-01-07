using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverterTests.Models.Th13.Stubs;

namespace ThScoreFileConverterTests.Models.Th13
{
    [TestClass]
    public class PracticeTests
    {
        internal static PracticeStub ValidStub { get; } = new PracticeStub()
        {
            Score = 123456u,
            ClearFlag = 7,
            EnableFlag = 8,
        };

        internal static byte[] MakeByteArray(IPractice practice)
            => TestUtils.MakeByteArray(practice.Score, practice.ClearFlag, practice.EnableFlag, (ushort)0);

        internal static void Validate(IPractice expected, IPractice actual)
        {
            Assert.AreEqual(expected.Score, actual.Score);
            Assert.AreEqual(expected.ClearFlag, actual.ClearFlag);
            Assert.AreEqual(expected.EnableFlag, actual.EnableFlag);
        }

        [TestMethod]
        public void PracticeTest()
        {
            var stub = new PracticeStub();
            var practice = new Practice();

            Validate(stub, practice);
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var practice = TestUtils.Create<Practice>(MakeByteArray(ValidStub));

            Validate(ValidStub, practice);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull()
        {
            var practice = new Practice();

            practice.ReadFrom(null!);

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
