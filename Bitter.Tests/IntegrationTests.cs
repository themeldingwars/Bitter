using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using static Bitter.BinaryStream;

namespace Bitter.Tests
{
    /// <summary>
    /// End-to-end scenario mirroring the README's quick usage guide: mixed types, arrays, lists,
    /// bits, and bit-length reads/writes with a differing target width, all on one stream.
    /// </summary>
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void MixedReadWrite_MatchesReadmeUsageGuide()
        {
            BinaryStream stream = new BinaryStream(Endianness.LittleEndian, Endianness.LittleEndian, TextEncoding.UTF8);

            byte[] bytes = { 1, 2, 4, 7 };
            int[] ints = { -1, 200, 7600, 984, -999 };
            List<float> floats = new List<float>(new float[] { 1f, -2f, 0.55f });

            stream.Write.ULong(123456);
            stream.Write.ByteArray(bytes);
            stream.Write.IntArrayAsBits(ints, 14);
            stream.Write.HalfList(floats);
            stream.Write.Bit(1);
            stream.Write.Bit(0);
            stream.Write.Bit(1);
            stream.Write.Bit(0);
            stream.Write.String("test");

            stream.Rewind();

            stream.Read.ULong().ShouldBe((ulong)123456);
            stream.Read.ByteArray(4).ShouldBe(bytes);
            // written as ints, read back narrower as shorts - matches the README's point that the
            // bit-length accessors don't have to round-trip through the same declared type.
            List<short> narrowedInts = stream.Read.ShortListFromBits(ints.Length, 14);
            for (int i = 0; i < ints.Length; i++)
            {
                narrowedInts[i].ShouldBe((short)ints[i]);
            }
            // half precision cannot represent 0.55 exactly, so it round-trips as the nearest
            // representable half value rather than the original float.
            stream.Read.HalfArray(floats.Count).ShouldBe(new float[] { 1f, -2f, 0.5498047f });
            stream.Read.BitArray(4).ShouldBe(new byte[] { 1, 0, 1, 0 });
            stream.Read.String(4).ShouldBe("test");

            stream.EndOfStream.ShouldBeTrue();
        }

        [TestMethod]
        public void SameStream_SupportsInterleavedReadsAndWrites()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.Int(1);
            stream.Rewind();
            int first = stream.Read.Int();

            stream.Write.Int(first + 1);
            stream.ByteOffset = 4;
            int second = stream.Read.Int();

            second.ShouldBe(2);
        }
    }
}
