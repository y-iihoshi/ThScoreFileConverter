//-----------------------------------------------------------------------
// <copyright file="Utils.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ThScoreFileConverter.Models
{
    /// <summary>
    /// Provides static methods for convenience.
    /// </summary>
    public static class Utils
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
        public static TEnum ParseEnum<TEnum>(string value)
            where TEnum : struct, Enum
            => ParseEnum<TEnum>(value, false);

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
        public static TEnum ParseEnum<TEnum>(string value, bool ignoreCase)
            where TEnum : struct, Enum
            => (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);

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
        public static TEnum ToEnum<TEnum>(object value)
            where TEnum : struct, Enum
        {
            var underlying = Convert.ChangeType(
                value, typeof(TEnum).GetEnumUnderlyingType(), CultureInfo.InvariantCulture);
            return Enum.IsDefined(typeof(TEnum), underlying) ? (TEnum)underlying : throw new InvalidCastException();
        }

        /// <summary>
        /// Gets the <c>IEnumerable{T}</c> instance to enumerate values of the <typeparamref name="TEnum"/>
        /// type.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type.</typeparam>
        /// <returns>
        /// The <c>IEnumerable{T}</c> instance to enumerate values of the <typeparamref name="TEnum"/> type.
        /// </returns>
        [CLSCompliant(false)]
        public static IEnumerable<TEnum> GetEnumerator<TEnum>()
            where TEnum : struct, Enum
            => Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

        /// <summary>
        /// Returns a string that represents the specified numeric value.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="number"/>.</typeparam>
        /// <param name="number">A numeric value.</param>
        /// <returns>A string that represents <paramref name="number"/>.</returns>
        public static string ToNumberString<T>(T number)
            where T : struct
            => ToNumberString(number, Settings.Instance.OutputNumberGroupSeparator.Value);

        /// <summary>
        /// Returns a string that represents the specified numeric value.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="number"/>.</typeparam>
        /// <param name="number">A numeric value.</param>
        /// <param name="outputSeparator">
        /// <c>true</c> if use a thousand separator character; otherwise, <c>false</c>.
        /// </param>
        /// <returns>A string that represents <paramref name="number"/>.</returns>
        public static string ToNumberString<T>(T number, bool outputSeparator)
            where T : struct
            => outputSeparator ? Format("{0:N0}", number) : number.ToString();

        /// <summary>
        /// Wraps the <c>string.Format()</c> method to specify an IFormatProvider instance.
        /// </summary>
        /// <param name="fmt">A composite format string.</param>
        /// <param name="args">An <c>Object</c> array containing zero or more objects to format.</param>
        /// <returns>
        /// A copy of <paramref name="fmt"/> in which the format items have been replaced by the string
        /// representation of the corresponding objects in <paramref name="args"/>.
        /// </returns>
        public static string Format(string fmt, params object[] args)
            => string.Format(CultureInfo.CurrentCulture, fmt, args);

        /// <summary>
        /// Makes a logical-and predicate by one or more predicates.
        /// </summary>
        /// <typeparam name="T">The type of the instance to evaluate.</typeparam>
        /// <param name="predicates">The predicates combined with logical-and operators.</param>
        /// <returns>A logical-and predicate.</returns>
        public static Func<T, bool> MakeAndPredicate<T>(params Func<T, bool>[] predicates)
            => arg => predicates.All(pred => pred(arg));

        /// <summary>
        /// Converts a one digit value from one-based to zero-based.
        /// </summary>
        /// <param name="input">A one digit value to convert.</param>
        /// <returns>A converted one digit value.</returns>
        public static int ToZeroBased(int input)
        {
            if ((input < 0) || (input > 9))
                throw new ArgumentOutOfRangeException(nameof(input));

            return (input + 9) % 10;
        }

        /// <summary>
        /// Converts a one digit value from zero-based to one-based.
        /// </summary>
        /// <param name="input">A one digit value to convert.</param>
        /// <returns>A converted one digit value.</returns>
        public static int ToOneBased(int input)
        {
            if ((input < 0) || (input > 9))
                throw new ArgumentOutOfRangeException(nameof(input));

            return (input + 1) % 10;
        }

        /// <summary>
        /// Returns the encoding associated with the specified code page identifier.
        /// </summary>
        /// <param name="codePage">The code page identifier of the preferred encoding.</param>
        /// <returns>The <c>Encoding</c> associated with <paramref name="codePage"/>.</returns>
        public static System.Text.Encoding GetEncoding(int codePage)
        {
            // To prevent BOM output for UTF-8
            return (codePage == 65001)
                ? new System.Text.UTF8Encoding(false) : System.Text.Encoding.GetEncoding(codePage);
        }
    }
}
