using NSubstitute;
using ThScoreFileConverter.Models.Th105;

namespace ThScoreFileConverter.Tests.Models.Th105;

internal static class CardForDeckExtensions
{
    internal static void ShouldBe(this ICardForDeck actual, ICardForDeck expected)
    {
        actual.Id.ShouldBe(expected.Id);
        actual.MaxNumber.ShouldBe(expected.MaxNumber);
    }
}

[TestClass]
public class CardForDeckTests
{
    internal static ICardForDeck MockCardForDeck(int id, int maxNumber)
    {
        var mock = Substitute.For<ICardForDeck>();
        _ = mock.Id.Returns(id);
        _ = mock.MaxNumber.Returns(maxNumber);
        return mock;
    }

    internal static byte[] MakeByteArray(ICardForDeck cardForDeck)
    {
        return TestUtils.MakeByteArray(cardForDeck.Id, cardForDeck.MaxNumber);
    }

    [TestMethod]
    public void CardForDeckTest()
    {
        var mock = Substitute.For<ICardForDeck>();
        var cardForDeck = new CardForDeck();

        cardForDeck.ShouldBe(mock);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = MockCardForDeck(1, 2);
        var cardForDeck = TestUtils.Create<CardForDeck>(MakeByteArray(mock));

        cardForDeck.ShouldBe(mock);
    }

    [TestMethod]
    public void ReadFromTestShortened()
    {
        var mock = MockCardForDeck(1, 2);
        var array = MakeByteArray(mock).SkipLast(1).ToArray();

        _ = Should.Throw<EndOfStreamException>(() => TestUtils.Create<CardForDeck>(array));
    }

    [TestMethod]
    public void ReadFromTestExceeded()
    {
        var mock = MockCardForDeck(1, 2);
        var array = MakeByteArray(mock).Concat(new byte[1] { 1 }).ToArray();

        var cardForDeck = TestUtils.Create<CardForDeck>(array);

        cardForDeck.ShouldBe(mock);
    }
}
