using System.IO;
using BenchmarkDotNet.Attributes;

namespace Bitter.Benchmarks
{
    // Round-trips a mix of scalar primitives 
    [MemoryDiagnoser]
    [SimpleJob(warmupCount: 3, iterationCount: 7)]
    public class ScalarReadWriteBenchmarks
    {
        private const int Iterations = 10_000;
        private const int BytesPerRound = 2 + 4 + 8 + 4 + 8; // UShort + UInt + ULong + Float + Double

        private BinaryStream _readStream;
        private BinaryStream _writeStream;

        [GlobalSetup]
        public void Setup()
        {
            var ms = new MemoryStream();
            var writer = new BinaryStream(ms);
            for (int i = 0; i < Iterations; i++)
            {
                writer.Write.UShort((ushort)i);
                writer.Write.UInt((uint)i);
                writer.Write.ULong((ulong)i);
                writer.Write.Float(i * 1.5f);
                writer.Write.Double(i * 1.5d);
            }
            writer.Flush();

            _readStream = new BinaryStream(ms);
            _writeStream = new BinaryStream(new MemoryStream(new byte[Iterations * BytesPerRound]));
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public long ReadScalars()
        {
            _readStream.ByteOffset = 0;

            long acc = 0;
            for (int i = 0; i < Iterations; i++)
            {
                acc += _readStream.Read.UShort();
                acc += _readStream.Read.UInt();
                acc += (long)_readStream.Read.ULong();
                acc += (long)_readStream.Read.Float();
                acc += (long)_readStream.Read.Double();
            }
            return acc;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public void WriteScalars()
        {
            _writeStream.ByteOffset = 0;

            for (int i = 0; i < Iterations; i++)
            {
                _writeStream.Write.UShort((ushort)i);
                _writeStream.Write.UInt((uint)i);
                _writeStream.Write.ULong((ulong)i);
                _writeStream.Write.Float(i * 1.5f);
                _writeStream.Write.Double(i * 1.5d);
            }
        }
    }
}
