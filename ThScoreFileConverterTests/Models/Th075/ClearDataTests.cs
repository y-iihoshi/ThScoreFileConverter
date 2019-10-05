using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th075;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th075.Stubs;

namespace ThScoreFileConverterTests.Models.Th075
{
    [TestClass]
    public class ClearDataTests
    {
        internal static ClearDataStub DefaultStub => new ClearDataStub()
        {
            UseCount = default,
            ClearCount = default,
            MaxCombo = default,
            MaxDamage = default,
            MaxBonuses = new List<int>(),
            CardGotCount = new List<short>(),
            CardTrialCount = new List<short>(),
            CardTrulyGot = new List<byte>(),
            Ranking = new List<IHighScore>()
        };

        internal static ClearDataStub ValidStub => new ClearDataStub()
        {
            UseCount = 1234,
            ClearCount = 2345,
            MaxCombo = 3456,
            MaxDamage = 4567,
            MaxBonuses = TestUtils.MakeRandomArray<int>(100).ToList(),
            CardGotCount = TestUtils.MakeRandomArray<short>(100).ToList(),
            CardTrialCount = TestUtils.MakeRandomArray<short>(100).ToList(),
            CardTrulyGot = TestUtils.MakeRandomArray<byte>(100).ToList(),
            Ranking = Enumerable.Range(0, 10)
                .Select(index => new HighScoreStub()
                {
                    EncodedName = new byte[] { 15, 37, 26, 50, 30, 43, (byte)(52 + index), 103 },
                    Name = Utils.Format("Player{0} ", index),
                    Month = (byte)(1 + index),
                    Day = (byte)(10 + index),
                    Score = 1234567 + index
                } as IHighScore).ToList()
        };

        internal static byte[] MakeByteArray(IClearData clearData)
            => TestUtils.MakeByteArray(
                clearData.UseCount,
                clearData.ClearCount,
                clearData.MaxCombo,
                clearData.MaxDamage,
                clearData.MaxBonuses.ToArray(),
                new byte[0xC8],
                clearData.CardGotCount.ToArray(),
                new byte[0x64],
                clearData.CardTrialCount.ToArray(),
                new byte[0x64],
                clearData.CardTrulyGot.ToArray(),
                new byte[0x38],
                clearData.Ranking.SelectMany(
                    element => HighScoreTests.MakeByteArray(element as HighScoreStub)).ToArray());

        internal static void Validate(IClearData expected, IClearData actual)
        {
            Assert.AreEqual(expected.UseCount, actual.UseCount);
            Assert.AreEqual(expected.ClearCount, actual.ClearCount);
            Assert.AreEqual(expected.MaxCombo, actual.MaxCombo);
            Assert.AreEqual(expected.MaxDamage, actual.MaxDamage);
            CollectionAssert.That.AreEqual(expected.MaxBonuses, actual.MaxBonuses);
            CollectionAssert.That.AreEqual(expected.CardGotCount, actual.CardGotCount);
            CollectionAssert.That.AreEqual(expected.CardTrialCount, actual.CardTrialCount);
            CollectionAssert.That.AreEqual(expected.CardTrulyGot, actual.CardTrulyGot);
            Assert.AreEqual(expected.Ranking.Count, actual.Ranking.Count);
            foreach (var index in Enumerable.Range(0, expected.Ranking.Count))
            {
                var highScoreStub = expected.Ranking[index];
                var highScore = actual.Ranking[index];
                Assert.AreEqual(highScoreStub.Name, highScore.Name);
                Assert.AreEqual(highScoreStub.Month, highScore.Month);
                Assert.AreEqual(highScoreStub.Day, highScore.Day);
                Assert.AreEqual(highScoreStub.Score, highScore.Score);
            }
        }

        [TestMethod]
        public void ClearDataTest() => TestUtils.Wrap(() =>
        {
            var clearData = new ClearData();

            Validate(DefaultStub, clearData);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var clearData = TestUtils.Create<ClearData>(MakeByteArray(ValidStub));

            Validate(ValidStub, clearData);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var clearData = new ClearData();
            clearData.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
