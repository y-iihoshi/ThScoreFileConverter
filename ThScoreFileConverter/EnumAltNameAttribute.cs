//-----------------------------------------------------------------------
// <copyright file="EnumAltNameAttribute.cs" company="None">
//     (c) 2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter
{
    using System;

    /// <summary>
    /// Provides alternative names of enumeration fields.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class EnumAltNameAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumAltNameAttribute"/> class.
        /// </summary>
        /// <param name="shortName">A short name of the enumeration field.</param>
        public EnumAltNameAttribute(string shortName)
        {
            this.ShortName = shortName;
            this.LongName = string.Empty;
        }

        /// <summary>
        /// Gets a short name of the enumeration field.
        /// </summary>
        public string ShortName { get; private set; }

        /// <summary>
        /// Gets or sets a long name of the enumeration field.
        /// </summary>
        public string LongName { get; set; }
    }
}
