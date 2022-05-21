using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th105;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th105;

[TestClass]
public class ClearDataTests
{
    internal static Mock<IClearData<TChara>> MockClearData<TChara>()
        where TChara : struct, Enum
    {
        static ICardForDeck CreateCardForDeck(int id)
        {
            var mock = new Mock<ICardForDeck>();
            _ = mock.SetupGet(c => c.Id).Returns(id);
            _ = mock.SetupGet(c => c.MaxNumber).Returns((id % 4) + 1);
            return mock.Object;
        }

        static ISpellCardResult<TChara> CreateSpellCardResult(TChara chara, int index)
        {
            var mock = new Mock<ISpellCardResult<TChara>>();
            _ = mock.SetupGet(m => m.Enemy).Returns(chara);
            _ = mock.SetupGet(m => m.Level).Returns(Level.Normal);
            _ = mock.SetupGet(m => m.Id).Returns(index + 1);
            _ = mock.SetupGet(m => m.TrialCount).Returns(index * 100);
            _ = mock.SetupGet(m => m.GotCount).Returns(index * 50);
            _ = mock.SetupGet(m => m.Frames).Returns(8901u - (uint)index);
            return mock.Object;
        }

        var mock = new Mock<IClearData<TChara>>();
        _ = mock.SetupGet(m => m.CardsForDeck).Returns(
            Enumerable.Range(1, 10)
                .Select(value => CreateCardForDeck(value))
                .ToDictionary(card => card.Id));
        _ = mock.SetupGet(m => m.SpellCardResults).Returns(
            EnumHelper<TChara>.Enumerable
                .Select((chara, index) => CreateSpellCardResult(chara, index))
                .ToDictionary(result => (result.Enemy, result.Id)));
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
        var clearData = TestUtils.Create<ClearData<TChara>>(MakeByteArray(mock.Object));

        Validate(mock.Object, clearData);
    }

    internal static void ReadFromTestShortenedHelper<TChara>()
        where TChara : struct, Enum
    {
        var mock = MockClearData<TChara>();
        var array = MakeByteArray(mock.Object).SkipLast(1).ToArray();

        _ = Assert.ThrowsException<EndOfStreamException>(() => TestUtils.Create<ClearData<TChara>>(array));
    }

    internal static void ReadFromTestExceededHelper<TChara>()
        where TChara : struct, Enum
    {
        var mock = MockClearData<TChara>();
        var array = MakeByteArray(mock.Object).Concat(new byte[1] { 1 }).ToArray();

        var clearData = TestUtils.Create<ClearData<TChara>>(array);

        Validate(mock.Object, clearData);
    }

    internal static void ReadFromTestDuplicatedHelper<TChara>()
        where TChara : struct, Enum
    {
        var mock = MockClearData<TChara>();
        var array = TestUtils.MakeByteArray(
            mock.Object.CardsForDeck.Count + 1,
            mock.Object.CardsForDeck.Select(pair => CardForDeckTests.MakeByteArray(pair.Value)),
            CardForDeckTests.MakeByteArray(mock.Object.CardsForDeck.First().Value),
            mock.Object.SpellCardResults.Count + 1,
            mock.Object.SpellCardResults.Select(pair => SpellCardResultTests.MakeByteArray(pair.Value)),
            SpellCardResultTests.MakeByteArray(mock.Object.SpellCardResults.First().Value));

        var clearData = TestUtils.Create<ClearData<TChara>>(array);

        Validate(mock.Object, clearData);
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
