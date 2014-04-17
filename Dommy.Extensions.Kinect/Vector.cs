namespace Dommy.Extensions.Kinect
{
    public class Vector
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public Vector(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Vector operator +(Vector m1, Vector m2)
        {
            return new Vector(m1.X + m2.X, m1.Y + m2.Y, m1.Z + m2.Z);
        }

        public static Vector operator -(Vector m1, Vector m2)
        {
            return new Vector(m1.X - m2.X, m1.Y - m2.Y, m1.Z - m2.Z);
        }

        public override bool Equals(object obj)
        {
            var v2 = (Vector)obj;

            return this.X == v2.X && this.Y == v2.Y && this.Z == v2.Z;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}