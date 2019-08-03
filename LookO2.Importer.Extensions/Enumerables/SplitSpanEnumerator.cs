using System;
using System.Runtime.CompilerServices;

namespace LookO2.Importer.Core.Extensions
{
    public ref struct SplitSpanEnumerator<T> where T : IEquatable<T>
    {
        public SplitSpanEnumerator(ReadOnlySpan<T> span, T separator)
        {
            Span = span;
            Separator = separator;
            Current = default;

            if (Span.IsEmpty)
                TrailingEmptyItem = true;
        }

        ReadOnlySpan<T> Span { get; set; }
        T Separator { get; }
        int SeparatorLength => 1;

        ReadOnlySpan<T> TrailingEmptyItemSentinel => Unsafe.As<T[]>(nameof(TrailingEmptyItemSentinel)).AsSpan();

        bool TrailingEmptyItem
        {
            get => Span == TrailingEmptyItemSentinel;
            set => Span = value ? TrailingEmptyItemSentinel : default;
        }

        public bool MoveNext()
        {
            if (TrailingEmptyItem)
            {
                TrailingEmptyItem = false;
                Current = default;
                return true;
            }

            if (Span.IsEmpty)
            {
                Span = Current = default;
                return false;
            }

            int idx = Span.IndexOf(Separator);
            if (idx < 0)
            {
                Current = Span;
                Span = default;
            }
            else
            {
                Current = Span.Slice(0, idx);
                Span = Span.Slice(idx + SeparatorLength);
                if (Span.IsEmpty)
                    TrailingEmptyItem = true;
            }

            return true;
        }

        public ReadOnlySpan<T> Current { get; private set; }
    }
}
