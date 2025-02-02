using NSubstitute;
using ThScoreFileConverter.Models.Th06;

namespace ThScoreFileConverter.Tests.Models.Th06;

internal static class CardAttackExtensions
{
    internal static void ShouldBe(this ICardAttack actual, ICardAttack expected)
    {
        actual.Signature.ShouldBe(expected.Signature);
        actual.Size1.ShouldBe(expected.Size1);
        actual.Size2.ShouldBe(expected.Size2);
        actual.FirstByteOfData.ShouldBe(expected.FirstByteOfData);
        actual.CardId.ShouldBe(expected.CardId);
        actual.CardName.ShouldBe(expected.CardName);
        actual.TrialCount.ShouldBe(expected.TrialCount);
        actual.ClearCount.ShouldBe(expected.ClearCount);
    }
}

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

    [TestMethod]
    public void CardAttackTestChapter()
    {
        var mock = MockCardAttack();
        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var cardAttack = new CardAttack(chapter);

        cardAttack.ShouldBe(mock);
        cardAttack.HasTried.ShouldBeTrue();
    }

    [TestMethod]
    public void CardAttackTestInvalidSignature()
    {
        var mock = MockCardAttack();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new CardAttack(chapter));
    }

    [TestMethod]
    public void CardAttackTestInvalidSize1()
    {
        var mock = MockCardAttack();
        var size = mock.Size1;
        _ = mock.Size1.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new CardAttack(chapter));
    }

    [TestMethod]
    public void CardAttackTestNotTried()
    {
        var mock = MockCardAttack();
        _ = mock.TrialCount.Returns((ushort)0);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var cardAttack = new CardAttack(chapter);

        cardAttack.ShouldBe(mock);
        cardAttack.HasTried.ShouldBeFalse();
    }
}
