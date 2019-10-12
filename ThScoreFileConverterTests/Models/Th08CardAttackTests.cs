using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using ThScoreFileConverterTests.Models.Th08.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th08CardAttackTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public short cardId;
            public Th08Converter.LevelPracticeWithTotal level;
            public byte[] cardName;
            public byte[] enemyName;
            public byte[] comment;
            public CardAttackCareerStub storyCareer;
            public CardAttackCareerStub practiceCareer;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "CATK",
            size1 = 0x22C,
            size2 = 0x22C,
            cardId = 123,
            level = Th08Converter.LevelPracticeWithTotal.Normal,
            cardName = TestUtils.MakeRandomArray<byte>(0x30),
            enemyName = TestUtils.MakeRandomArray<byte>(0x30),
            comment = TestUtils.MakeRandomArray<byte>(0x80),
            storyCareer = new CardAttackCareerStub(Th08CardAttackCareerTests.ValidStub),
            practiceCareer = new CardAttackCareerStub(Th08CardAttackCareerTests.ValidStub),
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                0u,
                (short)(properties.cardId - 1),
                (byte)0,
                (byte)properties.level,
                properties.cardName,
                properties.enemyName,
                properties.comment,
                Th08CardAttackCareerTests.MakeByteArray(properties.storyCareer),
                Th08CardAttackCareerTests.MakeByteArray(properties.practiceCareer),
                0u);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));

        internal static void Validate(in Th08CardAttackWrapper cardAttack, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, cardAttack.Signature);
            Assert.AreEqual(properties.size1, cardAttack.Size1);
            Assert.AreEqual(properties.size2, cardAttack.Size2);
            CollectionAssert.That.AreEqual(data, cardAttack.Data);
            Assert.AreEqual(data[0], cardAttack.FirstByteOfData);
            Assert.AreEqual(properties.cardId, cardAttack.CardId);
            Assert.AreEqual(properties.level, cardAttack.Level.Value);
            CollectionAssert.That.AreEqual(properties.cardName, cardAttack.CardName);
            CollectionAssert.That.AreEqual(properties.enemyName, cardAttack.EnemyName);
            CollectionAssert.That.AreEqual(properties.comment, cardAttack.Comment);
            Th08CardAttackCareerTests.Validate(properties.storyCareer, cardAttack.StoryCareer);
            Th08CardAttackCareerTests.Validate(properties.practiceCareer, cardAttack.PracticeCareer);
        }

        [TestMethod]
        public void Th08CardAttackTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var cardAttack = new Th08CardAttackWrapper(chapter);

            Validate(cardAttack, properties);
            Assert.IsTrue(cardAttack.HasTried().Value);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "cardAttack")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08CardAttackTestNullChapter() => TestUtils.Wrap(() =>
        {
            var cardAttack = new Th08CardAttackWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "cardAttack")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08CardAttackTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var cardAttack = new Th08CardAttackWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "cardAttack")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08CardAttackTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var cardAttack = new Th08CardAttackWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Th08Converter.LevelPracticeWithTotal));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "cardAttack")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th08CardAttackTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.level = TestUtils.Cast<Th08Converter.LevelPracticeWithTotal>(level);

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var cardAttack = new Th08CardAttackWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th08CardAttackTestNotTried() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.storyCareer.TrialCounts = properties.storyCareer.TrialCounts
                .Select(pair => (pair.Key, Value: (pair.Key == Th08Converter.CharaWithTotal.Total) ? 0 : pair.Value))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            properties.practiceCareer.TrialCounts = properties.practiceCareer.TrialCounts
                .Select(pair => (pair.Key, Value: (pair.Key == Th08Converter.CharaWithTotal.Total) ? 0 : pair.Value))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var cardAttack = new Th08CardAttackWrapper(chapter);

            Validate(cardAttack, properties);
            Assert.IsFalse(cardAttack.HasTried().Value);
        });
    }
}
