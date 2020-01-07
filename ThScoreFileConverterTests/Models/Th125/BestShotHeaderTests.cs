using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th125;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th125.Stubs;

namespace ThScoreFileConverterTests.Models.Th125
{
    [TestClass]
    public class BestShotHeaderTests
    {
        internal static BestShotHeaderStub ValidStub { get; } = new BestShotHeaderStub()
        {
            Signature = "BST2",
            Level = Level.Two,
            Scene = 3,
            Width = 4,
            Height = 5,
            Width2 = 6,
            Height2 = 7,
            HalfWidth = 8,
            HalfHeight = 9,
            DateTime = 10u,
            SlowRate = 11f,
            Fields = new BonusFields(12),
            ResultScore = 13,
            BasePoint = 14,
            RiskBonus = 15,
            BossShot = 16f,
            NiceShot = 17f,
            AngleBonus = 18f,
            MacroBonus = 19,
            FrontSideBackShot = 20,
            ClearShot = 21,
            Angle = 22f,
            ResultScore2 = 23,
            CardName = TestUtils.CP932Encoding.GetBytes("abcde").Concat(Enumerable.Repeat((byte)'\0', 75)).ToArray(),
        };

        internal static byte[] MakeByteArray(IBestShotHeader header)
            => TestUtils.MakeByteArray(
                header.Signature.ToCharArray(),
                (ushort)0,
                TestUtils.Cast<short>(header.Level + 1),
                header.Scene,
                (ushort)0,
                header.Width,
                header.Height,
                0u,
                header.Width2,
                header.Height2,
                header.HalfWidth,
                header.HalfHeight,
                header.DateTime,
                0u,
                header.SlowRate,
                header.Fields.Data,
                header.ResultScore,
                header.BasePoint,
                TestUtils.MakeRandomArray<byte>(0x8),
                header.RiskBonus,
                header.BossShot,
                header.NiceShot,
                header.AngleBonus,
                header.MacroBonus,
                header.FrontSideBackShot,
                header.ClearShot,
                TestUtils.MakeRandomArray<byte>(0x30),
                header.Angle,
                header.ResultScore2,
                0u,
                header.CardName);

        internal static void Validate(IBestShotHeader expected, IBestShotHeader actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Level, actual.Level);
            Assert.AreEqual(expected.Scene, actual.Scene);
            Assert.AreEqual(expected.Width, actual.Width);
            Assert.AreEqual(expected.Height, actual.Height);
            Assert.AreEqual(expected.Width2, actual.Width2);
            Assert.AreEqual(expected.Height2, actual.Height2);
            Assert.AreEqual(expected.HalfWidth, actual.HalfWidth);
            Assert.AreEqual(expected.HalfHeight, actual.HalfHeight);
            Assert.AreEqual(expected.DateTime, actual.DateTime);
            Assert.AreEqual(expected.SlowRate, actual.SlowRate);
            Assert.AreEqual(expected.Fields, actual.Fields);
            Assert.AreEqual(expected.ResultScore, actual.ResultScore);
            Assert.AreEqual(expected.BasePoint, actual.BasePoint);
            Assert.AreEqual(expected.RiskBonus, actual.RiskBonus);
            Assert.AreEqual(expected.BossShot, actual.BossShot);
            Assert.AreEqual(expected.NiceShot, actual.NiceShot);
            Assert.AreEqual(expected.AngleBonus, actual.AngleBonus);
            Assert.AreEqual(expected.MacroBonus, actual.MacroBonus);
            Assert.AreEqual(expected.FrontSideBackShot, actual.FrontSideBackShot);
            Assert.AreEqual(expected.ClearShot, actual.ClearShot);
            Assert.AreEqual(expected.Angle, actual.Angle);
            Assert.AreEqual(expected.ResultScore2, actual.ResultScore2);
            CollectionAssert.That.AreEqual(expected.CardName, actual.CardName);
        }

        [TestMethod]
        public void BestShotHeaderTest()
        {
            var stub = new BestShotHeaderStub();
            var header = new BestShotHeader();

            Validate(stub, header);
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var stub = ValidStub;
            var header = TestUtils.Create<BestShotHeader>(MakeByteArray(stub));

            Validate(stub, header);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull()
        {
            var header = new BestShotHeader();

            header.ReadFrom(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestEmptySignature()
        {
            var stub = new BestShotHeaderStub(ValidStub)
            {
                Signature = string.Empty,
            };

            _ = TestUtils.Create<BestShotHeader>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestShortenedSignature()
        {
            var stub = new BestShotHeaderStub(ValidStub);
            stub.Signature = stub.Signature.Substring(0, stub.Signature.Length - 1);

            _ = TestUtils.Create<BestShotHeader>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestExceededSignature()
        {
            var stub = new BestShotHeaderStub(ValidStub);
            stub.Signature += "E";

            _ = TestUtils.Create<BestShotHeader>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        }

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestInvalidLevel(int level)
        {
            var stub = new BestShotHeaderStub(ValidStub)
            {
                Level = TestUtils.Cast<Level>(level),
            };

            _ = TestUtils.Create<BestShotHeader>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedCardName()
        {
            var stub = new BestShotHeaderStub(ValidStub);
            stub.CardName = stub.CardName.SkipLast(1).ToArray();

            _ = TestUtils.Create<BestShotHeader>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ReadFromTestExceededCardName()
        {
            var stub = new BestShotHeaderStub(ValidStub);
            stub.CardName = stub.CardName.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

            var header = TestUtils.Create<BestShotHeader>(MakeByteArray(stub));

            Validate(ValidStub, header);
        }
    }
}
