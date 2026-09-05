using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Bitter.Tests
{
    [TestClass]
    public class BinaryUtilTests
    {
        [TestMethod]
        public void UShortFromBufferLE_ReadsLittleEndian()
        {
            byte[] buffer = { 0x02, 0x01 };
            BinaryUtil.UShortFromBufferLE(buffer).ShouldBe((ushort)0x0102);
        }

        [TestMethod]
        public void UShortFromBufferBE_ReadsBigEndian()
        {
            byte[] buffer = { 0x01, 0x02 };
            BinaryUtil.UShortFromBufferBE(buffer).ShouldBe((ushort)0x0102);
        }

        [TestMethod]
        public void UIntFromBufferLE_ReadsLittleEndian()
        {
            byte[] buffer = { 0x04, 0x03, 0x02, 0x01 };
            BinaryUtil.UIntFromBufferLE(buffer).ShouldBe((uint)0x01020304);
        }

        [TestMethod]
        public void UIntFromBufferBE_ReadsBigEndian()
        {
            byte[] buffer = { 0x01, 0x02, 0x03, 0x04 };
            BinaryUtil.UIntFromBufferBE(buffer).ShouldBe((uint)0x01020304);
        }

        [TestMethod]
        public void ULongFromBufferLE_ReadsLittleEndian()
        {
            byte[] buffer = { 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01 };
            BinaryUtil.ULongFromBufferLE(buffer).ShouldBe(0x0102030405060708UL);
        }

        [TestMethod]
        public void ULongFromBufferBE_ReadsBigEndian()
        {
            byte[] buffer = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
            BinaryUtil.ULongFromBufferBE(buffer).ShouldBe(0x0102030405060708UL);
        }

        [TestMethod]
        public void WriteToBufferLE_UShort_RoundTrips()
        {
            byte[] buffer = new byte[2];
            BinaryUtil.WriteToBufferLE(buffer, (ushort)0x0102);
            BinaryUtil.UShortFromBufferLE(buffer).ShouldBe((ushort)0x0102);
        }

        [TestMethod]
        public void WriteToBufferBE_UShort_RoundTrips()
        {
            byte[] buffer = new byte[2];
            BinaryUtil.WriteToBufferBE(buffer, (ushort)0x0102);
            BinaryUtil.UShortFromBufferBE(buffer).ShouldBe((ushort)0x0102);
        }

        [TestMethod]
        public void WriteToBufferLE_UInt_RoundTrips()
        {
            byte[] buffer = new byte[4];
            BinaryUtil.WriteToBufferLE(buffer, 0x01020304u);
            BinaryUtil.UIntFromBufferLE(buffer).ShouldBe(0x01020304u);
        }

        [TestMethod]
        public void WriteToBufferBE_UInt_RoundTrips()
        {
            byte[] buffer = new byte[4];
            BinaryUtil.WriteToBufferBE(buffer, 0x01020304u);
            BinaryUtil.UIntFromBufferBE(buffer).ShouldBe(0x01020304u);
        }

        [TestMethod]
        public void WriteToBufferLE_ULong_RoundTrips()
        {
            byte[] buffer = new byte[8];
            BinaryUtil.WriteToBufferLE(buffer, 0x0102030405060708UL);
            BinaryUtil.ULongFromBufferLE(buffer).ShouldBe(0x0102030405060708UL);
        }

        [TestMethod]
        public void WriteToBufferBE_ULong_RoundTrips()
        {
            byte[] buffer = new byte[8];
            BinaryUtil.WriteToBufferBE(buffer, 0x0102030405060708UL);
            BinaryUtil.ULongFromBufferBE(buffer).ShouldBe(0x0102030405060708UL);
        }

        [TestMethod]
        public void Offset_IsRespected()
        {
            byte[] buffer = { 0xFF, 0x02, 0x01, 0xFF };
            BinaryUtil.UShortFromBufferLE(buffer, 1).ShouldBe((ushort)0x0102);
        }

        [TestMethod]
        public void FloatUIntMap_SharesMemory()
        {
            BinaryUtil.FloatUIntMap map = new BinaryUtil.FloatUIntMap { Float = 1.5f };
            map.UInt.ShouldBe((uint)0x3FC00000);
        }

        [TestMethod]
        public void DoubleULongMap_SharesMemory()
        {
            BinaryUtil.DoubleULongMap map = new BinaryUtil.DoubleULongMap { Double = 1.5d };
            map.ULong.ShouldBe(0x3FF8000000000000UL);
        }
    }
}
