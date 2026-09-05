using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using static Bitter.BinaryStream;

namespace Bitter.Tests
{
    [TestClass]
    public class BinaryStreamTests
    {
        [TestMethod]
        public void DefaultConstructor_UsesInternalMemoryStream()
        {
            BinaryStream stream = new BinaryStream();

            stream.Write.Byte(42);
            stream.Rewind();

            stream.Read.Byte().ShouldBe((byte)42);
        }

        [TestMethod]
        public void Constructor_WithStream_Works()
        {
            using MemoryStream ms = new MemoryStream();
            BinaryStream stream = new BinaryStream(ms);

            stream.Write.Byte(7);
            stream.Rewind();

            stream.Read.Byte().ShouldBe((byte)7);
        }

        [TestMethod]
        public void Constructor_WithByteOrderAndBitOrder_SetsProperties()
        {
            BinaryStream stream = new BinaryStream(new MemoryStream(), Endianness.BigEndian, Endianness.BigEndian, TextEncoding.UTF8);

            stream.ByteOrder.ShouldBe(Endianness.BigEndian);
            stream.BitOrder.ShouldBe(Endianness.BigEndian);
            stream.DefaultTextEncoding.ShouldBe(TextEncoding.UTF8);
        }

        [TestMethod]
        public void Constructor_WithoutStream_UsesDefaults()
        {
            BinaryStream stream = new BinaryStream(Endianness.BigEndian, Endianness.LittleEndian, TextEncoding.UNICODE);

            stream.ByteOrder.ShouldBe(Endianness.BigEndian);
            stream.BitOrder.ShouldBe(Endianness.LittleEndian);
            stream.DefaultTextEncoding.ShouldBe(TextEncoding.UNICODE);
        }

        [TestMethod]
        public void ByteOrder_Property_CanBeChanged()
        {
            BinaryStream stream = TestHelpers.NewStream();
            stream.ByteOrder.ShouldBe(Endianness.LittleEndian);

            stream.ByteOrder = Endianness.BigEndian;
            stream.ByteOrder.ShouldBe(Endianness.BigEndian);
        }

        [TestMethod]
        public void DefaultTextEncoding_MapsDefaultToAscii()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.DefaultTextEncoding = TextEncoding.DEFAULT;

            stream.DefaultTextEncoding.ShouldBe(TextEncoding.ASCII);
        }

        [TestMethod]
        public void Length_ReflectsBytesWritten()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.ByteArray(new byte[] { 1, 2, 3, 4 });
            stream.Flush();

            stream.Length.ShouldBe(4);
        }

        [TestMethod]
        public void EndOfStream_BecomesTrueAfterReadingAllBytes()
        {
            BinaryStream stream = TestHelpers.NewStream();
            stream.Write.ByteArray(new byte[] { 1, 2, 3 });
            stream.Rewind();

            stream.Read.ByteArray(3).ShouldBe(new byte[] { 1, 2, 3 });
            stream.EndOfStream.ShouldBeTrue();
        }

        [TestMethod]
        public void ByteOffset_CanSeek()
        {
            BinaryStream stream = TestHelpers.NewStream();
            stream.Write.ByteArray(new byte[] { 10, 20, 30, 40 });

            stream.ByteOffset = 2;

            stream.Read.Byte().ShouldBe((byte)30);
        }

        [TestMethod]
        public void BitOffset_CanSeek()
        {
            BinaryStream stream = TestHelpers.NewStream();
            stream.Write.Byte(0b1010_1100);
            stream.Rewind();

            stream.BitOffset = 2;

            stream.Read.Bit().ShouldBe((byte)1);
        }

        [TestMethod]
        public void Flush_WritesDirtyBits()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.Bit(1);
            stream.Flush();
            stream.Length.ShouldBe(1);
        }

        [TestMethod]
        public void Dispose_DoesNotThrow()
        {
            BinaryStream stream = TestHelpers.NewStream();
            Should.NotThrow(() => stream.Dispose());
        }

        [TestMethod]
        public void Close_DoesNotThrow()
        {
            BinaryStream stream = TestHelpers.NewStream();
            Should.NotThrow(() => stream.Close());
        }
    }
}
