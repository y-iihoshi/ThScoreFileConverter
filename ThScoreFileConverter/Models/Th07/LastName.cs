//-----------------------------------------------------------------------
// <copyright file="LastName.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th07
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using ThScoreFileConverter.Properties;

    internal class LastName : Th06.Chapter
    {
        public const string ValidSignature = "LSNM";
        public const short ValidSize = 0x0018;

        public LastName(Th06.Chapter chapter)
            : base(chapter)
        {
            if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
            {
                throw new InvalidDataException(
                    Utils.Format(Resources.InvalidDataExceptionPropertyIsInvalid, nameof(this.Signature)));
            }

            if (this.Size1 != ValidSize)
            {
                throw new InvalidDataException(
                    Utils.Format(Resources.InvalidDataExceptionPropertyIsInvalid, nameof(this.Size1)));
            }

            using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
            {
                reader.ReadUInt32();    // always 0x00000001?
                this.Name = reader.ReadExactBytes(12);
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
        public byte[] Name { get; private set; }    // .Length = 12, null-terminated
    }
}
