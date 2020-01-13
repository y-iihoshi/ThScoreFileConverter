using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverterTests.Models.Th128.Stubs;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th128.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class ScoreReplacerTests
    {
        internal static IReadOnlyDictionary<RouteWithTotal, IClearData> ClearDataDictionary { get; } =
            new List<IClearData>
            {
                ClearDataTests.MakeValidStub(),
            }.ToDictionary(element => element.Route);

        [TestMethod]
        public void ScoreReplacerTest()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScoreReplacerTestNull()
        {
            _ = new ScoreReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ScoreReplacerTestEmpty()
        {
            var dictionary = new Dictionary<RouteWithTotal, IClearData>();
            var replacer = new ScoreReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("Player1", replacer.Replace("%T128SCRHA221"));
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("123,446,701", replacer.Replace("%T128SCRHA222"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("123446701", replacer.Replace("%T128SCRHA222"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestStage()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("A2 Clear", replacer.Replace("%T128SCRHA223"));
        }

        [TestMethod]
        public void ReplaceTestDateTime()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            var expected = new DateTime(1970, 1, 1).AddSeconds(34567890).ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
            Assert.AreEqual(expected, replacer.Replace("%T128SCRHA224"));
        }

        [TestMethod]
        public void ReplaceTestSlowRate()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("1.200%", replacer.Replace("%T128SCRHA225"));  // really...?
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<RouteWithTotal, IClearData>();
            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("--------", replacer.Replace("%T128SCRHA221"));
            Assert.AreEqual("0", replacer.Replace("%T128SCRHA222"));
            Assert.AreEqual("-------", replacer.Replace("%T128SCRHA223"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T128SCRHA224"));
            Assert.AreEqual("-----%", replacer.Replace("%T128SCRHA225"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub()
                {
                    Route = RouteWithTotal.A2,
                    Rankings = new Dictionary<Level, IReadOnlyList<IScoreData>>(),
                },
            }.ToDictionary(element => element.Route);

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("--------", replacer.Replace("%T128SCRHA221"));
            Assert.AreEqual("0", replacer.Replace("%T128SCRHA222"));
            Assert.AreEqual("-------", replacer.Replace("%T128SCRHA223"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T128SCRHA224"));
            Assert.AreEqual("-----%", replacer.Replace("%T128SCRHA225"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRanking()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub()
                {
                    Route = RouteWithTotal.A2,
                    Rankings = Utils.GetEnumerable<Level>().ToDictionary(
                        level => level,
                        level => new List<IScoreData>() as IReadOnlyList<IScoreData>),
                },
            }.ToDictionary(element => element.Route);

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("--------", replacer.Replace("%T128SCRHA221"));
            Assert.AreEqual("0", replacer.Replace("%T128SCRHA222"));
            Assert.AreEqual("-------", replacer.Replace("%T128SCRHA223"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T128SCRHA224"));
            Assert.AreEqual("-----%", replacer.Replace("%T128SCRHA225"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtra()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128SCRXA223", replacer.Replace("%T128SCRXA223"));
        }

        [TestMethod]
        public void ReplaceTestRouteExtra()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128SCRHEX23", replacer.Replace("%T128SCRHEX23"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128XXXHA221", replacer.Replace("%T128XXXHA221"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128SCRYA221", replacer.Replace("%T128SCRYA221"));
        }

        [TestMethod]
        public void ReplaceTestInvalidRoute()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128SCRHXX21", replacer.Replace("%T128SCRHXX21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidRank()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128SCRHA2X1", replacer.Replace("%T128SCRHA2X1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T128SCRHA22X", replacer.Replace("%T128SCRHA22X"));
        }
    }
}
