using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests;

[TestClass]
#if NETFRAMEWORK
[DeploymentItem(@"TestData\empty.xml", @"TestData")]
[DeploymentItem(@"TestData\no-root-node.xml", @"TestData")]
[DeploymentItem(@"TestData\invalid-root-node.xml", @"TestData")]
[DeploymentItem(@"TestData\wrong-namespace.xml", @"TestData")]
[DeploymentItem(@"TestData\no-child-nodes.xml", @"TestData")]
[DeploymentItem(@"TestData\no-last-title.xml", @"TestData")]
[DeploymentItem(@"TestData\empty-last-title.xml", @"TestData")]
[DeploymentItem(@"TestData\nonexistent-last-title.xml", @"TestData")]
[DeploymentItem(@"TestData\no-dictionary.xml", @"TestData")]
[DeploymentItem(@"TestData\empty-dictionary.xml", @"TestData")]
[DeploymentItem(@"TestData\invalid-key-value-types.xml", @"TestData")]
[DeploymentItem(@"TestData\no-key.xml", @"TestData")]
[DeploymentItem(@"TestData\empty-key.xml", @"TestData")]
[DeploymentItem(@"TestData\unknown-key.xml", @"TestData")]
[DeploymentItem(@"TestData\no-value.xml", @"TestData")]
[DeploymentItem(@"TestData\empty-value.xml", @"TestData")]
[DeploymentItem(@"TestData\no-best-shot-directory.xml", @"TestData")]
[DeploymentItem(@"TestData\empty-best-shot-directory.xml", @"TestData")]
[DeploymentItem(@"TestData\no-hide-untried-cards.xml", @"TestData")]
[DeploymentItem(@"TestData\empty-hide-untried-cards.xml", @"TestData")]
[DeploymentItem(@"TestData\invalid-hide-untried-cards.xml", @"TestData")]
[DeploymentItem(@"TestData\no-image-output-directory.xml", @"TestData")]
[DeploymentItem(@"TestData\empty-image-output-directory.xml", @"TestData")]
[DeploymentItem(@"TestData\no-output-directory.xml", @"TestData")]
[DeploymentItem(@"TestData\empty-output-directory.xml", @"TestData")]
[DeploymentItem(@"TestData\no-score-file.xml", @"TestData")]
[DeploymentItem(@"TestData\empty-score-file.xml", @"TestData")]
[DeploymentItem(@"TestData\no-template-files.xml", @"TestData")]
[DeploymentItem(@"TestData\empty-template-files.xml", @"TestData")]
[DeploymentItem(@"TestData\invalid-template-files.xml", @"TestData")]
[DeploymentItem(@"TestData\no-font-family-name.xml", @"TestData")]
[DeploymentItem(@"TestData\empty-font-family-name.xml", @"TestData")]
[DeploymentItem(@"TestData\no-font-size.xml", @"TestData")]
[DeploymentItem(@"TestData\empty-font-size.xml", @"TestData")]
[DeploymentItem(@"TestData\invalid-font-size.xml", @"TestData")]
[DeploymentItem(@"TestData\negative-font-size.xml", @"TestData")]
[DeploymentItem(@"TestData\zero-font-size.xml", @"TestData")]
[DeploymentItem(@"TestData\exceeded-font-size.xml", @"TestData")]
[DeploymentItem(@"TestData\no-output-number-group-separator.xml", @"TestData")]
[DeploymentItem(@"TestData\empty-output-number-group-separator.xml", @"TestData")]
[DeploymentItem(@"TestData\invalid-output-number-group-separator.xml", @"TestData")]
[DeploymentItem(@"TestData\no-input-code-page-id.xml", @"TestData")]
[DeploymentItem(@"TestData\empty-input-code-page-id.xml", @"TestData")]
[DeploymentItem(@"TestData\invalid-input-code-page-id.xml", @"TestData")]
[DeploymentItem(@"TestData\unsupported-input-code-page-id.xml", @"TestData")]
[DeploymentItem(@"TestData\no-output-code-page-id.xml", @"TestData")]
[DeploymentItem(@"TestData\empty-output-code-page-id.xml", @"TestData")]
[DeploymentItem(@"TestData\invalid-output-code-page-id.xml", @"TestData")]
[DeploymentItem(@"TestData\unsupported-output-code-page-id.xml", @"TestData")]
[DeploymentItem(@"TestData\no-language.xml", @"TestData")]
[DeploymentItem(@"TestData\empty-language.xml", @"TestData")]
[DeploymentItem(@"TestData\invalid-language.xml", @"TestData")]
[DeploymentItem(@"TestData\valid-language-ja-JP.xml", @"TestData")]
[DeploymentItem(@"TestData\invalid-character.xml", @"TestData")]
#endif
public class SettingsTests
{
    [TestMethod]
    public void SettingsTest()
    {
        var settings = new Settings();
        Assert.AreEqual(string.Empty, settings.LastTitle);
        Assert.AreEqual(0, settings.NumTitles);
        Assert.AreEqual(SystemFonts.MessageFontFamily.Source, settings.FontFamilyName);
        Assert.AreEqual(SystemFonts.MessageFontSize, settings.FontSize);
        Assert.AreEqual(true, settings.OutputNumberGroupSeparator);
        Assert.AreEqual(65001, settings.InputCodePageId);
        Assert.AreEqual(65001, settings.OutputCodePageId);
        Assert.AreEqual(CultureInfo.InvariantCulture.Name, settings.Language);
    }

    [TestMethod]
    public void LoadTestNullPath()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new Settings().Load(null!));
    }

    [TestMethod]
    public void LoadTestEmptyPath()
    {
        _ = Assert.ThrowsException<ArgumentException>(() => new Settings().Load(string.Empty));
    }

    [TestMethod]
    public void LoadTestNonexistentPath()
    {
        var settings = new Settings();
        settings.Load(@"TestData\nonexistent.xml");
        Assert.AreEqual(string.Empty, settings.LastTitle);
        Assert.AreEqual(0, settings.NumTitles);
        Assert.AreEqual(SystemFonts.MessageFontFamily.Source, settings.FontFamilyName);
        Assert.AreEqual(SystemFonts.MessageFontSize, settings.FontSize);
        Assert.AreEqual(true, settings.OutputNumberGroupSeparator);
        Assert.AreEqual(65001, settings.InputCodePageId);
        Assert.AreEqual(65001, settings.OutputCodePageId);
        Assert.AreEqual(CultureInfo.InvariantCulture.Name, settings.Language);
    }

    [TestMethod]
    public void LoadTestEmptyFile()
    {
        _ = Assert.ThrowsException<InvalidDataException>(() => new Settings().Load(@"TestData\empty.xml"));
    }

    [TestMethod]
    public void LoadTestNoRootNode()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\no-root-node.xml"));
    }

    [TestMethod]
    public void LoadTestInvalidRootNode()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\invalid-root-node.xml"));
    }

    [TestMethod]
    public void LoadTestWrongNamespace()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\wrong-namespace.xml"));
    }

    [TestMethod]
    public void LoadTestNoChildNodes()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\no-child-nodes.xml"));
    }

    [TestMethod]
    public void LoadTestNoLastTitle()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\no-last-title.xml"));
    }

    [TestMethod]
    public void LoadTestEmptyLastTitle()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\empty-last-title.xml"));
    }

    [TestMethod]
    public void LoadTestNonexistentLastTitle()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\nonexistent-last-title.xml"));
    }

    [TestMethod]
    public void LoadTestNoDictionary()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\no-dictionary.xml"));
    }

    [TestMethod]
    public void LoadTestEmptyDictionary()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\empty-dictionary.xml"));
    }

    [TestMethod]
    public void LoadTestInvalidKeyValueTypes()
    {
        var settings = new Settings();
        settings.Load(@"TestData\invalid-key-value-types.xml");
        Assert.AreEqual(1, settings.NumTitles);
    }

    [TestMethod]
    public void LoadTestNoKey()
    {
        _ = Assert.ThrowsException<InvalidDataException>(() => new Settings().Load(@"TestData\no-key.xml"));
    }

    [TestMethod]
    public void LoadTestEmptyKey()
    {
        var settings = new Settings();
        settings.Load(@"TestData\empty-key.xml");
        Assert.AreEqual(2, settings.NumTitles);
    }

    [TestMethod]
    public void LoadTestUnknownKey()
    {
        var settings = new Settings();
        settings.Load(@"TestData\unknown-key.xml");
        Assert.AreEqual(2, settings.NumTitles);
    }

    [TestMethod]
    public void LoadTestNoValue()
    {
        _ = Assert.ThrowsException<InvalidDataException>(() => new Settings().Load(@"TestData\no-value.xml"));
    }

    [TestMethod]
    public void LoadTestEmptyValue()
    {
        var settings = new Settings();
        settings.Load(@"TestData\empty-value.xml");

        Assert.AreEqual(1, settings.NumTitles);

        var expected = new SettingsPerTitle();
        var actual = settings.GetSettingsPerTitle(settings.LastTitle);
        Assert.AreEqual(expected.BestShotDirectory, actual.BestShotDirectory);
        Assert.AreEqual(expected.HideUntriedCards, actual.HideUntriedCards);
        Assert.AreEqual(expected.ImageOutputDirectory, actual.ImageOutputDirectory);
        Assert.AreEqual(expected.OutputDirectory, actual.OutputDirectory);
        Assert.AreEqual(expected.ScoreFile, actual.ScoreFile);
        CollectionAssert.That.AreEqual(expected.TemplateFiles, actual.TemplateFiles);
    }

    [TestMethod]
    public void LoadTestNoBestShotDirectory()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-best-shot-directory.xml");
        Assert.AreEqual(1, settings.NumTitles);
        Assert.AreEqual(string.Empty, settings.GetSettingsPerTitle(settings.LastTitle).BestShotDirectory);
    }

    [TestMethod]
    public void LoadTestEmptyBestShotDirectory()
    {
        var settings = new Settings();
        settings.Load(@"TestData\empty-best-shot-directory.xml");
        Assert.AreEqual(1, settings.NumTitles);
        Assert.AreEqual(string.Empty, settings.GetSettingsPerTitle(settings.LastTitle).BestShotDirectory);
    }

    [TestMethod]
    public void LoadTestNoHideUntriedCards()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-hide-untried-cards.xml");
        Assert.AreEqual(1, settings.NumTitles);
        Assert.IsTrue(settings.GetSettingsPerTitle(settings.LastTitle).HideUntriedCards);
    }

    [TestMethod]
    public void LoadTestEmptyHideUntriedCards()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\empty-hide-untried-cards.xml"));
    }

    [TestMethod]
    public void LoadTestInvalidHideUntriedCards()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\invalid-hide-untried-cards.xml"));
    }

    [TestMethod]
    public void LoadTestNoImageOutputDirectory()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-image-output-directory.xml");
        Assert.AreEqual(1, settings.NumTitles);
        Assert.AreEqual(string.Empty, settings.GetSettingsPerTitle(settings.LastTitle).ImageOutputDirectory);
    }

    [TestMethod]
    public void LoadTestEmptyImageOutputDirectory()
    {
        var settings = new Settings();
        settings.Load(@"TestData\empty-image-output-directory.xml");
        Assert.AreEqual(1, settings.NumTitles);
        Assert.AreEqual(string.Empty, settings.GetSettingsPerTitle(settings.LastTitle).ImageOutputDirectory);
    }

    [TestMethod]
    public void LoadTestNoOutputDirectory()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-output-directory.xml");
        Assert.AreEqual(1, settings.NumTitles);
        Assert.AreEqual(string.Empty, settings.GetSettingsPerTitle(settings.LastTitle).OutputDirectory);
    }

    [TestMethod]
    public void LoadTestEmptyOutputDirectory()
    {
        var settings = new Settings();
        settings.Load(@"TestData\empty-output-directory.xml");
        Assert.AreEqual(1, settings.NumTitles);
        Assert.AreEqual(string.Empty, settings.GetSettingsPerTitle(settings.LastTitle).OutputDirectory);
    }

    [TestMethod]
    public void LoadTestNoScoreFile()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-score-file.xml");
        Assert.AreEqual(1, settings.NumTitles);
        Assert.AreEqual(string.Empty, settings.GetSettingsPerTitle(settings.LastTitle).ScoreFile);
    }

    [TestMethod]
    public void LoadTestEmptyScoreFile()
    {
        var settings = new Settings();
        settings.Load(@"TestData\empty-score-file.xml");
        Assert.AreEqual(1, settings.NumTitles);
        Assert.AreEqual(string.Empty, settings.GetSettingsPerTitle(settings.LastTitle).ScoreFile);
    }

    [TestMethod]
    public void LoadTestNoTemplateFiles()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-template-files.xml");
        Assert.AreEqual(1, settings.NumTitles);
        CollectionAssert.That.AreEqual(
            Enumerable.Empty<string>(), settings.GetSettingsPerTitle(settings.LastTitle).TemplateFiles);
    }

    [TestMethod]
    public void LoadTestEmptyTemplateFiles()
    {
        var settings = new Settings();
        settings.Load(@"TestData\empty-template-files.xml");
        Assert.AreEqual(1, settings.NumTitles);
        CollectionAssert.That.AreEqual(
            Enumerable.Empty<string>(), settings.GetSettingsPerTitle(settings.LastTitle).TemplateFiles);
    }

    [TestMethod]
    public void LoadTestInvalidTemplateFiles()
    {
        var settings = new Settings();
        settings.Load(@"TestData\invalid-template-files.xml");
        Assert.AreEqual(1, settings.NumTitles);
        CollectionAssert.That.AreEqual(
            Enumerable.Empty<string>(), settings.GetSettingsPerTitle(settings.LastTitle).TemplateFiles);
    }

    [TestMethod]
    public void LoadTestNoFontFamilyName()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-font-family-name.xml");
        Assert.AreEqual(SystemFonts.MessageFontFamily.Source, settings.FontFamilyName);
    }

    [TestMethod]
    public void LoadTestEmptyFontFamilyName()
    {
        var settings = new Settings();
        settings.Load(@"TestData\empty-font-family-name.xml");
        Assert.AreEqual(SystemFonts.MessageFontFamily.Source, settings.FontFamilyName);
    }

    [TestMethod]
    public void LoadTestNoFontSize()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-font-size.xml");
        Assert.AreEqual(SystemFonts.MessageFontSize, settings.FontSize);
    }

    [TestMethod]
    public void LoadTestEmptyFontSize()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\empty-font-size.xml"));
    }

    [TestMethod]
    public void LoadTestInvalidFontSize()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\invalid-font-size.xml"));
    }

    [TestMethod]
    public void LoadTestNegativeFontSize()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\negative-font-size.xml"));
    }

    [TestMethod]
    public void LoadTestZeroFontSize()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\zero-font-size.xml"));
    }

    [TestMethod]
    public void LoadTestExceededFontSize()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\exceeded-font-size.xml"));
    }

    [TestMethod]
    public void LoadTestNoOutputNumberGroupSeparator()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-output-number-group-separator.xml");
        Assert.AreEqual(true, settings.OutputNumberGroupSeparator);
    }

    [TestMethod]
    public void LoadTestEmptyOutputNumberGroupSeparator()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\empty-output-number-group-separator.xml"));
    }

    [TestMethod]
    public void LoadTestInvalidOutputNumberGroupSeparator()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\invalid-output-number-group-separator.xml"));
    }

    [TestMethod]
    public void LoadTestNoInputCodePageId()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-input-code-page-id.xml");
        Assert.AreEqual(65001, settings.InputCodePageId);
    }

    [TestMethod]
    public void LoadTestEmptyInputCodePageId()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\empty-input-code-page-id.xml"));
    }

    [TestMethod]
    public void LoadTestInvalidInputCodePageId()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\invalid-input-code-page-id.xml"));
    }

    [TestMethod]
    public void LoadTestUnsupportedInputCodePageId()
    {
        var settings = new Settings();
        settings.Load(@"TestData\unsupported-input-code-page-id.xml");
        Assert.AreEqual(65001, settings.InputCodePageId);
    }

    [TestMethod]
    public void LoadTestNoOutputCodePageId()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-output-code-page-id.xml");
        Assert.AreEqual(65001, settings.OutputCodePageId);
    }

    [TestMethod]
    public void LoadTestEmptyOutputCodePageId()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\empty-output-code-page-id.xml"));
    }

    [TestMethod]
    public void LoadTestInvalidOutputCodePageId()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\invalid-output-code-page-id.xml"));
    }

    [TestMethod]
    public void LoadTestUnsupportedOutputCodePageId()
    {
        var settings = new Settings();
        settings.Load(@"TestData\unsupported-output-code-page-id.xml");
        Assert.AreEqual(65001, settings.OutputCodePageId);
    }

    [TestMethod]
    public void LoadTestNoLanguage()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-language.xml");
        Assert.AreEqual(CultureInfo.InvariantCulture.Name, settings.Language);
    }

    [TestMethod]
    public void LoadTestEmptyLanguage()
    {
        var settings = new Settings();
        settings.Load(@"TestData\empty-language.xml");
        Assert.AreEqual(CultureInfo.InvariantCulture.Name, settings.Language);
    }

    [TestMethod]
    public void LoadTestInvalidLanguage()
    {
        var settings = new Settings();
        settings.Load(@"TestData\invalid-language.xml");
        Assert.AreEqual(CultureInfo.InvariantCulture.Name, settings.Language);
    }

    [TestMethod]
    public void LoadTestValidLanguageJaJp()
    {
        var settings = new Settings();
        settings.Load(@"TestData\valid-language-ja-JP.xml");
        Assert.AreEqual("ja-JP", settings.Language);
    }

    [TestMethod]
    public void LoadTestInvalidCharacter()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => new Settings().Load(@"TestData\invalid-character.xml"));
    }

    [TestMethod]
    public void SaveTest()
    {
        var settings = new Settings
        {
            LastTitle = "TH06",
            FontFamilyName = "font-family-name",
            FontSize = 12.3,
            OutputNumberGroupSeparator = false,
            InputCodePageId = 932,
            OutputCodePageId = 932,
        };

        var settingsPerTitle = settings.GetSettingsPerTitle(settings.LastTitle);
        settingsPerTitle.BestShotDirectory = "bestshot";
        settingsPerTitle.HideUntriedCards = true;
        settingsPerTitle.ImageOutputDirectory = "images";
        settingsPerTitle.OutputDirectory = "output";
        settingsPerTitle.ScoreFile = "score.dat";
        settingsPerTitle.TemplateFiles = new[] { "template1", "template2" };

        var tempfile = Path.GetTempFileName();
        settings.Save(tempfile);

        var settings2 = new Settings();
        settings2.Load(tempfile);
        File.Delete(tempfile);

        Assert.AreEqual(settings.LastTitle, settings2.LastTitle);
        Assert.AreEqual(1, settings2.NumTitles);

        var expected = settings.GetSettingsPerTitle(settings.LastTitle);
        var actual = settings2.GetSettingsPerTitle(settings2.LastTitle);
        Assert.AreEqual(expected.BestShotDirectory, actual.BestShotDirectory);
        Assert.AreEqual(expected.HideUntriedCards, actual.HideUntriedCards);
        Assert.AreEqual(expected.ImageOutputDirectory, actual.ImageOutputDirectory);
        Assert.AreEqual(expected.OutputDirectory, actual.OutputDirectory);
        Assert.AreEqual(expected.ScoreFile, actual.ScoreFile);
        CollectionAssert.That.AreEqual(expected.TemplateFiles, actual.TemplateFiles);

        Assert.AreEqual(settings.FontFamilyName, settings2.FontFamilyName);
        Assert.AreEqual(settings.FontSize, settings2.FontSize);
        Assert.AreEqual(settings.OutputNumberGroupSeparator, settings2.OutputNumberGroupSeparator);
        Assert.AreEqual(settings.InputCodePageId, settings2.InputCodePageId);
        Assert.AreEqual(settings.OutputCodePageId, settings2.OutputCodePageId);
    }

    [TestMethod]
    public void LastTitleTestNull()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new Settings().LastTitle = null!);
    }

    [TestMethod]
    public void FontFamilyNameTestNull()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new Settings().FontFamilyName = null!);
    }

    [TestMethod]
    public void FontSizeTestNull()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new Settings().FontSize = null);
    }

    [TestMethod]
    public void OutputNumberGroupSeparatorTestNull()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new Settings().OutputNumberGroupSeparator = null);
    }

    [TestMethod]
    public void InputCodePageIdTestNull()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new Settings().InputCodePageId = null);
    }

    [TestMethod]
    public void OutputCodePageIdTestNull()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new Settings().OutputCodePageId = null);
    }

    [TestMethod]
    public void LanguageTestNull()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new Settings().Language = null);
    }

    [TestMethod]
    public void LanguageTestInvalid()
    {
        var settings = new Settings();
        var language = settings.Language;
        settings.Language = "invalid";
        Assert.AreEqual(language, settings.Language);
    }

    [TestMethod]
    public void SetPropertyTest()
    {
        var numCalled = 0;
        void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ++numCalled;
        }

        var settings = new Settings();
        settings.PropertyChanged += OnPropertyChanged;
        try
        {
            var lastTitle = "TH06";
            settings.LastTitle = lastTitle;
            Assert.AreEqual(lastTitle, settings.LastTitle);
            Assert.AreEqual(1, numCalled);
        }
        finally
        {
            settings.PropertyChanged -= OnPropertyChanged;
        }
    }

    [TestMethod]
    public void SetPropertyTestNoChange()
    {
        var numCalled = 0;
        void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ++numCalled;
        }

        var settings = new Settings();
        settings.PropertyChanged += OnPropertyChanged;
        try
        {
            var lastTitle = settings.LastTitle;
            settings.LastTitle = lastTitle;
            Assert.AreEqual(lastTitle, settings.LastTitle);
            Assert.AreEqual(0, numCalled);
        }
        finally
        {
            settings.PropertyChanged -= OnPropertyChanged;
        }
    }
}
