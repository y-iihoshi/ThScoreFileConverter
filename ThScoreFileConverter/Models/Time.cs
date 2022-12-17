//-----------------------------------------------------------------------
// <copyright file="Time.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using CommunityToolkit.Diagnostics;

namespace ThScoreFileConverter.Models;

/// <summary>
/// Represents a time.
/// A value of less than a second is treated as milliseconds or frames-per-second (fps).
/// </summary>
public class Time
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Time"/> class to a specified number of frames.
    /// </summary>
    /// <param name="frames">Number of frames.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="frames"/> is negative.</exception>
    public Time(long frames)
        : this(frames, true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Time"/> class to a specified number of frames
    /// or milliseconds.
    /// </summary>
    /// <param name="framesOrMilliseconds">Number of frames or milliseconds.</param>
    /// <param name="isFrames">
    /// <see langword="true"/> if treats <paramref name="framesOrMilliseconds"/> as a frames;
    /// <see langword="false"/> for milliseconds.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="framesOrMilliseconds"/> is negative.
    /// </exception>
    public Time(long framesOrMilliseconds, bool isFrames)
    {
        Guard.IsGreaterThanOrEqualTo(framesOrMilliseconds, 0);

        var seconds = framesOrMilliseconds / (isFrames ? 60 : 1000);
        var minutes = seconds / 60;
        var hours = minutes / 60;

        if (isFrames)
        {
            this.Milliseconds = -1;
            this.Frames = (int)(framesOrMilliseconds - (seconds * 60));
        }
        else
        {
            this.Milliseconds = (int)(framesOrMilliseconds - (seconds * 1000));
            this.Frames = -1;
        }

        this.Seconds = (int)(seconds - (minutes * 60));
        this.Minutes = (int)(minutes - (hours * 60));
        this.Hours = hours;
        this.IsFrames = isFrames;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Time"/> class to a specified number of hours,
    /// minutes, seconds and frames.
    /// </summary>
    /// <param name="hours">Number of hours.</param>
    /// <param name="minutes">Number of minutes.</param>
    /// <param name="seconds">Number of seconds.</param>
    /// <param name="frames">Number of frames.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="hours"/> is negative.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="minutes"/> is negative or exceeds 59.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="seconds"/> is negative or exceeds 59.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="frames"/> is negative or exceeds 59.
    /// </exception>
    public Time(long hours, int minutes, int seconds, int frames)
        : this(hours, minutes, seconds, frames, true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Time"/> class to a specified number of hours,
    /// minutes, seconds, and frames or milliseconds.
    /// </summary>
    /// <param name="hours">Number of hours.</param>
    /// <param name="minutes">Number of minutes.</param>
    /// <param name="seconds">Number of seconds.</param>
    /// <param name="framesOrMilliseconds">Number of frames or milliseconds.</param>
    /// <param name="isFrames">
    /// <see langword="true"/> if treats <paramref name="framesOrMilliseconds"/> as a frames;
    /// <see langword="false"/> for milliseconds.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="hours"/> is negative.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="minutes"/> is negative or exceeds 59.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="seconds"/> is negative or exceeds 59.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="framesOrMilliseconds"/> is negative or exceeds the maximum value:
    /// 59 if <paramref name="isFrames"/> is <see langword="true"/>; otherwise 999.
    /// </exception>
    public Time(long hours, int minutes, int seconds, int framesOrMilliseconds, bool isFrames)
    {
        Guard.IsGreaterThanOrEqualTo(hours, 0);
        Guard.IsInRange(minutes, 0, 60);
        Guard.IsInRange(seconds, 0, 60);
        Guard.IsInRange(framesOrMilliseconds, 0, isFrames ? 60 : 1000);

        this.Hours = hours;
        this.Minutes = minutes;
        this.Seconds = seconds;

        if (isFrames)
        {
            this.Milliseconds = -1;
            this.Frames = framesOrMilliseconds;
        }
        else
        {
            this.Milliseconds = framesOrMilliseconds;
            this.Frames = -1;
        }

        this.IsFrames = isFrames;
    }

    /// <summary>
    /// Gets the hours component of the time represented by the current instance.
    /// </summary>
    public long Hours { get; }

    /// <summary>
    /// Gets the minutes component of the time represented by the current instance.
    /// </summary>
    public int Minutes { get; }

    /// <summary>
    /// Gets the seconds component of the time represented by the current instance.
    /// </summary>
    public int Seconds { get; }

    /// <summary>
    /// Gets the milliseconds component of the time represented by the current instance.
    /// </summary>
    /// <remarks>This property should be used exclusively with <see cref="Frames"/>.</remarks>
    public int Milliseconds { get; }

    /// <summary>
    /// Gets the frames component of the time represented by the current instance.
    /// </summary>
    /// <remarks>This property should be used exclusively with <see cref="Milliseconds"/>.</remarks>
    public int Frames { get; }

    /// <summary>
    /// Gets a value indicating whether the value less than a second is treated as fps or milliseconds.
    /// <see langword="true"/> if fps; <see langword="false"/> for milliseconds.
    /// </summary>
    public bool IsFrames { get; }

    /// <summary>
    /// Returns a string that represents the current instance.
    /// The string is formatted such as <c>hh:mm:ss</c>.
    /// </summary>
    /// <returns>A string that represents the current instance.</returns>
    public override string ToString()
    {
        return Utils.Format("{0}:{1:D2}:{2:D2}", this.Hours, this.Minutes, this.Seconds);
    }

    /// <summary>
    /// Returns a string that represents the current instance.
    /// If <see cref="IsFrames"/> is <see langword="true"/>, the string is formatted such as <c>hh:mm:ss.ff</c>;
    /// otherwise, <c>hh:mm:ss.ddd</c>.
    /// </summary>
    /// <returns>A string that represents the current instance.</returns>
    public string ToLongString()
    {
        return this.IsFrames
            ? Utils.Format(
                "{0}:{1:D2}:{2:D2}.{3:D2}", this.Hours, this.Minutes, this.Seconds, this.Frames)
            : Utils.Format(
                "{0}:{1:D2}:{2:D2}.{3:D3}", this.Hours, this.Minutes, this.Seconds, this.Milliseconds);
    }
}
