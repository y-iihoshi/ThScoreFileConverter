//-----------------------------------------------------------------------
// <copyright file="ClearDataBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th10;

internal class ClearDataBase<TCharaWithTotal, TScoreData>
    : Chapter, IClearData<TCharaWithTotal>
    where TCharaWithTotal : struct, Enum
    where TScoreData : IBinaryReadable, IScoreData<StageProgress>, new()
{
    public const string ValidSignature = "CR";

    protected ClearDataBase(Chapter chapter, ushort validVersion, int validSize, int numCards)
        : base(chapter, ValidSignature, validVersion, validSize)
    {
        var levels = EnumHelper<Level>.Enumerable;
        var levelsExceptExtra = levels.Where(lv => lv != Level.Extra);
        var stages = EnumHelper<Stage>.Enumerable;
        var stagesExceptExtra = stages.Where(st => st != Stage.Extra);

        using var stream = new MemoryStream(this.Data, false);
        using var reader = new BinaryReader(stream);

        this.Chara = EnumHelper.To<TCharaWithTotal>(reader.ReadInt32());

        this.Rankings = levels.ToDictionary(
            level => level,
            _ => (IReadOnlyList<IScoreData<StageProgress>>)[.. Enumerable.Range(0, 10).Select(_ => BinaryReadableHelper.Create<TScoreData>(reader))]);

        this.TotalPlayCount = reader.ReadInt32();
        this.PlayTime = reader.ReadInt32();
        this.ClearCounts = levels.ToDictionary(level => level, _ => reader.ReadInt32());

        this.Practices = levelsExceptExtra.Cartesian(stagesExceptExtra)
            .ToDictionary(pair => pair, _ => BinaryReadableHelper.Create<Practice>(reader) as IPractice);

        this.Cards = Enumerable.Range(0, numCards)
            .Select(_ => BinaryReadableHelper.Create<SpellCard>(reader) as ISpellCard<Level>)
            .ToDictionary(card => card.Id);
    }

    public TCharaWithTotal Chara { get; }

    public IReadOnlyDictionary<Level, IReadOnlyList<IScoreData<StageProgress>>> Rankings { get; }

    public int TotalPlayCount { get; }

    public int PlayTime { get; }    // = seconds * 60fps

    public IReadOnlyDictionary<Level, int> ClearCounts { get; }

    public IReadOnlyDictionary<(Level Level, Stage Stage), IPractice> Practices { get; }

    public IReadOnlyDictionary<int, ISpellCard<Level>> Cards { get; }

    protected static bool CanInitialize(Chapter chapter)
    {
        return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal);
    }
}
