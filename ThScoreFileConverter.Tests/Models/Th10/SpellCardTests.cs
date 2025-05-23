﻿using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Models.Th10;

namespace ThScoreFileConverter.Tests.Models.Th10;

internal static class SpellCardExtensions
{
    internal static void ShouldBe(this ISpellCard<Level> actual, ISpellCard<Level> expected)
    {
        actual.Name.ShouldBe(expected.Name);
        actual.ClearCount.ShouldBe(expected.ClearCount);
        actual.TrialCount.ShouldBe(expected.TrialCount);
        actual.Id.ShouldBe(expected.Id);
        actual.Level.ShouldBe(expected.Level);
    }
}

[TestClass]
public class SpellCardTests
{
    internal static ISpellCard<Level> MockSpellCard()
    {
        var mock = Substitute.For<ISpellCard<Level>>();
        _ = mock.Name.Returns(TestUtils.MakeRandomArray(0x80));
        _ = mock.ClearCount.Returns(123);
        _ = mock.TrialCount.Returns(456);
        _ = mock.Id.Returns(789);
        _ = mock.Level.Returns(Level.Normal);
        return mock;
    }

    internal static byte[] MakeByteArray(ISpellCard<Level> spellCard)
    {
        return TestUtils.MakeByteArray(
            spellCard.Name,
            spellCard.ClearCount,
            spellCard.TrialCount,
            spellCard.Id - 1,
            (int)spellCard.Level);
    }

    [TestMethod]
    public void SpellCardTest()
    {
        var mock = Substitute.For<ISpellCard<Level>>();
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

        _ = Should.Throw<EndOfStreamException>(
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
