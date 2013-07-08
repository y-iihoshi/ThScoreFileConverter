using System;

namespace ThScoreFileConverter
{
    /// <summary>
    /// 上限値と下限値で定義される範囲を扱うクラス
    /// </summary>
    /// <typeparam Name="T">範囲に属する値の型</typeparam>
    public class Range<T> where T : IComparable<T>
    {
        /// <summary>
        /// 下限値
        /// </summary>
        public T Min { get; set; }

        /// <summary>
        /// 上限値
        /// </summary>
        public T Max { get; set; }

        /// <summary>
        /// 範囲を表す文字列に変換する
        /// </summary>
        /// <returns>"[下限値, 上限値]"</returns>
        public override string ToString()
        {
            return string.Format("[{0}, {1}]", this.Min, this.Max);
        }

        /// <summary>
        /// インスタンスの正当性をチェックする
        /// </summary>
        /// <returns>true: 正当、false: 不当</returns>
        public bool IsValid()
        {
            return this.Min.CompareTo(this.Max) <= 0;
        }

        /// <summary>
        /// value が範囲内にあるかチェックする
        /// </summary>
        /// <param Name="value"></param>
        /// <returns>true: 範囲内、false: 範囲外</returns>
        public bool Contains(T value)
        {
            return (this.Min.CompareTo(value) <= 0) && (value.CompareTo(this.Max) <= 0);
        }
    }
}
