using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th07;
using ThScoreFileConverter.Core.Tests.UnitTesting;

namespace ThScoreFileConverter.Core.Tests.Models.Th07;

[TestClass]
public class DefinitionsTests
{
    [TestMethod]
    public void CanPracticeTestValidLevel()
    {
        Assert.IsTrue(Definitions.CanPractice(Level.Easy));
        Assert.IsTrue(Definitions.CanPractice(Level.Normal));
        Assert.IsTrue(Definitions.CanPractice(Level.Hard));
        Assert.IsTrue(Definitions.CanPractice(Level.Lunatic));
        Assert.IsFalse(Definitions.CanPractice(Level.Extra));
        Assert.IsFalse(Definitions.CanPractice(Level.Phantasm));
    }

    public static IEnumerable<object[]> InvalidLevels => TestHelper.GetInvalidEnumerators<Level>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void CanPracticeTestInvalidLevel(int level)
    {
        var invalid = TestHelper.Cast<Level>(level);
        Assert.IsFalse(Definitions.CanPractice(invalid));
    }

    [TestMethod]
    public void CanPracticeTestValidStage()
    {
        Assert.IsTrue(Definitions.CanPractice(Stage.One));
        Assert.IsTrue(Definitions.CanPractice(Stage.Two));
        Assert.IsTrue(Definitions.CanPractice(Stage.Three));
        Assert.IsTrue(Definitions.CanPractice(Stage.Four));
        Assert.IsTrue(Definitions.CanPractice(Stage.Five));
        Assert.IsTrue(Definitions.CanPractice(Stage.Six));
        Assert.IsFalse(Definitions.CanPractice(Stage.Extra));
        Assert.IsFalse(Definitions.CanPractice(Stage.Phantasm));
    }

    public static IEnumerable<object[]> InvalidStages => TestHelper.GetInvalidEnumerators<Stage>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidStages))]
    public void CanPracticeTestInvalidStage(int level)
    {
        var invalid = TestHelper.Cast<Stage>(level);
        Assert.IsFalse(Definitions.CanPractice(invalid));
    }
}
