using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using static Bitter.BinaryStream;

namespace Bitter.Tests
{
    [TestClass]
    public class IntTests
    {
        [TestMethod]
        public void Int_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.Int(0);
            stream.Write.Int(int.MinValue);
            stream.Write.Int(int.MaxValue);
            stream.Write.Int(-1);
            stream.Rewind();

            stream.Read.Int().ShouldBe(0);
            stream.Read.Int().ShouldBe(int.MinValue);
            stream.Read.Int().ShouldBe(int.MaxValue);
            stream.Read.Int().ShouldBe(-1);
        }

        [TestMethod]
        public void IntArray_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            int[] values = { 0, -1, int.MinValue, int.MaxValue, 123456 };

            stream.Write.IntArray(values);
            stream.Rewind();

            stream.Read.IntArray(values.Length).ShouldBe(values);
        }

        [TestMethod]
        public void IntList_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<int> values = new List<int> { 0, -1, int.MinValue, int.MaxValue, 123456 };

            stream.Write.IntList(values);
            stream.Rewind();

            stream.Read.IntList(values.Count).ShouldBe(values);
        }

        [TestMethod]
        public void IntFromBits_RoundTrips_Positive()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.IntAsBits(500, 14);
            stream.Rewind();

            stream.Read.IntFromBits(14).ShouldBe(500);
        }

        [TestMethod]
        public void IntFromBits_RoundTrips_Negative()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.IntAsBits(-1, 32);
            stream.Rewind();

            stream.Read.IntFromBits(32).ShouldBe(-1);
        }

        [TestMethod]
        public void IntArrayFromBits_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            int[] values = { 100, 200, 300, 8191 };

            stream.Write.IntArrayAsBits(values, 14);
            stream.Rewind();

            stream.Read.IntArrayFromBits(values.Length, 14).ShouldBe(values);
        }

        [TestMethod]
        public void IntListFromBits_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<int> values = new List<int> { 100, 200, 300, 8191 };

            stream.Write.IntListAsBits(values, 14);
            stream.Rewind();

            stream.Read.IntListFromBits(values.Count, 14).ShouldBe(values);
        }

        [TestMethod]
        public void Int_RespectsByteOrder()
        {
            BinaryStream le = TestHelpers.NewStream(Endianness.LittleEndian);
            BinaryStream be = TestHelpers.NewStream(Endianness.BigEndian);

            le.Write.Int(0x01020304);
            be.Write.Int(0x01020304);
            le.Rewind();
            be.Rewind();

            le.Read.ByteArray(4).ShouldBe(new byte[] { 0x04, 0x03, 0x02, 0x01 });
            be.Read.ByteArray(4).ShouldBe(new byte[] { 0x01, 0x02, 0x03, 0x04 });
        }
    }
}
