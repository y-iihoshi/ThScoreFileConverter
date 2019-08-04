using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th08HighScoreWrapper
    {
        private static readonly Type ParentType = typeof(Th08Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+HighScore";

        private readonly PrivateObject pobj = null;

        public Th08HighScoreWrapper(ChapterWrapper chapter)
            => this.pobj = new PrivateObject(
                AssemblyNameToTest,
                TypeNameToTest,
                new Type[] { (chapter ?? new ChapterWrapper()).Target.GetType() },
                new object[] { chapter?.Target });
        public Th08HighScoreWrapper(uint score)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { score });
        public Th08HighScoreWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        public object Target
            => this.pobj.Target;
        public string Signature
            => this.pobj.GetProperty(nameof(this.Signature)) as string;
        public short? Size1
            => this.pobj.GetProperty(nameof(this.Size1)) as short?;
        public short? Size2
            => this.pobj.GetProperty(nameof(this.Size2)) as short?;
        public byte? FirstByteOfData
            => this.pobj.GetProperty(nameof(this.FirstByteOfData)) as byte?;
        public IReadOnlyCollection<byte> Data
            => this.pobj.GetProperty(nameof(this.Data)) as byte[];
        public uint? Score
            => this.pobj.GetProperty(nameof(this.Score)) as uint?;
        public float? SlowRate
            => this.pobj.GetProperty(nameof(this.SlowRate)) as float?;
        public Th08Converter.Chara? Chara
            => this.pobj.GetProperty(nameof(this.Chara)) as Th08Converter.Chara?;
        public ThConverter.Level? Level
            => this.pobj.GetProperty(nameof(this.Level)) as ThConverter.Level?;
        public Th08Converter.StageProgress? StageProgress
            => this.pobj.GetProperty(nameof(this.StageProgress)) as Th08Converter.StageProgress?;
        public IReadOnlyCollection<byte> Name
            => this.pobj.GetProperty(nameof(this.Name)) as byte[];
        public IReadOnlyCollection<byte> Date
            => this.pobj.GetProperty(nameof(this.Date)) as byte[];
        public ushort? ContinueCount
            => this.pobj.GetProperty(nameof(this.ContinueCount)) as ushort?;
        public byte? PlayerNum
            => this.pobj.GetProperty(nameof(this.PlayerNum)) as byte?;
        public uint? PlayTime
            => this.pobj.GetProperty(nameof(this.PlayTime)) as uint?;
        public int? PointItem
            => this.pobj.GetProperty(nameof(this.PointItem)) as int?;
        public int? MissCount
            => this.pobj.GetProperty(nameof(this.MissCount)) as int?;
        public int? BombCount
            => this.pobj.GetProperty(nameof(this.BombCount)) as int?;
        public int? LastSpellCount
            => this.pobj.GetProperty(nameof(this.LastSpellCount)) as int?;
        public int? PauseCount
            => this.pobj.GetProperty(nameof(this.PauseCount)) as int?;
        public int? TimePoint
            => this.pobj.GetProperty(nameof(this.TimePoint)) as int?;
        public int? HumanRate
            => this.pobj.GetProperty(nameof(this.HumanRate)) as int?;
        public IReadOnlyDictionary<int, byte> CardFlags
            => this.pobj.GetProperty(nameof(this.CardFlags)) as Dictionary<int, byte>;
    }
}
