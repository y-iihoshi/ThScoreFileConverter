//-----------------------------------------------------------------------
// <copyright file="CardAttack.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th07;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th07;

internal sealed class CardAttack : Th06.Chapter, ICardAttack   // per card
{
    public const string ValidSignature = "CATK";
    public const short ValidSize = 0x0078;

    public CardAttack(Th06.Chapter chapter)
        : base(chapter, ValidSignature, ValidSize)
    {
        var charas = EnumHelper<CharaWithTotal>.Enumerable;

        using var stream = new MemoryStream(this.Data, false);
        using var reader = new BinaryReader(stream);

        _ = reader.ReadUInt32();    // always 0x00000001?
        this.MaxBonuses = charas.ToDictionary(chara => chara, chara => reader.ReadUInt32());
        this.CardId = (short)(reader.ReadInt16() + 1);
        _ = reader.ReadByte();
#pragma warning disable IDE0306 // Simplify collection initialization
        this.CardName = new ReadOnlyCP932Bytes(reader.ReadExactBytes(0x30));
#pragma warning restore IDE0306 // Simplify collection initialization
        _ = reader.ReadByte();      // always 0x00?
        this.TrialCounts = charas.ToDictionary(chara => chara, chara => reader.ReadUInt16());
        this.ClearCounts = charas.ToDictionary(chara => chara, chara => reader.ReadUInt16());
    }

    public IReadOnlyDictionary<CharaWithTotal, uint> MaxBonuses { get; }

    public short CardId { get; }    // 1-based

    public IEnumerable<byte> CardName { get; }

    public IReadOnlyDictionary<CharaWithTotal, ushort> TrialCounts { get; }

    public IReadOnlyDictionary<CharaWithTotal, ushort> ClearCounts { get; }

    public bool HasTried => this.TrialCounts[CharaWithTotal.Total] > 0;
}
