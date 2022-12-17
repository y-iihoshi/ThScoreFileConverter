using System;
using ThScoreFileConverter.Interactivity;

namespace ThScoreFileConverter.Tests.Interactivity;

[TestClass]
public class OpenFileDialogActionTests
{
    [TestMethod]
    public void AddExtensionTest()
    {
        var action = new OpenFileDialogAction();
        Assert.IsTrue(action.AddExtension);

        action.AddExtension = false;
        Assert.IsFalse(action.AddExtension);
    }

    [TestMethod]
    public void AutoUpgradeEnabledTest()
    {
        var action = new OpenFileDialogAction();
        Assert.IsTrue(action.AutoUpgradeEnabled);

        action.AutoUpgradeEnabled = false;
        Assert.IsFalse(action.AutoUpgradeEnabled);
    }

    [TestMethod]
    public void CheckFileExistsTest()
    {
        var action = new OpenFileDialogAction();
        Assert.IsTrue(action.CheckFileExists);

        action.CheckFileExists = false;
        Assert.IsFalse(action.CheckFileExists);
    }

    [TestMethod]
    public void CheckPathExistsTest()
    {
        var action = new OpenFileDialogAction();
        Assert.IsTrue(action.CheckPathExists);

        action.CheckPathExists = false;
        Assert.IsFalse(action.CheckPathExists);
    }

    [TestMethod]
    public void DefaultExtTest()
    {
        var action = new OpenFileDialogAction();
        Assert.AreEqual(string.Empty, action.DefaultExt);

        var extension = "dat";
        action.DefaultExt = extension;
        Assert.AreEqual(extension, action.DefaultExt);
    }

    [TestMethod]
    public void DereferenceLinksTest()
    {
        var action = new OpenFileDialogAction();
        Assert.IsTrue(action.DereferenceLinks);

        action.DereferenceLinks = false;
        Assert.IsFalse(action.DereferenceLinks);
    }

    [TestMethod]
    public void FileNameTest()
    {
        var action = new OpenFileDialogAction();
        Assert.AreEqual(string.Empty, action.FileName);

        var name = "score.dat";
        action.FileName = name;
        Assert.AreEqual(name, action.FileName);
    }

    [TestMethod]
    public void FilterTest()
    {
        var action = new OpenFileDialogAction();
        Assert.AreEqual(string.Empty, action.Filter);

        var filter = "All files (*.*)|*.*";
        action.Filter = filter;
        Assert.AreEqual(filter, action.Filter);
    }

    [TestMethod]
    public void FilterIndexTest()
    {
        var action = new OpenFileDialogAction();
        Assert.AreEqual(1, action.FilterIndex);

        ++action.FilterIndex;
        Assert.AreEqual(2, action.FilterIndex);
    }

    [TestMethod]
    public void InitialDirectoryTest()
    {
        var action = new OpenFileDialogAction();
        Assert.AreEqual(string.Empty, action.InitialDirectory);

        var directory = Environment.CurrentDirectory;
        action.InitialDirectory = directory;
        Assert.AreEqual(directory, action.InitialDirectory);
    }

    [TestMethod]
    public void MultiselectTest()
    {
        var action = new OpenFileDialogAction();
        Assert.IsFalse(action.Multiselect);

        action.Multiselect = true;
        Assert.IsTrue(action.Multiselect);
    }

    [TestMethod]
    public void ReadOnlyCheckedTest()
    {
        var action = new OpenFileDialogAction();
        Assert.IsFalse(action.ReadOnlyChecked);

        action.ReadOnlyChecked = true;
        Assert.IsTrue(action.ReadOnlyChecked);
    }

    [TestMethod]
    public void RestoreDirectoryTest()
    {
        var action = new OpenFileDialogAction();
        Assert.IsFalse(action.RestoreDirectory);

        action.RestoreDirectory = true;
        Assert.IsTrue(action.RestoreDirectory);
    }

    [TestMethod]
    public void ShowHelpTest()
    {
        var action = new OpenFileDialogAction();
        Assert.IsFalse(action.ShowHelp);

        action.ShowHelp = true;
        Assert.IsTrue(action.ShowHelp);
    }

    [TestMethod]
    public void ShowReadOnlyTest()
    {
        var action = new OpenFileDialogAction();
        Assert.IsFalse(action.ShowReadOnly);

        action.ShowReadOnly = true;
        Assert.IsTrue(action.ShowReadOnly);
    }

    [TestMethod]
    public void SupportMultiDottedExtensionsTest()
    {
        var action = new OpenFileDialogAction();
        Assert.IsFalse(action.SupportMultiDottedExtensions);

        action.SupportMultiDottedExtensions = true;
        Assert.IsTrue(action.SupportMultiDottedExtensions);
    }

    [TestMethod]
    public void TitleTest()
    {
        var action = new OpenFileDialogAction();
        Assert.AreEqual(string.Empty, action.Title);

        var title = nameof(TitleTest);
        action.Title = title;
        Assert.AreEqual(title, action.Title);
    }

    [TestMethod]
    public void ValidateNamesTest()
    {
        var action = new OpenFileDialogAction();
        Assert.IsTrue(action.ValidateNames);

        action.ValidateNames = false;
        Assert.IsFalse(action.ValidateNames);
    }

    [TestMethod]
    public void CreateDialogTest()
    {
        var action = new OpenFileDialogAction
        {
            AddExtension = false,
            AutoUpgradeEnabled = false,
            CheckFileExists = false,
            CheckPathExists = false,
            DefaultExt = "dat",
            DereferenceLinks = false,
            FileName = "score.dat",
            Filter = "All files (*.*)|*.*",
            FilterIndex = 2,
            InitialDirectory = Environment.CurrentDirectory,
            Multiselect = true,
            ReadOnlyChecked = true,
            RestoreDirectory = true,
            ShowHelp = true,
            ShowReadOnly = true,
            Site = new Site(),
            SupportMultiDottedExtensions = true,
            Tag = new object(),
            Title = nameof(CreateDialogTest),
            ValidateNames = false,
        };

        using var dialog = action.CreateDialog();
        Assert.AreEqual(action.AddExtension, dialog.AddExtension);
        Assert.AreEqual(action.AutoUpgradeEnabled, dialog.AutoUpgradeEnabled);
        Assert.AreEqual(action.CheckFileExists, dialog.CheckFileExists);
        Assert.AreEqual(action.CheckPathExists, dialog.CheckPathExists);
        Assert.AreEqual(action.DefaultExt, dialog.DefaultExt);
        Assert.AreEqual(action.DereferenceLinks, dialog.DereferenceLinks);
        Assert.AreEqual(action.FileName, dialog.FileName);
        Assert.AreEqual(action.Filter, dialog.Filter);
        Assert.AreEqual(action.FilterIndex, dialog.FilterIndex);
        Assert.AreEqual(action.InitialDirectory, dialog.InitialDirectory);
        Assert.AreEqual(action.Multiselect, dialog.Multiselect);
        Assert.AreEqual(action.ReadOnlyChecked, dialog.ReadOnlyChecked);
        Assert.AreEqual(action.RestoreDirectory, dialog.RestoreDirectory);
        Assert.AreEqual(action.ShowHelp, dialog.ShowHelp);
        Assert.AreEqual(action.ShowReadOnly, dialog.ShowReadOnly);
        Assert.AreEqual(action.Site, dialog.Site);
        Assert.AreEqual(action.SupportMultiDottedExtensions, dialog.SupportMultiDottedExtensions);
        Assert.AreEqual(action.Tag, dialog.Tag);
        Assert.AreEqual(action.Title, dialog.Title);
        Assert.AreEqual(action.ValidateNames, dialog.ValidateNames);
    }
}
