using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th06;

[TestClass]
public class CardReplacerTests
{
    private static IEnumerable<ICardAttack> CreateCardAttacks()
    {
        var mock1 = CardAttackTests.MockCardAttack();

        var mock2 = CardAttackTests.MockCardAttack();
        _ = mock2.SetupGet(m => m.CardId).Returns((short)(mock1.Object.CardId + 1));
        _ = mock2.SetupGet(m => m.CardName).Returns(TestUtils.MakeRandomArray<byte>(0x24));
        _ = mock2.SetupGet(m => m.ClearCount).Returns(0);
        _ = mock2.SetupGet(m => m.TrialCount).Returns(0);
        _ = mock2.SetupGet(m => m.HasTried).Returns(false);

        return new[] { mock1.Object, mock2.Object };
    }

    internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
        CreateCardAttacks().ToDictionary(attack => (int)attack.CardId);

    [TestMethod]
    public void CardReplacerTest()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CardReplacerTestEmpty()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var replacer = new CardReplacer(cardAttacks, true);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(CardAttacks, false);
        Assert.AreEqual("火符「アグニレイディアンス」", replacer.Replace("%T06CARD23N"));
        Assert.AreEqual("水符「ベリーインレイク」", replacer.Replace("%T06CARD24N"));
    }

    [TestMethod]
    public void ReplaceTestRank()
    {
        var replacer = new CardReplacer(CardAttacks, false);
        Assert.AreEqual("Hard, Lunatic", replacer.Replace("%T06CARD23R"));
        Assert.AreEqual("Hard, Lunatic", replacer.Replace("%T06CARD24R"));
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("火符「アグニレイディアンス」", replacer.Replace("%T06CARD23N"));
        Assert.AreEqual("??????????", replacer.Replace("%T06CARD24N"));
    }

    [TestMethod]
    public void ReplaceTestHiddenRank()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("Hard, Lunatic", replacer.Replace("%T06CARD23R"));
        Assert.AreEqual("?????", replacer.Replace("%T06CARD24R"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentName()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("??????????", replacer.Replace("%T06CARD25N"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentRank()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("?????", replacer.Replace("%T06CARD25R"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("%T06XXXX23N", replacer.Replace("%T06XXXX23N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("%T06CARD65N", replacer.Replace("%T06CARD65N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("%T06CARD23X", replacer.Replace("%T06CARD23X"));
    }
}
