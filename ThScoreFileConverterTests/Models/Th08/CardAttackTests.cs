using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th08.Stubs;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class CardAttackTests
    {
        internal static CardAttackStub ValidStub { get; } = new CardAttackStub()
        {
            Signature = "CATK",
            Size1 = 0x22C,
            Size2 = 0x22C,
            CardId = 123,
            Level = LevelPracticeWithTotal.Lunatic,
            CardName = TestUtils.MakeRandomArray<byte>(0x30),
            EnemyName = TestUtils.MakeRandomArray<byte>(0x30),
            Comment = TestUtils.MakeRandomArray<byte>(0x80),
            StoryCareer = new CardAttackCareerStub(CardAttackCareerTests.ValidStub),
            PracticeCareer = new CardAttackCareerStub(CardAttackCareerTests.ValidStub),
        };

        internal static byte[] MakeByteArray(ICardAttack attack)
            => TestUtils.MakeByteArray(
                attack.Signature.ToCharArray(),
                attack.Size1,
                attack.Size2,
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

        internal static void Validate(ICardAttack expected, ICardAttack actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Size1, actual.Size1);
            Assert.AreEqual(expected.Size2, actual.Size2);
            Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
            Assert.AreEqual(expected.CardId, actual.CardId);
            Assert.AreEqual(expected.Level, actual.Level);
            CollectionAssert.That.AreEqual(expected.CardName, actual.CardName);
            CollectionAssert.That.AreEqual(expected.EnemyName, actual.EnemyName);
            CollectionAssert.That.AreEqual(expected.Comment, actual.Comment);
            CardAttackCareerTests.Validate(expected.StoryCareer, actual.StoryCareer);
            CardAttackCareerTests.Validate(expected.PracticeCareer, actual.PracticeCareer);
        }

        [TestMethod]
        public void CardAttackTestChapter()
        {
            var stub = ValidStub;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var cardAttack = new CardAttack(chapter);

            Validate(stub, cardAttack);
            Assert.IsTrue(cardAttack.HasTried());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CardAttackTestNullChapter()
        {
            _ = new CardAttack(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CardAttackTestInvalidSignature()
        {
            var stub = new CardAttackStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new CardAttack(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CardAttackTestInvalidSize1()
        {
            var stub = new CardAttackStub(ValidStub);
            --stub.Size1;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new CardAttack(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(LevelPracticeWithTotal));

        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void CardAttackTestInvalidLevel(int level)
        {
            var stub = new CardAttackStub(ValidStub)
            {
                Level = TestUtils.Cast<LevelPracticeWithTotal>(level),
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new CardAttack(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CardAttackTestNotTried()
        {
            var stub = new CardAttackStub(ValidStub)
            {
                StoryCareer = new CardAttackCareerStub(ValidStub.StoryCareer)
                {
                    TrialCounts = ValidStub.StoryCareer.TrialCounts
                        .Select(pair => (pair.Key, Value: pair.Key == CharaWithTotal.Total ? 0 : pair.Value))
                        .ToDictionary(pair => pair.Key, pair => pair.Value),
                },
                PracticeCareer = new CardAttackCareerStub(ValidStub.PracticeCareer)
                {
                    TrialCounts = ValidStub.PracticeCareer.TrialCounts
                        .Select(pair => (pair.Key, Value: pair.Key == CharaWithTotal.Total ? 0 : pair.Value))
                        .ToDictionary(pair => pair.Key, pair => pair.Value),
                },
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var cardAttack = new CardAttack(chapter);

            Validate(stub, cardAttack);
            Assert.IsFalse(cardAttack.HasTried());
        }
    }
}
