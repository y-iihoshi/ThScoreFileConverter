//-----------------------------------------------------------------------
// <copyright file="ThConverterEventArgs.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models;

/// <summary>
/// Represents the event data issued by the <see cref="ThConverter"/> class.
/// </summary>
internal sealed class ThConverterEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThConverterEventArgs"/> class.
    /// </summary>
    public ThConverterEventArgs()
    {
        this.Path = string.Empty;
        this.Current = 0;
        this.Total = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ThConverterEventArgs"/> class with the specified parameters.
    /// </summary>
    /// <param name="path">The path of the last output file.</param>
    /// <param name="current">The number of the files that have been output.</param>
    /// <param name="total">The total number of the files.</param>
    /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="path"/> is empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="current"/> is not positive or greater than <paramref name="total"/>.
    /// </exception>
    public ThConverterEventArgs(string path, int current, int total)
    {
        Guard.IsNotNullOrEmpty(path);
        Guard.IsGreaterThan(current, 0);
        Guard.IsGreaterThanOrEqualTo(total, current);

        this.Path = path;
        this.Current = current;
        this.Total = total;
    }

    /// <summary>
    /// Gets the path of the last output file.
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// Gets the number of the files that have been output.
    /// </summary>
    public int Current { get; }

    /// <summary>
    /// Gets the total number of the files.
    /// </summary>
    public int Total { get; }

    /// <summary>
    /// Gets a message string that represents the current instance.
    /// </summary>
    public string Message => StringHelper.Create($"({this.Current}/{this.Total}) {System.IO.Path.GetFileName(this.Path)} ");
}
