using System;

namespace LookO2.Importer.Core.Extensions
{
    public static class SpanExtensions
    {
        public static SplitSpanEnumerable<T> Split<T>(this in ReadOnlySpan<T> span, T separator)
            where T : IEquatable<T> => new SplitSpanEnumerable<T>(span, separator);
    }
}
