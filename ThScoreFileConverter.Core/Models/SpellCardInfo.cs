﻿//-----------------------------------------------------------------------
// <copyright file="SpellCardInfo.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Helpers;

namespace ThScoreFileConverter.Core.Models;

/// <summary>
/// Indicates information of a spell card.
/// </summary>
/// <typeparam name="TStage">An enumeration type of the stage.</typeparam>
/// <typeparam name="TLevel">An enumeration type of the level.</typeparam>
public class SpellCardInfo<TStage, TLevel>
    where TStage : struct, Enum
    where TLevel : struct, Enum
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpellCardInfo{TStage,TLevel}"/> class.
    /// </summary>
    /// <param name="id">A 1-based sequential number of the spell card.</param>
    /// <param name="name">A name of the spell card.</param>
    /// <param name="stage">The stage which the spell card is used.</param>
    /// <param name="levels">The level(s) which the spell card is used.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> is negative.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="stage"/> does not exist in the <typeparamref name="TStage"/> enumeration.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// At least one element of <paramref name="levels"/> does not exist in the <typeparamref name="TLevel"/>
    /// enumeration.
    /// </exception>
    /// <exception cref="ArgumentException"><paramref name="levels"/> has no elements.</exception>
    public SpellCardInfo(int id, string name, TStage stage, params TLevel[] levels)
    {
        Guard.IsGreaterThan(id, 0);
        Guard.IsNotNullOrEmpty(name);
        Guard.IsTrue(EnumHelper.IsDefined(stage), nameof(stage));
        Guard.IsNotEmpty(levels);
        Guard.IsTrue(levels.All(EnumHelper.IsDefined), nameof(levels));

        this.Id = id;
        this.Name = name;
        this.Stage = stage;
        this.Levels = levels;
    }

    /// <summary>
    /// Gets a 1-based sequential number of the current spell card.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets a name of the current spell card.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the stage which the current spell card is used.
    /// </summary>
    public TStage Stage { get; }

    /// <summary>
    /// Gets the level which the current spell card is used.
    /// </summary>
    public TLevel Level => this.Levels.First();

    /// <summary>
    /// Gets the levels which the current spell card is used.
    /// </summary>
    /// <remarks>This is for TH06 only.</remarks>
    public IEnumerable<TLevel> Levels { get; }
}
