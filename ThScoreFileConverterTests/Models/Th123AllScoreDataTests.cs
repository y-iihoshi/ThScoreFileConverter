using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th105;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th105;
using ThScoreFileConverterTests.Models.Th105.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th123AllScoreDataTests
    {
        internal struct Properties
        {
            public Dictionary<Th123Converter.Chara, byte> storyClearCounts;
            public Dictionary<int, ICardForDeck> systemCards;
            public Dictionary<
                Th123Converter.Chara, ClearDataTests.Properties<Th123Converter.Chara, Th123Converter.Level>> clearData;
        };

        internal static Properties GetValidProperties()
        {
            var charas = Utils.GetEnumerator<Th123Converter.Chara>()
                .Where(chara => chara != Th123Converter.Chara.Oonamazu);
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
                    chara => ClearDataTests.MakeValidProperties<Th123Converter.Chara, Th123Converter.Level>())
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

        internal static void Validate(
            in Th105AllScoreDataWrapper<Th123Converter, Th123Converter.Chara, Th123Converter.Level> allScoreData,
            in Properties properties)
        {
            CollectionAssert.That.AreEqual(properties.storyClearCounts.Values, allScoreData.StoryClearCounts.Values);

            foreach (var pair in properties.systemCards)
            {
                CardForDeckTests.Validate(pair.Value, allScoreData.SystemCards[pair.Key]);
            }

            foreach (var pair in properties.clearData)
            {
                ClearDataTests.Validate(pair.Value, allScoreData.ClearData[pair.Key]);
            }
        }

        [TestMethod]
        public void Th123AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th105AllScoreDataWrapper<
                Th123Converter, Th123Converter.Chara, Th123Converter.Level>();

            Assert.AreEqual(0, allScoreData.StoryClearCounts.Count);
            Assert.IsNull(allScoreData.SystemCards);
            Assert.IsNull(allScoreData.ClearData);
        });

        [TestMethod]
        public void Th123AllScoreDataReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();

            var allScoreData = Th105AllScoreDataWrapper<
                Th123Converter, Th123Converter.Chara, Th123Converter.Level>.Create(MakeByteArray(properties));

            Validate(allScoreData, properties);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th123AllScoreDataReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th105AllScoreDataWrapper<
                Th123Converter, Th123Converter.Chara, Th123Converter.Level>();
            allScoreData.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
