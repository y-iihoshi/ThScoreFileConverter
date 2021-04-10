using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.UnitTesting;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class FileHeaderTests
    {
        [TestMethod]
        public void IsValidTest()
        {
            var array = FileHeaderBaseTests.MakeByteArray(FileHeaderBaseTests.MakeProperties(0x0B, 0x1C));
            var header = TestUtils.Create<FileHeader>(array);

            Assert.IsTrue(header.IsValid);
        }

        [TestMethod]
        public void IsValidTestInitial()
        {
            var header = new FileHeader();
            Assert.IsFalse(header.IsValid);
        }

        [TestMethod]
        public void IsValidTestInvalidBase()
        {
            var properties = FileHeaderBaseTests.MakeProperties(0x0B, 0x1C);
            ++properties.size;
            var array = FileHeaderBaseTests.MakeByteArray(properties);
            var header = TestUtils.Create<FileHeader>(array);

            Assert.IsFalse(header.IsValid);
        }

        [TestMethod]
        public void IsValidTestInvalidVersion()
        {
            var array = FileHeaderBaseTests.MakeByteArray(FileHeaderBaseTests.MakeProperties(0x0C, 0x1C));
            var header = TestUtils.Create<FileHeader>(array);

            Assert.IsFalse(header.IsValid);
        }

        [TestMethod]
        public void IsValidTestInvalidSize()
        {
            var array = FileHeaderBaseTests.MakeByteArray(FileHeaderBaseTests.MakeProperties(0x0B, 0x1D));
            var header = TestUtils.Create<FileHeader>(array);

            Assert.IsFalse(header.IsValid);
        }
    }
}
