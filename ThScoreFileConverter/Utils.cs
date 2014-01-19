using System;
using System.Collections.Generic;
using System.IO;
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
        /// T 型インスタンスの集合のうち条件を満たす要素数を返す
        /// </summary>
        /// <typeparam name="T">任意の型</typeparam>
        /// <param name="set">T 型インスタンスの列挙可能な集合</param>
        /// <param name="predicator">条件を表すデリゲート</param>
        /// <returns>条件を満たす T 型インスタンスの数</returns>
        public static int CountIf<T>(IEnumerable<T> set, Predicate<T> predicator)
        {
            int count = 0;
            foreach (var element in set)
                if (predicator(element))
                    count++;
            return count;
        }

        /// <summary>
        /// ItemCollection インスタンスから条件を満たす要素を返す
        /// </summary>
        /// <typeparam name="T">items の各要素をこの型と見なす</typeparam>
        /// <param name="items">ItemCollection インスタンス</param>
        /// <param name="predicator">条件を表すデリゲート</param>
        /// <returns>条件を満たす T 型インスタンス</returns>
        public static T Find<T>(ItemCollection items, Predicate<T> predicator)
        {
            foreach (T item in items)
                if (predicator(item))
                    return item;
            return default(T);
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
            private Predicate<T>[] preds;

            public And(params Predicate<T>[] preds)
            {
                this.preds = preds;
            }

            private bool Pred(T t)
            {
                foreach (var pred in this.preds)
                    if (!pred(t))
                        return false;
                return true;
            }

            public static implicit operator Predicate<T>(And<T> from)
            {
                return from.Pred;
            }
        }
    }
}
