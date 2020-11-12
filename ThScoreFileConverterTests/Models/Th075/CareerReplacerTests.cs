using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th075;
using Level = ThScoreFileConverter.Models.Th075.Level;

namespace ThScoreFileConverterTests.Models.Th075
{
    [TestClass]
    public class CareerReplacerTests
    {
        internal static IReadOnlyDictionary<(CharaWithReserved, Level), IClearData> ClearData { get; } =
            Utils.GetEnumerable<Level>().ToDictionary(
                level => (CharaWithReserved.Reimu, level),
                level => ClearDataTests.MockClearData().Object);

        [TestMethod]
        public void CareerReplacerTest()
        {
            var replacer = new CareerReplacer(ClearData);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CareerReplacerTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new CareerReplacer(null!));

        [TestMethod]
        public void CareerReplacerTestEmpty()
        {
            var clearData = new Dictionary<(CharaWithReserved, Level), IClearData>();
            var replacer = new CareerReplacer(clearData);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestMaxBonus()
        {
            var replacer = new CareerReplacer(ClearData);
            Assert.AreEqual("36", replacer.Replace("%T75C001RM1"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var replacer = new CareerReplacer(ClearData);
            Assert.AreEqual("32", replacer.Replace("%T75C001RM2"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var replacer = new CareerReplacer(ClearData);
            Assert.AreEqual("28", replacer.Replace("%T75C001RM3"));
        }

        [TestMethod]
        public void ReplaceTestStar()
        {
            var replacer = new CareerReplacer(ClearData);
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

            var clearData = Utils.GetEnumerable<Level>().ToDictionary(
                level => (CharaWithReserved.Reimu, level),
                level => CreateClearData());

            var replacer = new CareerReplacer(clearData);
            Assert.AreEqual(string.Empty, replacer.Replace("%T75C001RM4"));
        }

        [TestMethod]
        public void ReplaceTestTotalMaxBonus()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CareerReplacer(ClearData);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("23,400", replacer.Replace("%T75C000RM1"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("23400", replacer.Replace("%T75C000RM1"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CareerReplacer(ClearData);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("23,000", replacer.Replace("%T75C000RM2"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("23000", replacer.Replace("%T75C000RM2"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CareerReplacer(ClearData);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("22,600", replacer.Replace("%T75C000RM3"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("22600", replacer.Replace("%T75C000RM3"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestTotalStar()
        {
            var replacer = new CareerReplacer(ClearData);
            Assert.AreEqual("%T75C000RM4", replacer.Replace("%T75C000RM4"));
        }

        [TestMethod]
        public void ReplaceTestMeiling()
        {
            var replacer = new CareerReplacer(ClearData);
            Assert.AreEqual("%T75C001ML1", replacer.Replace("%T75C001ML1"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentMaxBonus()
        {
            var replacer = new CareerReplacer(ClearData);
            Assert.AreEqual("0", replacer.Replace("%T75C001MR1"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentClearCount()
        {
            var replacer = new CareerReplacer(ClearData);
            Assert.AreEqual("0", replacer.Replace("%T75C001MR2"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentTrialCount()
        {
            var replacer = new CareerReplacer(ClearData);
            Assert.AreEqual("0", replacer.Replace("%T75C001MR3"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentStar()
        {
            var replacer = new CareerReplacer(ClearData);
            Assert.AreEqual(string.Empty, replacer.Replace("%T75C001MR4"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CareerReplacer(ClearData);
            Assert.AreEqual("%T75X001RM1", replacer.Replace("%T75X001RM1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CareerReplacer(ClearData);
            Assert.AreEqual("%T75C101RM1", replacer.Replace("%T75C101RM1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CareerReplacer(ClearData);
            Assert.AreEqual("%T75C001XX1", replacer.Replace("%T75C001XX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CareerReplacer(ClearData);
            Assert.AreEqual("%T75C001RMX", replacer.Replace("%T75C001RMX"));
        }
    }
}
