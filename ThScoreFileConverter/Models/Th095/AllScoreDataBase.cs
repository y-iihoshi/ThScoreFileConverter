//-----------------------------------------------------------------------
// <copyright file="AllScoreDataBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th095
{
    internal class AllScoreDataBase<TScore, TStatus>
        where TScore : IChapter
        where TStatus : IStatus
    {
        private readonly List<TScore> scores;

        protected AllScoreDataBase(int numCards)
        {
            this.scores = new List<TScore>(numCards);
        }

        public HeaderBase? Header { get; private set; }

        public IReadOnlyList<TScore> Scores => this.scores;

        public TStatus? Status { get; private set; }

        public void Set(HeaderBase header)
        {
            this.Header = header;
        }

        public void Set(TScore score)
        {
            this.scores.Add(score);
        }

        public void Set(TStatus status)
        {
            this.Status = status;
        }
    }
}
