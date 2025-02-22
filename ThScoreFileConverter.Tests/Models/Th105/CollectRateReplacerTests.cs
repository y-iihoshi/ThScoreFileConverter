﻿using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th105;
using ThScoreFileConverter.Models.Th105;

namespace ThScoreFileConverter.Tests.Models.Th105;

[TestClass]
public class CollectRateReplacerTests
{
    private static IClearData<Chara> MockClearData()
    {
        var spellCardResults = new[]
        {
            SpellCardResultTests.MockSpellCardResult(Chara.Reimu, Level.Normal, 5, 34, 12, default),
            SpellCardResultTests.MockSpellCardResult(Chara.Reimu, Level.Hard, 6, 78, 56, default),
            SpellCardResultTests.MockSpellCardResult(Chara.Iku, Level.Hard, 10, 90, 0, default),
            SpellCardResultTests.MockSpellCardResult(Chara.Tenshi, Level.Hard, 18, 0, 0, default),
        }.ToDictionary(result => (result.Enemy, result.Id));

        var mock = Substitute.For<IClearData<Chara>>();
        _ = mock.SpellCardResults.Returns(spellCardResults);
        return mock;
    }

    internal static IReadOnlyDictionary<Chara, IClearData<Chara>> ClearDataDictionary { get; } =
        new[] { (Chara.Marisa, MockClearData()) }.ToDictionary();

    [TestMethod]
    public void CollectRateReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CollectRateReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData<Chara>>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestGotCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T105CRGHMR1").ShouldBe("invoked: 1");
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T105CRGHMR2").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestLevelTotalGotCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T105CRGTMR1").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestLevelTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T105CRGTMR2").ShouldBe("invoked: 3");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData<Chara>>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        replacer.Replace("%T105CRGHMR1").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptySpellCardResults()
    {
        var clearData = Substitute.For<IClearData<Chara>>();
        _ = clearData.SpellCardResults.Returns(ImmutableDictionary<(Chara, int), ISpellCardResult<Chara>>.Empty);

        var dictionary = new[] { (Chara.Marisa, clearData) }.ToDictionary();
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        replacer.Replace("%T105CRGHMR1").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T105XXXHMR1").ShouldBe("%T105XXXHMR1");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T105CRGXMR1").ShouldBe("%T105CRGXMR1");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T105CRGHXX1").ShouldBe("%T105CRGHXX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T105CRGHMRX").ShouldBe("%T105CRGHMRX");
    }
}
