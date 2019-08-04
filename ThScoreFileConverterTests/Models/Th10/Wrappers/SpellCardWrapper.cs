﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;

namespace ThScoreFileConverterTests.Models.Th10.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0053 and CS0703.
    internal sealed class SpellCardWrapper
    {
        private readonly SpellCard original = null;

        public static SpellCardWrapper Create(byte[] array)
        {
            var spellCard = new SpellCardWrapper();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    spellCard.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return spellCard;
        }

        public SpellCardWrapper() => this.original = new SpellCard();

        public object Target => this.original;
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public IReadOnlyCollection<byte> Name => this.original.Name;
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public int? ClearCount => this.original.ClearCount;
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public int? TrialCount => this.original.TrialCount;
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public int? Id => this.original.Id;
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public ThConverter.Level? Level => this.original.Level;
        public bool? HasTried => this.original.HasTried;

        public void ReadFrom(BinaryReader reader) => this.original.ReadFrom(reader);
    }
}
