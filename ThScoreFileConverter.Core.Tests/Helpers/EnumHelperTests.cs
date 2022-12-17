using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using Protagonist = ThScoreFileConverter.Core.Tests.Extensions.Protagonist;
using UnnamedCharacter = ThScoreFileConverter.Core.Tests.Extensions.UnnamedCharacter;

namespace ThScoreFileConverter.Core.Tests.Helpers;

[TestClass]
public class EnumHelperTests
{
    [TestMethod]
    public void ParseEnumTestValidName()
    {
        Assert.AreEqual(DayOfWeek.Sunday, EnumHelper.Parse<DayOfWeek>("Sunday"));
    }

    [TestMethod]
    public void ParseEnumTestInvalidName()
    {
        _ = Assert.ThrowsException<ArgumentException>(() => EnumHelper.Parse<DayOfWeek>("Sun"));
    }

    [TestMethod]
    public void ParseEnumTestEmpty()
    {
        _ = Assert.ThrowsException<ArgumentException>(() => EnumHelper.Parse<DayOfWeek>(string.Empty));
    }

    [TestMethod]
    public void ParseEnumTestNull()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => EnumHelper.Parse<DayOfWeek>(null!));
    }

    [TestMethod]
    public void ParseEnumTestCaseSensitiveValidName()
    {
        Assert.AreEqual(DayOfWeek.Sunday, EnumHelper.Parse<DayOfWeek>("Sunday", false));
    }

    [TestMethod]
    public void ParseEnumTestCaseSensitiveInvalidName()
    {
        _ = Assert.ThrowsException<ArgumentException>(() => EnumHelper.Parse<DayOfWeek>("sunday", false));
    }

    [TestMethod]
    public void ParseEnumTestCaseInsensitiveValidName()
    {
        Assert.AreEqual(DayOfWeek.Sunday, EnumHelper.Parse<DayOfWeek>("Sunday", true));
        Assert.AreEqual(DayOfWeek.Sunday, EnumHelper.Parse<DayOfWeek>("sunday", true));
    }

    [TestMethod]
    public void ToEnumTest()
    {
        var value = 0;
        Assert.AreEqual(DayOfWeek.Sunday, EnumHelper.To<DayOfWeek>(value));
    }

    [TestMethod]
    public void ToEnumTestInvalidValue()
    {
        var value = 7;
        _ = Assert.ThrowsException<InvalidCastException>(() => EnumHelper.To<DayOfWeek>(value));
    }

    [TestMethod]
    public void EnumerableTest()
    {
        var i = (int)DayOfWeek.Sunday;
        foreach (var value in EnumHelper<DayOfWeek>.Enumerable)
            Assert.AreEqual((DayOfWeek)i++, value);
        Assert.AreEqual(7, i);
    }

    [TestMethod]
    public void NumValuesTest()
    {
        Assert.AreEqual(7, EnumHelper<DayOfWeek>.NumValues);
    }

    [TestMethod]
    public void CartesianTest()
    {
        var expected = new[]
        {
            (Protagonist.Reimu, UnnamedCharacter.大妖精),
            (Protagonist.Reimu, UnnamedCharacter.小悪魔),
            (Protagonist.Reimu, UnnamedCharacter.名無しの本読み妖怪),
            (Protagonist.Marisa, UnnamedCharacter.大妖精),
            (Protagonist.Marisa, UnnamedCharacter.小悪魔),
            (Protagonist.Marisa, UnnamedCharacter.名無しの本読み妖怪),
        };

        CollectionAssert.That.AreEqual(expected, EnumHelper.Cartesian<Protagonist, UnnamedCharacter>());
    }

    [TestMethod]
    public void IsDefinedTest()
    {
        Assert.IsTrue(EnumHelper.IsDefined(DayOfWeek.Sunday));
        Assert.IsFalse(EnumHelper.IsDefined((DayOfWeek)7));
    }
}
