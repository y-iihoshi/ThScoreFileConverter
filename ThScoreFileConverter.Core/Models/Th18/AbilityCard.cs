//-----------------------------------------------------------------------
// <copyright file="AbilityCard.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace ThScoreFileConverter.Core.Models.Th18;

/// <summary>
/// Represents an ability card of UM.
/// </summary>
public class AbilityCard
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AbilityCard"/> class.
    /// </summary>
    /// <param name="id">Identifier of the card.</param>
    /// <param name="name">Name of the card.</param>
    /// <param name="type">Type of the card.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> is negative.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    public AbilityCard(int id, string name, AbilityCardType type)
    {
        if (id < 0)
            throw new ArgumentOutOfRangeException(nameof(id));
        if (name is null)
            throw new ArgumentNullException(nameof(name));

        this.Id = id;
        this.Name = name;
        this.Type = type;
    }

    /// <summary>
    /// Gets the 0-based identifier.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the type.
    /// </summary>
    public AbilityCardType Type { get; }
}
