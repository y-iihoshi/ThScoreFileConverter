//-----------------------------------------------------------------------
// <copyright file="Pair.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models
{
    using System;

    /// <summary>
    /// Indicates a pair of instances.
    /// </summary>
    /// <typeparam name="TFirst">The type of the <see cref="First"/> property.</typeparam>
    /// <typeparam name="TSecond">The type of the <see cref="Second"/> property.</typeparam>
    public class Pair<TFirst, TSecond> : IEquatable<Pair<TFirst, TSecond>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pair{TFirst,TSecond}"/> class.
        /// </summary>
        /// <param name="first">The value of the first component of the pair.</param>
        /// <param name="second">The value of the second component of the pair.</param>
        protected Pair(TFirst first, TSecond second)
        {
            this.First = first;
            this.Second = second;
        }

        /// <summary>
        /// Gets the value of the first component of the pair.
        /// </summary>
        protected TFirst First { get; private set; }

        /// <summary>
        /// Gets the value of the second component of the pair.
        /// </summary>
        protected TSecond Second { get; private set; }

        /// <summary>
        /// Determines whether the specified instance is equal to the current instance.
        /// </summary>
        /// <param name="obj">The instance to compare with the current instance.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="obj"/> is equal to <c>this</c>; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null))
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            return this.Equals(obj as Pair<TFirst, TSecond>);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            return this.First.GetHashCode() ^ this.Second.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified instance is equal to the current instance.
        /// </summary>
        /// <param name="other">The instance to compare with the current instance.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="other"/> is equal to <c>this</c>; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Pair<TFirst, TSecond> other)
        {
            if (object.ReferenceEquals(other, null))
                return false;

            return this.First.Equals(other.First) && this.Second.Equals(other.Second);
        }
    }
}
