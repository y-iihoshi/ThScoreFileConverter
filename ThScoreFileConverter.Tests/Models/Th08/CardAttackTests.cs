using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverter.Tests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class CardAttackTests
{
    internal static Mock<ICardAttack> MockCardAttack()
    {
        var mock = new Mock<ICardAttack>();
        _ = mock.SetupGet(m => m.Signature).Returns("CATK");
        _ = mock.SetupGet(m => m.Size1).Returns(0x22C);
        _ = mock.SetupGet(m => m.Size2).Returns(0x22C);
        _ = mock.SetupGet(m => m.CardId).Returns(123);
        _ = mock.SetupGet(m => m.Level).Returns(LevelPracticeWithTotal.Lunatic);
        _ = mock.SetupGet(m => m.CardName).Returns(TestUtils.MakeRandomArray<byte>(0x30));
        _ = mock.SetupGet(m => m.EnemyName).Returns(TestUtils.MakeRandomArray<byte>(0x30));
        _ = mock.SetupGet(m => m.Comment).Returns(TestUtils.MakeRandomArray<byte>(0x80));
        _ = mock.SetupGet(m => m.StoryCareer).Returns(CardAttackCareerTests.MockCardAttackCareer().Object);
        _ = mock.SetupGet(m => m.PracticeCareer).Returns(CardAttackCareerTests.MockCardAttackCareer().Object);
        SetupHasTried(mock);

        return mock;
    }

    internal static void SetupHasTried(Mock<ICardAttack> mock)
    {
        var hasStoryTried =
            mock.Object.StoryCareer.TrialCounts.TryGetValue(CharaWithTotal.Total, out var storyTrialCount)
            && (storyTrialCount > 0);
        var hasPracticeTried =
            mock.Object.PracticeCareer.TrialCounts.TryGetValue(CharaWithTotal.Total, out var practiceTrialCount)
            && (practiceTrialCount > 0);
        _ = mock.SetupGet(m => m.HasTried).Returns(hasStoryTried || hasPracticeTried);
    }

    internal static byte[] MakeByteArray(ICardAttack attack)
    {
        return TestUtils.MakeByteArray(
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
    }

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
        var mock = MockCardAttack();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        var cardAttack = new CardAttack(chapter);

        Validate(mock.Object, cardAttack);
        Assert.IsTrue(cardAttack.HasTried);
    }

    [TestMethod]
    public void CardAttackTestInvalidSignature()
    {
        var mock = MockCardAttack();
        var signature = mock.Object.Signature;
        _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        _ = Assert.ThrowsException<InvalidDataException>(() => new CardAttack(chapter));
    }

    [TestMethod]
    public void CardAttackTestInvalidSize1()
    {
        var mock = MockCardAttack();
        var size = mock.Object.Size1;
        _ = mock.SetupGet(m => m.Size1).Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        _ = Assert.ThrowsException<InvalidDataException>(() => new CardAttack(chapter));
    }

    public static IEnumerable<object[]> InvalidLevels => TestUtils.GetInvalidEnumerators<LevelPracticeWithTotal>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void CardAttackTestInvalidLevel(int level)
    {
        var mock = MockCardAttack();
        _ = mock.SetupGet(m => m.Level).Returns((LevelPracticeWithTotal)level);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        _ = Assert.ThrowsException<InvalidCastException>(() => new CardAttack(chapter));
    }

    [TestMethod]
    public void CardAttackTestNotTried()
    {
        var storyCareerMock = CardAttackCareerTests.MockCardAttackCareer();
        var trialCounts = storyCareerMock.Object.TrialCounts
            .Select(pair => (pair.Key, pair.Key == CharaWithTotal.Total ? 0 : pair.Value)).ToDictionary();
        _ = storyCareerMock.SetupGet(m => m.TrialCounts).Returns(trialCounts);

        var practiceCareerMock = CardAttackCareerTests.MockCardAttackCareer();
        _ = practiceCareerMock.SetupGet(m => m.TrialCounts).Returns(trialCounts);

        var mock = MockCardAttack();
        _ = mock.SetupGet(m => m.StoryCareer).Returns(storyCareerMock.Object);
        _ = mock.SetupGet(m => m.PracticeCareer).Returns(practiceCareerMock.Object);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        var cardAttack = new CardAttack(chapter);

        Validate(mock.Object, cardAttack);
        Assert.IsFalse(cardAttack.HasTried);
    }
}
