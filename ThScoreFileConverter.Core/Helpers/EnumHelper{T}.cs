//-----------------------------------------------------------------------
// <copyright file="EnumHelper{T}.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace ThScoreFileConverter.Core.Helpers;

/// <summary>
/// Provides helper functions for <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">An enum type.</typeparam>
public static class EnumHelper<T>
    where T : struct, Enum
{
    private static readonly Array Values = Enum.GetValues(typeof(T));

#pragma warning disable CA1000 // Do not declare static members on generic types
    /// <summary>
    /// Gets the <see cref="IEnumerable{T}"/> instance to enumerate values of the <typeparamref name="T"/> type.
    /// </summary>
    public static IEnumerable<T> Enumerable { get; } =
#if NET5_0_OR_GREATER
        Enum.GetValues<T>();
#else
        Values.Cast<T>();
#endif

    /// <summary>
    /// Gets the number of values of the <typeparamref name="T"/> type.
    /// </summary>
    public static int NumValues { get; } = Values.Length;
#pragma warning restore CA1000 // Do not declare static members on generic types
}
