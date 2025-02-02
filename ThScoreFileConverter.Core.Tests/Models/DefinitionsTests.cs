using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Core.Tests.Models;

[TestClass]
public class DefinitionsTests
{
    [TestMethod]
    public void CanPracticeTestValidLevel()
    {
        Definitions.CanPractice(Level.Easy).ShouldBeTrue();
        Definitions.CanPractice(Level.Normal).ShouldBeTrue();
        Definitions.CanPractice(Level.Hard).ShouldBeTrue();
        Definitions.CanPractice(Level.Lunatic).ShouldBeTrue();
        Definitions.CanPractice(Level.Extra).ShouldBeFalse();
    }

    public static IEnumerable<object[]> InvalidLevels => TestHelper.GetInvalidEnumerators<Level>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void CanPracticeTestInvalidLevel(int level)
    {
        Definitions.CanPractice((Level)level).ShouldBeFalse();
    }

    [TestMethod]
    public void CanPracticeTestValidStage()
    {
        Definitions.CanPractice(Stage.One).ShouldBeTrue();
        Definitions.CanPractice(Stage.Two).ShouldBeTrue();
        Definitions.CanPractice(Stage.Three).ShouldBeTrue();
        Definitions.CanPractice(Stage.Four).ShouldBeTrue();
        Definitions.CanPractice(Stage.Five).ShouldBeTrue();
        Definitions.CanPractice(Stage.Six).ShouldBeTrue();
        Definitions.CanPractice(Stage.Extra).ShouldBeFalse();
    }

    public static IEnumerable<object[]> InvalidStages => TestHelper.GetInvalidEnumerators<Stage>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidStages))]
    public void CanPracticeTestInvalidStage(int stage)
    {
        Definitions.CanPractice((Stage)stage).ShouldBeFalse();
    }
}
