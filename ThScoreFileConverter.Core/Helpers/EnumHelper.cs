//-----------------------------------------------------------------------
// <copyright file="EnumHelper.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using ThScoreFileConverter.Core.Extensions;

namespace ThScoreFileConverter.Core.Helpers;

/// <summary>
/// Provides helper functions for enum.
/// </summary>
public static class EnumHelper
{
    /// <summary>
    /// Converts the string representation of the name or numeric value of one or more enumerated
    /// constants to an equivalent enumerated instance.
    /// </summary>
    /// <typeparam name="TEnum">The enumeration type.</typeparam>
    /// <param name="value">A string containing the name or value to convert.</param>
    /// <returns>
    /// An instance of <typeparamref name="TEnum"/> whose value is represented by
    /// <paramref name="value"/>.
    /// </returns>
    [CLSCompliant(false)]
    public static TEnum Parse<TEnum>(string value)
        where TEnum : struct, Enum
    {
        return Parse<TEnum>(value, false);
    }

    /// <summary>
    /// Converts the string representation of the name or numeric value of one or more enumerated
    /// constants to an equivalent enumerated instance.
    /// </summary>
    /// <typeparam name="TEnum">The enumeration type.</typeparam>
    /// <param name="value">A string containing the name or value to convert.</param>
    /// <param name="ignoreCase"><c>true</c> if ignore case; <c>false</c> to regard case.</param>
    /// <returns>
    /// An instance of <typeparamref name="TEnum"/> whose value is represented by
    /// <paramref name="value"/>.
    /// </returns>
    [CLSCompliant(false)]
    public static TEnum Parse<TEnum>(string value, bool ignoreCase)
        where TEnum : struct, Enum
    {
        return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
    }

    /// <summary>
    /// Converts a given integral value to an equivalent enumerated instance.
    /// </summary>
    /// <typeparam name="TEnum">The enumeration type.</typeparam>
    /// <param name="value">An integral value to convert.</param>
    /// <returns>
    /// An instance of <typeparamref name="TEnum"/> whose value is represented by
    /// <paramref name="value"/>.
    /// </returns>
    /// <exception cref="InvalidCastException">No enumerator equal to <paramref name="value"/> exists.</exception>
    [CLSCompliant(false)]
    public static TEnum To<TEnum>(object value)
        where TEnum : struct, Enum
    {
        var underlying = Convert.ChangeType(
            value, typeof(TEnum).GetEnumUnderlyingType(), CultureInfo.InvariantCulture);
        return Enum.IsDefined(typeof(TEnum), underlying) ? (TEnum)underlying : throw new InvalidCastException();
    }

    /// <summary>
    /// Creates the Cartesian product of two enums.
    /// </summary>
    /// <typeparam name="T1">The enum type of the first sequence.</typeparam>
    /// <typeparam name="T2">The enum type of the second sequence.</typeparam>
    /// <returns>The Cartesian product of <typeparamref name="T1"/> and <typeparamref name="T2"/>.</returns>
    public static IEnumerable<(T1 First, T2 Second)> Cartesian<T1, T2>()
        where T1 : struct, Enum
        where T2 : struct, Enum
    {
        return EnumHelper<T1>.Enumerable.Cartesian(EnumHelper<T2>.Enumerable);
    }
}
