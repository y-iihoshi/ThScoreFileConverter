using NSubstitute;
using ThScoreFileConverter.Core.Models.Th105;
using ThScoreFileConverter.Models.Th105;

namespace ThScoreFileConverter.Tests.Models.Th105;

internal static class SpellCardResultExtensions
{
    internal static void ShouldBe<TChara>(this ISpellCardResult<TChara> actual, ISpellCardResult<TChara> expected)
        where TChara : struct, Enum
    {
        actual.Enemy.ShouldBe(expected.Enemy);
        actual.Level.ShouldBe(expected.Level);
        actual.Id.ShouldBe(expected.Id);
        actual.TrialCount.ShouldBe(expected.TrialCount);
        actual.GotCount.ShouldBe(expected.GotCount);
        actual.Frames.ShouldBe(expected.Frames);
    }
}

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

    internal static void SpellCardResultTestHelper<TChara>()
        where TChara : struct, Enum
    {
        var mock = Substitute.For<ISpellCardResult<TChara>>();
        var spellCardResult = new SpellCardResult<TChara>();

        spellCardResult.ShouldBe(mock);
    }

    internal static void ReadFromTestHelper<TChara>()
        where TChara : struct, Enum
    {
        var mock = MockSpellCardResult<TChara>();
        var spellCardResult = TestUtils.Create<SpellCardResult<TChara>>(MakeByteArray(mock));

        spellCardResult.ShouldBe(mock);
    }

    internal static void ReadFromTestShortenedHelper<TChara>()
        where TChara : struct, Enum
    {
        var mock = MockSpellCardResult<TChara>();

        _ = Should.Throw<EndOfStreamException>(() => TestUtils.Create<SpellCardResult<TChara>>(MakeByteArray(mock)[..^1]));
    }

    internal static void ReadFromTestExceededHelper<TChara>()
        where TChara : struct, Enum
    {
        var mock = MockSpellCardResult<TChara>();
        var spellCardResult = TestUtils.Create<SpellCardResult<TChara>>([.. MakeByteArray(mock), 1]);

        spellCardResult.ShouldBe(mock);
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
