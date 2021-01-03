//-----------------------------------------------------------------------
// <copyright file="AllScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;
using IClearData = ThScoreFileConverter.Models.Th06.IClearData<
    ThScoreFileConverter.Models.Th07.Chara,
    ThScoreFileConverter.Models.Th07.Level>;
using IHighScore = ThScoreFileConverter.Models.Th07.IHighScore<
    ThScoreFileConverter.Models.Th07.Chara,
    ThScoreFileConverter.Models.Th07.Level,
    ThScoreFileConverter.Models.Th07.StageProgress>;

namespace ThScoreFileConverter.Models.Th07
{
    internal class AllScoreData
    {
        private readonly Dictionary<(Chara, Level), IReadOnlyList<IHighScore>> rankings;
        private readonly Dictionary<Chara, IClearData> clearData;
        private readonly Dictionary<int, ICardAttack> cardAttacks;
        private readonly Dictionary<(Chara, Level, Stage), IPracticeScore> practiceScores;

        public AllScoreData()
        {
            var numCharas = EnumHelper<Chara>.NumValues;
            var numPairs = numCharas * EnumHelper<Level>.NumValues;
            this.rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>(numPairs);
            this.clearData = new Dictionary<Chara, IClearData>(numCharas);
            this.cardAttacks = new Dictionary<int, ICardAttack>(Definitions.CardTable.Count);
            this.practiceScores = new Dictionary<(Chara, Level, Stage), IPracticeScore>(numPairs);
        }

        public Header? Header { get; private set; }

        public IReadOnlyDictionary<(Chara Chara, Level Level), IReadOnlyList<IHighScore>> Rankings => this.rankings;

        public IReadOnlyDictionary<Chara, IClearData> ClearData => this.clearData;

        public IReadOnlyDictionary<int, ICardAttack> CardAttacks => this.cardAttacks;

        public IReadOnlyDictionary<(Chara Chara, Level Level, Stage Stage), IPracticeScore> PracticeScores
            => this.practiceScores;

        public PlayStatus? PlayStatus { get; private set; }

        public LastName? LastName { get; private set; }

        public VersionInfo? VersionInfo { get; private set; }

        public void Set(Header header)
        {
            this.Header = header;
        }

        public void Set(IHighScore score)
        {
            var key = (score.Chara, score.Level);
            _ = this.rankings.TryAdd(key, new List<IHighScore>(Definitions.InitialRanking));
            var ranking = this.rankings[key].ToList();
            ranking.Add(score);
            ranking.Sort((lhs, rhs) => rhs.Score.CompareTo(lhs.Score));
            ranking.RemoveAt(ranking.Count - 1);
            this.rankings[key] = ranking;
        }

        public void Set(IClearData data)
        {
            _ = this.clearData.TryAdd(data.Chara, data);
        }

        public void Set(ICardAttack attack)
        {
            _ = this.cardAttacks.TryAdd(attack.CardId, attack);
        }

        public void Set(IPracticeScore score)
        {
            if ((score.Level != Level.Extra) && (score.Level != Level.Phantasm) &&
                (score.Stage != Stage.Extra) && (score.Stage != Stage.Phantasm))
            {
                var key = (score.Chara, score.Level, score.Stage);
                _ = this.practiceScores.TryAdd(key, score);
            }
        }

        public void Set(PlayStatus status)
        {
            this.PlayStatus = status;
        }

        public void Set(LastName name)
        {
            this.LastName = name;
        }

        public void Set(VersionInfo info)
        {
            this.VersionInfo = info;
        }
    }
}
