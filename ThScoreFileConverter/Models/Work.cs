﻿//-----------------------------------------------------------------------
// <copyright file="Work.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models;

/// <summary>
/// Represents a Touhou work.
/// </summary>
public sealed class Work
{
    /// <summary>
    /// Gets or sets a number string. Should be a property name of <see cref="Resources.StringResources"/>.
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the work is supported by this tool.
    /// </summary>
    public bool IsSupported { get; set; }
}
