using System.IO;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th07;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverter.Tests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th07;

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

    internal static void Validate(ICardAttack expected, ICardAttack actual)
    {
        Assert.AreEqual(expected.Signature, actual.Signature);
        Assert.AreEqual(expected.Size1, actual.Size1);
        Assert.AreEqual(expected.Size2, actual.Size2);
        Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
        CollectionAssert.That.AreEqual(expected.MaxBonuses.Values, actual.MaxBonuses.Values);
        Assert.AreEqual(expected.CardId, actual.CardId);
        CollectionAssert.That.AreEqual(expected.CardName, actual.CardName);
        CollectionAssert.That.AreEqual(expected.TrialCounts.Values, actual.TrialCounts.Values);
        CollectionAssert.That.AreEqual(expected.ClearCounts.Values, actual.ClearCounts.Values);
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
        var trialCounts = mock.TrialCounts;
        _ = mock.TrialCounts.Returns(
            trialCounts.ToDictionary(
                pair => pair.Key,
                pair => (pair.Key == CharaWithTotal.Total) ? (ushort)0 : pair.Value));

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var cardAttack = new CardAttack(chapter);

        Validate(mock, cardAttack);
        Assert.IsFalse(cardAttack.HasTried);
    }
}
