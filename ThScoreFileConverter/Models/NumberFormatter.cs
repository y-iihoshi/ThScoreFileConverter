﻿//-----------------------------------------------------------------------
// <copyright file="NumberFormatter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace ThScoreFileConverter.Models
{
    /// <summary>
    /// Provides several methods for formatting numbers.
    /// </summary>
#if !DEBUG
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Instantiated by the DI container.")]
#endif
    internal class NumberFormatter : INumberFormatter
    {
        private readonly ISettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberFormatter"/> class.
        /// </summary>
        /// <param name="settings">The settings of this application.</param>
        public NumberFormatter(ISettings settings)
        {
            this.settings = settings;
        }

        /// <inheritdoc/>
        public string FormatNumber<T>(T number)
            where T : struct
        {
            return (this.settings.OutputNumberGroupSeparator is bool output && output)
                ? Utils.Format("{0:N0}", number) : (number.ToString() ?? string.Empty);
        }

        /// <inheritdoc/>
        public string FormatPercent(double number, int precision)
        {
            if (precision is < 0 or > 99)
                throw new ArgumentOutOfRangeException(nameof(precision));

            return Utils.Format($"{{0:F{precision}}}%", number);
        }
    }
}
