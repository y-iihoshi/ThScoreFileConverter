using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NSubstitute;
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
    internal static ICardAttack MockCardAttack()
    {
        var storyCareer = CardAttackCareerTests.MockCardAttackCareer();
        var practiceCareer = CardAttackCareerTests.MockCardAttackCareer();

        var mock = Substitute.For<ICardAttack>();
        _ = mock.Signature.Returns("CATK");
        _ = mock.Size1.Returns((short)0x22C);
        _ = mock.Size2.Returns((short)0x22C);
        _ = mock.CardId.Returns((short)123);
        _ = mock.Level.Returns(LevelPracticeWithTotal.Lunatic);
        _ = mock.CardName.Returns(TestUtils.MakeRandomArray(0x30));
        _ = mock.EnemyName.Returns(TestUtils.MakeRandomArray(0x30));
        _ = mock.Comment.Returns(TestUtils.MakeRandomArray(0x80));
        _ = mock.StoryCareer.Returns(storyCareer);
        _ = mock.PracticeCareer.Returns(practiceCareer);
        SetupHasTried(mock);

        return mock;
    }

    internal static void SetupHasTried(ICardAttack mock)
    {
        var hasStoryTried =
            mock.StoryCareer.TrialCounts.TryGetValue(CharaWithTotal.Total, out var storyTrialCount)
            && (storyTrialCount > 0);
        var hasPracticeTried =
            mock.PracticeCareer.TrialCounts.TryGetValue(CharaWithTotal.Total, out var practiceTrialCount)
            && (practiceTrialCount > 0);
        _ = mock.HasTried.Returns(hasStoryTried || hasPracticeTried);
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

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var cardAttack = new CardAttack(chapter);

        Validate(mock, cardAttack);
        Assert.IsTrue(cardAttack.HasTried);
    }

    [TestMethod]
    public void CardAttackTestInvalidSignature()
    {
        var mock = MockCardAttack();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new CardAttack(chapter));
    }

    [TestMethod]
    public void CardAttackTestInvalidSize1()
    {
        var mock = MockCardAttack();
        var size = mock.Size1;
        _ = mock.Size1.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new CardAttack(chapter));
    }

    public static IEnumerable<object[]> InvalidLevels => TestUtils.GetInvalidEnumerators<LevelPracticeWithTotal>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void CardAttackTestInvalidLevel(int level)
    {
        var mock = MockCardAttack();
        _ = mock.Level.Returns((LevelPracticeWithTotal)level);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidCastException>(() => new CardAttack(chapter));
    }

    [TestMethod]
    public void CardAttackTestNotTried()
    {
        var storyCareerMock = CardAttackCareerTests.MockCardAttackCareer();
        var trialCounts = storyCareerMock.TrialCounts
            .Select(pair => (pair.Key, pair.Key == CharaWithTotal.Total ? 0 : pair.Value)).ToDictionary();
        _ = storyCareerMock.TrialCounts.Returns(trialCounts);

        var practiceCareerMock = CardAttackCareerTests.MockCardAttackCareer();
        _ = practiceCareerMock.TrialCounts.Returns(trialCounts);

        var mock = MockCardAttack();
        _ = mock.StoryCareer.Returns(storyCareerMock);
        _ = mock.PracticeCareer.Returns(practiceCareerMock);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var cardAttack = new CardAttack(chapter);

        Validate(mock, cardAttack);
        Assert.IsFalse(cardAttack.HasTried);
    }
}
