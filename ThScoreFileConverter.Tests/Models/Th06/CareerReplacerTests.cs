using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Moq;
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

        var mock2 = CardAttackTests.MockCardAttack();
        _ = mock2.SetupGet(m => m.CardId).Returns((short)(mock1.Object.CardId + 1));
        _ = mock2.SetupGet(m => m.CardName).Returns(TestUtils.MakeRandomArray<byte>(0x24));
        _ = mock2.SetupGet(m => m.ClearCount).Returns((ushort)(mock1.Object.ClearCount + 2));
        _ = mock2.SetupGet(m => m.TrialCount).Returns((ushort)(mock1.Object.TrialCount + 3));

        return new[] { mock1.Object, mock2.Object };
    }

    internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
        CreateCardAttacks().ToDictionary(attack => (int)attack.CardId);

    private static Mock<INumberFormatter> MockNumberFormatter()
    {
        var mock = new Mock<INumberFormatter>();
        _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => "invoked: " + value.ToString());
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
        var formatterMock = MockNumberFormatter();
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var replacer = new CareerReplacer(cardAttacks, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 456", replacer.Replace("%T06C231"));
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 789", replacer.Replace("%T06C232"));
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 914", replacer.Replace("%T06C001"));
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);

        Assert.AreEqual("invoked: 1581", replacer.Replace("%T06C002"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T06C011"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T06C012"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("%T06X231", replacer.Replace("%T06X231"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("%T06C651", replacer.Replace("%T06C651"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("%T06C233", replacer.Replace("%T06C233"));
    }
}
