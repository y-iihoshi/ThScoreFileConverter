//using System;

namespace ThScoreFileConverter
{
    /// <summary>
    /// 2 つの型のインスタンスのペア
    /// </summary>
    /// <typeparam name="Type1">First プロパティの型</typeparam>
    /// <typeparam name="Type2">Second プロパティの型</typeparam>
    public class Pair<Type1, Type2>
    {
        protected Type1 First { get; private set; }
        protected Type2 Second { get; private set; }

        protected Pair(Type1 first, Type2 second)
        {
            this.First = first;
            this.Second = second;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var target = (Pair<Type1, Type2>)obj;
            return (this.First.Equals(target.First) && this.Second.Equals(target.Second));
        }

        public override int GetHashCode()
        {
            return this.First.GetHashCode() ^ this.Second.GetHashCode();
        }
    }
}
