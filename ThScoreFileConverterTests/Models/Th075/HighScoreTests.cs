using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th075;
using ThScoreFileConverterTests.Models.Th075.Stubs;
using ThScoreFileConverterTests.UnitTesting;

namespace ThScoreFileConverterTests.Models.Th075
{
    [TestClass]
    public class HighScoreTests
    {
        internal static HighScoreStub ValidStub { get; } = new HighScoreStub()
        {
            EncodedName = new byte[] { 15, 37, 26, 50, 30, 43, 53, 103 },
            Name = "Player1 ",
            Month = 6,
            Day = 15,
            Score = 1234567,
        };

        internal static byte[] MakeByteArray(in HighScoreStub stub)
        {
            return TestUtils.MakeByteArray(
                stub.EncodedName.ToArray(),
                stub.Month,
                stub.Day,
                new byte[2],
                stub.Score);
        }

        internal static void Validate(IHighScore expected, IHighScore actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Month, actual.Month);
            Assert.AreEqual(expected.Day, actual.Day);
            Assert.AreEqual(expected.Score, actual.Score);
        }

        [TestMethod]
        public void HighScoreTest()
        {
            var highScore = new HighScore();

            Assert.AreEqual(string.Empty, highScore.Name);
            Assert.AreEqual(default, highScore.Month);
            Assert.AreEqual(default, highScore.Day);
            Assert.AreEqual(default, highScore.Score);
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var highScore = TestUtils.Create<HighScore>(MakeByteArray(ValidStub));

            Validate(ValidStub, highScore);
        }

        [TestMethod]
        public void ReadFromTestShortenedName()
        {
            var stub = new HighScoreStub(ValidStub);
            stub.EncodedName = stub.EncodedName.SkipLast(1).ToArray();

            _ = Assert.ThrowsException<InvalidDataException>(
                () => _ = TestUtils.Create<HighScore>(MakeByteArray(stub)));
        }

        [TestMethod]
        public void ReadFromTestExceededName()
        {
            var stub = new HighScoreStub(ValidStub);
            stub.EncodedName = stub.EncodedName.Concat(new byte[] { default }).ToArray();

            _ = Assert.ThrowsException<InvalidDataException>(
                () => _ = TestUtils.Create<HighScore>(MakeByteArray(stub)));
        }

        [DataTestMethod]
        [DataRow(0, 0)]
        [DataRow(1, 1)]
        [DataRow(1, 31)]
        [DataRow(2, 1)]
        [DataRow(2, 28)]
        [DataRow(2, 29)]
        [DataRow(3, 1)]
        [DataRow(3, 31)]
        [DataRow(4, 1)]
        [DataRow(4, 30)]
        [DataRow(5, 1)]
        [DataRow(5, 31)]
        [DataRow(6, 1)]
        [DataRow(6, 30)]
        [DataRow(7, 1)]
        [DataRow(7, 31)]
        [DataRow(8, 1)]
        [DataRow(8, 31)]
        [DataRow(9, 1)]
        [DataRow(9, 30)]
        [DataRow(10, 1)]
        [DataRow(10, 31)]
        [DataRow(11, 1)]
        [DataRow(11, 30)]
        [DataRow(12, 1)]
        [DataRow(12, 31)]
        public void ReadFromTestValidMonthDay(int month, int day)
        {
            var stub = new HighScoreStub(ValidStub)
            {
                Month = (byte)month,
                Day = (byte)day,
            };

            var highScore = TestUtils.Create<HighScore>(MakeByteArray(stub));

            Validate(stub, highScore);
        }

        [DataTestMethod]
        [DataRow(0, -1)]
        [DataRow(0, 1)]
        [DataRow(1, 0)]
        [DataRow(1, 32)]
        [DataRow(2, 0)]
        [DataRow(2, 30)]
        [DataRow(3, 0)]
        [DataRow(3, 32)]
        [DataRow(4, 0)]
        [DataRow(4, 31)]
        [DataRow(5, 0)]
        [DataRow(5, 32)]
        [DataRow(6, 0)]
        [DataRow(6, 31)]
        [DataRow(7, 0)]
        [DataRow(7, 32)]
        [DataRow(8, 0)]
        [DataRow(8, 32)]
        [DataRow(9, 0)]
        [DataRow(9, 31)]
        [DataRow(10, 0)]
        [DataRow(10, 32)]
        [DataRow(11, 0)]
        [DataRow(11, 31)]
        [DataRow(12, 0)]
        [DataRow(12, 32)]
        public void ReadFromTestInvalidMonthDay(int month, int day)
        {
            var properties = new HighScoreStub(ValidStub)
            {
                Month = (byte)month,
                Day = (byte)day,
            };

            _ = Assert.ThrowsException<InvalidDataException>(
                () => _ = TestUtils.Create<HighScore>(MakeByteArray(properties)));
        }
    }
}
