using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models.Th125;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests.Models.Th125
{
    [TestClass]
    public class BestShotHeaderTests
    {
        internal struct Properties
        {
            public string signature;
            public Level level;
            public short scene;
            public short width;
            public short height;
            public short width2;
            public short height2;
            public short halfWidth;
            public short halfHeight;
            public uint dateTime;
            public float slowRate;
            public int fields;
            public int resultScore;
            public int basePoint;
            public int riskBonus;
            public float bossShot;
            public float niceShot;
            public float angleBonus;
            public int macroBonus;
            public int frontSideBackShot;
            public int clearShot;
            public float angle;
            public int resultScore2;
            public byte[] cardName;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "BST2",
            level = Level.Two,
            scene = 3,
            width = 4,
            height = 5,
            width2 = 6,
            height2 = 7,
            halfWidth = 8,
            halfHeight = 9,
            dateTime = 10u,
            slowRate = 11f,
            fields = 12,
            resultScore = 13,
            basePoint = 14,
            riskBonus = 15,
            bossShot = 16f,
            niceShot = 17f,
            angleBonus = 18f,
            macroBonus = 19,
            frontSideBackShot = 20,
            clearShot = 21,
            angle = 22f,
            resultScore2 = 23,
            cardName = TestUtils.MakeRandomArray<byte>(0x50),
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
                0u,
                properties.width2,
                properties.height2,
                properties.halfWidth,
                properties.halfHeight,
                properties.dateTime,
                0u,
                properties.slowRate,
                properties.fields,
                properties.resultScore,
                properties.basePoint,
                TestUtils.MakeRandomArray<byte>(0x8),
                properties.riskBonus,
                properties.bossShot,
                properties.niceShot,
                properties.angleBonus,
                properties.macroBonus,
                properties.frontSideBackShot,
                properties.clearShot,
                TestUtils.MakeRandomArray<byte>(0x30),
                properties.angle,
                properties.resultScore2,
                0u,
                properties.cardName);

        internal static void Validate(in Properties expected, in BestShotHeader actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            Assert.AreEqual(expected.signature, actual.Signature);
            Assert.AreEqual(expected.level, actual.Level);
            Assert.AreEqual(expected.scene, actual.Scene);
            Assert.AreEqual(expected.width, actual.Width);
            Assert.AreEqual(expected.height, actual.Height);
            Assert.AreEqual(expected.width2, actual.Width2);
            Assert.AreEqual(expected.height2, actual.Height2);
            Assert.AreEqual(expected.halfWidth, actual.HalfWidth);
            Assert.AreEqual(expected.halfHeight, actual.HalfHeight);
            Assert.AreEqual(expected.dateTime, actual.DateTime);
            Assert.AreEqual(expected.slowRate, actual.SlowRate);
            Assert.AreEqual(expected.fields, actual.Fields.Data);
            Assert.AreEqual(expected.resultScore, actual.ResultScore);
            Assert.AreEqual(expected.basePoint, actual.BasePoint);
            Assert.AreEqual(expected.riskBonus, actual.RiskBonus);
            Assert.AreEqual(expected.bossShot, actual.BossShot);
            Assert.AreEqual(expected.niceShot, actual.NiceShot);
            Assert.AreEqual(expected.angleBonus, actual.AngleBonus);
            Assert.AreEqual(expected.macroBonus, actual.MacroBonus);
            Assert.AreEqual(expected.frontSideBackShot, actual.FrontSideBackShot);
            Assert.AreEqual(expected.clearShot, actual.ClearShot);
            Assert.AreEqual(expected.angle, actual.Angle);
            Assert.AreEqual(expected.resultScore2, actual.ResultScore2);
            CollectionAssert.That.AreEqual(expected.cardName, actual.CardName);
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
            var properties = ValidProperties;

            var header = TestUtils.Create<BestShotHeader>(MakeByteArray(properties));

            Validate(properties, header);
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

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.level = TestUtils.Cast<Level>(level);

            _ = TestUtils.Create<BestShotHeader>(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedCardName() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.cardName = properties.cardName.Take(properties.cardName.Length - 1).ToArray();

            _ = TestUtils.Create<BestShotHeader>(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestExceededCardName() => TestUtils.Wrap(() =>
        {
            var validProperties = ValidProperties;
            var properties = validProperties;
            properties.cardName = properties.cardName.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

            var header = TestUtils.Create<BestShotHeader>(MakeByteArray(properties));

            Validate(validProperties, header);
        });
    }
}
