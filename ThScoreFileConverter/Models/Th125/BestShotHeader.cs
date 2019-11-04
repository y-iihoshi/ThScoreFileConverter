//-----------------------------------------------------------------------
// <copyright file="BestShotHeader.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th125
{
    internal class BestShotHeader : IBinaryReadable
    {
        public const string ValidSignature = "BST2";
        public const int SignatureSize = 4;

        public string Signature { get; private set; }

        public Level Level { get; private set; }

        public short Scene { get; private set; }        // 1-based

        public short Width { get; private set; }

        public short Height { get; private set; }

        public short Width2 { get; private set; }       // ???

        public short Height2 { get; private set; }      // ???

        public short HalfWidth { get; private set; }    // ???

        public short HalfHeight { get; private set; }   // ???

        public uint DateTime { get; private set; }

        public float SlowRate { get; private set; }     // Really...?

        public BonusFields Fields { get; private set; }

        public int ResultScore { get; private set; }

        public int BasePoint { get; private set; }

        public int RiskBonus { get; private set; }

        public float BossShot { get; private set; }

        public float NiceShot { get; private set; }     // minimum = 1.20?

        public float AngleBonus { get; private set; }

        public int MacroBonus { get; private set; }

        public int FrontSideBackShot { get; private set; }  // Really...?

        public int ClearShot { get; private set; }

        public float Angle { get; private set; }

        public int ResultScore2 { get; private set; }   // ???

        public IEnumerable<byte> CardName { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            this.Signature = Encoding.Default.GetString(reader.ReadExactBytes(SignatureSize));
            if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                throw new InvalidDataException();

            reader.ReadUInt16();    // always 0x0405?
            this.Level = Utils.ToEnum<Level>(reader.ReadInt16() - 1);
            this.Scene = reader.ReadInt16();
            reader.ReadUInt16();    // 0x0100 ... Version?
            this.Width = reader.ReadInt16();
            this.Height = reader.ReadInt16();
            reader.ReadUInt32();    // always 0x00000000?
            this.Width2 = reader.ReadInt16();
            this.Height2 = reader.ReadInt16();
            this.HalfWidth = reader.ReadInt16();
            this.HalfHeight = reader.ReadInt16();
            this.DateTime = reader.ReadUInt32();
            reader.ReadUInt32();    // always 0x00000000?
            this.SlowRate = reader.ReadSingle();
            this.Fields = new BonusFields(reader.ReadInt32());
            this.ResultScore = reader.ReadInt32();
            this.BasePoint = reader.ReadInt32();
            reader.ReadExactBytes(0x08);
            this.RiskBonus = reader.ReadInt32();
            this.BossShot = reader.ReadSingle();
            this.NiceShot = reader.ReadSingle();
            this.AngleBonus = reader.ReadSingle();
            this.MacroBonus = reader.ReadInt32();
            this.FrontSideBackShot = reader.ReadInt32();
            this.ClearShot = reader.ReadInt32();
            reader.ReadExactBytes(0x30);
            this.Angle = reader.ReadSingle();
            this.ResultScore2 = reader.ReadInt32();
            reader.ReadUInt32();
            this.CardName = reader.ReadExactBytes(0x50);
        }

        public struct BonusFields
        {
            private BitVector32 data;

            public BonusFields(int data) => this.data = new BitVector32(data);

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
    }
}
