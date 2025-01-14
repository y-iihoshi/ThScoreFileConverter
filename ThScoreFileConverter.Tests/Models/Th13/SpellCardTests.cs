﻿using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th13;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th13;

[TestClass]
public class SpellCardTests
{
    internal static ISpellCard<TLevel> MockSpellCard<TLevel>()
        where TLevel : struct, Enum
    {
        var mock = Substitute.For<ISpellCard<TLevel>>();
        _ = mock.Name.Returns(TestUtils.MakeRandomArray(0x80));
        _ = mock.ClearCount.Returns(1);
        _ = mock.PracticeClearCount.Returns(2);
        _ = mock.TrialCount.Returns(3);
        _ = mock.PracticeTrialCount.Returns(4);
        _ = mock.Id.Returns(5);
        _ = mock.Level.Returns(TestUtils.Cast<TLevel>(1));
        _ = mock.PracticeScore.Returns(6789);
        return mock;
    }

    internal static byte[] MakeByteArray<TLevel>(ISpellCard<TLevel> spellCard)
        where TLevel : struct, Enum
    {
        return TestUtils.MakeByteArray(
            spellCard.Name,
            spellCard.ClearCount,
            spellCard.PracticeClearCount,
            spellCard.TrialCount,
            spellCard.PracticeTrialCount,
            spellCard.Id - 1,
            TestUtils.Cast<int>(spellCard.Level),
            spellCard.PracticeScore);
    }

    internal static void Validate<TLevel>(ISpellCard<TLevel> expected, ISpellCard<TLevel> actual)
        where TLevel : struct, Enum
    {
        CollectionAssert.That.AreEqual(expected.Name, actual.Name);
        Assert.AreEqual(expected.ClearCount, actual.ClearCount);
        Assert.AreEqual(expected.PracticeClearCount, actual.PracticeClearCount);
        Assert.AreEqual(expected.TrialCount, actual.TrialCount);
        Assert.AreEqual(expected.PracticeTrialCount, actual.PracticeTrialCount);
        Assert.AreEqual(expected.Id, actual.Id);
        Assert.AreEqual(expected.Level, actual.Level);
        Assert.AreEqual(expected.PracticeScore, actual.PracticeScore);
    }

    internal static void SpellCardTestHelper<TLevel>()
        where TLevel : struct, Enum
    {
        var mock = Substitute.For<ISpellCard<TLevel>>();
        var spellCard = new SpellCard<TLevel>();

        Validate(mock, spellCard);
        Assert.IsFalse(spellCard.HasTried);
    }

    internal static void ReadFromTestHelper<TLevel>()
        where TLevel : struct, Enum
    {
        var mock = MockSpellCard<TLevel>();

        var spellCard = TestUtils.Create<SpellCard<TLevel>>(MakeByteArray(mock));

        Validate(mock, spellCard);
        Assert.IsTrue(spellCard.HasTried);
    }

    internal static void ReadFromTestShortenedNameHelper<TLevel>()
        where TLevel : struct, Enum
    {
        var mock = MockSpellCard<TLevel>();
        var name = mock.Name;
        _ = mock.Name.Returns(name.SkipLast(1).ToArray());

        _ = Assert.ThrowsException<InvalidCastException>(
            () => TestUtils.Create<SpellCard<TLevel>>(MakeByteArray(mock)));
    }

    internal static void ReadFromTestExceededNameHelper<TLevel>()
        where TLevel : struct, Enum
    {
        var mock = MockSpellCard<TLevel>();
        var name = mock.Name;
        _ = mock.Name.Returns(name.Concat(TestUtils.MakeRandomArray(1)).ToArray());

        _ = Assert.ThrowsException<InvalidCastException>(
            () => TestUtils.Create<SpellCard<TLevel>>(MakeByteArray(mock)));
    }

    internal static void ReadFromTestInvalidLevelHelper<TLevel>(int level)
        where TLevel : struct, Enum
    {
        var mock = MockSpellCard<TLevel>();
        _ = mock.Level.Returns(TestUtils.Cast<TLevel>(level));

        _ = Assert.ThrowsException<InvalidCastException>(
            () => TestUtils.Create<SpellCard<TLevel>>(MakeByteArray(mock)));
    }

    public static IEnumerable<object[]> InvalidTh13LevelPractices => TestUtils.GetInvalidEnumerators<LevelPractice>();

    public static IEnumerable<object[]> InvalidLevels => TestUtils.GetInvalidEnumerators<Level>();

    #region Th13

    [TestMethod]
    public void Th13SpellCardTest()
    {
        SpellCardTestHelper<LevelPractice>();
    }

    [TestMethod]
    public void Th13ReadFromTest()
    {
        ReadFromTestHelper<LevelPractice>();
    }

    [TestMethod]
    public void Th13ReadFromTestShortenedName()
    {
        ReadFromTestShortenedNameHelper<LevelPractice>();
    }

    [TestMethod]
    public void Th13ReadFromTestExceededName()
    {
        ReadFromTestExceededNameHelper<LevelPractice>();
    }

    [DataTestMethod]
    [DynamicData(nameof(InvalidTh13LevelPractices))]
    public void Th13ReadFromTestInvalidLevel(int level)
    {
        ReadFromTestInvalidLevelHelper<LevelPractice>(level);
    }

    #endregion

    #region Th14

    [TestMethod]
    public void Th14SpellCardTest()
    {
        SpellCardTestHelper<Level>();
    }

    [TestMethod]
    public void Th14ReadFromTest()
    {
        ReadFromTestHelper<Level>();
    }

    [TestMethod]
    public void Th14ReadFromTestShortenedName()
    {
        ReadFromTestShortenedNameHelper<Level>();
    }

    [TestMethod]
    public void Th14ReadFromTestExceededName()
    {
        ReadFromTestExceededNameHelper<Level>();
    }

    [DataTestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void Th14ReadFromTestInvalidLevel(int level)
    {
        ReadFromTestInvalidLevelHelper<Level>(level);
    }

    #endregion

    #region Th16

    [TestMethod]
    public void Th16SpellCardTest()
    {
        SpellCardTestHelper<Level>();
    }

    [TestMethod]
    public void Th16ReadFromTest()
    {
        ReadFromTestHelper<Level>();
    }

    [TestMethod]
    public void Th16ReadFromTestShortenedName()
    {
        ReadFromTestShortenedNameHelper<Level>();
    }

    [TestMethod]
    public void Th16ReadFromTestExceededName()
    {
        ReadFromTestExceededNameHelper<Level>();
    }

    [DataTestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void Th16ReadFromTestInvalidLevel(int level)
    {
        ReadFromTestInvalidLevelHelper<Level>(level);
    }

    #endregion
}
