using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Tests.Models.Th16;

[TestClass]
public class SpellCardTests
{
    public static IEnumerable<object[]> InvalidLevels => TestUtils.GetInvalidEnumerators<Level>();

    [TestMethod]
    public void SpellCardTest()
    {
        Th13.SpellCardTests.SpellCardTestHelper<Level>();
    }

    [TestMethod]
    public void ReadFromTest()
    {
        Th13.SpellCardTests.ReadFromTestHelper<Level>();
    }

    [TestMethod]
    public void ReadFromTestShortenedName()
    {
        Th13.SpellCardTests.ReadFromTestShortenedNameHelper<Level>();
    }

    [TestMethod]
    public void ReadFromTestExceededName()
    {
        Th13.SpellCardTests.ReadFromTestExceededNameHelper<Level>();
    }

    [DataTestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void ReadFromTestInvalidLevel(int level)
    {
        Th13.SpellCardTests.ReadFromTestInvalidLevelHelper<Level>(level);
    }
}
