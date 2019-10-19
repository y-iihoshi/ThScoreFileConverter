using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th095;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th095.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th095BestShotHeaderTests
    {
        internal static BestShotHeaderStub ValidStub { get; } = new BestShotHeaderStub()
        {
            Signature = "BSTS",
            Level = Level.Two,
            Scene = 3,
            Width = 4,
            Height = 5,
            Score = 6,
            SlowRate = 7f,
            CardName = TestUtils.MakeRandomArray<byte>(0x50)
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
                header.Score,
                header.SlowRate,
                header.CardName);

        internal static void Validate(IBestShotHeader expected, in Th095BestShotHeaderWrapper actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Level, actual.Level);
            Assert.AreEqual(expected.Scene, actual.Scene);
            Assert.AreEqual(expected.Width, actual.Width);
            Assert.AreEqual(expected.Height, actual.Height);
            Assert.AreEqual(expected.Score, actual.Score);
            Assert.AreEqual(expected.SlowRate, actual.SlowRate);
            CollectionAssert.That.AreEqual(expected.CardName, actual.CardName);
        }

        [TestMethod]
        public void Th095BestShotHeaderTest() => TestUtils.Wrap(() =>
        {
            var stub = new BestShotHeaderStub();
            var header = new Th095BestShotHeaderWrapper();

            Validate(stub, header);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var header = Th095BestShotHeaderWrapper.Create(MakeByteArray(ValidStub));

            Validate(ValidStub, header);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var header = new Th095BestShotHeaderWrapper();

            header.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestEmptySignature() => TestUtils.Wrap(() =>
        {
            var stub = new BestShotHeaderStub(ValidStub)
            {
                Signature = string.Empty,
            };

            _ = Th095BestShotHeaderWrapper.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestShortenedSignature() => TestUtils.Wrap(() =>
        {
            var stub = new BestShotHeaderStub(ValidStub);
            stub.Signature = stub.Signature.Substring(0, stub.Signature.Length - 1);

            _ = Th095BestShotHeaderWrapper.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestExceededSignature() => TestUtils.Wrap(() =>
        {
            var stub = new BestShotHeaderStub(ValidStub);
            stub.Signature += "E";

            _ = Th095BestShotHeaderWrapper.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var stub = new BestShotHeaderStub(ValidStub)
            {
                Level = TestUtils.Cast<Level>(level),
            };

            _ = Th095BestShotHeaderWrapper.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedCardName() => TestUtils.Wrap(() =>
        {
            var stub = new BestShotHeaderStub(ValidStub);
            stub.CardName = stub.CardName.SkipLast(1).ToArray();

            _ = Th095BestShotHeaderWrapper.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestExceededCardName() => TestUtils.Wrap(() =>
        {
            var stub = new BestShotHeaderStub(ValidStub);
            stub.CardName = stub.CardName.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

            var header = Th095BestShotHeaderWrapper.Create(MakeByteArray(stub));

            Validate(ValidStub, header);
        });
    }
}
