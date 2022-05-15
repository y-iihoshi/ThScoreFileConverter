using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverterTests.UnitTesting;

namespace ThScoreFileConverterTests.Models.Th128;

[TestClass]
public class SpellCardTests
{
    internal static Mock<ISpellCard> MockInitialSpellCard()
    {
        var mock = new Mock<ISpellCard>();
        _ = mock.SetupGet(m => m.Name).Returns(Enumerable.Empty<byte>());
        return mock;
    }

    internal static Mock<ISpellCard> MockSpellCard()
    {
        var mock = new Mock<ISpellCard>();
        _ = mock.SetupGet(m => m.Name).Returns(TestUtils.MakeRandomArray<byte>(0x80));
        _ = mock.SetupGet(m => m.NoMissCount).Returns(12);
        _ = mock.SetupGet(m => m.NoIceCount).Returns(34);
        _ = mock.SetupGet(m => m.TrialCount).Returns(56);
        _ = mock.SetupGet(m => m.Id).Returns(78);
        _ = mock.SetupGet(m => m.Level).Returns(Level.Normal);
        _ = mock.SetupGet(m => m.HasTried).Returns(true);
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
        CollectionAssert.That.AreEqual(expected.Name, actual.Name);
        Assert.AreEqual(expected.NoMissCount, actual.NoMissCount);
        Assert.AreEqual(expected.NoIceCount, actual.NoIceCount);
        Assert.AreEqual(expected.TrialCount, actual.TrialCount);
        Assert.AreEqual(expected.Id, actual.Id);
        Assert.AreEqual(expected.Level, actual.Level);
    }

    [TestMethod]
    public void SpellCardTest()
    {
        var mock = MockInitialSpellCard();
        var spellCard = new SpellCard();

        Validate(mock.Object, spellCard);
        Assert.IsFalse(spellCard.HasTried);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = MockSpellCard();

        var spellCard = TestUtils.Create<SpellCard>(MakeByteArray(mock.Object));

        Validate(mock.Object, spellCard);
        Assert.IsTrue(spellCard.HasTried);
    }

    [TestMethod]
    public void ReadFromTestShortenedName()
    {
        var mock = MockSpellCard();
        var name = mock.Object.Name;
        _ = mock.SetupGet(m => m.Name).Returns(name.SkipLast(1).ToArray());

        _ = Assert.ThrowsException<EndOfStreamException>(
            () => TestUtils.Create<SpellCard>(MakeByteArray(mock.Object)));
    }

    [TestMethod]
    public void ReadFromTestExceededName()
    {
        var mock = MockSpellCard();
        var name = mock.Object.Name;
        _ = mock.SetupGet(m => m.Name).Returns(name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray());

        _ = Assert.ThrowsException<InvalidCastException>(
            () => TestUtils.Create<SpellCard>(MakeByteArray(mock.Object)));
    }

    public static IEnumerable<object[]> InvalidLevels
        => TestUtils.GetInvalidEnumerators(typeof(Level));

    [DataTestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void ReadFromTestInvalidLevel(int level)
    {
        var mock = MockSpellCard();
        _ = mock.SetupGet(m => m.Level).Returns(TestUtils.Cast<Level>(level));

        _ = Assert.ThrowsException<InvalidCastException>(
            () => TestUtils.Create<SpellCard>(MakeByteArray(mock.Object)));
    }
}
