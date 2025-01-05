//-----------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using CommunityToolkit.Diagnostics;

namespace ThScoreFileConverter.Core.Extensions;

/// <summary>
/// Provides some extension methods for <see cref="Type"/>.
/// </summary>
public static class TypeExtensions
{
    private static readonly Dictionary<Type, string> LeafNamespaceCache = [];

    /// <summary>
    /// Gets the name of the "leaf" namespace of <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>The name of the "leaf" namespace of <paramref name="type"/>.</returns>
    public static string GetLeafNamespace(this Type type)
    {
        Guard.IsNotNull(type);

        if (LeafNamespaceCache.TryGetValue(type, out var value))
        {
            return value;
        }

        if (string.IsNullOrEmpty(type.Namespace))
        {
            value = string.Empty;
        }
        else
        {
            var index = type.Namespace.LastIndexOf(Type.Delimiter);
            value = (index >= 0) ? type.Namespace[(index + 1)..] : type.Namespace;
        }

        LeafNamespaceCache.Add(type, value);
        return value;
    }
}
