//-----------------------------------------------------------------------
// <copyright file="EncodingHelper.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace ThScoreFileConverter.Helpers
{
    /// <summary>
    /// Contains read-only instances of <see cref="System.Text.Encoding"/> class for convenience.
    /// </summary>
    public static class EncodingHelper
    {
        static EncodingHelper()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            CP932 = System.Text.Encoding.GetEncoding(932);
            Default = System.Text.Encoding.Default;
            UTF8NoBOM = new System.Text.UTF8Encoding(false);
            Encodings = new Dictionary<int, System.Text.Encoding>();
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
        public static System.Text.Encoding UTF8NoBOM { get; }

        /// <summary>
        /// Gets the dictionary caching <see cref="System.Text.Encoding"/> instances.
        /// </summary>
        private static IDictionary<int, System.Text.Encoding> Encodings { get; }

        /// <summary>
        /// Returns the encoding associated with the specified code page identifier.
        /// </summary>
        /// <param name="codePage">The code page identifier of the preferred encoding.</param>
        /// <returns>The <see cref="System.Text.Encoding"/> associated with <paramref name="codePage"/>.</returns>
        public static System.Text.Encoding GetEncoding(int codePage)
        {
            if (Encodings.TryGetValue(codePage, out var encoding))
                return encoding;

            // To prevent BOM output for UTF-8
            encoding = (codePage == 65001) ? UTF8NoBOM : System.Text.Encoding.GetEncoding(codePage);
            Encodings.Add(codePage, encoding);
            return encoding;
        }
    }
}
