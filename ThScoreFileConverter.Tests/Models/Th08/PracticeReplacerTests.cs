using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using Stage = ThScoreFileConverter.Core.Models.Th08.Stage;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class PracticeReplacerTests
{
    internal static IReadOnlyDictionary<Chara, IPracticeScore> PracticeScores { get; } =
        new[] { PracticeScoreTests.MockPracticeScore() }.ToDictionary(score => score.Chara);

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
        var practiceScores = ImmutableDictionary<Chara, IPracticeScore>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("invoked: 260", replacer.Replace("%T08PRACHMA6A1"));
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("invoked: 62", replacer.Replace("%T08PRACHMA6A2"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var practiceScores = ImmutableDictionary<Chara, IPracticeScore>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08PRACHMA6A1"));
    }

    [TestMethod]
    public void ReplaceTestEmptyHighScores()
    {
        var practiceScoreMock = PracticeScoreTests.MockPracticeScore();
        _ = practiceScoreMock.HighScores.Returns(ImmutableDictionary<(Stage, Level), int>.Empty);
        var practiceScores = new[] { practiceScoreMock }.ToDictionary(score => score.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08PRACHMA6A1"));
    }

    [TestMethod]
    public void ReplaceTestEmptyPlayCounts()
    {
        var practiceScoreMock = PracticeScoreTests.MockPracticeScore();
        _ = practiceScoreMock.PlayCounts.Returns(ImmutableDictionary<(Stage, Level), int>.Empty);
        var practiceScores = new[] { practiceScoreMock }.ToDictionary(score => score.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08PRACHMA6A2"));
    }

    [TestMethod]
    public void ReplaceTestLevelExtra()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("%T08PRACXMA6A1", replacer.Replace("%T08PRACXMA6A1"));
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("%T08PRACHMAEX1", replacer.Replace("%T08PRACHMAEX1"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var practiceScoreMock = PracticeScoreTests.MockPracticeScore();
        var highScores = practiceScoreMock.HighScores;
        _ = practiceScoreMock.HighScores.Returns(highScores.Where(pair => pair.Key.Level != Level.Normal).ToDictionary());
        var practiceScores = new[] { practiceScoreMock }.ToDictionary(score => score.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08PRACNMA6A1"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08PRACHRY6A1"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentStage()
    {
        var practiceScoreMock = PracticeScoreTests.MockPracticeScore();
        var highScores = practiceScoreMock.HighScores;
        _ = practiceScoreMock.HighScores.Returns(highScores.Where(pair => pair.Key.Stage != Stage.Five).ToDictionary());
        var practiceScores = new[] { practiceScoreMock }.ToDictionary(score => score.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08PRACHMA5A1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("%T08XXXXHMA6A1", replacer.Replace("%T08XXXXHMA6A1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("%T08PRACYMA6A1", replacer.Replace("%T08PRACYMA6A1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("%T08PRACHXX6A1", replacer.Replace("%T08PRACHXX6A1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("%T08PRACHMAXX1", replacer.Replace("%T08PRACHMAXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        Assert.AreEqual("%T08PRACHMA6AX", replacer.Replace("%T08PRACHMA6AX"));
    }
}
