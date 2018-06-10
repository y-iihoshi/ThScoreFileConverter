using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th075HighScoreTests
    {
        [TestMethod()]
        public void Th075HighScoreTest() => TestUtils.Wrap(() =>
        {
            var highScore = new Th075HighScoreWrapper();

            Assert.IsNull(highScore.Name);
            Assert.AreEqual(default, highScore.Month.Value);
            Assert.AreEqual(default, highScore.Day.Value);
            Assert.AreEqual(default, highScore.Score.Value);
        });

        internal static void ReadFromTestHelper(Th075HighScoreWrapper highScore, byte[] array)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    highScore.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }

        [TestMethod()]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var highScore = new Th075HighScoreWrapper();
            var name = new byte[] { 15, 37, 26, 50, 30, 43, 53, 103 };
            byte month = 6;
            byte day = 15;
            var unknown = new byte[2];
            var score = 1234567;
            ReadFromTestHelper(
                highScore,
                TestUtils.MakeByteArray(name, month, day, unknown, score));

            Assert.AreEqual("Player1 ", highScore.Name);
            Assert.AreEqual(month, highScore.Month);
            Assert.AreEqual(day, highScore.Day);
            Assert.AreEqual(score, highScore.Score);
        });

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var highScore = new Th075HighScoreWrapper();
            highScore.ReadFrom(null);
            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedName() => TestUtils.Wrap(() =>
        {
            var highScore = new Th075HighScoreWrapper();
            var name = new byte[] { 15, 37, 26, 50, 30, 43, 53 };
            byte month = 6;
            byte day = 15;
            var unknown = new byte[2];
            var score = 1234567;
            ReadFromTestHelper(
                highScore,
                TestUtils.MakeByteArray(name, month, day, unknown, score));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod()]
        public void ReadFromTestExceededName() => TestUtils.Wrap(() =>
        {
            var highScore = new Th075HighScoreWrapper();
            var name = new byte[] { 15, 37, 26, 50, 30, 43, 53, 103, 1 };
            byte month = 6;
            byte day = 15;
            var unknown = new byte[2];
            var score = 1234567;
            ReadFromTestHelper(
                highScore,
                TestUtils.MakeByteArray(name, month, day, unknown, score));

            Assert.AreEqual("Player1 ", highScore.Name);
            Assert.AreNotEqual(month, highScore.Month);
            Assert.AreNotEqual(day, highScore.Day);
            Assert.AreNotEqual(score, highScore.Score);
        });

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestZeroMonth() => TestUtils.Wrap(() =>
        {
            var highScore = new Th075HighScoreWrapper();
            var name = new byte[] { 15, 37, 26, 50, 30, 43, 53, 103 };
            byte month = 0;
            byte day = 15;
            var unknown = new byte[2];
            var score = 1234567;
            ReadFromTestHelper(
                highScore,
                TestUtils.MakeByteArray(name, month, day, unknown, score));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod()]
        public void ReadFromTestJanuary() => TestUtils.Wrap(() =>
        {
            var highScore = new Th075HighScoreWrapper();
            var name = new byte[] { 15, 37, 26, 50, 30, 43, 53, 103 };
            byte month = 1;
            byte day = 15;
            var unknown = new byte[2];
            var score = 1234567;
            ReadFromTestHelper(
                highScore,
                TestUtils.MakeByteArray(name, month, day, unknown, score));

            Assert.AreEqual("Player1 ", highScore.Name);
            Assert.AreEqual(month, highScore.Month);
            Assert.AreEqual(day, highScore.Day);
            Assert.AreEqual(score, highScore.Score);
        });

        [TestMethod()]
        public void ReadFromTestDecember() => TestUtils.Wrap(() =>
        {
            var highScore = new Th075HighScoreWrapper();
            var name = new byte[] { 15, 37, 26, 50, 30, 43, 53, 103 };
            byte month = 12;
            byte day = 15;
            var unknown = new byte[2];
            var score = 1234567;
            ReadFromTestHelper(
                highScore,
                TestUtils.MakeByteArray(name, month, day, unknown, score));

            Assert.AreEqual("Player1 ", highScore.Name);
            Assert.AreEqual(month, highScore.Month);
            Assert.AreEqual(day, highScore.Day);
            Assert.AreEqual(score, highScore.Score);
        });

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestInvalidMonth() => TestUtils.Wrap(() =>
        {
            var highScore = new Th075HighScoreWrapper();
            var name = new byte[] { 15, 37, 26, 50, 30, 43, 53, 103 };
            byte month = 13;
            byte day = 15;
            var unknown = new byte[2];
            var score = 1234567;
            ReadFromTestHelper(
                highScore,
                TestUtils.MakeByteArray(name, month, day, unknown, score));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestZeroDay() => TestUtils.Wrap(() =>
        {
            var highScore = new Th075HighScoreWrapper();
            var name = new byte[] { 15, 37, 26, 50, 30, 43, 53, 103 };
            byte month = 6;
            byte day = 0;
            var unknown = new byte[2];
            var score = 1234567;
            ReadFromTestHelper(
                highScore,
                TestUtils.MakeByteArray(name, month, day, unknown, score));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod()]
        public void ReadFromTestFirstDay() => TestUtils.Wrap(() =>
        {
            var highScore = new Th075HighScoreWrapper();
            var name = new byte[] { 15, 37, 26, 50, 30, 43, 53, 103 };
            byte month = 6;
            byte day = 1;
            var unknown = new byte[2];
            var score = 1234567;
            ReadFromTestHelper(
                highScore,
                TestUtils.MakeByteArray(name, month, day, unknown, score));

            Assert.AreEqual("Player1 ", highScore.Name);
            Assert.AreEqual(month, highScore.Month);
            Assert.AreEqual(day, highScore.Day);
            Assert.AreEqual(score, highScore.Score);
        });

        [TestMethod()]
        public void ReadFromTestLastDay() => TestUtils.Wrap(() =>
        {
            // Yes, I know June 31 is invalid.
            var highScore = new Th075HighScoreWrapper();
            var name = new byte[] { 15, 37, 26, 50, 30, 43, 53, 103 };
            byte month = 6;
            byte day = 31;
            var unknown = new byte[2];
            var score = 1234567;
            ReadFromTestHelper(
                highScore,
                TestUtils.MakeByteArray(name, month, day, unknown, score));

            Assert.AreEqual("Player1 ", highScore.Name);
            Assert.AreEqual(month, highScore.Month);
            Assert.AreEqual(day, highScore.Day);
            Assert.AreEqual(score, highScore.Score);
        });

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestInvalidDay() => TestUtils.Wrap(() =>
        {
            var highScore = new Th075HighScoreWrapper();
            var name = new byte[] { 15, 37, 26, 50, 30, 43, 53, 103 };
            byte month = 6;
            byte day = 32;
            var unknown = new byte[2];
            var score = 1234567;
            ReadFromTestHelper(
                highScore,
                TestUtils.MakeByteArray(name, month, day, unknown, score));

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
