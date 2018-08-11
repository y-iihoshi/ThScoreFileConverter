using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th125BestShotHeaderTests
    {
        internal struct Properties
        {
            public string signature;
            public Th125Converter.Level level;
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
            level = Th125Converter.Level.Lv2,
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

        internal static void Validate(in Th125BestShotHeaderWrapper header, in Properties properties)
        {
            if (header == null)
                throw new ArgumentNullException(nameof(header));

            Assert.AreEqual(properties.signature, header.Signature);
            Assert.AreEqual(properties.level, header.Level);
            Assert.AreEqual(properties.scene, header.Scene);
            Assert.AreEqual(properties.width, header.Width);
            Assert.AreEqual(properties.height, header.Height);
            Assert.AreEqual(properties.width2, header.Width2);
            Assert.AreEqual(properties.height2, header.Height2);
            Assert.AreEqual(properties.halfWidth, header.HalfWidth);
            Assert.AreEqual(properties.halfHeight, header.HalfHeight);
            Assert.AreEqual(properties.dateTime, header.DateTime);
            Assert.AreEqual(properties.slowRate, header.SlowRate);
            Assert.AreEqual(properties.fields, header.Fields.Data);
            Assert.AreEqual(properties.resultScore, header.ResultScore);
            Assert.AreEqual(properties.basePoint, header.BasePoint);
            Assert.AreEqual(properties.riskBonus, header.RiskBonus);
            Assert.AreEqual(properties.bossShot, header.BossShot);
            Assert.AreEqual(properties.niceShot, header.NiceShot);
            Assert.AreEqual(properties.angleBonus, header.AngleBonus);
            Assert.AreEqual(properties.macroBonus, header.MacroBonus);
            Assert.AreEqual(properties.frontSideBackShot, header.FrontSideBackShot);
            Assert.AreEqual(properties.clearShot, header.ClearShot);
            Assert.AreEqual(properties.angle, header.Angle);
            Assert.AreEqual(properties.resultScore2, header.ResultScore2);
            CollectionAssert.AreEqual(properties.cardName, header.CardName?.ToArray());
        }

        [TestMethod]
        public void Th125BestShotHeaderTest() => TestUtils.Wrap(() =>
        {
            var properties = new Properties();
            var header = new Th125BestShotHeaderWrapper();

            Validate(header, properties);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var header = Th125BestShotHeaderWrapper.Create(MakeByteArray(ValidProperties));

            Validate(header, ValidProperties);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var header = new Th125BestShotHeaderWrapper();

            header.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestEmptySignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = string.Empty;

            var header = Th125BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.Substring(0, properties.signature.Length - 1);

            var header = Th125BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestExceededSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature += "E";

            var header = Th125BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Assert.AreNotEqual(properties.signature, header.Signature);
            Assert.AreEqual(properties.signature.Substring(0, ValidProperties.signature.Length), header.Signature);
            Assert.AreNotEqual(properties.level, header.Level);
            Assert.AreNotEqual(properties.scene, header.Scene);
            Assert.AreNotEqual(properties.width, header.Width);
            Assert.AreNotEqual(properties.height, header.Height);
            Assert.AreNotEqual(properties.width2, header.Width2);
            Assert.AreNotEqual(properties.height2, header.Height2);
            Assert.AreNotEqual(properties.halfWidth, header.HalfWidth);
            Assert.AreNotEqual(properties.halfHeight, header.HalfHeight);
            Assert.AreNotEqual(properties.dateTime, header.DateTime);
            Assert.AreNotEqual(properties.slowRate, header.SlowRate);
            Assert.AreNotEqual(properties.fields, header.Fields.Data);
            Assert.AreNotEqual(properties.resultScore, header.ResultScore);
            Assert.AreNotEqual(properties.basePoint, header.BasePoint);
            Assert.AreNotEqual(properties.riskBonus, header.RiskBonus);
            Assert.AreNotEqual(properties.bossShot, header.BossShot);
            Assert.AreNotEqual(properties.niceShot, header.NiceShot);
            Assert.AreNotEqual(properties.angleBonus, header.AngleBonus);
            Assert.AreNotEqual(properties.macroBonus, header.MacroBonus);
            Assert.AreNotEqual(properties.frontSideBackShot, header.FrontSideBackShot);
            Assert.AreNotEqual(properties.clearShot, header.ClearShot);
            Assert.AreNotEqual(properties.angle, header.Angle);
            Assert.AreNotEqual(properties.resultScore2, header.ResultScore2);
            CollectionAssert.AreNotEqual(properties.cardName, header.CardName.ToArray());
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Th125Converter.Level));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.level = TestUtils.Cast<Th125Converter.Level>(level);

            var header = Th125BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedCardName() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.cardName = properties.cardName.Take(properties.cardName.Length - 1).ToArray();

            var header = Th125BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestExceededCardName() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.cardName = properties.cardName.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

            var header = Th125BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Validate(header, ValidProperties);
        });
    }
}
