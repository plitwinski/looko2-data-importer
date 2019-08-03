using System;

namespace LookO2.Importer.Core.Extensions
{
    public ref struct SplitSpanEnumerable<T> where T : IEquatable<T>
    {
        public SplitSpanEnumerable(in ReadOnlySpan<T> span, T separator)
        {
            Span = span;
            Separator = separator;
        }

        ReadOnlySpan<T> Span { get; }
        T Separator { get; }

        public SplitSpanEnumerator<T> GetEnumerator() => new SplitSpanEnumerator<T>(Span, Separator);
    }
}
