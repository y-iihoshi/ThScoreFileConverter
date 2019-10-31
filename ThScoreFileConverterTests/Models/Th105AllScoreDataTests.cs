using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th105;
using ThScoreFileConverterTests.Models.Wrappers;
using Chara = ThScoreFileConverter.Models.Th105.Chara;
using Level = ThScoreFileConverter.Models.Th105.Level;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th105AllScoreDataTests
    {
        internal struct Properties<TChara, TLevel>
            where TChara : struct, Enum
            where TLevel : struct, Enum
        {
            public Dictionary<TChara, byte> storyClearCounts;
            public Dictionary<int, CardForDeckTests.Properties> systemCards;
            public Dictionary<TChara, ClearDataTests.Properties<TChara, TLevel>> clearData;
        };

        internal static Properties<TChara, TLevel> GetValidProperties<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
        {
            var charas = Utils.GetEnumerator<TChara>()
                .Where(chara => TestUtils.Cast<int>(chara) != TestUtils.Cast<int>(Th123Converter.Chara.Oonamazu));
            return new Properties<TChara, TLevel>()
            {
                storyClearCounts = charas.ToDictionary(
                    chara => chara,
                    chara => TestUtils.Cast<byte>(chara)),
                systemCards = Enumerable.Range(1, 5).ToDictionary(
                    id => id,
                    id => new CardForDeckTests.Properties()
                    {
                        id = id,
                        maxNumber = id % 4 + 1
                    }),
                clearData = charas.ToDictionary(
                    chara => chara,
                    chara => ClearDataTests.GetValidProperties<TChara, TLevel>())
            };
        }

        internal static byte[] MakeByteArray<TChara, TLevel>(in Properties<TChara, TLevel> properties)
            where TChara : struct, Enum
            where TLevel : struct, Enum
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

        internal static void Validate<TParent, TChara, TLevel>(
            in Th105AllScoreDataWrapper<TParent, TChara, TLevel> allScoreData,
            in Properties<TChara, TLevel> properties)
            where TParent : ThConverter
            where TChara : struct, Enum
            where TLevel : struct, Enum
        {
            CollectionAssert.That.AreEqual(properties.storyClearCounts.Values, allScoreData.StoryClearCounts.Values);

            foreach (var pair in properties.systemCards)
            {
                CardForDeckTests.Validate(pair.Value, allScoreData.SystemCards[pair.Key]);
            }

            foreach (var pair in properties.clearData)
            {
                ClearDataTests.Validate(allScoreData.ClearData[pair.Key], pair.Value);
            }
        }

        internal static void Th105AllScoreDataTestHelper<TParent, TChara, TLevel>()
            where TParent : ThConverter
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var allScoreData = new Th105AllScoreDataWrapper<TParent, TChara, TLevel>();

                Assert.AreEqual(0, allScoreData.StoryClearCounts.Count);
                Assert.IsNull(allScoreData.SystemCards);
                Assert.IsNull(allScoreData.ClearData);
            });

        internal static void Th105AllScoreDataReadFromTestHelper<TParent, TChara, TLevel>()
            where TParent : ThConverter
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TChara, TLevel>();

                var allScoreData = Th105AllScoreDataWrapper<TParent, TChara, TLevel>.Create(MakeByteArray(properties));

                Validate(allScoreData, properties);
            });

        internal static void Th105AllScoreDataReadFromTestNullHelper<TParent, TChara, TLevel>()
            where TParent : ThConverter
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var allScoreData = new Th105AllScoreDataWrapper<TParent, TChara, TLevel>();
                allScoreData.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        #region Th105

        [TestMethod]
        public void Th105AllScoreDataTest()
            => Th105AllScoreDataTestHelper<Th105Converter, Chara, Level>();

        [TestMethod]
        public void Th105AllScoreDataReadFromTest()
            => Th105AllScoreDataReadFromTestHelper<Th105Converter, Chara, Level>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th105AllScoreDataReadFromTestNull()
            => Th105AllScoreDataReadFromTestNullHelper<Th105Converter, Chara, Level>();

        #endregion

        #region Th123

        [TestMethod]
        public void Th123AllScoreDataTest()
            => Th105AllScoreDataTestHelper<Th123Converter, Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        public void Th123AllScoreDataReadFromTest()
            => Th105AllScoreDataReadFromTestHelper<Th123Converter, Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th123AllScoreDataReadFromTestNull()
            => Th105AllScoreDataReadFromTestNullHelper<Th123Converter, Th123Converter.Chara, Th123Converter.Level>();

        #endregion
    }
}
