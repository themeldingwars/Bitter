using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Bitter.Tests
{
    [TestClass]
    public class BitTests
    {
        [TestMethod]
        public void Bit_RoundTrips_SingleBits()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.Bit(1);
            stream.Write.Bit(0);
            stream.Write.Bit(1);
            stream.Write.Bit(1);
            stream.Rewind();

            stream.Read.Bit().ShouldBe((byte)1);
            stream.Read.Bit().ShouldBe((byte)0);
            stream.Read.Bit().ShouldBe((byte)1);
            stream.Read.Bit().ShouldBe((byte)1);
        }

        [TestMethod]
        public void BitArray_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            byte[] bits = { 1, 0, 0, 1, 1, 0, 1, 0, 1, 1 };

            stream.Write.BitArray(bits);
            stream.Rewind();

            byte[] result = stream.Read.BitArray(bits.Length);
            result.ShouldBe(bits);
        }

        [TestMethod]
        public void BitList_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<byte> bits = new List<byte> { 0, 1, 1, 0, 0, 0, 1, 1 };

            stream.Write.BitList(bits);
            stream.Rewind();

            List<byte> result = stream.Read.BitList(bits.Count);
            result.ShouldBe(bits);
        }

        [TestMethod]
        public void Bit_CrossesByteBoundary()
        {
            BinaryStream stream = TestHelpers.NewStream();
            byte[] bits = new byte[20];
            for (int i = 0; i < bits.Length; i++)
            {
                bits[i] = (byte)(i % 2);
            }

            stream.Write.BitArray(bits);
            stream.Rewind();

            stream.Read.BitArray(bits.Length).ShouldBe(bits);
        }
    }
}
