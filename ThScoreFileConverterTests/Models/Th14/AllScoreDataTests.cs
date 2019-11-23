using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th14;

namespace ThScoreFileConverterTests.Models.Th14
{
    [TestClass]
    public class AllScoreDataTests
    {
        [TestMethod]
        public void Th14AllScoreDataTest()
            => Th13.AllScoreDataTests.AllScoreDataTestHelper<
                CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>();

        [TestMethod]
        public void Th14AllScoreDataSetHeaderTest()
            => Th13.AllScoreDataTests.SetHeaderTestHelper<
                CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>();

        [TestMethod]
        public void Th14AllScoreDataSetHeaderTestTwice()
            => Th13.AllScoreDataTests.SetHeaderTestTwiceHelper<
                CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>();

        [TestMethod]
        public void Th14AllScoreDataSetClearDataTest()
            => Th13.AllScoreDataTests.SetClearDataTestHelper<
                CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>();

        [TestMethod]
        public void Th14AllScoreDataSetClearDataTestTwice()
            => Th13.AllScoreDataTests.SetClearDataTestTwiceHelper<
                CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>();

        [TestMethod]
        public void Th14AllScoreDataSetStatusTest()
            => Th13.AllScoreDataTests.SetStatusTestHelper<
                CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>();

        [TestMethod]
        public void Th14AllScoreDataSetStatusTestTwice()
            => Th13.AllScoreDataTests.SetStatusTestTwiceHelper<
                CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice>();
    }
}
