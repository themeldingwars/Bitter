using static Bitter.BinaryWrapper;

namespace Bitter.Tests
{
    public class Vector3Model : BinaryWrapper, ReadWrite
    {
        public float X;
        public float Y;
        public float Z;

        public override void Read(BinaryStream bs)
        {
            X = bs.Read.Float();
            Y = bs.Read.Float();
            Z = bs.Read.Float();
        }

        public override void Write(BinaryStream bs)
        {
            bs.Write.Float(X);
            bs.Write.Float(Y);
            bs.Write.Float(Z);
        }

        public override bool Equals(object obj)
        {
            return obj is Vector3Model other && X == other.X && Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            return (X, Y, Z).GetHashCode();
        }
    }

    /// <summary>
    /// BinaryWrapper.Write(Stream)/Read(Stream) dispose the stream afterwards unless
    /// keepWriteOpen/keepReadOpen are set, so this variant leaves it open for reuse in tests.
    /// </summary>
    public class Vector3ModelLeaveStreamOpen : Vector3Model
    {
        public Vector3ModelLeaveStreamOpen()
        {
            keepReadOpen = true;
            keepWriteOpen = true;
        }
    }

    public class ScaledIntModel : BinaryWrapper, ReadWriteInitialize
    {
        public int Multiplier;
        public int Value;

        public void Init(object parameters = null)
        {
            Multiplier = parameters is int m ? m : 1;
        }

        public override void Read(BinaryStream bs)
        {
            Value = bs.Read.Int() * Multiplier;
        }

        public override void Write(BinaryStream bs)
        {
            bs.Write.Int(Value / Multiplier);
        }
    }
}
