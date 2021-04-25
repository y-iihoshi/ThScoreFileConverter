using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th123;

namespace ThScoreFileConverterTests.Models.Th123
{
    [TestClass]
    public class StageInfoTests
    {
        [TestMethod]
        public void StageInfoTest()
        {
            Th105.StageInfoTests.StageInfoTestHelper<Chara>();
        }
    }
}
