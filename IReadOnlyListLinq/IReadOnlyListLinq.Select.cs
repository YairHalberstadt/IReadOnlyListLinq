﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace IReadOnlyListLinq
{
    public static partial class IReadOnlyListLinq
    {
        public static IReadOnlyList<TResult> Select<TSource, TResult>(this IReadOnlyList<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            return new SelectIterator<TSource, TResult>(source, selector);
        }

        private sealed class SelectIterator<TSource, TResult> : Iterator<TResult>
        {
            private readonly IReadOnlyList<TSource> _source;

            private readonly Func<TSource, TResult> _selector;
            
            public SelectIterator(IReadOnlyList<TSource> source, Func<TSource, TResult> selector)
            {
                Debug.Assert(source != null && selector != null, "parameters to SelectListIterator must not be null");
                _source = source;
                _selector = selector;
            }

            public sealed override TResult this[int index] => _selector(_source[index]);

            public sealed override int Count => _source.Count;

            public override Iterator<TResult> Clone() =>
                new SelectIterator<TSource, TResult>(_source, _selector);

            public override bool MoveNext()
            {
                if (++_index >= Count)
                {
                    _current = default;
                    return false;
                }
                _current = _selector(_source[_index]);
                return true;
            }
        }

        public static IReadOnlyList<TResult> Select<TSource, TResult>(this IReadOnlyList<TSource> source, Func<TSource, int, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            return new SelectIndexedIterator<TSource, TResult>(source, selector);
        }

        private sealed class SelectIndexedIterator<TSource, TResult> : Iterator<TResult>
        {
            private readonly IReadOnlyList<TSource> _source;

            private readonly Func<TSource, int, TResult> _selector;

            public SelectIndexedIterator(IReadOnlyList<TSource> source, Func<TSource, int, TResult> selector)
            {
                Debug.Assert(source != null && selector != null, "parameters to SelectListIterator must not be null");
                _source = source;
                _selector = selector;
            }

            public sealed override TResult this[int index] => _selector(_source[index], index);

            public sealed override int Count => _source.Count;

            public override Iterator<TResult> Clone() =>
                new SelectIndexedIterator<TSource, TResult>(_source, _selector);

            public override bool MoveNext()
            {
                if (++_index >= Count)
                {
                    _current = default;
                    return false;
                }
                _current = _selector(_source[_index], _index);
                return true;
            }
        }
    }
}
