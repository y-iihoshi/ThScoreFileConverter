using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th123;
using ThScoreFileConverterTests.Extensions;
using ICardForDeck = ThScoreFileConverter.Models.Th105.ICardForDeck;
using IClearData = ThScoreFileConverter.Models.Th105.IClearData<ThScoreFileConverter.Models.Th123.Chara>;

namespace ThScoreFileConverterTests.Models.Th123
{
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
            static ICardForDeck CreateCardForDeck(int id)
            {
                var mock = new Mock<ICardForDeck>();
                _ = mock.SetupGet(c => c.Id).Returns(id);
                _ = mock.SetupGet(c => c.MaxNumber).Returns((id % 4) + 1);
                return mock.Object;
            }

            var charas = EnumHelper<Chara>.Enumerable.Where(chara => chara != Chara.Oonamazu);
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
                    chara => Th105.ClearDataTests.MockClearData<Chara>().Object),
            };
        }

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                new uint[2],
                properties.storyClearCounts.Select(pair => pair.Value).ToArray(),
                new byte[0x14 - properties.storyClearCounts.Count],
                new byte[0x28],
                3,
                new uint[3],
                2,
                new uint[2],
                properties.systemCards.Count,
                properties.systemCards.SelectMany(pair => Th105.CardForDeckTests.MakeByteArray(pair.Value)).ToArray(),
                properties.clearData.Count,
                properties.clearData.SelectMany(pair => Th105.ClearDataTests.MakeByteArray(pair.Value)).ToArray());

        internal static void Validate(in Properties expected, in AllScoreData actual)
        {
            CollectionAssert.That.AreEqual(expected.storyClearCounts.Values, actual.StoryClearCounts.Values);

            foreach (var pair in expected.systemCards)
            {
                Th105.CardForDeckTests.Validate(pair.Value, actual.SystemCards[pair.Key]);
            }

            foreach (var pair in expected.clearData)
            {
                Th105.ClearDataTests.Validate(pair.Value, actual.ClearData[pair.Key]);
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
                properties.storyClearCounts.Select(pair => pair.Value).ToArray(),
                new byte[0x14 - properties.storyClearCounts.Count],
                new byte[0x28],
                3,
                new uint[3],
                2,
                new uint[2],
                properties.systemCards.Count + 1,
                properties.systemCards.SelectMany(pair => Th105.CardForDeckTests.MakeByteArray(pair.Value)).ToArray(),
                Th105.CardForDeckTests.MakeByteArray(properties.systemCards.First().Value).ToArray(),
                properties.clearData.Count,
                properties.clearData.SelectMany(pair => Th105.ClearDataTests.MakeByteArray(pair.Value)).ToArray());

            var allScoreData = TestUtils.Create<AllScoreData>(array);

            Validate(properties, allScoreData);
        }

        [TestMethod]
        public void ReadFromTestExceededClearData()
        {
            var properties = MakeValidProperties();
            var array = TestUtils.MakeByteArray(
                new uint[2],
                properties.storyClearCounts.Select(pair => pair.Value).ToArray(),
                new byte[0x14 - properties.storyClearCounts.Count],
                new byte[0x28],
                3,
                new uint[3],
                2,
                new uint[2],
                properties.systemCards.Count,
                properties.systemCards.SelectMany(pair => Th105.CardForDeckTests.MakeByteArray(pair.Value)).ToArray(),
                properties.clearData.Count + 1,
                properties.clearData.SelectMany(pair => Th105.ClearDataTests.MakeByteArray(pair.Value)).ToArray(),
                Th105.ClearDataTests.MakeByteArray(properties.clearData.First().Value));

            var allScoreData = TestUtils.Create<AllScoreData>(array);

            Validate(properties, allScoreData);
        }
    }
}
