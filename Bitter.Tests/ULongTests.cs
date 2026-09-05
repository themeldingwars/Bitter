using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using static Bitter.BinaryStream;

namespace Bitter.Tests
{
    [TestClass]
    public class ULongTests
    {
        [TestMethod]
        public void ULong_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.ULong(0);
            stream.Write.ULong(ulong.MaxValue);
            stream.Write.ULong(123456789012345);
            stream.Rewind();

            stream.Read.ULong().ShouldBe((ulong)0);
            stream.Read.ULong().ShouldBe(ulong.MaxValue);
            stream.Read.ULong().ShouldBe((ulong)123456789012345);
        }

        [TestMethod]
        public void ULongArray_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            ulong[] values = { 0, ulong.MaxValue, 123456789012345, 42 };

            stream.Write.ULongArray(values);
            stream.Rewind();

            stream.Read.ULongArray(values.Length).ShouldBe(values);
        }

        [TestMethod]
        public void ULongList_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<ulong> values = new List<ulong> { 0, ulong.MaxValue, 123456789012345, 42 };

            stream.Write.ULongList(values);
            stream.Rewind();

            stream.Read.ULongList(values.Count).ShouldBe(values);
        }

        [TestMethod]
        public void ULongFromBits_RoundTrips_WithinInt()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.ULongAsBits(500, 14);
            stream.Rewind();

            stream.Read.ULongFromBits(14).ShouldBe((ulong)500);
        }

        [TestMethod]
        public void ULongFromBits_RoundTrips_Beyond32Bits()
        {
            BinaryStream stream = TestHelpers.NewStream();
            ulong value = 1UL << 40;

            stream.Write.ULongAsBits(value, 48);
            stream.Rewind();

            stream.Read.ULongFromBits(48).ShouldBe(value);
        }

        [TestMethod]
        public void ULongArrayFromBits_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            ulong[] values = { 100, 200, 300, 8191 };

            stream.Write.ULongArrayAsBits(values, 14);
            stream.Rewind();

            stream.Read.ULongArrayFromBits(values.Length, 14).ShouldBe(values);
        }

        [TestMethod]
        public void ULongListFromBits_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<ulong> values = new List<ulong> { 100, 200, 300, 8191 };

            stream.Write.ULongListAsBits(values, 14);
            stream.Rewind();

            stream.Read.ULongListFromBits(values.Count, 14).ShouldBe(values);
        }

        [TestMethod]
        public void ULong_RespectsByteOrder()
        {
            BinaryStream le = TestHelpers.NewStream(Endianness.LittleEndian);
            BinaryStream be = TestHelpers.NewStream(Endianness.BigEndian);

            le.Write.ULong(0x0102030405060708);
            be.Write.ULong(0x0102030405060708);
            le.Rewind();
            be.Rewind();

            le.Read.ByteArray(8).ShouldBe(new byte[] { 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01 });
            be.Read.ByteArray(8).ShouldBe(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 });
        }
    }
}
