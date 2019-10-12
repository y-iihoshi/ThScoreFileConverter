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
        public IReadOnlyDictionary<int, ICardAttack> CardAttacks
            => this.pobj.GetProperty(nameof(this.CardAttacks)) as IReadOnlyDictionary<int, ICardAttack>;
        public IReadOnlyDictionary<Chara, IPracticeScore> PracticeScores
            => this.pobj.GetProperty(nameof(this.PracticeScores)) as IReadOnlyDictionary<Chara, IPracticeScore>;
        public FLSP Flsp
            => this.pobj.GetProperty(nameof(this.Flsp)) as FLSP;
        public IPlayStatus PlayStatus
            => this.pobj.GetProperty(nameof(this.PlayStatus)) as IPlayStatus;
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
        public void Set(ICardAttack attack)
            => this.pobj.Invoke(nameof(Set), new object[] { attack }, CultureInfo.InvariantCulture);
        public void Set(IPracticeScore score)
            => this.pobj.Invoke(nameof(Set), new object[] { score }, CultureInfo.InvariantCulture);
        public void Set(FLSP flsp)
            => this.pobj.Invoke(nameof(Set), new object[] { flsp }, CultureInfo.InvariantCulture);
        public void Set(IPlayStatus status)
            => this.pobj.Invoke(nameof(Set), new object[] { status }, CultureInfo.InvariantCulture);
        public void Set(LastName name)
            => this.pobj.Invoke(nameof(Set), new object[] { name }, CultureInfo.InvariantCulture);
        public void Set(VersionInfo info)
            => this.pobj.Invoke(nameof(Set), new object[] { info }, CultureInfo.InvariantCulture);
    }
}
