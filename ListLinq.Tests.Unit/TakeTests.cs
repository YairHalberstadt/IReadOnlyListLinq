﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ListLinq.Tests.Unit
{
	public class TakeTests : ListLinqTests
	{
		[Fact]
		public void SameResultsRepeatCallsIntQueryIList()
		{
			var q = (from x in new[] { 9999, 0, 888, -1, 66, -777, 1, 2, -12345 }
					 where x > Int32.MinValue
					 select x).ToList();

			Assert.Equal(q.Take(9), q.Take(9));
		}

		[Fact]
		public void SameResultsRepeatCallsStringQueryIList()
		{
			var q = (from x in new[] { "!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", String.Empty }
					 where !String.IsNullOrEmpty(x)
					 select x).ToList();

			Assert.Equal(q.Take(7), q.Take(7));
		}

		[Fact]
		public void SourceEmptyCountPositive()
		{
			int[] source = { };
			Assert.Empty(source.Take(5));
		}

		[Fact]
		public void SourceNonEmptyCountNegative()
		{
			int[] source = { 2, 5, 9, 1 };
			Assert.Empty(source.Take(-5));
		}

		[Fact]
		public void SourceNonEmptyCountZero()
		{
			int[] source = { 2, 5, 9, 1 };
			Assert.Empty(source.Take( 0));
		}

		[Fact]
		public void SourceNonEmptyCountOne()
		{
			int[] source = { 2, 5, 9, 1 };
			int[] expected = { 2 };

			Assert.Equal(expected, source.Take(1));
		}

		[Fact]
		public void SourceNonEmptyTakeAllExactly()
		{
			int[] source = { 2, 5, 9, 1 };

			Assert.Equal(source, source.Take(source.Length));
		}

		[Fact]
		public void SourceNonEmptyTakeAllButOne()
		{
			int[] source = { 2, 5, 9, 1 };
			int[] expected = { 2, 5, 9 };

			Assert.Equal(expected, source.Take(3));
		}

		[Fact]
		public void RunOnce()
		{
			int[] source = { 2, 5, 9, 1 };
			int[] expected = { 2, 5, 9 };

			Assert.Equal(expected, source.RunOnce().Take(3));
		}

		[Fact]
		public void SourceNonEmptyTakeExcessive()
		{
			int?[] source = { 2, 5, null, 9, 1 };

			Assert.Equal(source, source.Take(source.Length + 1));
		}

		[Fact]
		public void ThrowsOnNullSource()
		{
			int[] source = null;
			AssertExtensions.Throws<ArgumentNullException>("source", () => source.Take(5));
		}

		[Fact]
		public void Count()
		{
			Assert.Equal(2, new[] { 1, 2, 3 }.Take(2).Count);
			Assert.Equal(0, new[] { 1, 2, 3 }.Take(0).Count);
		}

		[Fact]
		public void FollowWithTake()
		{
			var source = new[] { 5, 6, 7, 8 };
			var expected = new[] { 5, 6 };
			Assert.Equal(expected, source.Take(5).Take(3).Take(2).Take(40));
		}

		[Fact]
		public void FollowWithSkip()
		{
			var source = new[] { 1, 2, 3, 4, 5, 6 };
			var expected = new[] { 3, 4, 5 };
			Assert.Equal(expected, source.Take(5).Skip(2).Skip(-4));
		}

		[Fact]
		public void ElementAt()
		{
			var source = new[] { 1, 2, 3, 4, 5, 6 };
			var taken = source.Take(3);
			Assert.Equal(1, taken.ElementAt(0));
			Assert.Equal(1, taken[0]);
			Assert.Equal(3, taken.ElementAt(2));
			Assert.Equal(3, taken[2]);
			AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => taken.ElementAt(-1));
			AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => taken[-1]);
			AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => taken.ElementAt(3));
			AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => taken[3]);
		}

		[Fact]
		public void ElementAtOrDefault()
		{
			var source = new[] { 1, 2, 3, 4, 5, 6 };
			var taken = source.Take(3);
			Assert.Equal(1, taken.ElementAtOrDefault(0));
			Assert.Equal(3, taken.ElementAtOrDefault(2));
			Assert.Equal(0, taken.ElementAtOrDefault(-1));
			Assert.Equal(0, taken.ElementAtOrDefault(3));
		}

		[Fact]
		public void First()
		{
			var source = new[] { 1, 2, 3, 4, 5 };
			Assert.Equal(1, source.Take(1).First());
			Assert.Equal(1, source.Take(4).First());
			Assert.Equal(1, source.Take(40).First());
			Assert.Throws<InvalidOperationException>(() => source.Take(0).First());
			Assert.Throws<InvalidOperationException>(() => source.Skip(5).Take(10).First());
		}

		[Fact]
		public void FirstOrDefault()
		{
			var source = new[] { 1, 2, 3, 4, 5 };
			Assert.Equal(1, source.Take(1).FirstOrDefault());
			Assert.Equal(1, source.Take(4).FirstOrDefault());
			Assert.Equal(1, source.Take(40).FirstOrDefault());
			Assert.Equal(0, source.Take(0).FirstOrDefault());
			Assert.Equal(0, source.Skip(5).Take(10).FirstOrDefault());
		}

		[Fact]
		public void Last()
		{
			var source = new[] { 1, 2, 3, 4, 5 };
			Assert.Equal(1, source.Take(1).Last());
			Assert.Equal(5, source.Take(5).Last());
			Assert.Equal(5, source.Take(40).Last());
			Assert.Throws<InvalidOperationException>(() => source.Take(0).Last());
			Assert.Throws<InvalidOperationException>(() => Array.Empty<int>().Take(40).Last());
		}

		[Fact]
		public void LastOrDefault()
		{
			var source = new[] { 1, 2, 3, 4, 5 };
			Assert.Equal(1, source.Take(1).LastOrDefault());
			Assert.Equal(5, source.Take(5).LastOrDefault());
			Assert.Equal(5, source.Take(40).LastOrDefault());
			Assert.Equal(0, source.Take(0).LastOrDefault());
			Assert.Equal(0, Array.Empty<int>().Take(40).LastOrDefault());
		}

		[Fact]
		public void ToArray()
		{
			var source = new[] { 1, 2, 3, 4, 5 };
			Assert.Equal(new[] { 1, 2, 3, 4, 5 }, source.Take(5).ToArray());
			Assert.Equal(new[] { 1, 2, 3, 4, 5 }, source.Take(6).ToArray());
			Assert.Equal(new[] { 1, 2, 3, 4, 5 }, source.Take(40).ToArray());
			Assert.Equal(new[] { 1, 2, 3, 4 }, source.Take(4).ToArray());
			Assert.Equal(1, source.Take(1).ToArray().Single());
			Assert.Empty(source.Take(0).ToArray());
			Assert.Empty(source.Take(-10).ToArray());
		}

		[Fact]
		public void ToList()
		{
			var source = new[] { 1, 2, 3, 4, 5 };
			Assert.Equal(new[] { 1, 2, 3, 4, 5 }, source.Take(5).ToList());
			Assert.Equal(new[] { 1, 2, 3, 4, 5 }, source.Take(6).ToList());
			Assert.Equal(new[] { 1, 2, 3, 4, 5 }, source.Take(40).ToList());
			Assert.Equal(new[] { 1, 2, 3, 4 }, source.Take(4).ToList());
			Assert.Equal(1, source.Take(1).ToList().Single());
			Assert.Empty(source.Take( 0).ToList());
			Assert.Empty(source.Take( -10).ToList());
		}
		
		[Fact]
		public void TakeCanOnlyBeOneList()
		{
			var source = new[] { 2, 4, 6, 8, 10 };
			Assert.Equal(new[] { 2 }, source.Take( 1));
			Assert.Equal(new[] { 4 }, source.Skip(1).Take(1));
			Assert.Equal(new[] { 6 }, source.Take(3).Skip(2));
			Assert.Equal(new[] { 2 }, source.Take(3).Take(1));
		}

		[Fact]
		public void RepeatEnumerating()
		{
			var source = new[] { 1, 2, 3, 4, 5 };
			var taken = source.Take(3);
			Assert.Equal(taken, taken);
		}

		[Theory]
		[InlineData(1000)]
		[InlineData(1000000)]
		[InlineData(int.MaxValue)]
		public void LazySkipAllTakenForLargeNumbers(int largeNumber)
		{
			Assert.Empty(new InfiniteList<int>().Take(largeNumber).Skip(largeNumber));
			Assert.Empty(new InfiniteList<int>().Take(largeNumber).Skip(largeNumber).Skip(42));
			Assert.Empty(new InfiniteList<int>().Take(largeNumber).Skip(largeNumber / 2).Skip(largeNumber / 2 + 1));
		}

		[Theory]
		[InlineData(0, 0, 0)]
		[InlineData(1, 1, 1)]
		[InlineData(0, int.MaxValue, 100)]
		[InlineData(int.MaxValue, 0, 0)]
		[InlineData(0xffff, 1, 0)]
		[InlineData(1, 0xffff, 99)]
		[InlineData(int.MaxValue, int.MaxValue, 0)]
		[InlineData(1, int.MaxValue, 99)] // Regression test: The max index is precisely int.MaxValue.
		[InlineData(0, 100, 100)]
		[InlineData(10, 100, 90)]
		public void CountOfLazySkipTakeChain(int skip, int take, int expected)
		{
			var partition = Enumerable.Range(1, 100).ToList().Skip(skip).Take(take);
			Assert.Equal(expected, partition.Count());
			Assert.Equal(expected, partition.Select(i => i).Count());
			Assert.Equal(expected, partition.Select(i => i).ToArray().Length);
		}

		[Theory]
		[InlineData(new[] { 1, 2, 3, 4 }, 1, 3, 2, 4)]
		[InlineData(new[] { 1 }, 0, 1, 1, 1)]
		[InlineData(new[] { 1, 2, 3, 5, 8, 13 }, 1, int.MaxValue, 2, 13)] // Regression test: The max index is precisely int.MaxValue.
		[InlineData(new[] { 1, 2, 3, 5, 8, 13 }, 0, 2, 1, 2)]
		[InlineData(new[] { 1, 2, 3, 5, 8, 13 }, 500, 2, 0, 0)]
		[InlineData(new int[] { }, 10, 8, 0, 0)]
		public void FirstAndLastOfLazySkipTakeChain(IEnumerable<int> source, int skip, int take, int first, int last)
		{
			var partition = source.ToList().Skip(skip).Take(take);

			Assert.Equal(first, partition.FirstOrDefault());
			Assert.Equal(first, partition.ElementAtOrDefault(0));
			Assert.Equal(last, partition.LastOrDefault());
			Assert.Equal(last, partition.ElementAtOrDefault(partition.Count() - 1));
		}

		[Theory]
		[InlineData(new[] { 1, 2, 3, 4, 5 }, 1, 3, new[] { -1, 0, 1, 2 }, new[] { 0, 2, 3, 4 })]
		[InlineData(new[] { 0xfefe, 7000, 123 }, 0, 3, new[] { -1, 0, 1, 2 }, new[] { 0, 0xfefe, 7000, 123 })]
		[InlineData(new[] { 0xfefe }, 100, 100, new[] { -1, 0, 1, 2 }, new[] { 0, 0, 0, 0 })]
		[InlineData(new[] { 0xfefe, 123, 456, 7890, 5555, 55 }, 1, 10, new[] { -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, new[] { 0, 123, 456, 7890, 5555, 55, 0, 0, 0, 0, 0, 0, 0 })]
		public void ElementAtOfLazySkipTakeChain(IEnumerable<int> source, int skip, int take, int[] indices, int[] expectedValues)
		{
			var partition = source.ToList().Skip(skip).Take(take);

			Assert.Equal(indices.Length, expectedValues.Length);
			for (int i = 0; i < indices.Length; i++)
			{
				Assert.Equal(expectedValues[i], partition.ElementAtOrDefault(indices[i]));
			}
		}

		[Theory]
		[MemberData(nameof(IteratorTestsData))]
		public void RunIteratorTests(IReadOnlyList<int> source)
		{
			var takeCounts = new[] {int.MinValue, -1, 0, 1, int.MaxValue};
			foreach (var count in takeCounts)
			{
				var iterator = source.Take(count);
				new IteratorTests().RunTests(iterator);
			}
		}

		public static IEnumerable<object[]> IteratorTestsData()
		{
			yield return new object[] { Array.Empty<int>() };
			yield return new object[] { new int[1] };
			yield return new object[] { Enumerable.Range(1, 30).ToList() };
		}
	}
}