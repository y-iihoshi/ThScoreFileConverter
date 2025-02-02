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

        dialog.AddExtension.ShouldBe(action.AddExtension);
        dialog.CheckFileExists.ShouldBe(action.CheckFileExists);
        dialog.CheckPathExists.ShouldBe(action.CheckPathExists);
        dialog.DefaultExt.ShouldBe(action.DefaultExt);
        dialog.DereferenceLinks.ShouldBe(action.DereferenceLinks);
        dialog.FileName.ShouldBe(action.FileName);
        dialog.Filter.ShouldBe(action.Filter);
        dialog.FilterIndex.ShouldBe(action.FilterIndex);
        dialog.InitialDirectory.ShouldBe(action.InitialDirectory);
        dialog.Multiselect.ShouldBe(action.Multiselect);
        dialog.ReadOnlyChecked.ShouldBe(action.ReadOnlyChecked);
        dialog.RestoreDirectory.ShouldBe(action.RestoreDirectory);
        dialog.ShowReadOnly.ShouldBe(action.ShowReadOnly);
        dialog.Tag.ShouldBe(action.Tag);
        dialog.Title.ShouldBe(action.Title);
        dialog.ValidateNames.ShouldBe(action.ValidateNames);
    }
}
