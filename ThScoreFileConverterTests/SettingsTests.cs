using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models;

namespace ThScoreFileConverterTests
{
    [TestClass]
    public class SettingsTests
    {
        internal static string GetBackingFieldName(string propertyName) => $"<{propertyName}>k__BackingField";

        internal static T CreateInstance<T>(params object[] parameters)
        {
            var bindingFlags =
                BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return (T)typeof(T).InvokeMember(null, bindingFlags, null, null, parameters);
        }

        [TestInitialize]
        public void Initialize()
        {
            // Reset the singleton instance per test case.
            new PrivateType(typeof(Settings)).SetStaticField(
                GetBackingFieldName(nameof(Settings.Instance)), CreateInstance<Settings>());
        }

        [TestMethod]
        public void InstanceTest()
        {
            var instance1 = Settings.Instance;
            var instance2 = Settings.Instance;
            Assert.AreSame(instance1, instance2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoadTestNullPath()
        {
            Settings.Instance.Load(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LoadTestEmptyPath()
        {
            Settings.Instance.Load(string.Empty);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void LoadTestNonexistentPath()
        {
            Settings.Instance.Load(@"TestData\nonexistent.xml");
            Assert.AreEqual(string.Empty, Settings.Instance.LastTitle);
            Assert.AreEqual(0, Settings.Instance.Dictionary.Count);
            Assert.AreEqual(SystemFonts.MessageFontFamily.Source, Settings.Instance.FontFamilyName);
            Assert.AreEqual(SystemFonts.MessageFontSize, Settings.Instance.FontSize);
            Assert.AreEqual(true, Settings.Instance.OutputNumberGroupSeparator);
            Assert.AreEqual(65001, Settings.Instance.InputCodePageId);
            Assert.AreEqual(65001, Settings.Instance.OutputCodePageId);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestEmptyFile()
        {
            Settings.Instance.Load(@"TestData\empty.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-root-node.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestNoRootNode()
        {
            Settings.Instance.Load(@"TestData\no-root-node.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\invalid-root-node.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestInvalidRootNode()
        {
            Settings.Instance.Load(@"TestData\invalid-root-node.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\wrong-namespace.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestWrongNamespace()
        {
            Settings.Instance.Load(@"TestData\wrong-namespace.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-child-nodes.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestNoChildNodes()
        {
            Settings.Instance.Load(@"TestData\no-child-nodes.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-last-title.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestNoLastTitle()
        {
            Settings.Instance.Load(@"TestData\no-last-title.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-last-title.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestEmptyLastTitle()
        {
            Settings.Instance.Load(@"TestData\empty-last-title.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\nonexistent-last-title.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestNonexistentLastTitle()
        {
            Settings.Instance.Load(@"TestData\nonexistent-last-title.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-dictionary.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestNoDictionary()
        {
            Settings.Instance.Load(@"TestData\no-dictionary.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-dictionary.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestEmptyDictionary()
        {
            Settings.Instance.Load(@"TestData\empty-dictionary.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\invalid-key-value-types.xml", @"TestData")]
        public void LoadTestInvalidKeyValueTypes()
        {
            Settings.Instance.Load(@"TestData\invalid-key-value-types.xml");
            Assert.AreEqual(1, Settings.Instance.Dictionary.Count);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-key.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestNoKey()
        {
            Settings.Instance.Load(@"TestData\no-key.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-key.xml", @"TestData")]
        public void LoadTestEmptyKey()
        {
            Settings.Instance.Load(@"TestData\empty-key.xml");
            Assert.AreEqual(2, Settings.Instance.Dictionary.Count);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\unknown-key.xml", @"TestData")]
        public void LoadTestUnknownKey()
        {
            Settings.Instance.Load(@"TestData\unknown-key.xml");
            Assert.AreEqual(2, Settings.Instance.Dictionary.Count);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-value.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestNoValue()
        {
            Settings.Instance.Load(@"TestData\no-value.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-value.xml", @"TestData")]
        public void LoadTestEmptyValue()
        {
            Settings.Instance.Load(@"TestData\empty-value.xml");

            Assert.AreEqual(1, Settings.Instance.Dictionary.Count);

            var expected = new SettingsPerTitle();
            var actual = Settings.Instance.Dictionary.First().Value;
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
            Settings.Instance.Load(@"TestData\no-best-shot-directory.xml");
            Assert.AreEqual(1, Settings.Instance.Dictionary.Count);
            Assert.AreEqual(string.Empty, Settings.Instance.Dictionary.First().Value.BestShotDirectory);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-best-shot-directory.xml", @"TestData")]
        public void LoadTestEmptyBestShotDirectory()
        {
            Settings.Instance.Load(@"TestData\empty-best-shot-directory.xml");
            Assert.AreEqual(1, Settings.Instance.Dictionary.Count);
            Assert.AreEqual(string.Empty, Settings.Instance.Dictionary.First().Value.BestShotDirectory);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-hide-untried-cards.xml", @"TestData")]
        public void LoadTestNoHideUntriedCards()
        {
            Settings.Instance.Load(@"TestData\no-hide-untried-cards.xml");
            Assert.AreEqual(1, Settings.Instance.Dictionary.Count);
            Assert.IsTrue(Settings.Instance.Dictionary.First().Value.HideUntriedCards);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-hide-untried-cards.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestEmptyHideUntriedCards()
        {
            Settings.Instance.Load(@"TestData\empty-hide-untried-cards.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\invalid-hide-untried-cards.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestInvalidHideUntriedCards()
        {
            Settings.Instance.Load(@"TestData\invalid-hide-untried-cards.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-image-output-directory.xml", @"TestData")]
        public void LoadTestNoImageOutputDirectory()
        {
            Settings.Instance.Load(@"TestData\no-image-output-directory.xml");
            Assert.AreEqual(1, Settings.Instance.Dictionary.Count);
            Assert.AreEqual(string.Empty, Settings.Instance.Dictionary.First().Value.ImageOutputDirectory);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-image-output-directory.xml", @"TestData")]
        public void LoadTestEmptyImageOutputDirectory()
        {
            Settings.Instance.Load(@"TestData\empty-image-output-directory.xml");
            Assert.AreEqual(1, Settings.Instance.Dictionary.Count);
            Assert.AreEqual(string.Empty, Settings.Instance.Dictionary.First().Value.ImageOutputDirectory);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-output-directory.xml", @"TestData")]
        public void LoadTestNoOutputDirectory()
        {
            Settings.Instance.Load(@"TestData\no-output-directory.xml");
            Assert.AreEqual(1, Settings.Instance.Dictionary.Count);
            Assert.AreEqual(string.Empty, Settings.Instance.Dictionary.First().Value.OutputDirectory);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-output-directory.xml", @"TestData")]
        public void LoadTestEmptyOutputDirectory()
        {
            Settings.Instance.Load(@"TestData\empty-output-directory.xml");
            Assert.AreEqual(1, Settings.Instance.Dictionary.Count);
            Assert.AreEqual(string.Empty, Settings.Instance.Dictionary.First().Value.OutputDirectory);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-score-file.xml", @"TestData")]
        public void LoadTestNoScoreFile()
        {
            Settings.Instance.Load(@"TestData\no-score-file.xml");
            Assert.AreEqual(1, Settings.Instance.Dictionary.Count);
            Assert.AreEqual(string.Empty, Settings.Instance.Dictionary.First().Value.ScoreFile);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-score-file.xml", @"TestData")]
        public void LoadTestEmptyScoreFile()
        {
            Settings.Instance.Load(@"TestData\empty-score-file.xml");
            Assert.AreEqual(1, Settings.Instance.Dictionary.Count);
            Assert.AreEqual(string.Empty, Settings.Instance.Dictionary.First().Value.ScoreFile);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-template-files.xml", @"TestData")]
        public void LoadTestNoTemplateFiles()
        {
            Settings.Instance.Load(@"TestData\no-template-files.xml");
            Assert.AreEqual(1, Settings.Instance.Dictionary.Count);
            CollectionAssert.That.AreEqual(
                Enumerable.Empty<string>(), Settings.Instance.Dictionary.First().Value.TemplateFiles);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-template-files.xml", @"TestData")]
        public void LoadTestEmptyTemplateFiles()
        {
            Settings.Instance.Load(@"TestData\empty-template-files.xml");
            Assert.AreEqual(1, Settings.Instance.Dictionary.Count);
            CollectionAssert.That.AreEqual(
                Enumerable.Empty<string>(), Settings.Instance.Dictionary.First().Value.TemplateFiles);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\invalid-template-files.xml", @"TestData")]
        public void LoadTestInvalidTemplateFiles()
        {
            Settings.Instance.Load(@"TestData\invalid-template-files.xml");
            Assert.AreEqual(1, Settings.Instance.Dictionary.Count);
            CollectionAssert.That.AreEqual(
                Enumerable.Empty<string>(), Settings.Instance.Dictionary.First().Value.TemplateFiles);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-font-family-name.xml", @"TestData")]
        public void LoadTestNoFontFamilyName()
        {
            Settings.Instance.Load(@"TestData\no-font-family-name.xml");
            Assert.AreEqual(SystemFonts.MessageFontFamily.Source, Settings.Instance.FontFamilyName);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-font-family-name.xml", @"TestData")]
        public void LoadTestEmptyFontFamilyName()
        {
            Settings.Instance.Load(@"TestData\empty-font-family-name.xml");
            Assert.AreEqual(SystemFonts.MessageFontFamily.Source, Settings.Instance.FontFamilyName);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-font-size.xml", @"TestData")]
        public void LoadTestNoFontSize()
        {
            Settings.Instance.Load(@"TestData\no-font-size.xml");
            Assert.AreEqual(SystemFonts.MessageFontSize, Settings.Instance.FontSize);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-font-size.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestEmptyFontSize()
        {
            Settings.Instance.Load(@"TestData\empty-font-size.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\invalid-font-size.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestInvalidFontSize()
        {
            Settings.Instance.Load(@"TestData\invalid-font-size.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\negative-font-size.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestNegativeFontSize()
        {
            Settings.Instance.Load(@"TestData\negative-font-size.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\zero-font-size.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestZeroFontSize()
        {
            Settings.Instance.Load(@"TestData\zero-font-size.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\exceeded-font-size.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestExceededFontSize()
        {
            Settings.Instance.Load(@"TestData\exceeded-font-size.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-output-number-group-separator.xml", @"TestData")]
        public void LoadTestNoOutputNumberGroupSeparator()
        {
            Settings.Instance.Load(@"TestData\no-output-number-group-separator.xml");
            Assert.AreEqual(true, Settings.Instance.OutputNumberGroupSeparator);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-output-number-group-separator.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestEmptyOutputNumberGroupSeparator()
        {
            Settings.Instance.Load(@"TestData\empty-output-number-group-separator.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\invalid-output-number-group-separator.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestInvalidOutputNumberGroupSeparator()
        {
            Settings.Instance.Load(@"TestData\invalid-output-number-group-separator.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-input-code-page-id.xml", @"TestData")]
        public void LoadTestNoInputCodePageId()
        {
            Settings.Instance.Load(@"TestData\no-input-code-page-id.xml");
            Assert.AreEqual(65001, Settings.Instance.InputCodePageId);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-input-code-page-id.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestEmptyInputCodePageId()
        {
            Settings.Instance.Load(@"TestData\empty-input-code-page-id.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\invalid-input-code-page-id.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestInvalidInputCodePageId()
        {
            Settings.Instance.Load(@"TestData\invalid-input-code-page-id.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\unsupported-input-code-page-id.xml", @"TestData")]
        public void LoadTestUnsupportedInputCodePageId()
        {
            Settings.Instance.Load(@"TestData\unsupported-input-code-page-id.xml");
            Assert.AreEqual(65001, Settings.Instance.InputCodePageId);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\no-output-code-page-id.xml", @"TestData")]
        public void LoadTestNoOutputCodePageId()
        {
            Settings.Instance.Load(@"TestData\no-output-code-page-id.xml");
            Assert.AreEqual(65001, Settings.Instance.OutputCodePageId);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\empty-output-code-page-id.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestEmptyOutputCodePageId()
        {
            Settings.Instance.Load(@"TestData\empty-output-code-page-id.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\invalid-output-code-page-id.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestInvalidOutputCodePageId()
        {
            Settings.Instance.Load(@"TestData\invalid-output-code-page-id.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\unsupported-output-code-page-id.xml", @"TestData")]
        public void LoadTestUnsupportedOutputCodePageId()
        {
            Settings.Instance.Load(@"TestData\unsupported-output-code-page-id.xml");
            Assert.AreEqual(65001, Settings.Instance.OutputCodePageId);
        }

        [TestMethod]
        [DeploymentItem(@"TestData\invalid-character.xml", @"TestData")]
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadTestInvalidCharacter()
        {
            Settings.Instance.Load(@"TestData\invalid-character.xml");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void SaveTest()
        {
            var expectedLastTitle = "TH06";
            var expectedSettingsPerTitle = new SettingsPerTitle
            {
                BestShotDirectory = "bestshot",
                HideUntriedCards = true,
                ImageOutputDirectory = "images",
                OutputDirectory = "output",
                ScoreFile = "score.dat",
                TemplateFiles = new[] { "template1", "template2" },
            };
            var expectedFontFamilyName = "font-family-name";
            var expectedFontSize = 12.3;
            var expectedOutputNumberGroupSeparator = false;
            var expectedInputCodePageId = 932;
            var expectedOutputCodePageId = 932;

            Settings.Instance.LastTitle = expectedLastTitle;
            Settings.Instance.Dictionary.Add(expectedLastTitle, expectedSettingsPerTitle);
            Settings.Instance.FontFamilyName = expectedFontFamilyName;
            Settings.Instance.FontSize = expectedFontSize;
            Settings.Instance.OutputNumberGroupSeparator = expectedOutputNumberGroupSeparator;
            Settings.Instance.InputCodePageId = expectedInputCodePageId;
            Settings.Instance.OutputCodePageId = expectedOutputCodePageId;

            var tempfile = Path.GetTempFileName();
            Settings.Instance.Save(tempfile);

            Settings.Instance.LastTitle = string.Empty;
            Settings.Instance.Dictionary.Clear();
            Settings.Instance.FontFamilyName = string.Empty;
            Settings.Instance.FontSize = 0;
            Settings.Instance.OutputNumberGroupSeparator = true;
            Settings.Instance.InputCodePageId = 65001;
            Settings.Instance.OutputCodePageId = 65001;

            Settings.Instance.Load(tempfile);
            File.Delete(tempfile);

            Assert.AreEqual(expectedLastTitle, Settings.Instance.LastTitle);
            Assert.AreEqual(1, Settings.Instance.Dictionary.Count);
            Assert.IsTrue(Settings.Instance.Dictionary.ContainsKey(expectedLastTitle));

            var actualSettingsPerTitle = Settings.Instance.Dictionary[expectedLastTitle];
            Assert.AreEqual(expectedSettingsPerTitle.BestShotDirectory, actualSettingsPerTitle.BestShotDirectory);
            Assert.AreEqual(expectedSettingsPerTitle.HideUntriedCards, actualSettingsPerTitle.HideUntriedCards);
            Assert.AreEqual(expectedSettingsPerTitle.ImageOutputDirectory, actualSettingsPerTitle.ImageOutputDirectory);
            Assert.AreEqual(expectedSettingsPerTitle.OutputDirectory, actualSettingsPerTitle.OutputDirectory);
            Assert.AreEqual(expectedSettingsPerTitle.ScoreFile, actualSettingsPerTitle.ScoreFile);
            CollectionAssert.That.AreEqual(expectedSettingsPerTitle.TemplateFiles, actualSettingsPerTitle.TemplateFiles);

            Assert.AreEqual(expectedFontFamilyName, Settings.Instance.FontFamilyName);
            Assert.AreEqual(expectedFontSize, Settings.Instance.FontSize);
            Assert.AreEqual(expectedOutputNumberGroupSeparator, Settings.Instance.OutputNumberGroupSeparator);
            Assert.AreEqual(expectedInputCodePageId, Settings.Instance.InputCodePageId);
            Assert.AreEqual(expectedOutputCodePageId, Settings.Instance.OutputCodePageId);
        }
    }
}
