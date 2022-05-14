//-----------------------------------------------------------------------
// <copyright file="RandomHelper.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Helpers
{
    /// <summary>
    /// Provides helper functions related to pseudo-random numbers.
    /// </summary>
    public static class RandomHelper
    {
        /// <summary>
        /// Park-Miller RNG, using Schrage's method.
        /// Refer to https://en.wikipedia.org/wiki/Lehmer_random_number_generator for more details.
        /// </summary>
        /// <param name="state">The current state.</param>
        /// <returns>The next state.</returns>
        public static int ParkMillerRNG(int state)
        {
            const int M = int.MaxValue;
            const int A = 48271;   // 0xBC8F
            const int Q = M / A;   // 0xADC8
            const int R = M % A;   // 0x0D47

            var (div, rem) = MathHelper.DivRem(state, Q);
            var diff = (rem * A) - (div * R);
            return diff < 0 ? diff + M : diff;
        }
    }
}
