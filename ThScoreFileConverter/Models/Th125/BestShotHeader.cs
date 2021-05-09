//-----------------------------------------------------------------------
// <copyright file="BestShotHeader.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th125
{
    internal class BestShotHeader : IBinaryReadable, IBestShotHeader
    {
        public const string ValidSignature = "BST2";
        public const int SignatureSize = 4;

        public string Signature { get; private set; } = string.Empty;

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

        public IEnumerable<byte> CardName { get; private set; } = Enumerable.Empty<byte>();

        public void ReadFrom(BinaryReader reader)
        {
            this.Signature = Encoding.Default.GetString(reader.ReadExactBytes(SignatureSize));
            if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                throw new InvalidDataException();

            _ = reader.ReadUInt16();    // always 0x0405?
            this.Level = EnumHelper.To<Level>(reader.ReadInt16() - 1);
            this.Scene = reader.ReadInt16();
            _ = reader.ReadUInt16();    // 0x0100 ... Version?
            this.Width = reader.ReadInt16();
            this.Height = reader.ReadInt16();
            _ = reader.ReadUInt32();    // always 0x00000000?
            this.Width2 = reader.ReadInt16();
            this.Height2 = reader.ReadInt16();
            this.HalfWidth = reader.ReadInt16();
            this.HalfHeight = reader.ReadInt16();
            this.DateTime = reader.ReadUInt32();
            _ = reader.ReadUInt32();    // always 0x00000000?
            this.SlowRate = reader.ReadSingle();
            this.Fields = new BonusFields(reader.ReadInt32());
            this.ResultScore = reader.ReadInt32();
            this.BasePoint = reader.ReadInt32();
            _ = reader.ReadExactBytes(0x08);
            this.RiskBonus = reader.ReadInt32();
            this.BossShot = reader.ReadSingle();
            this.NiceShot = reader.ReadSingle();
            this.AngleBonus = reader.ReadSingle();
            this.MacroBonus = reader.ReadInt32();
            this.FrontSideBackShot = reader.ReadInt32();
            this.ClearShot = reader.ReadInt32();
            _ = reader.ReadExactBytes(0x30);
            this.Angle = reader.ReadSingle();
            this.ResultScore2 = reader.ReadInt32();
            _ = reader.ReadUInt32();
            this.CardName = reader.ReadExactBytes(0x50);
        }
    }
}
