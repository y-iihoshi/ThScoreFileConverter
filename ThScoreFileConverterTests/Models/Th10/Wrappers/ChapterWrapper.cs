using System;
using System.Collections.Generic;
using ThScoreFileConverter.Models.Th10;

namespace ThScoreFileConverterTests.Models.Th10.Wrappers
{
    /// <summary>
    /// To test protected members of <see cref="Chapter"/>.
    /// </summary>
    internal sealed class ChapterWrapper : Chapter
    {
        [Obsolete]
        public static ChapterWrapper Create(byte[] array)
            => new ChapterWrapper(TestUtils.Create<Chapter>(array));

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

        [Obsolete]
        public Chapter Target => this as Chapter;

        public new IReadOnlyCollection<byte> Data => base.Data;
    }
}
