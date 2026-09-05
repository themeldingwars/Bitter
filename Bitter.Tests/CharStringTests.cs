using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using static Bitter.BinaryStream;

namespace Bitter.Tests
{
    [TestClass]
    public class CharStringTests
    {
        [TestMethod]
        public void Char_RoundTrips_DefaultEncoding()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.Char('A');
            stream.Rewind();

            stream.Read.Char().ShouldBe('A');
        }

        [TestMethod]
        [DataRow(TextEncoding.ASCII)]
        [DataRow(TextEncoding.UTF8)]
        [DataRow(TextEncoding.UTF7)]
        [DataRow(TextEncoding.UNICODE)]
        [DataRow(TextEncoding.UTF32)]
        public void Char_RoundTrips_AllEncodings(TextEncoding encoding)
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.Char('Z', encoding);
            stream.Rewind();

            stream.Read.Char(encoding).ShouldBe('Z');
        }

        [TestMethod]
        public void CharArray_RoundTrips()
        {
            BinaryStream stream = TestHelpers.NewStream();
            char[] chars = { 'H', 'e', 'l', 'l', 'o' };

            stream.Write.CharArray(chars);
            stream.Rewind();

            stream.Read.CharArray(chars.Length).ShouldBe(chars);
        }

        [TestMethod]
        public void String_RoundTrips_SingleChar()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.String("A");
            stream.Rewind();

            stream.Read.String().ShouldBe("A");
        }

        [TestMethod]
        public void String_RoundTrips_FixedLength()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.String("test");
            stream.Rewind();

            stream.Read.String(4).ShouldBe("test");
        }

        [TestMethod]
        [DataRow(TextEncoding.ASCII)]
        [DataRow(TextEncoding.UTF8)]
        [DataRow(TextEncoding.UNICODE)]
        [DataRow(TextEncoding.UTF32)]
        public void String_RoundTrips_AllEncodings(TextEncoding encoding)
        {
            BinaryStream stream = TestHelpers.NewStream();
            string value = "Bitter";

            stream.Write.String(value, encoding);
            stream.Rewind();

            stream.Read.String(value.Length, encoding).ShouldBe(value);
        }

        [TestMethod]
        public void DefaultTextEncoding_IsUsed_WhenNotSpecified()
        {
            BinaryStream stream = TestHelpers.NewStream();
            stream.DefaultTextEncoding = TextEncoding.UNICODE;

            stream.Write.String("hi");
            stream.Rewind();

            stream.Read.String(2).ShouldBe("hi");
            stream.ByteOffset.ShouldBe(4);
        }

        [TestMethod]
        public void String_MultipleValues_RoundTripInSequence()
        {
            BinaryStream stream = TestHelpers.NewStream();

            stream.Write.String("foo");
            stream.Write.String("bar");
            stream.Rewind();

            stream.Read.String(3).ShouldBe("foo");
            stream.Read.String(3).ShouldBe("bar");
        }
    }
}
