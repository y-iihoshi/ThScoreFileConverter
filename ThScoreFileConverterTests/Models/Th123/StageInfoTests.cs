using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Th123
{
    [TestClass]
    public class StageInfoTests
    {
        [TestMethod]
        public void StageInfoTest()
            => Th105.StageInfoTests.StageInfoTestHelper<Th123Converter.Stage, Th123Converter.Chara>();
    }
}
