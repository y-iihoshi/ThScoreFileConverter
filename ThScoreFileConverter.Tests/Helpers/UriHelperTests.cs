using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Tests.Helpers;

[TestClass]
public class UriHelperTests
{
    [TestMethod]
    public void GetRelativePathTest()
    {
        var dir = @"C:\path\to\dir\";
        var file = @"C:\path\to\dir\dir2\file.ext";
        Assert.AreEqual("dir2/file.ext", UriHelper.GetRelativePath(dir, file));
    }

    [TestMethod]
    public void GetRelativePathTestNullRelativeTo()
    {
        string dir = null!;
        var file = @"C:\path\to\dir\dir2\file.ext";
        Assert.AreEqual(string.Empty, UriHelper.GetRelativePath(dir, file));
    }

    [TestMethod]
    public void GetRelativePathTestNullPath()
    {
        var dir = @"C:\path\to\dir\";
        string file = null!;
        Assert.AreEqual(string.Empty, UriHelper.GetRelativePath(dir, file));
    }

    [TestMethod]
    public void GetRelativePathTestEmptyRelativeTo()
    {
        var dir = string.Empty;
        var file = @"C:\path\to\dir\dir2\file.ext";
        Assert.AreEqual(string.Empty, UriHelper.GetRelativePath(dir, file));
    }

    [TestMethod]
    public void GetRelativePathTestEmptyPath()
    {
        var dir = @"C:\path\to\dir\";
        var file = string.Empty;
        Assert.AreEqual(string.Empty, UriHelper.GetRelativePath(dir, file));
    }

    [TestMethod]
    public void GetRelativePathTestInvalidRelativeTo()
    {
        var dir = "abcde";
        var file = @"C:\path\to\dir\dir2\file.ext";
        Assert.AreEqual(string.Empty, UriHelper.GetRelativePath(dir, file));
    }

    [TestMethod]
    public void GetRelativePathTestInvalidPath()
    {
        var dir = @"C:\path\to\dir\";
        var file = "abcde";
        Assert.AreEqual(string.Empty, UriHelper.GetRelativePath(dir, file));
    }

    [TestMethod]
    public void TryGetRelativePathTest()
    {
        var dir = @"C:\path\to\dir\";
        var file = @"C:\path\to\dir\dir2\file.ext";
        Assert.IsTrue(UriHelper.TryGetRelativePath(dir, file, out var path));
        Assert.AreEqual("dir2/file.ext", path);
    }

    [TestMethod]
    public void TryGetRelativePathTestNullRelativeTo()
    {
        string dir = null!;
        var file = @"C:\path\to\dir\dir2\file.ext";
        Assert.IsFalse(UriHelper.TryGetRelativePath(dir, file, out var path));
        Assert.AreEqual(string.Empty, path);
    }

    [TestMethod]
    public void TryGetRelativePathTestNullPath()
    {
        var dir = @"C:\path\to\dir\";
        string file = null!;
        Assert.IsFalse(UriHelper.TryGetRelativePath(dir, file, out var path));
        Assert.AreEqual(string.Empty, path);
    }

    [TestMethod]
    public void TryGetRelativePathTestEmptyRelativeTo()
    {
        var dir = string.Empty;
        var file = @"C:\path\to\dir\dir2\file.ext";
        Assert.IsFalse(UriHelper.TryGetRelativePath(dir, file, out var path));
        Assert.AreEqual(string.Empty, path);
    }

    [TestMethod]
    public void TryGetRelativePathTestEmptyPath()
    {
        var dir = @"C:\path\to\dir\";
        var file = string.Empty;
        Assert.IsFalse(UriHelper.TryGetRelativePath(dir, file, out var path));
        Assert.AreEqual(string.Empty, path);
    }

    [TestMethod]
    public void TryGetRelativePathTestInvalidRelativeTo()
    {
        var dir = "abcde";
        var file = @"C:\path\to\dir\dir2\file.ext";
        Assert.IsFalse(UriHelper.TryGetRelativePath(dir, file, out var path));
        Assert.AreEqual(string.Empty, path);
    }

    [TestMethod]
    public void TryGetRelativePathTestInvalidPath()
    {
        var dir = @"C:\path\to\dir\";
        var file = "abcde";
        Assert.IsFalse(UriHelper.TryGetRelativePath(dir, file, out var path));
        Assert.AreEqual(string.Empty, path);
    }
}
