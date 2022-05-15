using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverterTests.Models.Th06;

namespace ThScoreFileConverterTests.Helpers;

[TestClass]
public class BinaryReadableHelperTests
{
    [TestMethod]
    public void CreateTest()
    {
        using var stream = new MemoryStream(ChapterTests.MakeByteArray(ChapterTests.ValidProperties));
        using var reader = new BinaryReader(stream);
        var chapter = BinaryReadableHelper.Create<Chapter>(reader);
        ChapterTests.Validate(ChapterTests.ValidProperties, chapter);
    }

    [TestMethod]
    public void CreateTestNull()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => BinaryReadableHelper.Create<Chapter>(null!));
    }
}
