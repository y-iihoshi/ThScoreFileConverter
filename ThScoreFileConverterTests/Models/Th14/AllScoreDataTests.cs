using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th14;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;
using IStatus = ThScoreFileConverter.Models.Th125.IStatus;

namespace ThScoreFileConverterTests.Models.Th14
{
    [TestClass]
    public class AllScoreDataTests
    {
        [TestMethod]
        public void AllScoreDataTest()
        {
            Th13.AllScoreDataTests.AllScoreDataTestHelper<
                CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice, IScoreData, IStatus>();
        }

        [TestMethod]
        public void SetHeaderTest()
        {
            Th13.AllScoreDataTests.SetHeaderTestHelper<
                CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice, IScoreData, IStatus>();
        }

        [TestMethod]
        public void SetHeaderTestTwice()
        {
            Th13.AllScoreDataTests.SetHeaderTestTwiceHelper<
                CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice, IScoreData, IStatus>();
        }

        [TestMethod]
        public void SetClearDataTest()
        {
            Th13.AllScoreDataTests.SetClearDataTestHelper<
                CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice, IScoreData, IStatus>();
        }

        [TestMethod]
        public void SetClearDataTestTwice()
        {
            Th13.AllScoreDataTests.SetClearDataTestTwiceHelper<
                CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice, IScoreData, IStatus>();
        }

        [TestMethod]
        public void SetStatusTest()
        {
            Th13.AllScoreDataTests.SetStatusTestHelper<
                CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice, IScoreData, IStatus>();
        }

        [TestMethod]
        public void SetStatusTestTwice()
        {
            Th13.AllScoreDataTests.SetStatusTestTwiceHelper<
                CharaWithTotal, Level, LevelPractice, LevelPracticeWithTotal, StagePractice, IScoreData, IStatus>();
        }
    }
}
