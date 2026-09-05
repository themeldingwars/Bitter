using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using static Bitter.BinaryStream;

namespace Bitter.Tests
{
    [TestClass]
    public class LongTests
    {
        [TestMethod]
        public void Long_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.Long(0);
            stream.Write.Long(long.MinValue);
            stream.Write.Long(long.MaxValue);
            stream.Write.Long(-1);
            stream.Rewind();

            stream.Read.Long().ShouldBe(0L);
            stream.Read.Long().ShouldBe(long.MinValue);
            stream.Read.Long().ShouldBe(long.MaxValue);
            stream.Read.Long().ShouldBe(-1L);
        }

        [TestMethod]
        public void LongArray_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            long[] values = { 0, -1, long.MinValue, long.MaxValue, 123456789012345 };

            stream.Write.LongArray(values);
            stream.Rewind();

            stream.Read.LongArray(values.Length).ShouldBe(values);
        }

        [TestMethod]
        public void LongList_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<long> values = new List<long> { 0, -1, long.MinValue, long.MaxValue, 123456789012345 };

            stream.Write.LongList(values);
            stream.Rewind();

            stream.Read.LongList(values.Count).ShouldBe(values);
        }

        [TestMethod]
        public void LongFromBits_RoundTrips_WithinInt()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.LongAsBits(500, 14);
            stream.Rewind();

            stream.Read.LongFromBits(14).ShouldBe(500L);
        }

        [TestMethod]
        public void LongFromBits_RoundTrips_Beyond32Bits()
        {
            BinaryStream stream = TestHelpers.NewStream();
            long value = 1L << 40;

            stream.Write.LongAsBits(value, 48);
            stream.Rewind();

            stream.Read.LongFromBits(48).ShouldBe(value);
        }

        [TestMethod]
        public void LongFromBits_RoundTrips_Negative64()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.LongAsBits(-1, 64);
            stream.Rewind();

            stream.Read.LongFromBits(64).ShouldBe(-1L);
        }

        [TestMethod]
        public void LongArrayFromBits_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            long[] values = { 100, 200, 300, 8191 };

            stream.Write.LongArrayAsBits(values, 14);
            stream.Rewind();

            stream.Read.LongArrayFromBits(values.Length, 14).ShouldBe(values);
        }

        [TestMethod]
        public void LongListAsBits_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<long> values = new List<long> { 100, 200, 300, 8191 };

            stream.Write.LongAsBits(values, 14);
            stream.Rewind();

            stream.Read.LongListFromBits(values.Count, 14).ShouldBe(values);
        }

        [TestMethod]
        public void Long_RespectsByteOrder()
        {
            BinaryStream le = TestHelpers.NewStream(Endianness.LittleEndian);
            BinaryStream be = TestHelpers.NewStream(Endianness.BigEndian);

            le.Write.Long(0x0102030405060708);
            be.Write.Long(0x0102030405060708);
            le.Rewind();
            be.Rewind();

            le.Read.ByteArray(8).ShouldBe(new byte[] { 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01 });
            be.Read.ByteArray(8).ShouldBe(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 });
        }
    }
}
