using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;
using Level = ThScoreFileConverter.Models.Th07.Level;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class ClearDataTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public Dictionary<Level, byte> storyFlags;
            public Dictionary<Level, byte> practiceFlags;
            public Chara chara;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "CLRD",
            size1 = 0x1C,
            size2 = 0x1C,
            storyFlags = Utils.GetEnumerator<Level>()
                .Select((level, index) => new { level, index })
                .ToDictionary(pair => pair.level, pair => (byte)pair.index),
            practiceFlags = Utils.GetEnumerator<Level>()
                .Select((level, index) => new { level, index })
                .ToDictionary(pair => pair.level, pair => (byte)(10 - pair.index)),
            chara = Chara.ReimuB
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                0u,
                properties.storyFlags.Values.ToArray(),
                properties.practiceFlags.Values.ToArray(),
                (int)properties.chara);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));

        internal static void Validate(in ClearData clearData, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, clearData.Signature);
            Assert.AreEqual(properties.size1, clearData.Size1);
            Assert.AreEqual(properties.size2, clearData.Size2);
            Assert.AreEqual(data[0], clearData.FirstByteOfData);
            CollectionAssert.AreEqual(properties.storyFlags.Values, clearData.StoryFlags.Values.ToArray());
            CollectionAssert.AreEqual(properties.practiceFlags.Values, clearData.PracticeFlags.Values.ToArray());
            Assert.AreEqual(properties.chara, clearData.Chara);
        }

        [TestMethod]
        public void ClearDataTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var clearData = new ClearData(chapter.Target);

            Validate(clearData, properties);
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
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            _ = new ClearData(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            _ = new ClearData(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Chara));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ClearDataTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.chara = TestUtils.Cast<Chara>(chara);

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            _ = new ClearData(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
