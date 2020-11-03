using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverterTests.Models.Th128.Stubs;
using HeaderBase = ThScoreFileConverter.Models.Th095.HeaderBase;
using IStatus = ThScoreFileConverter.Models.Th125.IStatus;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class AllScoreDataTests
    {
        [TestMethod]
        public void AllScoreDataTest()
        {
            var allScoreData = new AllScoreData();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.ClearData.Count);
            Assert.IsNull(allScoreData.CardData);
            Assert.IsNull(allScoreData.Status);
        }

        [TestMethod]
        public void SetHeaderTest()
        {
            var array = Th095.HeaderBaseTests.MakeByteArray(Th095.HeaderBaseTests.ValidProperties);
            var header = TestUtils.Create<HeaderBase>(array);

            var allScoreData = new AllScoreData();
            allScoreData.Set(header);

            Assert.AreSame(header, allScoreData.Header);
        }

        [TestMethod]
        public void SetHeaderTestTwice()
        {
            var array = Th095.HeaderBaseTests.MakeByteArray(Th095.HeaderBaseTests.ValidProperties);
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
            var route = RouteWithTotal.A2;
            var clearData = new ClearDataStub { Route = route };

            var allScoreData = new AllScoreData();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData, allScoreData.ClearData[route]);
        }

        [TestMethod]
        public void SetClearDataTestTwice()
        {
            var route = RouteWithTotal.A2;
            var clearData1 = new ClearDataStub { Route = route };
            var clearData2 = new ClearDataStub { Route = route };

            var allScoreData = new AllScoreData();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1, allScoreData.ClearData[route]);
            Assert.AreNotSame(clearData2, allScoreData.ClearData[route]);
        }

        [TestMethod]
        public void SetCardDataTest()
        {
            var cardData = new CardDataStub();

            var allScoreData = new AllScoreData();
            allScoreData.Set(cardData);

            Assert.AreSame(cardData, allScoreData.CardData);
        }

        [TestMethod]
        public void SetCardDataTestTwice()
        {
            var cardData1 = new CardDataStub();
            var cardData2 = new CardDataStub();

            var allScoreData = new AllScoreData();
            allScoreData.Set(cardData1);
            allScoreData.Set(cardData2);

            Assert.AreNotSame(cardData1, allScoreData.CardData);
            Assert.AreSame(cardData2, allScoreData.CardData);
        }

        [TestMethod]
        public void SetStatusTest()
        {
            var status = Mock.Of<IStatus>();

            var allScoreData = new AllScoreData();
            allScoreData.Set(status);

            Assert.AreSame(status, allScoreData.Status);
        }

        [TestMethod]
        public void SetStatusTestTwice()
        {
            var status1 = Mock.Of<IStatus>();
            var status2 = Mock.Of<IStatus>();

            var allScoreData = new AllScoreData();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1, allScoreData.Status);
            Assert.AreSame(status2, allScoreData.Status);
        }
    }
}
