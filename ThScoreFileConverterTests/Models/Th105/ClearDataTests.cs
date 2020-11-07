using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th105;
using ThScoreFileConverterTests.Models.Th105.Stubs;
using Level = ThScoreFileConverter.Models.Th105.Level;

namespace ThScoreFileConverterTests.Models.Th105
{
    [TestClass]
    public class ClearDataTests
    {
        internal static ClearDataStub<TChara> MakeValidStub<TChara>()
            where TChara : struct, Enum
        {
            static ISpellCardResult<TChara> CreateSpellCardResult(TChara chara, int index)
            {
#if false
                return Mock.Of<ISpellCardResult<TChara>>(
                    m => (m.Enemy == chara)
                         && (m.Level == Level.Normal)
                         && (m.Id == index + 1)
                         && (m.TrialCount == index * 100)
                         && (m.GotCount == index * 50)
                         && (m.Frames == 8901u - (uint)index));
#else
                var mock = new Mock<ISpellCardResult<TChara>>();
                _ = mock.SetupGet(m => m.Enemy).Returns(chara);
                _ = mock.SetupGet(m => m.Level).Returns(Level.Normal);
                _ = mock.SetupGet(m => m.Id).Returns(index + 1);
                _ = mock.SetupGet(m => m.TrialCount).Returns(index * 100);
                _ = mock.SetupGet(m => m.GotCount).Returns(index * 50);
                _ = mock.SetupGet(m => m.Frames).Returns(8901u - (uint)index);
                return mock.Object;
#endif
            }

            return new ClearDataStub<TChara>()
            {
                CardsForDeck = Enumerable.Range(1, 10)
                    .Select(value => Mock.Of<ICardForDeck>(m => (m.Id == value) && (m.MaxNumber == (value % 4) + 1)))
                    .ToDictionary(card => card.Id),
                SpellCardResults = Utils.GetEnumerable<TChara>()
                    .Select((chara, index) => CreateSpellCardResult(chara, index))
                    .ToDictionary(result => (result.Enemy, result.Id)),
            };
        }

        internal static byte[] MakeByteArray<TChara>(IClearData<TChara> properties)
            where TChara : struct, Enum
            => TestUtils.MakeByteArray(
                properties.CardsForDeck.Count,
                properties.CardsForDeck
                    .SelectMany(pair => CardForDeckTests.MakeByteArray(pair.Value)).ToArray(),
                properties.SpellCardResults.Count,
                properties.SpellCardResults
                    .SelectMany(pair => SpellCardResultTests.MakeByteArray(pair.Value)).ToArray());

        internal static void Validate<TChara>(IClearData<TChara> expected, IClearData<TChara> actual)
            where TChara : struct, Enum
        {
            foreach (var pair in expected.CardsForDeck)
            {
                CardForDeckTests.Validate(pair.Value, actual.CardsForDeck[pair.Key]);
            }

            foreach (var pair in expected.SpellCardResults)
            {
                SpellCardResultTests.Validate(pair.Value, actual.SpellCardResults[(pair.Key.Chara, pair.Key.CardId)]);
            }
        }

        internal static void ClearDataTestHelper<TChara>()
            where TChara : struct, Enum
        {
            var clearData = new ClearData<TChara>();

            Assert.AreEqual(0, clearData.CardsForDeck.Count);
            Assert.AreEqual(0, clearData.SpellCardResults.Count);
        }

        internal static void ReadFromTestHelper<TChara>()
            where TChara : struct, Enum
        {
            var stub = MakeValidStub<TChara>();

            var clearData = TestUtils.Create<ClearData<TChara>>(MakeByteArray(stub));

            Validate(stub, clearData);
        }

        internal static void ReadFromTestNullHelper<TChara>()
            where TChara : struct, Enum
        {
            var clearData = new ClearData<TChara>();
            clearData.ReadFrom(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        internal static void ReadFromTestShortenedHelper<TChara>()
            where TChara : struct, Enum
        {
            var stub = MakeValidStub<TChara>();
            var array = MakeByteArray(stub).SkipLast(1).ToArray();

            _ = TestUtils.Create<ClearData<TChara>>(array);

            Assert.Fail(TestUtils.Unreachable);
        }

        internal static void ReadFromTestExceededHelper<TChara>()
            where TChara : struct, Enum
        {
            var stub = MakeValidStub<TChara>();
            var array = MakeByteArray(stub).Concat(new byte[1] { 1 }).ToArray();

            var clearData = TestUtils.Create<ClearData<TChara>>(array);

            Validate(stub, clearData);
        }

        internal static void ReadFromTestDuplicatedHelper<TChara>()
            where TChara : struct, Enum
        {
            var stub = MakeValidStub<TChara>();
            var array = TestUtils.MakeByteArray(
                stub.CardsForDeck.Count + 1,
                stub.CardsForDeck.SelectMany(pair => CardForDeckTests.MakeByteArray(pair.Value)).ToArray(),
                CardForDeckTests.MakeByteArray(stub.CardsForDeck.First().Value),
                stub.SpellCardResults.Count + 1,
                stub.SpellCardResults.SelectMany(pair => SpellCardResultTests.MakeByteArray(pair.Value)).ToArray(),
                SpellCardResultTests.MakeByteArray(stub.SpellCardResults.First().Value));

            var clearData = TestUtils.Create<ClearData<TChara>>(array);

            Validate(stub, clearData);
        }

        [TestMethod]
        public void ClearDataTest() => ClearDataTestHelper<Chara>();

        [TestMethod]
        public void ReadFromTest() => ReadFromTestHelper<Chara>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => ReadFromTestNullHelper<Chara>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortened() => ReadFromTestShortenedHelper<Chara>();

        [TestMethod]
        public void ReadFromTestExceeded() => ReadFromTestExceededHelper<Chara>();

        [TestMethod]
        public void ReadFromTestDuplicated() => ReadFromTestDuplicatedHelper<Chara>();
    }
}
