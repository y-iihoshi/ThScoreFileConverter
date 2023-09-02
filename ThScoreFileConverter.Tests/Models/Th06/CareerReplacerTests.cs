using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th06;

[TestClass]
public class CareerReplacerTests
{
    private static IEnumerable<ICardAttack> CreateCardAttacks()
    {
        var mock1 = CardAttackTests.MockCardAttack();
        var cardId = mock1.CardId;
        var clearCount = mock1.ClearCount;
        var trialCount = mock1.TrialCount;

        var mock2 = CardAttackTests.MockCardAttack();
        _ = mock2.CardId.Returns(++cardId);
        _ = mock2.CardName.Returns(TestUtils.MakeRandomArray(0x24));
        _ = mock2.ClearCount.Returns((ushort)(clearCount + 2));
        _ = mock2.TrialCount.Returns((ushort)(trialCount + 3));

        return new[] { mock1, mock2 };
    }

    internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
        CreateCardAttacks().ToDictionary(attack => (int)attack.CardId);

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<int>()).Returns(callInfo => $"invoked: {(int)callInfo[0]}");
        return mock;
    }

    [TestMethod]
    public void CareerReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CareerReplacerTestEmpty()
    {
        var formatterMock = MockNumberFormatter();
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var replacer = new CareerReplacer(cardAttacks, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 456", replacer.Replace("%T06C231"));
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 789", replacer.Replace("%T06C232"));
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 914", replacer.Replace("%T06C001"));
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);

        Assert.AreEqual("invoked: 1581", replacer.Replace("%T06C002"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T06C011"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T06C012"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T06X231", replacer.Replace("%T06X231"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T06C651", replacer.Replace("%T06C651"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T06C233", replacer.Replace("%T06C233"));
    }
}
