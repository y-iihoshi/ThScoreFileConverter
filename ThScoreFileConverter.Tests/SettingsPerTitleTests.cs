using System.ComponentModel;

namespace ThScoreFileConverter.Tests;

[TestClass]
public class SettingsPerTitleTests
{
    [TestMethod]
    public void SettingsPerTitleTest()
    {
        var setting = new SettingsPerTitle();
        setting.ScoreFile.ShouldBeEmpty();
        setting.BestShotDirectory.ShouldBeEmpty();
        setting.TemplateFiles.ShouldBeEmpty();
        setting.OutputDirectory.ShouldBeEmpty();
        setting.ImageOutputDirectory.ShouldBeEmpty();
        setting.HideUntriedCards.ShouldBeTrue();
    }

    [TestMethod]
    public void ScoreFileTestNull()
    {
        _ = Should.Throw<ArgumentNullException>(() => new SettingsPerTitle().ScoreFile = null!);
    }

    [TestMethod]
    public void BestShotDirectoryTestNull()
    {
        _ = Should.Throw<ArgumentNullException>(() => new SettingsPerTitle().BestShotDirectory = null!);
    }

    [TestMethod]
    public void TemplateFilesTestNull()
    {
        _ = Should.Throw<ArgumentNullException>(() => new SettingsPerTitle().TemplateFiles = null!);
    }

    [TestMethod]
    public void OutputDirectoryTestNull()
    {
        _ = Should.Throw<ArgumentNullException>(() => new SettingsPerTitle().OutputDirectory = null!);
    }

    [TestMethod]
    public void ImageOutputDirectoryNull()
    {
        _ = Should.Throw<ArgumentNullException>(() => new SettingsPerTitle().ImageOutputDirectory = null!);
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
            setting.ScoreFile.ShouldBe(scoreFile);
            numCalled.ShouldBe(1);
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
            setting.ScoreFile.ShouldBe(scoreFile);
            numCalled.ShouldBe(0);
        }
        finally
        {
            setting.PropertyChanged -= OnPropertyChanged;
        }
    }
}
