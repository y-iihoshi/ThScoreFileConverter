using System.IO;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Tests.Helpers;

[TestClass]
public class StreamHelperTests
{
    [TestMethod]
    public void CreateTest()
    {
        var path = "StreamHelperTests.CreateTest.txt";
        using var stream = StreamHelper.Create(path, FileMode.OpenOrCreate, FileAccess.Read);

#if DEBUG
        Assert.IsTrue(stream is FileStream);
        StringAssert.EndsWith(((FileStream)stream).Name, path, System.StringComparison.InvariantCulture);
        Assert.IsTrue(stream.CanRead);
#else
        Assert.IsTrue(stream is MemoryStream);
#endif
    }
}
