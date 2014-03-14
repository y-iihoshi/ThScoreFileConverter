//-----------------------------------------------------------------------
// <copyright file="IBinaryReadable.cs" company="None">
//     (c) 2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter
{
    using System.IO;

    /// <summary>
    /// Defines a method to read from a binary stream.
    /// </summary>
    public interface IBinaryReadable
    {
        /// <summary>
        /// Reads from a stream by using the specified <see cref="BinaryReader"/> instance.
        /// </summary>
        /// <param name="reader">The instance to use.</param>
        void ReadFrom(BinaryReader reader);
    }
}
