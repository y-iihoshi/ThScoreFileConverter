using System.IO;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Models.Th105;
using ThScoreFileConverter.Tests.UnitTesting;

#if NETFRAMEWORK
using ThScoreFileConverter.Core.Extensions;
#endif

namespace ThScoreFileConverter.Tests.Models.Th105;

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

    internal static void Validate(ICardForDeck expected, ICardForDeck actual)
    {
        Assert.AreEqual(expected.Id, actual.Id);
        Assert.AreEqual(expected.MaxNumber, actual.MaxNumber);
    }

    [TestMethod]
    public void CardForDeckTest()
    {
        var mock = Substitute.For<ICardForDeck>();
        var cardForDeck = new CardForDeck();

        Validate(mock, cardForDeck);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = MockCardForDeck(1, 2);
        var cardForDeck = TestUtils.Create<CardForDeck>(MakeByteArray(mock));

        Validate(mock, cardForDeck);
    }

    [TestMethod]
    public void ReadFromTestShortened()
    {
        var mock = MockCardForDeck(1, 2);
        var array = MakeByteArray(mock).SkipLast(1).ToArray();

        _ = Assert.ThrowsException<EndOfStreamException>(() => TestUtils.Create<CardForDeck>(array));
    }

    [TestMethod]
    public void ReadFromTestExceeded()
    {
        var mock = MockCardForDeck(1, 2);
        var array = MakeByteArray(mock).Concat(new byte[1] { 1 }).ToArray();

        var cardForDeck = TestUtils.Create<CardForDeck>(array);

        Validate(mock, cardForDeck);
    }
}
