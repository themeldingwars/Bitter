using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using static Bitter.BinaryStream;

namespace Bitter.Tests
{
    [TestClass]
    public class DoubleTests
    {
        [TestMethod]
        public void Double_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.Double(0d);
            stream.Write.Double(double.MinValue);
            stream.Write.Double(double.MaxValue);
            stream.Write.Double(-1.5d);
            stream.Write.Double(double.NaN);
            stream.Rewind();

            stream.Read.Double().ShouldBe(0d);
            stream.Read.Double().ShouldBe(double.MinValue);
            stream.Read.Double().ShouldBe(double.MaxValue);
            stream.Read.Double().ShouldBe(-1.5d);
            stream.Read.Double().ShouldBe(double.NaN);
        }

        [TestMethod]
        public void DoubleArray_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            double[] values = { 0d, -1.5d, double.MinValue, double.MaxValue, 3.14159265358979 };

            stream.Write.DoubleArray(values);
            stream.Rewind();

            stream.Read.DoubleArray(values.Length).ShouldBe(values);
        }

        [TestMethod]
        public void DoubleList_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<double> values = new List<double> { 0d, -1.5d, double.MinValue, double.MaxValue, 3.14159265358979 };

            stream.Write.DoubleList(values);
            stream.Rewind();

            stream.Read.DoubleList(values.Count).ShouldBe(values);
        }

        [TestMethod]
        public void Double_RespectsByteOrder()
        {
            BinaryStream le = TestHelpers.NewStream(Endianness.LittleEndian);
            BinaryStream be = TestHelpers.NewStream(Endianness.BigEndian);

            le.Write.Double(1.5d);
            be.Write.Double(1.5d);
            le.Rewind();
            be.Rewind();

            le.Read.Double().ShouldBe(1.5d);
            be.Read.Double().ShouldBe(1.5d);
        }
    }
}
