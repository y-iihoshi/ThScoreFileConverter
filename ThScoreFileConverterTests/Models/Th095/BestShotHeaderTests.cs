using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th095;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th095.Stubs;

namespace ThScoreFileConverterTests.Models.Th095
{
    [TestClass]
    public class BestShotHeaderTests
    {
        internal static BestShotHeaderStub ValidStub { get; } = new BestShotHeaderStub()
        {
            Signature = "BSTS",
            Level = Level.Two,
            Scene = 3,
            Width = 4,
            Height = 5,
            ResultScore = 6,
            SlowRate = 7f,
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
                header.ResultScore,
                header.SlowRate,
                header.CardName);

        internal static void Validate(IBestShotHeader expected, in IBestShotHeader actual)
        {
            if (actual is null)
                throw new ArgumentNullException(nameof(actual));

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Level, actual.Level);
            Assert.AreEqual(expected.Scene, actual.Scene);
            Assert.AreEqual(expected.Width, actual.Width);
            Assert.AreEqual(expected.Height, actual.Height);
            Assert.AreEqual(expected.ResultScore, actual.ResultScore);
            Assert.AreEqual(expected.SlowRate, actual.SlowRate);
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
            var header = TestUtils.Create<BestShotHeader>(MakeByteArray(ValidStub));

            Validate(ValidStub, header);
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
#if NETFRAMEWORK
            stub.Signature = stub.Signature.Substring(0, stub.Signature.Length - 1);
#else
            stub.Signature = stub.Signature[0..^1];
#endif

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
