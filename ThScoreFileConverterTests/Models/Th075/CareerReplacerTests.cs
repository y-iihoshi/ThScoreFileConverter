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
    public class CareerReplacerTests
    {
        internal static IReadOnlyDictionary<(CharaWithReserved, Level), IClearData> ClearData { get; } =
            EnumHelper<Level>.Enumerable.ToDictionary(
                level => (CharaWithReserved.Reimu, level),
                level => ClearDataTests.MockClearData().Object);

        private static Mock<INumberFormatter> MockNumberFormatter()
        {
            var mock = new Mock<INumberFormatter>();
            _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
                .Returns((object value) => "invoked: " + value.ToString());
            return mock;
        }

        [TestMethod]
        public void CareerReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearData, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CareerReplacerTestEmpty()
        {
            var clearData = new Dictionary<(CharaWithReserved, Level), IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(clearData, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestMaxBonus()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("invoked: 36", replacer.Replace("%T75C001RM1"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("invoked: 32", replacer.Replace("%T75C001RM2"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("invoked: 28", replacer.Replace("%T75C001RM3"));
        }

        [TestMethod]
        public void ReplaceTestStar()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("★", replacer.Replace("%T75C001RM4"));
        }

        [TestMethod]
        public void ReplaceTestNotStar()
        {
            static IClearData CreateClearData()
            {
                var mock = ClearDataTests.MockClearData();
                var cardTrulyGot = mock.Object.CardTrulyGot;
                _ = mock.SetupGet(m => m.CardTrulyGot).Returns(
                    cardTrulyGot.Select((got, index) => index == 0 ? (byte)0 : got).ToList());
                return mock.Object;
            }

            var clearData = EnumHelper<Level>.Enumerable.ToDictionary(
                level => (CharaWithReserved.Reimu, level),
                level => CreateClearData());
            var formatterMock = MockNumberFormatter();

            var replacer = new CareerReplacer(clearData, formatterMock.Object);
            Assert.AreEqual(string.Empty, replacer.Replace("%T75C001RM4"));
        }

        [TestMethod]
        public void ReplaceTestTotalMaxBonus()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("invoked: 23400", replacer.Replace("%T75C000RM1"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("invoked: 23000", replacer.Replace("%T75C000RM2"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("invoked: 22600", replacer.Replace("%T75C000RM3"));
        }

        [TestMethod]
        public void ReplaceTestTotalStar()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("%T75C000RM4", replacer.Replace("%T75C000RM4"));
        }

        [TestMethod]
        public void ReplaceTestMeiling()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("%T75C001ML1", replacer.Replace("%T75C001ML1"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentMaxBonus()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T75C001MR1"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T75C001MR2"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T75C001MR3"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentStar()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual(string.Empty, replacer.Replace("%T75C001MR4"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("%T75X001RM1", replacer.Replace("%T75X001RM1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("%T75C101RM1", replacer.Replace("%T75C101RM1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("%T75C001XX1", replacer.Replace("%T75C001XX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("%T75C001RMX", replacer.Replace("%T75C001RMX"));
        }
    }
}
