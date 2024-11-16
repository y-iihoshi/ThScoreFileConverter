//-----------------------------------------------------------------------
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
