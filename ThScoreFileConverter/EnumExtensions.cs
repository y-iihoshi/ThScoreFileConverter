//-----------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="None">
//     (c) 2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.DocumentationRules",
    "SA1649:FileHeaderFileNameDocumentationMustMatchTypeName",
    Justification = "Reviewed.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.LayoutRules",
    "SA1503:CurlyBracketsMustNotBeOmitted",
    Justification = "Reviewed.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.MaintainabilityRules",
    "SA1402:FileMayOnlyContainASingleClass",
    Justification = "Reviewed.")]

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
            public static readonly Dictionary<TEnum, TAttribute> Cache;

            /// <summary>
            /// Initializes static members of the <see cref="AttributeCache{TEnum,TAttribute}"/> class.
            /// </summary>
            [SuppressMessage(
                "Microsoft.Performance",
                "CA1810:InitializeReferenceTypeStaticFieldsInline",
                Justification = "Reviewed.")]
            static AttributeCache()
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

                    Cache = lookup[typeof(TAttribute)]
                        .ToDictionary(a => a.enumValue, a => (TAttribute)a.attr);
                }
                else
                {
                    // Should not raise any exception from here.
                }
            }
        }
    }

    /// <summary>
    /// Provides alternative names of enumeration fields.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class EnumAltNameAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumAltNameAttribute"/> class.
        /// </summary>
        /// <param name="shortName">A short name of the enumeration field.</param>
        public EnumAltNameAttribute(string shortName)
        {
            this.ShortName = shortName;
            this.LongName = string.Empty;
        }

        /// <summary>
        /// Gets a short name of the enumeration field.
        /// </summary>
        public string ShortName { get; private set; }

        /// <summary>
        /// Gets or sets a long name of the enumeration field.
        /// </summary>
        public string LongName { get; set; }
    }
}
