//-----------------------------------------------------------------------
// <copyright file="NumberFormatter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models;

/// <summary>
/// Provides several methods for formatting numbers.
/// </summary>
/// <param name="settings">The settings of this application.</param>
#if !DEBUG
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Instantiated by the DI container.")]
#endif
internal sealed class NumberFormatter(ISettings settings) : INumberFormatter
{
    private readonly ISettings settings = settings;

    /// <inheritdoc/>
    public string FormatNumber<T>(T number)
        where T : struct
    {
        return (this.settings.OutputNumberGroupSeparator is bool output && output)
            ? StringHelper.Create($"{number:N0}") : (number.ToString() ?? string.Empty);
    }

    /// <inheritdoc/>
    public string FormatPercent(double number, int precision)
    {
        Guard.IsInRange(precision, 0, 100);

        return StringHelper.Format(StringHelper.Create($"{{0:F{precision}}}%"), number);
    }
}
