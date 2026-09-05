using System.IO;
using BenchmarkDotNet.Attributes;

namespace Bitter.Benchmarks
{
    // Reads whole numeric arrays back out of a stream.
    [MemoryDiagnoser]
    [SimpleJob(warmupCount: 3, iterationCount: 7)]
    public class ArrayReadBenchmarks
    {
        [Params(1_000, 20_000)]
        public int Count;

        private BinaryStream _intStream;
        private BinaryStream _uintStream;
        private BinaryStream _shortStream;
        private BinaryStream _longStream;
        private BinaryStream _floatStream;
        private BinaryStream _doubleStream;

        [GlobalSetup]
        public void Setup()
        {
            _intStream = BuildStream(s => s.Write.IntArray(MakeInts()));
            _uintStream = BuildStream(s => s.Write.UIntArray(MakeUInts()));
            _shortStream = BuildStream(s => s.Write.ShortArray(MakeShorts()));
            _longStream = BuildStream(s => s.Write.LongArray(MakeLongs()));
            _floatStream = BuildStream(s => s.Write.FloatArray(MakeFloats()));
            _doubleStream = BuildStream(s => s.Write.DoubleArray(MakeDoubles()));
        }

        private static BinaryStream BuildStream(System.Action<BinaryStream> write)
        {
            var ms = new MemoryStream();
            var stream = new BinaryStream(ms);
            write(stream);
            stream.Flush();
            return new BinaryStream(ms);
        }

        private int[] MakeInts()
        {
            var a = new int[Count];
            for (int i = 0; i < Count; i++) a[i] = i;
            return a;
        }

        private uint[] MakeUInts()
        {
            var a = new uint[Count];
            for (int i = 0; i < Count; i++) a[i] = (uint)i;
            return a;
        }

        private short[] MakeShorts()
        {
            var a = new short[Count];
            for (int i = 0; i < Count; i++) a[i] = (short)i;
            return a;
        }

        private long[] MakeLongs()
        {
            var a = new long[Count];
            for (int i = 0; i < Count; i++) a[i] = i;
            return a;
        }

        private float[] MakeFloats()
        {
            var a = new float[Count];
            for (int i = 0; i < Count; i++) a[i] = i * 1.5f;
            return a;
        }

        private double[] MakeDoubles()
        {
            var a = new double[Count];
            for (int i = 0; i < Count; i++) a[i] = i * 1.5d;
            return a;
        }

        [Benchmark]
        public int[] IntArray()
        {
            _intStream.ByteOffset = 0;
            return _intStream.Read.IntArray(Count);
        }

        [Benchmark]
        public uint[] UIntArray()
        {
            _uintStream.ByteOffset = 0;
            return _uintStream.Read.UIntArray(Count);
        }

        [Benchmark]
        public short[] ShortArray()
        {
            _shortStream.ByteOffset = 0;
            return _shortStream.Read.ShortArray(Count);
        }

        [Benchmark]
        public long[] LongArray()
        {
            _longStream.ByteOffset = 0;
            return _longStream.Read.LongArray(Count);
        }

        [Benchmark]
        public float[] FloatArray()
        {
            _floatStream.ByteOffset = 0;
            return _floatStream.Read.FloatArray(Count);
        }

        [Benchmark]
        public double[] DoubleArray()
        {
            _doubleStream.ByteOffset = 0;
            return _doubleStream.Read.DoubleArray(Count);
        }
    }
}
