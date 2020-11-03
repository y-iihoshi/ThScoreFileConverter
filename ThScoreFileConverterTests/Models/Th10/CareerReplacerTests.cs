using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<
    ThScoreFileConverter.Models.Th10.CharaWithTotal, ThScoreFileConverter.Models.Th10.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th10
{
    [TestClass]
    public class CareerReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new[] { ClearDataTests.MockClearData().Object }.ToDictionary(clearData => clearData.Chara);

        [TestMethod]
        public void CareerReplacerTest()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CareerReplacerTestNull()
        {
            _ = new CareerReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CareerReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CareerReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("126", replacer.Replace("%T10C003RB1"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("459", replacer.Replace("%T10C003RB2"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CareerReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("19,635", replacer.Replace("%T10C000RB1"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("19635", replacer.Replace("%T10C000RB1"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CareerReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("56,265", replacer.Replace("%T10C000RB2"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("56265", replacer.Replace("%T10C000RB2"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CareerReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T10C003RB1"));
        }

        [TestMethod]
        public void ReplaceTestEmptyCards()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.ReimuB) && (m.Cards == new Dictionary<int, ISpellCard<Level>>()))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new CareerReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T10C003RB1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T10X003RB1", replacer.Replace("%T10X003RB1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T10C111RB1", replacer.Replace("%T10C111RB1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T10C003XX1", replacer.Replace("%T10C003XX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T10C003RBX", replacer.Replace("%T10C003RBX"));
        }
    }
}
