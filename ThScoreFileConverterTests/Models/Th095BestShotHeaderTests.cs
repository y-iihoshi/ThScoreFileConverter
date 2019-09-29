using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th095BestShotHeaderTests
    {
        internal struct Properties
        {
            public string signature;
            public Th095Converter.Level level;
            public short scene;
            public short width;
            public short height;
            public int score;
            public float slowRate;
            public byte[] cardName;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "BSTS",
            level = Th095Converter.Level.Lv2,
            scene = 3,
            width = 4,
            height = 5,
            score = 6,
            slowRate = 7f,
            cardName = TestUtils.MakeRandomArray<byte>(0x50)
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                (ushort)0,
                TestUtils.Cast<short>(properties.level + 1),
                properties.scene,
                (ushort)0,
                properties.width,
                properties.height,
                properties.score,
                properties.slowRate,
                properties.cardName);

        internal static void Validate(in Th095BestShotHeaderWrapper header, in Properties properties)
        {
            if (header == null)
                throw new ArgumentNullException(nameof(header));

            Assert.AreEqual(properties.signature, header.Signature);
            Assert.AreEqual(properties.level, header.Level);
            Assert.AreEqual(properties.scene, header.Scene);
            Assert.AreEqual(properties.width, header.Width);
            Assert.AreEqual(properties.height, header.Height);
            Assert.AreEqual(properties.score, header.Score);
            Assert.AreEqual(properties.slowRate, header.SlowRate);
            CollectionAssert.That.AreEqual(properties.cardName, header.CardName);
        }

        [TestMethod]
        public void Th095BestShotHeaderTest() => TestUtils.Wrap(() =>
        {
            var properties = new Properties();
            var header = new Th095BestShotHeaderWrapper();

            Validate(header, properties);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var header = Th095BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Validate(header, properties);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var header = new Th095BestShotHeaderWrapper();

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

            var header = Th095BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestShortenedSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.Substring(0, properties.signature.Length - 1);

            var header = Th095BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestExceededSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature += "E";

            var header = Th095BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Th095Converter.Level));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.level = TestUtils.Cast<Th095Converter.Level>(level);

            var header = Th095BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedCardName() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.cardName = properties.cardName.Take(properties.cardName.Length - 1).ToArray();

            var header = Th095BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestExceededCardName() => TestUtils.Wrap(() =>
        {
            var validProperties = ValidProperties;
            var properties = validProperties;
            properties.cardName = properties.cardName.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

            var header = Th095BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Validate(header, validProperties);
        });
    }
}
