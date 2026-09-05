using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Bitter.Tests
{
    [TestClass]
    public class SByteTests
    {
        [TestMethod]
        public void SByte_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.SByte(0);
            stream.Write.SByte(-128);
            stream.Write.SByte(127);
            stream.Write.SByte(-1);
            stream.Rewind();

            stream.Read.SByte().ShouldBe((sbyte)0);
            stream.Read.SByte().ShouldBe((sbyte)-128);
            stream.Read.SByte().ShouldBe((sbyte)127);
            stream.Read.SByte().ShouldBe((sbyte)-1);
        }

        [TestMethod]
        public void SByteArray_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            sbyte[] values = { 0, -1, 127, -128, 42 };

            stream.Write.SByteArray(values);
            stream.Rewind();

            stream.Read.SByteArray(values.Length).ShouldBe(values);
        }

        [TestMethod]
        public void SByteList_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<sbyte> values = new List<sbyte> { -5, 10, -15, 100 };

            stream.Write.SByteList(values);
            stream.Rewind();

            stream.Read.SByteList(values.Count).ShouldBe(values);
        }

        [TestMethod]
        public void SByteFromBits_RoundTrips_Positive()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.SByteAsBits(5, 4);
            stream.Rewind();

            stream.Read.SByteFromBits(4).ShouldBe((sbyte)5);
        }

        [TestMethod]
        public void SByteFromBits_RoundTrips_Negative()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.SByteAsBits(-1, 8);
            stream.Rewind();

            stream.Read.SByteFromBits(8).ShouldBe((sbyte)-1);
        }

        [TestMethod]
        public void SByteArrayFromBits_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            sbyte[] values = { 1, -2, 3, -4 };

            stream.Write.SByteArrayAsBits(values, 8);
            stream.Rewind();

            stream.Read.SByteArrayFromBits(values.Length, 8).ShouldBe(values);
        }

        [TestMethod]
        public void SByteListFromBits_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<sbyte> values = new List<sbyte> { 1, -2, 3, -4 };

            stream.Write.SByteListAsBits(values, 8);
            stream.Rewind();

            stream.Read.SByteListFromBits(values.Count, 8).ShouldBe(values);
        }
    }
}
