using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Models.Th06;

namespace ThScoreFileConverter.Tests.Models.Th06;

[TestClass]
public class CardReplacerTests
{
    private static ICardAttack[] CreateCardAttacks()
    {
        var mock1 = CardAttackTests.MockCardAttack();
        var cardId = mock1.CardId;

        var mock2 = CardAttackTests.MockCardAttack();
        _ = mock2.CardId.Returns(++cardId);
        _ = mock2.CardName.Returns(TestUtils.MakeRandomArray(0x24));
        _ = mock2.ClearCount.Returns((ushort)0);
        _ = mock2.TrialCount.Returns((ushort)0);
        _ = mock2.HasTried.Returns(false);

        return [mock1, mock2];
    }

    internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
        CreateCardAttacks().ToDictionary(attack => (int)attack.CardId);

    [TestMethod]
    public void CardReplacerTest()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CardReplacerTestEmpty()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var replacer = new CardReplacer(cardAttacks, true);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(CardAttacks, false);
        replacer.Replace("%T06CARD23N").ShouldBe("火符「アグニレイディアンス」");
        replacer.Replace("%T06CARD24N").ShouldBe("水符「ベリーインレイク」");
    }

    [TestMethod]
    public void ReplaceTestRank()
    {
        var replacer = new CardReplacer(CardAttacks, false);
        replacer.Replace("%T06CARD23R").ShouldBe("Hard, Lunatic");
        replacer.Replace("%T06CARD24R").ShouldBe("Hard, Lunatic");
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        replacer.Replace("%T06CARD23N").ShouldBe("火符「アグニレイディアンス」");
        replacer.Replace("%T06CARD24N").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestHiddenRank()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        replacer.Replace("%T06CARD23R").ShouldBe("Hard, Lunatic");
        replacer.Replace("%T06CARD24R").ShouldBe("?????");
    }

    [TestMethod]
    public void ReplaceTestNonexistentName()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        replacer.Replace("%T06CARD25N").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestNonexistentRank()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        replacer.Replace("%T06CARD25R").ShouldBe("?????");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        replacer.Replace("%T06XXXX23N").ShouldBe("%T06XXXX23N");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        replacer.Replace("%T06CARD65N").ShouldBe("%T06CARD65N");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        replacer.Replace("%T06CARD23X").ShouldBe("%T06CARD23X");
    }
}
