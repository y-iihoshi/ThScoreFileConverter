using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th13;

namespace ThScoreFileConverterTests.Models.Th13
{
    [TestClass]
    public class PracticeTests
    {
        internal static Mock<IPractice> MockPractice()
        {
            var mock = new Mock<IPractice>();
            _ = mock.SetupGet(m => m.Score).Returns(123456u);
            _ = mock.SetupGet(m => m.ClearFlag).Returns(7);
            _ = mock.SetupGet(m => m.EnableFlag).Returns(8);
            return mock;
        }

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
            var mock = new Mock<IPractice>();
            var practice = new Practice();

            Validate(mock.Object, practice);
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var mock = MockPractice();
            var practice = TestUtils.Create<Practice>(MakeByteArray(mock.Object));

            Validate(mock.Object, practice);
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
