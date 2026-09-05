using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Bitter.Tests
{
    [TestClass]
    public class BitStreamTests
    {
        [TestMethod]
        public void DefaultConstructor_UsesInternalMemoryStream()
        {
            BitStream stream = new BitStream();

            stream.WriteByte(9);
            stream.ByteOffset = 0;

            stream.ReadByte().ShouldBe((byte)9);
        }

        [TestMethod]
        public void Constructor_WithNullStream_UsesInternalMemoryStream()
        {
            BitStream stream = new BitStream(null);

            stream.WriteByte(9);
            stream.ByteOffset = 0;

            stream.ReadByte().ShouldBe((byte)9);
        }

        [TestMethod]
        public void ReadByte_And_WriteByte_Scalar_RoundTrip()
        {
            BitStream stream = new BitStream();
            stream.WriteByte(200);
            stream.ByteOffset = 0;

            stream.ReadByte().ShouldBe((byte)200);
        }

        [TestMethod]
        public void ReadByte_And_WriteByte_Array_RoundTrip()
        {
            BitStream stream = new BitStream();
            byte[] data = { 1, 2, 3, 4, 5 };

            stream.WriteByte(data);
            stream.ByteOffset = 0;

            stream.ReadByte(5).ShouldBe(data);
        }

        [TestMethod]
        public void ReadByte_Span_FillsBufferWithoutAllocating()
        {
            BitStream stream = new BitStream();
            stream.WriteByte(new byte[] { 10, 20, 30 });
            stream.ByteOffset = 0;

            Span<byte> buffer = stackalloc byte[3];
            stream.ReadByte(buffer);

            buffer.ToArray().ShouldBe(new byte[] { 10, 20, 30 });
        }

        [TestMethod]
        public void WriteByte_Span_RoundTrips()
        {
            BitStream stream = new BitStream();
            Span<byte> data = stackalloc byte[] { 7, 8, 9 };

            stream.WriteByte(data);
            stream.ByteOffset = 0;

            stream.ReadByte(3).ShouldBe(new byte[] { 7, 8, 9 });
        }

        [TestMethod]
        public void ReadBit_And_WriteBit_Scalar_RoundTrip()
        {
            BitStream stream = new BitStream();
            stream.WriteBit(1);
            stream.WriteBit(0);
            stream.WriteBit(1);
            stream.BitOffset = 0;

            stream.ReadBit().ShouldBe((byte)1);
            stream.ReadBit().ShouldBe((byte)0);
            stream.ReadBit().ShouldBe((byte)1);
        }

        [TestMethod]
        public void ReadBit_And_WriteBit_Array_RoundTrip()
        {
            BitStream stream = new BitStream();
            byte[] bits = { 1, 1, 0, 0, 1, 0, 1, 0, 1 };

            stream.WriteBit(bits);
            // ByteOffset must be reset before BitOffset: writing 9 bits advances byteOffset by
            // one, and the BitOffset setter alone does not rewind byteOffset.
            stream.ByteOffset = 0;
            stream.BitOffset = 0;

            stream.ReadBit(bits.Length).ShouldBe(bits);
        }

        [TestMethod]
        public void ReadByteFromBits_And_WriteByteAsBits_RoundTrip()
        {
            BitStream stream = new BitStream();
            stream.WriteByteAsBits(0b0000_1011, 4);
            stream.BitOffset = 0;

            stream.ReadByteFromBits(4).ShouldBe((byte)0b0000_1011);
        }

        [TestMethod]
        public void ReadBytesFromBits_And_WriteBytesAsBits_RoundTrip()
        {
            BitStream stream = new BitStream();
            byte[] data = { 0xAB, 0x0C };

            stream.WriteBytesAsBits(data, 12);
            stream.ByteOffset = 0;
            stream.BitOffset = 0;

            byte[] result = stream.ReadBytesFromBits(12);
            result.ShouldBe(new byte[] { 0xAB, 0x0C });
        }

        [TestMethod]
        public void WriteBytesAsBits_ReadOnlySpanOverload_RoundTrips()
        {
            BitStream stream = new BitStream();
            ReadOnlySpan<byte> data = stackalloc byte[] { 0xAB, 0x0C };

            stream.WriteBytesAsBits(data, 12);
            stream.ByteOffset = 0;
            stream.BitOffset = 0;

            stream.ReadBytesFromBits(12).ShouldBe(new byte[] { 0xAB, 0x0C });
        }

        [TestMethod]
        public void Length_ReflectsUnderlyingStreamLength()
        {
            BitStream stream = new BitStream();
            stream.WriteByte(new byte[] { 1, 2, 3 });
            stream.Flush();

            stream.Length.ShouldBe(3);
        }

        [TestMethod]
        public void EndOfStream_ReflectsPosition()
        {
            using MemoryStream ms = new MemoryStream(new byte[] { 1, 2 });
            BitStream stream = new BitStream(ms);

            stream.EndOfStream.ShouldBeFalse();
            stream.ReadByte();
            stream.EndOfStream.ShouldBeTrue();
        }

        [TestMethod]
        public void ByteOffset_Setter_Seeks()
        {
            BitStream stream = new BitStream();
            stream.WriteByte(new byte[] { 10, 20, 30 });

            stream.ByteOffset = 1;

            stream.ReadByte().ShouldBe((byte)20);
        }

        [TestMethod]
        public void BitOffset_Setter_CrossesByteBoundary()
        {
            BitStream stream = new BitStream();
            stream.WriteByte(new byte[] { 0b1111_0000, 0b0000_1111 });

            stream.BitOffset = 9;

            stream.ReadBit().ShouldBe((byte)1);
        }

        [TestMethod]
        public void MostSignificantBit_PropertyGetSet()
        {
            BitStream stream = new BitStream();
            stream.MostSignificantBit.ShouldBeFalse();

            stream.MostSignificantBit = true;

            stream.MostSignificantBit.ShouldBeTrue();
        }

        [TestMethod]
        public void CopyTo_DoesNotIncludeReadAheadBufferedByte()
        {
            // Known limitation: BitStream always keeps the byte at the current position
            // pre-fetched into an internal buffer (for bit-level access), and CopyTo delegates
            // straight to the underlying Stream.CopyTo from its raw Position - which is already
            // one byte past what has actually been consumed. So CopyTo silently drops that
            // buffered byte (here, all of it: after a full-array write the raw stream sits at
            // EOF already). This documents current behavior, not the ideal contract.
            BitStream stream = new BitStream();
            stream.WriteByte(new byte[] { 1, 2, 3 });

            using MemoryStream destination = new MemoryStream();
            stream.CopyTo(destination);

            destination.ToArray().ShouldBeEmpty();
        }

        [TestMethod]
        public void Dispose_DoesNotThrow()
        {
            BitStream stream = new BitStream();
            Should.NotThrow(() => stream.Dispose());
        }

        [TestMethod]
        public void Close_DoesNotThrow()
        {
            BitStream stream = new BitStream();
            Should.NotThrow(() => stream.Close());
        }
    }
}
