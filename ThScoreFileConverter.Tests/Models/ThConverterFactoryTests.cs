﻿using ThScoreFileConverter.Models;

namespace ThScoreFileConverter.Tests.Models;

[TestClass]
public class ThConverterFactoryTests
{
    [TestMethod]
    public void CanCreateTest()
    {
        Assert.IsTrue(ThConverterFactory.CanCreate("TH06"));
    }

    [TestMethod]
    public void CanCreateTestEmptyKey()
    {
        Assert.IsFalse(ThConverterFactory.CanCreate(string.Empty));
    }

    [TestMethod]
    public void CanCreateTestInvalidKey()
    {
        Assert.IsFalse(ThConverterFactory.CanCreate("invalidKey"));
    }

    [TestMethod]
    public void CreateTest()
    {
        var converter = ThConverterFactory.Create("TH06");
        var converterType = converter?.GetType();

        Assert.AreEqual(typeof(ThScoreFileConverter.Models.Th06.Converter), converterType);
    }

    [TestMethod]
    public void CreateTestEmptyKey()
    {
        var converter = ThConverterFactory.Create(string.Empty);

        Assert.IsNull(converter);
    }

    [TestMethod]
    public void CreateTestInvalidKey()
    {
        var converter = ThConverterFactory.Create("invalidKey");

        Assert.IsNull(converter);
    }
}
