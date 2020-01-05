using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverterTests.Models.Th128.Stubs;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class RouteExReplacerTests
    {
        internal static IReadOnlyDictionary<RouteWithTotal, IClearData> ClearDataDictionary { get; } =
            new List<IClearData>
            {
                new ClearDataStub
                {
                    Route = RouteWithTotal.A2,
                    TotalPlayCount = 23,
                    PlayTime = 4567890,
                    ClearCounts = Utils.GetEnumerator<Level>().ToDictionary(level => level, level => 100 - (int)level),
                },
                new ClearDataStub
                {
                    Route = RouteWithTotal.B1,
                    TotalPlayCount = 12,
                    PlayTime = 3456789,
                    ClearCounts = Utils.GetEnumerator<Level>().ToDictionary(level => level, level => 50 - (int)level),
                },
            }.ToDictionary(element => element.Route);

        [TestMethod]
        public void RouteExReplacerTest()
        {
            var replacer = new RouteExReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RouteExReplacerTestNull()
        {
            _ = new RouteExReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

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
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Route = RouteWithTotal.A2,
                    ClearCounts = new Dictionary<Level, int>(),
                },
            }.ToDictionary(element => element.Route);

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
