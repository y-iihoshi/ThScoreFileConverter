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

    public static IEnumerable<object[]> InvalidLevels => TestHelper.GetInvalidEnumerators<Level>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void CanPracticeTestInvalidLevel(int level)
    {
        Assert.IsFalse(Definitions.CanPractice((Level)level));
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

    public static IEnumerable<object[]> InvalidStages => TestHelper.GetInvalidEnumerators<Stage>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidStages))]
    public void CanPracticeTestInvalidStage(int stage)
    {
        Assert.IsFalse(Definitions.CanPractice((Stage)stage));
    }
}
