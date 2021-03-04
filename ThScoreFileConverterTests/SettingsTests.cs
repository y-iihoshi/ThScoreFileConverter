using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests
{
    [TestClass]
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
            => _ = Assert.ThrowsException<ArgumentNullException>(() => new Settings().Load(null!));

        [TestMethod]
        public void LoadTestEmptyPath()
            => _ = Assert.ThrowsException<ArgumentException>(() => new Settings().Load(string.Empty));

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
        [DeploymentItem(@"TestData\empty.xml", @"TestData")]
        public void LoadTestEmptyFile()
            => _ = Assert.ThrowsException<InvalidDataException>(() => new Settings().Load(@"TestData\empty.xml"));

        [TestMethod]
        [DeploymentItem(@"TestData\no-root-node.xml", @"TestData")]
        public void LoadTestNoRootNode()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\no-root-node.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\invalid-root-node.xml", @"TestData")]
        public void LoadTestInvalidRootNode()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\invalid-root-node.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\wrong-namespace.xml", @"TestData")]
        public void LoadTestWrongNamespace()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\wrong-namespace.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-child-nodes.xml", @"TestData")]
        public void LoadTestNoChildNodes()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\no-child-nodes.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-last-title.xml", @"TestData")]
        public void LoadTestNoLastTitle()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\no-last-title.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-last-title.xml", @"TestData")]
        public void LoadTestEmptyLastTitle()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\empty-last-title.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\nonexistent-last-title.xml", @"TestData")]
        public void LoadTestNonexistentLastTitle()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\nonexistent-last-title.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-dictionary.xml", @"TestData")]
        public void LoadTestNoDictionary()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\no-dictionary.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-dictionary.xml", @"TestData")]
        public void LoadTestEmptyDictionary()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\empty-dictionary.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\invalid-key-value-types.xml", @"TestData")]
        public void LoadTestInvalidKeyValueTypes()
        {
            var settings = new Settings();
            settings.Load(@"TestData\invalid-key-value-types.xml");
            Assert.AreEqual(1, settings.NumTitles);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-key.xml", @"TestData")]
        public void LoadTestNoKey()
            => _ = Assert.ThrowsException<InvalidDataException>(() => new Settings().Load(@"TestData\no-key.xml"));

        [TestMethod]
        [DeploymentItem(@"TestData\empty-key.xml", @"TestData")]
        public void LoadTestEmptyKey()
        {
            var settings = new Settings();
            settings.Load(@"TestData\empty-key.xml");
            Assert.AreEqual(2, settings.NumTitles);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\unknown-key.xml", @"TestData")]
        public void LoadTestUnknownKey()
        {
            var settings = new Settings();
            settings.Load(@"TestData\unknown-key.xml");
            Assert.AreEqual(2, settings.NumTitles);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-value.xml", @"TestData")]
        public void LoadTestNoValue()
            => _ = Assert.ThrowsException<InvalidDataException>(() => new Settings().Load(@"TestData\no-value.xml"));

        [TestMethod]
        [DeploymentItem(@"TestData\empty-value.xml", @"TestData")]
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
        [DeploymentItem(@"TestData\no-best-shot-directory.xml", @"TestData")]
        public void LoadTestNoBestShotDirectory()
        {
            var settings = new Settings();
            settings.Load(@"TestData\no-best-shot-directory.xml");
            Assert.AreEqual(1, settings.NumTitles);
            Assert.AreEqual(string.Empty, settings.GetSettingsPerTitle(settings.LastTitle).BestShotDirectory);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-best-shot-directory.xml", @"TestData")]
        public void LoadTestEmptyBestShotDirectory()
        {
            var settings = new Settings();
            settings.Load(@"TestData\empty-best-shot-directory.xml");
            Assert.AreEqual(1, settings.NumTitles);
            Assert.AreEqual(string.Empty, settings.GetSettingsPerTitle(settings.LastTitle).BestShotDirectory);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-hide-untried-cards.xml", @"TestData")]
        public void LoadTestNoHideUntriedCards()
        {
            var settings = new Settings();
            settings.Load(@"TestData\no-hide-untried-cards.xml");
            Assert.AreEqual(1, settings.NumTitles);
            Assert.IsTrue(settings.GetSettingsPerTitle(settings.LastTitle).HideUntriedCards);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-hide-untried-cards.xml", @"TestData")]
        public void LoadTestEmptyHideUntriedCards()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\empty-hide-untried-cards.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\invalid-hide-untried-cards.xml", @"TestData")]
        public void LoadTestInvalidHideUntriedCards()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\invalid-hide-untried-cards.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-image-output-directory.xml", @"TestData")]
        public void LoadTestNoImageOutputDirectory()
        {
            var settings = new Settings();
            settings.Load(@"TestData\no-image-output-directory.xml");
            Assert.AreEqual(1, settings.NumTitles);
            Assert.AreEqual(string.Empty, settings.GetSettingsPerTitle(settings.LastTitle).ImageOutputDirectory);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-image-output-directory.xml", @"TestData")]
        public void LoadTestEmptyImageOutputDirectory()
        {
            var settings = new Settings();
            settings.Load(@"TestData\empty-image-output-directory.xml");
            Assert.AreEqual(1, settings.NumTitles);
            Assert.AreEqual(string.Empty, settings.GetSettingsPerTitle(settings.LastTitle).ImageOutputDirectory);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-output-directory.xml", @"TestData")]
        public void LoadTestNoOutputDirectory()
        {
            var settings = new Settings();
            settings.Load(@"TestData\no-output-directory.xml");
            Assert.AreEqual(1, settings.NumTitles);
            Assert.AreEqual(string.Empty, settings.GetSettingsPerTitle(settings.LastTitle).OutputDirectory);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-output-directory.xml", @"TestData")]
        public void LoadTestEmptyOutputDirectory()
        {
            var settings = new Settings();
            settings.Load(@"TestData\empty-output-directory.xml");
            Assert.AreEqual(1, settings.NumTitles);
            Assert.AreEqual(string.Empty, settings.GetSettingsPerTitle(settings.LastTitle).OutputDirectory);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-score-file.xml", @"TestData")]
        public void LoadTestNoScoreFile()
        {
            var settings = new Settings();
            settings.Load(@"TestData\no-score-file.xml");
            Assert.AreEqual(1, settings.NumTitles);
            Assert.AreEqual(string.Empty, settings.GetSettingsPerTitle(settings.LastTitle).ScoreFile);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-score-file.xml", @"TestData")]
        public void LoadTestEmptyScoreFile()
        {
            var settings = new Settings();
            settings.Load(@"TestData\empty-score-file.xml");
            Assert.AreEqual(1, settings.NumTitles);
            Assert.AreEqual(string.Empty, settings.GetSettingsPerTitle(settings.LastTitle).ScoreFile);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-template-files.xml", @"TestData")]
        public void LoadTestNoTemplateFiles()
        {
            var settings = new Settings();
            settings.Load(@"TestData\no-template-files.xml");
            Assert.AreEqual(1, settings.NumTitles);
            CollectionAssert.That.AreEqual(
                Enumerable.Empty<string>(), settings.GetSettingsPerTitle(settings.LastTitle).TemplateFiles);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-template-files.xml", @"TestData")]
        public void LoadTestEmptyTemplateFiles()
        {
            var settings = new Settings();
            settings.Load(@"TestData\empty-template-files.xml");
            Assert.AreEqual(1, settings.NumTitles);
            CollectionAssert.That.AreEqual(
                Enumerable.Empty<string>(), settings.GetSettingsPerTitle(settings.LastTitle).TemplateFiles);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\invalid-template-files.xml", @"TestData")]
        public void LoadTestInvalidTemplateFiles()
        {
            var settings = new Settings();
            settings.Load(@"TestData\invalid-template-files.xml");
            Assert.AreEqual(1, settings.NumTitles);
            CollectionAssert.That.AreEqual(
                Enumerable.Empty<string>(), settings.GetSettingsPerTitle(settings.LastTitle).TemplateFiles);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-font-family-name.xml", @"TestData")]
        public void LoadTestNoFontFamilyName()
        {
            var settings = new Settings();
            settings.Load(@"TestData\no-font-family-name.xml");
            Assert.AreEqual(SystemFonts.MessageFontFamily.Source, settings.FontFamilyName);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-font-family-name.xml", @"TestData")]
        public void LoadTestEmptyFontFamilyName()
        {
            var settings = new Settings();
            settings.Load(@"TestData\empty-font-family-name.xml");
            Assert.AreEqual(SystemFonts.MessageFontFamily.Source, settings.FontFamilyName);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-font-size.xml", @"TestData")]
        public void LoadTestNoFontSize()
        {
            var settings = new Settings();
            settings.Load(@"TestData\no-font-size.xml");
            Assert.AreEqual(SystemFonts.MessageFontSize, settings.FontSize);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-font-size.xml", @"TestData")]
        public void LoadTestEmptyFontSize()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\empty-font-size.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\invalid-font-size.xml", @"TestData")]
        public void LoadTestInvalidFontSize()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\invalid-font-size.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\negative-font-size.xml", @"TestData")]
        public void LoadTestNegativeFontSize()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\negative-font-size.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\zero-font-size.xml", @"TestData")]
        public void LoadTestZeroFontSize()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\zero-font-size.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\exceeded-font-size.xml", @"TestData")]
        public void LoadTestExceededFontSize()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\exceeded-font-size.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-output-number-group-separator.xml", @"TestData")]
        public void LoadTestNoOutputNumberGroupSeparator()
        {
            var settings = new Settings();
            settings.Load(@"TestData\no-output-number-group-separator.xml");
            Assert.AreEqual(true, settings.OutputNumberGroupSeparator);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-output-number-group-separator.xml", @"TestData")]
        public void LoadTestEmptyOutputNumberGroupSeparator()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\empty-output-number-group-separator.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\invalid-output-number-group-separator.xml", @"TestData")]
        public void LoadTestInvalidOutputNumberGroupSeparator()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\invalid-output-number-group-separator.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-input-code-page-id.xml", @"TestData")]
        public void LoadTestNoInputCodePageId()
        {
            var settings = new Settings();
            settings.Load(@"TestData\no-input-code-page-id.xml");
            Assert.AreEqual(65001, settings.InputCodePageId);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-input-code-page-id.xml", @"TestData")]
        public void LoadTestEmptyInputCodePageId()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\empty-input-code-page-id.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\invalid-input-code-page-id.xml", @"TestData")]
        public void LoadTestInvalidInputCodePageId()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\invalid-input-code-page-id.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\unsupported-input-code-page-id.xml", @"TestData")]
        public void LoadTestUnsupportedInputCodePageId()
        {
            var settings = new Settings();
            settings.Load(@"TestData\unsupported-input-code-page-id.xml");
            Assert.AreEqual(65001, settings.InputCodePageId);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-output-code-page-id.xml", @"TestData")]
        public void LoadTestNoOutputCodePageId()
        {
            var settings = new Settings();
            settings.Load(@"TestData\no-output-code-page-id.xml");
            Assert.AreEqual(65001, settings.OutputCodePageId);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-output-code-page-id.xml", @"TestData")]
        public void LoadTestEmptyOutputCodePageId()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\empty-output-code-page-id.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\invalid-output-code-page-id.xml", @"TestData")]
        public void LoadTestInvalidOutputCodePageId()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => new Settings().Load(@"TestData\invalid-output-code-page-id.xml"));
        }

        [TestMethod]
        [DeploymentItem(@"TestData\unsupported-output-code-page-id.xml", @"TestData")]
        public void LoadTestUnsupportedOutputCodePageId()
        {
            var settings = new Settings();
            settings.Load(@"TestData\unsupported-output-code-page-id.xml");
            Assert.AreEqual(65001, settings.OutputCodePageId);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-language.xml", @"TestData")]
        public void LoadTestNoLanguage()
        {
            var settings = new Settings();
            settings.Load(@"TestData\no-language.xml");
            Assert.AreEqual(CultureInfo.InvariantCulture.Name, settings.Language);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-language.xml", @"TestData")]
        public void LoadTestEmptyLanguage()
        {
            var settings = new Settings();
            settings.Load(@"TestData\empty-language.xml");
            Assert.AreEqual(CultureInfo.InvariantCulture.Name, settings.Language);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\invalid-language.xml", @"TestData")]
        public void LoadTestInvalidLanguage()
        {
            var settings = new Settings();
            settings.Load(@"TestData\invalid-language.xml");
            Assert.AreEqual(CultureInfo.InvariantCulture.Name, settings.Language);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\valid-language-ja-JP.xml", @"TestData")]
        public void LoadTestValidLanguageJaJp()
        {
            var settings = new Settings();
            settings.Load(@"TestData\valid-language-ja-JP.xml");
            Assert.AreEqual("ja-JP", settings.Language);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\invalid-character.xml", @"TestData")]
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
            var actual= settings2.GetSettingsPerTitle(settings2.LastTitle);
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
            => _ = Assert.ThrowsException<ArgumentNullException>(() => new Settings().LastTitle = null!);

        [TestMethod]
        public void FontFamilyNameTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => new Settings().FontFamilyName = null!);

        [TestMethod]
        public void FontSizeTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => new Settings().FontSize = null);

        [TestMethod]
        public void OutputNumberGroupSeparatorTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => new Settings().OutputNumberGroupSeparator = null);

        [TestMethod]
        public void InputCodePageIdTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => new Settings().InputCodePageId = null);

        [TestMethod]
        public void OutputCodePageIdTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => new Settings().OutputCodePageId = null);

        [TestMethod]
        public void LanguageTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => new Settings().Language = null);

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
}
