using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class RouteReplacerTests
    {
        private static IEnumerable<IClearData> CreateClearDataList()
        {
            var levels = EnumHelper<Level>.Enumerable;
            return new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Route == RouteWithTotal.A2)
                         && (m.TotalPlayCount == 23)
                         && (m.PlayTime == 4567890)
                         && (m.ClearCounts == levels.ToDictionary(level => level, level => 100 - (int)level))),
                Mock.Of<IClearData>(
                    m => (m.Route == RouteWithTotal.B1)
                         && (m.TotalPlayCount == 12)
                         && (m.PlayTime == 3456789)
                         && (m.ClearCounts == levels.ToDictionary(level => level, level => 50 - (int)level))),
            };
        }

        internal static IReadOnlyDictionary<RouteWithTotal, IClearData> ClearDataDictionary { get; } =
            CreateClearDataList().ToDictionary(clearData => clearData.Route);

        private static Mock<INumberFormatter> MockNumberFormatter()
        {
            var mock = new Mock<INumberFormatter>();
            _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
                .Returns((object value) => "invoked: " + value.ToString());
            return mock;
        }

        [TestMethod]
        public void RouteReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new RouteReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void RouteReplacerTestNull()
        {
            var formatterMock = MockNumberFormatter();
            _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new RouteReplacer(null!, formatterMock.Object));
        }

        [TestMethod]
        public void RouteReplacerTestEmpty()
        {
            var dictionary = new Dictionary<RouteWithTotal, IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new RouteReplacer(dictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestTotalPlayCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new RouteReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 23", replacer.Replace("%T128ROUTEA21"));
        }

        [TestMethod]
        public void ReplaceTestPlayTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new RouteReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("21:08:51", replacer.Replace("%T128ROUTEA22"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new RouteReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 490", replacer.Replace("%T128ROUTEA23"));
        }

        [TestMethod]
        public void ReplaceTestRouteTotalTotalPlayCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new RouteReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 35", replacer.Replace("%T128ROUTETL1"));
        }

        [TestMethod]
        public void ReplaceTestRouteTotalPlayTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new RouteReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("37:09:04", replacer.Replace("%T128ROUTETL2"));
        }

        [TestMethod]
        public void ReplaceTestRouteTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new RouteReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 730", replacer.Replace("%T128ROUTETL3"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<RouteWithTotal, IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new RouteReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T128ROUTEA21"));
            Assert.AreEqual("0:00:00", replacer.Replace("%T128ROUTEA22"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T128ROUTEA23"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCounts()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Route == RouteWithTotal.A2) && (m.ClearCounts == new Dictionary<Level, int>()))
            }.ToDictionary(clearData => clearData.Route);
            var formatterMock = MockNumberFormatter();

            var replacer = new RouteReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T128ROUTEA23"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new RouteReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T128XXXXXA21", replacer.Replace("%T128XXXXXA21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidRoute()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new RouteReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T128ROUTEXX1", replacer.Replace("%T128ROUTEXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new RouteReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T128ROUTEA2X", replacer.Replace("%T128ROUTEA2X"));
        }
    }
}
