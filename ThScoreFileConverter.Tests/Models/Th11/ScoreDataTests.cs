using ThScoreFileConverter.Models.Th11;
using ThScoreFileConverter.Tests.UnitTesting;
using StageProgress = ThScoreFileConverter.Models.Th10.StageProgress;

namespace ThScoreFileConverter.Tests.Models.Th11;

[TestClass]
public class ScoreDataTests
{
    internal static int UnknownSize { get; } = 4;

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

    public static IEnumerable<object[]> InvalidStageProgresses => TestUtils.GetInvalidEnumerators<StageProgress>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidStageProgresses))]
    public void ReadFromTestInvalidStageProgress(int stageProgress)
    {
        Th10.ScoreDataTests.ReadFromTestInvalidStageProgressHelper<ScoreData, StageProgress>(
            UnknownSize, stageProgress);
    }
}
