//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Helpers;

namespace ThScoreFileConverter.Core.Models;

/// <summary>
/// Provides several model-dependent functions.
/// </summary>
public static class Definitions
{
    /// <summary>
    /// Gets wheter you can practice the specified level or not.
    /// </summary>
    /// <param name="level">A level.</param>
    /// <returns><see langword="true"/> if it can be practiced, otherwize <see langword="false"/>.</returns>
    public static bool CanPractice(Level level)
    {
        return EnumHelper.IsDefined(level) && (level != Level.Extra);
    }

    /// <summary>
    /// Gets wheter you can practice the specified stage or not.
    /// </summary>
    /// <param name="stage">A stage.</param>
    /// <returns><see langword="true"/> if it can be practiced, otherwize <see langword="false"/>.</returns>
    public static bool CanPractice(Stage stage)
    {
        return EnumHelper.IsDefined(stage) && (stage != Stage.Extra);
    }
}
