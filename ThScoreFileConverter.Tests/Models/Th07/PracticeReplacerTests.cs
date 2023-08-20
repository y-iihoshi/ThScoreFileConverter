using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th07;
using ThScoreFileConverter.Models.Th07;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverter.Tests.Models.Th07;

[TestClass]
public class PracticeReplacerTests
{
    internal static IReadOnlyDictionary<(Chara, Level, Stage), IPracticeScore> PracticeScores { get; } =
        new[] { PracticeScoreTests.MockPracticeScore() }
        .ToDictionary(element => (element.Chara, element.Level, element.Stage));

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
    public void ReplaceTestScore()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);

        Assert.AreEqual("invoked: 1234560", replacer.Replace("%T07PRACHRB61"));
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("invoked: 987", replacer.Replace("%T07PRACHRB62"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var practiceScores = ImmutableDictionary<(Chara, Level, Stage), IPracticeScore>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T07PRACHRB61"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T07PRACHRB62"));
    }

    [TestMethod]
    public void ReplaceTestLevelExtra()
    {
        var mock = PracticeScoreTests.MockPracticeScore();
        _ = mock.Level.Returns(Level.Extra);
        var practiceScores = new[] { mock }.ToDictionary(score => (score.Chara, score.Level, score.Stage));
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        Assert.AreEqual("%T07PRACXRB61", replacer.Replace("%T07PRACXRB61"));
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        var mock = PracticeScoreTests.MockPracticeScore();
        _ = mock.Stage.Returns(Stage.Extra);
        var practiceScores = new[] { mock }.ToDictionary(score => (score.Chara, score.Level, score.Stage));
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        Assert.AreEqual("%T07PRACHRBX1", replacer.Replace("%T07PRACHRBX1"));
    }

    [TestMethod]
    public void ReplaceTestLevelPhantasm()
    {
        var mock = PracticeScoreTests.MockPracticeScore();
        _ = mock.Level.Returns(Level.Phantasm);
        var practiceScores = new[] { mock }.ToDictionary(score => (score.Chara, score.Level, score.Stage));
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        Assert.AreEqual("%T07PRACPRB61", replacer.Replace("%T07PRACPRB61"));
    }

    [TestMethod]
    public void ReplaceTestStagePhantasm()
    {
        var mock = PracticeScoreTests.MockPracticeScore();
        _ = mock.Stage.Returns(Stage.Phantasm);
        var practiceScores = new[] { mock }.ToDictionary(score => (score.Chara, score.Level, score.Stage));
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        Assert.AreEqual("%T07PRACHRBP1", replacer.Replace("%T07PRACHRBP1"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T07PRACNRB61"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T07PRACHRA61"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T07PRACHRB51"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("%T07XXXXHRB61", replacer.Replace("%T07XXXXHRB61"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("%T07PRACYRB61", replacer.Replace("%T07PRACYRB61"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("%T07PRACHXX61", replacer.Replace("%T07PRACHXX61"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("%T07PRACHRBY1", replacer.Replace("%T07PRACHRBY1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("%T07PRACHRB6X", replacer.Replace("%T07PRACHRB6X"));
    }
}
