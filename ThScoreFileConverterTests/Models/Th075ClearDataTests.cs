using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th075ClearDataTests
    {
        [TestMethod()]
        public void Th075ClearDataTest()
        {
            try
            {
                var clearData = new Th075ClearDataWrapper();

                Assert.AreEqual(default, clearData.UseCount.Value);
                Assert.AreEqual(default, clearData.ClearCount.Value);
                Assert.AreEqual(default, clearData.MaxCombo.Value);
                Assert.AreEqual(default, clearData.MaxDamage.Value);
                Assert.AreEqual(0, clearData.MaxBonuses.Count);
                Assert.AreEqual(0, clearData.CardGotCount.Count);
                Assert.AreEqual(0, clearData.CardTrialCount.Count);
                Assert.AreEqual(0, clearData.CardTrulyGot.Count);
                Assert.AreEqual(0, clearData.RankingCount);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        internal static void ReadFromTestHelper(Th075ClearDataWrapper clearData, byte[] array)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    clearData.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }

        [TestMethod()]
        public void ReadFromTest()
        {
            try
            {
                var clearData = new Th075ClearDataWrapper();
                var useCount = 1234;
                var clearCount = 2345;
                var maxCombo = 3456;
                var maxDamage = 4567;
                var maxBonuses = TestUtils.MakeRandomArray<int>(100);
                var unknown1 = new byte[0xC8];
                var cardGotCount = TestUtils.MakeRandomArray<short>(100);
                var unknown2 = new byte[0x64];
                var cardTrialCount = TestUtils.MakeRandomArray<short>(100);
                var unknown3 = new byte[0x64];
                var cardTrulyGot = TestUtils.MakeRandomArray<byte>(100);
                var unknown4 = new byte[0x38];
                var months = new byte[10];
                var days = new byte[10];
                var scores = new int[10];
                var ranking = new List<byte>();

                foreach (var index in Enumerable.Range(0, 10))
                {
                    var name = new byte[] { 15, 37, 26, 50, 30, 43, (byte)(52 + index), 103 };
                    months[index] = (byte)(1 + index);
                    days[index] = (byte)(10 + index);
                    var unknown = new byte[2];
                    scores[index] = 1234567 + index;
                    ranking.AddRange(
                        TestUtils.MakeByteArray(name, months[index], days[index], unknown, scores[index]));
                }

                ReadFromTestHelper(
                    clearData,
                    TestUtils.MakeByteArray(
                        useCount,
                        clearCount,
                        maxCombo,
                        maxDamage,
                        maxBonuses,
                        unknown1,
                        cardGotCount,
                        unknown2,
                        cardTrialCount,
                        unknown3,
                        cardTrulyGot,
                        unknown4,
                        ranking.ToArray()));

                Assert.AreEqual(useCount, clearData.UseCount);
                Assert.AreEqual(clearCount, clearData.ClearCount);
                Assert.AreEqual(maxCombo, clearData.MaxCombo);
                Assert.AreEqual(maxDamage, clearData.MaxDamage);
                CollectionAssert.AreEqual(maxBonuses, clearData.MaxBonuses.ToArray());
                CollectionAssert.AreEqual(cardGotCount, clearData.CardGotCount.ToArray());
                CollectionAssert.AreEqual(cardTrialCount, clearData.CardTrialCount.ToArray());
                CollectionAssert.AreEqual(cardTrulyGot, clearData.CardTrulyGot.ToArray());
                Assert.AreEqual(10, clearData.RankingCount);
                foreach (var index in Enumerable.Range(0, 10))
                {
                    var highScore = clearData.RankingItem(index);
                    Assert.AreEqual(Utils.Format("Player{0} ", index), highScore.Name);
                    Assert.AreEqual(months[index], highScore.Month);
                    Assert.AreEqual(days[index], highScore.Day);
                    Assert.AreEqual(scores[index], highScore.Score);
                }
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull()
        {
            try
            {
                var clearData = new Th075ClearDataWrapper();
                clearData.ReadFrom(null);
                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

    }
}
