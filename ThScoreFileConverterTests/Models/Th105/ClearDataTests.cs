using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
        internal static ClearDataStub<TChara, TLevel> MakeValidStub<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => new ClearDataStub<TChara, TLevel>()
            {
                CardsForDeck = Enumerable.Range(1, 10)
                    .Select(value => new CardForDeckStub
                    {
                        Id = value,
                        MaxNumber = (value % 4) + 1
                    } as ICardForDeck)
                    .ToDictionary(card => card.Id),
                SpellCardResults = Utils.GetEnumerator<TChara>()
                    .Select((chara, index) => new SpellCardResultStub<TChara, TLevel>()
                    {
                        Enemy = chara,
                        Level = TestUtils.Cast<TLevel>(1),
                        Id = index + 1,
                        TrialCount = index * 100,
                        GotCount = index * 50,
                        Frames = 8901u - (uint)index
                    } as ISpellCardResult<TChara, TLevel>)
                    .ToDictionary(result => (result.Enemy, result.Id))
            };

        internal static byte[] MakeByteArray<TChara, TLevel>(IClearData<TChara, TLevel> properties)
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.MakeByteArray(
                properties.CardsForDeck.Count,
                properties.CardsForDeck
                    .SelectMany(pair => CardForDeckTests.MakeByteArray(pair.Value)).ToArray(),
                properties.SpellCardResults.Count,
                properties.SpellCardResults
                    .SelectMany(pair => SpellCardResultTests.MakeByteArray(pair.Value)).ToArray());

        internal static void Validate<TChara, TLevel>(
            IClearData<TChara, TLevel> expected, IClearData<TChara, TLevel> actual)
            where TChara : struct, Enum
            where TLevel : struct, Enum
        {
            foreach (var pair in expected.CardsForDeck)
            {
                CardForDeckTests.Validate(pair.Value, actual.CardsForDeck[pair.Key]);
            }

            foreach (var pair in expected.SpellCardResults)
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
                var stub = MakeValidStub<TChara, TLevel>();

                var clearData = TestUtils.Create<ClearData<TChara, TLevel>>(MakeByteArray(stub));

                Validate(stub, clearData);
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
                var stub = MakeValidStub<TChara, TLevel>();
                var array = MakeByteArray(stub).SkipLast(1).ToArray();

                _ = TestUtils.Create<ClearData<TChara, TLevel>>(array);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void ReadFromTestExceededHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = MakeValidStub<TChara, TLevel>();
                var array = MakeByteArray(stub).Concat(new byte[1] { 1 }).ToArray();

                var clearData = TestUtils.Create<ClearData<TChara, TLevel>>(array);

                Validate(stub, clearData);
            });

        internal static void ReadFromTestDuplicatedHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = MakeValidStub<TChara, TLevel>();
                var array = TestUtils.MakeByteArray(
                    stub.CardsForDeck.Count + 1,
                    stub.CardsForDeck
                        .SelectMany(pair => CardForDeckTests.MakeByteArray(pair.Value)).ToArray(),
                    CardForDeckTests.MakeByteArray(stub.CardsForDeck.First().Value),
                    stub.SpellCardResults.Count + 1,
                    stub.SpellCardResults
                        .SelectMany(pair => SpellCardResultTests.MakeByteArray(pair.Value)).ToArray(),
                    SpellCardResultTests.MakeByteArray(stub.SpellCardResults.First().Value));

                var clearData = TestUtils.Create<ClearData<TChara, TLevel>>(array);

                Validate(stub, clearData);
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
