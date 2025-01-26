using ThScoreFileConverter.Core.Helpers;
using Protagonist = ThScoreFileConverter.Core.Tests.Extensions.Protagonist;
using UnnamedCharacter = ThScoreFileConverter.Core.Tests.Extensions.UnnamedCharacter;

namespace ThScoreFileConverter.Core.Tests.Helpers;

[TestClass]
public class EnumHelperTests
{
    [TestMethod]
    public void ToEnumTest()
    {
        EnumHelper.To<DayOfWeek>(0).ShouldBe(DayOfWeek.Sunday);
    }

    [TestMethod]
    public void ToEnumTestInvalidValue()
    {
        _ = Should.Throw<InvalidCastException>(() => EnumHelper.To<DayOfWeek>(7));
    }

    [TestMethod]
    public void EnumerableTest()
    {
        var i = (int)DayOfWeek.Sunday;
        foreach (var value in EnumHelper<DayOfWeek>.Enumerable)
            value.ShouldBe((DayOfWeek)i++);
        i.ShouldBe(7);
    }

    [TestMethod]
    public void NumValuesTest()
    {
        EnumHelper<DayOfWeek>.NumValues.ShouldBe(7);
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

        EnumHelper.Cartesian<Protagonist, UnnamedCharacter>().ShouldBe(expected);
    }

    [TestMethod]
    public void IsDefinedTest()
    {
        EnumHelper.IsDefined(DayOfWeek.Sunday).ShouldBeTrue();
        EnumHelper.IsDefined((DayOfWeek)7).ShouldBeFalse();
    }
}
