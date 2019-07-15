using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th128CardDataTests
    {
        internal struct Properties
        {
            public string signature;
            public ushort version;
            public uint checksum;
            public int size;
            public Dictionary<int, Th128SpellCardTests.Properties> cards;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "CD",
            version = 1,
            checksum = 0u,
            size = 0x947C,
            cards = Enumerable.Range(1, 250).ToDictionary(
                index => index,
                index => new Th128SpellCardTests.Properties()
                {
                    name = TestUtils.MakeRandomArray<byte>(0x80),
                    noMissCount = 123 + index,
                    noIceCount = 456 + index,
                    trialCount = 789 + index,
                    id = index,
                    level = ThConverter.Level.Hard
                })
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.cards.Values.SelectMany(
                    card => Th128SpellCardTests.MakeByteArray(card)).ToArray());

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.version,
                properties.checksum,
                properties.size,
                MakeData(properties));

        internal static void Validate(in Th128CardDataWrapper clearData, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, clearData.Signature);
            Assert.AreEqual(properties.version, clearData.Version);
            Assert.AreEqual(properties.checksum, clearData.Checksum);
            Assert.AreEqual(properties.size, clearData.Size);
            CollectionAssert.AreEqual(data, clearData.Data.ToArray());

            foreach (var pair in properties.cards)
            {
                Th128SpellCardTests.Validate(clearData.CardsItem(pair.Key), pair.Value);
            }
        }

        [TestMethod]
        public void Th128CardDataTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = Th10ChapterWrapper.Create(MakeByteArray(properties));
            var clearData = new Th128CardDataWrapper(chapter);

            Validate(clearData, properties);
            Assert.IsFalse(clearData.IsValid.Value);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th128CardDataTestNullChapter() => TestUtils.Wrap(() =>
        {
            var clearData = new Th128CardDataWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th128CardDataTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = Th10ChapterWrapper.Create(MakeByteArray(properties));
            var clearData = new Th128CardDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th128CardDataTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.version;

            var chapter = Th10ChapterWrapper.Create(MakeByteArray(properties));
            var clearData = new Th128CardDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th128CardDataTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size;

            var chapter = Th10ChapterWrapper.Create(MakeByteArray(properties));
            var clearData = new Th128CardDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("CD", (ushort)1, 0x947C, true)]
        [DataRow("cd", (ushort)1, 0x947C, false)]
        [DataRow("CD", (ushort)0, 0x947C, false)]
        [DataRow("CD", (ushort)1, 0x947D, false)]
        public void Th128CardDataCanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = Th10ChapterWrapper.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(
                    expected, Th128CardDataWrapper.CanInitialize(chapter));
            });
    }
}
