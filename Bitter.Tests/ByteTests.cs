using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Bitter.Tests
{
    [TestClass]
    public class ByteTests
    {
        [TestMethod]
        public void Byte_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.Byte(0);
            stream.Write.Byte(255);
            stream.Write.Byte(127);
            stream.Rewind();

            stream.Read.Byte().ShouldBe((byte)0);
            stream.Read.Byte().ShouldBe((byte)255);
            stream.Read.Byte().ShouldBe((byte)127);
        }

        [TestMethod]
        public void ByteArray_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            byte[] values = { 0, 1, 254, 255, 42 };

            stream.Write.ByteArray(values);
            stream.Rewind();

            stream.Read.ByteArray(values.Length).ShouldBe(values);
        }

        [TestMethod]
        public void ByteList_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<byte> values = new List<byte> { 5, 10, 15, 250 };

            stream.Write.ByteList(values);
            stream.Rewind();

            stream.Read.ByteList(values.Count).ShouldBe(values);
        }

        [TestMethod]
        public void ByteFromBits_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.ByteAsBits(0x0F, 4);
            stream.Rewind();

            stream.Read.ByteFromBits(4).ShouldBe((byte)0x0F);
        }

        [TestMethod]
        public void ByteArrayFromBits_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            byte[] values = { 1, 2, 3, 4, 5 };

            stream.Write.ByteArrayAsBits(values, 3);
            stream.Rewind();

            stream.Read.ByteArrayFromBits(values.Length, 3).ShouldBe(values);
        }

        [TestMethod]
        public void ByteListFromBits_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<byte> values = new List<byte> { 1, 2, 3, 4, 5, 6 };

            stream.Write.ByteListAsBits(values, 3);
            stream.Rewind();

            stream.Read.ByteListFromBits(values.Count, 3).ShouldBe(values);
        }
    }
}
