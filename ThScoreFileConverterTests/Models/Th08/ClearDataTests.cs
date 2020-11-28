using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Extensions;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class ClearDataTests
    {
        internal static Mock<IClearData> MockClearData()
        {
            var pairs = EnumHelper.GetEnumerable<Level>().Select((level, index) => (level, index));
            var mock = new Mock<IClearData>();
            _ = mock.SetupGet(m => m.Signature).Returns("CLRD");
            _ = mock.SetupGet(m => m.Size1).Returns(0x24);
            _ = mock.SetupGet(m => m.Size2).Returns(0x24);
            _ = mock.SetupGet(m => m.StoryFlags).Returns(
                pairs.ToDictionary(pair => pair.level, pair => (PlayableStages)pair.index));
            _ = mock.SetupGet(m => m.PracticeFlags).Returns(
                pairs.ToDictionary(pair => pair.level, pair => (PlayableStages)(10 - pair.index)));
            _ = mock.SetupGet(m => m.Chara).Returns(CharaWithTotal.MarisaAlice);
            return mock;
        }

        internal static byte[] MakeByteArray(IClearData clearData)
            => TestUtils.MakeByteArray(
                clearData.Signature.ToCharArray(),
                clearData.Size1,
                clearData.Size2,
                0u,
                clearData.StoryFlags.Values.Select(value => (ushort)value).ToArray(),
                clearData.PracticeFlags.Values.Select(value => (ushort)value).ToArray(),
                (byte)0,
                (byte)clearData.Chara,
                (ushort)0);

        internal static void Validate(IClearData expected, IClearData actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Size1, actual.Size1);
            Assert.AreEqual(expected.Size2, actual.Size2);
            Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
            CollectionAssert.That.AreEqual(expected.StoryFlags.Values, actual.StoryFlags.Values);
            CollectionAssert.That.AreEqual(expected.PracticeFlags.Values, actual.PracticeFlags.Values);
            Assert.AreEqual(expected.Chara, actual.Chara);
        }

        [TestMethod]
        public void ClearDataTestChapter()
        {
            var mock = MockClearData();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            var clearData = new ClearData(chapter);

            Validate(mock.Object, clearData);
        }

        [TestMethod]
        public void ClearDataTestNullChapter()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new ClearData(null!));

        [TestMethod]
        public void ClearDataTestInvalidSignature()
        {
            var mock = MockClearData();
            var signature = mock.Object.Signature;
            _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new ClearData(chapter));
        }

        [TestMethod]
        public void ClearDataTestInvalidSize1()
        {
            var mock = MockClearData();
            var size = mock.Object.Size1;
            _ = mock.SetupGet(m => m.Size1).Returns(--size);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new ClearData(chapter));
        }

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(CharaWithTotal));

        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        public void ClearDataTestInvalidChara(int chara)
        {
            var mock = MockClearData();
            _ = mock.SetupGet(m => m.Chara).Returns(TestUtils.Cast<CharaWithTotal>(chara));

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidCastException>(() => _ = new ClearData(chapter));
        }
    }
}
