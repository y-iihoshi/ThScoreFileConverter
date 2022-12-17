using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;

namespace ThScoreFileConverter.Tests.UnitTesting;

public class UnreadableMemoryStream : MemoryStream
{
    public override bool CanRead => false;

    public override int Read(byte[] buffer, int offset, int count)
    {
        return ThrowHelper.ThrowNotSupportedException<int>();
    }

    public override int ReadByte()
    {
        return ThrowHelper.ThrowNotSupportedException<int>();
    }

    public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
    {
        return ThrowHelper.ThrowNotSupportedException<IAsyncResult>();
    }

    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        return ThrowHelper.ThrowNotSupportedException<Task<int>>();
    }
}
