using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Moq;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models.Th123;
using ThScoreFileConverter.Models.Th123;
using IClearData = ThScoreFileConverter.Models.Th105.IClearData<ThScoreFileConverter.Core.Models.Th123.Chara>;
using ISpellCardResult = ThScoreFileConverter.Models.Th105.ISpellCardResult<ThScoreFileConverter.Core.Models.Th123.Chara>;

namespace ThScoreFileConverter.Tests.Models.Th123;

[TestClass]
public class CardReplacerTests
{
    private static IClearData CreateClearData()
    {
#if false
        return Mock.Of<IClearData>(
            c => c.SpellCardResults == new[]
            {
                Mock.Of<ISpellCardResult>(
                    s => (s.Enemy == Chara.Meiling) && (s.Id == 0) && (s.TrialCount == 1)),
                Mock.Of<ISpellCardResult>(
                    s => (s.Enemy == Chara.Meiling) && (s.Id == 1) && (s.TrialCount == 0)),
            }.ToDictionary(result => (result.Enemy, result.Id)));   // causes CS8143
#else
        var mock = new Mock<IClearData>();
        _ = mock.SetupGet(m => m.SpellCardResults).Returns(
            new[]
            {
                Mock.Of<ISpellCardResult>(m => (m.Enemy == Chara.Meiling) && (m.Id == 0) && (m.TrialCount == 1)),
                Mock.Of<ISpellCardResult>(m => (m.Enemy == Chara.Meiling) && (m.Id == 1) && (m.TrialCount == 0)),
            }.ToDictionary(result => (result.Enemy, result.Id)));
        return mock.Object;
#endif
    }

    internal static IReadOnlyDictionary<Chara, IClearData> ClearDataDictionary { get; } =
        new[] { (Chara.Cirno, CreateClearData()) }.ToDictionary();

    [TestMethod]
    public void CardReplacerTest()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CardReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData>.Empty;
        var replacer = new CardReplacer(dictionary, false);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("彩翔「飛花落葉」", replacer.Replace("%T123CARD09CIN"));
        Assert.AreEqual("彩翔「飛花落葉」", replacer.Replace("%T123CARD10CIN"));
    }

    [TestMethod]
    public void ReplaceTestRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("Easy", replacer.Replace("%T123CARD09CIR"));
        Assert.AreEqual("Normal", replacer.Replace("%T123CARD10CIR"));
    }

    [TestMethod]
    public void ReplaceTestUntriedName()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        Assert.AreEqual("彩翔「飛花落葉」", replacer.Replace("%T123CARD09CIN"));
        Assert.AreEqual("??????????", replacer.Replace("%T123CARD10CIN"));
    }

    [TestMethod]
    public void ReplaceTestUntriedRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        Assert.AreEqual("Easy", replacer.Replace("%T123CARD09CIR"));
        Assert.AreEqual("?????", replacer.Replace("%T123CARD10CIR"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData>.Empty;
        var replacer = new CardReplacer(dictionary, true);
        Assert.AreEqual("??????????", replacer.Replace("%T123CARD09CIN"));
    }

    [TestMethod]
    public void ReplaceTestEmptySpellCardResults()
    {
        var dictionary = new Dictionary<Chara, IClearData>
        {
            {
                Chara.Marisa,
                Mock.Of<IClearData>(
                    m => m.SpellCardResults == ImmutableDictionary<(Chara, int), ISpellCardResult>.Empty)
            },
        };
        var replacer = new CardReplacer(dictionary, true);
        Assert.AreEqual("??????????", replacer.Replace("%T123CARD09CIN"));
    }

    [TestMethod]
    public void ReplaceTestTotalNumber()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("%T123CARD00CIN", replacer.Replace("%T123CARD00CIN"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("%T123CARD01MRN", replacer.Replace("%T123CARD01MRN"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("%T123XXXX09CIN", replacer.Replace("%T123XXXX09CIN"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("%T123CARD65CIN", replacer.Replace("%T123CARD65CIN"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("%T123CARD09XXN", replacer.Replace("%T123CARD09XXN"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("%T123CARD09CIX", replacer.Replace("%T123CARD09CIX"));
    }
}
