using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th06AllScoreDataTests
    {
        [TestMethod]
        public void Th06AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th06AllScoreDataWrapper();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.RankingsCount);
            Assert.AreEqual(0, allScoreData.ClearDataCount);
            Assert.AreEqual(0, allScoreData.CardAttacksCount);
            Assert.AreEqual(0, allScoreData.PracticeScoresCount);
        });

        [TestMethod]
        public void Th06AllScoreDataSetHeaderTest() => TestUtils.Wrap(() =>
        {
            var chapter = Th06ChapterWrapper.Create(
                Th06HeaderTests.MakeByteArray(Th06HeaderTests.GetValidProperties("TH6K")));
            var header = new Th06HeaderWrapper<Th06Converter>(chapter);

            var allScoreData = new Th06AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header.Target, allScoreData.Header.Target);
        });

        [TestMethod]
        public void Th06AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = Th06ChapterWrapper.Create(
                Th06HeaderTests.MakeByteArray(Th06HeaderTests.GetValidProperties("TH6K")));
            var header1 = new Th06HeaderWrapper<Th06Converter>(chapter);
            var header2 = new Th06HeaderWrapper<Th06Converter>(chapter);

            var allScoreData = new Th06AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1.Target, allScoreData.Header.Target);
            Assert.AreSame(header2.Target, allScoreData.Header.Target);
        });

        [TestMethod]
        public void Th06AllScoreDataSetHighScoreTest() => TestUtils.Wrap(() =>
        {
            var properties = Th06HighScoreTests.ValidProperties;
            properties.score = 876543u;
            var chapter = Th06ChapterWrapper.Create(Th06HighScoreTests.MakeByteArray(properties));
            var score = new Th06HighScoreWrapper(chapter);

            var allScoreData = new Th06AllScoreDataWrapper();
            allScoreData.Set(score);

            var pair = new Th06CharaLevelPairWrapper<Th06Converter, Th06Converter.Chara, ThConverter.Level>(
                properties.chara, properties.level);
            Assert.AreSame(score.Target, allScoreData.RankingItem(pair, 2).Target);
        });

        [TestMethod]
        public void Th06AllScoreDataSetHighScoreTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = Th06HighScoreTests.ValidProperties;
            properties.score = 876543u;
            var chapter = Th06ChapterWrapper.Create(Th06HighScoreTests.MakeByteArray(properties));
            var score1 = new Th06HighScoreWrapper(chapter);
            var score2 = new Th06HighScoreWrapper(chapter);

            var allScoreData = new Th06AllScoreDataWrapper();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            var pair = new Th06CharaLevelPairWrapper<Th06Converter, Th06Converter.Chara, ThConverter.Level>(
                properties.chara, properties.level);
            Assert.AreSame(score1.Target, allScoreData.RankingItem(pair, 2).Target);
            Assert.AreSame(score2.Target, allScoreData.RankingItem(pair, 3).Target);
        });

        [TestMethod]
        public void Th06AllScoreDataSetClearDataTest() => TestUtils.Wrap(() =>
        {
            var properties = Th06ClearDataTests.ValidProperties;
            var chapter = Th06ChapterWrapper.Create(Th06ClearDataTests.MakeByteArray(properties));
            var clearData = new Th06ClearDataWrapper(chapter);

            var allScoreData = new Th06AllScoreDataWrapper();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData.Target, allScoreData.ClearDataItem(properties.chara).Target);
        });

        [TestMethod]
        public void Th06AllScoreDataSetClearDataTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = Th06ClearDataTests.ValidProperties;
            var chapter = Th06ChapterWrapper.Create(Th06ClearDataTests.MakeByteArray(properties));
            var clearData1 = new Th06ClearDataWrapper(chapter);
            var clearData2 = new Th06ClearDataWrapper(chapter);

            var allScoreData = new Th06AllScoreDataWrapper();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1.Target, allScoreData.ClearDataItem(properties.chara).Target);
            Assert.AreNotSame(clearData2.Target, allScoreData.ClearDataItem(properties.chara).Target);
        });

        [TestMethod]
        public void Th06AllScoreDataSetCardAttackTest() => TestUtils.Wrap(() =>
        {
            var properties = Th06CardAttackTests.ValidProperties;
            var chapter = Th06ChapterWrapper.Create(Th06CardAttackTests.MakeByteArray(properties));
            var attack = new Th06CardAttackWrapper(chapter);

            var allScoreData = new Th06AllScoreDataWrapper();
            allScoreData.Set(attack);

            Assert.AreSame(attack.Target, allScoreData.CardAttacksItem(properties.cardId).Target);
        });

        [TestMethod]
        public void Th06AllScoreDataSetCardAttackTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = Th06CardAttackTests.ValidProperties;
            var chapter = Th06ChapterWrapper.Create(Th06CardAttackTests.MakeByteArray(properties));
            var attack1 = new Th06CardAttackWrapper(chapter);
            var attack2 = new Th06CardAttackWrapper(chapter);

            var allScoreData = new Th06AllScoreDataWrapper();
            allScoreData.Set(attack1);
            allScoreData.Set(attack2);

            Assert.AreSame(attack1.Target, allScoreData.CardAttacksItem(properties.cardId).Target);
            Assert.AreNotSame(attack2.Target, allScoreData.CardAttacksItem(properties.cardId).Target);
        });

        [TestMethod]
        public void Th06AllScoreDataSetPracticeScoreTest() => TestUtils.Wrap(() =>
        {
            var properties = Th06PracticeScoreTests.ValidProperties;
            properties.level = ThConverter.Level.Normal;
            properties.stage = ThConverter.Stage.St6;
            var chapter = Th06ChapterWrapper.Create(Th06PracticeScoreTests.MakeByteArray(properties));
            var score = new Th06PracticeScoreWrapper(chapter);

            var allScoreData = new Th06AllScoreDataWrapper();
            allScoreData.Set(score);

            var pair = new Th06CharaLevelPairWrapper<Th06Converter, Th06Converter.Chara, ThConverter.Level>(
                properties.chara, properties.level);
            Assert.AreSame(score.Target, allScoreData.PracticeScore(pair, properties.stage).Target);
        });

        [TestMethod]
        public void Th06AllScoreDataSetPracticeScoreTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = Th06PracticeScoreTests.ValidProperties;
            properties.level = ThConverter.Level.Normal;
            properties.stage = ThConverter.Stage.St6;
            var chapter = Th06ChapterWrapper.Create(Th06PracticeScoreTests.MakeByteArray(properties));
            var score1 = new Th06PracticeScoreWrapper(chapter);
            var score2 = new Th06PracticeScoreWrapper(chapter);

            var allScoreData = new Th06AllScoreDataWrapper();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            var pair = new Th06CharaLevelPairWrapper<Th06Converter, Th06Converter.Chara, ThConverter.Level>(
                properties.chara, properties.level);
            Assert.AreSame(score1.Target, allScoreData.PracticeScore(pair, properties.stage).Target);
            Assert.AreNotSame(score2.Target, allScoreData.PracticeScore(pair, properties.stage).Target);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(ThConverter.Level.Easy, ThConverter.Stage.St6)]
        [DataRow(ThConverter.Level.Extra, ThConverter.Stage.Extra)]
        [DataRow(ThConverter.Level.Extra, ThConverter.Stage.St6)]
        [DataRow(ThConverter.Level.Normal, ThConverter.Stage.Extra)]
        public void Th06AllScoreDataSetPracticeScoreTestInvalidPracticeStage(int level, int stage)
            => TestUtils.Wrap(() =>
            {
                var properties = Th06PracticeScoreTests.ValidProperties;
                properties.level = TestUtils.Cast<ThConverter.Level>(level);
                properties.stage = TestUtils.Cast<ThConverter.Stage>(stage);
                var chapter = Th06ChapterWrapper.Create(
                    Th06PracticeScoreTests.MakeByteArray(properties));
                var score = new Th06PracticeScoreWrapper(chapter);

                var allScoreData = new Th06AllScoreDataWrapper();
                allScoreData.Set(score);

                Assert.AreEqual(0, allScoreData.PracticeScoresCount);
            });
    }
}
