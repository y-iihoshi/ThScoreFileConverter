//-----------------------------------------------------------------------
// <copyright file="ShotTypeAttribute{T}.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Resources;

namespace ThScoreFileConverter.Core.Models;

/// <summary>
/// Provides names of the playable character's shot type represented as an enumeration field.
/// </summary>
/// <typeparam name="T">The enumeration type representing playable characters' shot types.</typeparam>
[CLSCompliant(false)]
public sealed class ShotTypeAttribute<T> : EnumDisplayAttribute
    where T : struct, Enum
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ShotTypeAttribute{T}"/> class.
    /// </summary>
    /// <param name="value">An enumeration field representing a playable character's shot type.</param>
    public ShotTypeAttribute(T value)
        : base($"{typeof(T).GetLeafNamespace()}.{value}", typeof(ShotTypeNames))
    {
        Guard.IsTrue(Enum.IsDefined(value));
        this.Value = value;
    }

    /// <summary>
    /// Gets the value of the playable character's shot type.
    /// </summary>
    public T Value { get; }
}
