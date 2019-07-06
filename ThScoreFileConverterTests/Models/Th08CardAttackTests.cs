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
            public Th08CardAttackCareerTests.Properties storyCareer;
            public Th08CardAttackCareerTests.Properties practiceCareer;
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
            storyCareer = new Th08CardAttackCareerTests.Properties(Th08CardAttackCareerTests.ValidProperties),
            practiceCareer = new Th08CardAttackCareerTests.Properties(Th08CardAttackCareerTests.ValidProperties),
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
            CollectionAssert.AreEqual(data, cardAttack.Data.ToArray());
            Assert.AreEqual(data[0], cardAttack.FirstByteOfData);
            Assert.AreEqual(properties.cardId, cardAttack.CardId);
            Assert.AreEqual(properties.level, cardAttack.Level.Value);
            CollectionAssert.AreEqual(properties.cardName, cardAttack.CardName.ToArray());
            CollectionAssert.AreEqual(properties.enemyName, cardAttack.EnemyName.ToArray());
            CollectionAssert.AreEqual(properties.comment, cardAttack.Comment.ToArray());
            Th08CardAttackCareerTests.Validate(cardAttack.StoryCareer, properties.storyCareer);
            Th08CardAttackCareerTests.Validate(cardAttack.PracticeCareer, properties.practiceCareer);
        }

        [TestMethod]
        public void Th08CardAttackTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = Th06ChapterWrapper.Create(MakeByteArray(properties));
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

            var chapter = Th06ChapterWrapper.Create(MakeByteArray(properties));
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

            var chapter = Th06ChapterWrapper.Create(MakeByteArray(properties));
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

            var chapter = Th06ChapterWrapper.Create(MakeByteArray(properties));
            var cardAttack = new Th08CardAttackWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th08CardAttackTestNotTried() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.storyCareer.trialCounts[Th08Converter.CharaWithTotal.Total] = 0;
            properties.practiceCareer.trialCounts[Th08Converter.CharaWithTotal.Total] = 0;

            var chapter = Th06ChapterWrapper.Create(MakeByteArray(properties));
            var cardAttack = new Th08CardAttackWrapper(chapter);

            Validate(cardAttack, properties);
            Assert.IsFalse(cardAttack.HasTried().Value);
        });
    }
}
