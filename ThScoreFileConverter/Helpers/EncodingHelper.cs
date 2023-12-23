//-----------------------------------------------------------------------
// <copyright file="EncodingHelper.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;

namespace ThScoreFileConverter.Helpers;

/// <summary>
/// Contains read-only instances of <see cref="Encoding"/> class for convenience.
/// </summary>
public static class EncodingHelper
{
    static EncodingHelper()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        CP932 = Encoding.GetEncoding(932);
        Default = Encoding.Default;
        UTF8 = Encoding.UTF8;
        UTF8NoBOM = new UTF8Encoding(false);
        Encodings = [];
    }

    /// <summary>
    /// Gets the code page 932 encoding.
    /// </summary>
    public static Encoding CP932 { get; }

    /// <summary>
    /// Gets the default encoding.
    /// </summary>
    public static Encoding Default { get; }

    /// <summary>
    /// Gets the UTF-8 encoding. The Unicode byte order mark is emitted.
    /// </summary>
    public static Encoding UTF8 { get; }

    /// <summary>
    /// Gets the UTF-8 encoding. The Unicode byte order mark is omitted.
    /// </summary>
    public static Encoding UTF8NoBOM { get; }

    /// <summary>
    /// Gets the dictionary caching <see cref="Encoding"/> instances.
    /// </summary>
    private static Dictionary<int, Encoding> Encodings { get; }

    /// <summary>
    /// Returns the encoding associated with the specified code page identifier.
    /// </summary>
    /// <param name="codePage">The code page identifier of the preferred encoding.</param>
    /// <returns>The <see cref="Encoding"/> associated with <paramref name="codePage"/>.</returns>
    public static Encoding GetEncoding(int codePage)
    {
        if (Encodings.TryGetValue(codePage, out var encoding))
            return encoding;

        // To prevent BOM output for UTF-8
        encoding = (codePage == 65001) ? UTF8NoBOM : Encoding.GetEncoding(codePage);
        Encodings.Add(codePage, encoding);
        return encoding;
    }
}
