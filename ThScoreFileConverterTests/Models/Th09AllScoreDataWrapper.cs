using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th09AllScoreDataWrapper
    {
        private static Type ParentType = typeof(Th09Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+AllScoreData";

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
                var header = this.pobj.GetProperty(nameof(Header));
                return (header != null) ? new Th06HeaderWrapper<Th09Converter>(header) : null;
            }
        }

        // NOTE: Th09Converter.{CharaLevelPair,HighScore} are private classes.
        // public IReadOnlyDictionary<CharaLevelPair, HighScore[]> Rankings
        //     => this.pobj.GetProperty(nameof(Rankings)) as Dictionary<CharaLevelPair, HighScore[]>;
        public object Rankings
            => this.pobj.GetProperty(nameof(Rankings));
        public int? RankingsCount
            => this.Rankings.GetType().GetProperty("Count").GetValue(this.Rankings) as int?;
        public object[] Ranking(Th06CharaLevelPairWrapper<Th09Converter, Th09Converter.Chara, ThConverter.Level> pair)
            => this.Rankings.GetType().GetProperty("Item").GetValue(this.Rankings, new object[] { pair.Target })
                as object[];
        public Th09HighScoreWrapper RankingItem(
            Th06CharaLevelPairWrapper<Th09Converter, Th09Converter.Chara, ThConverter.Level> pair, int index)
        {
            var item = this.Ranking(pair)[index];
            return (item != null) ? new Th09HighScoreWrapper(item) : null;
        }

        public Th09PlayStatusWrapper PlayStatus
        {
            get
            {
                var status = this.pobj.GetProperty(nameof(PlayStatus));
                return (status != null) ? new Th09PlayStatusWrapper(status) : null;
            }
        }

        public Th07LastNameWrapper<Th09Converter> LastName
        {
            get
            {
                var name = this.pobj.GetProperty(nameof(LastName));
                return (name != null) ? new Th07LastNameWrapper<Th09Converter>(name) : null;
            }
        }

        public Th07VersionInfoWrapper<Th09Converter> VersionInfo
        {
            get
            {
                var info = this.pobj.GetProperty(nameof(VersionInfo));
                return (info != null) ? new Th07VersionInfoWrapper<Th09Converter>(info) : null;
            }
        }

        public void Set(Th06HeaderWrapper<Th09Converter> header)
            => this.pobj.Invoke(nameof(Set), new object[] { header.Target }, CultureInfo.InvariantCulture);
        public void Set(Th09HighScoreWrapper score)
            => this.pobj.Invoke(nameof(Set), new object[] { score.Target }, CultureInfo.InvariantCulture);
        public void Set(Th09PlayStatusWrapper status)
            => this.pobj.Invoke(nameof(Set), new object[] { status.Target }, CultureInfo.InvariantCulture);
        public void Set(Th07LastNameWrapper<Th09Converter> name)
            => this.pobj.Invoke(nameof(Set), new object[] { name.Target }, CultureInfo.InvariantCulture);
        public void Set(Th07VersionInfoWrapper<Th09Converter> info)
            => this.pobj.Invoke(nameof(Set), new object[] { info.Target }, CultureInfo.InvariantCulture);
    }
}
