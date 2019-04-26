//-----------------------------------------------------------------------
// <copyright file="Encoding.cs" company="None">
//     (c) 2019 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models
{
    /// <summary>
    /// Contains read-only instances of <see cref="System.Text.Encoding"/> class for convenience.
    /// </summary>
    public static class Encoding
    {
        /// <summary>
        /// Gets the code page 932 encoding.
        /// </summary>
        public static System.Text.Encoding CP932 => System.Text.Encoding.GetEncoding(932);

        /// <summary>
        /// Gets the default encoding.
        /// </summary>
        public static System.Text.Encoding Default => System.Text.Encoding.Default;

        /// <summary>
        /// Gets the UTF-8 encoding. The Unicode byte order mark is omitted.
        /// </summary>
        public static System.Text.Encoding UTF8 => new System.Text.UTF8Encoding(false);
    }
}
