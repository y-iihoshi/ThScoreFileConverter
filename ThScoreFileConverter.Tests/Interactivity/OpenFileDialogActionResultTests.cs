using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Core.Tests.UnitTesting;
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

        Assert.AreEqual(filename, result.FileName);
        CollectionAssert.That.AreEqual(filenames, result.FileNames);

        filenames[0] = "bs02_1.dat";
        CollectionAssert.That.AreNotEqual(filenames, result.FileNames);
    }

    [TestMethod]
    public void OpenFileDialogActionResultTestNullFilename()
    {
        var filenames = new string[] { "bs01_1.dat", "bs01_2.dat" };
        _ = Assert.ThrowsException<ArgumentNullException>(() => new OpenFileDialogActionResult(null!, filenames));
    }

    [TestMethod]
    public void OpenFileDialogActionResultTestNullFilenames()
    {
        var filename = "score.dat";
        _ = Assert.ThrowsException<ArgumentNullException>(() => new OpenFileDialogActionResult(filename, null!));
    }
}
