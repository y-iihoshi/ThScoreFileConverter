using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th07;
using Level = ThScoreFileConverter.Models.Th07.Level;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th07AllScoreDataWrapper
    {
        private static readonly Type ParentType = typeof(Th07Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+AllScoreData";

        private readonly PrivateObject pobj = null;

        public Th07AllScoreDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;

        public Header Header
            => this.pobj.GetProperty(nameof(this.Header)) as Header;
        public IReadOnlyDictionary<(Chara, Level), List<HighScore>> Rankings
            => this.pobj.GetProperty(nameof(this.Rankings)) as Dictionary<(Chara, Level), List<HighScore>>;
        public IReadOnlyDictionary<Chara, ClearData> ClearData
            => this.pobj.GetProperty(nameof(this.ClearData)) as Dictionary<Chara, ClearData>;
        public IReadOnlyDictionary<int, CardAttack> CardAttacks
            => this.pobj.GetProperty(nameof(this.CardAttacks)) as Dictionary<int, CardAttack>;
        public IReadOnlyDictionary<(Chara, Level), Dictionary<Th07Converter.Stage, PracticeScore>> PracticeScores
            => this.pobj.GetProperty(nameof(this.PracticeScores))
                as Dictionary<(Chara, Level), Dictionary<Th07Converter.Stage, PracticeScore>>;
        public PlayStatus PlayStatus
            => this.pobj.GetProperty(nameof(this.PlayStatus)) as PlayStatus;
        public LastName LastName
            => this.pobj.GetProperty(nameof(this.LastName)) as LastName;
        public VersionInfo VersionInfo
            => this.pobj.GetProperty(nameof(this.VersionInfo)) as VersionInfo;

        public void Set(Header header)
            => this.pobj.Invoke(nameof(Set), new object[] { header }, CultureInfo.InvariantCulture);
        public void Set(HighScore score)
            => this.pobj.Invoke(nameof(Set), new object[] { score }, CultureInfo.InvariantCulture);
        public void Set(ClearData data)
            => this.pobj.Invoke(nameof(Set), new object[] { data }, CultureInfo.InvariantCulture);
        public void Set(CardAttack attack)
            => this.pobj.Invoke(nameof(Set), new object[] { attack }, CultureInfo.InvariantCulture);
        public void Set(PracticeScore score)
            => this.pobj.Invoke(nameof(Set), new object[] { score }, CultureInfo.InvariantCulture);
        public void Set(PlayStatus status)
            => this.pobj.Invoke(nameof(Set), new object[] { status }, CultureInfo.InvariantCulture);
        public void Set(LastName name)
            => this.pobj.Invoke(nameof(Set), new object[] { name }, CultureInfo.InvariantCulture);
        public void Set(VersionInfo info)
            => this.pobj.Invoke(nameof(Set), new object[] { info }, CultureInfo.InvariantCulture);
    }
}
