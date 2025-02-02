using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace ThScoreFileConverter.Tests;

[TestClass]
public class SettingsTests
{
    [TestMethod]
    public void SettingsTest()
    {
        var settings = new Settings();
        settings.LastTitle.ShouldBeEmpty();
        settings.NumTitles.ShouldBe(0);
        settings.FontFamilyName.ShouldBe(SystemFonts.MessageFontFamily.Source);
        settings.FontSize.ShouldBe(SystemFonts.MessageFontSize);
        settings.OutputNumberGroupSeparator.ShouldBe(true);
        settings.InputCodePageId.ShouldBe(65001);
        settings.OutputCodePageId.ShouldBe(65001);
        settings.Language.ShouldBe(CultureInfo.InvariantCulture.Name);
        settings.WindowWidth.ShouldBe(Settings.WindowMinWidth);
        settings.WindowHeight.ShouldBe(Settings.WindowMinHeight);
        settings.MainContentHeight.ShouldBe(Settings.MainContentMinHeight);
        settings.SubContentHeight.ShouldBe(Settings.SubContentMinHeight);
    }

    [TestMethod]
    public void LoadTestNullPath()
    {
        _ = Should.Throw<ArgumentNullException>(() => new Settings().Load(null!));
    }

    [TestMethod]
    public void LoadTestEmptyPath()
    {
        _ = Should.Throw<ArgumentException>(() => new Settings().Load(string.Empty));
    }

    [TestMethod]
    public void LoadTestNonexistentPath()
    {
        var settings = new Settings();
        settings.Load(@"TestData\nonexistent.xml");
        settings.LastTitle.ShouldBeEmpty();
        settings.NumTitles.ShouldBe(0);
        settings.FontFamilyName.ShouldBe(SystemFonts.MessageFontFamily.Source);
        settings.FontSize.ShouldBe(SystemFonts.MessageFontSize);
        settings.OutputNumberGroupSeparator.ShouldBe(true);
        settings.InputCodePageId.ShouldBe(65001);
        settings.OutputCodePageId.ShouldBe(65001);
        settings.Language.ShouldBe(CultureInfo.InvariantCulture.Name);
        settings.WindowWidth.ShouldBe(Settings.WindowMinWidth);
        settings.WindowHeight.ShouldBe(Settings.WindowMinHeight);
        settings.MainContentHeight.ShouldBe(Settings.MainContentMinHeight);
        settings.SubContentHeight.ShouldBe(Settings.SubContentMinHeight);
    }

    [TestMethod]
    public void LoadTestEmptyFile()
    {
        _ = Should.Throw<InvalidDataException>(() => new Settings().Load(@"TestData\empty.xml"));
    }

    [TestMethod]
    public void LoadTestNoRootNode()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\no-root-node.xml"));
    }

    [TestMethod]
    public void LoadTestInvalidRootNode()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\invalid-root-node.xml"));
    }

    [TestMethod]
    public void LoadTestWrongNamespace()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\wrong-namespace.xml"));
    }

    [TestMethod]
    public void LoadTestNoChildNodes()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\no-child-nodes.xml"));
    }

    [TestMethod]
    public void LoadTestNoLastTitle()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\no-last-title.xml"));
    }

    [TestMethod]
    public void LoadTestEmptyLastTitle()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\empty-last-title.xml"));
    }

    [TestMethod]
    public void LoadTestNonexistentLastTitle()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\nonexistent-last-title.xml"));
    }

    [TestMethod]
    public void LoadTestNoDictionary()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\no-dictionary.xml"));
    }

    [TestMethod]
    public void LoadTestEmptyDictionary()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\empty-dictionary.xml"));
    }

    [TestMethod]
    public void LoadTestInvalidKeyValueTypes()
    {
        var settings = new Settings();
        settings.Load(@"TestData\invalid-key-value-types.xml");
        settings.NumTitles.ShouldBe(1);
    }

    [TestMethod]
    public void LoadTestNoKey()
    {
        _ = Should.Throw<InvalidDataException>(() => new Settings().Load(@"TestData\no-key.xml"));
    }

    [TestMethod]
    public void LoadTestEmptyKey()
    {
        var settings = new Settings();
        settings.Load(@"TestData\empty-key.xml");
        settings.NumTitles.ShouldBe(2);
    }

    [TestMethod]
    public void LoadTestUnknownKey()
    {
        var settings = new Settings();
        settings.Load(@"TestData\unknown-key.xml");
        settings.NumTitles.ShouldBe(2);
    }

    [TestMethod]
    public void LoadTestNoValue()
    {
        _ = Should.Throw<InvalidDataException>(() => new Settings().Load(@"TestData\no-value.xml"));
    }

    [TestMethod]
    public void LoadTestEmptyValue()
    {
        var settings = new Settings();
        settings.Load(@"TestData\empty-value.xml");

        settings.NumTitles.ShouldBe(1);

        var expected = new SettingsPerTitle();
        var actual = settings.GetSettingsPerTitle(settings.LastTitle);
        actual.BestShotDirectory.ShouldBe(expected.BestShotDirectory);
        actual.HideUntriedCards.ShouldBe(expected.HideUntriedCards);
        actual.ImageOutputDirectory.ShouldBe(expected.ImageOutputDirectory);
        actual.OutputDirectory.ShouldBe(expected.OutputDirectory);
        actual.ScoreFile.ShouldBe(expected.ScoreFile);
        actual.TemplateFiles.ShouldBe(expected.TemplateFiles);
    }

    [TestMethod]
    public void LoadTestNoBestShotDirectory()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-best-shot-directory.xml");
        settings.NumTitles.ShouldBe(1);
        settings.GetSettingsPerTitle(settings.LastTitle).BestShotDirectory.ShouldBeEmpty();
    }

    [TestMethod]
    public void LoadTestEmptyBestShotDirectory()
    {
        var settings = new Settings();
        settings.Load(@"TestData\empty-best-shot-directory.xml");
        settings.NumTitles.ShouldBe(1);
        settings.GetSettingsPerTitle(settings.LastTitle).BestShotDirectory.ShouldBeEmpty();
    }

    [TestMethod]
    public void LoadTestNoHideUntriedCards()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-hide-untried-cards.xml");
        settings.NumTitles.ShouldBe(1);
        settings.GetSettingsPerTitle(settings.LastTitle).HideUntriedCards.ShouldBeTrue();
    }

    [TestMethod]
    public void LoadTestEmptyHideUntriedCards()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\empty-hide-untried-cards.xml"));
    }

    [TestMethod]
    public void LoadTestInvalidHideUntriedCards()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\invalid-hide-untried-cards.xml"));
    }

    [TestMethod]
    public void LoadTestNoImageOutputDirectory()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-image-output-directory.xml");
        settings.NumTitles.ShouldBe(1);
        settings.GetSettingsPerTitle(settings.LastTitle).ImageOutputDirectory.ShouldBeEmpty();
    }

    [TestMethod]
    public void LoadTestEmptyImageOutputDirectory()
    {
        var settings = new Settings();
        settings.Load(@"TestData\empty-image-output-directory.xml");
        settings.NumTitles.ShouldBe(1);
        settings.GetSettingsPerTitle(settings.LastTitle).ImageOutputDirectory.ShouldBeEmpty();
    }

    [TestMethod]
    public void LoadTestNoOutputDirectory()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-output-directory.xml");
        settings.NumTitles.ShouldBe(1);
        settings.GetSettingsPerTitle(settings.LastTitle).OutputDirectory.ShouldBeEmpty();
    }

    [TestMethod]
    public void LoadTestEmptyOutputDirectory()
    {
        var settings = new Settings();
        settings.Load(@"TestData\empty-output-directory.xml");
        settings.NumTitles.ShouldBe(1);
        settings.GetSettingsPerTitle(settings.LastTitle).OutputDirectory.ShouldBeEmpty();
    }

    [TestMethod]
    public void LoadTestNoScoreFile()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-score-file.xml");
        settings.NumTitles.ShouldBe(1);
        settings.GetSettingsPerTitle(settings.LastTitle).ScoreFile.ShouldBeEmpty();
    }

    [TestMethod]
    public void LoadTestEmptyScoreFile()
    {
        var settings = new Settings();
        settings.Load(@"TestData\empty-score-file.xml");
        settings.NumTitles.ShouldBe(1);
        settings.GetSettingsPerTitle(settings.LastTitle).ScoreFile.ShouldBeEmpty();
    }

    [TestMethod]
    public void LoadTestNoTemplateFiles()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-template-files.xml");
        settings.NumTitles.ShouldBe(1);
        settings.GetSettingsPerTitle(settings.LastTitle).TemplateFiles.ShouldBeEmpty();
    }

    [TestMethod]
    public void LoadTestEmptyTemplateFiles()
    {
        var settings = new Settings();
        settings.Load(@"TestData\empty-template-files.xml");
        settings.NumTitles.ShouldBe(1);
        settings.GetSettingsPerTitle(settings.LastTitle).TemplateFiles.ShouldBeEmpty();
    }

    [TestMethod]
    public void LoadTestInvalidTemplateFiles()
    {
        var settings = new Settings();
        settings.Load(@"TestData\invalid-template-files.xml");
        settings.NumTitles.ShouldBe(1);
        settings.GetSettingsPerTitle(settings.LastTitle).TemplateFiles.ShouldBeEmpty();
    }

    [TestMethod]
    public void LoadTestNoFontFamilyName()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-optional-settings.xml");
        settings.FontFamilyName.ShouldBe(SystemFonts.MessageFontFamily.Source);
    }

    [TestMethod]
    public void LoadTestEmptyFontFamilyName()
    {
        var settings = new Settings();
        settings.Load(@"TestData\empty-font-family-name.xml");
        settings.FontFamilyName.ShouldBe(SystemFonts.MessageFontFamily.Source);
    }

    [TestMethod]
    public void LoadTestNoFontSize()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-optional-settings.xml");
        settings.FontSize.ShouldBe(SystemFonts.MessageFontSize);
    }

    [TestMethod]
    public void LoadTestEmptyFontSize()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\empty-font-size.xml"));
    }

    [TestMethod]
    public void LoadTestInvalidFontSize()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\invalid-font-size.xml"));
    }

    [TestMethod]
    public void LoadTestNegativeFontSize()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\negative-font-size.xml"));
    }

    [TestMethod]
    public void LoadTestZeroFontSize()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\zero-font-size.xml"));
    }

    [TestMethod]
    public void LoadTestExceededFontSize()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\exceeded-font-size.xml"));
    }

    [TestMethod]
    public void LoadTestNoOutputNumberGroupSeparator()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-optional-settings.xml");
        settings.OutputNumberGroupSeparator.ShouldBe(true);
    }

    [TestMethod]
    public void LoadTestEmptyOutputNumberGroupSeparator()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\empty-output-number-group-separator.xml"));
    }

    [TestMethod]
    public void LoadTestInvalidOutputNumberGroupSeparator()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\invalid-output-number-group-separator.xml"));
    }

    [TestMethod]
    public void LoadTestNoInputCodePageId()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-optional-settings.xml");
        settings.InputCodePageId.ShouldBe(65001);
    }

    [TestMethod]
    public void LoadTestEmptyInputCodePageId()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\empty-input-code-page-id.xml"));
    }

    [TestMethod]
    public void LoadTestInvalidInputCodePageId()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\invalid-input-code-page-id.xml"));
    }

    [TestMethod]
    public void LoadTestUnsupportedInputCodePageId()
    {
        var settings = new Settings();
        settings.Load(@"TestData\unsupported-input-code-page-id.xml");
        settings.InputCodePageId.ShouldBe(65001);
    }

    [TestMethod]
    public void LoadTestNoOutputCodePageId()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-optional-settings.xml");
        settings.OutputCodePageId.ShouldBe(65001);
    }

    [TestMethod]
    public void LoadTestEmptyOutputCodePageId()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\empty-output-code-page-id.xml"));
    }

    [TestMethod]
    public void LoadTestInvalidOutputCodePageId()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\invalid-output-code-page-id.xml"));
    }

    [TestMethod]
    public void LoadTestUnsupportedOutputCodePageId()
    {
        var settings = new Settings();
        settings.Load(@"TestData\unsupported-output-code-page-id.xml");
        settings.OutputCodePageId.ShouldBe(65001);
    }

    [TestMethod]
    public void LoadTestNoLanguage()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-optional-settings.xml");
        settings.Language.ShouldBe(CultureInfo.InvariantCulture.Name);
    }

    [TestMethod]
    public void LoadTestEmptyLanguage()
    {
        var settings = new Settings();
        settings.Load(@"TestData\empty-language.xml");
        settings.Language.ShouldBe(CultureInfo.InvariantCulture.Name);
    }

    [TestMethod]
    public void LoadTestInvalidLanguage()
    {
        var settings = new Settings();
        settings.Load(@"TestData\invalid-language.xml");
        settings.Language.ShouldBe(CultureInfo.InvariantCulture.Name);
    }

    [TestMethod]
    public void LoadTestValidLanguageJaJp()
    {
        var settings = new Settings();
        settings.Load(@"TestData\valid-language-ja-JP.xml");
        settings.Language.ShouldBe("ja-JP");
    }

    [TestMethod]
    public void LoadTestNoWindowWidth()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-optional-settings.xml");
        settings.WindowWidth.ShouldBe(Settings.WindowMinWidth);
    }

    [TestMethod]
    public void LoadTestEmptyWindowWidth()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\empty-window-width.xml"));
    }

    [TestMethod]
    public void LoadTestInvalidWindowWidth()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\invalid-window-width.xml"));
    }

    [TestMethod]
    public void LoadTestNegativeWindowWidth()
    {
        var settings = new Settings();
        settings.Load(@"TestData\negative-window-width.xml");
        settings.WindowWidth.ShouldBe(Settings.WindowMinWidth);
    }

    [TestMethod]
    public void LoadTestZeroWindowWidth()
    {
        var settings = new Settings();
        settings.Load(@"TestData\zero-window-width.xml");
        settings.WindowWidth.ShouldBe(Settings.WindowMinWidth);
    }

    [TestMethod]
    public void LoadTestBelowWindowWidth()
    {
        var settings = new Settings();
        settings.Load(@"TestData\below-window-width.xml");
        settings.WindowWidth.ShouldBe(Settings.WindowMinWidth);
    }

    [TestMethod]
    public void LoadTestNoWindowHeight()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-optional-settings.xml");
        settings.WindowHeight.ShouldBe(Settings.WindowMinHeight);
    }

    [TestMethod]
    public void LoadTestEmptyWindowHeight()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\empty-window-height.xml"));
    }

    [TestMethod]
    public void LoadTestInvalidWindowHeight()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\invalid-window-height.xml"));
    }

    [TestMethod]
    public void LoadTestNegativeWindowHeight()
    {
        var settings = new Settings();
        settings.Load(@"TestData\negative-window-height.xml");
        settings.WindowHeight.ShouldBe(Settings.WindowMinHeight);
    }

    [TestMethod]
    public void LoadTestZeroWindowHeight()
    {
        var settings = new Settings();
        settings.Load(@"TestData\zero-window-height.xml");
        settings.WindowHeight.ShouldBe(Settings.WindowMinHeight);
    }

    [TestMethod]
    public void LoadTestBelowWindowHeight()
    {
        var settings = new Settings();
        settings.Load(@"TestData\below-window-height.xml");
        settings.WindowHeight.ShouldBe(Settings.WindowMinHeight);
    }

    [TestMethod]
    public void LoadTestNoMainContentHeight()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-optional-settings.xml");
        settings.MainContentHeight.ShouldBe(Settings.MainContentMinHeight);
    }

    [TestMethod]
    public void LoadTestEmptyMainContentHeight()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\empty-main-content-height.xml"));
    }

    [TestMethod]
    public void LoadTestInvalidMainContentHeight()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\invalid-main-content-height.xml"));
    }

    [TestMethod]
    public void LoadTestNegativeMainContentHeight()
    {
        var settings = new Settings();
        settings.Load(@"TestData\negative-main-content-height.xml");
        settings.MainContentHeight.ShouldBe(Settings.MainContentMinHeight);
    }

    [TestMethod]
    public void LoadTestZeroMainContentHeight()
    {
        var settings = new Settings();
        settings.Load(@"TestData\zero-main-content-height.xml");
        settings.MainContentHeight.ShouldBe(Settings.MainContentMinHeight);
    }

    [TestMethod]
    public void LoadTestBelowMainContentHeight()
    {
        var settings = new Settings();
        settings.Load(@"TestData\below-main-content-height.xml");
        settings.MainContentHeight.ShouldBe(Settings.MainContentMinHeight);
    }

    [TestMethod]
    public void LoadTestNoSubContentHeight()
    {
        var settings = new Settings();
        settings.Load(@"TestData\no-optional-settings.xml");
        settings.SubContentHeight.ShouldBe(Settings.SubContentMinHeight);
    }

    [TestMethod]
    public void LoadTestEmptySubContentHeight()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\empty-sub-content-height.xml"));
    }

    [TestMethod]
    public void LoadTestInvalidSubContentHeight()
    {
        _ = Should.Throw<InvalidDataException>(
            () => new Settings().Load(@"TestData\invalid-sub-content-height.xml"));
    }

    [TestMethod]
    public void LoadTestNegativeSubContentHeight()
    {
        var settings = new Settings();
        settings.Load(@"TestData\negative-sub-content-height.xml");
        settings.SubContentHeight.ShouldBe(Settings.SubContentMinHeight);
    }

    [TestMethod]
    public void LoadTestZeroSubContentHeight()
    {
        var settings = new Settings();
        settings.Load(@"TestData\zero-sub-content-height.xml");
        settings.SubContentHeight.ShouldBe(Settings.SubContentMinHeight);
    }

    [TestMethod]
    public void LoadTestBelowSubContentHeight()
    {
        var settings = new Settings();
        settings.Load(@"TestData\below-sub-content-height.xml");
        settings.SubContentHeight.ShouldBe(Settings.SubContentMinHeight);
    }

    [TestMethod]
    public void LoadTestInvalidCharacter()
    {
        _ = Should.Throw<InvalidDataException>(
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
            Language = "ja-JP",
            WindowWidth = 480.5,
            WindowHeight = 480.5,
            MainContentHeight = 240.5,
            SubContentHeight = 80.5,
        };

        var settingsPerTitle = settings.GetSettingsPerTitle(settings.LastTitle);
        settingsPerTitle.BestShotDirectory = "bestshot";
        settingsPerTitle.HideUntriedCards = true;
        settingsPerTitle.ImageOutputDirectory = "images";
        settingsPerTitle.OutputDirectory = "output";
        settingsPerTitle.ScoreFile = "score.dat";
        settingsPerTitle.TemplateFiles = ["template1", "template2"];

        var tempfile = Path.GetTempFileName();
        settings.Save(tempfile);

        var settings2 = new Settings();
        settings2.Load(tempfile);
        File.Delete(tempfile);

        settings2.LastTitle.ShouldBe(settings.LastTitle);
        settings2.NumTitles.ShouldBe(1);

        var expected = settings.GetSettingsPerTitle(settings.LastTitle);
        var actual = settings2.GetSettingsPerTitle(settings2.LastTitle);
        actual.BestShotDirectory.ShouldBe(expected.BestShotDirectory);
        actual.HideUntriedCards.ShouldBe(expected.HideUntriedCards);
        actual.ImageOutputDirectory.ShouldBe(expected.ImageOutputDirectory);
        actual.OutputDirectory.ShouldBe(expected.OutputDirectory);
        actual.ScoreFile.ShouldBe(expected.ScoreFile);
        actual.TemplateFiles.ShouldBe(expected.TemplateFiles);

        settings2.FontFamilyName.ShouldBe(settings.FontFamilyName);
        settings2.FontSize.ShouldBe(settings.FontSize);
        settings2.OutputNumberGroupSeparator.ShouldBe(settings.OutputNumberGroupSeparator);
        settings2.InputCodePageId.ShouldBe(settings.InputCodePageId);
        settings2.OutputCodePageId.ShouldBe(settings.OutputCodePageId);
        settings2.Language.ShouldBe(settings.Language);

        settings2.WindowWidth.ShouldBe(settings.WindowWidth);
        settings2.WindowHeight.ShouldBe(settings.WindowHeight);
        settings2.MainContentHeight.ShouldBe(settings.MainContentHeight);
        settings2.SubContentHeight.ShouldBe(settings.SubContentHeight);
    }

    [TestMethod]
    public void LastTitleTestNull()
    {
        _ = Should.Throw<ArgumentNullException>(() => new Settings().LastTitle = null!);
    }

    [TestMethod]
    public void FontFamilyNameTestNull()
    {
        _ = Should.Throw<ArgumentNullException>(() => new Settings().FontFamilyName = null!);
    }

    [TestMethod]
    public void FontSizeTestNull()
    {
        _ = Should.Throw<ArgumentNullException>(() => new Settings().FontSize = null);
    }

    [TestMethod]
    public void OutputNumberGroupSeparatorTestNull()
    {
        _ = Should.Throw<ArgumentNullException>(() => new Settings().OutputNumberGroupSeparator = null);
    }

    [TestMethod]
    public void InputCodePageIdTestNull()
    {
        _ = Should.Throw<ArgumentNullException>(() => new Settings().InputCodePageId = null);
    }

    [TestMethod]
    public void OutputCodePageIdTestNull()
    {
        _ = Should.Throw<ArgumentNullException>(() => new Settings().OutputCodePageId = null);
    }

    [TestMethod]
    public void LanguageTestNull()
    {
        _ = Should.Throw<ArgumentNullException>(() => new Settings().Language = null);
    }

    [TestMethod]
    public void LanguageTestInvalid()
    {
        var settings = new Settings();
        var language = settings.Language;
        settings.Language = "invalid";
        settings.Language.ShouldBe(language);
    }

    [TestMethod]
    public void WindowWidthTestNull()
    {
        _ = Should.Throw<ArgumentNullException>(() => new Settings().WindowWidth = null);
    }

    [TestMethod]
    public void WindowHeightTestNull()
    {
        _ = Should.Throw<ArgumentNullException>(() => new Settings().WindowHeight = null);
    }

    [TestMethod]
    public void MainContentHeightTestNull()
    {
        _ = Should.Throw<ArgumentNullException>(() => new Settings().MainContentHeight = null);
    }

    [TestMethod]
    public void SubContentHeightTestNull()
    {
        _ = Should.Throw<ArgumentNullException>(() => new Settings().SubContentHeight = null);
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
            settings.LastTitle.ShouldBe(lastTitle);
            numCalled.ShouldBe(1);
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
            settings.LastTitle.ShouldBe(lastTitle);
            numCalled.ShouldBe(0);
        }
        finally
        {
            settings.PropertyChanged -= OnPropertyChanged;
        }
    }
}
