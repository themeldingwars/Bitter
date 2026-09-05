using System.IO;
using static Bitter.BinaryStream;

namespace Bitter.Tests
{
    internal static class TestHelpers
    {
        public static BinaryStream NewStream(Endianness byteOrder = Endianness.LittleEndian, Endianness bitOrder = Endianness.LittleEndian)
        {
            return new BinaryStream(new MemoryStream(), byteOrder, bitOrder, TextEncoding.ASCII);
        }

        public static void Rewind(this BinaryStream stream)
        {
            stream.Flush();
            stream.ByteOffset = 0;
            stream.BitOffset = 0;
        }
    }
}
