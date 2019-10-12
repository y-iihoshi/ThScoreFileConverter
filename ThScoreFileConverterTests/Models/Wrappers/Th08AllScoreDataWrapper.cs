using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using LastName = ThScoreFileConverter.Models.Th07.LastName;
using VersionInfo = ThScoreFileConverter.Models.Th07.VersionInfo;
using IHighScore = ThScoreFileConverter.Models.Th08.IHighScore<
    ThScoreFileConverter.Models.Th08.Chara,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th08.StageProgress>;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th08AllScoreDataWrapper
    {
        private static readonly Type ParentType = typeof(Th08Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+AllScoreData";

        private readonly PrivateObject pobj = null;

        public Th08AllScoreDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;

        public Header Header
            => this.pobj.GetProperty(nameof(this.Header)) as Header;
        public IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> Rankings
            => this.pobj.GetProperty(nameof(this.Rankings))
                as IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>>;
        public IReadOnlyDictionary<CharaWithTotal, IClearData> ClearData
            => this.pobj.GetProperty(nameof(this.ClearData)) as IReadOnlyDictionary<CharaWithTotal, IClearData>;

        // NOTE: Th08Converter.CardAttack is a private class.
        // public IReadOnlyDictionary<int, CardAttack> CardAttacks
        //     => this.pobj.GetProperty(nameof(this.CardAttacks)) as Dictionary<int, CardAttack>;
        public object CardAttacks
            => this.pobj.GetProperty(nameof(this.CardAttacks));
        public int? CardAttacksCount
            => this.CardAttacks.GetType().GetProperty("Count").GetValue(this.CardAttacks) as int?;
        public Th08CardAttackWrapper CardAttacksItem(int id)
            => new Th08CardAttackWrapper(
                this.CardAttacks.GetType().GetProperty("Item").GetValue(this.CardAttacks, new object[] { id }));

        // NOTE: Th08Converter.PracticeScore is a private class.
        // public IReadOnlyDictionary<Chara, PracticeScore> PracticeScores
        //     => this.pobj.GetProperty(nameof(this.PracticeScores)) as Dictionary<Chara, PracticeScore>;
        public object PracticeScores
            => this.pobj.GetProperty(nameof(this.PracticeScores));
        public int? PracticeScoresCount
            => this.PracticeScores.GetType().GetProperty("Count").GetValue(this.PracticeScores) as int?;
        public Th08PracticeScoreWrapper PracticeScoresItem(Chara chara)
            => new Th08PracticeScoreWrapper(
                this.PracticeScores.GetType().GetProperty("Item").GetValue(
                    this.PracticeScores, new object[] { chara }));

        public Th08FlspWrapper Flsp
        {
            get
            {
                var flsp = this.pobj.GetProperty(nameof(this.Flsp));
                return (flsp != null) ? new Th08FlspWrapper(flsp) : null;
            }
        }

        public Th08PlayStatusWrapper PlayStatus
        {
            get
            {
                var status = this.pobj.GetProperty(nameof(this.PlayStatus));
                return (status != null) ? new Th08PlayStatusWrapper(status) : null;
            }
        }

        public LastName LastName
            => this.pobj.GetProperty(nameof(this.LastName)) as LastName;
        public VersionInfo VersionInfo
            => this.pobj.GetProperty(nameof(this.VersionInfo)) as VersionInfo;

        public void Set(Header header)
            => this.pobj.Invoke(nameof(Set), new object[] { header }, CultureInfo.InvariantCulture);
        public void Set(IHighScore score)
            => this.pobj.Invoke(nameof(Set), new object[] { score }, CultureInfo.InvariantCulture);
        public void Set(IClearData data)
            => this.pobj.Invoke(nameof(Set), new object[] { data }, CultureInfo.InvariantCulture);
        public void Set(Th08CardAttackWrapper attack)
            => this.pobj.Invoke(nameof(Set), new object[] { attack.Target }, CultureInfo.InvariantCulture);
        public void Set(Th08PracticeScoreWrapper score)
            => this.pobj.Invoke(nameof(Set), new object[] { score.Target }, CultureInfo.InvariantCulture);
        public void Set(Th08FlspWrapper flsp)
            => this.pobj.Invoke(nameof(Set), new object[] { flsp.Target }, CultureInfo.InvariantCulture);
        public void Set(Th08PlayStatusWrapper status)
            => this.pobj.Invoke(nameof(Set), new object[] { status.Target }, CultureInfo.InvariantCulture);
        public void Set(LastName name)
            => this.pobj.Invoke(nameof(Set), new object[] { name }, CultureInfo.InvariantCulture);
        public void Set(VersionInfo info)
            => this.pobj.Invoke(nameof(Set), new object[] { info }, CultureInfo.InvariantCulture);
    }
}
