using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Core.Models.Th175;
using ThScoreFileConverter.Models.Th175;
using ThScoreFileConverter.Squirrel;

namespace ThScoreFileConverter.Tests.Models.Th175;

[TestClass]
public class SaveDataTests
{
    internal static void ValidateAsDefault(SaveData saveData)
    {
        Assert.IsNotNull(saveData);
        Assert.AreEqual(0, saveData.ScoreDictionary.Count);
        Assert.AreEqual(0, saveData.TimeDictionary.Count);
        Assert.AreEqual(0, saveData.SpellDictionary.Count);
        Assert.AreEqual(0, saveData.TutorialCountDictionary.Count);
        Assert.AreEqual(0, saveData.ReachedStageDictionary.Count);
        Assert.AreEqual(0, saveData.UseCountDictionary.Count);
        Assert.AreEqual(0, saveData.RetireCountDictionary.Count);
        Assert.AreEqual(0, saveData.ClearCountDictionary.Count);
        Assert.AreEqual(0, saveData.PerfectClearCountDictionary.Count);
        Assert.AreEqual(0, saveData.EndingCountDictionary.Count);
        Assert.AreEqual(0, saveData.BgmPlayCountDictionary.Count);
    }

    [TestMethod]
    public void SaveDataTest()
    {
        var saveData = new SaveData();
        ValidateAsDefault(saveData);
    }

    [TestMethod]
    public void SaveDataTestEmpty()
    {
        var saveData = new SaveData(new SQTable());
        ValidateAsDefault(saveData);
    }

    [TestMethod]
    public void SaveDataTestInvalid()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            { new SQInteger(1), new SQInteger(2) },
        }));

        ValidateAsDefault(saveData);
    }

    [TestMethod]
    public void SaveDataTestInvalidDictionary()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            { new SQString("bgm_play_count"), new SQInteger(2) },
        }));

        ValidateAsDefault(saveData);
    }

    [TestMethod]
    public void SaveDataTestEmptyStringIntDictionary()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            { new SQString("bgm_play_count"), new SQTable() },
        }));

        ValidateAsDefault(saveData);
    }

    [TestMethod]
    public void SaveDataTestInvalidStringIntDictionary()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            {
                new SQString("bgm_play_count"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    { new SQInteger(2), new SQString("op") },
                })
            },
        }));

        ValidateAsDefault(saveData);
    }

    [TestMethod]
    public void SaveDataTestBgmPlayCountDictionary()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            {
                new SQString("bgm_play_count"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    { new SQString("op"), new SQInteger(2) },
                })
            },
        }));

        Assert.IsNotNull(saveData);
        Assert.AreEqual(0, saveData.ScoreDictionary.Count);
        Assert.AreEqual(0, saveData.TimeDictionary.Count);
        Assert.AreEqual(0, saveData.SpellDictionary.Count);
        Assert.AreEqual(0, saveData.TutorialCountDictionary.Count);
        Assert.AreEqual(0, saveData.ReachedStageDictionary.Count);
        Assert.AreEqual(0, saveData.UseCountDictionary.Count);
        Assert.AreEqual(0, saveData.RetireCountDictionary.Count);
        Assert.AreEqual(0, saveData.ClearCountDictionary.Count);
        Assert.AreEqual(0, saveData.PerfectClearCountDictionary.Count);
        Assert.AreEqual(0, saveData.EndingCountDictionary.Count);
        Assert.AreEqual(1, saveData.BgmPlayCountDictionary.Count);
        Assert.IsTrue(saveData.BgmPlayCountDictionary.ContainsKey("op"));
        Assert.AreEqual(2, saveData.BgmPlayCountDictionary["op"]);
    }

    [TestMethod]
    public void SaveDataTestEmptyCharaIntDictionary()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            { new SQString("tutorial_count"), new SQTable() },
        }));

        ValidateAsDefault(saveData);
    }

    [TestMethod]
    public void SaveDataTestInvalidCharaIntDictionary()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            {
                new SQString("tutorial_count"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    { new SQInteger(3), new SQString("reimu") },
                })
            },
        }));

        ValidateAsDefault(saveData);
    }

    [TestMethod]
    public void SaveDataTestInvalidChara()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            {
                new SQString("tutorial_count"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    { new SQString("unknown"), new SQInteger(3) },
                })
            },
        }));

        ValidateAsDefault(saveData);
    }

    [TestMethod]
    public void SaveDataTestTutorialCountDictionary()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            {
                new SQString("tutorial_count"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    { new SQString("reimu"), new SQInteger(3) },
                })
            },
        }));

        Assert.IsNotNull(saveData);
        Assert.AreEqual(0, saveData.ScoreDictionary.Count);
        Assert.AreEqual(0, saveData.TimeDictionary.Count);
        Assert.AreEqual(0, saveData.SpellDictionary.Count);
        Assert.AreEqual(1, saveData.TutorialCountDictionary.Count);
        Assert.IsTrue(saveData.TutorialCountDictionary.ContainsKey(Chara.Reimu));
        Assert.AreEqual(3, saveData.TutorialCountDictionary[Chara.Reimu]);
        Assert.AreEqual(0, saveData.ReachedStageDictionary.Count);
        Assert.AreEqual(0, saveData.UseCountDictionary.Count);
        Assert.AreEqual(0, saveData.RetireCountDictionary.Count);
        Assert.AreEqual(0, saveData.ClearCountDictionary.Count);
        Assert.AreEqual(0, saveData.PerfectClearCountDictionary.Count);
        Assert.AreEqual(0, saveData.EndingCountDictionary.Count);
        Assert.AreEqual(0, saveData.BgmPlayCountDictionary.Count);
    }

    [TestMethod]
    public void SaveDataTestReachedStageDictionary()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            {
                new SQString("character_reach_stage"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    { new SQString("marisa"), new SQInteger(4) },
                })
            },
        }));

        Assert.IsNotNull(saveData);
        Assert.AreEqual(0, saveData.ScoreDictionary.Count);
        Assert.AreEqual(0, saveData.TimeDictionary.Count);
        Assert.AreEqual(0, saveData.SpellDictionary.Count);
        Assert.AreEqual(0, saveData.TutorialCountDictionary.Count);
        Assert.AreEqual(1, saveData.ReachedStageDictionary.Count);
        Assert.IsTrue(saveData.ReachedStageDictionary.ContainsKey(Chara.Marisa));
        Assert.AreEqual(4, saveData.ReachedStageDictionary[Chara.Marisa]);
        Assert.AreEqual(0, saveData.UseCountDictionary.Count);
        Assert.AreEqual(0, saveData.RetireCountDictionary.Count);
        Assert.AreEqual(0, saveData.ClearCountDictionary.Count);
        Assert.AreEqual(0, saveData.PerfectClearCountDictionary.Count);
        Assert.AreEqual(0, saveData.EndingCountDictionary.Count);
        Assert.AreEqual(0, saveData.BgmPlayCountDictionary.Count);
    }

    [TestMethod]
    public void SaveDataTestUseCountDictionary()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            {
                new SQString("character_use_count"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    { new SQString("kanako"), new SQInteger(5) },
                })
            },
        }));

        Assert.IsNotNull(saveData);
        Assert.AreEqual(0, saveData.ScoreDictionary.Count);
        Assert.AreEqual(0, saveData.TimeDictionary.Count);
        Assert.AreEqual(0, saveData.SpellDictionary.Count);
        Assert.AreEqual(0, saveData.TutorialCountDictionary.Count);
        Assert.AreEqual(0, saveData.ReachedStageDictionary.Count);
        Assert.AreEqual(1, saveData.UseCountDictionary.Count);
        Assert.IsTrue(saveData.UseCountDictionary.ContainsKey(Chara.Kanako));
        Assert.AreEqual(5, saveData.UseCountDictionary[Chara.Kanako]);
        Assert.AreEqual(0, saveData.RetireCountDictionary.Count);
        Assert.AreEqual(0, saveData.ClearCountDictionary.Count);
        Assert.AreEqual(0, saveData.PerfectClearCountDictionary.Count);
        Assert.AreEqual(0, saveData.EndingCountDictionary.Count);
        Assert.AreEqual(0, saveData.BgmPlayCountDictionary.Count);
    }

    [TestMethod]
    public void SaveDataTestRetireCountDictionary()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            {
                new SQString("character_retire_count"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    { new SQString("murasa"), new SQInteger(6) },
                })
            },
        }));

        Assert.IsNotNull(saveData);
        Assert.AreEqual(0, saveData.ScoreDictionary.Count);
        Assert.AreEqual(0, saveData.TimeDictionary.Count);
        Assert.AreEqual(0, saveData.SpellDictionary.Count);
        Assert.AreEqual(0, saveData.TutorialCountDictionary.Count);
        Assert.AreEqual(0, saveData.ReachedStageDictionary.Count);
        Assert.AreEqual(0, saveData.UseCountDictionary.Count);
        Assert.AreEqual(1, saveData.RetireCountDictionary.Count);
        Assert.IsTrue(saveData.RetireCountDictionary.ContainsKey(Chara.Minamitsu));
        Assert.AreEqual(6, saveData.RetireCountDictionary[Chara.Minamitsu]);
        Assert.AreEqual(0, saveData.ClearCountDictionary.Count);
        Assert.AreEqual(0, saveData.PerfectClearCountDictionary.Count);
        Assert.AreEqual(0, saveData.EndingCountDictionary.Count);
        Assert.AreEqual(0, saveData.BgmPlayCountDictionary.Count);
    }

    [TestMethod]
    public void SaveDataTestClearCountDictionary()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            {
                new SQString("character_clear_count"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    { new SQString("jyoon"), new SQInteger(7) },
                })
            },
        }));

        Assert.IsNotNull(saveData);
        Assert.AreEqual(0, saveData.ScoreDictionary.Count);
        Assert.AreEqual(0, saveData.TimeDictionary.Count);
        Assert.AreEqual(0, saveData.SpellDictionary.Count);
        Assert.AreEqual(0, saveData.TutorialCountDictionary.Count);
        Assert.AreEqual(0, saveData.ReachedStageDictionary.Count);
        Assert.AreEqual(0, saveData.UseCountDictionary.Count);
        Assert.AreEqual(0, saveData.RetireCountDictionary.Count);
        Assert.AreEqual(1, saveData.ClearCountDictionary.Count);
        Assert.IsTrue(saveData.ClearCountDictionary.ContainsKey(Chara.JoonShion));
        Assert.AreEqual(7, saveData.ClearCountDictionary[Chara.JoonShion]);
        Assert.AreEqual(0, saveData.PerfectClearCountDictionary.Count);
        Assert.AreEqual(0, saveData.EndingCountDictionary.Count);
        Assert.AreEqual(0, saveData.BgmPlayCountDictionary.Count);
    }

    [TestMethod]
    public void SaveDataTestPerfectClearCountDictionary()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            {
                new SQString("character_perfect_clear_count"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    { new SQString("flandre"), new SQInteger(8) },
                })
            },
        }));

        Assert.IsNotNull(saveData);
        Assert.AreEqual(0, saveData.ScoreDictionary.Count);
        Assert.AreEqual(0, saveData.TimeDictionary.Count);
        Assert.AreEqual(0, saveData.SpellDictionary.Count);
        Assert.AreEqual(0, saveData.TutorialCountDictionary.Count);
        Assert.AreEqual(0, saveData.ReachedStageDictionary.Count);
        Assert.AreEqual(0, saveData.UseCountDictionary.Count);
        Assert.AreEqual(0, saveData.RetireCountDictionary.Count);
        Assert.AreEqual(0, saveData.ClearCountDictionary.Count);
        Assert.AreEqual(1, saveData.PerfectClearCountDictionary.Count);
        Assert.IsTrue(saveData.PerfectClearCountDictionary.ContainsKey(Chara.Flandre));
        Assert.AreEqual(8, saveData.PerfectClearCountDictionary[Chara.Flandre]);
        Assert.AreEqual(0, saveData.EndingCountDictionary.Count);
        Assert.AreEqual(0, saveData.BgmPlayCountDictionary.Count);
    }

    [TestMethod]
    public void SaveDataTestEndingCountDictionary()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            {
                new SQString("ending_count"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    { new SQString("reimu"), new SQInteger(9) },
                })
            },
        }));

        Assert.IsNotNull(saveData);
        Assert.AreEqual(0, saveData.ScoreDictionary.Count);
        Assert.AreEqual(0, saveData.TimeDictionary.Count);
        Assert.AreEqual(0, saveData.SpellDictionary.Count);
        Assert.AreEqual(0, saveData.TutorialCountDictionary.Count);
        Assert.AreEqual(0, saveData.ReachedStageDictionary.Count);
        Assert.AreEqual(0, saveData.UseCountDictionary.Count);
        Assert.AreEqual(0, saveData.RetireCountDictionary.Count);
        Assert.AreEqual(0, saveData.ClearCountDictionary.Count);
        Assert.AreEqual(0, saveData.PerfectClearCountDictionary.Count);
        Assert.AreEqual(1, saveData.EndingCountDictionary.Count);
        Assert.IsTrue(saveData.EndingCountDictionary.ContainsKey(Chara.Reimu));
        Assert.AreEqual(9, saveData.EndingCountDictionary[Chara.Reimu]);
        Assert.AreEqual(0, saveData.BgmPlayCountDictionary.Count);
    }

    [TestMethod]
    public void SaveDataTestEmptyCharaIntArrayDictionary()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            { new SQString("score_easy"), new SQTable() },
        }));

        ValidateAsDefault(saveData);
    }

    [TestMethod]
    public void SaveDataTestInvalidCharaIntArrayDictionary()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            {
                new SQString("score_easy"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    {
                        new SQArray(Enumerable.Range(1, 10).Select(value => new SQInteger(value))),
                        new SQString("reimu")
                    },
                })
            },
        }));

        ValidateAsDefault(saveData);
    }

    [TestMethod]
    public void SaveDataTestInvalidArray()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            {
                new SQString("score_easy"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    { new SQString("reimu"), new SQInteger(1) },
                })
            },
        }));

        ValidateAsDefault(saveData);
    }

    [TestMethod]
    public void SaveDataTestInvalidIntArray()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            {
                new SQString("score_easy"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    {
                        new SQString("reimu"),
                        new SQArray(Enumerable.Range(1, 10).Select(value => new SQFloat(value)))
                    },
                })
            },
        }));

        ValidateAsDefault(saveData);
    }

    [TestMethod]
    public void SaveDataTestScoreDictionary()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            {
                new SQString("score_easy"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    {
                        new SQString("reimu"),
                        new SQArray(Enumerable.Range(1, 10).Select(value => new SQInteger(value)))
                    },
                })
            },
            {
                new SQString("score_normal"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    {
                        new SQString("marisa"),
                        new SQArray(Enumerable.Range(11, 10).Select(value => new SQInteger(value)))
                    },
                })
            },
            {
                new SQString("score_hard"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    {
                        new SQString("kanako"),
                        new SQArray(Enumerable.Range(21, 10).Select(value => new SQInteger(value)))
                    },
                })
            },
            {
                new SQString("score_rush"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    {
                        new SQString("murasa"),
                        new SQArray(Enumerable.Range(31, 10).Select(value => new SQInteger(value)))
                    },
                })
            },
        }));

        Assert.IsNotNull(saveData);
        Assert.AreEqual(4, saveData.ScoreDictionary.Count);
        Assert.IsTrue(saveData.ScoreDictionary.ContainsKey((Level.Easy, Chara.Reimu)));
        Assert.AreEqual(2, saveData.ScoreDictionary[(Level.Easy, Chara.Reimu)].ElementAtOrDefault(1));
        Assert.IsTrue(saveData.ScoreDictionary.ContainsKey((Level.Normal, Chara.Marisa)));
        Assert.AreEqual(13, saveData.ScoreDictionary[(Level.Normal, Chara.Marisa)].ElementAtOrDefault(2));
        Assert.IsTrue(saveData.ScoreDictionary.ContainsKey((Level.Hard, Chara.Kanako)));
        Assert.AreEqual(24, saveData.ScoreDictionary[(Level.Hard, Chara.Kanako)].ElementAtOrDefault(3));
        Assert.IsTrue(saveData.ScoreDictionary.ContainsKey((Level.Rush, Chara.Minamitsu)));
        Assert.AreEqual(35, saveData.ScoreDictionary[(Level.Rush, Chara.Minamitsu)].ElementAtOrDefault(4));
        Assert.AreEqual(0, saveData.TimeDictionary.Count);
        Assert.AreEqual(0, saveData.SpellDictionary.Count);
        Assert.AreEqual(0, saveData.TutorialCountDictionary.Count);
        Assert.AreEqual(0, saveData.ReachedStageDictionary.Count);
        Assert.AreEqual(0, saveData.UseCountDictionary.Count);
        Assert.AreEqual(0, saveData.RetireCountDictionary.Count);
        Assert.AreEqual(0, saveData.ClearCountDictionary.Count);
        Assert.AreEqual(0, saveData.PerfectClearCountDictionary.Count);
        Assert.AreEqual(0, saveData.EndingCountDictionary.Count);
        Assert.AreEqual(0, saveData.BgmPlayCountDictionary.Count);
    }

    [TestMethod]
    public void SaveDataTestTimeDictionary()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            {
                new SQString("time_easy"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    {
                        new SQString("reimu"),
                        new SQArray(Enumerable.Range(1, 10).Select(value => new SQInteger(value)))
                    },
                })
            },
            {
                new SQString("time_normal"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    {
                        new SQString("marisa"),
                        new SQArray(Enumerable.Range(11, 10).Select(value => new SQInteger(value)))
                    },
                })
            },
            {
                new SQString("time_hard"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    {
                        new SQString("kanako"),
                        new SQArray(Enumerable.Range(21, 10).Select(value => new SQInteger(value)))
                    },
                })
            },
            {
                new SQString("time_rush"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    {
                        new SQString("murasa"),
                        new SQArray(Enumerable.Range(31, 10).Select(value => new SQInteger(value)))
                    },
                })
            },
        }));

        Assert.IsNotNull(saveData);
        Assert.AreEqual(0, saveData.ScoreDictionary.Count);
        Assert.AreEqual(4, saveData.TimeDictionary.Count);
        Assert.IsTrue(saveData.TimeDictionary.ContainsKey((Level.Easy, Chara.Reimu)));
        Assert.AreEqual(2, saveData.TimeDictionary[(Level.Easy, Chara.Reimu)].ElementAtOrDefault(1));
        Assert.IsTrue(saveData.TimeDictionary.ContainsKey((Level.Normal, Chara.Marisa)));
        Assert.AreEqual(13, saveData.TimeDictionary[(Level.Normal, Chara.Marisa)].ElementAtOrDefault(2));
        Assert.IsTrue(saveData.TimeDictionary.ContainsKey((Level.Hard, Chara.Kanako)));
        Assert.AreEqual(24, saveData.TimeDictionary[(Level.Hard, Chara.Kanako)].ElementAtOrDefault(3));
        Assert.IsTrue(saveData.TimeDictionary.ContainsKey((Level.Rush, Chara.Minamitsu)));
        Assert.AreEqual(35, saveData.TimeDictionary[(Level.Rush, Chara.Minamitsu)].ElementAtOrDefault(4));
        Assert.AreEqual(0, saveData.SpellDictionary.Count);
        Assert.AreEqual(0, saveData.TutorialCountDictionary.Count);
        Assert.AreEqual(0, saveData.ReachedStageDictionary.Count);
        Assert.AreEqual(0, saveData.UseCountDictionary.Count);
        Assert.AreEqual(0, saveData.RetireCountDictionary.Count);
        Assert.AreEqual(0, saveData.ClearCountDictionary.Count);
        Assert.AreEqual(0, saveData.PerfectClearCountDictionary.Count);
        Assert.AreEqual(0, saveData.EndingCountDictionary.Count);
        Assert.AreEqual(0, saveData.BgmPlayCountDictionary.Count);
    }

    [TestMethod]
    public void SaveDataTestSpellDictionary()
    {
        var saveData = new SaveData(new SQTable(new Dictionary<SQObject, SQObject>
        {
            {
                new SQString("spell_easy"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    {
                        new SQString("reimu"),
                        new SQArray(Enumerable.Range(1, 10).Select(value => new SQInteger(value)))
                    },
                })
            },
            {
                new SQString("spell_normal"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    {
                        new SQString("marisa"),
                        new SQArray(Enumerable.Range(11, 10).Select(value => new SQInteger(value)))
                    },
                })
            },
            {
                new SQString("spell_hard"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    {
                        new SQString("kanako"),
                        new SQArray(Enumerable.Range(21, 10).Select(value => new SQInteger(value)))
                    },
                })
            },
            {
                new SQString("spell_rush"),
                new SQTable(new Dictionary<SQObject, SQObject>
                {
                    {
                        new SQString("murasa"),
                        new SQArray(Enumerable.Range(31, 10).Select(value => new SQInteger(value)))
                    },
                })
            },
        }));

        Assert.IsNotNull(saveData);
        Assert.AreEqual(0, saveData.ScoreDictionary.Count);
        Assert.AreEqual(0, saveData.TimeDictionary.Count);
        Assert.AreEqual(4, saveData.SpellDictionary.Count);
        Assert.IsTrue(saveData.SpellDictionary.ContainsKey((Level.Easy, Chara.Reimu)));
        Assert.AreEqual(2, saveData.SpellDictionary[(Level.Easy, Chara.Reimu)].ElementAtOrDefault(1));
        Assert.IsTrue(saveData.SpellDictionary.ContainsKey((Level.Normal, Chara.Marisa)));
        Assert.AreEqual(13, saveData.SpellDictionary[(Level.Normal, Chara.Marisa)].ElementAtOrDefault(2));
        Assert.IsTrue(saveData.SpellDictionary.ContainsKey((Level.Hard, Chara.Kanako)));
        Assert.AreEqual(24, saveData.SpellDictionary[(Level.Hard, Chara.Kanako)].ElementAtOrDefault(3));
        Assert.IsTrue(saveData.SpellDictionary.ContainsKey((Level.Rush, Chara.Minamitsu)));
        Assert.AreEqual(35, saveData.SpellDictionary[(Level.Rush, Chara.Minamitsu)].ElementAtOrDefault(4));
        Assert.AreEqual(0, saveData.TutorialCountDictionary.Count);
        Assert.AreEqual(0, saveData.ReachedStageDictionary.Count);
        Assert.AreEqual(0, saveData.UseCountDictionary.Count);
        Assert.AreEqual(0, saveData.RetireCountDictionary.Count);
        Assert.AreEqual(0, saveData.ClearCountDictionary.Count);
        Assert.AreEqual(0, saveData.PerfectClearCountDictionary.Count);
        Assert.AreEqual(0, saveData.EndingCountDictionary.Count);
        Assert.AreEqual(0, saveData.BgmPlayCountDictionary.Count);
    }
}
