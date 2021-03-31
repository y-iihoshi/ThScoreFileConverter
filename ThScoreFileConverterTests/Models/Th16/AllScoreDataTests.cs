using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th16;

namespace ThScoreFileConverterTests.Models.Th16
{
    [TestClass]
    public class AllScoreDataTests
    {
        [TestMethod]
        public void AllScoreDataTest()
            => Th13.AllScoreDataTests.AllScoreDataTestHelper<
                CharaWithTotal, Level, Level, LevelWithTotal, StagePractice, IScoreData>();

        [TestMethod]
        public void SetHeaderTest()
            => Th13.AllScoreDataTests.SetHeaderTestHelper<
                CharaWithTotal, Level, Level, LevelWithTotal, StagePractice, IScoreData>();

        [TestMethod]
        public void SetHeaderTestTwice()
            => Th13.AllScoreDataTests.SetHeaderTestTwiceHelper<
                CharaWithTotal, Level, Level, LevelWithTotal, StagePractice, IScoreData>();

        [TestMethod]
        public void SetClearDataTest()
            => Th13.AllScoreDataTests.SetClearDataTestHelper<
                CharaWithTotal, Level, Level, LevelWithTotal, StagePractice, IScoreData>();

        [TestMethod]
        public void SetClearDataTestTwice()
            => Th13.AllScoreDataTests.SetClearDataTestTwiceHelper<
                CharaWithTotal, Level, Level, LevelWithTotal, StagePractice, IScoreData>();

        [TestMethod]
        public void SetStatusTest()
            => Th13.AllScoreDataTests.SetStatusTestHelper<
                CharaWithTotal, Level, Level, LevelWithTotal, StagePractice, IScoreData>();

        [TestMethod]
        public void SetStatusTestTwice()
            => Th13.AllScoreDataTests.SetStatusTestTwiceHelper<
                CharaWithTotal, Level, Level, LevelWithTotal, StagePractice, IScoreData>();
    }
}
