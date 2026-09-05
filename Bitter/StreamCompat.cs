using System;
using System.Buffers;
using System.IO;

namespace Bitter
{
    /// <summary>
    /// Stream.Read(Span&lt;byte&gt;)/Write(ReadOnlySpan&lt;byte&gt;) only exist on the Stream base class from
    /// netstandard2.1/.NET Core 2.1 onward. On netstandard2.0 (even with the System.Memory package, which
    /// only backports the Span/Memory types themselves) we fall back to a pooled array.
    /// </summary>
    internal static class StreamCompat
    {
        public static void ReadSpan(Stream stream, Span<byte> buffer)
        {
            if (buffer.Length == 0)
            {
                return;
            }
#if NETSTANDARD2_0
            byte[] rented = ArrayPool<byte>.Shared.Rent(buffer.Length);
            try
            {
                stream.Read(rented, 0, buffer.Length);
                rented.AsSpan(0, buffer.Length).CopyTo(buffer);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(rented);
            }
#else
            stream.Read(buffer);
#endif
        }

        public static void WriteSpan(Stream stream, ReadOnlySpan<byte> buffer)
        {
            if (buffer.Length == 0)
            {
                return;
            }
#if NETSTANDARD2_0
            byte[] rented = ArrayPool<byte>.Shared.Rent(buffer.Length);
            try
            {
                buffer.CopyTo(rented);
                stream.Write(rented, 0, buffer.Length);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(rented);
            }
#else
            stream.Write(buffer);
#endif
        }
    }
}
