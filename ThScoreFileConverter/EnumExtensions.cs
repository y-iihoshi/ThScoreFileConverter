//-----------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="None">
//     (c) 2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

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
        [CLSCompliant(false)]
        public static string ToShortName<T>(this T enumValue)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            EnumAltNameAttribute attr;
            if (AttributeCache<T, EnumAltNameAttribute>.Cache.TryGetValue(enumValue, out attr))
                return attr.ShortName;
            else
                return string.Empty;
        }

        /// <summary>
        /// Gets a long name of the specified enumeration value.
        /// </summary>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <param name="enumValue">An enumeration value.</param>
        /// <returns>A long name of <paramref name="enumValue"/>.</returns>
        [CLSCompliant(false)]
        public static string ToLongName<T>(this T enumValue)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            EnumAltNameAttribute attr;
            if (AttributeCache<T, EnumAltNameAttribute>.Cache.TryGetValue(enumValue, out attr))
                return attr.LongName;
            else
                return string.Empty;
        }

        /// <summary>
        /// Provides cache of attribute information.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type.</typeparam>
        /// <typeparam name="TAttribute">The attribute type for <typeparamref name="TEnum"/>.</typeparam>
        private static class AttributeCache<TEnum, TAttribute>
            where TEnum : struct, IComparable, IFormattable, IConvertible
            where TAttribute : Attribute
        {
            /// <summary>
            /// Caches attribute information collected by reflection.
            /// </summary>
            public static readonly Dictionary<TEnum, TAttribute> Cache = InitializeCache();

            /// <summary>
            /// Initializes the cache.
            /// </summary>
            /// <returns>The cache of attribute information.</returns>
            private static Dictionary<TEnum, TAttribute> InitializeCache()
            {
                var type = typeof(TEnum);
                if (type.IsEnum)
                {
                    var lookup = type.GetFields()
                        .Where(field => field.FieldType == type)
                        .SelectMany(
                            field => field.GetCustomAttributes(false),
                            (field, attr) => new { enumValue = (TEnum)field.GetValue(null), attr })
                        .ToLookup(a => a.attr.GetType());

                    return lookup[typeof(TAttribute)]
                        .ToDictionary(a => a.enumValue, a => a.attr as TAttribute);
                }
                else
                {
                    return new Dictionary<TEnum, TAttribute>();
                }
            }
        }
    }
}
