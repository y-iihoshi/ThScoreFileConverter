//-----------------------------------------------------------------------
// <copyright file="IStringReplaceable.cs" company="None">
//     (c) 2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter
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
