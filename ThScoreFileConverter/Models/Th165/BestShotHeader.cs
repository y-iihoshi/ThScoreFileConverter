//-----------------------------------------------------------------------
// <copyright file="BestShotHeader.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.IO;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th165;

internal class BestShotHeader : IBinaryReadable, IBestShotHeader
{
    public const string ValidSignature = "BST4";
    public const int SignatureSize = 4;

    public string Signature { get; private set; } = string.Empty;

    public Day Weekday { get; private set; }

    public short Dream { get; private set; }// 1-based

    public short Width { get; private set; }

    public short Height { get; private set; }

    public short Width2 { get; private set; }

    public short Height2 { get; private set; }

    public short HalfWidth { get; private set; }

    public short HalfHeight { get; private set; }

    public float SlowRate { get; private set; }

    public uint DateTime { get; private set; }

    public float Angle { get; private set; } // -PI .. +PI [rad]

    public int Score { get; private set; }

    public HashtagFields Fields { get; private set; }

    public int Score2 { get; private set; }

    public int BasePoint { get; private set; } // FIXME

    public int NumViewed { get; private set; }

    public int NumLikes { get; private set; }

    public int NumFavs { get; private set; }

    public int NumBullets { get; private set; }

    public int NumBulletsNearby { get; private set; }

    public int RiskBonus { get; private set; } // max(NumBulletsNearby, 2) * 40 .. min(NumBulletsNearby, 25) * 40

    public float BossShot { get; private set; } // 1.20? .. 2.00

    public float AngleBonus { get; private set; } // 1.00? .. 1.30

    public int MacroBonus { get; private set; } // 0 .. 60?

    public float LikesPerView { get; private set; }

    public float FavsPerView { get; private set; }

    public int NumHashtags { get; private set; }

    public int NumRedBullets { get; private set; }

    public int NumPurpleBullets { get; private set; }

    public int NumBlueBullets { get; private set; }

    public int NumCyanBullets { get; private set; }

    public int NumGreenBullets { get; private set; }

    public int NumYellowBullets { get; private set; }

    public int NumOrangeBullets { get; private set; }

    public int NumLightBullets { get; private set; }

    public void ReadFrom(BinaryReader reader)
    {
        this.Signature = EncodingHelper.Default.GetString(reader.ReadExactBytes(SignatureSize));
        if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
            throw new InvalidDataException();

        _ = reader.ReadUInt16(); // always 0x0401?
        this.Weekday = EnumHelper.To<Day>(reader.ReadInt16());
        this.Dream = (short)(reader.ReadInt16() + 1);
        _ = reader.ReadUInt16(); // 0x0100 ... Version?
        this.Width = reader.ReadInt16();
        this.Height = reader.ReadInt16();
        _ = reader.ReadInt32(); // always 0?
        this.Width2 = reader.ReadInt16();
        this.Height2 = reader.ReadInt16();
        this.HalfWidth = reader.ReadInt16();
        this.HalfHeight = reader.ReadInt16();
        _ = reader.ReadInt32(); // always 0?
        this.SlowRate = reader.ReadSingle();
        this.DateTime = reader.ReadUInt32();
        _ = reader.ReadInt32(); // always 0?
        this.Angle = reader.ReadSingle();
        this.Score = reader.ReadInt32();
        _ = reader.ReadInt32(); // always 0?
        this.Fields = new HashtagFields(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
        _ = reader.ReadBytes(0x28); // always all 0?
        this.Score2 = reader.ReadInt32();
        this.BasePoint = reader.ReadInt32();
        this.NumViewed = reader.ReadInt32();
        this.NumLikes = reader.ReadInt32();
        this.NumFavs = reader.ReadInt32();
        this.NumBullets = reader.ReadInt32();
        this.NumBulletsNearby = reader.ReadInt32();
        this.RiskBonus = reader.ReadInt32();
        this.BossShot = reader.ReadSingle();
        _ = reader.ReadInt32(); // always 0? (Nice Shot?)
        this.AngleBonus = reader.ReadSingle();
        this.MacroBonus = reader.ReadInt32();
        _ = reader.ReadInt32(); // always 0? (Front/Side/Back Shot?)
        _ = reader.ReadInt32(); // always 0? (Clear Shot?)
        this.LikesPerView = reader.ReadSingle();
        this.FavsPerView = reader.ReadSingle();
        this.NumHashtags = reader.ReadInt32();
        this.NumRedBullets = reader.ReadInt32();
        this.NumPurpleBullets = reader.ReadInt32();
        this.NumBlueBullets = reader.ReadInt32();
        this.NumCyanBullets = reader.ReadInt32();
        this.NumGreenBullets = reader.ReadInt32();
        this.NumYellowBullets = reader.ReadInt32();
        this.NumOrangeBullets = reader.ReadInt32();
        this.NumLightBullets = reader.ReadInt32();
        _ = reader.ReadBytes(0x44); // always all 0?
        _ = reader.ReadBytes(0x34);
    }
}
