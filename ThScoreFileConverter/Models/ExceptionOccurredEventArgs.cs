//-----------------------------------------------------------------------
// <copyright file="ExceptionOccurredEventArgs.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace ThScoreFileConverter.Models;

/// <summary>
/// Represents the event data that indicates occurring of an exception.
/// </summary>
/// <param name="ex">The exception data.</param>
internal sealed class ExceptionOccurredEventArgs(Exception ex) : EventArgs
{
    /// <summary>
    /// Gets the exception data.
    /// </summary>
    public Exception Exception { get; } = ex;
}
