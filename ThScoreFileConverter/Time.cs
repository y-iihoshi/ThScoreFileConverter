using System;

namespace ThScoreFileConverter
{
    /// <summary>
    /// 時間を表すクラス
    /// 秒未満の値をミリ秒またはフレーム数/秒 (fps) で扱う
    /// </summary>
    public class Time
    {
        public long Hours { get; private set; }
        public int Minutes { get; private set; }
        public int Seconds { get; private set; }
        public int Milliseconds { get; private set; }
        public int Frames { get; private set; }

        /// <summary>
        /// 秒未満の値をフレーム数/秒 (fps) で扱う場合 true、ミリ秒で扱う場合 false
        /// </summary>
        public bool IsFrames { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="framesOrMilliseconds">ミリ秒またはフレーム数/秒 (fps)</param>
        /// <param name="isFrames">秒未満の値を fps で扱う場合 true、ミリ秒で扱う場合 false</param>
        public Time(long framesOrMilliseconds, bool isFrames = true)
        {
            var seconds = framesOrMilliseconds / (isFrames ? 60 : 1000);
            var minutes = seconds / 60;
            var hours = minutes / 60;

            if (isFrames)
            {
                this.Milliseconds = -1;
                this.Frames = (int)(framesOrMilliseconds - seconds * 60);
            }
            else
            {
                this.Milliseconds = (int)(framesOrMilliseconds - seconds * 1000);
                this.Frames = -1;
            }
            this.Seconds = (int)(seconds - minutes * 60);
            this.Minutes = (int)(minutes - hours * 60);
            this.Hours = hours;
            this.IsFrames = isFrames;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="hours">時</param>
        /// <param name="minutes">分</param>
        /// <param name="seconds">秒</param>
        /// <param name="framesOrMilliseconds">ミリ秒または fps</param>
        /// <param name="isFrames">秒未満の値を fps で扱う場合 true、ミリ秒で扱う場合 false</param>
        public Time(long hours, int minutes, int seconds, int framesOrMilliseconds, bool isFrames = true)
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
        /// 本クラスのインスタンスを文字列化する
        /// </summary>
        /// <returns>変換後の文字列 (hh:mm:ss)</returns>
        public override string ToString()
        {
            return string.Format("{0}:{1:D2}:{2:D2}", this.Hours, this.Minutes, this.Seconds);
        }

        /// <summary>
        /// 本クラスのインスタンスを文字列化する
        /// </summary>
        /// <returns>変換後の文字列 (hh:mm:ss.ddd or hh:mm:ss.ff)</returns>
        public string ToLongString()
        {
            if (this.IsFrames)
                return string.Format(
                    "{0}:{1:D2}:{2:D2}.{3:D2}", this.Hours, this.Minutes, this.Seconds, this.Frames);
            else
                return string.Format(
                    "{0}:{1:D2}:{2:D2}.{3:D3}", this.Hours, this.Minutes, this.Seconds, this.Milliseconds);
        }
    }
}
