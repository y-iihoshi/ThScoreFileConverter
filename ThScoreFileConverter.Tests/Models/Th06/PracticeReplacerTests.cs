using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th06;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;

namespace ThScoreFileConverter.Tests.Models.Th06;

[TestClass]
public class PracticeReplacerTests
{
    internal static IReadOnlyDictionary<(Chara, Level, Stage), IPracticeScore> PracticeScores { get; } =
        new[] { PracticeScoreTests.MockPracticeScore() }
        .ToDictionary(score => (score.Chara, score.Level, score.Stage));

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<int>()).Returns(callInfo => $"invoked: {(int)callInfo[0]}");
        return mock;
    }

    [TestMethod]
    public void PracticeReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void PracticeReplacerTestEmpty()
    {
        var practiceScores = ImmutableDictionary<(Chara, Level, Stage), IPracticeScore>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);

        Assert.AreEqual("invoked: 123456", replacer.Replace("%T06PRACHRB6"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var practiceScores = ImmutableDictionary<(Chara, Level, Stage), IPracticeScore>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T06PRACHRB6"));
    }

    [TestMethod]
    public void ReplaceTestLevelExtra()
    {
        var mock = PracticeScoreTests.MockPracticeScore();
        _ = mock.Level.Returns(Level.Extra);
        var practiceScores = new[] { mock }.ToDictionary(score => (score.Chara, score.Level, score.Stage));
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        Assert.AreEqual("%T06PRACXRB6", replacer.Replace("%T06PRACXRB6"));
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        var mock = PracticeScoreTests.MockPracticeScore();
        _ = mock.Stage.Returns(Stage.Extra);
        var practiceScores = new[] { mock }.ToDictionary(score => (score.Chara, score.Level, score.Stage));
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        Assert.AreEqual("%T06PRACHRBX", replacer.Replace("%T06PRACHRBX"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T06PRACNRB6"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T06PRACHRA6"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T06PRACHRB5"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("%T06XXXXHRB6", replacer.Replace("%T06XXXXHRB6"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("%T06PRACYRB6", replacer.Replace("%T06PRACYRB6"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("%T06PRACHXX6", replacer.Replace("%T06PRACHXX6"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("%T06PRACHRBY", replacer.Replace("%T06PRACHRBY"));
    }
}
