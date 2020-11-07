using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th165;
using ThScoreFileConverterTests.Models.Th095;
using ThScoreFileConverterTests.Models.Th165.Stubs;
using HeaderBase = ThScoreFileConverter.Models.Th095.HeaderBase;

namespace ThScoreFileConverterTests.Models.Th165
{
    [TestClass]
    public class AllScoreDataTests
    {
        [TestMethod]
        public void AllScoreDataTest()
        {
            var allScoreData = new AllScoreData();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.Scores.Count);
            Assert.IsNull(allScoreData.Status);
        }

        [TestMethod]
        public void SetHeaderTest()
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
            var header = TestUtils.Create<HeaderBase>(array);

            var allScoreData = new AllScoreData();
            allScoreData.Set(header);

            Assert.AreSame(header, allScoreData.Header);
        }

        [TestMethod]
        public void SetHeaderTestTwice()
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
            var header1 = TestUtils.Create<HeaderBase>(array);
            var header2 = TestUtils.Create<HeaderBase>(array);

            var allScoreData = new AllScoreData();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1, allScoreData.Header);
            Assert.AreSame(header2, allScoreData.Header);
        }

        [TestMethod]
        public void SetClearDataTest()
        {
            var score = Mock.Of<IScore>();

            var allScoreData = new AllScoreData();
            allScoreData.Set(score);

            Assert.AreSame(score, allScoreData.Scores[0]);
        }

        [TestMethod]
        public void SetClearDataTestTwice()
        {
            var score1 = Mock.Of<IScore>();
            var score2 = Mock.Of<IScore>();

            var allScoreData = new AllScoreData();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreSame(score1, allScoreData.Scores[0]);
            Assert.AreSame(score2, allScoreData.Scores[1]);
        }

        [TestMethod]
        public void SetStatusTest()
        {
            var status = new StatusStub();

            var allScoreData = new AllScoreData();
            allScoreData.Set(status);

            Assert.AreSame(status, allScoreData.Status);
        }

        [TestMethod]
        public void SetStatusTestTwice()
        {
            var status1 = new StatusStub();
            var status2 = new StatusStub();

            var allScoreData = new AllScoreData();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1, allScoreData.Status);
            Assert.AreSame(status2, allScoreData.Status);
        }
    }
}
