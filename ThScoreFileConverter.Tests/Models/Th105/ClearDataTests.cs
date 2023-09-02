using System;
using System.IO;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th105;
using ThScoreFileConverter.Models.Th105;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th105;

[TestClass]
public class ClearDataTests
{
    internal static IClearData<TChara> MockClearData<TChara>()
        where TChara : struct, Enum
    {
        static ICardForDeck MockCardForDeck(int id)
        {
            return CardForDeckTests.MockCardForDeck(id, (id % 4) + 1);
        }

        static ISpellCardResult<TChara> MockSpellCardResult(TChara enemy, int index)
        {
            return SpellCardResultTests.MockSpellCardResult(enemy, Level.Normal, index + 1, index * 100, index * 50, 8901u - (uint)index);
        }

        var cardsForDeck = Enumerable.Range(1, 10).Select(MockCardForDeck).ToDictionary(card => card.Id);
        var spellCardResults = EnumHelper<TChara>.Enumerable.Select(MockSpellCardResult).ToDictionary(result => (result.Enemy, result.Id));
        var mock = Substitute.For<IClearData<TChara>>();
        _ = mock.CardsForDeck.Returns(cardsForDeck);
        _ = mock.SpellCardResults.Returns(spellCardResults);
        return mock;
    }

    internal static byte[] MakeByteArray<TChara>(IClearData<TChara> properties)
        where TChara : struct, Enum
    {
        return TestUtils.MakeByteArray(
            properties.CardsForDeck.Count,
            properties.CardsForDeck.Select(pair => CardForDeckTests.MakeByteArray(pair.Value)),
            properties.SpellCardResults.Count,
            properties.SpellCardResults.Select(pair => SpellCardResultTests.MakeByteArray(pair.Value)));
    }

    internal static void Validate<TChara>(IClearData<TChara> expected, IClearData<TChara> actual)
        where TChara : struct, Enum
    {
        foreach (var pair in expected.CardsForDeck)
        {
            CardForDeckTests.Validate(pair.Value, actual.CardsForDeck[pair.Key]);
        }

        foreach (var pair in expected.SpellCardResults)
        {
            SpellCardResultTests.Validate(pair.Value, actual.SpellCardResults[(pair.Key.Chara, pair.Key.CardId)]);
        }
    }

    internal static void ClearDataTestHelper<TChara>()
        where TChara : struct, Enum
    {
        var clearData = new ClearData<TChara>();

        Assert.AreEqual(0, clearData.CardsForDeck.Count);
        Assert.AreEqual(0, clearData.SpellCardResults.Count);
    }

    internal static void ReadFromTestHelper<TChara>()
        where TChara : struct, Enum
    {
        var mock = MockClearData<TChara>();
        var clearData = TestUtils.Create<ClearData<TChara>>(MakeByteArray(mock));

        Validate(mock, clearData);
    }

    internal static void ReadFromTestShortenedHelper<TChara>()
        where TChara : struct, Enum
    {
        var mock = MockClearData<TChara>();
        var array = MakeByteArray(mock).SkipLast(1).ToArray();

        _ = Assert.ThrowsException<EndOfStreamException>(() => TestUtils.Create<ClearData<TChara>>(array));
    }

    internal static void ReadFromTestExceededHelper<TChara>()
        where TChara : struct, Enum
    {
        var mock = MockClearData<TChara>();
        var array = MakeByteArray(mock).Concat(new byte[1] { 1 }).ToArray();

        var clearData = TestUtils.Create<ClearData<TChara>>(array);

        Validate(mock, clearData);
    }

    internal static void ReadFromTestDuplicatedHelper<TChara>()
        where TChara : struct, Enum
    {
        var mock = MockClearData<TChara>();
        var array = TestUtils.MakeByteArray(
            mock.CardsForDeck.Count + 1,
            mock.CardsForDeck.Select(pair => CardForDeckTests.MakeByteArray(pair.Value)),
            CardForDeckTests.MakeByteArray(mock.CardsForDeck.First().Value),
            mock.SpellCardResults.Count + 1,
            mock.SpellCardResults.Select(pair => SpellCardResultTests.MakeByteArray(pair.Value)),
            SpellCardResultTests.MakeByteArray(mock.SpellCardResults.First().Value));

        var clearData = TestUtils.Create<ClearData<TChara>>(array);

        Validate(mock, clearData);
    }

    [TestMethod]
    public void ClearDataTest()
    {
        ClearDataTestHelper<Chara>();
    }

    [TestMethod]
    public void ReadFromTest()
    {
        ReadFromTestHelper<Chara>();
    }

    [TestMethod]
    public void ReadFromTestShortened()
    {
        ReadFromTestShortenedHelper<Chara>();
    }

    [TestMethod]
    public void ReadFromTestExceeded()
    {
        ReadFromTestExceededHelper<Chara>();
    }

    [TestMethod]
    public void ReadFromTestDuplicated()
    {
        ReadFromTestDuplicatedHelper<Chara>();
    }
}
