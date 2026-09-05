using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using static Bitter.BinaryStream;

namespace Bitter.Tests
{
    [TestClass]
    public class UShortTests
    {
        [TestMethod]
        public void UShort_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.UShort(0);
            stream.Write.UShort(ushort.MaxValue);
            stream.Write.UShort(12345);
            stream.Rewind();

            stream.Read.UShort().ShouldBe((ushort)0);
            stream.Read.UShort().ShouldBe(ushort.MaxValue);
            stream.Read.UShort().ShouldBe((ushort)12345);
        }

        [TestMethod]
        public void UShortArray_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            ushort[] values = { 0, ushort.MaxValue, 1234, 5678 };

            stream.Write.UShortArray(values);
            stream.Rewind();

            stream.Read.UShortArray(values.Length).ShouldBe(values);
        }

        [TestMethod]
        public void UShortList_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<ushort> values = new List<ushort> { 0, ushort.MaxValue, 1234, 5678 };

            stream.Write.UShortList(values);
            stream.Rewind();

            stream.Read.UShortList(values.Count).ShouldBe(values);
        }

        [TestMethod]
        public void UShortFromBits_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.UShortAsBits(500, 12);
            stream.Rewind();

            stream.Read.UShortFromBits(12).ShouldBe((ushort)500);
        }

        [TestMethod]
        public void UShortArrayFromBits_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            ushort[] values = { 100, 200, 300, 400 };

            stream.Write.UShortArrayAsBits(values, 9);
            stream.Rewind();

            stream.Read.UShortArrayFromBits(values.Length, 9).ShouldBe(values);
        }

        [TestMethod]
        public void UShortListFromBits_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<ushort> values = new List<ushort> { 100, 200, 300, 400 };

            stream.Write.UShortListAsBits(values, 9);
            stream.Rewind();

            stream.Read.UShortListFromBits(values.Count, 9).ShouldBe(values);
        }

        [TestMethod]
        public void UShort_RespectsByteOrder()
        {
            BinaryStream le = TestHelpers.NewStream(Endianness.LittleEndian);
            BinaryStream be = TestHelpers.NewStream(Endianness.BigEndian);

            le.Write.UShort(0x0102);
            be.Write.UShort(0x0102);
            le.Rewind();
            be.Rewind();

            le.Read.ByteArray(2).ShouldBe(new byte[] { 0x02, 0x01 });
            be.Read.ByteArray(2).ShouldBe(new byte[] { 0x01, 0x02 });
        }
    }
}
