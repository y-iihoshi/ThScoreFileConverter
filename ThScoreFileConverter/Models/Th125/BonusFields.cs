//-----------------------------------------------------------------------
// <copyright file="BonusFields.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Specialized;

namespace ThScoreFileConverter.Models.Th125;

internal struct BonusFields
{
    private BitVector32 data;

    public BonusFields(int data)
    {
        this.data = new BitVector32(data);
    }

    public int Data => this.data.Data;

    public bool TwoShot => this.data[0x00000004];

    public bool NiceShot => this.data[0x00000008];

    public bool RiskBonus => this.data[0x00000010];

    public bool RedShot => this.data[0x00000040];

    public bool PurpleShot => this.data[0x00000080];

    public bool BlueShot => this.data[0x00000100];

    public bool CyanShot => this.data[0x00000200];

    public bool GreenShot => this.data[0x00000400];

    public bool YellowShot => this.data[0x00000800];

    public bool OrangeShot => this.data[0x00001000];

    public bool ColorfulShot => this.data[0x00002000];

    public bool RainbowShot => this.data[0x00004000];

    public bool SoloShot => this.data[0x00010000];

    public bool MacroBonus => this.data[0x00400000];

    public bool FrontShot => this.data[0x01000000];

    public bool BackShot => this.data[0x02000000];

    public bool SideShot => this.data[0x04000000];

    public bool ClearShot => this.data[0x08000000];

    public bool CatBonus => this.data[0x10000000];
}
