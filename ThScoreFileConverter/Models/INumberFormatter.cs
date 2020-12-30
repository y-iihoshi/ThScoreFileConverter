//-----------------------------------------------------------------------
// <copyright file="INumberFormatter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models
{
    /// <summary>
    /// Defines several methods for formatting numbers.
    /// </summary>
    public interface INumberFormatter
    {
        /// <summary>
        /// Converts the specified number to its equivalent string representation.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="number"/>.</typeparam>
        /// <param name="number">A value to be converted.</param>
        /// <returns>The string representation of <paramref name="number"/>.</returns>
        string FormatNumber<T>(T number)
            where T : struct;

        /// <summary>
        /// Converts a number to the string that represents a percentage.
        /// </summary>
        /// <param name="number">A value to be converted to a string. It should have been multiplied by 100.</param>
        /// <param name="precision">The number of fractional digits of the percentage.</param>
        /// <returns>The string representation of a percentage.</returns>
        string FormatPercent(double number, int precision);
    }
}
