﻿using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Models.Th128;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th128;

internal static class CardDataExtensions
{
    internal static void ShouldBe(this ICardData actual, ICardData expected)
    {
        actual.Signature.ShouldBe(expected.Signature);
        actual.Version.ShouldBe(expected.Version);
        actual.Checksum.ShouldBe(expected.Checksum);
        actual.Size.ShouldBe(expected.Size);

        foreach (var pair in expected.Cards)
        {
            actual.Cards[pair.Key].ShouldBe(pair.Value);
        }
    }
}

[TestClass]
public class CardDataTests
{
    internal static ICardData MockCardData()
    {
        static ISpellCard MockSpellCard(int index)
        {
            var mock = Substitute.For<ISpellCard>();
            _ = mock.Name.Returns(TestUtils.MakeRandomArray(0x80));
            _ = mock.NoMissCount.Returns(123 + index);
            _ = mock.NoIceCount.Returns(456 + index);
            _ = mock.TrialCount.Returns(789 + index);
            _ = mock.Id.Returns(index);
            _ = mock.Level.Returns(Level.Hard);
            _ = mock.HasTried.Returns(true);
            return mock;
        }

        var cards = Enumerable.Range(1, 250).ToDictionary(index => index, MockSpellCard);

        var mock = Substitute.For<ICardData>();
        _ = mock.Signature.Returns("CD");
        _ = mock.Version.Returns((ushort)1);
        _ = mock.Checksum.Returns(0u);
        _ = mock.Size.Returns(0x947C);
        _ = mock.Cards.Returns(cards);
        return mock;
    }

    internal static byte[] MakeByteArray(ICardData cardData)
    {
        return TestUtils.MakeByteArray(
            cardData.Signature.ToCharArray(),
            cardData.Version,
            cardData.Checksum,
            cardData.Size,
            cardData.Cards.Values.Select(SpellCardTests.MakeByteArray));
    }

    [TestMethod]
    public void CardDataTestChapter()
    {
        var mock = MockCardData();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var clearData = new CardData(chapter);

        clearData.ShouldBe(mock);
        clearData.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void CardDataTestInvalidSignature()
    {
        var mock = MockCardData();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new CardData(chapter));
    }

    [TestMethod]
    public void CardDataTestInvalidVersion()
    {
        var mock = MockCardData();
        var version = mock.Version;
        _ = mock.Version.Returns(++version);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new CardData(chapter));
    }

    [TestMethod]
    public void CardDataTestInvalidSize()
    {
        var mock = MockCardData();
        var size = mock.Size;
        _ = mock.Size.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new CardData(chapter));
    }

    [DataTestMethod]
    [DataRow("CD", (ushort)1, 0x947C, true)]
    [DataRow("cd", (ushort)1, 0x947C, false)]
    [DataRow("CD", (ushort)0, 0x947C, false)]
    [DataRow("CD", (ushort)1, 0x947D, false)]
    public void CanInitializeTest(string signature, ushort version, int size, bool expected)
    {
        var checksum = 0u;
        var data = new byte[size];

        var chapter = TestUtils.Create<Chapter>(
            TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

        CardData.CanInitialize(chapter).ShouldBe(expected);
    }
}
