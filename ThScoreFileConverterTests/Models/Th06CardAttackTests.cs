using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th06CardAttackTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public short cardId;
            public byte[] cardName;
            public ushort trialCount;
            public ushort clearCount;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "CATK",
            size1 = 0x40,
            size2 = 0x40,
            cardId = 123,
            cardName = TestUtils.MakeRandomArray<byte>(0x24),
            trialCount = 789,
            clearCount = 456
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                new byte[8],
                (short)(properties.cardId - 1),
                new byte[6],
                properties.cardName,
                properties.trialCount,
                properties.clearCount);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));

        internal static void Validate(in Th06CardAttackWrapper cardAttack, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, cardAttack.Signature);
            Assert.AreEqual(properties.size1, cardAttack.Size1);
            Assert.AreEqual(properties.size2, cardAttack.Size2);
            CollectionAssert.AreEqual(data, cardAttack.Data.ToArray());
            Assert.AreEqual(data[0], cardAttack.FirstByteOfData);
            Assert.AreEqual(properties.cardId, cardAttack.CardId);
            CollectionAssert.AreEqual(properties.cardName, cardAttack.CardName.ToArray());
            Assert.AreEqual(properties.trialCount, cardAttack.TrialCount);
            Assert.AreEqual(properties.clearCount, cardAttack.ClearCount);
        }

        [TestMethod()]
        public void Th06CardAttackTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(MakeByteArray(properties));
            var cardAttack = new Th06CardAttackWrapper(chapter);

            Validate(cardAttack, properties);
            Assert.IsTrue(cardAttack.HasTried().Value);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "cardAttack")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th06CardAttackTestNullChapter() => TestUtils.Wrap(() =>
        {
            var cardAttack = new Th06CardAttackWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "cardAttack")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06CardAttackTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(MakeByteArray(properties));
            var cardAttack = new Th06CardAttackWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "cardAttack")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06CardAttackTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.size1;

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(MakeByteArray(properties));
            var cardAttack = new Th06CardAttackWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod()]
        public void Th06CardAttackTestNotTried() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.trialCount = 0;

            var chapter = Th06ChapterWrapper<Th06Converter>.Create(MakeByteArray(properties));
            var cardAttack = new Th06CardAttackWrapper(chapter);

            Validate(cardAttack, properties);
            Assert.IsFalse(cardAttack.HasTried().Value);
        });
    }
}
