﻿//-----------------------------------------------------------------------
// <copyright file="EnumHelper{T}.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ThScoreFileConverter.Core.Helpers;

/// <summary>
/// Provides helper functions for <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">An enum type.</typeparam>
public static class EnumHelper<T>
    where T : struct, Enum
{
    private static readonly T[] Values = Enum.GetValues<T>();

#pragma warning disable CA1000 // Do not declare static members on generic types
    /// <summary>
    /// Gets the <see cref="IEnumerable{T}"/> instance to enumerate values of the <typeparamref name="T"/> type.
    /// </summary>
    public static IEnumerable<T> Enumerable { get; } = Values;

    /// <summary>
    /// Gets the number of values of the <typeparamref name="T"/> type.
    /// </summary>
    public static int NumValues { get; } = Values.Length;
#pragma warning restore CA1000 // Do not declare static members on generic types
}
