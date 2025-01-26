using ThScoreFileConverter.Models.Th06;

namespace ThScoreFileConverter.Tests.Models.Th06.Wrappers;

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

    public ChapterWrapper(Chapter chapter, string expectedSignature, short expectedSize)
        : base(chapter, expectedSignature, expectedSize)
    {
    }

    public new IReadOnlyCollection<byte> Data => base.Data;
}
