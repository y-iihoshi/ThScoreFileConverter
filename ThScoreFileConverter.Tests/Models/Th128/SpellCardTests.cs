﻿using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Models.Th128;

namespace ThScoreFileConverter.Tests.Models.Th128;

[TestClass]
public class SpellCardTests
{
    internal static ISpellCard MockInitialSpellCard()
    {
        var mock = Substitute.For<ISpellCard>();
        _ = mock.Name.Returns([]);
        return mock;
    }

    internal static ISpellCard MockSpellCard()
    {
        var mock = Substitute.For<ISpellCard>();
        _ = mock.Name.Returns(TestUtils.MakeRandomArray(0x80));
        _ = mock.NoMissCount.Returns(12);
        _ = mock.NoIceCount.Returns(34);
        _ = mock.TrialCount.Returns(56);
        _ = mock.Id.Returns(78);
        _ = mock.Level.Returns(Level.Normal);
        _ = mock.HasTried.Returns(true);
        return mock;
    }

    internal static byte[] MakeByteArray(ISpellCard spellCard)
    {
        return TestUtils.MakeByteArray(
            spellCard.Name,
            spellCard.NoMissCount,
            spellCard.NoIceCount,
            0u,
            spellCard.TrialCount,
            spellCard.Id - 1,
            (int)spellCard.Level);
    }

    internal static void Validate(ISpellCard expected, ISpellCard actual)
    {
        actual.Name.ShouldBe(expected.Name);
        actual.NoMissCount.ShouldBe(expected.NoMissCount);
        actual.NoIceCount.ShouldBe(expected.NoIceCount);
        actual.TrialCount.ShouldBe(expected.TrialCount);
        actual.Id.ShouldBe(expected.Id);
        actual.Level.ShouldBe(expected.Level);
    }

    [TestMethod]
    public void SpellCardTest()
    {
        var mock = MockInitialSpellCard();
        var spellCard = new SpellCard();

        Validate(mock, spellCard);
        spellCard.HasTried.ShouldBeFalse();
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = MockSpellCard();

        var spellCard = TestUtils.Create<SpellCard>(MakeByteArray(mock));

        Validate(mock, spellCard);
        spellCard.HasTried.ShouldBeTrue();
    }

    [TestMethod]
    public void ReadFromTestShortenedName()
    {
        var mock = MockSpellCard();
        var name = mock.Name;
        _ = mock.Name.Returns(name.SkipLast(1).ToArray());

        _ = Should.Throw<EndOfStreamException>(
            () => TestUtils.Create<SpellCard>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestExceededName()
    {
        var mock = MockSpellCard();
        var name = mock.Name;
        _ = mock.Name.Returns(name.Concat(TestUtils.MakeRandomArray(1)).ToArray());

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
