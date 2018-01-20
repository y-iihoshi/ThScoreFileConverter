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
                        switch (arg)
                        {
                            case byte[] bytesArg:
                                writer.Write(bytesArg);
                                break;
                            case char[] charsArg:
                                writer.Write(charsArg);
                                break;
                            case sbyte sbyteArg:
                                writer.Write(sbyteArg);
                                break;
                            case byte byteArg:
                                writer.Write(byteArg);
                                break;
                            case char charArg:
                                writer.Write(charArg);
                                break;
                            case short shortArg:
                                writer.Write(shortArg);
                                break;
                            case ushort ushortArg:
                                writer.Write(ushortArg);
                                break;
                            case int intArg:
                                writer.Write(intArg);
                                break;
                            case uint uintArg:
                                writer.Write(uintArg);
                                break;
                            case long longArg:
                                writer.Write(longArg);
                                break;
                            case ulong ulongArg:
                                writer.Write(ulongArg);
                                break;
                            case bool boolArg:
                                writer.Write(boolArg);
                                break;
                            case double doubleArg:
                                writer.Write(doubleArg);
                                break;
                            case float floatArg:
                                writer.Write(floatArg);
                                break;
                            case string stringArg:
                                writer.Write(stringArg);
                                break;
                            case decimal decimalArg:
                                writer.Write(decimalArg);
                                break;
                            default:
                                break;
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
