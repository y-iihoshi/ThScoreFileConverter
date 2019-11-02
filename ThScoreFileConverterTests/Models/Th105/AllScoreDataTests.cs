using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th105;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th105.Stubs;
using Level = ThScoreFileConverter.Models.Th105.Level;

namespace ThScoreFileConverterTests.Models.Th105
{
    [TestClass]
    public class AllScoreDataTests
    {
        internal struct Properties
        {
            public IReadOnlyDictionary<Chara, byte> storyClearCounts;
            public IReadOnlyDictionary<int, ICardForDeck> systemCards;
            public IReadOnlyDictionary<Chara, IClearData<Chara, Level>> clearData;
        };

        internal static Properties MakeValidProperties()
        {
            var charas = Utils.GetEnumerator<Chara>();
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
                    chara => ClearDataTests.MakeValidStub<Chara, Level>() as IClearData<Chara, Level>)
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
                properties.systemCards.SelectMany(pair => CardForDeckTests.MakeByteArray(pair.Value)).ToArray(),
                properties.clearData.Count,
                properties.clearData.SelectMany(pair => ClearDataTests.MakeByteArray(pair.Value)).ToArray());

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
