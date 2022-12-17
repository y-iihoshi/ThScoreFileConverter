using System.Collections.Generic;
using System.Linq;
using Moq;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th105;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models.Th105;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th105;

[TestClass]
public class AllScoreDataTests
{
    internal struct Properties
    {
        public IReadOnlyDictionary<Chara, byte> storyClearCounts;
        public IReadOnlyDictionary<int, ICardForDeck> systemCards;
        public IReadOnlyDictionary<Chara, IClearData<Chara>> clearData;
    }

    internal static Properties MakeValidProperties()
    {
        static ICardForDeck CreateCardForDeck(int id)
        {
            var mock = new Mock<ICardForDeck>();
            _ = mock.SetupGet(c => c.Id).Returns(id);
            _ = mock.SetupGet(c => c.MaxNumber).Returns((id % 4) + 1);
            return mock.Object;
        }

        var charas = EnumHelper<Chara>.Enumerable;
        return new Properties()
        {
            storyClearCounts = charas.ToDictionary(
                chara => chara,
                chara => TestUtils.Cast<byte>(chara)),
            systemCards = Enumerable.Range(1, 5).ToDictionary(
                id => id,
                id => CreateCardForDeck(id)),
            clearData = charas.ToDictionary(
                chara => chara,
                chara => ClearDataTests.MockClearData<Chara>().Object),
        };
    }

    internal static byte[] MakeByteArray(in Properties properties)
    {
        return TestUtils.MakeByteArray(
            new uint[2],
            properties.storyClearCounts.Select(pair => pair.Value),
            new byte[0x14 - properties.storyClearCounts.Count],
            new byte[0x28],
            3,
            new uint[3],
            2,
            new uint[2],
            properties.systemCards.Count,
            properties.systemCards.Select(pair => CardForDeckTests.MakeByteArray(pair.Value)),
            properties.clearData.Count,
            properties.clearData.Select(pair => ClearDataTests.MakeByteArray(pair.Value)));
    }

    internal static void Validate(in Properties expected, in AllScoreData actual)
    {
        CollectionAssert.That.AreEqual(expected.storyClearCounts.Values, actual.StoryClearCounts.Values);

        foreach (var pair in expected.systemCards)
        {
            CardForDeckTests.Validate(pair.Value, actual.SystemCards[pair.Key]);
        }

        foreach (var pair in expected.clearData)
        {
            ClearDataTests.Validate(pair.Value, actual.ClearData[pair.Key]);
        }
    }

    [TestMethod]
    public void AllScoreDataTest()
    {
        var allScoreData = new AllScoreData();

        Assert.AreEqual(0, allScoreData.StoryClearCounts.Count);
        Assert.AreEqual(0, allScoreData.SystemCards.Count);
        Assert.AreEqual(0, allScoreData.ClearData.Count);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var properties = MakeValidProperties();

        var allScoreData = TestUtils.Create<AllScoreData>(MakeByteArray(properties));

        Validate(properties, allScoreData);
    }

    [TestMethod]
    public void ReadFromTestDuplicatedSystemCard()
    {
        var properties = MakeValidProperties();
        var array = TestUtils.MakeByteArray(
            new uint[2],
            properties.storyClearCounts.Select(pair => pair.Value),
            new byte[0x14 - properties.storyClearCounts.Count],
            new byte[0x28],
            3,
            new uint[3],
            2,
            new uint[2],
            properties.systemCards.Count + 1,
            properties.systemCards.Select(pair => CardForDeckTests.MakeByteArray(pair.Value)),
            CardForDeckTests.MakeByteArray(properties.systemCards.First().Value),
            properties.clearData.Count,
            properties.clearData.Select(pair => ClearDataTests.MakeByteArray(pair.Value)));

        var allScoreData = TestUtils.Create<AllScoreData>(array);

        Validate(properties, allScoreData);
    }

    [TestMethod]
    public void ReadFromTestExceededClearData()
    {
        var properties = MakeValidProperties();
        var array = TestUtils.MakeByteArray(
            new uint[2],
            properties.storyClearCounts.Select(pair => pair.Value),
            new byte[0x14 - properties.storyClearCounts.Count],
            new byte[0x28],
            3,
            new uint[3],
            2,
            new uint[2],
            properties.systemCards.Count,
            properties.systemCards.Select(pair => CardForDeckTests.MakeByteArray(pair.Value)),
            properties.clearData.Count + 1,
            properties.clearData.Select(pair => ClearDataTests.MakeByteArray(pair.Value)),
            ClearDataTests.MakeByteArray(properties.clearData.First().Value));

        var allScoreData = TestUtils.Create<AllScoreData>(array);

        Validate(properties, allScoreData);
    }
}
