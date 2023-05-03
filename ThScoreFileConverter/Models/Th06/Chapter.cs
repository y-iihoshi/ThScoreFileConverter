//-----------------------------------------------------------------------
// <copyright file="Chapter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.IO;
using System.Linq;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Resources;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th06;

internal class Chapter : IBinaryReadable, IChapter
{
    public Chapter()
    {
        this.Signature = string.Empty;
        this.Size1 = 0;
        this.Size2 = 0;
        this.Data = Array.Empty<byte>();
    }

    protected Chapter(Chapter chapter)
    {
        this.Signature = chapter.Signature;
        this.Size1 = chapter.Size1;
        this.Size2 = chapter.Size2;
        this.Data = new byte[chapter.Data.Length];
        chapter.Data.CopyTo(this.Data, 0);
    }

    protected Chapter(Chapter chapter, string expectedSignature, short expectedSize)
        : this(chapter)
    {
        if (!this.Signature.Equals(expectedSignature, StringComparison.Ordinal))
        {
            ThrowHelper.ThrowInvalidDataException(
                StringHelper.Format(ExceptionMessages.InvalidDataExceptionPropertyIsInvalid, nameof(this.Signature)));
        }

        if (this.Size1 != expectedSize)
        {
            ThrowHelper.ThrowInvalidDataException(
                StringHelper.Format(ExceptionMessages.InvalidDataExceptionPropertyIsInvalid, nameof(this.Size1)));
        }
    }

    public string Signature { get; private set; }

    public short Size1 { get; private set; }

    public short Size2 { get; private set; }    // always equal to size1?

    public byte FirstByteOfData => this.Data.FirstOrDefault();

    protected byte[] Data { get; private set; }

    public void ReadFrom(BinaryReader reader)
    {
        this.Signature = EncodingHelper.Default.GetString(reader.ReadExactBytes(4));
        this.Size1 = reader.ReadInt16();
        this.Size2 = reader.ReadInt16();
        this.Data = reader.ReadExactBytes(this.Size1 - this.Signature.Length - (sizeof(short) * 2));
    }
}
