using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using WPFLocalizeExtension.Engine;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class WorkTests
    {
        [TestMethod]
        public void WorkTest()
        {
            var work = new Work { Number = "TH06" };
            var culture = LocalizeDictionary.Instance.Culture;

            try
            {
                LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo("en-US");
                Assert.AreEqual("the Embodiment of Scarlet Devil", work.Title);

                LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo("ja-JP");
                Assert.AreEqual("東方紅魔郷", work.Title);

                LocalizeDictionary.Instance.Culture = CultureInfo.InvariantCulture;
                Assert.AreEqual("the Embodiment of Scarlet Devil", work.Title);
            }
            finally
            {
                LocalizeDictionary.Instance.Culture = culture;
            }
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
            var culture = LocalizeDictionary.Instance.Culture;
            var includes = LocalizeDictionary.Instance.IncludeInvariantCulture;

            try
            {
                LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo("ja-JP");
                Assert.AreEqual("東方紅魔郷", work.Title);

                LocalizeDictionary.Instance.IncludeInvariantCulture = false;
                Assert.AreEqual("東方紅魔郷", work.Title);
            }
            finally
            {
                LocalizeDictionary.Instance.IncludeInvariantCulture = includes;
                LocalizeDictionary.Instance.Culture = culture;
            }
        }
    }
}
