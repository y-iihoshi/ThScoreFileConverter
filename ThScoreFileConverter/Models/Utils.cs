//-----------------------------------------------------------------------
// <copyright file="Utils.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using ThScoreFileConverter.Properties;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Extensions;
using WPFLocalizeExtension.Providers;

namespace ThScoreFileConverter.Models
{
    /// <summary>
    /// Provides static methods for convenience.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Returns a string that represents the specified numeric value.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="number"/>.</typeparam>
        /// <param name="number">A numeric value.</param>
        /// <returns>A string that represents <paramref name="number"/>.</returns>
        public static string ToNumberString<T>(T number)
            where T : struct
        {
            return ToNumberString(number, Settings.Instance.OutputNumberGroupSeparator!.Value);
        }

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
        {
            return outputSeparator ? Format("{0:N0}", number) : (number.ToString() ?? string.Empty);
        }

        /// <summary>
        /// Wraps the <c>string.Format()</c> method to specify an IFormatProvider instance.
        /// </summary>
        /// <param name="fmt">A composite format string.</param>
        /// <param name="args">An <see cref="object"/> array containing zero or more objects to format.</param>
        /// <returns>
        /// A copy of <paramref name="fmt"/> in which the format items have been replaced by the string
        /// representation of the corresponding objects in <paramref name="args"/>.
        /// </returns>
        public static string Format(string fmt, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, fmt, args);
        }

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
        /// Gets a localized value.
        /// </summary>
        /// <typeparam name="T">The type of the returned value.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The resolved localized object.</returns>
        public static T GetLocalizedValues<T>(string key)
        {
            return LocalizeDictionary.Instance.DefaultProvider is ResxLocalizationProvider
                ? LocExtension.GetLocalizedValue<T>($"{nameof(ThScoreFileConverter)}:{nameof(Resources)}:{key}")
                : LocExtension.GetLocalizedValue<T>(key);
        }
    }
}
