using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class RouteReplacerTests
    {
        private static IEnumerable<IClearData> CreateClearDataList()
        {
            var levels = Utils.GetEnumerable<Level>();
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

        [TestMethod]
        public void RouteReplacerTest()
        {
            var replacer = new RouteReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RouteReplacerTestNull()
        {
            _ = new RouteReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void RouteReplacerTestEmpty()
        {
            var dictionary = new Dictionary<RouteWithTotal, IClearData>();
            var replacer = new RouteReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestTotalPlayCount()
        {
            var replacer = new RouteReplacer(ClearDataDictionary);
            Assert.AreEqual("23", replacer.Replace("%T128ROUTEA21"));
        }

        [TestMethod]
        public void ReplaceTestPlayTime()
        {
            var replacer = new RouteReplacer(ClearDataDictionary);
            Assert.AreEqual("21:08:51", replacer.Replace("%T128ROUTEA22"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var replacer = new RouteReplacer(ClearDataDictionary);
            Assert.AreEqual("490", replacer.Replace("%T128ROUTEA23"));
        }

        [TestMethod]
        public void ReplaceTestRouteTotalTotalPlayCount()
        {
            var replacer = new RouteReplacer(ClearDataDictionary);
            Assert.AreEqual("35", replacer.Replace("%T128ROUTETL1"));
        }

        [TestMethod]
        public void ReplaceTestRouteTotalPlayTime()
        {
            var replacer = new RouteReplacer(ClearDataDictionary);
            Assert.AreEqual("37:09:04", replacer.Replace("%T128ROUTETL2"));
        }

        [TestMethod]
        public void ReplaceTestRouteTotalClearCount()
        {
            var replacer = new RouteReplacer(ClearDataDictionary);
            Assert.AreEqual("730", replacer.Replace("%T128ROUTETL3"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<RouteWithTotal, IClearData>();
            var replacer = new RouteReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T128ROUTEA21"));
            Assert.AreEqual("0:00:00", replacer.Replace("%T128ROUTEA22"));
            Assert.AreEqual("0", replacer.Replace("%T128ROUTEA23"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCounts()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Route == RouteWithTotal.A2) && (m.ClearCounts == new Dictionary<Level, int>()))
            }.ToDictionary(clearData => clearData.Route);

            var replacer = new RouteReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T128ROUTEA23"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new RouteReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128XXXXXA21", replacer.Replace("%T128XXXXXA21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidRoute()
        {
            var replacer = new RouteReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128ROUTEXX1", replacer.Replace("%T128ROUTEXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new RouteReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128ROUTEA2X", replacer.Replace("%T128ROUTEA2X"));
        }
    }
}
