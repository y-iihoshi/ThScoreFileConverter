using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ThScoreFileConverterTests.Models
{
    public class UnreadableMemoryStream : MemoryStream
    {
        public override bool CanRead => false;

        public override int Read(byte[] buffer, int offset, int count)
            => throw new NotSupportedException();

        public override int ReadByte()
            => throw new NotSupportedException();

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            => throw new NotSupportedException();

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            => throw new NotSupportedException();
    }
}
