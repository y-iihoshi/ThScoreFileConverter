﻿//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

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
    /// <returns><c>true</c> if it can be practiced, otherwize <c>false</c>.</returns>
    public static bool CanPractice(Level level)
    {
        return level != Level.Extra;
    }

    /// <summary>
    /// Gets wheter you can practice the specified stage or not.
    /// </summary>
    /// <param name="stage">A stage.</param>
    /// <returns><c>true</c> if it can be practiced, otherwize <c>false</c>.</returns>
    public static bool CanPractice(Stage stage)
    {
        return stage != Stage.Extra;
    }
}