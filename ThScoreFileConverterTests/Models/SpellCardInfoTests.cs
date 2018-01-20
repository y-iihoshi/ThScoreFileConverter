using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ThScoreFileConverter.Models.Tests
{
    using CardInfo = SpellCardInfo<ThConverter.Stage, ThConverter.Level>;
    using Level = ThConverter.Level;
    using Stage = ThConverter.Stage;

    [TestClass()]
    public class SpellCardInfoTests
    {
        [TestMethod()]
        public void SpellCardInfoTest()
        {
            var info = new CardInfo(1, "月符「ムーンライトレイ」", Stage.St1, Level.Hard, Level.Lunatic);

            Assert.AreEqual(1, info.Id);
            Assert.AreEqual("月符「ムーンライトレイ」", info.Name);
            Assert.AreEqual(Stage.St1, info.Stage);
            Assert.AreEqual(Level.Hard, info.Level);
            CollectionAssert.AreEqual(new Level[]{ Level.Hard, Level.Lunatic }, info.Levels);
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "info")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SpellCardInfoTestNegativeId()
        {
            var info = new CardInfo(-1, "月符「ムーンライトレイ」", Stage.St1, Level.Hard, Level.Lunatic);
            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "info")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SpellCardInfoTestZeroId()
        {
            var info = new CardInfo(0, "月符「ムーンライトレイ」", Stage.St1, Level.Hard, Level.Lunatic);
            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "info")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SpellCardInfoTestNullName()
        {
            var info = new CardInfo(1, null, Stage.St1, Level.Hard, Level.Lunatic);
            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "info")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void SpellCardInfoTestEmptyName()
        {
            var info = new CardInfo(1, string.Empty, Stage.St1, Level.Hard, Level.Lunatic);
            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "info")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SpellCardInfoTestInvalidStage()
        {
            var invalid = (Stage)(Enum.GetValues(typeof(Stage)).Cast<int>().Max() + 1);
            var info = new CardInfo(1, "月符「ムーンライトレイ」", invalid, Level.Hard, Level.Lunatic);
            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "info")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SpellCardInfoTestInvalidLevel()
        {
            var invalid = (Level)(Enum.GetValues(typeof(Level)).Cast<int>().Max() + 1);
            var info = new CardInfo(1, "月符「ムーンライトレイ」", Stage.St1, Level.Hard, invalid);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod()]
        public void SpellCardInfoTestOneLevel()
        {
            var info = new CardInfo(1, "霜符「フロストコラムス」", Stage.St1, Level.Hard);

            Assert.AreEqual(1, info.Id);
            Assert.AreEqual("霜符「フロストコラムス」", info.Name);
            Assert.AreEqual(Stage.St1, info.Stage);
            Assert.AreEqual(Level.Hard, info.Level);
            CollectionAssert.AreEqual(new Level[]{ Level.Hard }, info.Levels);
        }
    }
}
