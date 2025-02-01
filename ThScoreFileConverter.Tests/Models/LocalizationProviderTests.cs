using System.Globalization;
using System.Windows;
using ThScoreFileConverter.Core.Resources;
using ThScoreFileConverter.Models;
using WPFLocalizeExtension.Providers;

namespace ThScoreFileConverter.Tests.Models;

[TestClass]
public class LocalizationProviderTests
{
    [TestMethod]
    public void InstanceTest()
    {
        var provider1 = LocalizationProvider.Instance;
        var provider2 = LocalizationProvider.Instance;
        provider2.ShouldBe(provider1);
    }

    [TestMethod]
    public void AvailableCulturesTest()
    {
        var cultures = LocalizationProvider.Instance.AvailableCultures;
        cultures.Any(culture => culture.Name == "en-US").ShouldBeTrue();
        cultures.Any(culture => culture.Name == "ja-JP").ShouldBeTrue();
    }

    [TestMethod]
    public void GetFullyQualifiedResourceKeyTest()
    {
        var key = "abc";
        var fqKey = LocalizationProvider.Instance.GetFullyQualifiedResourceKey(key, new DependencyObject());
        fqKey.ShouldBeOfType<FQAssemblyDictionaryKey>().Key.ShouldBe(key);
    }

    [TestMethod]
    public void GetLocalizedObjectTest()
    {
        var objEnUS = LocalizationProvider.Instance.GetLocalizedObject(
            "TH06", new DependencyObject(), CultureInfo.GetCultureInfo("en-US"));
        objEnUS.ShouldBe("the Embodiment of Scarlet Devil");

        var objJaJP = LocalizationProvider.Instance.GetLocalizedObject(
            "TH06", new DependencyObject(), CultureInfo.GetCultureInfo("ja-JP"));
        objJaJP.ShouldBe("東方紅魔郷");
    }

    [TestMethod]
    public void GetLocalizedObjectTestEmptyKey()
    {
        var obj = LocalizationProvider.Instance.GetLocalizedObject(
            string.Empty, new DependencyObject(), CultureInfo.GetCultureInfo("ja-JP"));
        obj.ShouldBeNull();
    }

    [TestMethod]
    public void GetLocalizedObjectTestNonexistentKey()
    {
        var obj = LocalizationProvider.Instance.GetLocalizedObject(
            "TH01", new DependencyObject(), CultureInfo.GetCultureInfo("ja-JP"));
        obj.ShouldBeNull();
    }

    [TestMethod]
    public void GetLocalizedObjectTestUnsupportedCulture()
    {
        var key = "TH06";
        var obj = LocalizationProvider.Instance.GetLocalizedObject(
            key, new DependencyObject(), CultureInfo.GetCultureInfo("de"));
        obj.ShouldBe(StringResources.ResourceManager.GetObject(key, CultureInfo.InvariantCulture));
    }

    [TestMethod]
    public void ProviderErrorTest()
    {
        var invoked = false;

        void OnError(object sender, ProviderErrorEventArgs args)
        {
            invoked = true;
        }

        LocalizationProvider.Instance.ProviderError += OnError;
        try
        {
            _ = LocalizationProvider.Instance.GetLocalizedObject(
                "TH01", new DependencyObject(), CultureInfo.GetCultureInfo("ja-JP"));
            invoked.ShouldBeTrue();
        }
        finally
        {
            LocalizationProvider.Instance.ProviderError -= OnError;
        }
    }

    [TestMethod]
    public void ProviderErrorTestException()
    {
        var numInvoked = 0;

#pragma warning disable CA2201 // Do not raise reserved exception types
        void OnError(object sender, ProviderErrorEventArgs args)
        {
            ++numInvoked;
            if (numInvoked == 1)
                throw new Exception(nameof(OnError));
        }
#pragma warning restore CA2201 // Do not raise reserved exception types

        LocalizationProvider.Instance.ProviderError += OnError;
        try
        {
            _ = LocalizationProvider.Instance.GetLocalizedObject(
                "TH01", new DependencyObject(), CultureInfo.GetCultureInfo("ja-JP"));
            numInvoked.ShouldBe(2);
        }
        finally
        {
            LocalizationProvider.Instance.ProviderError -= OnError;
        }
    }
}
