//-----------------------------------------------------------------------
// <copyright file="FileHeader.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th07
{
    internal class FileHeader : FileHeaderBase
    {
        public const short ValidVersion = 0x000B;
        public const int ValidSize = 0x0000001C;

        public override bool IsValid => base.IsValid && (this.Version == ValidVersion) && (this.Size == ValidSize);
    }
}
