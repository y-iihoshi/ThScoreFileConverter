//-----------------------------------------------------------------------
// <copyright file="IStringReplaceable.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models
{
    /// <summary>
    /// Defines a method to replace a string.
    /// </summary>
    internal interface IStringReplaceable
    {
        /// <summary>
        /// Replaces a string.
        /// </summary>
        /// <param name="input">An input string to replace.</param>
        /// <returns>The replaced string.</returns>
        string Replace(string input);
    }
}
