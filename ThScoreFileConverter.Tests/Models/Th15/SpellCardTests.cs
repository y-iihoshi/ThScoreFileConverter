using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverter.Tests.UnitTesting;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverter.Tests.Models.Th15;

[TestClass]
public class SpellCardTests
{
    internal static Mock<ISpellCard> MockSpellCard()
    {
        var mock = new Mock<ISpellCard>();
        _ = mock.SetupGet(m => m.Name).Returns(TestUtils.MakeRandomArray<byte>(0x80));
        _ = mock.SetupGet(m => m.ClearCount).Returns(1);
        _ = mock.SetupGet(m => m.PracticeClearCount).Returns(2);
        _ = mock.SetupGet(m => m.TrialCount).Returns(3);
        _ = mock.SetupGet(m => m.PracticeTrialCount).Returns(4);
        _ = mock.SetupGet(m => m.Id).Returns(5);
        _ = mock.SetupGet(m => m.Level).Returns(Level.Normal);
        _ = mock.SetupGet(m => m.PracticeScore).Returns(6789);
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
            TestUtils.Cast<int>(spellCard.Level),
            spellCard.PracticeScore);
    }

    internal static void Validate(ISpellCard expected, ISpellCard actual)
    {
        CollectionAssert.That.AreEqual(expected.Name, actual.Name);
        Assert.AreEqual(expected.ClearCount, actual.ClearCount);
        Assert.AreEqual(expected.PracticeClearCount, actual.PracticeClearCount);
        Assert.AreEqual(expected.TrialCount, actual.TrialCount);
        Assert.AreEqual(expected.PracticeTrialCount, actual.PracticeTrialCount);
        Assert.AreEqual(expected.Id, actual.Id);
        Assert.AreEqual(expected.Level, actual.Level);
        Assert.AreEqual(expected.PracticeScore, actual.PracticeScore);
    }

    [TestMethod]
    public void SpellCardTest()
    {
        var mock = new Mock<ISpellCard>();
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

        _ = Assert.ThrowsException<InvalidCastException>(
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
