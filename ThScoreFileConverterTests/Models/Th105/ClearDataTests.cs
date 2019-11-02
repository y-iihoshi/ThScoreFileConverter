using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th105;
using ThScoreFileConverterTests.Models.Th105.Stubs;
using Chara = ThScoreFileConverter.Models.Th105.Chara;
using Level = ThScoreFileConverter.Models.Th105.Level;

namespace ThScoreFileConverterTests.Models.Th105
{
    [TestClass]
    public class ClearDataTests
    {
        internal struct Properties<TChara, TLevel>
            where TChara : struct, Enum
            where TLevel : struct, Enum
        {
            public IReadOnlyDictionary<int, ICardForDeck> cardsForDeck;
            public IReadOnlyDictionary<
                (TChara Chara, int CardId),
                SpellCardResultTests.Properties<TChara, TLevel>> spellCardResults;
        };

        internal static Properties<TChara, TLevel> MakeValidProperties<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => new Properties<TChara, TLevel>()
            {
                cardsForDeck = Enumerable.Range(1, 10)
                    .Select(value => new CardForDeckStub
                    {
                        Id = value,
                        MaxNumber = (value % 4) + 1
                    } as ICardForDeck)
                    .ToDictionary(card => card.Id),
                spellCardResults = Utils.GetEnumerator<TChara>()
                    .Select((chara, index) => new SpellCardResultTests.Properties<TChara, TLevel>()
                    {
                        enemy = chara,
                        level = TestUtils.Cast<TLevel>(1),
                        id = index + 1,
                        trialCount = index * 100,
                        gotCount = index * 50,
                        frames = 8901u - (uint)index
                    })
                    .ToDictionary(result => (result.enemy, result.id))
            };

        internal static byte[] MakeByteArray<TChara, TLevel>(in Properties<TChara, TLevel> properties)
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.MakeByteArray(
                properties.cardsForDeck.Count,
                properties.cardsForDeck
                    .SelectMany(pair => CardForDeckTests.MakeByteArray(pair.Value)).ToArray(),
                properties.spellCardResults.Count,
                properties.spellCardResults
                    .SelectMany(pair => SpellCardResultTests.MakeByteArray(pair.Value)).ToArray());

        internal static void Validate<TChara, TLevel>(
            in Properties<TChara, TLevel> expected, in ClearData<TChara, TLevel> actual)
            where TChara : struct, Enum
            where TLevel : struct, Enum
        {
            foreach (var pair in expected.cardsForDeck)
            {
                CardForDeckTests.Validate(pair.Value, actual.CardsForDeck[pair.Key]);
            }

            foreach (var pair in expected.spellCardResults)
            {
                SpellCardResultTests.Validate(
                    pair.Value, actual.SpellCardResults[(pair.Key.Chara, pair.Key.CardId)]);
            }
        }

        internal static void ClearDataTestHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var clearData = new ClearData<TChara, TLevel>();

                Assert.IsNull(clearData.CardsForDeck);
                Assert.IsNull(clearData.SpellCardResults);
            });

        internal static void ReadFromTestHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = MakeValidProperties<TChara, TLevel>();

                var clearData = TestUtils.Create<ClearData<TChara, TLevel>>(MakeByteArray(properties));

                Validate(properties, clearData);
            });

        internal static void ReadFromTestNullHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var clearData = new ClearData<TChara, TLevel>();
                clearData.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void ReadFromTestShortenedHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = MakeValidProperties<TChara, TLevel>();
                var array = MakeByteArray(properties).SkipLast(1).ToArray();

                _ = TestUtils.Create<ClearData<TChara, TLevel>>(array);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void ReadFromTestExceededHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = MakeValidProperties<TChara, TLevel>();
                var array = MakeByteArray(properties).Concat(new byte[1] { 1 }).ToArray();

                var clearData = TestUtils.Create<ClearData<TChara, TLevel>>(array);

                Validate(properties, clearData);
            });

        internal static void ReadFromTestDuplicatedHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = MakeValidProperties<TChara, TLevel>();
                var array = TestUtils.MakeByteArray(
                    properties.cardsForDeck.Count + 1,
                    properties.cardsForDeck
                        .SelectMany(pair => CardForDeckTests.MakeByteArray(pair.Value)).ToArray(),
                    CardForDeckTests.MakeByteArray(properties.cardsForDeck.First().Value),
                    properties.spellCardResults.Count + 1,
                    properties.spellCardResults
                        .SelectMany(pair => SpellCardResultTests.MakeByteArray(pair.Value)).ToArray(),
                    SpellCardResultTests.MakeByteArray(properties.spellCardResults.First().Value));

                var clearData = TestUtils.Create<ClearData<TChara, TLevel>>(array);

                Validate(properties, clearData);
            });

        #region Th105

        [TestMethod]
        public void Th105ClearDataTest()
            => ClearDataTestHelper<Chara, Level>();

        [TestMethod]
        public void Th105ClearDataReadFromTest()
            => ReadFromTestHelper<Chara, Level>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th105ClearDataReadFromTestNull()
            => ReadFromTestNullHelper<Chara, Level>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th105ClearDataReadFromTestShortened()
            => ReadFromTestShortenedHelper<Chara, Level>();

        [TestMethod]
        public void Th105ClearDataReadFromTestExceeded()
            => ReadFromTestExceededHelper<Chara, Level>();

        [TestMethod]
        public void Th105ClearDataReadFromTestDuplicated()
            => ReadFromTestDuplicatedHelper<Chara, Level>();

        #endregion

        #region Th123

        [TestMethod]
        public void Th123ClearDataTest()
            => ClearDataTestHelper<Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        public void Th123ClearDataReadFromTest()
            => ReadFromTestHelper<Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th123ClearDataReadFromTestNull()
            => ReadFromTestNullHelper<Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th123ClearDataReadFromTestShortened()
            => ReadFromTestShortenedHelper<Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        public void Th123ClearDataReadFromTestExceeded()
            => ReadFromTestExceededHelper<Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        public void Th123ClearDataReadFromTestDuplicated()
            => ReadFromTestDuplicatedHelper<Th123Converter.Chara, Th123Converter.Level>();

        #endregion
    }
}
