using System.Buffers;

namespace CommonShims;

#if NETSTANDARD2_0
public static class BinaryWriterExtensions
{
    public static void Write(this BinaryWriter writer, ReadOnlySpan<byte> buffer)
    {
        if (writer.GetType() == typeof(BinaryWriter))
        {
            writer.BaseStream.Write(buffer);
        }
        else
        {
            var array = ArrayPool<byte>.Shared.Rent(buffer.Length);
            try
            {
                buffer.CopyTo(array);
                writer.Write(array, 0, buffer.Length);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(array);
            }
        }
    }
}
#endif
