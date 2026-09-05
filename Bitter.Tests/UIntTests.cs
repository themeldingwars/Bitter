using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using static Bitter.BinaryStream;

namespace Bitter.Tests
{
    [TestClass]
    public class UIntTests
    {
        [TestMethod]
        public void UInt_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.UInt(0);
            stream.Write.UInt(uint.MaxValue);
            stream.Write.UInt(123456789);
            stream.Rewind();

            stream.Read.UInt().ShouldBe((uint)0);
            stream.Read.UInt().ShouldBe(uint.MaxValue);
            stream.Read.UInt().ShouldBe((uint)123456789);
        }

        [TestMethod]
        public void UIntArray_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            uint[] values = { 0, uint.MaxValue, 123456, 987654 };

            stream.Write.UIntArray(values);
            stream.Rewind();

            stream.Read.UIntArray(values.Length).ShouldBe(values);
        }

        [TestMethod]
        public void UIntList_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<uint> values = new List<uint> { 0, uint.MaxValue, 123456, 987654 };

            stream.Write.UIntList(values);
            stream.Rewind();

            stream.Read.UIntList(values.Count).ShouldBe(values);
        }

        [TestMethod]
        public void UIntFromBits_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.UIntAsBits(8191, 14);
            stream.Rewind();

            stream.Read.UIntFromBits(14).ShouldBe((uint)8191);
        }

        [TestMethod]
        public void UIntArrayFromBits_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            uint[] values = { 100, 200, 300, 8191 };

            stream.Write.UIntArrayAsBits(values, 14);
            stream.Rewind();

            stream.Read.UIntArrayFromBits(values.Length, 14).ShouldBe(values);
        }

        [TestMethod]
        public void UIntListFromBits_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<uint> values = new List<uint> { 100, 200, 300, 8191 };

            stream.Write.UIntListAsBits(values, 14);
            stream.Rewind();

            stream.Read.UIntListFromBits(values.Count, 14).ShouldBe(values);
        }

        [TestMethod]
        public void UInt_RespectsByteOrder()
        {
            BinaryStream le = TestHelpers.NewStream(Endianness.LittleEndian);
            BinaryStream be = TestHelpers.NewStream(Endianness.BigEndian);

            le.Write.UInt(0x01020304);
            be.Write.UInt(0x01020304);
            le.Rewind();
            be.Rewind();

            le.Read.ByteArray(4).ShouldBe(new byte[] { 0x04, 0x03, 0x02, 0x01 });
            be.Read.ByteArray(4).ShouldBe(new byte[] { 0x01, 0x02, 0x03, 0x04 });
        }
    }
}
