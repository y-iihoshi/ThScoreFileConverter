using NSubstitute;
using ThScoreFileConverter.Core.Models.Th13;
using ThScoreFileConverter.Models.Th13;

namespace ThScoreFileConverter.Tests.Models.Th13;

internal static class SpellCardExtensions
{
    internal static void ShouldBe<TLevel>(this ISpellCard<TLevel> actual, ISpellCard<TLevel> expected)
        where TLevel : struct, Enum
    {
        actual.Name.ShouldBe(expected.Name);
        actual.ClearCount.ShouldBe(expected.ClearCount);
        actual.PracticeClearCount.ShouldBe(expected.PracticeClearCount);
        actual.TrialCount.ShouldBe(expected.TrialCount);
        actual.PracticeTrialCount.ShouldBe(expected.PracticeTrialCount);
        actual.Id.ShouldBe(expected.Id);
        actual.Level.ShouldBe(expected.Level);
        actual.PracticeScore.ShouldBe(expected.PracticeScore);
    }
}

[TestClass]
public class SpellCardTests
{
    internal static ISpellCard<TLevel> MockSpellCard<TLevel>()
        where TLevel : struct, Enum
    {
        var mock = Substitute.For<ISpellCard<TLevel>>();
        _ = mock.Name.Returns(TestUtils.MakeRandomArray(0x80));
        _ = mock.ClearCount.Returns(1);
        _ = mock.PracticeClearCount.Returns(2);
        _ = mock.TrialCount.Returns(3);
        _ = mock.PracticeTrialCount.Returns(4);
        _ = mock.Id.Returns(5);
        _ = mock.Level.Returns(TestUtils.Cast<TLevel>(1));
        _ = mock.PracticeScore.Returns(6789);
        return mock;
    }

    internal static byte[] MakeByteArray<TLevel>(ISpellCard<TLevel> spellCard)
        where TLevel : struct, Enum
    {
        return TestUtils.MakeByteArray(
            spellCard.Name,
            spellCard.ClearCount,
            spellCard.PracticeClearCount,
            spellCard.TrialCount,
            spellCard.PracticeTrialCount,
            spellCard.Id - 1,
            TestUtils.Cast<int>(spellCard.Level),
            spellCard.PracticeScore);
    }

    internal static void SpellCardTestHelper<TLevel>()
        where TLevel : struct, Enum
    {
        var mock = Substitute.For<ISpellCard<TLevel>>();
        var spellCard = new SpellCard<TLevel>();

        spellCard.ShouldBe(mock);
        spellCard.HasTried.ShouldBeFalse();
    }

    internal static void ReadFromTestHelper<TLevel>()
        where TLevel : struct, Enum
    {
        var mock = MockSpellCard<TLevel>();

        var spellCard = TestUtils.Create<SpellCard<TLevel>>(MakeByteArray(mock));

        spellCard.ShouldBe(mock);
        spellCard.HasTried.ShouldBeTrue();
    }

    internal static void ReadFromTestShortenedNameHelper<TLevel>()
        where TLevel : struct, Enum
    {
        var mock = MockSpellCard<TLevel>();
        var name = mock.Name;
        _ = mock.Name.Returns([.. name.SkipLast(1)]);

        _ = Should.Throw<InvalidCastException>(
            () => TestUtils.Create<SpellCard<TLevel>>(MakeByteArray(mock)));
    }

    internal static void ReadFromTestExceededNameHelper<TLevel>()
        where TLevel : struct, Enum
    {
        var mock = MockSpellCard<TLevel>();
        var name = mock.Name;
        _ = mock.Name.Returns([.. name, .. TestUtils.MakeRandomArray(1)]);

        _ = Should.Throw<InvalidCastException>(
            () => TestUtils.Create<SpellCard<TLevel>>(MakeByteArray(mock)));
    }

    internal static void ReadFromTestInvalidLevelHelper<TLevel>(int level)
        where TLevel : struct, Enum
    {
        var mock = MockSpellCard<TLevel>();
        _ = mock.Level.Returns(TestUtils.Cast<TLevel>(level));

        _ = Should.Throw<InvalidCastException>(
            () => TestUtils.Create<SpellCard<TLevel>>(MakeByteArray(mock)));
    }

    public static IEnumerable<object[]> InvalidLevelPractices => TestUtils.GetInvalidEnumerators<LevelPractice>();

    [TestMethod]
    public void SpellCardTest()
    {
        SpellCardTestHelper<LevelPractice>();
    }

    [TestMethod]
    public void ReadFromTest()
    {
        ReadFromTestHelper<LevelPractice>();
    }

    [TestMethod]
    public void ReadFromTestShortenedName()
    {
        ReadFromTestShortenedNameHelper<LevelPractice>();
    }

    [TestMethod]
    public void ReadFromTestExceededName()
    {
        ReadFromTestExceededNameHelper<LevelPractice>();
    }

    [TestMethod]
    [DynamicData(nameof(InvalidLevelPractices))]
    public void ReadFromTestInvalidLevel(int level)
    {
        ReadFromTestInvalidLevelHelper<LevelPractice>(level);
    }
}
