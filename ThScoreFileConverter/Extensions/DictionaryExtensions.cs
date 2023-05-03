//-----------------------------------------------------------------------
// <copyright file="DictionaryExtensions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#if NETFRAMEWORK

using System.Collections.Generic;
using CommunityToolkit.Diagnostics;

namespace ThScoreFileConverter.Extensions;

/// <summary>
/// Provides some extension methods for <see cref="IDictionary{TKey, TValue}"/> types.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Attempts to add the specified key and value to the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to be added the element.</param>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="value">The value of the element to add. It can be <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the key/value pair was added to the dictionary successfully; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        Guard.IsNotNull(dictionary);

        if (dictionary.ContainsKey(key))
            return false;

        dictionary.Add(key, value);
        return true;
    }
}

#endif
