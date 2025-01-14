﻿using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th123;

namespace ThScoreFileConverter.Core.Tests.Models.Th123;

[TestClass]
public class DefinitionsTests
{
    [TestMethod]
    public void HasStoryTestValidChara()
    {
        Assert.IsFalse(Definitions.HasStory(Chara.Reimu));
        Assert.IsFalse(Definitions.HasStory(Chara.Marisa));
        Assert.IsFalse(Definitions.HasStory(Chara.Sakuya));
        Assert.IsFalse(Definitions.HasStory(Chara.Alice));
        Assert.IsFalse(Definitions.HasStory(Chara.Patchouli));
        Assert.IsFalse(Definitions.HasStory(Chara.Youmu));
        Assert.IsFalse(Definitions.HasStory(Chara.Remilia));
        Assert.IsFalse(Definitions.HasStory(Chara.Yuyuko));
        Assert.IsFalse(Definitions.HasStory(Chara.Yukari));
        Assert.IsFalse(Definitions.HasStory(Chara.Suika));
        Assert.IsFalse(Definitions.HasStory(Chara.Reisen));
        Assert.IsFalse(Definitions.HasStory(Chara.Aya));
        Assert.IsFalse(Definitions.HasStory(Chara.Komachi));
        Assert.IsFalse(Definitions.HasStory(Chara.Iku));
        Assert.IsFalse(Definitions.HasStory(Chara.Tenshi));
        Assert.IsTrue(Definitions.HasStory(Chara.Sanae));
        Assert.IsTrue(Definitions.HasStory(Chara.Cirno));
        Assert.IsTrue(Definitions.HasStory(Chara.Meiling));
        Assert.IsFalse(Definitions.HasStory(Chara.Utsuho));
        Assert.IsFalse(Definitions.HasStory(Chara.Suwako));
        Assert.IsFalse(Definitions.HasStory(Chara.Catfish));
    }

    public static IEnumerable<object[]> InvalidCharacters => TestHelper.GetInvalidEnumerators<Chara>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidCharacters))]
    public void CanPracticeTestInvalidChara(int chara)
    {
        Assert.IsFalse(Definitions.HasStory((Chara)chara));
    }
}
