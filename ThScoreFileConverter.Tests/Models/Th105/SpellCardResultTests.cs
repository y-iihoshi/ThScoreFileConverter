using NSubstitute;
using ThScoreFileConverter.Core.Models.Th105;
using ThScoreFileConverter.Models.Th105;

namespace ThScoreFileConverter.Tests.Models.Th105;

[TestClass]
public class SpellCardResultTests
{
    internal static ISpellCardResult<TChara> MockSpellCardResult<TChara>(TChara enemy, Level level, int id, int trialCount, int gotCount, uint frames)
        where TChara : struct, Enum
    {
        var mock = Substitute.For<ISpellCardResult<TChara>>();
        _ = mock.Enemy.Returns(enemy);
        _ = mock.Level.Returns(level);
        _ = mock.Id.Returns(id);
        _ = mock.TrialCount.Returns(trialCount);
        _ = mock.GotCount.Returns(gotCount);
        _ = mock.Frames.Returns(frames);
        return mock;
    }

    internal static ISpellCardResult<TChara> MockSpellCardResult<TChara>()
        where TChara : struct, Enum
    {
        return MockSpellCardResult(TestUtils.Cast<TChara>(1), Level.Hard, 3, 67, 45, 8901u);
    }

    internal static byte[] MakeByteArray<TChara>(ISpellCardResult<TChara> properties)
        where TChara : struct, Enum
    {
        return TestUtils.MakeByteArray(
            TestUtils.Cast<int>(properties.Enemy),
            (int)properties.Level,
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
        var mock = Substitute.For<ISpellCardResult<TChara>>();
        var spellCardResult = new SpellCardResult<TChara>();

        Validate(mock, spellCardResult);
    }

    internal static void ReadFromTestHelper<TChara>()
        where TChara : struct, Enum
    {
        var mock = MockSpellCardResult<TChara>();
        var spellCardResult = TestUtils.Create<SpellCardResult<TChara>>(MakeByteArray(mock));

        Validate(mock, spellCardResult);
    }

    internal static void ReadFromTestShortenedHelper<TChara>()
        where TChara : struct, Enum
    {
        var mock = MockSpellCardResult<TChara>();
        var array = MakeByteArray(mock).SkipLast(1).ToArray();

        _ = Assert.ThrowsException<EndOfStreamException>(() => TestUtils.Create<SpellCardResult<TChara>>(array));
    }

    internal static void ReadFromTestExceededHelper<TChara>()
        where TChara : struct, Enum
    {
        var mock = MockSpellCardResult<TChara>();
        var array = MakeByteArray(mock).Concat(new byte[1] { 1 }).ToArray();

        var spellCardResult = TestUtils.Create<SpellCardResult<TChara>>(array);

        Validate(mock, spellCardResult);
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
