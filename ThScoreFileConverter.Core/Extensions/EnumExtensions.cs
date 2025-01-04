﻿//-----------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Helpers;

namespace ThScoreFileConverter.Core.Extensions;

/// <summary>
/// Provides some extension methods for enumeration types.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets a short name of the specified enumeration value.
    /// </summary>
    /// <typeparam name="T">The enumeration type.</typeparam>
    /// <param name="enumValue">An enumeration value.</param>
    /// <returns>A short name of <paramref name="enumValue"/>.</returns>
    public static string ToShortName<T>(this T enumValue)
        where T : struct, Enum
    {
        return enumValue.ToMember().ShortName;
    }

    /// <summary>
    /// Gets a long name of the specified enumeration value.
    /// </summary>
    /// <typeparam name="T">The enumeration type.</typeparam>
    /// <param name="enumValue">An enumeration value.</param>
    /// <returns>A long name of <paramref name="enumValue"/>.</returns>
    public static string ToLongName<T>(this T enumValue)
        where T : struct, Enum
    {
        return enumValue.ToMember().LongName;
    }

    /// <summary>
    /// Gets a pattern string of the specified enumeration value.
    /// </summary>
    /// <typeparam name="T">The enumeration type.</typeparam>
    /// <param name="enumValue">An enumeration value.</param>
    /// <returns>A pattern string of <paramref name="enumValue"/>.</returns>
    public static string ToPattern<T>(this T enumValue)
        where T : struct, Enum
    {
        return enumValue.ToMember().Pattern;
    }

    /// <summary>
    /// Gets a display name of the specified enumeration value.
    /// </summary>
    /// <typeparam name="T">The enumeration type.</typeparam>
    /// <param name="enumValue">An enumeration value.</param>
    /// <returns>
    /// The localized display name of <paramref name="enumValue"/>, if <paramref name="enumValue"/> has been set
    /// <see cref="DisplayAttribute"/> properly; otherwise, the non-localized name of <paramref name="enumValue"/>.
    /// </returns>
    public static string ToDisplayName<T>(this T enumValue)
        where T : struct, Enum
    {
        var member = enumValue.ToMember();
        return member.DisplayAttribute?.GetName() ?? member.Name;
    }

    /// <summary>
    /// Gets a shortened display name of the specified enumeration value.
    /// </summary>
    /// <typeparam name="T">The enumeration type.</typeparam>
    /// <param name="enumValue">An enumeration value.</param>
    /// <returns>
    /// The localized shortened display name of <paramref name="enumValue"/>, if <paramref name="enumValue"/> has been
    /// set <see cref="DisplayAttribute"/> properly; otherwise, the non-localized name of <paramref name="enumValue"/>.
    /// </returns>
    public static string ToDisplayShortName<T>(this T enumValue)
        where T : struct, Enum
    {
        var member = enumValue.ToMember();
        return member.DisplayAttribute?.GetShortName() ?? member.Name;
    }

    /// <summary>
    /// Gets the name of the character represented as a given enumeration value.
    /// </summary>
    /// <typeparam name="T">The enumeration type.</typeparam>
    /// <param name="enumValue">An enumeration value.</param>
    /// <param name="index">The index specifying one name from multiple character names.</param>
    /// <returns>The name of the character represented as <paramref name="enumValue"/>.</returns>
    public static string ToCharaName<T>(this T enumValue, int index = 0)
        where T : struct, Enum
    {
        return enumValue.ToMember().CharacterAttributes.TryGetValue(index, out var attribute)
            ? attribute.GetLocalizedName() : string.Empty;
    }

    /// <summary>
    /// Gets the full name of the character represented as a given enumeration value.
    /// </summary>
    /// <typeparam name="T">The enumeration type.</typeparam>
    /// <param name="enumValue">An enumeration value.</param>
    /// <param name="index">The index specifying one name from multiple character names.</param>
    /// <returns>The full name of the character represented as <paramref name="enumValue"/>.</returns>
    public static string ToCharaFullName<T>(this T enumValue, int index = 0)
        where T : struct, Enum
    {
        return enumValue.ToMember().CharacterAttributes.TryGetValue(index, out var attribute)
            ? attribute.GetLocalizedFullName() : string.Empty;
    }

    /// <summary>
    /// Gets the <see cref="EnumHelper{T}.Member"/> instance corresponding to the specified enumeration value.
    /// </summary>
    /// <typeparam name="T">The enumeration type.</typeparam>
    /// <param name="enumValue">An enumeration value.</param>
    /// <returns>The <see cref="EnumHelper{T}.Member"/> instance corresponding to <paramref name="enumValue"/>.</returns>
    internal static EnumHelper<T>.Member ToMember<T>(this T enumValue)
        where T : struct, Enum
    {
        Guard.IsTrue(Enum.IsDefined(enumValue));
        return EnumHelper<T>.Members[enumValue];
    }
}
