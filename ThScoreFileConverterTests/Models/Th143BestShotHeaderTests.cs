using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th143BestShotHeaderTests
    {
        internal struct Properties
        {
            public string signature;
            public Th143Converter.Day day;
            public short scene;
            public short width;
            public short height;
            public uint dateTime;
            public float slowRate;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "BST3",
            day = Th143Converter.Day.Day2,
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

        internal static void Validate(in Th143BestShotHeaderWrapper header, in Properties properties)
        {
            if (header == null)
                throw new ArgumentNullException(nameof(header));

            Assert.AreEqual(properties.signature, header.Signature);
            Assert.AreEqual(properties.day, header.Day);
            Assert.AreEqual(properties.scene, header.Scene);
            Assert.AreEqual(properties.width, header.Width);
            Assert.AreEqual(properties.height, header.Height);
            Assert.AreEqual(properties.dateTime, header.DateTime);
            Assert.AreEqual(properties.slowRate, header.SlowRate);
        }

        [TestMethod]
        public void Th143BestShotHeaderTest() => TestUtils.Wrap(() =>
        {
            var properties = new Properties();
            var header = new Th143BestShotHeaderWrapper();

            Validate(header, properties);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var header = Th143BestShotHeaderWrapper.Create(MakeByteArray(ValidProperties));

            Validate(header, ValidProperties);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var header = new Th143BestShotHeaderWrapper();

            header.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestEmptySignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = string.Empty;

            var header = Th143BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestShortenedSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.Substring(0, properties.signature.Length - 1);

            var header = Th143BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestExceededSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature += "E";

            var header = Th143BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidDays
            => TestUtils.GetInvalidEnumerators(typeof(Th143Converter.Day));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidDays))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestInvalidDay(int day) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.day = TestUtils.Cast<Th143Converter.Day>(day);

            var header = Th143BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
