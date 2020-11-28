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
    public class RouteExReplacerTests
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

        [TestMethod]
        public void RouteExReplacerTest()
        {
            var replacer = new RouteExReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void RouteExReplacerTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new RouteExReplacer(null!));

        [TestMethod]
        public void RouteExReplacerTestEmpty()
        {
            var dictionary = new Dictionary<RouteWithTotal, IClearData>();
            var replacer = new RouteExReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestTotalPlayCount()
        {
            var replacer = new RouteExReplacer(ClearDataDictionary);
            Assert.AreEqual("23", replacer.Replace("%T128ROUTEEXHA21"));
        }

        [TestMethod]
        public void ReplaceTestPlayTime()
        {
            var replacer = new RouteExReplacer(ClearDataDictionary);
            Assert.AreEqual("21:08:51", replacer.Replace("%T128ROUTEEXHA22"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var replacer = new RouteExReplacer(ClearDataDictionary);
            Assert.AreEqual("98", replacer.Replace("%T128ROUTEEXHA23"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalTotalPlayCount()
        {
            var replacer = new RouteExReplacer(ClearDataDictionary);
            Assert.AreEqual("23", replacer.Replace("%T128ROUTEEXTA21"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalPlayTime()
        {
            var replacer = new RouteExReplacer(ClearDataDictionary);
            Assert.AreEqual("21:08:51", replacer.Replace("%T128ROUTEEXTA22"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalClearCount()
        {
            var replacer = new RouteExReplacer(ClearDataDictionary);
            Assert.AreEqual("490", replacer.Replace("%T128ROUTEEXTA23"));
        }

        [TestMethod]
        public void ReplaceTestRouteTotalTotalPlayCount()
        {
            var replacer = new RouteExReplacer(ClearDataDictionary);
            Assert.AreEqual("35", replacer.Replace("%T128ROUTEEXHTL1"));
        }

        [TestMethod]
        public void ReplaceTestRouteTotalPlayTime()
        {
            var replacer = new RouteExReplacer(ClearDataDictionary);
            Assert.AreEqual("37:09:04", replacer.Replace("%T128ROUTEEXHTL2"));
        }

        [TestMethod]
        public void ReplaceTestRouteTotalClearCount()
        {
            var replacer = new RouteExReplacer(ClearDataDictionary);
            Assert.AreEqual("146", replacer.Replace("%T128ROUTEEXHTL3"));
        }

        [TestMethod]
        public void ReplaceTestTotalTotalPlayCount()
        {
            var replacer = new RouteExReplacer(ClearDataDictionary);
            Assert.AreEqual("35", replacer.Replace("%T128ROUTEEXTTL1"));
        }

        [TestMethod]
        public void ReplaceTestTotalPlayTime()
        {
            var replacer = new RouteExReplacer(ClearDataDictionary);
            Assert.AreEqual("37:09:04", replacer.Replace("%T128ROUTEEXTTL2"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var replacer = new RouteExReplacer(ClearDataDictionary);
            Assert.AreEqual("730", replacer.Replace("%T128ROUTEEXTTL3"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<RouteWithTotal, IClearData>();
            var replacer = new RouteExReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T128ROUTEEXHA21"));
            Assert.AreEqual("0:00:00", replacer.Replace("%T128ROUTEEXHA22"));
            Assert.AreEqual("0", replacer.Replace("%T128ROUTEEXHA23"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCounts()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Route == RouteWithTotal.A2) && (m.ClearCounts == new Dictionary<Level, int>()))
            }.ToDictionary(clearData => clearData.Route);

            var replacer = new RouteExReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T128ROUTEEXHA23"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtra()
        {
            var replacer = new RouteExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128ROUTEEXXA21", replacer.Replace("%T128ROUTEEXXA21"));
        }

        [TestMethod]
        public void ReplaceTestRouteExtra()
        {
            var replacer = new RouteExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128ROUTEEXHEX1", replacer.Replace("%T128ROUTEEXHEX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new RouteExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128XXXXXXXHA21", replacer.Replace("%T128XXXXXXXHA21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new RouteExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128ROUTEEXYA21", replacer.Replace("%T128ROUTEEXYA21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidRoute()
        {
            var replacer = new RouteExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128ROUTEEXHXX1", replacer.Replace("%T128ROUTEEXHXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new RouteExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128ROUTEEXHA2X", replacer.Replace("%T128ROUTEEXHA2X"));
        }
    }
}
