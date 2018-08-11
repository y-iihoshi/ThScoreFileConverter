using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th06ClearDataTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public Dictionary<ThConverter.Level, byte> storyFlags;
            public Dictionary<ThConverter.Level, byte> practiceFlags;
            public Th06Converter.Chara chara;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "CLRD",
            size1 = 0x18,
            size2 = 0x18,
            storyFlags = Utils.GetEnumerator<ThConverter.Level>()
                .Select((level, index) => new { level, index })
                .ToDictionary(pair => pair.level, pair => (byte)pair.index),
            practiceFlags = Utils.GetEnumerator<ThConverter.Level>()
                .Select((level, index) => new { level, index })
                .ToDictionary(pair => pair.level, pair => (byte)(10 - pair.index)),
            chara = Th06Converter.Chara.ReimuB
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                0u,
                properties.storyFlags.Values.ToArray(),
                properties.practiceFlags.Values.ToArray(),
                (short)properties.chara);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));

        internal static void Validate(in Th06ClearDataWrapper clearData, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, clearData.Signature);
            Assert.AreEqual(properties.size1, clearData.Size1);
            Assert.AreEqual(properties.size2, clearData.Size2);
            CollectionAssert.AreEqual(data, clearData.Data.ToArray());
            Assert.AreEqual(data[0], clearData.FirstByteOfData);
            CollectionAssert.AreEqual(properties.storyFlags.Values, clearData.StoryFlags.Values.ToArray());
            CollectionAssert.AreEqual(properties.practiceFlags.Values, clearData.PracticeFlags.Values.ToArray());
            Assert.AreEqual(properties.chara, clearData.Chara);
        }

        [TestMethod()]
        public void Th06ClearDataTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(MakeByteArray(properties));
            var clearData = new Th06ClearDataWrapper(chapter);

            Validate(clearData, properties);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th06ClearDataTestNullChapter() => TestUtils.Wrap(() =>
        {
            var clearData = new Th06ClearDataWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06ClearDataTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(MakeByteArray(properties));
            var clearData = new Th06ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06ClearDataTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.size1;

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(MakeByteArray(properties));
            var clearData = new Th06ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(ThConverter.Level));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th06ClearDataTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.chara = TestUtils.Cast<Th06Converter.Chara>(chara);

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(MakeByteArray(properties));
            var clearData = new Th06ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
