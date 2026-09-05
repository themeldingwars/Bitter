using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using static Bitter.BinaryStream;

namespace Bitter.Tests
{
    [TestClass]
    public class ShortTests
    {
        [TestMethod]
        public void Short_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.Short(0);
            stream.Write.Short(short.MinValue);
            stream.Write.Short(short.MaxValue);
            stream.Write.Short(-1);
            stream.Rewind();

            stream.Read.Short().ShouldBe((short)0);
            stream.Read.Short().ShouldBe(short.MinValue);
            stream.Read.Short().ShouldBe(short.MaxValue);
            stream.Read.Short().ShouldBe((short)-1);
        }

        [TestMethod]
        public void ShortArray_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            short[] values = { 0, -1, short.MinValue, short.MaxValue, 1234 };

            stream.Write.ShortArray(values);
            stream.Rewind();

            stream.Read.ShortArray(values.Length).ShouldBe(values);
        }

        [TestMethod]
        public void ShortList_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<short> values = new List<short> { 0, -1, short.MinValue, short.MaxValue, 1234 };

            stream.Write.ShortList(values);
            stream.Rewind();

            stream.Read.ShortList(values.Count).ShouldBe(values);
        }

        [TestMethod]
        public void ShortFromBits_RoundTrips_Positive()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.ShortAsBits(500, 12);
            stream.Rewind();

            stream.Read.ShortFromBits(12).ShouldBe((short)500);
        }

        [TestMethod]
        public void ShortFromBits_RoundTrips_Negative()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.ShortAsBits(-1, 16);
            stream.Rewind();

            stream.Read.ShortFromBits(16).ShouldBe((short)-1);
        }

        [TestMethod]
        public void ShortArrayFromBits_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            short[] values = { 100, 200, 300, 400 };

            stream.Write.ShortArrayAsBits(values, 16);
            stream.Rewind();

            stream.Read.ShortArrayFromBits(values.Length, 16).ShouldBe(values);
        }

        [TestMethod]
        public void ShortListFromBits_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<short> values = new List<short> { 100, 200, 300, 400 };

            stream.Write.ShortListAsBits(values, 16);
            stream.Rewind();

            stream.Read.ShortListFromBits(values.Count, 16).ShouldBe(values);
        }

        [TestMethod]
        public void Short_RespectsByteOrder()
        {
            BinaryStream le = TestHelpers.NewStream(Endianness.LittleEndian);
            BinaryStream be = TestHelpers.NewStream(Endianness.BigEndian);

            le.Write.Short(0x0102);
            be.Write.Short(0x0102);
            le.Rewind();
            be.Rewind();

            byte[] leBytes = le.Read.ByteArray(2);
            byte[] beBytes = be.Read.ByteArray(2);

            leBytes.ShouldBe(new byte[] { 0x02, 0x01 });
            beBytes.ShouldBe(new byte[] { 0x01, 0x02 });
        }
    }
}
