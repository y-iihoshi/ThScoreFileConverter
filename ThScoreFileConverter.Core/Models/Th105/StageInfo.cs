//-----------------------------------------------------------------------
// <copyright file="StageInfo.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th105;

/// <summary>
/// Indicates information of a SWR stage.
/// </summary>
/// <typeparam name="TChara">The type of a enemy character.</typeparam>
/// <param name="stage">The stage.</param>
/// <param name="enemy">The enemy character of <paramref name="stage"/>.</param>
/// <param name="cardIds">The identifiers of cards used by <paramref name="enemy"/>.</param>
public class StageInfo<TChara>(Stage stage, TChara enemy, IEnumerable<int> cardIds)
    where TChara : struct, Enum
{
    /// <summary>
    /// Gets the stage.
    /// </summary>
    public Stage Stage { get; } = stage;

    /// <summary>
    /// Gets the enemy character of the current stage.
    /// </summary>
    public TChara Enemy { get; } = enemy;

    /// <summary>
    /// Gets the identifiers of cards used by <see cref="Enemy"/>.
    /// An identifier is a 0-based number.
    /// </summary>
    public IEnumerable<int> CardIds { get; } = cardIds.ToList();
}
