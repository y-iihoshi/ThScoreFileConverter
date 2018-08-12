using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th105ClearDataTests
    {
        internal struct Properties<TChara, TLevel>
            where TChara : struct, Enum
            where TLevel : struct, Enum
        {
            public Dictionary<int, Th105CardForDeckTests.Properties> cardsForDeck;
            public Dictionary<
                Th105CharaCardIdPairTests.Properties<TChara>,
                Th105SpellCardResultTests.Properties<TChara, TLevel>> spellCardResults;
        };

        internal static Properties<TChara, TLevel> GetValidProperties<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => new Properties<TChara, TLevel>()
            {
                cardsForDeck = Enumerable.Range(1, 10)
                    .Select(value => new Th105CardForDeckTests.Properties() { id = value, maxNumber = (value % 4) + 1 })
                    .ToDictionary(card => card.id, card => card),
                spellCardResults = Utils.GetEnumerator<TChara>()
                    .Select((chara, index) => new Th105SpellCardResultTests.Properties<TChara, TLevel>()
                    {
                        enemy = chara,
                        level = TestUtils.Cast<TLevel>(1),
                        id = index + 1,
                        trialCount = index * 100,
                        gotCount = index * 50,
                        frames = 8901u - (uint)index
                    })
                    .ToDictionary(
                        result => new Th105CharaCardIdPairTests.Properties<TChara>()
                        {
                            chara = result.enemy,
                            cardId = result.id
                        },
                        result => result)
            };

        internal static byte[] MakeByteArray<TChara, TLevel>(in Properties<TChara, TLevel> properties)
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.MakeByteArray(
                properties.cardsForDeck.Count,
                properties.cardsForDeck
                    .SelectMany(pair => Th105CardForDeckTests.MakeByteArray(pair.Value)).ToArray(),
                properties.spellCardResults.Count,
                properties.spellCardResults
                    .SelectMany(pair => Th105SpellCardResultTests.MakeByteArray(pair.Value)).ToArray());

        internal static void Validate<TParent, TChara, TLevel>(
            in Th105ClearDataWrapper<TParent, TChara, TLevel> clearData,
            in Properties<TChara, TLevel> properties)
            where TParent : ThConverter
            where TChara : struct, Enum
            where TLevel : struct, Enum
        {
            foreach (var pair in properties.cardsForDeck)
            {
                Th105CardForDeckTests.Validate(clearData.CardsForDeckItem(pair.Key), pair.Value);
            }

            foreach (var pair in properties.spellCardResults)
            {
                var charaCardIdPair = new Th105CharaCardIdPairWrapper<TParent, TChara>(pair.Key.chara, pair.Key.cardId);
                Th105SpellCardResultTests.Validate(clearData.SpellCardResultsItem(charaCardIdPair), pair.Value);
            }
        }

        internal static void ClearDataTestHelper<TParent, TChara, TLevel>()
            where TParent : ThConverter
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var clearData = new Th105ClearDataWrapper<TParent, TChara, TLevel>();

                Assert.IsNull(clearData.CardsForDeck);
                Assert.IsNull(clearData.SpellCardResults);
            });

        internal static void ReadFromTestHelper<TParent, TChara, TLevel>()
            where TParent : ThConverter
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TChara, TLevel>();

                var clearData = Th105ClearDataWrapper<TParent, TChara, TLevel>.Create(MakeByteArray(properties));

                Validate(clearData, properties);
            });

        internal static void ReadFromTestNullHelper<TParent, TChara, TLevel>()
            where TParent : ThConverter
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var clearData = new Th105ClearDataWrapper<TParent, TChara, TLevel>();
                clearData.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void ReadFromTestShortenedHelper<TParent, TChara, TLevel>()
            where TParent : ThConverter
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TChara, TLevel>();
                var array = MakeByteArray(properties);
                array = array.Take(array.Length - 1).ToArray();

                Th105ClearDataWrapper<TParent, TChara, TLevel>.Create(array);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void ReadFromTestExceededHelper<TParent, TChara, TLevel>()
            where TParent : ThConverter
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TChara, TLevel>();
                var array = MakeByteArray(properties).Concat(new byte[1] { 1 }).ToArray();

                var clearData = Th105ClearDataWrapper<TParent, TChara, TLevel>.Create(array);

                Validate(clearData, properties);
            });

        internal static void ReadFromTestDuplicatedHelper<TParent, TChara, TLevel>()
            where TParent : ThConverter
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TChara, TLevel>();
                var array = TestUtils.MakeByteArray(
                    properties.cardsForDeck.Count + 1,
                    properties.cardsForDeck
                        .SelectMany(pair => Th105CardForDeckTests.MakeByteArray(pair.Value)).ToArray(),
                    Th105CardForDeckTests.MakeByteArray(properties.cardsForDeck.First().Value),
                    properties.spellCardResults.Count + 1,
                    properties.spellCardResults
                        .SelectMany(pair => Th105SpellCardResultTests.MakeByteArray(pair.Value)).ToArray(),
                    Th105SpellCardResultTests.MakeByteArray(properties.spellCardResults.First().Value));

                var clearData = Th105ClearDataWrapper<TParent, TChara, TLevel>.Create(array);

                Validate(clearData, properties);
            });

        #region Th105

        [TestMethod]
        public void Th105ClearDataTest()
            => ClearDataTestHelper<Th105Converter, Th105Converter.Chara, Th105Converter.Level>();

        [TestMethod]
        public void Th105ClearDataReadFromTest()
            => ReadFromTestHelper<Th105Converter, Th105Converter.Chara, Th105Converter.Level>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th105ClearDataReadFromTestNull()
            => ReadFromTestNullHelper<Th105Converter, Th105Converter.Chara, Th105Converter.Level>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th105ClearDataReadFromTestShortened()
            => ReadFromTestShortenedHelper<Th105Converter, Th105Converter.Chara, Th105Converter.Level>();

        [TestMethod]
        public void Th105ClearDataReadFromTestExceeded()
            => ReadFromTestExceededHelper<Th105Converter, Th105Converter.Chara, Th105Converter.Level>();

        [TestMethod]
        public void Th105ClearDataReadFromTestDuplicated()
            => ReadFromTestDuplicatedHelper<Th105Converter, Th105Converter.Chara, Th105Converter.Level>();

        #endregion

        #region Th123

        [TestMethod]
        public void Th123ClearDataTest()
            => ClearDataTestHelper<Th123Converter, Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        public void Th123ClearDataReadFromTest()
            => ReadFromTestHelper<Th123Converter, Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th123ClearDataReadFromTestNull()
            => ReadFromTestNullHelper<Th123Converter, Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th123ClearDataReadFromTestShortened()
            => ReadFromTestShortenedHelper<Th123Converter, Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        public void Th123ClearDataReadFromTestExceeded()
            => ReadFromTestExceededHelper<Th123Converter, Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        public void Th123ClearDataReadFromTestDuplicated()
            => ReadFromTestDuplicatedHelper<Th123Converter, Th123Converter.Chara, Th123Converter.Level>();

        #endregion
    }
}
