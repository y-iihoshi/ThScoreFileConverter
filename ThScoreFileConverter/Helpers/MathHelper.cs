//-----------------------------------------------------------------------
// <copyright file="MathHelper.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace ThScoreFileConverter.Helpers
{
    /// <summary>
    /// Provides helper methods for mathematical functions.
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Produces the quotient and the remainder of two signed 32-bit numbers.
        /// </summary>
        /// <param name="left">The dividend.</param>
        /// <param name="right">The divisor.</param>
        /// <returns>The quotient and the remainder of the specified numbers.</returns>
        public static (int Quotient, int Remainder) DivRem(int left, int right)
        {
#if NET6_0_OR_GREATER
            return Math.DivRem(left, right);
#else
            var div = Math.DivRem(left, right, out var rem);
            return (div, rem);
#endif
        }
    }
}
