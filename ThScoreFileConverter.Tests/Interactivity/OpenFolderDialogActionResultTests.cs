using ThScoreFileConverter.Interactivity;

namespace ThScoreFileConverter.Tests.Interactivity;

[TestClass]
public class OpenFolderDialogActionResultTests
{
    [DataTestMethod]
    [DataRow("")]
    [DataRow(@"path\to\some\folder")]
    public void OpenFolderDialogActionResultTest(string path)
    {
        var result = new OpenFolderDialogActionResult(path);

        result.FolderName.ShouldBe(path);
    }

    [TestMethod]
    public void OpenFolderDialogActionResultTestNull()
    {
        _ = Should.Throw<ArgumentNullException>(() => new OpenFolderDialogActionResult(null!));
    }
}
