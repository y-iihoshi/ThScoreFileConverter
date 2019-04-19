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
    public class Th07CardAttackTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public Dictionary<Th07Converter.CharaWithTotal, uint> maxBonuses;
            public short cardId;
            public byte[] cardName;
            public Dictionary<Th07Converter.CharaWithTotal, ushort> trialCounts;
            public Dictionary<Th07Converter.CharaWithTotal, ushort> clearCounts;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "CATK",
            size1 = 0x78,
            size2 = 0x78,
            maxBonuses = Utils.GetEnumerator<Th07Converter.CharaWithTotal>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => (uint)pair.index),
            cardId = 123,
            cardName = TestUtils.MakeRandomArray<byte>(0x30),
            trialCounts = Utils.GetEnumerator<Th07Converter.CharaWithTotal>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => (ushort)(10 + pair.index)),
            clearCounts = Utils.GetEnumerator<Th07Converter.CharaWithTotal>()
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

        internal static void Validate(in Th07CardAttackWrapper cardAttack, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, cardAttack.Signature);
            Assert.AreEqual(properties.size1, cardAttack.Size1);
            Assert.AreEqual(properties.size2, cardAttack.Size2);
            CollectionAssert.AreEqual(data, cardAttack.Data.ToArray());
            Assert.AreEqual(data[0], cardAttack.FirstByteOfData);
            CollectionAssert.AreEqual(properties.maxBonuses.Values, cardAttack.MaxBonuses.Values.ToArray());
            Assert.AreEqual(properties.cardId, cardAttack.CardId);
            CollectionAssert.AreEqual(properties.cardName, cardAttack.CardName.ToArray());
            CollectionAssert.AreEqual(properties.trialCounts.Values, cardAttack.TrialCounts.Values.ToArray());
            CollectionAssert.AreEqual(properties.clearCounts.Values, cardAttack.ClearCounts.Values.ToArray());
        }

        [TestMethod]
        public void Th07CardAttackTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = Th06ChapterWrapper<Th07Converter>.Create(MakeByteArray(properties));
            var cardAttack = new Th07CardAttackWrapper(chapter);

            Validate(cardAttack, properties);
            Assert.IsTrue(cardAttack.HasTried().Value);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "cardAttack")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07CardAttackTestNullChapter() => TestUtils.Wrap(() =>
        {
            var cardAttack = new Th07CardAttackWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "cardAttack")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07CardAttackTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant(); 

            var chapter = Th06ChapterWrapper<Th07Converter>.Create(MakeByteArray(properties));
            var cardAttack = new Th07CardAttackWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "cardAttack")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07CardAttackTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = Th06ChapterWrapper<Th07Converter>.Create(MakeByteArray(properties));
            var cardAttack = new Th07CardAttackWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th07CardAttackTestNotTried() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.trialCounts[Th07Converter.CharaWithTotal.Total] = 0;

            var chapter = Th06ChapterWrapper<Th07Converter>.Create(MakeByteArray(properties));
            var cardAttack = new Th07CardAttackWrapper(chapter);

            Validate(cardAttack, properties);
            Assert.IsFalse(cardAttack.HasTried().Value);
        });
    }
}
