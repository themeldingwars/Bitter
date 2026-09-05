using System.IO;
using BenchmarkDotNet.Attributes;

namespace Bitter.Benchmarks
{
    // Writes whole numeric arrays to a stream.
    [MemoryDiagnoser]
    [SimpleJob(warmupCount: 3, iterationCount: 7)]
    public class ArrayWriteBenchmarks
    {
        [Params(1_000, 20_000)]
        public int Count;

        private int[] _ints;
        private uint[] _uints;
        private short[] _shorts;
        private long[] _longs;
        private float[] _floats;
        private double[] _doubles;

        private BinaryStream _intStream;
        private BinaryStream _uintStream;
        private BinaryStream _shortStream;
        private BinaryStream _longStream;
        private BinaryStream _floatStream;
        private BinaryStream _doubleStream;

        [GlobalSetup]
        public void Setup()
        {
            _ints = new int[Count];
            _uints = new uint[Count];
            _shorts = new short[Count];
            _longs = new long[Count];
            _floats = new float[Count];
            _doubles = new double[Count];
            for (int i = 0; i < Count; i++)
            {
                _ints[i] = i;
                _uints[i] = (uint)i;
                _shorts[i] = (short)i;
                _longs[i] = i;
                _floats[i] = i * 1.5f;
                _doubles[i] = i * 1.5d;
            }

            _intStream = new BinaryStream(new MemoryStream(new byte[Count * 4]));
            _uintStream = new BinaryStream(new MemoryStream(new byte[Count * 4]));
            _shortStream = new BinaryStream(new MemoryStream(new byte[Count * 2]));
            _longStream = new BinaryStream(new MemoryStream(new byte[Count * 8]));
            _floatStream = new BinaryStream(new MemoryStream(new byte[Count * 4]));
            _doubleStream = new BinaryStream(new MemoryStream(new byte[Count * 8]));
        }

        [Benchmark]
        public void IntArray()
        {
            _intStream.ByteOffset = 0;
            _intStream.Write.IntArray(_ints);
        }

        [Benchmark]
        public void UIntArray()
        {
            _uintStream.ByteOffset = 0;
            _uintStream.Write.UIntArray(_uints);
        }

        [Benchmark]
        public void ShortArray()
        {
            _shortStream.ByteOffset = 0;
            _shortStream.Write.ShortArray(_shorts);
        }

        [Benchmark]
        public void LongArray()
        {
            _longStream.ByteOffset = 0;
            _longStream.Write.LongArray(_longs);
        }

        [Benchmark]
        public void FloatArray()
        {
            _floatStream.ByteOffset = 0;
            _floatStream.Write.FloatArray(_floats);
        }

        [Benchmark]
        public void DoubleArray()
        {
            _doubleStream.ByteOffset = 0;
            _doubleStream.Write.DoubleArray(_doubles);
        }
    }
}
