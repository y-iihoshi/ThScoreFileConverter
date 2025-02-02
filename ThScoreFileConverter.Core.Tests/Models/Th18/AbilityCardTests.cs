using ThScoreFileConverter.Core.Models.Th18;

namespace ThScoreFileConverter.Core.Tests.Models.Th18;

[TestClass]
public class AbilityCardTests
{
    [TestMethod]
    public void AbilityCardTest()
    {
        var expectedId = 0;
        var expectedName = string.Empty;
        var expectedType = AbilityCardType.Unknown;

        var card = new AbilityCard(expectedId, expectedName, expectedType);

        card.Id.ShouldBe(expectedId);
        card.Name.ShouldBe(expectedName);
        card.Type.ShouldBe(expectedType);
    }

    [TestMethod]
    public void AbilityCardTestNegativeId()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(
            () => new AbilityCard(-1, string.Empty, AbilityCardType.Unknown));
    }

    [TestMethod]
    public void AbilityCardTestNullName()
    {
        _ = Should.Throw<ArgumentNullException>(
            () => new AbilityCard(0, null!, AbilityCardType.Unknown));
    }
}
