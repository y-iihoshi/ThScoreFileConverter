using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Moq;
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
        _ = mock2.SetupGet(m => m.MaxBonuses).Returns(
            mock1.Object.MaxBonuses.ToDictionary(pair => pair.Key, pair => pair.Value * 1000));
        _ = mock2.SetupGet(m => m.CardId).Returns((short)(mock1.Object.CardId + 1));
        _ = mock2.SetupGet(m => m.CardName).Returns(TestUtils.MakeRandomArray<byte>(0x30));
        _ = mock2.SetupGet(m => m.TrialCounts).Returns(
            mock1.Object.TrialCounts.ToDictionary(pair => pair.Key, pair => (ushort)(pair.Value * 3)));
        _ = mock2.SetupGet(m => m.ClearCounts).Returns(
            mock1.Object.ClearCounts.ToDictionary(pair => pair.Key, pair => (ushort)(pair.Value * 2)));

        return new[] { mock1.Object, mock2.Object };
    }

    internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
        CreateCardAttacks().ToDictionary(attack => (int)attack.CardId);

    private static Mock<INumberFormatter> MockNumberFormatter()
    {
        var mock = new Mock<INumberFormatter>();
        _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => $"invoked: {value}");
        return mock;
    }

    [TestMethod]
    public void CareerReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CareerReplacerTestEmpty()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(cardAttacks, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestMaxBonus()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T07C123RB1"));
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 9", replacer.Replace("%T07C123RB2"));
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 11", replacer.Replace("%T07C123RB3"));
    }

    [TestMethod]
    public void ReplaceTestTotalMaxBonus()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);

        Assert.AreEqual("invoked: 1001", replacer.Replace("%T07C000RB1"));
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 27", replacer.Replace("%T07C000RB2"));
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 44", replacer.Replace("%T07C000RB3"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentMaxBonus()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T07C001RB1"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T07C001RB2"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T07C001RB3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("%T07X123RB1", replacer.Replace("%T07X123RB1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("%T07C142RB1", replacer.Replace("%T07C142RB1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("%T07C123RB4", replacer.Replace("%T07C123RB4"));
    }
}
