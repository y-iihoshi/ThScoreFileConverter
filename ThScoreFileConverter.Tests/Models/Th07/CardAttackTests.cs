using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th07;
using ThScoreFileConverter.Models.Th07;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th07;

internal static class CardAttackExtensions
{
    internal static void ShouldBe(this ICardAttack actual, ICardAttack expected)
    {
        actual.Signature.ShouldBe(expected.Signature);
        actual.Size1.ShouldBe(expected.Size1);
        actual.Size2.ShouldBe(expected.Size2);
        actual.FirstByteOfData.ShouldBe(expected.FirstByteOfData);
        actual.MaxBonuses.Values.ShouldBe(expected.MaxBonuses.Values);
        actual.CardId.ShouldBe(expected.CardId);
        actual.CardName.ShouldBe(expected.CardName);
        actual.TrialCounts.Values.ShouldBe(expected.TrialCounts.Values);
        actual.ClearCounts.Values.ShouldBe(expected.ClearCounts.Values);
    }
}

[TestClass]
public class CardAttackTests
{
    internal static ICardAttack MockCardAttack()
    {
        var pairs = EnumHelper<CharaWithTotal>.Enumerable.Select((chara, index) => (chara, index)).ToArray();
        var mock = Substitute.For<ICardAttack>();

        _ = mock.Signature.Returns("CATK");
        _ = mock.Size1.Returns((short)0x78);
        _ = mock.Size2.Returns((short)0x78);
        _ = mock.MaxBonuses.Returns(pairs.ToDictionary(pair => pair.chara, pair => (uint)pair.index));
        _ = mock.CardId.Returns((short)123);
        _ = mock.CardName.Returns(TestUtils.MakeRandomArray(0x30));
        _ = mock.TrialCounts.Returns(pairs.ToDictionary(pair => pair.chara, pair => (ushort)(10 + pair.index)));
        _ = mock.ClearCounts.Returns(pairs.ToDictionary(pair => pair.chara, pair => (ushort)(10 - pair.index)));

        var hasTried = mock.TrialCounts.TryGetValue(CharaWithTotal.Total, out var count) && (count > 0);
        _ = mock.HasTried.Returns(hasTried);

        return mock;
    }

    internal static byte[] MakeByteArray(ICardAttack cardAttack)
    {
        return TestUtils.MakeByteArray(
            cardAttack.Signature.ToCharArray(),
            cardAttack.Size1,
            cardAttack.Size2,
            0u,
            cardAttack.MaxBonuses.Values,
            (short)(cardAttack.CardId - 1),
            (byte)0,
            cardAttack.CardName,
            (byte)0,
            cardAttack.TrialCounts.Values,
            cardAttack.ClearCounts.Values);
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
        var trialCounts = mock.TrialCounts;
        _ = mock.TrialCounts.Returns(
            trialCounts.ToDictionary(
                pair => pair.Key,
                pair => (pair.Key == CharaWithTotal.Total) ? (ushort)0 : pair.Value));

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var cardAttack = new CardAttack(chapter);

        cardAttack.ShouldBe(mock);
        cardAttack.HasTried.ShouldBeFalse();
    }
}
