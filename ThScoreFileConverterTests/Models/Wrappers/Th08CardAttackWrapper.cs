using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th08CardAttackWrapper
    {
        private static readonly Type ParentType = typeof(Th08Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+CardAttack";

        private readonly PrivateObject pobj = null;

        public Th08CardAttackWrapper(ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th08CardAttackWrapper(object original)
            => this.pobj = new PrivateObject(original);

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
        public short? CardId
            => this.pobj.GetProperty(nameof(this.CardId)) as short?;
        public LevelPracticeWithTotal? Level
            => this.pobj.GetProperty(nameof(this.Level)) as LevelPracticeWithTotal?;
        public IEnumerable<byte> CardName
            => this.pobj.GetProperty(nameof(this.CardName)) as IEnumerable<byte>;
        public IEnumerable<byte> EnemyName
            => this.pobj.GetProperty(nameof(this.EnemyName)) as IEnumerable<byte>;
        public IEnumerable<byte> Comment
            => this.pobj.GetProperty(nameof(this.Comment)) as IEnumerable<byte>;
        public ICardAttackCareer StoryCareer
            => this.pobj.GetProperty(nameof(this.StoryCareer)) as ICardAttackCareer;
        public ICardAttackCareer PracticeCareer
            => this.pobj.GetProperty(nameof(this.PracticeCareer)) as ICardAttackCareer;

        public bool? HasTried()
            => this.pobj.Invoke(nameof(HasTried), new object[] { }, CultureInfo.InvariantCulture) as bool?;
    }
}
