using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Interactivity;

namespace ThScoreFileConverterTests.Interactivity;

[TestClass]
public class FolderBrowserDialogActionTests
{
    [TestMethod]
    public void DescriptionTest()
    {
        var action = new FolderBrowserDialogAction();
        Assert.AreEqual(string.Empty, action.Description);

        var description = "description";
        action.Description = description;
        Assert.AreEqual(description, action.Description);
    }

    [TestMethod]
    public void RootFolderTest()
    {
        var action = new FolderBrowserDialogAction();
        Assert.AreEqual(Environment.SpecialFolder.Desktop, action.RootFolder);

        var folder = Environment.SpecialFolder.MyDocuments;
        action.RootFolder = folder;
        Assert.AreEqual(folder, action.RootFolder);
    }

    [TestMethod]
    public void SelectedPathTest()
    {
        var action = new FolderBrowserDialogAction();
        Assert.AreEqual(string.Empty, action.SelectedPath);

        var path = Environment.CurrentDirectory;
        action.SelectedPath = path;
        Assert.AreEqual(path, action.SelectedPath);
    }

    [TestMethod]
    public void ShowNewFolderButtonTest()
    {
        var action = new FolderBrowserDialogAction();
        Assert.IsTrue(action.ShowNewFolderButton);

        action.ShowNewFolderButton = false;
        Assert.IsFalse(action.ShowNewFolderButton);
    }

    [TestMethod]
    public void CreateDialogTest()
    {
        var action = new FolderBrowserDialogAction
        {
            Description = "description",
            RootFolder = Environment.SpecialFolder.MyDocuments,
            SelectedPath = Environment.CurrentDirectory,
            ShowNewFolderButton = false,
            Site = new Site(),
            Tag = new object(),
        };

        using var dialog = action.CreateDialog();
        Assert.AreEqual(action.Description, dialog.Description);
        Assert.AreEqual(action.RootFolder, dialog.RootFolder);
        Assert.AreEqual(action.SelectedPath, dialog.SelectedPath);
        Assert.AreEqual(action.ShowNewFolderButton, dialog.ShowNewFolderButton);
        Assert.AreEqual(action.Site, dialog.Site);
        Assert.AreEqual(action.Tag, dialog.Tag);
    }
}
