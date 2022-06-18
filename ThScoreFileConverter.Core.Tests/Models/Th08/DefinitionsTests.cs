using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Core.Tests.UnitTesting;

namespace ThScoreFileConverter.Core.Tests.Models.Th08;

[TestClass]
public class DefinitionsTests
{
    [TestMethod]
    public void CanPracticeTestValidStage()
    {
        Assert.IsTrue(Definitions.CanPractice(Stage.One));
        Assert.IsTrue(Definitions.CanPractice(Stage.Two));
        Assert.IsTrue(Definitions.CanPractice(Stage.Three));
        Assert.IsTrue(Definitions.CanPractice(Stage.FourUncanny));
        Assert.IsTrue(Definitions.CanPractice(Stage.FourPowerful));
        Assert.IsTrue(Definitions.CanPractice(Stage.Five));
        Assert.IsTrue(Definitions.CanPractice(Stage.FinalA));
        Assert.IsTrue(Definitions.CanPractice(Stage.FinalB));
        Assert.IsFalse(Definitions.CanPractice(Stage.Extra));
    }

    public static IEnumerable<object[]> InvalidStages => TestHelper.GetInvalidEnumerators(typeof(Stage));

    [DataTestMethod]
    [DynamicData(nameof(InvalidStages))]
    public void CanPracticeTestInvalidStage(int level)
    {
        var invalid = TestHelper.Cast<Stage>(level);
        Assert.IsFalse(Definitions.CanPractice(invalid));
    }
}
