using NSubstitute;
using ThScoreFileConverter.Core.Models.Th143;
using ThScoreFileConverter.Models.Th143;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th143;

internal static class ItemStatusExtensions
{
    internal static void ShouldBe(this IItemStatus actual, IItemStatus expected)
    {
        actual.Signature.ShouldBe(expected.Signature);
        actual.Version.ShouldBe(expected.Version);
        actual.Checksum.ShouldBe(expected.Checksum);
        actual.Size.ShouldBe(expected.Size);
        actual.Item.ShouldBe(expected.Item);
        actual.UseCount.ShouldBe(expected.UseCount);
        actual.ClearedCount.ShouldBe(expected.ClearedCount);
        actual.ClearedScenes.ShouldBe(expected.ClearedScenes);
        actual.ItemLevel.ShouldBe(expected.ItemLevel);
        actual.AvailableCount.ShouldBe(expected.AvailableCount);
        actual.FramesOrRanges.ShouldBe(expected.FramesOrRanges);
    }
}

[TestClass]
public class ItemStatusTests
{
    internal static IItemStatus MockItemStatus()
    {
        var mock = Substitute.For<IItemStatus>();
        _ = mock.Signature.Returns("TI");
        _ = mock.Version.Returns((ushort)1);
        _ = mock.Checksum.Returns(0u);
        _ = mock.Size.Returns(0x34);
        _ = mock.Item.Returns(ItemWithTotal.NoItem);
        _ = mock.UseCount.Returns(98);
        _ = mock.ClearedCount.Returns(76);
        _ = mock.ClearedScenes.Returns(54);
        _ = mock.ItemLevel.Returns(3);
        _ = mock.AvailableCount.Returns(2);
        _ = mock.FramesOrRanges.Returns(1);
        return mock;
    }

    internal static byte[] MakeByteArray(IItemStatus itemStatus)
    {
        return TestUtils.MakeByteArray(
            itemStatus.Signature.ToCharArray(),
            itemStatus.Version,
            itemStatus.Checksum,
            itemStatus.Size,
            (int)itemStatus.Item,
            itemStatus.UseCount,
            itemStatus.ClearedCount,
            itemStatus.ClearedScenes,
            itemStatus.ItemLevel,
            0,
            itemStatus.AvailableCount,
            itemStatus.FramesOrRanges,
            new int[2]);
    }

    [TestMethod]
    public void ItemStatusTestChapter()
    {
        var mock = MockItemStatus();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var itemStatus = new ItemStatus(chapter);

        itemStatus.ShouldBe(mock);
        itemStatus.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void ItemStatusTestInvalidSignature()
    {
        var mock = MockItemStatus();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new ItemStatus(chapter));
    }

    [TestMethod]
    public void ItemStatusTestInvalidVersion()
    {
        var mock = MockItemStatus();
        var version = mock.Version;
        _ = mock.Version.Returns(++version);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new ItemStatus(chapter));
    }

    [TestMethod]
    public void ItemStatusTestInvalidSize()
    {
        var mock = MockItemStatus();
        var size = mock.Size;
        _ = mock.Size.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new ItemStatus(chapter));
    }

    public static IEnumerable<object[]> InvalidItems => TestUtils.GetInvalidEnumerators<ItemWithTotal>();

    [TestMethod]
    [DynamicData(nameof(InvalidItems))]
    public void ItemStatusTestInvalidItems(int item)
    {
        var mock = MockItemStatus();
        _ = mock.Item.Returns((ItemWithTotal)item);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidCastException>(() => new ItemStatus(chapter));
    }

    [TestMethod]
    [DataRow("TI", (ushort)1, 0x34, true)]
    [DataRow("ti", (ushort)1, 0x34, false)]
    [DataRow("TI", (ushort)0, 0x34, false)]
    [DataRow("TI", (ushort)1, 0x35, false)]
    public void CanInitializeTest(string signature, ushort version, int size, bool expected)
    {
        var checksum = 0u;
        var data = new byte[size];

        var chapter = TestUtils.Create<Chapter>(
            TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

        ItemStatus.CanInitialize(chapter).ShouldBe(expected);
    }
}
