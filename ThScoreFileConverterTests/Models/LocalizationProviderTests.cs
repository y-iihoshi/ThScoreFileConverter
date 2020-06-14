using System.Globalization;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Properties;
using WPFLocalizeExtension.Providers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class LocalizationProviderTests
    {
        [TestMethod]
        public void InstanceTest()
        {
            var provider1 = LocalizationProvider.Instance;
            var provider2 = LocalizationProvider.Instance;
            Assert.AreEqual(provider1, provider2);
        }

        [TestMethod]
        public void AvailableCulturesTest()
        {
            var cultures = LocalizationProvider.Instance.AvailableCultures;
            Assert.IsTrue(cultures.Any(culture => culture.Name == "en-US"));
            Assert.IsTrue(cultures.Any(culture => culture.Name == "ja-JP"));
        }

        [TestMethod]
        public void GetFullyQualifiedResourceKeyTest()
        {
            var key = "abc";
            var fqKey = LocalizationProvider.Instance.GetFullyQualifiedResourceKey(key, new DependencyObject());
            Assert.IsTrue(fqKey is FQAssemblyDictionaryKey);
            Assert.AreEqual(key, ((FQAssemblyDictionaryKey)fqKey).Key);
        }

        [TestMethod]
        public void GetLocalizedObjectTest()
        {
            var objEnUS = LocalizationProvider.Instance.GetLocalizedObject(
                "TH06", new DependencyObject(), CultureInfo.GetCultureInfo("en-US"));
            Assert.AreEqual("the Embodiment of Scarlet Devil", objEnUS);

            var objJaJP = LocalizationProvider.Instance.GetLocalizedObject(
                "TH06", new DependencyObject(), CultureInfo.GetCultureInfo("ja-JP"));
            Assert.AreEqual("東方紅魔郷", objJaJP);
        }

        [TestMethod]
        public void GetLocalizedObjectTestNullKey()
        {
            var obj = LocalizationProvider.Instance.GetLocalizedObject(
                null!, new DependencyObject(), CultureInfo.GetCultureInfo("ja-JP"));
            Assert.IsNull(obj);
        }

        [TestMethod]
        public void GetLocalizedObjectTestEmptyKey()
        {
            var obj = LocalizationProvider.Instance.GetLocalizedObject(
                string.Empty, new DependencyObject(), CultureInfo.GetCultureInfo("ja-JP"));
            Assert.IsNull(obj);
        }

        [TestMethod]
        public void GetLocalizedObjectTestNonexistentKey()
        {
            var obj = LocalizationProvider.Instance.GetLocalizedObject(
                "TH01", new DependencyObject(), CultureInfo.GetCultureInfo("ja-JP"));
            Assert.IsNull(obj);
        }

        [TestMethod]
        public void GetLocalizedObjectTestNullTarget()
        {
            var obj = LocalizationProvider.Instance.GetLocalizedObject(
                "TH06", null!, CultureInfo.GetCultureInfo("ja-JP"));
            Assert.AreEqual("東方紅魔郷", obj);
        }

        [TestMethod]
        public void GetLocalizedObjectTestNullCulture()
        {
            var key = "TH06";
            var obj = LocalizationProvider.Instance.GetLocalizedObject(key, new DependencyObject(), null!);
            Assert.AreEqual(Resources.ResourceManager.GetObject(key, CultureInfo.CurrentUICulture), obj);
        }

        [TestMethod]
        public void GetLocalizedObjectTestUnsupportedCulture()
        {
            var key = "TH06";
            var obj = LocalizationProvider.Instance.GetLocalizedObject(
                key, new DependencyObject(), CultureInfo.GetCultureInfo("de"));
            Assert.AreEqual(Resources.ResourceManager.GetObject(key, CultureInfo.InvariantCulture), obj);
        }
    }
}
