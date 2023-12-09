using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th06;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverter.Tests.UnitTesting;
using IClearData = ThScoreFileConverter.Models.Th06.IClearData<
    ThScoreFileConverter.Core.Models.Th06.Chara, ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Tests.Models.Th06;

[TestClass]
public class ClearDataTests
{
    internal static IClearData MockClearData()
    {
        var pairs = EnumHelper<Level>.Enumerable.Select((level, index) => (level, index)).ToArray();
        var mock = Substitute.For<IClearData>();
        _ = mock.Signature.Returns("CLRD");
        _ = mock.Size1.Returns((short)0x18);
        _ = mock.Size2.Returns((short)0x18);
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
            (short)clearData.Chara);
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

    public static IEnumerable<object[]> InvalidCharacters => TestUtils.GetInvalidEnumerators<Level>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidCharacters))]
    public void ClearDataTestInvalidChara(int chara)
    {
        var mock = MockClearData();
        _ = mock.Chara.Returns((Chara)chara);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidCastException>(() => new ClearData(chapter));
    }
}
