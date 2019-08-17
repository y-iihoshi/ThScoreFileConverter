﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th06AllScoreDataWrapper
    {
        private static readonly Type ParentType = typeof(Th06Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+AllScoreData";

        private readonly PrivateObject pobj = null;

        public Th06AllScoreDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;

        public Header Header
            => this.pobj.GetProperty(nameof(this.Header)) as Header;
        public IReadOnlyDictionary<(Th06Converter.Chara, ThConverter.Level), List<HighScore>> Rankings
            => this.pobj.GetProperty(nameof(this.Rankings))
                as Dictionary<(Th06Converter.Chara, ThConverter.Level), List<HighScore>>;

        // NOTE: Th06Converter.ClearData is a private class.
        // public IReadOnlyDictionary<Chara, ClearData> ClearData
        //     => this.pobj.GetProperty(nameof(this.ClearData)) as Dictionary<Chara, ClearData>;
        public object ClearData
            => this.pobj.GetProperty(nameof(this.ClearData));
        public int? ClearDataCount
            => this.ClearData.GetType().GetProperty("Count").GetValue(this.ClearData) as int?;
        public Th06ClearDataWrapper ClearDataItem(Th06Converter.Chara chara)
            => new Th06ClearDataWrapper(
                this.ClearData.GetType().GetProperty("Item").GetValue(this.ClearData, new object[] { chara }));

        // NOTE: Th06Converter.CardAttack is a private class.
        // public IReadOnlyDictionary<int, CardAttack> CardAttacks
        //     => this.pobj.GetProperty(nameof(this.CardAttacks)) as Dictionary<int, CardAttack>;
        public object CardAttacks
            => this.pobj.GetProperty(nameof(this.CardAttacks));
        public int? CardAttacksCount
            => this.CardAttacks.GetType().GetProperty("Count").GetValue(this.CardAttacks) as int?;
        public Th06CardAttackWrapper CardAttacksItem(int id)
            => new Th06CardAttackWrapper(
                this.CardAttacks.GetType().GetProperty("Item").GetValue(this.CardAttacks, new object[] { id }));

        // NOTE: Th06Converter.PracticeScore are private classes.
        // public IReadOnlyDictionary<(Chara, Level), Dictionary<Stage, PracticeScore>> PracticeScores
        //     => this.pobj.GetProperty(nameof(this.PracticeScores))
        //         as Dictionary<(Chara, Level), Dictionary<Stage, PracticeScore>>;
        public object PracticeScores
            => this.pobj.GetProperty(nameof(this.PracticeScores));
        public int? PracticeScoresCount
            => this.PracticeScores.GetType().GetProperty("Count").GetValue(this.PracticeScores) as int?;
        public object PracticeScoresPerCharaLevelPair(Th06Converter.Chara chara, ThConverter.Level level)
            => this.PracticeScores.GetType().GetProperty("Item").GetValue(
                this.PracticeScores, new object[] { (chara, level) });
        public Th06PracticeScoreWrapper PracticeScore(
            Th06Converter.Chara chara, ThConverter.Level level, ThConverter.Stage stage)
        {
            var scoresPerPair = this.PracticeScoresPerCharaLevelPair(chara, level);
            return new Th06PracticeScoreWrapper(
                scoresPerPair.GetType().GetProperty("Item").GetValue(scoresPerPair, new object[] { stage }));
        }

        public void Set(Header header)
            => this.pobj.Invoke(nameof(Set), new object[] { header }, CultureInfo.InvariantCulture);
        public void Set(HighScore score)
            => this.pobj.Invoke(nameof(Set), new object[] { score }, CultureInfo.InvariantCulture);
        public void Set(Th06ClearDataWrapper data)
            => this.pobj.Invoke(nameof(Set), new object[] { data.Target }, CultureInfo.InvariantCulture);
        public void Set(Th06CardAttackWrapper attack)
            => this.pobj.Invoke(nameof(Set), new object[] { attack.Target }, CultureInfo.InvariantCulture);
        public void Set(Th06PracticeScoreWrapper score)
            => this.pobj.Invoke(nameof(Set), new object[] { score.Target }, CultureInfo.InvariantCulture);
    }
}
