using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Models.Th18;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th18;

[TestClass]
public class AbilityCardReplacerTests
{
    internal static IAbilityCardHolder MockAbilityCardHolder()
    {
        var mock = Substitute.For<IAbilityCardHolder>();
        _ = mock.AbilityCards.Returns(Enumerable.Range(0, 56).Select(number => (byte)(number % 4)));
        _ = mock.InitialHoldAbilityCards.Returns(TestUtils.MakeRandomArray(0x30));
        return mock;
    }

    internal static IAbilityCardHolder AbilityCardHolder { get; } = MockAbilityCardHolder();

    [TestMethod]
    public void AbilityCardReplacerTest()
    {
        var replacer = new AbilityCardReplacer(AbilityCardHolder);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new AbilityCardReplacer(AbilityCardHolder);
        Assert.AreEqual("太古の勾玉", replacer.Replace("%T18ABIL22"));
    }

    [TestMethod]
    public void ReplaceTestNotCleared()
    {
        var replacer = new AbilityCardReplacer(AbilityCardHolder);
        Assert.AreEqual("??????????", replacer.Replace("%T18ABIL05"));
    }

    [TestMethod]
    public void ReplaceTestZeroNumber()
    {
        var replacer = new AbilityCardReplacer(AbilityCardHolder);
        Assert.AreEqual("%T18ABIL00", replacer.Replace("%T18ABIL00"));
    }

    [TestMethod]
    public void ReplaceTestExceededNumber()
    {
        var replacer = new AbilityCardReplacer(AbilityCardHolder);
        Assert.AreEqual("%T18ABIL57", replacer.Replace("%T18ABIL57"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new AbilityCardReplacer(AbilityCardHolder);
        Assert.AreEqual("%T18XXXX22", replacer.Replace("%T18XXXX22"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new AbilityCardReplacer(AbilityCardHolder);
        Assert.AreEqual("%T18ABILXX", replacer.Replace("%T18ABILXX"));
    }
}
