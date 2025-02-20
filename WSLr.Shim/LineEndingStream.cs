using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSLr.Shim;

internal class LineEndingStream(Stream baseStream) : Stream
{
    public override void Flush()
    {
        throw new NotImplementedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        var baseCount = baseStream.Read(buffer, offset, count);
        for (var i = 0; i < baseCount - 1; i++)
        {
            if (buffer[i] != '\r' || buffer[i + 1] != '\n') continue;

            for (var j = i; j < baseCount - 1; j++)
            {
                buffer[j] = buffer[j + 1];
            }

            baseCount--;
        }

        return baseCount;
    }

    public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }

    public override bool CanRead => baseStream.CanRead;

    public override bool CanSeek => baseStream.CanSeek;

    public override bool CanWrite => baseStream.CanWrite;

    public override long Length { get; }

    public override long Position { get; set; }
}
