using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Models.Th07.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th09AllScoreDataWrapper
    {
        private static readonly Type ParentType = typeof(Th09Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+AllScoreData";

        private readonly PrivateObject pobj = null;

        public Th09AllScoreDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;

        public Th06HeaderWrapper<Th09Converter> Header
        {
            get
            {
                var header = this.pobj.GetProperty(nameof(this.Header));
                return (header != null) ? new Th06HeaderWrapper<Th09Converter>(header) : null;
            }
        }

        // NOTE: Th09Converter.HighScore are private classes.
        // public IReadOnlyDictionary<(Chara, Level), HighScore[]> Rankings
        //     => this.pobj.GetProperty(nameof(this.Rankings)) as Dictionary<(Chara, Level), HighScore[]>;
        public object Rankings
            => this.pobj.GetProperty(nameof(this.Rankings));
        public int? RankingsCount
            => this.Rankings.GetType().GetProperty("Count").GetValue(this.Rankings) as int?;
        public object[] Ranking(Th09Converter.Chara chara, ThConverter.Level level)
            => this.Rankings.GetType().GetProperty("Item").GetValue(this.Rankings, new object[] { (chara, level) })
                as object[];
        public Th09HighScoreWrapper RankingItem(Th09Converter.Chara chara, ThConverter.Level level, int index)
        {
            var item = this.Ranking(chara, level)[index];
            return (item != null) ? new Th09HighScoreWrapper(item) : null;
        }

        public Th09PlayStatusWrapper PlayStatus
        {
            get
            {
                var status = this.pobj.GetProperty(nameof(this.PlayStatus));
                return (status != null) ? new Th09PlayStatusWrapper(status) : null;
            }
        }

        public LastName LastName
            => this.pobj.GetProperty(nameof(this.LastName)) as LastName;

        public VersionInfoWrapper VersionInfo
        {
            get
            {
                var info = this.pobj.GetProperty(nameof(this.VersionInfo));
                return (info != null) ? new VersionInfoWrapper(info) : null;
            }
        }

        public void Set(Th06HeaderWrapper<Th09Converter> header)
            => this.pobj.Invoke(nameof(Set), new object[] { header.Target }, CultureInfo.InvariantCulture);
        public void Set(Th09HighScoreWrapper score)
            => this.pobj.Invoke(nameof(Set), new object[] { score.Target }, CultureInfo.InvariantCulture);
        public void Set(Th09PlayStatusWrapper status)
            => this.pobj.Invoke(nameof(Set), new object[] { status.Target }, CultureInfo.InvariantCulture);
        public void Set(LastName name)
            => this.pobj.Invoke(nameof(Set), new object[] { name }, CultureInfo.InvariantCulture);
        public void Set(VersionInfoWrapper info)
            => this.pobj.Invoke(nameof(Set), new object[] { info.Target }, CultureInfo.InvariantCulture);
    }
}
