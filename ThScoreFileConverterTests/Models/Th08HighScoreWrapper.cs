using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Tests
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th08HighScoreWrapper
    {
        private static Type ParentType = typeof(Th08Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+HighScore";

        private PrivateObject pobj = null;

        public Th08HighScoreWrapper(Th06ChapterWrapper<Th08Converter> chapter)
        {
            if (chapter == null)
            {
                var ch = new Th06ChapterWrapper<Th08Converter>();
                this.pobj = new PrivateObject(
                    AssemblyNameToTest,
                    TypeNameToTest,
                    new Type[] { ch.Target.GetType() },
                    new object[] { null });
            }
            else
            {
                this.pobj = new PrivateObject(
                    AssemblyNameToTest,
                    TypeNameToTest,
                    new Type[] { chapter.Target.GetType() },
                    new object[] { chapter.Target });
            }
        }

        public Th08HighScoreWrapper(uint score)
        {
            this.pobj = new PrivateObject(
                AssemblyNameToTest,
                TypeNameToTest,
                new Type[] { score.GetType() },
                new object[] { score });
        }

        // NOTE: Enabling the following causes CA1811.
        // public object Target => this.pobj.Target;

        public string Signature
            => this.pobj.GetProperty(nameof(Signature)) as string;
        public short? Size1
            => this.pobj.GetProperty(nameof(Size1)) as short?;
        public short? Size2
            => this.pobj.GetProperty(nameof(Size2)) as short?;
        public byte? FirstByteOfData
            => this.pobj.GetProperty(nameof(FirstByteOfData)) as byte?;
        public IReadOnlyCollection<byte> Data
            => this.pobj.GetProperty(nameof(Data)) as byte[];
        public uint? Score
            => this.pobj.GetProperty(nameof(Score)) as uint?;
        public float? SlowRate
            => this.pobj.GetProperty(nameof(SlowRate)) as float?;
        public Th08Converter.Chara? Chara
            => this.pobj.GetProperty(nameof(Chara)) as Th08Converter.Chara?;
        public ThConverter.Level? Level
            => this.pobj.GetProperty(nameof(Level)) as ThConverter.Level?;
        public Th08Converter.StageProgress? StageProgress
            => this.pobj.GetProperty(nameof(StageProgress)) as Th08Converter.StageProgress?;
        public IReadOnlyCollection<byte> Name
            => this.pobj.GetProperty(nameof(Name)) as byte[];
        public IReadOnlyCollection<byte> Date
            => this.pobj.GetProperty(nameof(Date)) as byte[];
        public ushort? ContinueCount
            => this.pobj.GetProperty(nameof(ContinueCount)) as ushort?;
        public byte? PlayerNum
            => this.pobj.GetProperty(nameof(PlayerNum)) as byte?;
        public uint? PlayTime
            => this.pobj.GetProperty(nameof(PlayTime)) as uint?;
        public int? PointItem
            => this.pobj.GetProperty(nameof(PointItem)) as int?;
        public int? MissCount
            => this.pobj.GetProperty(nameof(MissCount)) as int?;
        public int? BombCount
            => this.pobj.GetProperty(nameof(BombCount)) as int?;
        public int? LastSpellCount
            => this.pobj.GetProperty(nameof(LastSpellCount)) as int?;
        public int? PauseCount
            => this.pobj.GetProperty(nameof(PauseCount)) as int?;
        public int? TimePoint
            => this.pobj.GetProperty(nameof(TimePoint)) as int?;
        public int? HumanRate
            => this.pobj.GetProperty(nameof(HumanRate)) as int?;
        public IReadOnlyDictionary<int, byte> CardFlags
            => this.pobj.GetProperty(nameof(CardFlags)) as Dictionary<int, byte>;
    }
}
