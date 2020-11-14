using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter;
using ThScoreFileConverter.Models.Th11;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<
    ThScoreFileConverter.Models.Th11.CharaWithTotal, ThScoreFileConverter.Models.Th10.StageProgress>;
using ISpellCard = ThScoreFileConverter.Models.Th10.ISpellCard<ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverterTests.Models.Th11
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
        public void CareerReplacerTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new CareerReplacer(null!));

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
            Assert.AreEqual("126", replacer.Replace("%T11C003RS1"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("459", replacer.Replace("%T11C003RS2"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CareerReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("36,925", replacer.Replace("%T11C000RS1"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("36925", replacer.Replace("%T11C000RS1"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CareerReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("95,200", replacer.Replace("%T11C000RS2"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("95200", replacer.Replace("%T11C000RS2"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CareerReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T11C003RS1"));
        }

        [TestMethod]
        public void ReplaceTestEmptyCards()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.ReimuSuika) && (m.Cards == new Dictionary<int, ISpellCard>()))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new CareerReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T11C003RS1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11X003RS1", replacer.Replace("%T11X003RS1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11C176RS1", replacer.Replace("%T11C176RS1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11C003XX1", replacer.Replace("%T11C003XX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11C003RSX", replacer.Replace("%T11C003RSX"));
        }
    }
}
