using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th06.Stubs;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Th06
{
    [TestClass]
    public class ClearDataTests
    {
        internal static ClearDataStub ValidStub { get; } = new ClearDataStub()
        {
            Signature = "CLRD",
            Size1 = 0x18,
            Size2 = 0x18,
            StoryFlags = Utils.GetEnumerator<Level>()
                .Select((level, index) => new { level, index })
                .ToDictionary(pair => pair.level, pair => (byte)pair.index),
            PracticeFlags = Utils.GetEnumerator<Level>()
                .Select((level, index) => new { level, index })
                .ToDictionary(pair => pair.level, pair => (byte)(10 - pair.index)),
            Chara = Chara.ReimuB
        };

        internal static byte[] MakeByteArray(IClearData clearData)
            => TestUtils.MakeByteArray(
                clearData.Signature.ToCharArray(),
                clearData.Size1,
                clearData.Size2,
                0u,
                clearData.StoryFlags.Values.ToArray(),
                clearData.PracticeFlags.Values.ToArray(),
                (short)clearData.Chara);

        internal static void Validate(IClearData expected, IClearData actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Size1, actual.Size1);
            Assert.AreEqual(expected.Size2, actual.Size2);
            Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
            CollectionAssert.That.AreEqual(expected.StoryFlags?.Values, actual.StoryFlags?.Values);
            CollectionAssert.That.AreEqual(expected.PracticeFlags?.Values, actual.PracticeFlags?.Values);
            Assert.AreEqual(expected.Chara, actual.Chara);
        }

        [TestMethod]
        public void ClearDataTestChapter() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(MakeByteArray(ValidStub));
            var clearData = new ClearData(chapter.Target);

            Validate(ValidStub, clearData);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ClearDataTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new ClearData(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new ClearDataStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new ClearData(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var stub = new ClearDataStub(ValidStub);
            --stub.Size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new ClearData(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ClearDataTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var stub = new ClearDataStub(ValidStub)
            {
                Chara = TestUtils.Cast<Chara>(chara),
            };

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new ClearData(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
