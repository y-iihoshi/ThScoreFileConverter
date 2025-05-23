﻿using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Models.Th08;
using Stage = ThScoreFileConverter.Core.Models.Th08.Stage;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class PracticeReplacerTests
{
    internal static IReadOnlyDictionary<Chara, IPracticeScore> PracticeScores { get; } =
        new[] { PracticeScoreTests.MockPracticeScore() }.ToDictionary(score => score.Chara);

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
        var practiceScores = ImmutableDictionary<Chara, IPracticeScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T08PRACHMA6A1").ShouldBe("invoked: 260");
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T08PRACHMA6A2").ShouldBe("invoked: 62");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var practiceScores = ImmutableDictionary<Chara, IPracticeScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        replacer.Replace("%T08PRACHMA6A1").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptyHighScores()
    {
        var practiceScoreMock = PracticeScoreTests.MockPracticeScore();
        _ = practiceScoreMock.HighScores.Returns(ImmutableDictionary<(Stage, Level), int>.Empty);
        var practiceScores = new[] { practiceScoreMock }.ToDictionary(score => score.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        replacer.Replace("%T08PRACHMA6A1").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptyPlayCounts()
    {
        var practiceScoreMock = PracticeScoreTests.MockPracticeScore();
        _ = practiceScoreMock.PlayCounts.Returns(ImmutableDictionary<(Stage, Level), int>.Empty);
        var practiceScores = new[] { practiceScoreMock }.ToDictionary(score => score.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        replacer.Replace("%T08PRACHMA6A2").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestLevelExtra()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T08PRACXMA6A1").ShouldBe("%T08PRACXMA6A1");
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T08PRACHMAEX1").ShouldBe("%T08PRACHMAEX1");
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var practiceScoreMock = PracticeScoreTests.MockPracticeScore();
        var highScores = practiceScoreMock.HighScores;
        _ = practiceScoreMock.HighScores.Returns(highScores.Where(pair => pair.Key.Level != Level.Normal).ToDictionary());
        var practiceScores = new[] { practiceScoreMock }.ToDictionary(score => score.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        replacer.Replace("%T08PRACNMA6A1").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T08PRACHRY6A1").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentStage()
    {
        var practiceScoreMock = PracticeScoreTests.MockPracticeScore();
        var highScores = practiceScoreMock.HighScores;
        _ = practiceScoreMock.HighScores.Returns(highScores.Where(pair => pair.Key.Stage != Stage.Five).ToDictionary());
        var practiceScores = new[] { practiceScoreMock }.ToDictionary(score => score.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new PracticeReplacer(practiceScores, formatterMock);
        replacer.Replace("%T08PRACHMA5A1").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T08XXXXHMA6A1").ShouldBe("%T08XXXXHMA6A1");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T08PRACYMA6A1").ShouldBe("%T08PRACYMA6A1");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T08PRACHXX6A1").ShouldBe("%T08PRACHXX6A1");
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T08PRACHMAXX1").ShouldBe("%T08PRACHMAXX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(PracticeScores, formatterMock);
        replacer.Replace("%T08PRACHMA6AX").ShouldBe("%T08PRACHMA6AX");
    }
}
