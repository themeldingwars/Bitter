using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Bitter.Tests
{
    [TestClass]
    public class HalfTests
    {
        [TestMethod]
        public void Half_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.Half(0f);
            stream.Write.Half(1f);
            stream.Write.Half(-1f);
            stream.Write.Half(0.5f);
            stream.Write.Half(-123.25f);
            stream.Rewind();

            stream.Read.Half().ShouldBe(0f);
            stream.Read.Half().ShouldBe(1f);
            stream.Read.Half().ShouldBe(-1f);
            stream.Read.Half().ShouldBe(0.5f);
            stream.Read.Half().ShouldBe(-123.25f);
        }

        [TestMethod]
        public void HalfArray_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            float[] values = { 0f, 1f, -1f, 0.5f, 123.25f };

            stream.Write.HalfArray(values);
            stream.Rewind();

            stream.Read.HalfArray(values.Length).ShouldBe(values);
        }

        [TestMethod]
        public void HalfList_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<float> values = new List<float> { 0f, 1f, -1f, 0.5f, 123.25f };

            stream.Write.HalfList(values);
            stream.Rewind();

            stream.Read.HalfList(values.Count).ShouldBe(values);
        }
    }
}
