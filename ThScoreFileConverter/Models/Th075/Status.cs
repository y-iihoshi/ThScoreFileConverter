//-----------------------------------------------------------------------
// <copyright file="Status.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ThScoreFileConverter.Models.Th075
{
    internal class Status : IBinaryReadable
    {
        private Dictionary<Chara, IReadOnlyDictionary<Chara, int>> arcadeScores;

        public Status()
        {
        }

        public string LastName { get; private set; }

        public IReadOnlyDictionary<Chara, IReadOnlyDictionary<Chara, int>> ArcadeScores => this.arcadeScores;

        public void ReadFrom(BinaryReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            var charas = Utils.GetEnumerator<Chara>();
            var unknownCharas = Enumerable.Range(1, 4);
            var numScores = charas.Count() + unknownCharas.Count();

            this.LastName = new string(reader.ReadExactBytes(8).Select(ch => Definitions.CharTable[ch]).ToArray());

            this.arcadeScores = new Dictionary<Chara, IReadOnlyDictionary<Chara, int>>(numScores);
            foreach (var chara in charas)
            {
                this.arcadeScores[chara] = charas.ToDictionary(enemy => enemy, _ => reader.ReadInt32() - 10);
                _ = unknownCharas.Select(_ => reader.ReadInt32()).ToList();
            }

            foreach (var unknownChara in unknownCharas)
            {
                _ = charas.Select(_ => reader.ReadInt32()).ToList();
                _ = unknownCharas.Select(_ => reader.ReadInt32()).ToList();
            }

            // FIXME... BGM flags?
            reader.ReadExactBytes(0x28);

            reader.ReadExactBytes(0x100);
        }
    }
}
