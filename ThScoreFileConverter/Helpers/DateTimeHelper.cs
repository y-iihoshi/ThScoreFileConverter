//-----------------------------------------------------------------------
// <copyright file="DateTimeHelper.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Globalization;

namespace ThScoreFileConverter.Helpers;

/// <summary>
/// Provides helper functions related to date and time.
/// </summary>
public static class DateTimeHelper
{
    /// <summary>
    /// Gets the date and time format string used by <see cref="GetString(double?)"/>.
    /// </summary>
    public static string ValidFormat { get; } = "yyyy/MM/dd HH:mm:ss";

    /// <summary>
    /// Gets the format string indicating the invalid point in time.
    /// </summary>
    public static string InvalidFormat { get; } = "----/--/-- --:--:--";

    /// <summary>
    /// Gets a string representation of the specified Unix time.
    /// </summary>
    /// <param name="unixTime">The number of seconds that have elapsed since the Unix epoch.</param>
    /// <returns>
    /// A string representation of <paramref name="unixTime"/> formatted with <see cref="ValidFormat"/>;
    /// <see cref="InvalidFormat"/> if <paramref name="unixTime"/> is <see langword="null"/>.
    /// </returns>
    public static string GetString(double? unixTime)
    {
        return unixTime.HasValue
            ? DateTime.UnixEpoch.AddSeconds(unixTime.Value).ToLocalTime().ToString(ValidFormat, CultureInfo.CurrentCulture)
            : InvalidFormat;
    }
}
