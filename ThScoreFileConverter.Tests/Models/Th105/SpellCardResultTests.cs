using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Models.Th105;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th105;

[TestClass]
public class SpellCardResultTests
{
    internal static Mock<ISpellCardResult<TChara>> MockSpellCardResult<TChara>()
        where TChara : struct, Enum
    {
        var mock = new Mock<ISpellCardResult<TChara>>();
        _ = mock.SetupGet(m => m.Enemy).Returns(TestUtils.Cast<TChara>(1));
        _ = mock.SetupGet(m => m.Level).Returns(Level.Hard);
        _ = mock.SetupGet(m => m.Id).Returns(3);
        _ = mock.SetupGet(m => m.TrialCount).Returns(67);
        _ = mock.SetupGet(m => m.GotCount).Returns(45);
        _ = mock.SetupGet(m => m.Frames).Returns(8901);
        return mock;
    }

    internal static byte[] MakeByteArray<TChara>(ISpellCardResult<TChara> properties)
        where TChara : struct, Enum
    {
        return TestUtils.MakeByteArray(
            TestUtils.Cast<int>(properties.Enemy),
            TestUtils.Cast<int>(properties.Level),
            properties.Id,
            properties.TrialCount,
            properties.GotCount,
            properties.Frames);
    }

    internal static void Validate<TChara>(ISpellCardResult<TChara> expected, ISpellCardResult<TChara> actual)
        where TChara : struct, Enum
    {
        Assert.AreEqual(expected.Enemy, actual.Enemy);
        Assert.AreEqual(expected.Level, actual.Level);
        Assert.AreEqual(expected.Id, actual.Id);
        Assert.AreEqual(expected.TrialCount, actual.TrialCount);
        Assert.AreEqual(expected.GotCount, actual.GotCount);
        Assert.AreEqual(expected.Frames, actual.Frames);
    }

    internal static void SpellCardResultTestHelper<TChara>()
        where TChara : struct, Enum
    {
        var mock = new Mock<ISpellCardResult<TChara>>();
        var spellCardResult = new SpellCardResult<TChara>();

        Validate(mock.Object, spellCardResult);
    }

    internal static void ReadFromTestHelper<TChara>()
        where TChara : struct, Enum
    {
        var mock = MockSpellCardResult<TChara>();
        var spellCardResult = TestUtils.Create<SpellCardResult<TChara>>(MakeByteArray(mock.Object));

        Validate(mock.Object, spellCardResult);
    }

    internal static void ReadFromTestShortenedHelper<TChara>()
        where TChara : struct, Enum
    {
        var mock = MockSpellCardResult<TChara>();
        var array = MakeByteArray(mock.Object).SkipLast(1).ToArray();

        _ = Assert.ThrowsException<EndOfStreamException>(() => TestUtils.Create<SpellCardResult<TChara>>(array));
    }

    internal static void ReadFromTestExceededHelper<TChara>()
        where TChara : struct, Enum
    {
        var mock = MockSpellCardResult<TChara>();
        var array = MakeByteArray(mock.Object).Concat(new byte[1] { 1 }).ToArray();

        var spellCardResult = TestUtils.Create<SpellCardResult<TChara>>(array);

        Validate(mock.Object, spellCardResult);
    }

    [TestMethod]
    public void SpellCardResultTest()
    {
        SpellCardResultTestHelper<Chara>();
    }

    [TestMethod]
    public void ReadFromTest()
    {
        ReadFromTestHelper<Chara>();
    }

    [TestMethod]
    public void ReadFromTestShortened()
    {
        ReadFromTestShortenedHelper<Chara>();
    }

    [TestMethod]
    public void ReadFromTestExceeded()
    {
        ReadFromTestExceededHelper<Chara>();
    }
}
