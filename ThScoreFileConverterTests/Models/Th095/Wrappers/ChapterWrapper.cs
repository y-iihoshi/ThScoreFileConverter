using System.Collections.Generic;
using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverterTests.Models.Th095.Wrappers
{
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
}
