﻿//-----------------------------------------------------------------------
// <copyright file="EnumHelper.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

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
    /// Returns a boolean telling whether a given integral value exists in the <typeparamref name="TEnum"/> type.
    /// </summary>
    /// <typeparam name="TEnum">The enumeration type.</typeparam>
    /// <param name="value">The value of a constant in <typeparamref name="TEnum"/>.</param>
    /// <returns>
    /// <see langword="true"/> if a given integral value exists in <typeparamref name="TEnum"/>;
    /// <see langword="false"/> otherwise.
    /// </returns>
    /// <remarks>
    /// To avoid circular references, do not call it from the following attributes
    /// (use <see cref="Enum.IsDefined{TEnum}(TEnum)"/> instead):
    /// <see cref="Models.PatternAttribute"/>, <see cref="Models.EnumDisplayAttribute"/>,
    /// <see cref="Models.CharacterAttribute"/>, <see cref="Models.ShotTypeAttribute{T}"/>.
    /// </remarks>
    public static bool IsDefined<TEnum>(TEnum value)
        where TEnum : struct, Enum
    {
        return EnumHelper<TEnum>.Members.ContainsKey(value);
    }
}
