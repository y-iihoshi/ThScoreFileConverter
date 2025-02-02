using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th07;
using ThScoreFileConverter.Models.Th07;

namespace ThScoreFileConverter.Tests.Models.Th07;

[TestClass]
public class PracticeReplacerTests
{
    internal static IReadOnlyDictionary<(Chara, Level, Stage), IPracticeScore> PracticeScores { get; } =
        new[] { PracticeScoreTests.MockPracticeScore() }
        .ToDictionary(element => (element.Chara, element.Level, element.Stage));

    [TestMethod]
    public void PracticeReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void PracticeReplacerTestEmpty()
    {
        var practiceScores = ImmutableDictionary<(Chara, Level, Stage), IPracticeScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);

        replacer.Replace("%T07PRACHRB61").ShouldBe("invoked: 1234560");
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T07PRACHRB62").ShouldBe("invoked: 987");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var practiceScores = ImmutableDictionary<(Chara, Level, Stage), IPracticeScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        replacer.Replace("%T07PRACHRB61").ShouldBe("invoked: 0");
        replacer.Replace("%T07PRACHRB62").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestLevelExtra()
    {
        var mock = PracticeScoreTests.MockPracticeScore();
        _ = mock.Level.Returns(Level.Extra);
        var practiceScores = new[] { mock }.ToDictionary(score => (score.Chara, score.Level, score.Stage));
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        replacer.Replace("%T07PRACXRB61").ShouldBe("%T07PRACXRB61");
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        var mock = PracticeScoreTests.MockPracticeScore();
        _ = mock.Stage.Returns(Stage.Extra);
        var practiceScores = new[] { mock }.ToDictionary(score => (score.Chara, score.Level, score.Stage));
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        replacer.Replace("%T07PRACHRBX1").ShouldBe("%T07PRACHRBX1");
    }

    [TestMethod]
    public void ReplaceTestLevelPhantasm()
    {
        var mock = PracticeScoreTests.MockPracticeScore();
        _ = mock.Level.Returns(Level.Phantasm);
        var practiceScores = new[] { mock }.ToDictionary(score => (score.Chara, score.Level, score.Stage));
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        replacer.Replace("%T07PRACPRB61").ShouldBe("%T07PRACPRB61");
    }

    [TestMethod]
    public void ReplaceTestStagePhantasm()
    {
        var mock = PracticeScoreTests.MockPracticeScore();
        _ = mock.Stage.Returns(Stage.Phantasm);
        var practiceScores = new[] { mock }.ToDictionary(score => (score.Chara, score.Level, score.Stage));
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        replacer.Replace("%T07PRACHRBP1").ShouldBe("%T07PRACHRBP1");
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T07PRACNRB61").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T07PRACHRA61").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T07PRACHRB51").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T07XXXXHRB61").ShouldBe("%T07XXXXHRB61");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T07PRACYRB61").ShouldBe("%T07PRACYRB61");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T07PRACHXX61").ShouldBe("%T07PRACHXX61");
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T07PRACHRBY1").ShouldBe("%T07PRACHRBY1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T07PRACHRB6X").ShouldBe("%T07PRACHRB6X");
    }
}
