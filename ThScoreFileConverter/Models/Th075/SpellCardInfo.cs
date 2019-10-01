//-----------------------------------------------------------------------
// <copyright file="SpellCardInfo.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th075
{
    internal class SpellCardInfo
    {
        public SpellCardInfo(string name, Th075Converter.Chara enemy, Th075Converter.Level level)
        {
            this.Name = name;
            this.Enemy = enemy;
            this.Level = level;
        }

        public string Name { get; }

        public Th075Converter.Chara Enemy { get; }

        public Th075Converter.Level Level { get; }
    }
}
