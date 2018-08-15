using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th08AllScoreDataWrapper
    {
        private static Type ParentType = typeof(Th08Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+AllScoreData";

        private readonly PrivateObject pobj = null;

        public Th08AllScoreDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;

        public Th06HeaderWrapper<Th08Converter> Header
        {
            get
            {
                var header = this.pobj.GetProperty(nameof(Header));
                return (header != null) ? new Th06HeaderWrapper<Th08Converter>(header) : null;
            }
        }

        // NOTE: Th08Converter.{CharaLevelPair,HighScore} are private classes.
        // public IReadOnlyDictionary<CharaLevelPair, List<HighScore>> Rankings
        //     => this.pobj.GetProperty(nameof(Rankings)) as Dictionary<CharaLevelPair, List<HighScore>>;
        public object Rankings
            => this.pobj.GetProperty(nameof(Rankings));
        public int? RankingsCount
            => this.Rankings.GetType().GetProperty("Count").GetValue(this.Rankings) as int?;
        public object Ranking(Th06CharaLevelPairWrapper<Th08Converter, Th08Converter.Chara, ThConverter.Level> pair)
            => this.Rankings.GetType().GetProperty("Item").GetValue(this.Rankings, new object[] { pair.Target });
        public Th08HighScoreWrapper RankingItem(
            Th06CharaLevelPairWrapper<Th08Converter, Th08Converter.Chara, ThConverter.Level> pair, int index)
        {
            var ranking = this.Ranking(pair);
            return new Th08HighScoreWrapper(
                ranking.GetType().GetProperty("Item").GetValue(ranking, new object[] { index }));
        }

        // NOTE: Th08Converter.ClearData is a private class.
        // public IReadOnlyDictionary<Chara, ClearData> ClearData
        //     => this.pobj.GetProperty(nameof(ClearData)) as Dictionary<Chara, ClearData>;
        public object ClearData
            => this.pobj.GetProperty(nameof(ClearData));
        public int? ClearDataCount
            => this.ClearData.GetType().GetProperty("Count").GetValue(this.ClearData) as int?;
        public Th08ClearDataWrapper ClearDataItem(Th08Converter.CharaWithTotal chara)
            => new Th08ClearDataWrapper(
                this.ClearData.GetType().GetProperty("Item").GetValue(this.ClearData, new object[] { chara }));

        // NOTE: Th08Converter.CardAttack is a private class.
        // public IReadOnlyDictionary<int, CardAttack> CardAttacks
        //     => this.pobj.GetProperty(nameof(CardAttacks)) as Dictionary<int, CardAttack>;
        public object CardAttacks
            => this.pobj.GetProperty(nameof(CardAttacks));
        public int? CardAttacksCount
            => this.CardAttacks.GetType().GetProperty("Count").GetValue(this.CardAttacks) as int?;
        public Th08CardAttackWrapper CardAttacksItem(int id)
            => new Th08CardAttackWrapper(
                this.CardAttacks.GetType().GetProperty("Item").GetValue(this.CardAttacks, new object[] { id }));

        // NOTE: Th08Converter.PracticeScore is a private class.
        // public IReadOnlyDictionary<Chara, PracticeScore> PracticeScores
        //     => this.pobj.GetProperty(nameof(PracticeScores)) as Dictionary<Chara, PracticeScore>;
        public object PracticeScores
            => this.pobj.GetProperty(nameof(PracticeScores));
        public int? PracticeScoresCount
            => this.PracticeScores.GetType().GetProperty("Count").GetValue(this.PracticeScores) as int?;
        public Th08PracticeScoreWrapper PracticeScoresItem(Th08Converter.Chara chara)
            => new Th08PracticeScoreWrapper(
                this.PracticeScores.GetType().GetProperty("Item").GetValue(
                    this.PracticeScores, new object[] { chara }));

        public Th08FlspWrapper Flsp
        {
            get
            {
                var flsp = this.pobj.GetProperty(nameof(Flsp));
                return (flsp != null) ? new Th08FlspWrapper(flsp) : null;
            }
        }

        public Th08PlayStatusWrapper PlayStatus
        {
            get
            {
                var status = this.pobj.GetProperty(nameof(PlayStatus));
                return (status != null) ? new Th08PlayStatusWrapper(status) : null;
            }
        }

        public Th07LastNameWrapper<Th08Converter> LastName
        {
            get
            {
                var name = this.pobj.GetProperty(nameof(LastName));
                return (name != null) ? new Th07LastNameWrapper<Th08Converter>(name) : null;
            }
        }

        public Th07VersionInfoWrapper<Th08Converter> VersionInfo
        {
            get
            {
                var info = this.pobj.GetProperty(nameof(VersionInfo));
                return (info != null) ? new Th07VersionInfoWrapper<Th08Converter>(info) : null;
            }
        }

        public void Set(Th06HeaderWrapper<Th08Converter> header)
            => this.pobj.Invoke(nameof(Set), new object[] { header.Target }, CultureInfo.InvariantCulture);
        public void Set(Th08HighScoreWrapper score)
            => this.pobj.Invoke(nameof(Set), new object[] { score.Target }, CultureInfo.InvariantCulture);
        public void Set(Th08ClearDataWrapper data)
            => this.pobj.Invoke(nameof(Set), new object[] { data.Target }, CultureInfo.InvariantCulture);
        public void Set(Th08CardAttackWrapper attack)
            => this.pobj.Invoke(nameof(Set), new object[] { attack.Target }, CultureInfo.InvariantCulture);
        public void Set(Th08PracticeScoreWrapper score)
            => this.pobj.Invoke(nameof(Set), new object[] { score.Target }, CultureInfo.InvariantCulture);
        public void Set(Th08FlspWrapper flsp)
            => this.pobj.Invoke(nameof(Set), new object[] { flsp.Target }, CultureInfo.InvariantCulture);
        public void Set(Th08PlayStatusWrapper status)
            => this.pobj.Invoke(nameof(Set), new object[] { status.Target }, CultureInfo.InvariantCulture);
        public void Set(Th07LastNameWrapper<Th08Converter> name)
            => this.pobj.Invoke(nameof(Set), new object[] { name.Target }, CultureInfo.InvariantCulture);
        public void Set(Th07VersionInfoWrapper<Th08Converter> info)
            => this.pobj.Invoke(nameof(Set), new object[] { info.Target }, CultureInfo.InvariantCulture);
    }
}
