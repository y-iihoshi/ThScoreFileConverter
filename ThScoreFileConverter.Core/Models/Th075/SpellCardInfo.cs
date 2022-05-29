//-----------------------------------------------------------------------
// <copyright file="SpellCardInfo.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th075;

/// <summary>
/// Indicates information of a IMP spell card.
/// </summary>
public class SpellCardInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpellCardInfo"/> class.
    /// </summary>
    /// <param name="name">A name of the spell card.</param>
    /// <param name="enemy">The enemy character who use the spell card.</param>
    /// <param name="level">The level which the spell card is used.</param>
    public SpellCardInfo(string name, Chara enemy, Level level)
    {
        this.Name = name;
        this.Enemy = enemy;
        this.Level = level;
    }

    /// <summary>
    /// Gets a name of the current spell card.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the enemy character who uses the current spell card.
    /// </summary>
    public Chara Enemy { get; }

    /// <summary>
    /// Gets the level which the current spell card is used.
    /// </summary>
    public Level Level { get; }
}
