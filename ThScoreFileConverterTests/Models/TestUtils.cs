using System.IO;
using System.Linq;
using System.Text;

namespace ThScoreFileConverter.Models.Tests
{
    public static class TestUtils
    {
        public static byte[] MakeByteArray(params object[] args)
        {
            byte[] array = null;

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream();
                using (var writer = new BinaryWriter(stream, Encoding.Default))
                {
                    stream = null;

                    args.ToList().ForEach(arg =>
                    {
                        // I want to use C# 7 type switch...

                        var bytesArg = arg as byte[];
                        if (bytesArg != null)
                            writer.Write(bytesArg);

                        var charsArg = arg as char[];
                        if (charsArg != null)
                            writer.Write(charsArg);

                        var sbyteArg = arg as sbyte?;
                        if (sbyteArg != null)
                            writer.Write(sbyteArg.Value);

                        var byteArg = arg as byte?;
                        if (byteArg != null)
                            writer.Write(byteArg.Value);

                        var charArg = arg as char?;
                        if (charArg != null)
                            writer.Write(charArg.Value);

                        var shortArg = arg as short?;
                        if (shortArg != null)
                            writer.Write(shortArg.Value);

                        var ushortArg = arg as ushort?;
                        if (ushortArg != null)
                            writer.Write(ushortArg.Value);

                        var intArg = arg as int?;
                        if (intArg != null)
                            writer.Write(intArg.Value);

                        var uintArg = arg as uint?;
                        if (uintArg != null)
                            writer.Write(uintArg.Value);

                        var longArg = arg as long?;
                        if (longArg != null)
                            writer.Write(longArg.Value);

                        var ulongArg = arg as ulong?;
                        if (ulongArg != null)
                            writer.Write(ulongArg.Value);

                        var boolArg = arg as bool?;
                        if (boolArg != null)
                            writer.Write(boolArg.Value);

                        var doubleArg = arg as double?;
                        if (doubleArg != null)
                            writer.Write(doubleArg.Value);

                        var floatArg = arg as float?;
                        if (floatArg != null)
                            writer.Write(floatArg.Value);

                        var stringArg = arg as string;
                        if (stringArg != null)
                            writer.Write(stringArg);

                        var decimalArg = arg as decimal?;
                        if (decimalArg != null)
                            writer.Write(decimalArg.Value);
                    });

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
