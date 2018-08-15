using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th06AllScoreDataWrapper
    {
        private static Type ParentType = typeof(Th06Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+AllScoreData";

        private readonly PrivateObject pobj = null;

        public Th06AllScoreDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;

        public Th06HeaderWrapper<Th06Converter> Header
        {
            get
            {
                var header = this.pobj.GetProperty(nameof(Header));
                return (header != null) ? new Th06HeaderWrapper<Th06Converter>(header) : null;
            }
        }

        // NOTE: Th06Converter.{CharaLevelPair,HighScore} are private classes.
        // public IReadOnlyDictionary<CharaLevelPair, List<HighScore>> Rankings
        //     => this.pobj.GetProperty(nameof(Rankings)) as Dictionary<CharaLevelPair, List<HighScore>>;
        public object Rankings
            => this.pobj.GetProperty(nameof(Rankings));
        public int? RankingsCount
            => this.Rankings.GetType().GetProperty("Count").GetValue(this.Rankings) as int?;
        public object Ranking(Th06CharaLevelPairWrapper<Th06Converter, Th06Converter.Chara, ThConverter.Level> pair)
            => this.Rankings.GetType().GetProperty("Item").GetValue(this.Rankings, new object[] { pair.Target });
        public Th06HighScoreWrapper RankingItem(
            Th06CharaLevelPairWrapper<Th06Converter, Th06Converter.Chara, ThConverter.Level> pair, int index)
        {
            var ranking = this.Ranking(pair);
            return new Th06HighScoreWrapper(
                ranking.GetType().GetProperty("Item").GetValue(ranking, new object[] { index }));
        }

        // NOTE: Th06Converter.ClearData is a private class.
        // public IReadOnlyDictionary<Chara, ClearData> ClearData
        //     => this.pobj.GetProperty(nameof(ClearData)) as Dictionary<Chara, ClearData>;
        public object ClearData
            => this.pobj.GetProperty(nameof(ClearData));
        public int? ClearDataCount
            => this.ClearData.GetType().GetProperty("Count").GetValue(this.ClearData) as int?;
        public Th06ClearDataWrapper ClearDataItem(Th06Converter.Chara chara)
            => new Th06ClearDataWrapper(
                this.ClearData.GetType().GetProperty("Item").GetValue(this.ClearData, new object[] { chara }));

        // NOTE: Th06Converter.CardAttack is a private class.
        // public IReadOnlyDictionary<int, CardAttack> CardAttacks
        //     => this.pobj.GetProperty(nameof(CardAttacks)) as Dictionary<int, CardAttack>;
        public object CardAttacks
            => this.pobj.GetProperty(nameof(CardAttacks));
        public int? CardAttacksCount
            => this.CardAttacks.GetType().GetProperty("Count").GetValue(this.CardAttacks) as int?;
        public Th06CardAttackWrapper CardAttacksItem(int id)
            => new Th06CardAttackWrapper(
                this.CardAttacks.GetType().GetProperty("Item").GetValue(this.CardAttacks, new object[] { id }));

        // NOTE: Th06Converter.{CharaLevelPair,PracticeScore} are private classes.
        // public IReadOnlyDictionary<CharaLevelPair, Dictionary<Stage, PracticeScore>> PracticeScores
        //     => this.pobj.GetProperty(nameof(PracticeScores))
        //         as Dictionary<CharaLevelPair, Dictionary<Stage, PracticeScore>>;
        public object PracticeScores
            => this.pobj.GetProperty(nameof(PracticeScores));
        public int? PracticeScoresCount
            => this.PracticeScores.GetType().GetProperty("Count").GetValue(this.PracticeScores) as int?;
        public object PracticeScoresPerCharaLevelPair(
            Th06CharaLevelPairWrapper<Th06Converter, Th06Converter.Chara, ThConverter.Level> pair)
            => this.PracticeScores.GetType().GetProperty("Item").GetValue(
                this.PracticeScores, new object[] { pair.Target });
        public Th06PracticeScoreWrapper PracticeScore(
            Th06CharaLevelPairWrapper<Th06Converter, Th06Converter.Chara, ThConverter.Level> pair,
            ThConverter.Stage stage)
        {
            var scoresPerPair = this.PracticeScoresPerCharaLevelPair(pair);
            return new Th06PracticeScoreWrapper(
                scoresPerPair.GetType().GetProperty("Item").GetValue(scoresPerPair, new object[] { stage }));
        }

        public void Set(Th06HeaderWrapper<Th06Converter> header)
            => this.pobj.Invoke(nameof(Set), new object[] { header.Target }, CultureInfo.InvariantCulture);
        public void Set(Th06HighScoreWrapper score)
            => this.pobj.Invoke(nameof(Set), new object[] { score.Target }, CultureInfo.InvariantCulture);
        public void Set(Th06ClearDataWrapper data)
            => this.pobj.Invoke(nameof(Set), new object[] { data.Target }, CultureInfo.InvariantCulture);
        public void Set(Th06CardAttackWrapper attack)
            => this.pobj.Invoke(nameof(Set), new object[] { attack.Target }, CultureInfo.InvariantCulture);
        public void Set(Th06PracticeScoreWrapper score)
            => this.pobj.Invoke(nameof(Set), new object[] { score.Target }, CultureInfo.InvariantCulture);
    }
}
