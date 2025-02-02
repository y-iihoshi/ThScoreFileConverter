using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th123;
using ThScoreFileConverter.Models.Th123;
using static ThScoreFileConverter.Tests.Models.Th105.CardForDeckExtensions;
using static ThScoreFileConverter.Tests.Models.Th105.ClearDataExtensions;
using ICardForDeck = ThScoreFileConverter.Models.Th105.ICardForDeck;
using IClearData = ThScoreFileConverter.Models.Th105.IClearData<ThScoreFileConverter.Core.Models.Th123.Chara>;

namespace ThScoreFileConverter.Tests.Models.Th123;

[TestClass]
public class AllScoreDataTests
{
    internal struct Properties
    {
        public IReadOnlyDictionary<Chara, byte> storyClearCounts;
        public IReadOnlyDictionary<int, ICardForDeck> systemCards;
        public IReadOnlyDictionary<Chara, IClearData> clearData;
    }

    internal static Properties MakeValidProperties()
    {
        static ICardForDeck MockCardForDeck(int id)
        {
            return Th105.CardForDeckTests.MockCardForDeck(id, (id % 4) + 1);
        }

        var charas = EnumHelper<Chara>.Enumerable.Where(chara => chara != Chara.Catfish).ToArray();
        return new Properties()
        {
            storyClearCounts = charas.ToDictionary(chara => chara, chara => (byte)chara),
            systemCards = Enumerable.Range(1, 5).ToDictionary(id => id, MockCardForDeck),
            clearData = charas.ToDictionary(chara => chara, chara => Th105.ClearDataTests.MockClearData<Chara>()),
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
            properties.systemCards.Select(pair => Th105.CardForDeckTests.MakeByteArray(pair.Value)),
            properties.clearData.Count,
            properties.clearData.Select(pair => Th105.ClearDataTests.MakeByteArray(pair.Value)));
    }

    internal static void Validate(in Properties expected, in AllScoreData actual)
    {
        actual.StoryClearCounts.Values.ShouldBe(expected.storyClearCounts.Values);

        foreach (var pair in expected.systemCards)
        {
            actual.SystemCards[pair.Key].ShouldBe(pair.Value);
        }

        foreach (var pair in expected.clearData)
        {
            actual.ClearData[pair.Key].ShouldBe(pair.Value);
        }
    }

    [TestMethod]
    public void AllScoreDataTest()
    {
        var allScoreData = new AllScoreData();

        allScoreData.StoryClearCounts.ShouldBeEmpty();
        allScoreData.SystemCards.ShouldBeEmpty();
        allScoreData.ClearData.ShouldBeEmpty();
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
            properties.systemCards.Select(pair => Th105.CardForDeckTests.MakeByteArray(pair.Value)),
            Th105.CardForDeckTests.MakeByteArray(properties.systemCards.First().Value),
            properties.clearData.Count,
            properties.clearData.Select(pair => Th105.ClearDataTests.MakeByteArray(pair.Value)));

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
            properties.systemCards.Select(pair => Th105.CardForDeckTests.MakeByteArray(pair.Value)),
            properties.clearData.Count + 1,
            properties.clearData.Select(pair => Th105.ClearDataTests.MakeByteArray(pair.Value)),
            Th105.ClearDataTests.MakeByteArray(properties.clearData.First().Value));

        var allScoreData = TestUtils.Create<AllScoreData>(array);

        Validate(properties, allScoreData);
    }
}
