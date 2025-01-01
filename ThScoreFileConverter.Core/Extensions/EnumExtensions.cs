//-----------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ThScoreFileConverter.Core.Models;

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
        return AttributeCache<T, EnumAltNameAttribute>.Cache.TryGetValue(enumValue, out var attr)
            ? attr.ShortName : string.Empty;
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
        return AttributeCache<T, EnumAltNameAttribute>.Cache.TryGetValue(enumValue, out var attr)
            ? attr.LongName : string.Empty;
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
        return AttributeCache<T, PatternAttribute>.Cache.TryGetValue(enumValue, out var attr)
            ? attr.Pattern : string.Empty;
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
        return AttributeCache<T, DisplayAttribute>.Cache.TryGetValue(enumValue, out var attr)
            ? attr.GetName() ?? enumValue.ToString() : enumValue.ToString();
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
        return AttributeCache<T, DisplayAttribute>.Cache.TryGetValue(enumValue, out var attr)
            ? attr.GetShortName() ?? enumValue.ToString() : enumValue.ToString();
    }

    /// <summary>
    /// Gets the name of the character represented as a given enumeration value.
    /// </summary>
    /// <typeparam name="T">The enumeration type.</typeparam>
    /// <param name="enumValue">An enumeration value.</param>
    /// <returns>The name of the character represented as <paramref name="enumValue"/>.</returns>
    public static string ToCharaName<T>(this T enumValue)
        where T : struct, Enum
    {
        return AttributeCache<T, CharacterAttribute>.Cache.TryGetValue(enumValue, out var attr)
            ? attr.GetLocalizedName() : string.Empty;
    }

    /// <summary>
    /// Gets the full name of the character represented as a given enumeration value.
    /// </summary>
    /// <typeparam name="T">The enumeration type.</typeparam>
    /// <param name="enumValue">An enumeration value.</param>
    /// <returns>The full name of the character represented as <paramref name="enumValue"/>.</returns>
    public static string ToCharaFullName<T>(this T enumValue)
        where T : struct, Enum
    {
        return AttributeCache<T, CharacterAttribute>.Cache.TryGetValue(enumValue, out var attr)
            ? attr.GetLocalizedFullName() : string.Empty;
    }

    /// <summary>
    /// Provides cache of attribute information.
    /// </summary>
    /// <typeparam name="TEnum">The enumeration type.</typeparam>
    /// <typeparam name="TAttribute">The attribute type for <typeparamref name="TEnum"/>.</typeparam>
    private static class AttributeCache<TEnum, TAttribute>
        where TEnum : struct, Enum
        where TAttribute : Attribute
    {
        /// <summary>
        /// Gets the cache of attribute information collected by reflection.
        /// </summary>
        public static IReadOnlyDictionary<TEnum, TAttribute> Cache { get; } = InitializeCache();

        /// <summary>
        /// Initializes the cache.
        /// </summary>
        /// <returns>The cache of attribute information.</returns>
        private static Dictionary<TEnum, TAttribute> InitializeCache()
        {
            var type = typeof(TEnum);
            Debug.Assert(type.IsEnum, $"{nameof(TEnum)} must be an enum type.");

            static bool IsAllowedMultiple(MemberInfo memberInfo)
            {
                return memberInfo.GetCustomAttribute<AttributeUsageAttribute>(false) is { } usage
                    && usage.AllowMultiple;
            }

            Debug.Assert(
                !IsAllowedMultiple(typeof(TAttribute)),
                $"{nameof(TAttribute)} must not be allowed multiple instances.");

            return type.GetFields()
                .Where(field => field.FieldType == type)
                .Select(static field =>
                    (enumValue: field.GetValue(null) is TEnum value ? value : default,
                        attr: field.GetCustomAttribute<TAttribute>(false)))
                .Where(static pair => pair.attr is not null)
                .ToDictionary(static pair => pair.enumValue, static pair => pair.attr!);
        }
    }
}
