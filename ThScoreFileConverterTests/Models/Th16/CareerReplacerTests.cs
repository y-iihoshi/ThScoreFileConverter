using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter;
using ThScoreFileConverter.Models.Th16;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverterTests.Models.Th16
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
        public void ReplaceTestStoryClearCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("15", replacer.Replace("%T16CS003AY1"));
        }

        [TestMethod]
        public void ReplaceTestStoryTrialCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("59", replacer.Replace("%T16CS003AY2"));
        }

        [TestMethod]
        public void ReplaceTestStoryTotalClearCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CareerReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("8,568", replacer.Replace("%T16CS000AY1"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("8568", replacer.Replace("%T16CS000AY1"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestStoryTotalTrialCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CareerReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("13,804", replacer.Replace("%T16CS000AY2"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("13804", replacer.Replace("%T16CS000AY2"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestPracticeClearCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("37", replacer.Replace("%T16CP003AY1"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTrialCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("81", replacer.Replace("%T16CP003AY2"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTotalClearCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CareerReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("11,186", replacer.Replace("%T16CP000AY1"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("11186", replacer.Replace("%T16CP000AY1"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestPracticeTotalTrialCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CareerReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("16,422", replacer.Replace("%T16CP000AY2"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("16422", replacer.Replace("%T16CP000AY2"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CareerReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T16CS003AY1"));
        }

        [TestMethod]
        public void ReplaceTestEmptyCards()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Aya) && (m.Cards == new Dictionary<int, ISpellCard>()))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new CareerReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T16CS003AY1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16XS003AY1", replacer.Replace("%T16XS003AY1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidGameMode()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16CX003AY1", replacer.Replace("%T16CX003AY1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16CS121AY1", replacer.Replace("%T16CS121AY1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16CS003XX1", replacer.Replace("%T16CS003XX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16CS003AYX", replacer.Replace("%T16CS003AYX"));
        }
    }
}
