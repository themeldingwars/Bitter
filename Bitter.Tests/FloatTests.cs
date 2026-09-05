using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using static Bitter.BinaryStream;

namespace Bitter.Tests
{
    [TestClass]
    public class FloatTests
    {
        [TestMethod]
        public void Float_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.Float(0f);
            stream.Write.Float(float.MinValue);
            stream.Write.Float(float.MaxValue);
            stream.Write.Float(-1.5f);
            stream.Write.Float(float.NaN);
            stream.Rewind();

            stream.Read.Float().ShouldBe(0f);
            stream.Read.Float().ShouldBe(float.MinValue);
            stream.Read.Float().ShouldBe(float.MaxValue);
            stream.Read.Float().ShouldBe(-1.5f);
            stream.Read.Float().ShouldBe(float.NaN);
        }

        [TestMethod]
        public void FloatArray_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            float[] values = { 0f, -1.5f, float.MinValue, float.MaxValue, 3.14159f };

            stream.Write.FloatArray(values);
            stream.Rewind();

            stream.Read.FloatArray(values.Length).ShouldBe(values);
        }

        [TestMethod]
        public void FloatList_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<float> values = new List<float> { 0f, -1.5f, float.MinValue, float.MaxValue, 3.14159f };

            stream.Write.FloatList(values);
            stream.Rewind();

            stream.Read.FloatList(values.Count).ShouldBe(values);
        }

        [TestMethod]
        public void Float_RespectsByteOrder()
        {
            BinaryStream le = TestHelpers.NewStream(Endianness.LittleEndian);
            BinaryStream be = TestHelpers.NewStream(Endianness.BigEndian);

            le.Write.Float(1.5f);
            be.Write.Float(1.5f);
            le.Rewind();
            be.Rewind();

            le.Read.Float().ShouldBe(1.5f);
            be.Read.Float().ShouldBe(1.5f);
        }
    }
}
