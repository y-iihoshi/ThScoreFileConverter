using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th11;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th10.StageProgress>;
using StageProgress = ThScoreFileConverter.Models.Th10.StageProgress;

namespace ThScoreFileConverterTests.Models.Th11
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
