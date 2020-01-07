using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th08.Stubs;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class ClearDataTests
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
            var stub = ValidStub;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var clearData = new ClearData(chapter);

            Validate(stub, clearData);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ClearDataTestNullChapter()
        {
            _ = new ClearData(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidSignature()
        {
            var stub = new ClearDataStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidSize1()
        {
            var stub = new ClearDataStub(ValidStub);
            --stub.Size1;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(CharaWithTotal));

        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ClearDataTestInvalidChara(int chara)
        {
            var stub = new ClearDataStub(ValidStub)
            {
                Chara = TestUtils.Cast<CharaWithTotal>(chara),
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
