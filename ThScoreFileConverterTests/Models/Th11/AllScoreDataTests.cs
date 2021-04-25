using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th11;
using StageProgress = ThScoreFileConverter.Models.Th10.StageProgress;

namespace ThScoreFileConverterTests.Models.Th11
{
    [TestClass]
    public class AllScoreDataTests
    {
        [TestMethod]
        public void AllScoreDataTest()
        {
            Th10.AllScoreDataTests.AllScoreDataTestHelper<CharaWithTotal, StageProgress>();
        }

        [TestMethod]
        public void SetHeaderTest()
        {
            Th10.AllScoreDataTests.SetHeaderTestHelper<CharaWithTotal, StageProgress>();
        }

        [TestMethod]
        public void SetHeaderTestTwice()
        {
            Th10.AllScoreDataTests.SetHeaderTestTwiceHelper<CharaWithTotal, StageProgress>();
        }

        [TestMethod]
        public void SetClearDataTest()
        {
            Th10.AllScoreDataTests.SetClearDataTestHelper<CharaWithTotal, StageProgress>();
        }

        [TestMethod]
        public void SetClearDataTestTwice()
        {
            Th10.AllScoreDataTests.SetClearDataTestTwiceHelper<CharaWithTotal, StageProgress>();
        }

        [TestMethod]
        public void SetStatusTest()
        {
            Th10.AllScoreDataTests.SetStatusTestHelper<CharaWithTotal, StageProgress>();
        }

        [TestMethod]
        public void SetStatusTestTwice()
        {
            Th10.AllScoreDataTests.SetStatusTestTwiceHelper<CharaWithTotal, StageProgress>();
        }
    }
}
