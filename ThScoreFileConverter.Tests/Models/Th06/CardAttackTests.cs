using System.IO;
using NSubstitute;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th06;

[TestClass]
public class CardAttackTests
{
    internal static ICardAttack MockCardAttack()
    {
        var attack = Substitute.For<ICardAttack>();
        _ = attack.Signature.Returns("CATK");
        _ = attack.Size1.Returns((short)0x40);
        _ = attack.Size2.Returns((short)0x40);
        _ = attack.CardId.Returns((short)23);
        _ = attack.CardName.Returns(TestUtils.MakeRandomArray(0x24));
        _ = attack.TrialCount.Returns((ushort)789);
        _ = attack.ClearCount.Returns((ushort)456);
        _ = attack.HasTried.Returns(true);
        return attack;
    }

    internal static byte[] MakeByteArray(ICardAttack cardAttack)
    {
        return TestUtils.MakeByteArray(
            cardAttack.Signature.ToCharArray(),
            cardAttack.Size1,
            cardAttack.Size2,
            new byte[8],
            (short)(cardAttack.CardId - 1),
            new byte[6],
            cardAttack.CardName,
            cardAttack.TrialCount,
            cardAttack.ClearCount);
    }

    internal static void Validate(ICardAttack expected, ICardAttack actual)
    {
        Assert.AreEqual(expected.Signature, actual.Signature);
        Assert.AreEqual(expected.Size1, actual.Size1);
        Assert.AreEqual(expected.Size2, actual.Size2);
        Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
        Assert.AreEqual(expected.CardId, actual.CardId);
        CollectionAssert.That.AreEqual(expected.CardName, actual.CardName);
        Assert.AreEqual(expected.TrialCount, actual.TrialCount);
        Assert.AreEqual(expected.ClearCount, actual.ClearCount);
    }

    [TestMethod]
    public void CardAttackTestChapter()
    {
        var mock = MockCardAttack();
        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var cardAttack = new CardAttack(chapter);

        Validate(mock, cardAttack);
        Assert.IsTrue(cardAttack.HasTried);
    }

    [TestMethod]
    public void CardAttackTestInvalidSignature()
    {
        var mock = MockCardAttack();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new CardAttack(chapter));
    }

    [TestMethod]
    public void CardAttackTestInvalidSize1()
    {
        var mock = MockCardAttack();
        var size = mock.Size1;
        _ = mock.Size1.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new CardAttack(chapter));
    }

    [TestMethod]
    public void CardAttackTestNotTried()
    {
        var mock = MockCardAttack();
        _ = mock.TrialCount.Returns((ushort)0);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var cardAttack = new CardAttack(chapter);

        Validate(mock, cardAttack);
        Assert.IsFalse(cardAttack.HasTried);
    }
}
