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
        UriHelper.GetRelativePath(dir, file).ShouldBe("dir2/file.ext");
    }

    [TestMethod]
    public void GetRelativePathTestNullRelativeTo()
    {
        string dir = null!;
        var file = @"C:\path\to\dir\dir2\file.ext";
        UriHelper.GetRelativePath(dir, file).ShouldBeEmpty();
    }

    [TestMethod]
    public void GetRelativePathTestNullPath()
    {
        var dir = @"C:\path\to\dir\";
        string file = null!;
        UriHelper.GetRelativePath(dir, file).ShouldBeEmpty();
    }

    [TestMethod]
    public void GetRelativePathTestEmptyRelativeTo()
    {
        var dir = string.Empty;
        var file = @"C:\path\to\dir\dir2\file.ext";
        UriHelper.GetRelativePath(dir, file).ShouldBeEmpty();
    }

    [TestMethod]
    public void GetRelativePathTestEmptyPath()
    {
        var dir = @"C:\path\to\dir\";
        var file = string.Empty;
        UriHelper.GetRelativePath(dir, file).ShouldBeEmpty();
    }

    [TestMethod]
    public void GetRelativePathTestInvalidRelativeTo()
    {
        var dir = "abcde";
        var file = @"C:\path\to\dir\dir2\file.ext";
        UriHelper.GetRelativePath(dir, file).ShouldBeEmpty();
    }

    [TestMethod]
    public void GetRelativePathTestInvalidPath()
    {
        var dir = @"C:\path\to\dir\";
        var file = "abcde";
        UriHelper.GetRelativePath(dir, file).ShouldBeEmpty();
    }

    [TestMethod]
    public void TryGetRelativePathTest()
    {
        var dir = @"C:\path\to\dir\";
        var file = @"C:\path\to\dir\dir2\file.ext";
        UriHelper.TryGetRelativePath(dir, file, out var path).ShouldBeTrue();
        path.ShouldBe("dir2/file.ext");
    }

    [TestMethod]
    public void TryGetRelativePathTestNullRelativeTo()
    {
        string dir = null!;
        var file = @"C:\path\to\dir\dir2\file.ext";
        UriHelper.TryGetRelativePath(dir, file, out var path).ShouldBeFalse();
        path.ShouldBeEmpty();
    }

    [TestMethod]
    public void TryGetRelativePathTestNullPath()
    {
        var dir = @"C:\path\to\dir\";
        string file = null!;
        UriHelper.TryGetRelativePath(dir, file, out var path).ShouldBeFalse();
        path.ShouldBeEmpty();
    }

    [TestMethod]
    public void TryGetRelativePathTestEmptyRelativeTo()
    {
        var dir = string.Empty;
        var file = @"C:\path\to\dir\dir2\file.ext";
        UriHelper.TryGetRelativePath(dir, file, out var path).ShouldBeFalse();
        path.ShouldBeEmpty();
    }

    [TestMethod]
    public void TryGetRelativePathTestEmptyPath()
    {
        var dir = @"C:\path\to\dir\";
        var file = string.Empty;
        UriHelper.TryGetRelativePath(dir, file, out var path).ShouldBeFalse();
        path.ShouldBeEmpty();
    }

    [TestMethod]
    public void TryGetRelativePathTestInvalidRelativeTo()
    {
        var dir = "abcde";
        var file = @"C:\path\to\dir\dir2\file.ext";
        UriHelper.TryGetRelativePath(dir, file, out var path).ShouldBeFalse();
        path.ShouldBeEmpty();
    }

    [TestMethod]
    public void TryGetRelativePathTestInvalidPath()
    {
        var dir = @"C:\path\to\dir\";
        var file = "abcde";
        UriHelper.TryGetRelativePath(dir, file, out var path).ShouldBeFalse();
        path.ShouldBeEmpty();
    }
}
