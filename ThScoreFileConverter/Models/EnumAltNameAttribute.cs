//-----------------------------------------------------------------------
// <copyright file="EnumAltNameAttribute.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace ThScoreFileConverter.Models
{
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
        public string ShortName { get; }

        /// <summary>
        /// Gets or sets a long name of the enumeration field.
        /// </summary>
        public string LongName { get; set; }
    }
}
