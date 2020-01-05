//-----------------------------------------------------------------------
// <copyright file="AllScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using IHighScore = ThScoreFileConverter.Models.Th08.IHighScore<
    ThScoreFileConverter.Models.Th08.Chara,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th08.StageProgress>;

namespace ThScoreFileConverter.Models.Th08
{
    internal class AllScoreData
    {
        private readonly Dictionary<(Chara, Level), IReadOnlyList<IHighScore>> rankings;
        private readonly Dictionary<CharaWithTotal, IClearData> clearData;
        private readonly Dictionary<int, ICardAttack> cardAttacks;
        private readonly Dictionary<Chara, IPracticeScore> practiceScores;

        public AllScoreData()
        {
            var numCharas = Enum.GetValues(typeof(Chara)).Length;
            var numPairs = numCharas * Enum.GetValues(typeof(Level)).Length;
            this.rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>(numPairs);
            this.clearData = new Dictionary<CharaWithTotal, IClearData>(Enum.GetValues(typeof(CharaWithTotal)).Length);
            this.cardAttacks = new Dictionary<int, ICardAttack>(Definitions.CardTable.Count);
            this.practiceScores = new Dictionary<Chara, IPracticeScore>(numCharas);
        }

        public Header? Header { get; private set; }

        public IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> Rankings => this.rankings;

        public IReadOnlyDictionary<CharaWithTotal, IClearData> ClearData => this.clearData;

        public IReadOnlyDictionary<int, ICardAttack> CardAttacks => this.cardAttacks;

        public IReadOnlyDictionary<Chara, IPracticeScore> PracticeScores => this.practiceScores;

        public FLSP? Flsp { get; private set; }

        public IPlayStatus? PlayStatus { get; private set; }

        public Th07.LastName? LastName { get; private set; }

        public Th07.VersionInfo? VersionInfo { get; private set; }

        public void Set(Header header)
        {
            this.Header = header;
        }

        public void Set(IHighScore score)
        {
            var key = (score.Chara, score.Level);
            if (!this.rankings.ContainsKey(key))
                this.rankings.Add(key, new List<IHighScore>(Definitions.InitialRanking));
            var ranking = this.rankings[key].ToList();
            ranking.Add(score);
            ranking.Sort((lhs, rhs) => rhs.Score.CompareTo(lhs.Score));
            ranking.RemoveAt(ranking.Count - 1);
            this.rankings[key] = ranking;
        }

        public void Set(IClearData data)
        {
            if (!this.clearData.ContainsKey(data.Chara))
                this.clearData.Add(data.Chara, data);
        }

        public void Set(ICardAttack attack)
        {
            if (!this.cardAttacks.ContainsKey(attack.CardId))
                this.cardAttacks.Add(attack.CardId, attack);
        }

        public void Set(IPracticeScore score)
        {
            if (!this.practiceScores.ContainsKey(score.Chara))
                this.practiceScores.Add(score.Chara, score);
        }

        public void Set(FLSP flsp)
        {
            this.Flsp = flsp;
        }

        public void Set(IPlayStatus status)
        {
            this.PlayStatus = status;
        }

        public void Set(Th07.LastName name)
        {
            this.LastName = name;
        }

        public void Set(Th07.VersionInfo info)
        {
            this.VersionInfo = info;
        }
    }
}
