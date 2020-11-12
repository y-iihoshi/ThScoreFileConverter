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
    public class CharaReplacerTests
    {
        internal static IReadOnlyDictionary<(CharaWithReserved, Level), IClearData> ClearData { get; } =
            Utils.GetEnumerable<Level>().ToDictionary(
                level => (CharaWithReserved.Reimu, level),
                level => ClearDataTests.MockClearData().Object);

        [TestMethod]
        public void CharaReplacerTest()
        {
            var replacer = new CharaReplacer(ClearData);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CharaReplacerTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new CharaReplacer(null!));

        [TestMethod]
        public void CharaReplacerTestEmpty()
        {
            var clearData = new Dictionary<(CharaWithReserved, Level), IClearData>();
            var replacer = new CharaReplacer(clearData);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestUseCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CharaReplacer(ClearData);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("1,234", replacer.Replace("%T75CHRHRM1"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("1234", replacer.Replace("%T75CHRHRM1"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CharaReplacer(ClearData);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("2,345", replacer.Replace("%T75CHRHRM2"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("2345", replacer.Replace("%T75CHRHRM2"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestMaxCombo()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CharaReplacer(ClearData);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("3,456", replacer.Replace("%T75CHRHRM3"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("3456", replacer.Replace("%T75CHRHRM3"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestMaxDamage()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CharaReplacer(ClearData);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("4,567", replacer.Replace("%T75CHRHRM4"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("4567", replacer.Replace("%T75CHRHRM4"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var clearData = new Dictionary<(CharaWithReserved, Level), IClearData>();
            var replacer = new CharaReplacer(clearData);
            Assert.AreEqual("0", replacer.Replace("%T75CHRHRM1"));
            Assert.AreEqual("0", replacer.Replace("%T75CHRHRM2"));
            Assert.AreEqual("0", replacer.Replace("%T75CHRHRM3"));
            Assert.AreEqual("0", replacer.Replace("%T75CHRHRM4"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentUseCount()
        {
            var replacer = new CharaReplacer(ClearData);
            Assert.AreEqual("0", replacer.Replace("%T75CHRHMR1"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentClearCount()
        {
            var replacer = new CharaReplacer(ClearData);
            Assert.AreEqual("0", replacer.Replace("%T75CHRHMR2"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentMaxCombo()
        {
            var replacer = new CharaReplacer(ClearData);
            Assert.AreEqual("0", replacer.Replace("%T75CHRHMR3"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentMaxDamage()
        {
            var replacer = new CharaReplacer(ClearData);
            Assert.AreEqual("0", replacer.Replace("%T75CHRHMR4"));
        }

        [TestMethod]
        public void ReplaceTestMeiling()
        {
            var replacer = new CharaReplacer(ClearData);
            Assert.AreEqual("%T75CHRHML1", replacer.Replace("%T75CHRHML1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CharaReplacer(ClearData);
            Assert.AreEqual("%T75XXXHRM1", replacer.Replace("%T75XXXHRM1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new CharaReplacer(ClearData);
            Assert.AreEqual("%T75CHRXRM1", replacer.Replace("%T75CHRXRM1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CharaReplacer(ClearData);
            Assert.AreEqual("%T75CHRHXX1", replacer.Replace("%T75CHRHXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CharaReplacer(ClearData);
            Assert.AreEqual("%T75CHRHRMX", replacer.Replace("%T75CHRHRMX"));
        }
    }
}
