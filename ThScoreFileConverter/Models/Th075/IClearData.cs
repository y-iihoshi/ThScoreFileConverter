//-----------------------------------------------------------------------
// <copyright file="IClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th075
{
    internal interface IClearData
    {
        IReadOnlyList<short> CardGotCount { get; }

        IReadOnlyList<short> CardTrialCount { get; }

        IReadOnlyList<byte> CardTrulyGot { get; }

        int ClearCount { get; }

        IReadOnlyList<int> MaxBonuses { get; }

        int MaxCombo { get; }

        int MaxDamage { get; }

        IReadOnlyList<IHighScore> Ranking { get; }

        int UseCount { get; }
    }
}
