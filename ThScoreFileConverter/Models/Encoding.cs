//-----------------------------------------------------------------------
// <copyright file="Encoding.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models
{
    /// <summary>
    /// Contains read-only instances of <see cref="System.Text.Encoding"/> class for convenience.
    /// </summary>
    public static class Encoding
    {
        static Encoding()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            CP932 = System.Text.Encoding.GetEncoding(932);
            Default = System.Text.Encoding.Default;
            UTF8 = new System.Text.UTF8Encoding(false);
        }

        /// <summary>
        /// Gets the code page 932 encoding.
        /// </summary>
        public static System.Text.Encoding CP932 { get; }

        /// <summary>
        /// Gets the default encoding.
        /// </summary>
        public static System.Text.Encoding Default { get; }

        /// <summary>
        /// Gets the UTF-8 encoding. The Unicode byte order mark is omitted.
        /// </summary>
        public static System.Text.Encoding UTF8 { get; }
    }
}
