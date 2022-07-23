//-----------------------------------------------------------------------
// <copyright file="BestShotDeveloper.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models;

/// <summary>
/// Provides the best shot development functions.
/// </summary>
internal static class BestShotDeveloper
{
    /// <summary>
    /// Develops a best shot data in PNG format image.
    /// </summary>
    /// <typeparam name="THeader">The type of a best shot header.</typeparam>
    /// <param name="input">The input stream that treats the best shot data to convert.</param>
    /// <param name="output">The stream for outputting developed PNG format image.</param>
    /// <param name="pixelFormat">The pixel format for the developing PNG format image.</param>
    /// <returns>The best shot header.</returns>
    public static THeader Develop<THeader>(Stream input, Stream output, PixelFormat pixelFormat)
        where THeader : IBestShotHeader, IBinaryReadable, new()
    {
        if (pixelFormat is not (PixelFormat.Format24bppRgb or PixelFormat.Format32bppArgb))
            throw new NotImplementedException();

        using var decoded = new MemoryStream();

        using var reader = new BinaryReader(input, EncodingHelper.UTF8NoBOM, true);
        var header = BinaryReadableHelper.Create<THeader>(reader);

        Lzss.Decompress(input, decoded);

        _ = decoded.Seek(0, SeekOrigin.Begin);
        using var bitmap = new Bitmap(header.Width, header.Height, pixelFormat);
        bitmap.SetResolution(96, 96);

        try
        {
#if !NET5_0_OR_GREATER
            var permission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
            permission.Demand();
#endif

            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, header.Width, header.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            var source = decoded.ToArray();
            var destination = bitmapData.Scan0;

            if (pixelFormat == PixelFormat.Format24bppRgb)
            {
                var sourceStride = 3 * header.Width;    // "3" means 24bpp.
                for (var sourceIndex = 0; sourceIndex < source.Length; sourceIndex += sourceStride)
                {
                    Marshal.Copy(source, sourceIndex, destination, sourceStride);
                    destination += bitmapData.Stride;
                }
            }
            else
            {
                Marshal.Copy(source, 0, destination, source.Length);
            }

            bitmap.UnlockBits(bitmapData);
        }
        catch (SecurityException e)
        {
            Console.WriteLine(e.ToString());
        }

        bitmap.Save(output, ImageFormat.Png);
        output.Flush();
        output.SetLength(output.Position);

        return header;
    }
}
