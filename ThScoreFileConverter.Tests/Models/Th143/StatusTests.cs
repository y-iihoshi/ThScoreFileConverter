using NSubstitute;
using ThScoreFileConverter.Core.Models.Th143;
using ThScoreFileConverter.Models.Th143;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th143;

[TestClass]
public class StatusTests
{
    internal static IStatus MockStatus()
    {
        var mock = Substitute.For<IStatus>();
        _ = mock.Signature.Returns("ST");
        _ = mock.Version.Returns((ushort)1);
        _ = mock.Checksum.Returns(0u);
        _ = mock.Size.Returns(0x224);
        _ = mock.LastName.Returns(TestUtils.CP932Encoding.GetBytes("Player1     \0\0"));
        _ = mock.BgmFlags.Returns(TestUtils.MakeRandomArray(9));
        _ = mock.TotalPlayTime.Returns(12345678);
        _ = mock.LastMainItem.Returns(ItemWithTotal.Camera);
        _ = mock.LastSubItem.Returns(ItemWithTotal.Doll);
        _ = mock.NicknameFlags.Returns(
            Enumerable.Range(0, 71).Select(value => (byte)((value % 3 == 0) ? 0 : 1)).ToArray());
        return mock;
    }

    internal static byte[] MakeByteArray(IStatus status)
    {
        return TestUtils.MakeByteArray(
            status.Signature.ToCharArray(),
            status.Version,
            status.Checksum,
            status.Size,
            status.LastName,
            new byte[0x12],
            status.BgmFlags,
            new byte[0x17],
            status.TotalPlayTime,
            0,
            (int)status.LastMainItem,
            (int)status.LastSubItem,
            new byte[0x54],
            status.NicknameFlags,
            new byte[0x12D]);
    }

    internal static void Validate(IStatus expected, IStatus actual)
    {
        actual.Signature.ShouldBe(expected.Signature);
        actual.Version.ShouldBe(expected.Version);
        actual.Checksum.ShouldBe(expected.Checksum);
        actual.Size.ShouldBe(expected.Size);
        actual.LastName.ShouldBe(expected.LastName);
        actual.BgmFlags.ShouldBe(expected.BgmFlags);
        actual.TotalPlayTime.ShouldBe(expected.TotalPlayTime);
        actual.LastMainItem.ShouldBe(expected.LastMainItem);
        actual.LastSubItem.ShouldBe(expected.LastSubItem);
        actual.NicknameFlags.ShouldBe(expected.NicknameFlags);
    }

    [TestMethod]
    public void StatusTestChapter()
    {
        var mock = MockStatus();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var status = new Status(chapter);

        Validate(mock, status);
        status.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void StatusTestInvalidSignature()
    {
        var mock = MockStatus();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new Status(chapter));
    }

    [TestMethod]
    public void StatusTestInvalidVersion()
    {
        var mock = MockStatus();
        var version = mock.Version;
        _ = mock.Version.Returns(++version);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new Status(chapter));
    }

    [TestMethod]
    public void StatusTestInvalidSize()
    {
        var mock = MockStatus();
        var size = mock.Size;
        _ = mock.Size.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new Status(chapter));
    }

    [DataTestMethod]
    [DataRow("ST", (ushort)1, 0x224, true)]
    [DataRow("st", (ushort)1, 0x224, false)]
    [DataRow("ST", (ushort)0, 0x224, false)]
    [DataRow("ST", (ushort)1, 0x225, false)]
    public void CanInitializeTest(string signature, ushort version, int size, bool expected)
    {
        var checksum = 0u;
        var data = new byte[size];

        var chapter = TestUtils.Create<Chapter>(
            TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

        Status.CanInitialize(chapter).ShouldBe(expected);
    }

    public static IEnumerable<object[]> InvalidItems => TestUtils.GetInvalidEnumerators<ItemWithTotal>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidItems))]
    public void StatusTestInvalidLastMainItem(int item)
    {
        var mock = MockStatus();
        _ = mock.LastMainItem.Returns((ItemWithTotal)item);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidCastException>(() => new Status(chapter));
    }

    [DataTestMethod]
    [DynamicData(nameof(InvalidItems))]
    public void StatusTestInvalidLastSubItem(int item)
    {
        var mock = MockStatus();
        _ = mock.LastSubItem.Returns((ItemWithTotal)item);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidCastException>(() => new Status(chapter));
    }
}
