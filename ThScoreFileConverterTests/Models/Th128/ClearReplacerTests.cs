using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th128.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class ClearReplacerTests
    {
        internal static IReadOnlyDictionary<RouteWithTotal, IClearData> ClearDataDictionary { get; } = new[]
        {
            Mock.Of<IClearData>(
                c => (c.Route == RouteWithTotal.A2)
                     && (c.Rankings == Utils.GetEnumerable<Level>().ToDictionary(
                        level => level,
                        level => Enumerable.Range(0, 10).Select(
                            index => Mock.Of<IScoreData>(
                                s => (s.StageProgress ==
                                        (level == Level.Extra ? StageProgress.Extra : (StageProgress)(5 - (index % 5))))
                                     && (s.DateTime == (uint)index % 2))).ToList() as IReadOnlyList<IScoreData>)))
        }.ToDictionary(clearData => clearData.Route);

        [TestMethod]
        public void ClearReplacerTest()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ClearReplacerTestNull()
        {
            _ = new ClearReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ClearReplacerTestEmpty()
        {
            var dictionary = new Dictionary<RouteWithTotal, IClearData>();
            var replacer = new ClearReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("Stage A2-3", replacer.Replace("%T128CLEARHA2"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtra()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128CLEARXA2", replacer.Replace("%T128CLEARXA2"));
        }

        [TestMethod]
        public void ReplaceTestRouteExtra()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128CLEARHEX", replacer.Replace("%T128CLEARHEX"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<RouteWithTotal, IClearData>();
            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T128CLEARHA2"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Route == RouteWithTotal.A2)
                         && (m.Rankings == new Dictionary<Level, IReadOnlyList<IScoreData>>()))
            }.ToDictionary(clearData => clearData.Route);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T128CLEARHA2"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRanking()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Route == RouteWithTotal.A2)
                         && (m.Rankings == Utils.GetEnumerable<Level>().ToDictionary(
                            level => level,
                            level => new List<IScoreData>() as IReadOnlyList<IScoreData>)))
            }.ToDictionary(clearData => clearData.Route);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T128CLEARHA2"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128XXXXXHA2", replacer.Replace("%T128XXXXXHA2"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128CLEARYA2", replacer.Replace("%T128CLEARYA2"));
        }

        [TestMethod]
        public void ReplaceTestInvalidRoute()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128CLEARHXX", replacer.Replace("%T128CLEARHXX"));
        }
    }
}
