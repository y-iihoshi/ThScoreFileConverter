using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th075;
using ThScoreFileConverterTests.Models.Th075.Stubs;
using Level = ThScoreFileConverter.Models.Th075.Level;

namespace ThScoreFileConverterTests.Models.Th075
{
    [TestClass]
    public class CardReplacerTests
    {
        internal static IReadOnlyDictionary<(CharaWithReserved, Level), IClearData> ClearData { get; } =
            Utils.GetEnumerator<Level>().ToDictionary(
                level => (CharaWithReserved.Reimu, level),
                level => new ClearDataStub(ClearDataTests.ValidStub) as IClearData)
            .Concat(Utils.GetEnumerator<Level>().ToDictionary(
                level => (CharaWithReserved.Marisa, level),
                level => new ClearDataStub(ClearDataTests.ValidStub)
                {
                    CardTrialCount = Enumerable.Repeat(0, 100).Select(count => (short)count).ToList(),
                } as IClearData))
            .ToDictionary(pair => pair.Key, pair => pair.Value);

        [TestMethod]
        public void CardReplacerTest()
        {
            var replacer = new CardReplacer(ClearData, true);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CardReplacerTestNull()
        {
            _ = new CardReplacer(null, true);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CardReplacerTestEmpty()
        {
            var clearData = new Dictionary<(CharaWithReserved, Level), IClearData>();
            var replacer = new CardReplacer(clearData, true);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var replacer = new CardReplacer(ClearData, false);
            Assert.AreEqual("符の壱「スターダストレヴァリエ」-Easy-", replacer.Replace("%T75CARD001RMN"));
            Assert.AreEqual("符の壱「アーティフルチャンター」-Easy-", replacer.Replace("%T75CARD001MRN"));
        }

        [TestMethod]
        public void ReplaceTestRank()
        {
            var replacer = new CardReplacer(ClearData, false);
            Assert.AreEqual("Easy", replacer.Replace("%T75CARD001RMR"));
            Assert.AreEqual("Easy", replacer.Replace("%T75CARD001MRR"));
        }

        [TestMethod]
        public void ReplaceTestHiddenName()
        {
            var replacer = new CardReplacer(ClearData, true);
            Assert.AreEqual("符の壱「スターダストレヴァリエ」-Easy-", replacer.Replace("%T75CARD001RMN"));
            Assert.AreEqual("??????????", replacer.Replace("%T75CARD001MRN"));
        }

        [TestMethod]
        public void ReplaceTestHiddenRank()
        {
            var replacer = new CardReplacer(ClearData, true);
            Assert.AreEqual("Easy", replacer.Replace("%T75CARD001RMR"));
            Assert.AreEqual("?????", replacer.Replace("%T75CARD001MRR"));
        }

        [TestMethod]
        public void ReplaceTestMeiling()
        {
            var replacer = new CardReplacer(ClearData, true);
            Assert.AreEqual("%T75CARD001MLN", replacer.Replace("%T75CARD001MLN"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentName()
        {
            var replacer = new CardReplacer(ClearData, true);
            Assert.AreEqual("%T75CARD101RMN", replacer.Replace("%T75CARD101RMN"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentRank()
        {
            var replacer = new CardReplacer(ClearData, true);
            Assert.AreEqual("%T75CARD101RMR", replacer.Replace("%T75CARD101RMR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CardReplacer(ClearData, true);
            Assert.AreEqual("%T75XXXX001RMN", replacer.Replace("%T75XXXX001RMN"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CardReplacer(ClearData, true);
            Assert.AreEqual("%T75CARD101RMN", replacer.Replace("%T75CARD101RMN"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CardReplacer(ClearData, true);
            Assert.AreEqual("%T75CARD001XXN", replacer.Replace("%T75CARD001XXN"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CardReplacer(ClearData, true);
            Assert.AreEqual("%T75CARD001RMX", replacer.Replace("%T75CARD001RMX"));
        }
    }
}
