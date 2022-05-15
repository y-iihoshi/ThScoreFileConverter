//-----------------------------------------------------------------------
// <copyright file="BinaryReadableHelper.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverter.Helpers;

/// <summary>
/// Helper functions for <see cref="IBinaryReadable"/>.
/// </summary>
public static class BinaryReadableHelper
{
    /// <summary>
    /// Creates an instance by using <see cref="BinaryReader"/>.
    /// </summary>
    /// <typeparam name="T">The type to be instantiated.</typeparam>
    /// <param name="reader">A <see cref="BinaryReader"/>.</param>
    /// <returns>The created instance.</returns>
    public static T Create<T>(BinaryReader reader)
        where T : IBinaryReadable, new()
    {
        var instance = new T();
        instance.ReadFrom(reader);
        return instance;
    }
}
