// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Vector.cs" company="TrollCorp">
//   Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
// <summary>
//   Vector class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Dommy.Extensions.Kinect
{
    /// <summary>
    /// Vector class.
    /// </summary>
    public class Vector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// </summary>
        /// <param name="x">X position.</param>
        /// <param name="y">Y position.</param>
        /// <param name="z">Z position.</param>
        public Vector(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Gets the X position of vector.
        /// </summary>
        public float X { get; private set; }

        /// <summary>
        /// Gets the Y position of vector.
        /// </summary>
        public float Y { get; private set; }

        /// <summary>
        /// Gets the Z position of vector.
        /// </summary>
        public float Z { get; private set; }

        /// <summary>
        /// Add two vector.
        /// </summary>
        /// <param name="vector1">Vector 1</param>
        /// <param name="vector2">Vector 2</param>
        /// <returns>Added vector.</returns>
        public static Vector operator +(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X + vector2.X, vector1.Y + vector2.Y, vector1.Z + vector2.Z);
        }

        /// <summary>
        /// Subtract two vector.
        /// </summary>
        /// <param name="vector1">Vector 1</param>
        /// <param name="vector2">Vector 2</param>
        /// <returns>Subtracted vector.</returns>
        public static Vector operator -(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X - vector2.X, vector1.Y - vector2.Y, vector1.Z - vector2.Z);
        }

        /// <summary>
        /// Verify equality between two vectors.
        /// </summary>
        /// <param name="obj">Vector to compare.</param>
        /// <returns>Indicate if vectors are equals.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((Vector)obj);
        }

        /// <summary>
        /// Get hash code for vector.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.X.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Y.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Z.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Convert vector to string.
        /// </summary>
        /// <returns>Display string of vector.</returns>
        public override string ToString()
        {
            return string.Format("X:{0}, Y: {1},Z: {2}", (int)this.X, (int)this.Y, (int)this.Z);
        }

        /// <summary>
        /// Equality of to vector.
        /// </summary>
        /// <param name="vector">Vector to compare.</param>
        /// <returns>Indicate if two vector is equals.</returns>
        private bool Equals(Vector vector)
        {
            return this.X.Equals(vector.X) && this.Y.Equals(vector.Y) && this.Z.Equals(vector.Z);
        }
    }
}