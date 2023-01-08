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
        var type = typeof(TEnum);
        var underlying = Convert.ChangeType(value, type.GetEnumUnderlyingType(), CultureInfo.InvariantCulture);
        return Enum.IsDefined(type, underlying) ? (TEnum)underlying : throw new InvalidCastException();
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

    /// <summary>
    /// Returns a <see cref="bool"/> telling whether a given integral value, or its name as a string, exists in a specified enumeration.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
    /// <param name="value">The value or name of a constant in <typeparamref name="TEnum"/>.</param>
    /// <returns>
    /// <see langword="true"/> if a given integral value exists in a specified enumeration;
    /// <see langword="false"/>, otherwise.
    /// </returns>
    public static bool IsDefined<TEnum>(TEnum value)
        where TEnum : struct, Enum
    {
#if NET5_0_OR_GREATER
        return Enum.IsDefined(value);
#else
        return Enum.IsDefined(typeof(TEnum), value);
#endif
    }
}
