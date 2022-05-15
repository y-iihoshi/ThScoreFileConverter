//-----------------------------------------------------------------------
// <copyright file="SpellCardInfo.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th075;

internal class SpellCardInfo
{
    public SpellCardInfo(string name, Chara enemy, Level level)
    {
        this.Name = name;
        this.Enemy = enemy;
        this.Level = level;
    }

    public string Name { get; }

    public Chara Enemy { get; }

    public Level Level { get; }
}
