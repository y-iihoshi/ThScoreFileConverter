using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using ThScoreFileConverter.Models.Th143;
using ThScoreFileConverterTests.Models.Th143.Stubs;

namespace ThScoreFileConverterTests.Models.Th143
{
    [TestClass]
    public class BestShotHeaderTests
    {
        internal static IBestShotHeader ValidStub { get; } = new BestShotHeaderStub
        {
            Signature = "BST3",
            Day = Day.Second,
            Scene = 3,
            Width = 4,
            Height = 5,
            DateTime = 6,
            SlowRate = 7f
        };

        internal static byte[] MakeByteArray(IBestShotHeader header)
            => TestUtils.MakeByteArray(
                header.Signature.ToCharArray(),
                (ushort)0,
                TestUtils.Cast<short>(header.Day),
                (short)(header.Scene - 1),
                (ushort)0,
                header.Width,
                header.Height,
                0u,
                header.DateTime,
                header.SlowRate,
                TestUtils.MakeRandomArray<byte>(0x58));

        internal static void Validate(IBestShotHeader expected, IBestShotHeader actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Day, actual.Day);
            Assert.AreEqual(expected.Scene, actual.Scene);
            Assert.AreEqual(expected.Width, actual.Width);
            Assert.AreEqual(expected.Height, actual.Height);
            Assert.AreEqual(expected.DateTime, actual.DateTime);
            Assert.AreEqual(expected.SlowRate, actual.SlowRate);
        }

        [TestMethod]
        public void BestShotHeaderTest() => TestUtils.Wrap(() =>
        {
            var stub = new BestShotHeaderStub();
            var header = new BestShotHeader();

            Validate(stub, header);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var header = TestUtils.Create<BestShotHeader>(MakeByteArray(ValidStub));

            Validate(ValidStub, header);
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
            var stub = new BestShotHeaderStub(ValidStub)
            {
                Signature = string.Empty,
            };

            _ = TestUtils.Create<BestShotHeader>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestShortenedSignature() => TestUtils.Wrap(() =>
        {
            var stub = new BestShotHeaderStub(ValidStub);
            stub.Signature = stub.Signature.Substring(0, stub.Signature.Length - 1);

            _ = TestUtils.Create<BestShotHeader>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestExceededSignature() => TestUtils.Wrap(() =>
        {
            var stub = new BestShotHeaderStub(ValidStub);
            stub.Signature += "E";

            _ = TestUtils.Create<BestShotHeader>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidDays
            => TestUtils.GetInvalidEnumerators(typeof(Day));

        [DataTestMethod]
        [DynamicData(nameof(InvalidDays))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestInvalidDay(int day) => TestUtils.Wrap(() =>
        {
            var stub = new BestShotHeaderStub(ValidStub)
            {
                Day = TestUtils.Cast<Day>(day),
            };

            _ = TestUtils.Create<BestShotHeader>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
