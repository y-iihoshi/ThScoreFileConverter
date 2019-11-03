using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th123;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th105.Stubs;
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
        };

        internal static Properties MakeValidProperties()
        {
            var charas = Utils.GetEnumerator<Chara>().Where(chara => chara != Chara.Oonamazu);
            return new Properties()
            {
                storyClearCounts = charas.ToDictionary(
                    chara => chara,
                    chara => TestUtils.Cast<byte>(chara)),
                systemCards = Enumerable.Range(1, 5).ToDictionary(
                    id => id,
                    id => new CardForDeckStub
                    {
                        Id = id,
                        MaxNumber = id % 4 + 1
                    } as ICardForDeck),
                clearData = charas.ToDictionary(
                    chara => chara,
                    chara => Th105.ClearDataTests.MakeValidStub<Chara>() as IClearData)
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
        public void AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new AllScoreData();

            Assert.AreEqual(0, allScoreData.StoryClearCounts.Count);
            Assert.IsNull(allScoreData.SystemCards);
            Assert.IsNull(allScoreData.ClearData);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = MakeValidProperties();

            var allScoreData = TestUtils.Create<AllScoreData>(MakeByteArray(properties));

            Validate(properties, allScoreData);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var allScoreData = new AllScoreData();
            allScoreData.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
