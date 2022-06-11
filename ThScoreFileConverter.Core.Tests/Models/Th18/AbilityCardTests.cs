using System;
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

        Assert.AreEqual(expectedId, card.Id);
        Assert.AreEqual(expectedName, card.Name);
        Assert.AreEqual(expectedType, card.Type);
    }

    [TestMethod]
    public void AbilityCardTestNegativeId()
    {
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new AbilityCard(-1, string.Empty, AbilityCardType.Unknown));
    }

    [TestMethod]
    public void AbilityCardTestNullName()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(
            () => new AbilityCard(0, null!, AbilityCardType.Unknown));
    }
}
