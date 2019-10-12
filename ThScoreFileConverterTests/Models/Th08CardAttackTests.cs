using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using ThScoreFileConverterTests.Models.Th08;
using ThScoreFileConverterTests.Models.Th08.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th08CardAttackTests
    {
        internal static CardAttackStub ValidStub => new CardAttackStub()
        {
            Signature = "CATK",
            Size1 = 0x22C,
            Size2 = 0x22C,
            CardId = 123,
            Level = LevelPracticeWithTotal.Normal,
            CardName = TestUtils.MakeRandomArray<byte>(0x30),
            EnemyName = TestUtils.MakeRandomArray<byte>(0x30),
            Comment = TestUtils.MakeRandomArray<byte>(0x80),
            StoryCareer = new CardAttackCareerStub(CardAttackCareerTests.ValidStub),
            PracticeCareer = new CardAttackCareerStub(CardAttackCareerTests.ValidStub),
        };

        internal static byte[] MakeData(ICardAttack attack)
            => TestUtils.MakeByteArray(
                0u,
                (short)(attack.CardId - 1),
                (byte)0,
                (byte)attack.Level,
                attack.CardName,
                attack.EnemyName,
                attack.Comment,
                CardAttackCareerTests.MakeByteArray(attack.StoryCareer),
                CardAttackCareerTests.MakeByteArray(attack.PracticeCareer),
                0u);

        internal static byte[] MakeByteArray(ICardAttack attack)
            => TestUtils.MakeByteArray(
                attack.Signature.ToCharArray(), attack.Size1, attack.Size2, MakeData(attack));

        internal static void Validate(ICardAttack expected, in Th08CardAttackWrapper actual)
        {
            var data = MakeData(expected);

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Size1, actual.Size1);
            Assert.AreEqual(expected.Size2, actual.Size2);
            CollectionAssert.That.AreEqual(data, actual.Data);
            Assert.AreEqual(data[0], actual.FirstByteOfData);
            Assert.AreEqual(expected.CardId, actual.CardId);
            Assert.AreEqual(expected.Level, actual.Level.Value);
            CollectionAssert.That.AreEqual(expected.CardName, actual.CardName);
            CollectionAssert.That.AreEqual(expected.EnemyName, actual.EnemyName);
            CollectionAssert.That.AreEqual(expected.Comment, actual.Comment);
            CardAttackCareerTests.Validate(expected.StoryCareer, actual.StoryCareer);
            CardAttackCareerTests.Validate(expected.PracticeCareer, actual.PracticeCareer);
        }

        [TestMethod]
        public void Th08CardAttackTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            var cardAttack = new Th08CardAttackWrapper(chapter);

            Validate(stub, cardAttack);
            Assert.IsTrue(cardAttack.HasTried().Value);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08CardAttackTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new Th08CardAttackWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08CardAttackTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new CardAttackStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th08CardAttackWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08CardAttackTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var stub = new CardAttackStub(ValidStub);
            --stub.Size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th08CardAttackWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(LevelPracticeWithTotal));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th08CardAttackTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var stub = new CardAttackStub(ValidStub)
            {
                Level = TestUtils.Cast<LevelPracticeWithTotal>(level),
            };

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th08CardAttackWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th08CardAttackTestNotTried() => TestUtils.Wrap(() =>
        {
            var stub = new CardAttackStub(ValidStub)
            {
                StoryCareer = new CardAttackCareerStub(ValidStub.StoryCareer)
                {
                    TrialCounts = ValidStub.StoryCareer.TrialCounts
                        .Select(pair => (pair.Key, Value: (pair.Key == CharaWithTotal.Total) ? 0 : pair.Value))
                        .ToDictionary(pair => pair.Key, pair => pair.Value),
                },
                PracticeCareer = new CardAttackCareerStub(ValidStub.PracticeCareer)
                {
                    TrialCounts = ValidStub.PracticeCareer.TrialCounts
                        .Select(pair => (pair.Key, Value: (pair.Key == CharaWithTotal.Total) ? 0 : pair.Value))
                        .ToDictionary(pair => pair.Key, pair => pair.Value),
                },
            };

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            var cardAttack = new Th08CardAttackWrapper(chapter);

            Validate(stub, cardAttack);
            Assert.IsFalse(cardAttack.HasTried().Value);
        });
    }
}
