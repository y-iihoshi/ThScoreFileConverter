using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverter.Tests.UnitTesting;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverter.Tests.Models.Th07;

[TestClass]
public class CareerReplacerTests
{
    private static IEnumerable<ICardAttack> CreateCardAttacks()
    {
        var mock1 = CardAttackTests.MockCardAttack();

        var mock2 = CardAttackTests.MockCardAttack();
        _ = mock2.MaxBonuses.Returns(_ => mock1.MaxBonuses.ToDictionary(pair => pair.Key, pair => pair.Value * 1000));
        _ = mock2.CardId.Returns(_ => (short)(mock1.CardId + 1));
        _ = mock2.CardName.Returns(TestUtils.MakeRandomArray(0x30));
        _ = mock2.TrialCounts.Returns(_ => mock1.TrialCounts.ToDictionary(pair => pair.Key, pair => (ushort)(pair.Value * 3)));
        _ = mock2.ClearCounts.Returns(_ => mock1.ClearCounts.ToDictionary(pair => pair.Key, pair => (ushort)(pair.Value * 2)));

        return new[] { mock1, mock2 };
    }

    internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
        CreateCardAttacks().ToDictionary(attack => (int)attack.CardId);

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<long>()).Returns(callInfo => $"invoked: {(long)callInfo[0]}");
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
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(cardAttacks, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestMaxBonus()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T07C123RB1"));
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 9", replacer.Replace("%T07C123RB2"));
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 11", replacer.Replace("%T07C123RB3"));
    }

    [TestMethod]
    public void ReplaceTestTotalMaxBonus()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);

        Assert.AreEqual("invoked: 1001", replacer.Replace("%T07C000RB1"));
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 27", replacer.Replace("%T07C000RB2"));
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 44", replacer.Replace("%T07C000RB3"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentMaxBonus()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T07C001RB1"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T07C001RB2"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T07C001RB3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T07X123RB1", replacer.Replace("%T07X123RB1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T07C142RB1", replacer.Replace("%T07C142RB1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T07C123RB4", replacer.Replace("%T07C123RB4"));
    }
}
