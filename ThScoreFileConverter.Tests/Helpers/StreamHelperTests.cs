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
        stream.ShouldBeOfType<FileStream>().Name.ShouldEndWith(path, Case.Sensitive);
        stream.CanRead.ShouldBeTrue();
#else
        _ = stream.ShouldBeOfType<MemoryStream>();
#endif
    }
}
