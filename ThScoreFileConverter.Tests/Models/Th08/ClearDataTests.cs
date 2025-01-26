using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverter.Tests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class ClearDataTests
{
    internal static IClearData MockClearData()
    {
        var pairs = EnumHelper<Level>.Enumerable.Select((level, index) => (level, index)).ToArray();
        var mock = Substitute.For<IClearData>();
        _ = mock.Signature.Returns("CLRD");
        _ = mock.Size1.Returns((short)0x24);
        _ = mock.Size2.Returns((short)0x24);
        _ = mock.StoryFlags.Returns(pairs.ToDictionary(pair => pair.level, pair => (PlayableStages)pair.index));
        _ = mock.PracticeFlags.Returns(pairs.ToDictionary(pair => pair.level, pair => (PlayableStages)(10 - pair.index)));
        _ = mock.Chara.Returns(CharaWithTotal.MarisaAlice);
        return mock;
    }

    internal static byte[] MakeByteArray(IClearData clearData)
    {
        return TestUtils.MakeByteArray(
            clearData.Signature.ToCharArray(),
            clearData.Size1,
            clearData.Size2,
            0u,
            clearData.StoryFlags.Values.Select(value => (ushort)value),
            clearData.PracticeFlags.Values.Select(value => (ushort)value),
            (byte)0,
            (byte)clearData.Chara,
            (ushort)0);
    }

    internal static void Validate(IClearData expected, IClearData actual)
    {
        Assert.AreEqual(expected.Signature, actual.Signature);
        Assert.AreEqual(expected.Size1, actual.Size1);
        Assert.AreEqual(expected.Size2, actual.Size2);
        Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
        CollectionAssert.That.AreEqual(expected.StoryFlags.Values, actual.StoryFlags.Values);
        CollectionAssert.That.AreEqual(expected.PracticeFlags.Values, actual.PracticeFlags.Values);
        Assert.AreEqual(expected.Chara, actual.Chara);
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
        _ = Assert.ThrowsException<InvalidDataException>(() => new ClearData(chapter));
    }

    [TestMethod]
    public void ClearDataTestInvalidSize1()
    {
        var mock = MockClearData();
        var size = mock.Size1;
        _ = mock.Size1.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new ClearData(chapter));
    }

    public static IEnumerable<object[]> InvalidCharacters => TestUtils.GetInvalidEnumerators<CharaWithTotal>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidCharacters))]
    public void ClearDataTestInvalidChara(int chara)
    {
        var mock = MockClearData();
        _ = mock.Chara.Returns((CharaWithTotal)chara);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidCastException>(() => new ClearData(chapter));
    }
}
