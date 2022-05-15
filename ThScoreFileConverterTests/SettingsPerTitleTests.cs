using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;

namespace ThScoreFileConverterTests;

[TestClass]
public class SettingsPerTitleTests
{
    [TestMethod]
    public void SettingsPerTitleTest()
    {
        var setting = new SettingsPerTitle();
        Assert.AreEqual(string.Empty, setting.ScoreFile);
        Assert.AreEqual(string.Empty, setting.BestShotDirectory);
        Assert.AreEqual(0, setting.TemplateFiles.Count());
        Assert.AreEqual(string.Empty, setting.OutputDirectory);
        Assert.AreEqual(string.Empty, setting.ImageOutputDirectory);
        Assert.AreEqual(true, setting.HideUntriedCards);
    }

    [TestMethod]
    public void ScoreFileTestNull()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new SettingsPerTitle().ScoreFile = null!);
    }

    [TestMethod]
    public void BestShotDirectoryTestNull()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new SettingsPerTitle().BestShotDirectory = null!);
    }

    [TestMethod]
    public void TemplateFilesTestNull()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new SettingsPerTitle().TemplateFiles = null!);
    }

    [TestMethod]
    public void OutputDirectoryTestNull()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new SettingsPerTitle().OutputDirectory = null!);
    }

    [TestMethod]
    public void ImageOutputDirectoryNull()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new SettingsPerTitle().ImageOutputDirectory = null!);
    }

    [TestMethod]
    public void SetPropertyTest()
    {
        var numCalled = 0;
        void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ++numCalled;
        }

        var setting = new SettingsPerTitle();
        setting.PropertyChanged += OnPropertyChanged;
        try
        {
            var scoreFile = nameof(setting.ScoreFile);
            setting.ScoreFile = scoreFile;
            Assert.AreEqual(scoreFile, setting.ScoreFile);
            Assert.AreEqual(1, numCalled);
        }
        finally
        {
            setting.PropertyChanged -= OnPropertyChanged;
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

        var setting = new SettingsPerTitle();
        setting.PropertyChanged += OnPropertyChanged;
        try
        {
            var scoreFile = setting.ScoreFile;
            setting.ScoreFile = scoreFile;
            Assert.AreEqual(scoreFile, setting.ScoreFile);
            Assert.AreEqual(0, numCalled);
        }
        finally
        {
            setting.PropertyChanged -= OnPropertyChanged;
        }
    }
}
