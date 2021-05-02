using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th143;
using ThScoreFileConverterTests.UnitTesting;

namespace ThScoreFileConverterTests.Models.Th143
{
    [TestClass]
    public class BestShotHeaderTests
    {
        internal static Mock<IBestShotHeader> MockInitialBestShotHeader()
        {
            var mock = new Mock<IBestShotHeader>();
            _ = mock.SetupGet(m => m.Signature).Returns(string.Empty);
            return mock;
        }

        internal static Mock<IBestShotHeader> MockBestShotHeader()
        {
            var mock = new Mock<IBestShotHeader>();
            _ = mock.SetupGet(m => m.Signature).Returns("BST3");
            _ = mock.SetupGet(m => m.Day).Returns(Day.Second);
            _ = mock.SetupGet(m => m.Scene).Returns(3);
            _ = mock.SetupGet(m => m.Width).Returns(4);
            _ = mock.SetupGet(m => m.Height).Returns(5);
            _ = mock.SetupGet(m => m.DateTime).Returns(6);
            _ = mock.SetupGet(m => m.SlowRate).Returns(7);
            return mock;
        }

        internal static byte[] MakeByteArray(IBestShotHeader header)
        {
            return TestUtils.MakeByteArray(
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
        }

        internal static void Validate(IBestShotHeader expected, IBestShotHeader actual)
        {
            if (actual is null)
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
        public void BestShotHeaderTest()
        {
            var mock = MockInitialBestShotHeader();
            var header = new BestShotHeader();

            Validate(mock.Object, header);
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var mock = MockBestShotHeader();
            var header = TestUtils.Create<BestShotHeader>(MakeByteArray(mock.Object));

            Validate(mock.Object, header);
        }

        [TestMethod]
        public void ReadFromTestEmptySignature()
        {
            var mock = new Mock<IBestShotHeader>();
            _ = mock.SetupGet(m => m.Signature).Returns(string.Empty);

            _ = Assert.ThrowsException<InvalidDataException>(
                () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock.Object)));
        }

        [TestMethod]
        public void ReadFromTestShortenedSignature()
        {
            var mock = MockBestShotHeader();
            var signature = mock.Object.Signature;
#if NETFRAMEWORK
            _ = mock.SetupGet(m => m.Signature).Returns(signature.Substring(0, signature.Length - 1));
#else
            _ = mock.SetupGet(m => m.Signature).Returns(signature[0..^1]);
#endif

            _ = Assert.ThrowsException<InvalidDataException>(
                () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock.Object)));
        }

        [TestMethod]
        public void ReadFromTestExceededSignature()
        {
            var mock = MockBestShotHeader();
            var signature = mock.Object.Signature;
            _ = mock.SetupGet(m => m.Signature).Returns(signature + "E");

            _ = Assert.ThrowsException<InvalidCastException>(
                () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock.Object)));
        }

        public static IEnumerable<object[]> InvalidDays
            => TestUtils.GetInvalidEnumerators(typeof(Day));

        [DataTestMethod]
        [DynamicData(nameof(InvalidDays))]
        public void ReadFromTestInvalidDay(int day)
        {
            var mock = MockBestShotHeader();
            _ = mock.SetupGet(m => m.Day).Returns(TestUtils.Cast<Day>(day));

            _ = Assert.ThrowsException<InvalidCastException>(
                () => TestUtils.Create<BestShotHeader>(MakeByteArray(mock.Object)));
        }
    }
}
