//-----------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace ThScoreFileConverter.Extensions
{
    /// <summary>
    /// Provides some extension methods for <see cref="IEnumerable{T}"/> types.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Bypasses a specified number of elements from the end of a sequence and then returns the remaining elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to return elements from.</param>
        /// <param name="count">The number of elements to skip before returning the remaining elements.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that contains the elements that occur before the specified index in the
        /// input sequence.
        /// </returns>
        public static IEnumerable<TSource> SkipLast<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return source.Take(source.Count() - count);
        }

        /// <summary>
        /// Returns a specified number of contiguous elements from the end of a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to return elements from.</param>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that contains the specified number of elements from the end of the input
        /// sequence.
        /// </returns>
        public static IEnumerable<TSource> TakeLast<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return source.Skip(source.Count() - count);
        }
    }
}
