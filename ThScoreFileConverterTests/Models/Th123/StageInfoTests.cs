using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th123;
using Stage = ThScoreFileConverter.Models.Th105.Stage;

namespace ThScoreFileConverterTests.Models.Th123
{
    [TestClass]
    public class StageInfoTests
    {
        [TestMethod]
        public void StageInfoTest() => Th105.StageInfoTests.StageInfoTestHelper<Stage, Chara>();
    }
}
