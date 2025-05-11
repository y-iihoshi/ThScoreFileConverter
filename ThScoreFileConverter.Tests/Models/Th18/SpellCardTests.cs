using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Models.Th18;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Tests.Models.Th18;

internal static class SpellCardExtensions
{
    internal static void ShouldBe(this ISpellCard actual, ISpellCard expected)
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
    internal static ISpellCard MockSpellCard()
    {
        var mock = Substitute.For<ISpellCard>();
        _ = mock.Name.Returns(TestUtils.MakeRandomArray(0xC0));
        _ = mock.ClearCount.Returns(1);
        _ = mock.PracticeClearCount.Returns(2);
        _ = mock.TrialCount.Returns(3);
        _ = mock.PracticeTrialCount.Returns(4);
        _ = mock.Id.Returns(5);
        _ = mock.Level.Returns(Level.Normal);
        _ = mock.PracticeScore.Returns(6789);
        return mock;
    }

    internal static byte[] MakeByteArray(ISpellCard spellCard)
    {
        return TestUtils.MakeByteArray(
            spellCard.Name,
            spellCard.ClearCount,
            spellCard.PracticeClearCount,
            spellCard.TrialCount,
            spellCard.PracticeTrialCount,
            spellCard.Id - 1,
            (int)spellCard.Level,
            spellCard.PracticeScore);
    }

    [TestMethod]
    public void SpellCardTest()
    {
        var mock = Substitute.For<ISpellCard>();
        var spellCard = new SpellCard();

        spellCard.ShouldBe(mock);
        spellCard.HasTried.ShouldBeFalse();
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = MockSpellCard();

        var spellCard = TestUtils.Create<SpellCard>(MakeByteArray(mock));

        spellCard.ShouldBe(mock);
        spellCard.HasTried.ShouldBeTrue();
    }

    [TestMethod]
    public void ReadFromTestShortenedName()
    {
        var mock = MockSpellCard();
        var name = mock.Name;
        _ = mock.Name.Returns([.. name.SkipLast(1)]);

        _ = Should.Throw<InvalidCastException>(
            () => TestUtils.Create<SpellCard>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestExceededName()
    {
        var mock = MockSpellCard();
        var name = mock.Name;
        _ = mock.Name.Returns([.. name, .. TestUtils.MakeRandomArray(1)]);

        _ = Should.Throw<InvalidCastException>(
            () => TestUtils.Create<SpellCard>(MakeByteArray(mock)));
    }

    public static IEnumerable<object[]> InvalidLevels => TestUtils.GetInvalidEnumerators<Level>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void ReadFromTestInvalidLevel(int level)
    {
        var mock = MockSpellCard();
        _ = mock.Level.Returns((Level)level);

        _ = Should.Throw<InvalidCastException>(
            () => TestUtils.Create<SpellCard>(MakeByteArray(mock)));
    }
}
