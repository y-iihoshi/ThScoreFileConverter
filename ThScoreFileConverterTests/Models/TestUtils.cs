using System;
using System.IO;
using System.Text;

namespace ThScoreFileConverter.Models.Tests
{
    public static class TestUtils
    {
        public static byte[] MakeByteArray(params object[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            byte[] array = null;

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream();
                using (var writer = new BinaryWriter(stream, Encoding.Default))
                {
                    stream = null;

                    foreach (var arg in args)
                    {
                        // I want to use C# 7 type switch...

                        var bytesArg = arg as byte[];
                        if (bytesArg != null)
                        {
                            writer.Write(bytesArg);
                            continue;
                        }

                        var charsArg = arg as char[];
                        if (charsArg != null)
                        {
                            writer.Write(charsArg);
                            continue;
                        }

                        var sbyteArg = arg as sbyte?;
                        if (sbyteArg != null)
                        {
                            writer.Write(sbyteArg.Value);
                            continue;
                        }

                        var byteArg = arg as byte?;
                        if (byteArg != null)
                        {
                            writer.Write(byteArg.Value);
                            continue;
                        }

                        var charArg = arg as char?;
                        if (charArg != null)
                        {
                            writer.Write(charArg.Value);
                            continue;
                        }

                        var shortArg = arg as short?;
                        if (shortArg != null)
                        {
                            writer.Write(shortArg.Value);
                            continue;
                        }

                        var ushortArg = arg as ushort?;
                        if (ushortArg != null)
                        {
                            writer.Write(ushortArg.Value);
                            continue;
                        }

                        var intArg = arg as int?;
                        if (intArg != null)
                        {
                            writer.Write(intArg.Value);
                            continue;
                        }

                        var uintArg = arg as uint?;
                        if (uintArg != null)
                        {
                            writer.Write(uintArg.Value);
                            continue;
                        }

                        var longArg = arg as long?;
                        if (longArg != null)
                        {
                            writer.Write(longArg.Value);
                            continue;
                        }

                        var ulongArg = arg as ulong?;
                        if (ulongArg != null)
                        {
                            writer.Write(ulongArg.Value);
                            continue;
                        }

                        var boolArg = arg as bool?;
                        if (boolArg != null)
                        {
                            writer.Write(boolArg.Value);
                            continue;
                        }

                        var doubleArg = arg as double?;
                        if (doubleArg != null)
                        {
                            writer.Write(doubleArg.Value);
                            continue;
                        }

                        var floatArg = arg as float?;
                        if (floatArg != null)
                        {
                            writer.Write(floatArg.Value);
                            continue;
                        }

                        var stringArg = arg as string;
                        if (stringArg != null)
                        {
                            writer.Write(stringArg);
                            continue;
                        }

                        var decimalArg = arg as decimal?;
                        if (decimalArg != null)
                        {
                            writer.Write(decimalArg.Value);
                            continue;
                        }
                    }

                    writer.Flush();

                    array = new byte[writer.BaseStream.Length];
                    writer.BaseStream.Position = 0;
                    writer.BaseStream.Read(array, 0, array.Length);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return array;
        }
    }
}
