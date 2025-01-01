using System;
using ThScoreFileConverter.Interactivity;

namespace ThScoreFileConverter.Tests.Interactivity;

[TestClass]
public class OpenFolderDialogActionTests
{
    [TestMethod]
    public void CreateDialogTest()
    {
        var action = new OpenFolderDialogAction
        {
            AddToRecent = true,
            ClientGuid = Guid.NewGuid(),
            CustomPlaces = [],
            DefaultDirectory = @"default\directory",
            DereferenceLinks = true,
            FolderName = @"folder\name",
            InitialDirectory = @"initial\directory",
            Multiselect = true,
            RootDirectory = @"root\directory",
            ShowHiddenItems = true,
            Tag = new object(),
            Title = "title",
            ValidateNames = true,
        };

        var dialog = action.CreateDialog();

        Assert.AreEqual(action.AddToRecent, dialog.AddToRecent);
        Assert.AreEqual(action.ClientGuid, dialog.ClientGuid);
        Assert.AreEqual(action.CustomPlaces, dialog.CustomPlaces);
        Assert.AreEqual(action.DefaultDirectory, dialog.DefaultDirectory);
        Assert.AreEqual(action.DereferenceLinks, dialog.DereferenceLinks);
        Assert.AreEqual(action.FolderName, dialog.FolderName);
        Assert.AreEqual(action.InitialDirectory, dialog.InitialDirectory);
        Assert.AreEqual(action.Multiselect, dialog.Multiselect);
        Assert.AreEqual(action.RootDirectory, dialog.RootDirectory);
        Assert.AreEqual(action.ShowHiddenItems, dialog.ShowHiddenItems);
        Assert.AreEqual(action.Title, dialog.Title);
        Assert.AreEqual(action.ValidateNames, dialog.ValidateNames);
        Assert.AreEqual(action.Tag, dialog.Tag);
    }
}
