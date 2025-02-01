using ThScoreFileConverter.Core.Models.Th175;
using ThScoreFileConverter.Models.Th175;
using ThScoreFileConverter.Squirrel;

namespace ThScoreFileConverter.Tests.Models.Th175;

[TestClass]
public class SaveDataTests
{
    internal static void ValidateAsDefault(SaveData saveData)
    {
        _ = saveData.ShouldNotBeNull();
        saveData.ScoreDictionary.ShouldBeEmpty();
        saveData.TimeDictionary.ShouldBeEmpty();
        saveData.SpellDictionary.ShouldBeEmpty();
        saveData.TutorialCountDictionary.ShouldBeEmpty();
        saveData.ReachedStageDictionary.ShouldBeEmpty();
        saveData.UseCountDictionary.ShouldBeEmpty();
        saveData.RetireCountDictionary.ShouldBeEmpty();
        saveData.ClearCountDictionary.ShouldBeEmpty();
        saveData.PerfectClearCountDictionary.ShouldBeEmpty();
        saveData.EndingCountDictionary.ShouldBeEmpty();
        saveData.BgmPlayCountDictionary.ShouldBeEmpty();
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

        _ = saveData.ShouldNotBeNull();
        saveData.ScoreDictionary.ShouldBeEmpty();
        saveData.TimeDictionary.ShouldBeEmpty();
        saveData.SpellDictionary.ShouldBeEmpty();
        saveData.TutorialCountDictionary.ShouldBeEmpty();
        saveData.ReachedStageDictionary.ShouldBeEmpty();
        saveData.UseCountDictionary.ShouldBeEmpty();
        saveData.RetireCountDictionary.ShouldBeEmpty();
        saveData.ClearCountDictionary.ShouldBeEmpty();
        saveData.PerfectClearCountDictionary.ShouldBeEmpty();
        saveData.EndingCountDictionary.ShouldBeEmpty();
        saveData.BgmPlayCountDictionary.Count.ShouldBe(1);
#if NET9_0_OR_GREATER
        saveData.BgmPlayCountDictionary.ShouldContainKeyAndValue("op", 2);
#else
        saveData.BgmPlayCountDictionary.ContainsKey("op").ShouldBeTrue();
        saveData.BgmPlayCountDictionary["op"].ShouldBe(2);
#endif
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

        _ = saveData.ShouldNotBeNull();
        saveData.ScoreDictionary.ShouldBeEmpty();
        saveData.TimeDictionary.ShouldBeEmpty();
        saveData.SpellDictionary.ShouldBeEmpty();
        saveData.TutorialCountDictionary.Count.ShouldBe(1);
#if NET9_0_OR_GREATER
        saveData.TutorialCountDictionary.ShouldContainKeyAndValue(Chara.Reimu, 3);
#else
        saveData.TutorialCountDictionary.ContainsKey(Chara.Reimu).ShouldBeTrue();
        saveData.TutorialCountDictionary[Chara.Reimu].ShouldBe(3);
#endif
        saveData.ReachedStageDictionary.ShouldBeEmpty();
        saveData.UseCountDictionary.ShouldBeEmpty();
        saveData.RetireCountDictionary.ShouldBeEmpty();
        saveData.ClearCountDictionary.ShouldBeEmpty();
        saveData.PerfectClearCountDictionary.ShouldBeEmpty();
        saveData.EndingCountDictionary.ShouldBeEmpty();
        saveData.BgmPlayCountDictionary.ShouldBeEmpty();
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

        _ = saveData.ShouldNotBeNull();
        saveData.ScoreDictionary.ShouldBeEmpty();
        saveData.TimeDictionary.ShouldBeEmpty();
        saveData.SpellDictionary.ShouldBeEmpty();
        saveData.TutorialCountDictionary.ShouldBeEmpty();
        saveData.ReachedStageDictionary.Count.ShouldBe(1);
#if NET9_0_OR_GREATER
        saveData.ReachedStageDictionary.ShouldContainKeyAndValue(Chara.Marisa, 4);
#else
        saveData.ReachedStageDictionary.ContainsKey(Chara.Marisa).ShouldBeTrue();
        saveData.ReachedStageDictionary[Chara.Marisa].ShouldBe(4);
#endif
        saveData.UseCountDictionary.ShouldBeEmpty();
        saveData.RetireCountDictionary.ShouldBeEmpty();
        saveData.ClearCountDictionary.ShouldBeEmpty();
        saveData.PerfectClearCountDictionary.ShouldBeEmpty();
        saveData.EndingCountDictionary.ShouldBeEmpty();
        saveData.BgmPlayCountDictionary.ShouldBeEmpty();
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

        _ = saveData.ShouldNotBeNull();
        saveData.ScoreDictionary.ShouldBeEmpty();
        saveData.TimeDictionary.ShouldBeEmpty();
        saveData.SpellDictionary.ShouldBeEmpty();
        saveData.TutorialCountDictionary.ShouldBeEmpty();
        saveData.ReachedStageDictionary.ShouldBeEmpty();
        saveData.UseCountDictionary.Count.ShouldBe(1);
#if NET9_0_OR_GREATER
        saveData.UseCountDictionary.ShouldContainKeyAndValue(Chara.Kanako, 5);
#else
        saveData.UseCountDictionary.ContainsKey(Chara.Kanako).ShouldBeTrue();
        saveData.UseCountDictionary[Chara.Kanako].ShouldBe(5);
#endif
        saveData.RetireCountDictionary.ShouldBeEmpty();
        saveData.ClearCountDictionary.ShouldBeEmpty();
        saveData.PerfectClearCountDictionary.ShouldBeEmpty();
        saveData.EndingCountDictionary.ShouldBeEmpty();
        saveData.BgmPlayCountDictionary.ShouldBeEmpty();
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

        _ = saveData.ShouldNotBeNull();
        saveData.ScoreDictionary.ShouldBeEmpty();
        saveData.TimeDictionary.ShouldBeEmpty();
        saveData.SpellDictionary.ShouldBeEmpty();
        saveData.TutorialCountDictionary.ShouldBeEmpty();
        saveData.ReachedStageDictionary.ShouldBeEmpty();
        saveData.UseCountDictionary.ShouldBeEmpty();
        saveData.RetireCountDictionary.Count.ShouldBe(1);
#if NET9_0_OR_GREATER
        saveData.RetireCountDictionary.ShouldContainKeyAndValue(Chara.Minamitsu, 6);
#else
        saveData.RetireCountDictionary.ContainsKey(Chara.Minamitsu).ShouldBeTrue();
        saveData.RetireCountDictionary[Chara.Minamitsu].ShouldBe(6);
#endif
        saveData.ClearCountDictionary.ShouldBeEmpty();
        saveData.PerfectClearCountDictionary.ShouldBeEmpty();
        saveData.EndingCountDictionary.ShouldBeEmpty();
        saveData.BgmPlayCountDictionary.ShouldBeEmpty();
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

        _ = saveData.ShouldNotBeNull();
        saveData.ScoreDictionary.ShouldBeEmpty();
        saveData.TimeDictionary.ShouldBeEmpty();
        saveData.SpellDictionary.ShouldBeEmpty();
        saveData.TutorialCountDictionary.ShouldBeEmpty();
        saveData.ReachedStageDictionary.ShouldBeEmpty();
        saveData.UseCountDictionary.ShouldBeEmpty();
        saveData.RetireCountDictionary.ShouldBeEmpty();
        saveData.ClearCountDictionary.Count.ShouldBe(1);
#if NET9_0_OR_GREATER
        saveData.ClearCountDictionary.ShouldContainKeyAndValue(Chara.JoonShion, 7);
#else
        saveData.ClearCountDictionary.ContainsKey(Chara.JoonShion).ShouldBeTrue();
        saveData.ClearCountDictionary[Chara.JoonShion].ShouldBe(7);
#endif
        saveData.PerfectClearCountDictionary.ShouldBeEmpty();
        saveData.EndingCountDictionary.ShouldBeEmpty();
        saveData.BgmPlayCountDictionary.ShouldBeEmpty();
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

        _ = saveData.ShouldNotBeNull();
        saveData.ScoreDictionary.ShouldBeEmpty();
        saveData.TimeDictionary.ShouldBeEmpty();
        saveData.SpellDictionary.ShouldBeEmpty();
        saveData.TutorialCountDictionary.ShouldBeEmpty();
        saveData.ReachedStageDictionary.ShouldBeEmpty();
        saveData.UseCountDictionary.ShouldBeEmpty();
        saveData.RetireCountDictionary.ShouldBeEmpty();
        saveData.ClearCountDictionary.ShouldBeEmpty();
        saveData.PerfectClearCountDictionary.Count.ShouldBe(1);
#if NET9_0_OR_GREATER
        saveData.PerfectClearCountDictionary.ShouldContainKeyAndValue(Chara.Flandre, 8);
#else
        saveData.PerfectClearCountDictionary.ContainsKey(Chara.Flandre).ShouldBeTrue();
        saveData.PerfectClearCountDictionary[Chara.Flandre].ShouldBe(8);
#endif
        saveData.EndingCountDictionary.ShouldBeEmpty();
        saveData.BgmPlayCountDictionary.ShouldBeEmpty();
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

        _ = saveData.ShouldNotBeNull();
        saveData.ScoreDictionary.ShouldBeEmpty();
        saveData.TimeDictionary.ShouldBeEmpty();
        saveData.SpellDictionary.ShouldBeEmpty();
        saveData.TutorialCountDictionary.ShouldBeEmpty();
        saveData.ReachedStageDictionary.ShouldBeEmpty();
        saveData.UseCountDictionary.ShouldBeEmpty();
        saveData.RetireCountDictionary.ShouldBeEmpty();
        saveData.ClearCountDictionary.ShouldBeEmpty();
        saveData.PerfectClearCountDictionary.ShouldBeEmpty();
        saveData.EndingCountDictionary.Count.ShouldBe(1);
#if NET9_0_OR_GREATER
        saveData.EndingCountDictionary.ShouldContainKeyAndValue(Chara.Reimu, 9);
#else
        saveData.EndingCountDictionary.ContainsKey(Chara.Reimu).ShouldBeTrue();
        saveData.EndingCountDictionary[Chara.Reimu].ShouldBe(9);
#endif
        saveData.BgmPlayCountDictionary.ShouldBeEmpty();
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

        _ = saveData.ShouldNotBeNull();
        saveData.ScoreDictionary.Count.ShouldBe(4);
#if NET9_0_OR_GREATER
        saveData.ScoreDictionary.ShouldContainKey((Level.Easy, Chara.Reimu));
        saveData.ScoreDictionary.ShouldContainKey((Level.Normal, Chara.Marisa));
        saveData.ScoreDictionary.ShouldContainKey((Level.Hard, Chara.Kanako));
        saveData.ScoreDictionary.ShouldContainKey((Level.Rush, Chara.Minamitsu));
#else
        saveData.ScoreDictionary.ContainsKey((Level.Easy, Chara.Reimu)).ShouldBeTrue();
        saveData.ScoreDictionary.ContainsKey((Level.Normal, Chara.Marisa)).ShouldBeTrue();
        saveData.ScoreDictionary.ContainsKey((Level.Hard, Chara.Kanako)).ShouldBeTrue();
        saveData.ScoreDictionary.ContainsKey((Level.Rush, Chara.Minamitsu)).ShouldBeTrue();
#endif
        saveData.ScoreDictionary[(Level.Easy, Chara.Reimu)].ElementAtOrDefault(1).ShouldBe(2);
        saveData.ScoreDictionary[(Level.Normal, Chara.Marisa)].ElementAtOrDefault(2).ShouldBe(13);
        saveData.ScoreDictionary[(Level.Hard, Chara.Kanako)].ElementAtOrDefault(3).ShouldBe(24);
        saveData.ScoreDictionary[(Level.Rush, Chara.Minamitsu)].ElementAtOrDefault(4).ShouldBe(35);
        saveData.TimeDictionary.ShouldBeEmpty();
        saveData.SpellDictionary.ShouldBeEmpty();
        saveData.TutorialCountDictionary.ShouldBeEmpty();
        saveData.ReachedStageDictionary.ShouldBeEmpty();
        saveData.UseCountDictionary.ShouldBeEmpty();
        saveData.RetireCountDictionary.ShouldBeEmpty();
        saveData.ClearCountDictionary.ShouldBeEmpty();
        saveData.PerfectClearCountDictionary.ShouldBeEmpty();
        saveData.EndingCountDictionary.ShouldBeEmpty();
        saveData.BgmPlayCountDictionary.ShouldBeEmpty();
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

        _ = saveData.ShouldNotBeNull();
        saveData.ScoreDictionary.ShouldBeEmpty();
        saveData.TimeDictionary.Count.ShouldBe(4);
#if NET9_0_OR_GREATER
        saveData.TimeDictionary.ShouldContainKey((Level.Easy, Chara.Reimu));
        saveData.TimeDictionary.ShouldContainKey((Level.Normal, Chara.Marisa));
        saveData.TimeDictionary.ShouldContainKey((Level.Hard, Chara.Kanako));
        saveData.TimeDictionary.ShouldContainKey((Level.Rush, Chara.Minamitsu));
#else
        saveData.TimeDictionary.ContainsKey((Level.Easy, Chara.Reimu)).ShouldBeTrue();
        saveData.TimeDictionary.ContainsKey((Level.Normal, Chara.Marisa)).ShouldBeTrue();
        saveData.TimeDictionary.ContainsKey((Level.Hard, Chara.Kanako)).ShouldBeTrue();
        saveData.TimeDictionary.ContainsKey((Level.Rush, Chara.Minamitsu)).ShouldBeTrue();
#endif
        saveData.TimeDictionary[(Level.Easy, Chara.Reimu)].ElementAtOrDefault(1).ShouldBe(2);
        saveData.TimeDictionary[(Level.Normal, Chara.Marisa)].ElementAtOrDefault(2).ShouldBe(13);
        saveData.TimeDictionary[(Level.Hard, Chara.Kanako)].ElementAtOrDefault(3).ShouldBe(24);
        saveData.TimeDictionary[(Level.Rush, Chara.Minamitsu)].ElementAtOrDefault(4).ShouldBe(35);
        saveData.SpellDictionary.ShouldBeEmpty();
        saveData.TutorialCountDictionary.ShouldBeEmpty();
        saveData.ReachedStageDictionary.ShouldBeEmpty();
        saveData.UseCountDictionary.ShouldBeEmpty();
        saveData.RetireCountDictionary.ShouldBeEmpty();
        saveData.ClearCountDictionary.ShouldBeEmpty();
        saveData.PerfectClearCountDictionary.ShouldBeEmpty();
        saveData.EndingCountDictionary.ShouldBeEmpty();
        saveData.BgmPlayCountDictionary.ShouldBeEmpty();
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

        _ = saveData.ShouldNotBeNull();
        saveData.ScoreDictionary.ShouldBeEmpty();
        saveData.TimeDictionary.ShouldBeEmpty();
        saveData.SpellDictionary.Count.ShouldBe(4);
#if NET9_0_OR_GREATER
        saveData.SpellDictionary.ShouldContainKey((Level.Easy, Chara.Reimu));
        saveData.SpellDictionary.ShouldContainKey((Level.Normal, Chara.Marisa));
        saveData.SpellDictionary.ShouldContainKey((Level.Hard, Chara.Kanako));
        saveData.SpellDictionary.ShouldContainKey((Level.Rush, Chara.Minamitsu));
#else
        saveData.SpellDictionary.ContainsKey((Level.Easy, Chara.Reimu)).ShouldBeTrue();
        saveData.SpellDictionary.ContainsKey((Level.Normal, Chara.Marisa)).ShouldBeTrue();
        saveData.SpellDictionary.ContainsKey((Level.Hard, Chara.Kanako)).ShouldBeTrue();
        saveData.SpellDictionary.ContainsKey((Level.Rush, Chara.Minamitsu)).ShouldBeTrue();
#endif
        saveData.SpellDictionary[(Level.Easy, Chara.Reimu)].ElementAtOrDefault(1).ShouldBe(2);
        saveData.SpellDictionary[(Level.Normal, Chara.Marisa)].ElementAtOrDefault(2).ShouldBe(13);
        saveData.SpellDictionary[(Level.Hard, Chara.Kanako)].ElementAtOrDefault(3).ShouldBe(24);
        saveData.SpellDictionary[(Level.Rush, Chara.Minamitsu)].ElementAtOrDefault(4).ShouldBe(35);
        saveData.TutorialCountDictionary.ShouldBeEmpty();
        saveData.ReachedStageDictionary.ShouldBeEmpty();
        saveData.UseCountDictionary.ShouldBeEmpty();
        saveData.RetireCountDictionary.ShouldBeEmpty();
        saveData.ClearCountDictionary.ShouldBeEmpty();
        saveData.PerfectClearCountDictionary.ShouldBeEmpty();
        saveData.EndingCountDictionary.ShouldBeEmpty();
        saveData.BgmPlayCountDictionary.ShouldBeEmpty();
    }
}
