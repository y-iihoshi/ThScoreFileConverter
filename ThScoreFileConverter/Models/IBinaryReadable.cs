//-----------------------------------------------------------------------
// <copyright file="IBinaryReadable.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.IO;

namespace ThScoreFileConverter.Models;

/// <summary>
/// Defines a method to read from a binary stream.
/// </summary>
public interface IBinaryReadable
{
    /// <summary>
    /// Reads from a stream by using the specified <see cref="BinaryReader"/> instance.
    /// </summary>
    /// <param name="reader">The instance to use.</param>
    void ReadFrom(BinaryReader reader);
}
