using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Bitter.Tests
{
    [TestClass]
    public class TypeReadWriteTests
    {
        [TestMethod]
        public void Type_RoundTrips_ReadWriteObject()
        {
            BinaryStream stream = TestHelpers.NewStream();
            Vector3Model original = new Vector3Model { X = 1f, Y = -2f, Z = 3.5f };

            stream.Write.Type(original);
            stream.Rewind();

            Vector3Model result = stream.Read.Type<Vector3Model>();
            result.ShouldBe(original);
        }

        [TestMethod]
        public void TypeList_RoundTrips_ReadWriteObjects_WithIntCount()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<Vector3Model> original = new List<Vector3Model>
            {
                new Vector3Model { X = 1f, Y = 2f, Z = 3f },
                new Vector3Model { X = -1f, Y = -2f, Z = -3f },
            };

            stream.Write.TypeList(original);
            stream.Rewind();

            List<Vector3Model> result = stream.Read.TypeList<Vector3Model>(original.Count);
            result.ShouldBe(original);
        }

        [TestMethod]
        public void TypeList_RoundTrips_ReadWriteObjects_WithUintCount()
        {
            BinaryStream stream = TestHelpers.NewStream();
            List<Vector3Model> original = new List<Vector3Model>
            {
                new Vector3Model { X = 1f, Y = 2f, Z = 3f },
            };

            stream.Write.TypeList(original);
            stream.Rewind();

            List<Vector3Model> result = stream.Read.TypeList<Vector3Model>((uint)original.Count);
            result.ShouldBe(original);
        }

        [TestMethod]
        public void Type_RoundTrips_ReadWriteInitializeObject_WithParameters()
        {
            BinaryStream stream = TestHelpers.NewStream();

            ScaledIntModel toWrite = new ScaledIntModel { Multiplier = 5, Value = 25 };
            stream.Write.Type(toWrite);
            stream.Rewind();

            ScaledIntModel result = stream.Read.Type<ScaledIntModel>(5);
            result.Value.ShouldBe(25);
        }

        [TestMethod]
        public void TypeList_RoundTrips_ReadWriteInitializeObjects_WithIntCount()
        {
            BinaryStream stream = TestHelpers.NewStream();
            stream.Write.Int(2);
            stream.Write.Int(4);
            stream.Rewind();

            List<ScaledIntModel> result = stream.Read.TypeList<ScaledIntModel>(2, 2);

            result.Count.ShouldBe(2);
            result[0].Value.ShouldBe(4);
            result[1].Value.ShouldBe(8);
        }

        [TestMethod]
        public void TypeList_RoundTrips_ReadWriteInitializeObjects_WithUintCount()
        {
            BinaryStream stream = TestHelpers.NewStream();
            stream.Write.Int(3);
            stream.Rewind();

            List<ScaledIntModel> result = stream.Read.TypeList<ScaledIntModel>((uint)1, 3);

            result.Count.ShouldBe(1);
            result[0].Value.ShouldBe(9);
        }
    }
}
