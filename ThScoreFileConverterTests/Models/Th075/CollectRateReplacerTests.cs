using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th075;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverterTests.Models.Th075
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        private static IClearData CreateClearData()
        {
            var mock = ClearDataTests.MockClearData();
            _ = mock.SetupGet(m => m.CardGotCount).Returns(
                Enumerable.Range(1, 100).Select(count => (short)(count % 5)).ToList());
            _ = mock.SetupGet(m => m.CardTrialCount).Returns(
                Enumerable.Range(1, 100).Select(count => (short)(count % 7)).ToList());
            _ = mock.SetupGet(m => m.CardTrulyGot).Returns(
                Enumerable.Range(1, 100).Select(count => (byte)(count % 3)).ToList());
            return mock.Object;
        }

        internal static IReadOnlyDictionary<(CharaWithReserved, Level), IClearData> ClearData { get; } =
            EnumHelper<Level>.Enumerable.ToDictionary(
                level => (CharaWithReserved.Reimu, level),
                level => CreateClearData());

        private static Mock<INumberFormatter> MockNumberFormatter()
        {
            var mock = new Mock<INumberFormatter>();
            _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
                .Returns((object value) => "invoked: " + value.ToString());
            return mock;
        }

        [TestMethod]
        public void CollectRateReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearData, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CollectRateReplacerTestNull()
        {
            var formatterMock = MockNumberFormatter();
            _ = Assert.ThrowsException<ArgumentNullException>(
                () => _ = new CollectRateReplacer(null!, formatterMock.Object));
        }

        [TestMethod]
        public void CollectRateReplacerTestEmpty()
        {
            var clearData = new Dictionary<(CharaWithReserved, Level), IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(clearData, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("invoked: 20", replacer.Replace("%T75CRGHRM1"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("invoked: 21", replacer.Replace("%T75CRGHRM2"));
        }

        [TestMethod]
        public void ReplaceTestTrulyGot()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("invoked: 16", replacer.Replace("%T75CRGHRM3"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("invoked: 80", replacer.Replace("%T75CRGTRM1"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("invoked: 86", replacer.Replace("%T75CRGTRM2"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrulyGot()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("invoked: 67", replacer.Replace("%T75CRGTRM3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCount()
        {
            var clearData = new Dictionary<(CharaWithReserved, Level), IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(clearData, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T75CRGHRM1"));
        }

        [TestMethod]
        public void ReplaceTestEmptyTrialCount()
        {
            var clearData = new Dictionary<(CharaWithReserved, Level), IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(clearData, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T75CRGHRM2"));
        }

        [TestMethod]
        public void ReplaceTestEmptyTrulyGot()
        {
            var clearData = new Dictionary<(CharaWithReserved, Level), IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(clearData, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T75CRGHRM3"));
        }

        [TestMethod]
        public void ReplaceTestMeiling()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("%T75CRGHML1", replacer.Replace("%T75CRGHML1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("%T75XXXHRM1", replacer.Replace("%T75XXXHRM1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("%T75CRGXRM1", replacer.Replace("%T75CRGXRM1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("%T75CRGHXX1", replacer.Replace("%T75CRGHXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("%T75CRGHRMX", replacer.Replace("%T75CRGHRMX"));
        }
    }
}
