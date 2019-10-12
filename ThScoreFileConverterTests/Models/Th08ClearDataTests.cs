using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using ThScoreFileConverterTests.Models.Th08.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th08ClearDataTests
    {
        internal static ClearDataStub ValidStub { get; } = new ClearDataStub()
        {
            Signature = "CLRD",
            Size1 = 0x24,
            Size2 = 0x24,
            StoryFlags = Utils.GetEnumerator<Level>()
                .Select((level, index) => (level, index))
                .ToDictionary(pair => pair.level, pair => (PlayableStages)pair.index),
            PracticeFlags = Utils.GetEnumerator<Level>()
                .Select((level, index) => (level, index))
                .ToDictionary(pair => pair.level, pair => (PlayableStages)(10 - pair.index)),
            Chara = CharaWithTotal.MarisaAlice
        };

        internal static byte[] MakeData(IClearData clearData)
            => TestUtils.MakeByteArray(
                0u,
                clearData.StoryFlags.Values.Select(value => (ushort)value).ToArray(),
                clearData.PracticeFlags.Values.Select(value => (ushort)value).ToArray(),
                (byte)0,
                (byte)clearData.Chara,
                (ushort)0);

        internal static byte[] MakeByteArray(IClearData clearData)
            => TestUtils.MakeByteArray(
                clearData.Signature.ToCharArray(), clearData.Size1, clearData.Size2, MakeData(clearData));

        internal static void Validate(IClearData expected, in Th08ClearDataWrapper actual)
        {
            var data = MakeData(expected);

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Size1, actual.Size1);
            Assert.AreEqual(expected.Size2, actual.Size2);
            CollectionAssert.That.AreEqual(data, actual.Data);
            Assert.AreEqual(data[0], actual.FirstByteOfData);
            CollectionAssert.That.AreEqual(expected.StoryFlags.Values, actual.StoryFlags.Values);
            CollectionAssert.That.AreEqual(expected.PracticeFlags.Values, actual.PracticeFlags.Values);
            Assert.AreEqual(expected.Chara, actual.Chara);
        }

        [TestMethod]
        public void Th08ClearDataTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            var clearData = new Th08ClearDataWrapper(chapter);

            Validate(stub, clearData);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08ClearDataTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new Th08ClearDataWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08ClearDataTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new ClearDataStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th08ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08ClearDataTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var stub = new ClearDataStub(ValidStub);
            --stub.Size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th08ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(CharaWithTotal));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th08ClearDataTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var stub = new ClearDataStub(ValidStub)
            {
                Chara = TestUtils.Cast<CharaWithTotal>(chara),
            };

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th08ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
