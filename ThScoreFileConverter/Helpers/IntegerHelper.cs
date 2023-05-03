//-----------------------------------------------------------------------
// <copyright file="IntegerHelper.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using CommunityToolkit.Diagnostics;

namespace ThScoreFileConverter.Helpers;

/// <summary>
/// Provides helper functions for integers.
/// </summary>
public static class IntegerHelper
{
    /// <summary>
    /// Converts a one digit value from one-based to zero-based.
    /// </summary>
    /// <param name="input">A one digit value to convert.</param>
    /// <returns>A converted one digit value.</returns>
    public static int ToZeroBased(int input)
    {
        Guard.IsInRange(input, 0, 10);

        return (input + 9) % 10;
    }

    /// <summary>
    /// Converts a one digit value from zero-based to one-based.
    /// </summary>
    /// <param name="input">A one digit value to convert.</param>
    /// <returns>A converted one digit value.</returns>
    public static int ToOneBased(int input)
    {
        Guard.IsInRange(input, 0, 10);

        return (input + 1) % 10;
    }

    /// <summary>
    /// Converts the string representation of a number to its 32-bit signed integer equivalent.
    /// </summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <returns>A 32-bit signed integer equivalent to the number contained in <paramref name="s"/>.</returns>
    public static int Parse(string s)
    {
        return int.Parse(s, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets the number of decimal digits.
    /// </summary>
    /// <param name="value">A decimal value.</param>
    /// <returns>The number of digits.</returns>
    public static int GetNumDigits(int value)
    {
        return value == 0 ? 1 : (int)Math.Floor(Math.Log10(Math.Abs(value)) + 1);
    }
}
