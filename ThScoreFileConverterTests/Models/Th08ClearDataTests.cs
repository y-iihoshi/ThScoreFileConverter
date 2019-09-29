using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th08ClearDataTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public Dictionary<Level, int> storyFlags;
            public Dictionary<Level, int> practiceFlags;
            public Th08Converter.CharaWithTotal chara;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "CLRD",
            size1 = 0x24,
            size2 = 0x24,
            storyFlags = Utils.GetEnumerator<Level>()
                .Select((level, index) => new { level, index })
                .ToDictionary(pair => pair.level, pair => pair.index),
            practiceFlags = Utils.GetEnumerator<Level>()
                .Select((level, index) => new { level, index })
                .ToDictionary(pair => pair.level, pair => (10 - pair.index)),
            chara = Th08Converter.CharaWithTotal.MarisaAlice
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                0u,
                properties.storyFlags.Values.Select(value => (ushort)value).ToArray(),
                properties.practiceFlags.Values.Select(value => (ushort)value).ToArray(),
                (byte)0,
                (byte)properties.chara,
                (ushort)0);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));

        internal static void Validate(in Th08ClearDataWrapper clearData, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, clearData.Signature);
            Assert.AreEqual(properties.size1, clearData.Size1);
            Assert.AreEqual(properties.size2, clearData.Size2);
            CollectionAssert.That.AreEqual(data, clearData.Data);
            Assert.AreEqual(data[0], clearData.FirstByteOfData);
            CollectionAssert.That.AreEqual(properties.storyFlags.Values, clearData.ValuesOfStoryFlags);
            CollectionAssert.That.AreEqual(properties.practiceFlags.Values, clearData.ValuesOfPracticeFlags);
            Assert.AreEqual(properties.chara, clearData.Chara);
        }

        [TestMethod]
        public void Th08ClearDataTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var clearData = new Th08ClearDataWrapper(chapter);

            Validate(clearData, properties);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08ClearDataTestNullChapter() => TestUtils.Wrap(() =>
        {
            var clearData = new Th08ClearDataWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08ClearDataTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var clearData = new Th08ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08ClearDataTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var clearData = new Th08ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Th08Converter.CharaWithTotal));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th08ClearDataTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.chara = TestUtils.Cast<Th08Converter.CharaWithTotal>(chara);

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var clearData = new Th08ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
