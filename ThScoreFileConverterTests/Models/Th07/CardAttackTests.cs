using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class CardAttackTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public Dictionary<CharaWithTotal, uint> maxBonuses;
            public short cardId;
            public byte[] cardName;
            public Dictionary<CharaWithTotal, ushort> trialCounts;
            public Dictionary<CharaWithTotal, ushort> clearCounts;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "CATK",
            size1 = 0x78,
            size2 = 0x78,
            maxBonuses = Utils.GetEnumerator<CharaWithTotal>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => (uint)pair.index),
            cardId = 123,
            cardName = TestUtils.MakeRandomArray<byte>(0x30),
            trialCounts = Utils.GetEnumerator<CharaWithTotal>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => (ushort)(10 + pair.index)),
            clearCounts = Utils.GetEnumerator<CharaWithTotal>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => (ushort)(10 - pair.index))
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                0u,
                properties.maxBonuses.Values.ToArray(),
                (short)(properties.cardId - 1),
                (byte)0,
                properties.cardName,
                (byte)0,
                properties.trialCounts.Values.ToArray(),
                properties.clearCounts.Values.ToArray());

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));

        internal static void Validate(in CardAttack cardAttack, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, cardAttack.Signature);
            Assert.AreEqual(properties.size1, cardAttack.Size1);
            Assert.AreEqual(properties.size2, cardAttack.Size2);
            Assert.AreEqual(data[0], cardAttack.FirstByteOfData);
            CollectionAssert.AreEqual(properties.maxBonuses.Values, cardAttack.MaxBonuses.Values.ToArray());
            Assert.AreEqual(properties.cardId, cardAttack.CardId);
            CollectionAssert.AreEqual(properties.cardName, cardAttack.CardName.ToArray());
            CollectionAssert.AreEqual(properties.trialCounts.Values, cardAttack.TrialCounts.Values.ToArray());
            CollectionAssert.AreEqual(properties.clearCounts.Values, cardAttack.ClearCounts.Values.ToArray());
        }

        [TestMethod]
        public void CardAttackTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var cardAttack = new CardAttack(chapter.Target);

            Validate(cardAttack, properties);
            Assert.IsTrue(cardAttack.HasTried());
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CardAttackTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new CardAttack(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CardAttackTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            _ = new CardAttack(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CardAttackTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            _ = new CardAttack(chapter.Target);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void CardAttackTestNotTried() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.trialCounts[CharaWithTotal.Total] = 0;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var cardAttack = new CardAttack(chapter.Target);
            Validate(cardAttack, properties);
            Assert.IsFalse(cardAttack.HasTried());
        });
    }
}
