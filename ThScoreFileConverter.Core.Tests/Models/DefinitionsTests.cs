using System.Collections.Generic;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Tests.UnitTesting;

namespace ThScoreFileConverter.Core.Tests.Models;

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
    }

    public static IEnumerable<object[]> InvalidLevels => TestHelper.GetInvalidEnumerators(typeof(Level));

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
