using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ThScoreFileConverter
{
    public static class Utils
    {
        /// <summary>
        /// 列挙型 T の全要素名を連結した文字列を生成する
        /// </summary>
        /// <typeparam Name="T">列挙型</typeparam>
        /// <param Name="separator">各要素名の間に挿入される区切り記号</param>
        /// <returns>連結後の文字列</returns>
        public static string JoinEnumNames<T>(string separator)
        {
            return string.Join(separator, Enum.GetNames(typeof(T)));
        }

        /// <summary>
        /// Enum.Parse() の Wrapper 関数
        /// </summary>
        /// <typeparam Name="T">列挙型</typeparam>
        /// <param Name="value">変換する名前または値が含まれている文字列</param>
        /// <returns></returns>
        public static T ParseEnum<T>(string value, bool ignoreCase = false)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        /// <summary>
        /// 数値から文字列への変換
        /// </summary>
        /// <typeparam name="T">数値の型</typeparam>
        /// <param name="number">数値</param>
        /// <param name="outputSeparator">桁区切り形式に変換する場合 true</param>
        /// <returns>変換後の文字列</returns>
        public static string ToNumberString<T>(T number, bool outputSeparator) where T : struct
        {
            return outputSeparator ? string.Format("{0:N0}", number) : number.ToString();
        }

        /// <summary>
        /// 例外を throw しない MatchEvaluator デリゲートへの変換
        /// <paramref name="evaluator"/> で例外が発生した場合は正規表現に一致した文字列がそのまま返る
        /// </summary>
        /// <param name="evaluator">変換元の MatchEvaluator デリゲート</param>
        /// <returns>変換後の MatchEvaluator デリゲート</returns>
        public static MatchEvaluator ToNothrowEvaluator(MatchEvaluator evaluator)
        {
            return match =>
            {
                try { return evaluator(match); }
                catch { return match.ToString(); }
            };
        }

        /// <summary>
        /// バイナリーからの読み込みが可能なクラス向けのインターフェース
        /// </summary>
        public interface IBinaryReadable
        {
            void ReadFrom(BinaryReader reader);
        }

        /// <summary>
        /// http://www.atmarkit.co.jp/fdotnet/special/cs20review01/cs20review01_03.html
        /// </summary>
        /// <typeparam name="T">Type of the object to compare</typeparam>
        public class And<T>
        {
            private Func<T, bool>[] predicates;

            public And(params Func<T, bool>[] predicates)
            {
                this.predicates = predicates;
            }

            private bool Pred(T obj)
            {
                return this.predicates.All(pred => pred(obj));
            }

            public static implicit operator Func<T, bool>(And<T> from)
            {
                return from.Pred;
            }
        }
    }
}
