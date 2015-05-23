//-----------------------------------------------------------------------
// <copyright file="Time.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models
{
    using System;

    /// <summary>
    /// Represents a time.
    /// A value of less than a second is treated as milliseconds or frames-per-second (fps).
    /// </summary>
    public class Time
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Time"/> class to a specified number of frames.
        /// </summary>
        /// <param name="frames">Number of frames</param>
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
        /// <c>true</c> if treats <paramref name="framesOrMilliseconds"/> as a frames; <c>false</c> for
        /// milliseconds.
        /// </param>
        public Time(long framesOrMilliseconds, bool isFrames)
        {
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
        /// <c>true</c> if treats <paramref name="framesOrMilliseconds"/> as a frames; <c>false</c> for
        /// milliseconds.
        /// </param>
        public Time(long hours, int minutes, int seconds, int framesOrMilliseconds, bool isFrames)
        {
            if (minutes >= 60)
                throw new ArgumentOutOfRangeException("minutes");
            if (seconds >= 60)
                throw new ArgumentOutOfRangeException("seconds");
            if (framesOrMilliseconds >= (isFrames ? 60 : 1000))
                throw new ArgumentOutOfRangeException("framesOrMilliseconds");

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
        public long Hours { get; private set; }

        /// <summary>
        /// Gets the minutes component of the time represented by the current instance.
        /// </summary>
        public int Minutes { get; private set; }

        /// <summary>
        /// Gets the seconds component of the time represented by the current instance.
        /// </summary>
        public int Seconds { get; private set; }

        /// <summary>
        /// Gets the milliseconds component of the time represented by the current instance.
        /// </summary>
        /// <remarks>This property should be used exclusively with <see cref="Frames"/>.</remarks>
        public int Milliseconds { get; private set; }

        /// <summary>
        /// Gets the frames component of the time represented by the current instance.
        /// </summary>
        /// <remarks>This property should be used exclusively with <see cref="Milliseconds"/>.</remarks>
        public int Frames { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the value less than a second is treated as fps or milliseconds.
        /// <c>true</c> if fps; <c>false</c> for milliseconds.
        /// </summary>
        public bool IsFrames { get; private set; }

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
        /// If <see cref="IsFrames"/> is <c>true</c>, the string is formatted such as <c>hh:mm:ss.ff</c>;
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
}
