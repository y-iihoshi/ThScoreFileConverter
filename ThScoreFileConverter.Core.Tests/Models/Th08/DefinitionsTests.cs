using ThScoreFileConverter.Core.Models.Th08;

namespace ThScoreFileConverter.Core.Tests.Models.Th08;

[TestClass]
public class DefinitionsTests
{
    [TestMethod]
    public void CanPracticeTestValidStage()
    {
        Definitions.CanPractice(Stage.One).ShouldBeTrue();
        Definitions.CanPractice(Stage.Two).ShouldBeTrue();
        Definitions.CanPractice(Stage.Three).ShouldBeTrue();
        Definitions.CanPractice(Stage.FourUncanny).ShouldBeTrue();
        Definitions.CanPractice(Stage.FourPowerful).ShouldBeTrue();
        Definitions.CanPractice(Stage.Five).ShouldBeTrue();
        Definitions.CanPractice(Stage.FinalA).ShouldBeTrue();
        Definitions.CanPractice(Stage.FinalB).ShouldBeTrue();
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
