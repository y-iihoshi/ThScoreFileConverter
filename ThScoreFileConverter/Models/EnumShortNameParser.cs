//-----------------------------------------------------------------------
// <copyright file="EnumShortNameParser.cs" company="None">
//     (c) 2014-2015 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides a parser for the enumeration type which fields have short names.
    /// </summary>
    /// <typeparam name="TEnum">The enumeration type which fields have short names.</typeparam>
    [CLSCompliant(false)]
    public sealed class EnumShortNameParser<TEnum>
        where TEnum : struct, IComparable, IFormattable, IConvertible
    {
        /// <summary>
        /// Elements of <typeparamref name="TEnum"/>.
        /// </summary>
        private static readonly IEnumerable<TEnum> Elements = Utils.GetEnumerator<TEnum>();

        /// <summary>
        /// A regular expression of the short names of <typeparamref name="TEnum"/>.
        /// </summary>
        private static readonly string PatternImpl =
            string.Join("|", Elements.Select(elem => elem.ToShortName()).Distinct().ToArray());

        /// <summary>
        /// Gets a regular expression of the short names of <typeparamref name="TEnum"/>.
        /// </summary>
        public string Pattern
        {
            get { return PatternImpl; }
        }

        /// <summary>
        /// Converts from the string matched with the pattern to a value of <typeparamref name="TEnum"/>.
        /// </summary>
        /// <param name="shortName">The string matched with the pattern.</param>
        /// <returns>A value of <typeparamref name="TEnum"/>.</returns>
        public TEnum Parse(string shortName)
        {
            return Elements.First(
                elem => elem.ToShortName().Equals(shortName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
