using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using static Bitter.BinaryStream;

namespace Bitter.Tests
{
    [TestClass]
    public class BinaryWrapperTests
    {
        [TestMethod]
        public void Write_And_Read_ByteArray_RoundTrips()
        {
            Vector3Model original = new Vector3Model { X = 1f, Y = 2f, Z = 3f };

            original.Write(out byte[] bytes);
            bytes.Length.ShouldBe(12);

            Vector3Model result = new Vector3Model();
            result.Read(bytes);

            result.ShouldBe(original);
        }

        [TestMethod]
        public void Write_And_Read_Stream_RoundTrips()
        {
            // Plain BinaryWrapper.Write(Stream)/Read(Stream) dispose the stream afterwards,
            // so a stream that's reused across both calls needs keepWriteOpen/keepReadOpen.
            Vector3ModelLeaveStreamOpen original = new Vector3ModelLeaveStreamOpen { X = 4f, Y = 5f, Z = 6f };
            using MemoryStream ms = new MemoryStream();

            original.Write(ms);
            ms.Position = 0;

            Vector3ModelLeaveStreamOpen result = new Vector3ModelLeaveStreamOpen();
            result.Read(ms);

            result.ShouldBe(original);
        }

        [TestMethod]
        public void Write_Stream_DisposesStreamByDefault()
        {
            Vector3Model original = new Vector3Model { X = 1f, Y = 1f, Z = 1f };
            MemoryStream ms = new MemoryStream();

            original.Write(ms);

            Should.Throw<ObjectDisposedException>(() => ms.Position = 0);
        }

        [TestMethod]
        public void Write_And_Read_File_RoundTrips()
        {
            string path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            try
            {
                Vector3Model original = new Vector3Model { X = -1f, Y = 0f, Z = 100f };
                original.Write(path);

                Vector3Model result = new Vector3Model();
                result.Read(path);

                result.ShouldBe(original);
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        [TestMethod]
        public void Read_File_ThatThrows_ClosesTheFile()
        {
            string path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            try
            {
                File.WriteAllBytes(path, new byte[12]);
                ThrowingModel model = new ThrowingModel();
                Action read = () => model.Read(path);
                read.ShouldThrow<InvalidDataException>();

                Action delete = () => File.Delete(path);

                delete.ShouldNotThrow();
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        [TestMethod]
        public void Read_File_OpenElsewhereForReading_Reads()
        {
            string path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            try
            {
                Vector3Model original = new Vector3Model { X = 1f, Y = 2f, Z = 3f };
                original.Write(path);
                using FileStream other = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

                Vector3Model result = new Vector3Model();
                result.Read(path);

                result.ShouldBe(original);
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        [TestMethod]
        public void Read_ReadOnlyFile_Reads()
        {
            string path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            try
            {
                Vector3Model original = new Vector3Model { X = 1f, Y = 2f, Z = 3f };
                original.Write(path);
                File.SetAttributes(path, FileAttributes.ReadOnly);

                Vector3Model result = new Vector3Model();
                result.Read(path);

                result.ShouldBe(original);
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.SetAttributes(path, FileAttributes.Normal);
                    File.Delete(path);
                }
            }
        }

        [TestMethod]
        public void Write_File_ThatThrows_ClosesTheFile()
        {
            string path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            try
            {
                ThrowingModel model = new ThrowingModel();
                Action write = () => model.Write(path);
                write.ShouldThrow<InvalidDataException>();

                Action delete = () => File.Delete(path);

                delete.ShouldNotThrow();
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        [TestMethod]
        public void Write_File_OverLongerFile_ReplacesIt()
        {
            string path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            try
            {
                File.WriteAllBytes(path, new byte[100]);
                Vector3Model model = new Vector3Model { X = 1f, Y = 2f, Z = 3f };

                model.Write(path);
                long length = new FileInfo(path).Length;

                length.ShouldBe(12);
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        [TestMethod]
        public void ByteOrder_Affects_Serialization()
        {
            Vector3Model model = new Vector3Model { X = 1f, Y = 2f, Z = 3f };
            model.ByteOrder = Endianness.BigEndian;

            model.Write(out byte[] beBytes);

            model.ByteOrder = Endianness.LittleEndian;
            model.Write(out byte[] leBytes);

            beBytes.ShouldNotBe(leBytes);
        }

        [TestMethod]
        public void DefaultTextEncoding_DefaultsToDefault()
        {
            Vector3Model model = new Vector3Model();
            model.DefaultTextEncoding.ShouldBe(TextEncoding.DEFAULT);
        }
    }
}
