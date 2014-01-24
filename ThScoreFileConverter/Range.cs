//-----------------------------------------------------------------------
// <copyright file="Range.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.LayoutRules",
    "SA1503:CurlyBracketsMustNotBeOmitted",
    Justification = "Reviewed.")]

namespace ThScoreFileConverter
{
    using System;

    /// <summary>
    /// Represents a range determined by the upper and lower limit values.
    /// </summary>
    /// <typeparam name="T">The type of the value that is in the range.</typeparam>
    public class Range<T> where T : IComparable<T>
    {
        /// <summary>
        /// Gets or sets the lower limit value.
        /// </summary>
        public T Min { get; set; }

        /// <summary>
        /// Gets or sets the upper limit value.
        /// </summary>
        public T Max { get; set; }

        /// <summary>
        /// Returns a string that represents the current instance.
        /// The string is formatted such as <c>[<see cref="Min"/>, <see cref="Max"/>]</c>.
        /// </summary>
        /// <returns>A string that represents the current instance.</returns>
        public override string ToString()
        {
            return string.Format("[{0}, {1}]", this.Min, this.Max);
        }

        /// <summary>
        /// Checks whether the current instance represents a valid range.
        /// </summary>
        /// <returns><c>true</c> if valid; <c>false</c> for invalid.</returns>
        public bool IsValid()
        {
            return this.Min.CompareTo(this.Max) <= 0;
        }

        /// <summary>
        /// Checks whether the specified value is in the range represented by the current instance.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="value"/> is in the range; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(T value)
        {
            return (this.Min.CompareTo(value) <= 0) && (value.CompareTo(this.Max) <= 0);
        }
    }
}
