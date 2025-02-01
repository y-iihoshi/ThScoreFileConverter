using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th06;
using ThScoreFileConverter.Models.Th06;

namespace ThScoreFileConverter.Tests.Models.Th06;

[TestClass]
public class PracticeReplacerTests
{
    internal static IReadOnlyDictionary<(Chara, Level, Stage), IPracticeScore> PracticeScores { get; } =
        new[] { PracticeScoreTests.MockPracticeScore() }
        .ToDictionary(score => (score.Chara, score.Level, score.Stage));

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
    public void ReplaceTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);

        replacer.Replace("%T06PRACHRB6").ShouldBe("invoked: 123456");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var practiceScores = ImmutableDictionary<(Chara, Level, Stage), IPracticeScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        replacer.Replace("%T06PRACHRB6").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestLevelExtra()
    {
        var mock = PracticeScoreTests.MockPracticeScore();
        _ = mock.Level.Returns(Level.Extra);
        var practiceScores = new[] { mock }.ToDictionary(score => (score.Chara, score.Level, score.Stage));
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        replacer.Replace("%T06PRACXRB6").ShouldBe("%T06PRACXRB6");
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        var mock = PracticeScoreTests.MockPracticeScore();
        _ = mock.Stage.Returns(Stage.Extra);
        var practiceScores = new[] { mock }.ToDictionary(score => (score.Chara, score.Level, score.Stage));
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        replacer.Replace("%T06PRACHRBX").ShouldBe("%T06PRACHRBX");
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T06PRACNRB6").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T06PRACHRA6").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T06PRACHRB5").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T06XXXXHRB6").ShouldBe("%T06XXXXHRB6");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T06PRACYRB6").ShouldBe("%T06PRACYRB6");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T06PRACHXX6").ShouldBe("%T06PRACHXX6");
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T06PRACHRBY").ShouldBe("%T06PRACHRBY");
    }
}
