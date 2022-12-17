using System.Collections.Generic;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverter.Tests.UnitTesting;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th128.StageProgress>;

namespace ThScoreFileConverter.Tests.Models.Th128;

[TestClass]
public class ScoreDataTests
{
    internal static byte[] MakeByteArray(IScoreData scoreData)
    {
        return Th10.ScoreDataTests.MakeByteArray(scoreData, UnknownSize);
    }

    internal static int UnknownSize { get; } = 8;

    [TestMethod]
    public void ScoreDataTest()
    {
        Th10.ScoreDataTests.ScoreDataTestHelper<ScoreData, StageProgress>();
    }

    [TestMethod]
    public void ReadFromTest()
    {
        Th10.ScoreDataTests.ReadFromTestHelper<ScoreData, StageProgress>(UnknownSize);
    }

    [TestMethod]
    public void ReadFromTestShortenedName()
    {
        Th10.ScoreDataTests.ReadFromTestShortenedNameHelper<ScoreData, StageProgress>(UnknownSize);
    }

    [TestMethod]
    public void ReadFromTestExceededName()
    {
        Th10.ScoreDataTests.ReadFromTestExceededNameHelper<ScoreData, StageProgress>(UnknownSize);
    }

    public static IEnumerable<object[]> InvalidStageProgresses
        => TestUtils.GetInvalidEnumerators(typeof(StageProgress));

    [DataTestMethod]
    [DynamicData(nameof(InvalidStageProgresses))]
    public void ReadFromTestInvalidStageProgress(int stageProgress)
    {
        Th10.ScoreDataTests.ReadFromTestInvalidStageProgressHelper<ScoreData, StageProgress>(
            UnknownSize, stageProgress);
    }
}
