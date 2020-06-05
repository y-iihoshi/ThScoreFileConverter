using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using WPFLocalizeExtension.Engine;
using System.Globalization;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class WorkTests
    {
        [TestMethod]
        public void WorkTest()
        {
            var work = new Work { Number = "TH06", IsSupported = true };

            LocalizeDictionary.Instance.Culture = new CultureInfo("en-US");
            Assert.AreEqual("the Embodiment of Scarlet Devil", work.Title);

            LocalizeDictionary.Instance.Culture = new CultureInfo("ja-JP");
            Assert.AreEqual("東方紅魔郷", work.Title);

            LocalizeDictionary.Instance.Culture = CultureInfo.InvariantCulture;
            Assert.AreEqual("the Embodiment of Scarlet Devil", work.Title);
        }

        [TestMethod]
        public void WorkTestDefault()
        {
            var work = new Work();

            Assert.AreEqual(string.Empty, work.Number);
            Assert.AreEqual(string.Empty, work.Title);
            Assert.AreEqual(false, work.IsSupported);
        }

        [TestMethod]
        public void LocalizeDictionaryPropertyChangedTest()
        {
            var work = new Work { Number = "TH06" };

            LocalizeDictionary.Instance.Culture = new CultureInfo("ja-JP");
            Assert.AreEqual("東方紅魔郷", work.Title);

            LocalizeDictionary.Instance.IncludeInvariantCulture = false;
            Assert.AreEqual("東方紅魔郷", work.Title);
        }
    }
}
