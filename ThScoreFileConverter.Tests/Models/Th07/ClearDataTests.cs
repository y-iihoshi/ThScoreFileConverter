using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th07;
using ThScoreFileConverter.Models.Th07;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;
using IClearData = ThScoreFileConverter.Models.Th06.IClearData<
    ThScoreFileConverter.Core.Models.Th07.Chara, ThScoreFileConverter.Core.Models.Th07.Level>;

namespace ThScoreFileConverter.Tests.Models.Th07;

[TestClass]
public class ClearDataTests
{
    internal static IClearData MockClearData()
    {
        var pairs = EnumHelper<Level>.Enumerable.Select((level, index) => (level, index)).ToArray();
        var mock = Substitute.For<IClearData>();
        _ = mock.Signature.Returns("CLRD");
        _ = mock.Size1.Returns((short)0x1C);
        _ = mock.Size2.Returns((short)0x1C);
        _ = mock.StoryFlags.Returns(pairs.ToDictionary(pair => pair.level, pair => (byte)pair.index));
        _ = mock.PracticeFlags.Returns(pairs.ToDictionary(pair => pair.level, pair => (byte)(10 - pair.index)));
        _ = mock.Chara.Returns(Chara.ReimuB);
        return mock;
    }

    internal static byte[] MakeByteArray(IClearData clearData)
    {
        return TestUtils.MakeByteArray(
            clearData.Signature.ToCharArray(),
            clearData.Size1,
            clearData.Size2,
            0u,
            clearData.StoryFlags.Values,
            clearData.PracticeFlags.Values,
            (int)clearData.Chara);
    }

    internal static void Validate(IClearData expected, IClearData actual)
    {
        actual.Signature.ShouldBe(expected.Signature);
        actual.Size1.ShouldBe(expected.Size1);
        actual.Size2.ShouldBe(expected.Size2);
        actual.FirstByteOfData.ShouldBe(expected.FirstByteOfData);
        actual.StoryFlags.Values.ShouldBe(expected.StoryFlags.Values);
        actual.PracticeFlags.Values.ShouldBe(expected.PracticeFlags.Values);
        actual.Chara.ShouldBe(expected.Chara);
    }

    [TestMethod]
    public void ClearDataTestChapter()
    {
        var mock = MockClearData();
        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var clearData = new ClearData(chapter);

        Validate(mock, clearData);
    }

    [TestMethod]
    public void ClearDataTestInvalidSignature()
    {
        var mock = MockClearData();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new ClearData(chapter));
    }

    [TestMethod]
    public void ClearDataTestInvalidSize1()
    {
        var mock = MockClearData();
        var size = mock.Size1;
        _ = mock.Size1.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new ClearData(chapter));
    }

    public static IEnumerable<object[]> InvalidCharacters => TestUtils.GetInvalidEnumerators<Chara>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidCharacters))]
    public void ClearDataTestInvalidChara(int chara)
    {
        var mock = MockClearData();
        _ = mock.Chara.Returns((Chara)chara);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidCastException>(() => new ClearData(chapter));
    }
}
