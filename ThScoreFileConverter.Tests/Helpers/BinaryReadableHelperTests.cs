using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverter.Tests.Models.Th06;

namespace ThScoreFileConverter.Tests.Helpers;

[TestClass]
public class BinaryReadableHelperTests
{
    [TestMethod]
    public void CreateTest()
    {
        using var stream = new MemoryStream(ChapterTests.MakeByteArray(ChapterTests.ValidProperties));
        using var reader = new BinaryReader(stream);
        var chapter = BinaryReadableHelper.Create<Chapter>(reader);
        chapter.ShouldBe(ChapterTests.ValidProperties);
    }

    [TestMethod]
    public void CreateTestNull()
    {
        _ = Should.Throw<ArgumentNullException>(() => BinaryReadableHelper.Create<Chapter>(null!));
    }
}
