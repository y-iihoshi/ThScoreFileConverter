using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th07;

[TestClass]
public class CardReplacerTests
{
    private static ICardAttack[] CreateCardAttacks()
    {
        var mock1 = CardAttackTests.MockCardAttack();
        var maxBonuses = mock1.MaxBonuses;
        var cardId = mock1.CardId;
        var trialCounts = mock1.TrialCounts;
        var clearCounts = mock1.ClearCounts;

        var mock2 = CardAttackTests.MockCardAttack();
        _ = mock2.MaxBonuses.Returns(maxBonuses.ToDictionary(pair => pair.Key, pair => pair.Value * 1000));
        _ = mock2.CardId.Returns(++cardId);
        _ = mock2.CardName.Returns(TestUtils.MakeRandomArray(0x30));
        _ = mock2.TrialCounts.Returns(trialCounts.ToDictionary(pair => pair.Key, pair => (ushort)0));
        _ = mock2.ClearCounts.Returns(clearCounts.ToDictionary(pair => pair.Key, pair => (ushort)0));
        _ = mock2.HasTried.Returns(false);

        return [mock1, mock2];
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
        Assert.AreEqual("式輝「プリンセス天狐 -Illusion-」", replacer.Replace("%T07CARD123N"));
        Assert.AreEqual("式弾「アルティメットブディスト」", replacer.Replace("%T07CARD124N"));
    }

    [TestMethod]
    public void ReplaceTestRank()
    {
        var replacer = new CardReplacer(CardAttacks, false);
        Assert.AreEqual("Extra", replacer.Replace("%T07CARD123R"));
        Assert.AreEqual("Extra", replacer.Replace("%T07CARD124R"));
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("式輝「プリンセス天狐 -Illusion-」", replacer.Replace("%T07CARD123N"));
        Assert.AreEqual("??????????", replacer.Replace("%T07CARD124N"));
    }

    [TestMethod]
    public void ReplaceTestHiddenRank()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("Extra", replacer.Replace("%T07CARD123R"));
        Assert.AreEqual("?????", replacer.Replace("%T07CARD124R"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentName()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("??????????", replacer.Replace("%T07CARD125N"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentRank()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("?????", replacer.Replace("%T07CARD125R"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("%T07XXXX123N", replacer.Replace("%T07XXXX123N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("%T07CARD142N", replacer.Replace("%T07CARD142N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("%T07CARD123X", replacer.Replace("%T07CARD123X"));
    }
}
