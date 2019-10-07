using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.Models.Th10.Stubs;

namespace ThScoreFileConverterTests.Models.Th10
{
    [TestClass]
    public class PracticeTests
    {
        internal static PracticeStub ValidStub { get; } = new PracticeStub()
        {
            Score = 123456u,
            StageFlag = 789u
        };

        internal static byte[] MakeByteArray(IPractice practice)
            => TestUtils.MakeByteArray(practice.Score, practice.StageFlag);

        internal static void Validate(IPractice expected, IPractice actual)
        {
            Assert.AreEqual(expected.Score, actual.Score);
            Assert.AreEqual(expected.StageFlag, actual.StageFlag);
        }

        [TestMethod]
        public void PracticeTest() => TestUtils.Wrap(() =>
        {
            var stub = new PracticeStub();
            var practice = new Practice();

            Validate(stub, practice);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var practice = TestUtils.Create<Practice>(MakeByteArray(stub));

            Validate(stub, practice);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var practice = new Practice();
            practice.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
