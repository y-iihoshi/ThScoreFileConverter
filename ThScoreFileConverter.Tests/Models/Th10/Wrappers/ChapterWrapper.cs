using ThScoreFileConverter.Models.Th10;

namespace ThScoreFileConverter.Tests.Models.Th10.Wrappers;

/// <summary>
/// To test protected members of <see cref="Chapter"/>.
/// </summary>
internal sealed class ChapterWrapper : Chapter
{
    public ChapterWrapper()
        : base()
    {
    }

    public ChapterWrapper(Chapter chapter)
        : base(chapter)
    {
    }

    public ChapterWrapper(Chapter chapter, string expectedSignature, ushort expectedVersion, int expectedSize)
        : base(chapter, expectedSignature, expectedVersion, expectedSize)
    {
    }

    public new IReadOnlyCollection<byte> Data => base.Data;
}
