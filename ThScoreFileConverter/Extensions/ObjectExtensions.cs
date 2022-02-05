//-----------------------------------------------------------------------
// <copyright file="ObjectExtensions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace ThScoreFileConverter.Extensions
{
    /// <summary>
    /// Provides some extension methods for <see cref="object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns a non-null string that represents the specified object.
        /// </summary>
        /// <param name="obj">The object to get its string representation.</param>
        /// <param name="nonNullStr">The non-null string to be returned when <paramref name="obj"/> is <c>null</c>.</param>
        /// <returns>A non-null string that represents <paramref name="obj"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="nonNullStr"/> is <c>null</c>.</exception>
        public static string ToNonNullString(this object obj, string nonNullStr = "(null)")
        {
            if (nonNullStr is null)
            {
                throw new ArgumentNullException(nameof(nonNullStr));
            }

            return obj?.ToString() ?? nonNullStr;
        }
    }
}
