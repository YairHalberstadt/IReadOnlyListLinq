﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Linq2List.Tests.Unit
{
    public static class TestExtensions
    {
        public static IReadOnlyList<T> RunOnce<T>(this IReadOnlyList<T> source)
            => source == null ? null : new RunOnceList<T>(source);

        private class RunOnceList<T> : IReadOnlyList<T>
        {
            private readonly IReadOnlyList<T> _source;
            private readonly HashSet<int> _called = new HashSet<int>();

            private void AssertAll()
            {
                Assert.Empty(_called);
                _called.Add(-1);
            }

            private void AssertIndex(int index)
            {
                Assert.False(_called.Contains(-1));
                Assert.True(_called.Add(index));
            }

            public RunOnceList(IReadOnlyList<T> source)
            {
                _source = source;
            }

            public IEnumerator<T> GetEnumerator()
            {
                AssertAll();
                return _source.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public void Add(T item)
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            public bool Contains(T item)
            {
                AssertAll();
                return _source.Contains(item);
            }

            public bool Remove(T item)
            {
                throw new NotSupportedException();
            }

            public int Count => _source.Count;

            public bool IsReadOnly => true;

            public void Insert(int index, T item)
            {
                throw new NotSupportedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotSupportedException();
            }

            public T this[int index]
            {
                get
                {
                    AssertIndex(index);
                    return _source[index];
                }
                set { throw new NotSupportedException(); }
            }
        }
    }
}
