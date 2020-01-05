using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th11;
using ThScoreFileConverterTests.Models.Th10.Stubs;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th10.StageProgress>;
using StageProgress = ThScoreFileConverter.Models.Th10.StageProgress;

namespace ThScoreFileConverterTests.Models.Th11
{
    [TestClass]
    public class ScoreDataTests
    {
        internal static ScoreDataStub<StageProgress> ValidStub { get; }
            = Th10.ScoreDataTests.MakeValidStub<StageProgress>();

        internal static byte[] MakeByteArray(IScoreData scoreData)
            => Th10.ScoreDataTests.MakeByteArray(scoreData, 4);

        [TestMethod]
        public void ReadFromTest()
        {
            var array = MakeByteArray(ValidStub);
            var scoreData = TestUtils.Create<ScoreData>(array);

            Th10.ScoreDataTests.Validate(ValidStub, scoreData);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull()
        {
            var scoreData = new ScoreData();
            scoreData.ReadFrom(null!);

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
