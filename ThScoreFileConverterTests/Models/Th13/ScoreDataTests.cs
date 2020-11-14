using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th13;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th13
{
    [TestClass]
    public class ScoreDataTests
    {
        internal static byte[] MakeByteArray(IScoreData scoreData)
            => Th10.ScoreDataTests.MakeByteArray(scoreData, 4);

        [TestMethod]
        public void ReadFromTest()
        {
            var mock = Th10.ScoreDataTests.MockScoreData<StageProgress>();
            var array = MakeByteArray(mock.Object);
            var scoreData = TestUtils.Create<ScoreData>(array);

            Th10.ScoreDataTests.Validate(mock.Object, scoreData);
        }

        [TestMethod]
        public void ReadFromTestNull()
        {
            var scoreData = new ScoreData();
            _ = Assert.ThrowsException<ArgumentNullException>(() => scoreData.ReadFrom(null!));
        }
    }
}
