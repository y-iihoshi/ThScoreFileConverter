﻿using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverter.Tests.Models.Th07;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class FileHeaderTests
{
    [TestMethod]
    public void IsValidTest()
    {
        var array = FileHeaderBaseTests.MakeByteArray(FileHeaderBaseTests.MakeProperties(0x01, 0x1C));
        var header = TestUtils.Create<FileHeader>(array);

        header.IsValid.ShouldBeTrue();
    }

    [TestMethod]
    public void IsValidTestInitial()
    {
        var header = new FileHeader();
        header.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void IsValidTestInvalidBase()
    {
        var properties = FileHeaderBaseTests.MakeProperties(0x01, 0x1C);
        ++properties.size;
        var array = FileHeaderBaseTests.MakeByteArray(properties);
        var header = TestUtils.Create<FileHeader>(array);

        header.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void IsValidTestInvalidVersion()
    {
        var array = FileHeaderBaseTests.MakeByteArray(FileHeaderBaseTests.MakeProperties(0x02, 0x1C));
        var header = TestUtils.Create<FileHeader>(array);

        header.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void IsValidTestInvalidSize()
    {
        var array = FileHeaderBaseTests.MakeByteArray(FileHeaderBaseTests.MakeProperties(0x01, 0x1D));
        var header = TestUtils.Create<FileHeader>(array);

        header.IsValid.ShouldBeFalse();
    }
}
