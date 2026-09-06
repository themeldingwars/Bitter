using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace Bitter
{
    public static class BinaryUtil
    {

        public static ushort UShortFromBufferLE(ReadOnlySpan<byte> buffer, int offset = 0)
        {
            return BinaryPrimitives.ReadUInt16LittleEndian(buffer.Slice(offset));
        }
        public static ushort UShortFromBufferBE(ReadOnlySpan<byte> buffer, int offset = 0)
        {
            return BinaryPrimitives.ReadUInt16BigEndian(buffer.Slice(offset));
        }
        public static uint UIntFromBufferLE(ReadOnlySpan<byte> buffer, int offset = 0)
        {
            return BinaryPrimitives.ReadUInt32LittleEndian(buffer.Slice(offset));
        }
        public static uint UIntFromBufferBE(ReadOnlySpan<byte> buffer, int offset = 0)
        {
            return BinaryPrimitives.ReadUInt32BigEndian(buffer.Slice(offset));
        }
        public static ulong ULongFromBufferLE(ReadOnlySpan<byte> buffer, int offset = 0)
        {
            return BinaryPrimitives.ReadUInt64LittleEndian(buffer.Slice(offset));
        }
        public static ulong ULongFromBufferBE(ReadOnlySpan<byte> buffer, int offset = 0)
        {
            return BinaryPrimitives.ReadUInt64BigEndian(buffer.Slice(offset));
        }

        // Conversion methods for easier migration from the old FloatByteMap/DoubleByteMap helpers.
        public static float FloatFromBufferLE(ReadOnlySpan<byte> buffer, int offset = 0)
        {
            var map = new FloatUIntMap { UInt = UIntFromBufferLE(buffer, offset) };
            return map.Float;
        }
        public static float FloatFromBufferBE(ReadOnlySpan<byte> buffer, int offset = 0)
        {
            var map = new FloatUIntMap { UInt = UIntFromBufferBE(buffer, offset) };
            return map.Float;
        }
        public static double DoubleFromBufferLE(ReadOnlySpan<byte> buffer, int offset = 0)
        {
            var map = new DoubleULongMap { ULong = ULongFromBufferLE(buffer, offset) };
            return map.Double;
        }
        public static double DoubleFromBufferBE(ReadOnlySpan<byte> buffer, int offset = 0)
        {
            var map = new DoubleULongMap { ULong = ULongFromBufferBE(buffer, offset) };
            return map.Double;
        }

        public static void WriteToBufferLE(Span<byte> buffer, ushort value, int offset = 0)
        {
            BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(offset), value);
        }
        public static void WriteToBufferBE(Span<byte> buffer, ushort value, int offset = 0)
        {
            BinaryPrimitives.WriteUInt16BigEndian(buffer.Slice(offset), value);
        }
        public static void WriteToBufferLE(Span<byte> buffer, uint value, int offset = 0)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(offset), value);
        }
        public static void WriteToBufferBE(Span<byte> buffer, uint value, int offset = 0)
        {
            BinaryPrimitives.WriteUInt32BigEndian(buffer.Slice(offset), value);
        }
        public static void WriteToBufferLE(Span<byte> buffer, ulong value, int offset = 0)
        {
            BinaryPrimitives.WriteUInt64LittleEndian(buffer.Slice(offset), value);
        }
        public static void WriteToBufferBE(Span<byte> buffer, ulong value, int offset = 0)
        {
            BinaryPrimitives.WriteUInt64BigEndian(buffer.Slice(offset), value);
        }

        // Conversion methods for easier migration from the old FloatByteMap/DoubleByteMap helpers.
        public static void WriteToBufferLE(Span<byte> buffer, float value, int offset = 0)
        {
            WriteToBufferLE(buffer, new FloatUIntMap { Float = value }.UInt, offset);
        }
        public static void WriteToBufferBE(Span<byte> buffer, float value, int offset = 0)
        {
            WriteToBufferBE(buffer, new FloatUIntMap { Float = value }.UInt, offset);
        }
        public static void WriteToBufferLE(Span<byte> buffer, double value, int offset = 0)
        {
            WriteToBufferLE(buffer, new DoubleULongMap { Double = value }.ULong, offset);
        }
        public static void WriteToBufferBE(Span<byte> buffer, double value, int offset = 0)
        {
            WriteToBufferBE(buffer, new DoubleULongMap { Double = value }.ULong, offset);
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct FloatUIntMap
        {
            [FieldOffset(0)] public float Float;
            [FieldOffset(0)] public uint UInt;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct DoubleULongMap
        {
            [FieldOffset(0)] public double Double;
            [FieldOffset(0)] public ulong ULong;
        }
    }
}
