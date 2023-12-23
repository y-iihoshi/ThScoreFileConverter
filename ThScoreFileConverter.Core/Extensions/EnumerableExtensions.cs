﻿//-----------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Diagnostics;

namespace ThScoreFileConverter.Core.Extensions;

/// <summary>
/// Provides some extension methods for <see cref="IEnumerable{T}"/> types.
/// </summary>
public static class EnumerableExtensions
{
#if NETFRAMEWORK
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
        Guard.IsNotNull(source);

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
        Guard.IsNotNull(source);

        return source.Skip(source.Count() - count);
    }
#endif

#if !NET8_0_OR_GREATER
    /// <summary>
    /// Creates a <see cref="Dictionary{TKey, TValue}"/> from an <see cref="IEnumerable{T}"/> of
    /// <see cref="KeyValuePair{TKey, TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TValue">The type of the value of <paramref name="source"/>.</typeparam>
    /// <param name="source">
    /// An <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{TKey, TValue}"/> to create a
    /// <see cref="Dictionary{TKey, TValue}"/> from.
    /// </param>
    /// <returns>
    /// A <see cref="Dictionary{TKey, TValue}"/> that contains values of type <typeparamref name="TValue"/>.
    /// </returns>
    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>> source)
        where TKey : notnull
    {
        Guard.IsNotNull(source);

        return source.ToDictionary(static pair => pair.Key, static pair => pair.Value);
    }

    /// <summary>
    /// Creates a <see cref="Dictionary{TKey, TValue}"/> from an <see cref="IEnumerable{T}"/> of
    /// <typeparamref name="TKey"/> and <typeparamref name="TValue"/> pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of the key of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TValue">The type of the value of <paramref name="source"/>.</typeparam>
    /// <param name="source">
    /// An <see cref="IEnumerable{T}"/> of <typeparamref name="TKey"/> and <typeparamref name="TValue"/> pairs to
    /// create a <see cref="Dictionary{TKey, TValue}"/> from.
    /// </param>
    /// <returns>
    /// A <see cref="Dictionary{TKey, TValue}"/> that contains values of type <typeparamref name="TValue"/>.
    /// </returns>
    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
        this IEnumerable<(TKey Key, TValue Value)> source)
        where TKey : notnull
    {
        Guard.IsNotNull(source);

        return source.ToDictionary(static pair => pair.Key, static pair => pair.Value);
    }
#endif

    /// <summary>
    /// Creates the Cartesian product of two sequences.
    /// </summary>
    /// <typeparam name="T1">The type of elements of the first sequence.</typeparam>
    /// <typeparam name="T2">The type of elements of the second sequence.</typeparam>
    /// <param name="first">The first sequence.</param>
    /// <param name="second">The second sequence.</param>
    /// <returns>The Cartesian product of <paramref name="first"/> and <paramref name="second"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="first"/> or <paramref name="second"/> is <see langword="null"/>.
    /// </exception>
    public static IEnumerable<(T1 First, T2 Second)> Cartesian<T1, T2>(
        this IEnumerable<T1> first, IEnumerable<T2> second)
    {
        Guard.IsNotNull(first);
        Guard.IsNotNull(second);

        return first.SelectMany(element1 => second.Select(element2 => (element1, element2)));
    }
}
