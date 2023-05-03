using System;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using Protagonist = ThScoreFileConverter.Core.Tests.Extensions.Protagonist;
using UnnamedCharacter = ThScoreFileConverter.Core.Tests.Extensions.UnnamedCharacter;

namespace ThScoreFileConverter.Core.Tests.Helpers;

[TestClass]
public class EnumHelperTests
{
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
