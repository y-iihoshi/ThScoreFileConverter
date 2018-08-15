using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th08CardAttackWrapper
    {
        private static Type ParentType = typeof(Th08Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+CardAttack";

        private readonly PrivateObject pobj = null;

        public Th08CardAttackWrapper(Th06ChapterWrapper<Th08Converter> chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th08CardAttackWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        public object Target
            => this.pobj.Target;
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
        public short? CardId
            => this.pobj.GetProperty(nameof(CardId)) as short?;
        public Th08Converter.LevelPracticeWithTotal? Level
            => this.pobj.GetProperty(nameof(Level)) as Th08Converter.LevelPracticeWithTotal?;
        public IReadOnlyCollection<byte> CardName
            => this.pobj.GetProperty(nameof(CardName)) as byte[];
        public IReadOnlyCollection<byte> EnemyName
            => this.pobj.GetProperty(nameof(EnemyName)) as byte[];
        public IReadOnlyCollection<byte> Comment
            => this.pobj.GetProperty(nameof(Comment)) as byte[];
        public Th08CardAttackCareerWrapper StoryCareer
            => new Th08CardAttackCareerWrapper(this.pobj.GetProperty(nameof(StoryCareer)));
        public Th08CardAttackCareerWrapper PracticeCareer
            => new Th08CardAttackCareerWrapper(this.pobj.GetProperty(nameof(PracticeCareer)));

        public bool? HasTried()
            => this.pobj.Invoke(nameof(HasTried), new object[] { }, CultureInfo.InvariantCulture) as bool?;
    }
}
