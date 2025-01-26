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

        Assert.AreEqual(path, result.FolderName);
    }

    [TestMethod]
    public void OpenFolderDialogActionResultTestNull()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new OpenFolderDialogActionResult(null!));
    }
}
