using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests.Models
{
    using CardInfo = SpellCardInfo<Stage, Level>;

    [TestClass]
    public class SpellCardInfoTests
    {
        [TestMethod]
        public void SpellCardInfoTest()
        {
            var info = new CardInfo(1, "月符「ムーンライトレイ」", Stage.One, Level.Hard, Level.Lunatic);

            Assert.AreEqual(1, info.Id);
            Assert.AreEqual("月符「ムーンライトレイ」", info.Name);
            Assert.AreEqual(Stage.One, info.Stage);
            Assert.AreEqual(Level.Hard, info.Level);
            CollectionAssert.That.AreEqual(new[] { Level.Hard, Level.Lunatic }, info.Levels);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SpellCardInfoTestNegativeId()
        {
            _ = new CardInfo(-1, "月符「ムーンライトレイ」", Stage.One, Level.Hard, Level.Lunatic);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SpellCardInfoTestZeroId()
        {
            _ = new CardInfo(0, "月符「ムーンライトレイ」", Stage.One, Level.Hard, Level.Lunatic);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SpellCardInfoTestNullName()
        {
            _ = new CardInfo(1, null, Stage.One, Level.Hard, Level.Lunatic);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SpellCardInfoTestEmptyName()
        {
            _ = new CardInfo(1, string.Empty, Stage.One, Level.Hard, Level.Lunatic);
            Assert.Fail(TestUtils.Unreachable);
        }

        public static IEnumerable<object[]> InvalidStages
            => TestUtils.GetInvalidEnumerators(typeof(Stage));

        [DataTestMethod]
        [DynamicData(nameof(InvalidStages))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SpellCardInfoTestInvalidStage(int stage)
        {
            var invalid = TestUtils.Cast<Stage>(stage);
            _ = new CardInfo(1, "月符「ムーンライトレイ」", invalid, Level.Hard, Level.Lunatic);
            Assert.Fail(TestUtils.Unreachable);
        }

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SpellCardInfoTestInvalidLevel(int level)
        {
            var invalid = TestUtils.Cast<Level>(level);
            _ = new CardInfo(1, "月符「ムーンライトレイ」", Stage.One, Level.Hard, invalid);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void SpellCardInfoTestOneLevel()
        {
            var info = new CardInfo(1, "霜符「フロストコラムス」", Stage.One, Level.Hard);

            Assert.AreEqual(1, info.Id);
            Assert.AreEqual("霜符「フロストコラムス」", info.Name);
            Assert.AreEqual(Stage.One, info.Stage);
            Assert.AreEqual(Level.Hard, info.Level);
            CollectionAssert.That.AreEqual(new[] { Level.Hard }, info.Levels);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SpellCardInfoTestZeroLevels()
        {
            _ = new CardInfo(1, "霜符「フロストコラムス」", Stage.One);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SpellCardInfoTestNullLevels()
        {
            _ = new CardInfo(1, "霜符「フロストコラムス」", Stage.One, null);
            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
