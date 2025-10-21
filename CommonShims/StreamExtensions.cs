﻿using System.Buffers;

namespace CommonShims;

#if NETSTANDARD2_0
public static class StreamExtensions
{
    public static int Read(this Stream stream, Span<byte> buffer)
    {
        var sharedBuffer = ArrayPool<byte>.Shared.Rent(buffer.Length);
        try
        {
            var numRead = stream.Read(sharedBuffer, 0, buffer.Length);
            if ((uint)numRead > (uint)buffer.Length)
                throw new IOException("IOStream is too long.");

            new ReadOnlySpan<byte>(sharedBuffer, 0, numRead).CopyTo(buffer);
            return numRead;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(sharedBuffer);
        }
    }

    public static void Write(this Stream stream, ReadOnlySpan<byte> buffer)
    {
        var sharedBuffer = ArrayPool<byte>.Shared.Rent(buffer.Length);
        try
        {
            buffer.CopyTo(sharedBuffer);
            stream.Write(sharedBuffer, 0, buffer.Length);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(sharedBuffer);
        }
    }
}
#endif
