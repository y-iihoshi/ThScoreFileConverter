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

        dialog.AddToRecent.ShouldBe(action.AddToRecent);
        dialog.ClientGuid.ShouldBe(action.ClientGuid);
        dialog.CustomPlaces.ShouldBe(action.CustomPlaces);
        dialog.DefaultDirectory.ShouldBe(action.DefaultDirectory);
        dialog.DereferenceLinks.ShouldBe(action.DereferenceLinks);
        dialog.FolderName.ShouldBe(action.FolderName);
        dialog.InitialDirectory.ShouldBe(action.InitialDirectory);
        dialog.Multiselect.ShouldBe(action.Multiselect);
        dialog.RootDirectory.ShouldBe(action.RootDirectory);
        dialog.ShowHiddenItems.ShouldBe(action.ShowHiddenItems);
        dialog.Title.ShouldBe(action.Title);
        dialog.ValidateNames.ShouldBe(action.ValidateNames);
        dialog.Tag.ShouldBe(action.Tag);
    }
}
