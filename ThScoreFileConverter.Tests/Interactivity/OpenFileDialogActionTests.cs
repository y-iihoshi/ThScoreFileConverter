using System;
using ThScoreFileConverter.Interactivity;

namespace ThScoreFileConverter.Tests.Interactivity;

[TestClass]
public class OpenFileDialogActionTests
{
    [TestMethod]
    public void CreateDialogTest()
    {
        var action = new OpenFileDialogAction
        {
            AddExtension = false,
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
            ShowReadOnly = true,
            Tag = new object(),
            Title = nameof(CreateDialogTest),
            ValidateNames = false,
        };

        var dialog = action.CreateDialog();

        Assert.AreEqual(action.AddExtension, dialog.AddExtension);
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
        Assert.AreEqual(action.ShowReadOnly, dialog.ShowReadOnly);
        Assert.AreEqual(action.Tag, dialog.Tag);
        Assert.AreEqual(action.Title, dialog.Title);
        Assert.AreEqual(action.ValidateNames, dialog.ValidateNames);
    }
}
