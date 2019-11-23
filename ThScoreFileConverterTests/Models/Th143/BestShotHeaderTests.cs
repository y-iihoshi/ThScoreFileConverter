using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverterTests.Models.Th143
{
    [TestClass]
    public class BestShotHeaderTests
    {
        internal struct Properties
        {
            public string signature;
            public Day day;
            public short scene;
            public short width;
            public short height;
            public uint dateTime;
            public float slowRate;
        };

        internal static Properties ValidProperties { get; } = new Properties()
        {
            signature = "BST3",
            day = Day.Second,
            scene = 3,
            width = 4,
            height = 5,
            dateTime = 6,
            slowRate = 7f
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                (ushort)0,
                TestUtils.Cast<short>(properties.day),
                (short)(properties.scene - 1),
                (ushort)0,
                properties.width,
                properties.height,
                0u,
                properties.dateTime,
                properties.slowRate,
                TestUtils.MakeRandomArray<byte>(0x58));

        internal static void Validate(in Properties expected, in BestShotHeader actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            Assert.AreEqual(expected.signature, actual.Signature);
            Assert.AreEqual(expected.day, actual.Day);
            Assert.AreEqual(expected.scene, actual.Scene);
            Assert.AreEqual(expected.width, actual.Width);
            Assert.AreEqual(expected.height, actual.Height);
            Assert.AreEqual(expected.dateTime, actual.DateTime);
            Assert.AreEqual(expected.slowRate, actual.SlowRate);
        }

        [TestMethod]
        public void BestShotHeaderTest() => TestUtils.Wrap(() =>
        {
            var properties = new Properties();
            var header = new BestShotHeader();

            Validate(properties, header);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var header = TestUtils.Create<BestShotHeader>(MakeByteArray(ValidProperties));

            Validate(ValidProperties, header);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var header = new BestShotHeader();

            header.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestEmptySignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = string.Empty;

            _ = TestUtils.Create<BestShotHeader>(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestShortenedSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.Substring(0, properties.signature.Length - 1);

            _ = TestUtils.Create<BestShotHeader>(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestExceededSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature += "E";

            _ = TestUtils.Create<BestShotHeader>(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidDays
            => TestUtils.GetInvalidEnumerators(typeof(Day));

        [DataTestMethod]
        [DynamicData(nameof(InvalidDays))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestInvalidDay(int day) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.day = TestUtils.Cast<Day>(day);

            _ = TestUtils.Create<BestShotHeader>(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
