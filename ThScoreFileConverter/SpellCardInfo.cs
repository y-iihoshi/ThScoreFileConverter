//-----------------------------------------------------------------------
// <copyright file="SpellCardInfo.cs" company="None">
//     (c) 2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter
{
    using System;

    /// <summary>
    /// Indicates information of a spell card.
    /// </summary>
    /// <typeparam name="TStage">An enumeration type of the stage.</typeparam>
    /// <typeparam name="TLevel">An enumeration type of the level.</typeparam>
    internal class SpellCardInfo<TStage, TLevel>
        where TStage : struct, IComparable, IFormattable, IConvertible
        where TLevel : struct, IComparable, IFormattable, IConvertible
    {
        /// <summary>
        /// Indicates the level(s) which the current spell card is used.
        /// </summary>
        private TLevel[] levels;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellCardInfo{TStage,TLevel}"/> class.
        /// </summary>
        /// <param name="id">A 1-based sequential number of the spell card.</param>
        /// <param name="name">A name of the spell card.</param>
        /// <param name="stage">The stage which the spell card is used.</param>
        /// <param name="levels">The level(s) which the spell card is used.</param>
        public SpellCardInfo(int id, string name, TStage stage, params TLevel[] levels)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("number");

            this.Id = id;
            this.Name = name;
            this.Stage = stage;
            this.levels = levels;
        }

        /// <summary>
        /// Gets a 1-based sequential number of the current spell card.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets a name of the current spell card.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the stage which the current spell card is used.
        /// </summary>
        public TStage Stage { get; private set; }

        /// <summary>
        /// Gets the level which the current spell card is used.
        /// </summary>
        public TLevel Level
        {
            get { return this.levels[0]; }
        }

        /// <summary>
        /// Gets the levels which the current spell card is used.
        /// </summary>
        /// <remarks>This is for TH06 only.</remarks>
        public TLevel[] Levels
        {
            get { return this.levels; }
        }
    }
}
