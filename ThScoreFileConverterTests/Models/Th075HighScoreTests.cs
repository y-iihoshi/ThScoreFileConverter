using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th075HighScoreTests
    {
        internal struct Properties
        {
            public byte[] encodedName;
            public string decodedName;
            public byte month;
            public byte day;
            public int score;
        };

        internal static Properties ValidProperties => new Properties()
        {
            encodedName = new byte[] { 15, 37, 26, 50, 30, 43, 53, 103 },
            decodedName = "Player1 ",
            month = 6,
            day = 15,
            score = 1234567
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.encodedName,
                properties.month,
                properties.day,
                new byte[2],
                properties.score);

        internal static void Validate(in Th075HighScoreWrapper highScore, in Properties properties)
        {
            Assert.AreEqual(properties.decodedName, highScore.Name);
            Assert.AreEqual(properties.month, highScore.Month);
            Assert.AreEqual(properties.day, highScore.Day);
            Assert.AreEqual(properties.score, highScore.Score);
        }

        [TestMethod]
        public void Th075HighScoreTest() => TestUtils.Wrap(() =>
        {
            var highScore = new Th075HighScoreWrapper();

            Assert.IsNull(highScore.Name);
            Assert.AreEqual(default, highScore.Month.Value);
            Assert.AreEqual(default, highScore.Day.Value);
            Assert.AreEqual(default, highScore.Score.Value);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var highScore = Th075HighScoreWrapper.Create(MakeByteArray(ValidProperties));

            Validate(highScore, ValidProperties);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var highScore = new Th075HighScoreWrapper();
            highScore.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestShortenedName() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.encodedName = properties.encodedName.Take(properties.encodedName.Length - 1).ToArray();

            Th075HighScoreWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestExceededName() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.encodedName = properties.encodedName.Concat(new byte[] { default }).ToArray();

            var highScore = Th075HighScoreWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1)]
        [DataRow(12)]
        public void ReadFromTestValidMonth(int month) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.month = (byte)month;

            var highScore = Th075HighScoreWrapper.Create(MakeByteArray(properties));

            Validate(highScore, properties);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(13)]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestInvalidMonth(int month) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.month = (byte)month;

            Th075HighScoreWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1, 1)]
        [DataRow(1, 31)]
        [DataRow(2, 28)]
        [DataRow(2, 29)]
        [DataRow(3, 31)]
        [DataRow(4, 30)]
        [DataRow(5, 31)]
        [DataRow(6, 30)]
        [DataRow(7, 31)]
        [DataRow(8, 31)]
        [DataRow(9, 30)]
        [DataRow(10, 31)]
        [DataRow(11, 30)]
        [DataRow(12, 31)]
        public void ReadFromTestValidDay(int month, int day) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.month = (byte)month;
            properties.day = (byte)day;

            var highScore = Th075HighScoreWrapper.Create(MakeByteArray(properties));

            Validate(highScore, properties);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1, 0)]
        [DataRow(1, 32)]
        [DataRow(2, 30)]
        [DataRow(3, 32)]
        [DataRow(4, 31)]
        [DataRow(5, 32)]
        [DataRow(6, 31)]
        [DataRow(7, 32)]
        [DataRow(8, 32)]
        [DataRow(9, 31)]
        [DataRow(10, 32)]
        [DataRow(11, 31)]
        [DataRow(12, 32)]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestInvalidDay(int month, int day) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.month = (byte)month;
            properties.day = (byte)day;

            Th075HighScoreWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
