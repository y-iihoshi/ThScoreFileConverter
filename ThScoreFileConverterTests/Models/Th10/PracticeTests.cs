using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.UnitTesting;

namespace ThScoreFileConverterTests.Models.Th10
{
    [TestClass]
    public class PracticeTests
    {
        internal static Mock<IPractice> MockPractice()
        {
            var mock = new Mock<IPractice>();
            _ = mock.SetupGet(m => m.Score).Returns(123456u);
            _ = mock.SetupGet(m => m.StageFlag).Returns(789u);
            return mock;
        }

        internal static byte[] MakeByteArray(IPractice practice)
            => TestUtils.MakeByteArray(practice.Score, practice.StageFlag);

        internal static void Validate(IPractice expected, IPractice actual)
        {
            Assert.AreEqual(expected.Score, actual.Score);
            Assert.AreEqual(expected.StageFlag, actual.StageFlag);
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
    }
}
