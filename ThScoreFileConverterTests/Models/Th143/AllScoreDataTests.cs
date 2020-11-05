using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th143;
using ThScoreFileConverterTests.Models.Th095;
using ThScoreFileConverterTests.Models.Th143.Stubs;
using HeaderBase = ThScoreFileConverter.Models.Th095.HeaderBase;

namespace ThScoreFileConverterTests.Models.Th143
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
            Assert.AreEqual(0, allScoreData.ItemStatuses.Count);
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
        public void SetScoreTest()
        {
            var score = new ScoreStub();

            var allScoreData = new AllScoreData();
            allScoreData.Set(score);

            Assert.AreSame(score, allScoreData.Scores[0]);
        }

        [TestMethod]
        public void SetScoreTestTwice()
        {
            var score1 = new ScoreStub();
            var score2 = new ScoreStub();

            var allScoreData = new AllScoreData();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreSame(score1, allScoreData.Scores[0]);
            Assert.AreSame(score2, allScoreData.Scores[1]);
        }

        [TestMethod]
        public void SetItemStatusTest()
        {
            var item = ItemWithTotal.Fablic;
            var status = Mock.Of<IItemStatus>(m => m.Item == item);

            var allScoreData = new AllScoreData();
            allScoreData.Set(status);

            Assert.AreSame(status, allScoreData.ItemStatuses[item]);
        }

        [TestMethod]
        public void SetItemStatusTestTwice()
        {
            var item = ItemWithTotal.Fablic;
            var status1 = Mock.Of<IItemStatus>(m => m.Item == item);
            var status2 = Mock.Of<IItemStatus>(m => m.Item == item);

            var allScoreData = new AllScoreData();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreSame(status1, allScoreData.ItemStatuses[item]);
            Assert.AreNotSame(status2, allScoreData.ItemStatuses[item]);
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
