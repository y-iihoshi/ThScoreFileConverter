using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Core.Models.Th123;

namespace ThScoreFileConverter.Tests.Models.Th123;

[TestClass]
public class SpellCardResultTests
{
    [TestMethod]
    public void SpellCardResultTest()
    {
        Th105.SpellCardResultTests.SpellCardResultTestHelper<Chara>();
    }

    [TestMethod]
    public void ReadFromTest()
    {
        Th105.SpellCardResultTests.ReadFromTestHelper<Chara>();
    }

    [TestMethod]
    public void ReadFromTestShortened()
    {
        Th105.SpellCardResultTests.ReadFromTestShortenedHelper<Chara>();
    }

    [TestMethod]
    public void ReadFromTestExceeded()
    {
        Th105.SpellCardResultTests.ReadFromTestExceededHelper<Chara>();
    }
}
