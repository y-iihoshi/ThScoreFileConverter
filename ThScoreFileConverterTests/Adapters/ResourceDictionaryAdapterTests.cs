using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Adapters;

namespace ThScoreFileConverterTests.Adapters;

[TestClass]
public class ResourceDictionaryAdapterTests
{
    [TestMethod]
    public void ResourceDictionaryAdapterTest()
    {
        var dictionary = new ResourceDictionary();
        var adapter = new ResourceDictionaryAdapter(dictionary);
        Assert.IsNotNull(adapter);
    }

    [TestMethod]
    public void FontFamilyKeyTest()
    {
        Assert.AreEqual(nameof(ResourceDictionaryAdapter.FontFamilyKey), ResourceDictionaryAdapter.FontFamilyKey);
    }

    [TestMethod]
    public void FontSizeKeyTest()
    {
        Assert.AreEqual(nameof(ResourceDictionaryAdapter.FontSizeKey), ResourceDictionaryAdapter.FontSizeKey);
    }

    [TestMethod]
    public void FontFamilyTest()
    {
        var fontFamily = new FontFamily();
        var dictionary = new ResourceDictionary
        {
            { ResourceDictionaryAdapter.FontFamilyKey, fontFamily },
        };
        var adapter = new ResourceDictionaryAdapter(dictionary);
        Assert.AreEqual(fontFamily, adapter.FontFamily);
    }

    [TestMethod]
    public void FontFamilyTestNonexistent()
    {
        var dictionary = new ResourceDictionary();
        var adapter = new ResourceDictionaryAdapter(dictionary);
        Assert.IsNotNull(adapter.FontFamily);
    }

    [TestMethod]
    public void FontSizeTest()
    {
        var fontSize = default(double);
        var dictionary = new ResourceDictionary
        {
            { ResourceDictionaryAdapter.FontSizeKey, fontSize },
        };
        var adapter = new ResourceDictionaryAdapter(dictionary);
        Assert.AreEqual(fontSize, adapter.FontSize);
    }

    [TestMethod]
    public void FontSizeTestNonexistent()
    {
        var dictionary = new ResourceDictionary();
        var adapter = new ResourceDictionaryAdapter(dictionary);
        Assert.AreEqual(default, adapter.FontSize);
    }

    [TestMethod]
    public void UpdateResourcesTest()
    {
        var defaultFontFamily = new FontFamily();
        var dictionary = new ResourceDictionary
        {
            { ResourceDictionaryAdapter.FontFamilyKey, defaultFontFamily },
            { ResourceDictionaryAdapter.FontSizeKey, default },
        };
        var adapter = new ResourceDictionaryAdapter(dictionary);

        var fontFamily = SystemFonts.MessageFontFamily;
        var fontSize = SystemFonts.MessageFontSize;

        adapter.UpdateResources(fontFamily, fontSize);

        Assert.AreEqual(fontFamily, adapter.FontFamily);
        Assert.AreEqual(fontSize, adapter.FontSize);
    }

    [TestMethod]
    public void UpdateResourcesTestNullFontFamily()
    {
        var defaultFontFamily = new FontFamily();
        var dictionary = new ResourceDictionary
        {
            { ResourceDictionaryAdapter.FontFamilyKey, defaultFontFamily },
            { ResourceDictionaryAdapter.FontSizeKey, default },
        };
        var adapter = new ResourceDictionaryAdapter(dictionary);

        var fontSize = SystemFonts.MessageFontSize;

        adapter.UpdateResources((FontFamily)null!, fontSize);

        Assert.AreEqual(defaultFontFamily, adapter.FontFamily);
        Assert.AreEqual(fontSize, adapter.FontSize);
    }

    [TestMethod]
    public void UpdateResourcesTestNullFontSize()
    {
        var defaultFontFamily = new FontFamily();
        var dictionary = new ResourceDictionary
        {
            { ResourceDictionaryAdapter.FontFamilyKey, defaultFontFamily },
            { ResourceDictionaryAdapter.FontSizeKey, default },
        };
        var adapter = new ResourceDictionaryAdapter(dictionary);

        var fontFamily = SystemFonts.MessageFontFamily;

        adapter.UpdateResources(fontFamily, null);

        Assert.AreEqual(fontFamily, adapter.FontFamily);
        Assert.AreEqual(default, adapter.FontSize);
    }

    [TestMethod]
    public void UpdateResourcesTestFontFamilyName()
    {
        var defaultFontFamily = new FontFamily();
        var dictionary = new ResourceDictionary
        {
            { ResourceDictionaryAdapter.FontFamilyKey, defaultFontFamily },
            { ResourceDictionaryAdapter.FontSizeKey, default },
        };
        var adapter = new ResourceDictionaryAdapter(dictionary);

        var fontFamilyName = SystemFonts.MessageFontFamily.Source;
        var fontSize = SystemFonts.MessageFontSize;

        adapter.UpdateResources(fontFamilyName, fontSize);

        Assert.AreEqual(fontFamilyName, adapter.FontFamily.Source);
        Assert.AreEqual(fontSize, adapter.FontSize);
    }
}
