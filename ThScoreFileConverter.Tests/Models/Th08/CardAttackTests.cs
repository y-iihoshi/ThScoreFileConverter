using NSubstitute;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Models.Th08;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th08;

internal static class CardAttackExtensions
{
    internal static void ShouldBe(this ICardAttack actual, ICardAttack expected)
    {
        actual.Signature.ShouldBe(expected.Signature);
        actual.Size1.ShouldBe(expected.Size1);
        actual.Size2.ShouldBe(expected.Size2);
        actual.FirstByteOfData.ShouldBe(expected.FirstByteOfData);
        actual.CardId.ShouldBe(expected.CardId);
        actual.Level.ShouldBe(expected.Level);
        actual.CardName.ShouldBe(expected.CardName);
        actual.EnemyName.ShouldBe(expected.EnemyName);
        actual.Comment.ShouldBe(expected.Comment);
        actual.StoryCareer.ShouldBe(expected.StoryCareer);
        actual.PracticeCareer.ShouldBe(expected.PracticeCareer);
    }
}

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

    [TestMethod]
    public void CardAttackTestChapter()
    {
        var mock = MockCardAttack();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var cardAttack = new CardAttack(chapter);

        cardAttack.ShouldBe(mock);
        cardAttack.HasTried.ShouldBeTrue();
    }

    [TestMethod]
    public void CardAttackTestInvalidSignature()
    {
        var mock = MockCardAttack();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new CardAttack(chapter));
    }

    [TestMethod]
    public void CardAttackTestInvalidSize1()
    {
        var mock = MockCardAttack();
        var size = mock.Size1;
        _ = mock.Size1.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new CardAttack(chapter));
    }

    public static IEnumerable<object[]> InvalidLevels => TestUtils.GetInvalidEnumerators<LevelPracticeWithTotal>();

    [TestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void CardAttackTestInvalidLevel(int level)
    {
        var mock = MockCardAttack();
        _ = mock.Level.Returns((LevelPracticeWithTotal)level);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidCastException>(() => new CardAttack(chapter));
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

        cardAttack.ShouldBe(mock);
        cardAttack.HasTried.ShouldBeFalse();
    }
}
