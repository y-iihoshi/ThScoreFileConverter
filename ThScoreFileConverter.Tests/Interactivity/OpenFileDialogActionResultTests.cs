using ThScoreFileConverter.Interactivity;

namespace ThScoreFileConverter.Tests.Interactivity;

[TestClass]
public class OpenFileDialogActionResultTests
{
    [TestMethod]
    public void OpenFileDialogActionResultTest()
    {
        var filename = "score.dat";
        var filenames = new string[] { "bs01_1.dat", "bs01_2.dat" };
        var result = new OpenFileDialogActionResult(filename, filenames);

        result.FileName.ShouldBe(filename);
        result.FileNames.ShouldBe(filenames);

        filenames[0] = "bs02_1.dat";
        result.FileNames.ShouldNotBe(filenames);
    }

    [TestMethod]
    public void OpenFileDialogActionResultTestNullFilename()
    {
        var filenames = new string[] { "bs01_1.dat", "bs01_2.dat" };
        _ = Should.Throw<ArgumentNullException>(() => new OpenFileDialogActionResult(null!, filenames));
    }

    [TestMethod]
    public void OpenFileDialogActionResultTestNullFilenames()
    {
        var filename = "score.dat";
        _ = Should.Throw<ArgumentNullException>(() => new OpenFileDialogActionResult(filename, null!));
    }
}
