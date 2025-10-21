using System.Runtime.InteropServices;

namespace CommonShims;

public static class SpanExtensions
{
#if NETSTANDARD2_0
    public static unsafe string ToStringFast(this ReadOnlySpan<char> span)
    {
        if (span.Length == 0)
            return string.Empty;

        fixed (char* p = &MemoryMarshal.GetReference(span))
        {
            return new string(p, 0, span.Length);
        }
    }

#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToStringFast(this ReadOnlySpan<char> span) => new string(span);
#endif
}
