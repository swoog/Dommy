// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementChecker.cs" company="TrollCorp">
//   Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
// <summary>
//   Defines the MovementChecker type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Dommy.Extensions.Kinect
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// The movement checker.
    /// </summary>
    public class MovementChecker
    {
        /// <summary>
        /// All vectors of the movement.
        /// </summary>
        private readonly List<Vector> checkMovements = new List<Vector>();

        /// <summary>
        /// The joint type to check.
        /// </summary>
        private readonly BodyJointType jointType;

        /// <summary>
        /// Time to execute the movement.
        /// </summary>
        private readonly TimeSpan timeToExecute;

        /// <summary>
        /// The step number of the checker.
        /// </summary>
        private int step;

        /// <summary>
        /// Actual position.
        /// </summary>
        private Vector actualPosition;

        /// <summary>
        /// Last position.
        /// </summary>
        private DateTime lastPosition = DateTime.MinValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementChecker"/> class.
        /// </summary>
        /// <param name="jointType">Joint type to check.</param>
        /// <param name="time">Time to check the movement.</param>
        public MovementChecker(BodyJointType jointType, TimeSpan time)
        {
            this.jointType = jointType;
            this.timeToExecute = time;
        }

        /// <summary>
        /// Describe a vector movement.
        /// </summary>
        /// <param name="vector">Vector of the movement.</param>
        /// <returns>Movement checker.</returns>
        public MovementChecker To(Vector vector)
        {
            // Add function to the list.
            this.checkMovements.Add(vector);
            return this;
        }

        /// <summary>
        /// Describe a vector movement.
        /// </summary>
        /// <param name="x">Right movement.</param>
        /// <returns>Movement checker.</returns>
        public MovementChecker ToRight(int x)
        {
            return this.To(new Vector(x, 0, 0));
        }

        /// <summary>
        /// Describe a vector movement.
        /// </summary>
        /// <param name="x">Left movement.</param>
        /// <returns>Movement checker.</returns>
        public MovementChecker ToLeft(int x)
        {
            return this.To(new Vector(-x, 0, 0));
        }

        /// <summary>
        /// Increment the skeleton movement.
        /// </summary>
        /// <param name="skeleton">Skeleton to check.</param>
        /// <returns>Indicate if the check is good.</returns>
        public bool Check(ISkeleton skeleton)
        {
            // Update workflow movement

            // Update current position
            Vector position = skeleton[this.jointType];

            if (this.actualPosition == null || DateTime.Now.Subtract(this.lastPosition) > this.timeToExecute)
            {
                Debug.WriteLine("position({0}) : timeToExecute", position);
                this.actualPosition = position;
                this.lastPosition = DateTime.Now;
                return false;
            }

            // Create vector movement
            var movement = position - this.actualPosition;

            if (this.Check(this.checkMovements[this.step], movement))
            {
                this.step++;

                // New position is the actual position + movement
                this.actualPosition += movement;
                this.lastPosition = DateTime.Now;
            }

            if (this.step >= this.checkMovements.Count)
            {
                this.step = 0;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check movement for a vector.
        /// </summary>
        /// <param name="vector">Vector to check.</param>
        /// <param name="movement">Movement checked.</param>
        /// <returns>Indicate that the movement is good.</returns>
        private bool Check(Vector vector, Vector movement)
        {
            float? averageX = vector.X == 0 ? default(float?) : movement.X / vector.X;
            float? averageY = vector.Y == 0 ? default(float?) : movement.Y / vector.Y;
            float? averageZ = vector.Z == 0 ? default(float?) : movement.Z / vector.Z;

            if (this.AvgEqual(averageX, averageY, averageZ))
            {
                if ((!averageX.HasValue || averageX >= 1)
                    && (!averageY.HasValue || averageY >= 1)
                    && (!averageZ.HasValue || averageZ >= 1))
                {
                    Debug.WriteLine("actual({0}), movement({1}) : Check Step {2}", this.actualPosition, movement, this.step);
                    return true;
                }

                if ((!averageX.HasValue || (averageX >= 0 && averageX < 1))
                    && (!averageY.HasValue || (averageY >= 0 && averageY < 1))
                    && (!averageZ.HasValue || (averageZ >= 0 && averageZ < 1)))
                {
                    Debug.WriteLine("actual({0}), movement({1}) : progresse Step {2}", this.actualPosition, movement, this.step);
                    return false;
                }
            }

            Debug.WriteLine("actual({0}), movement({1}) : not equal to Step {2}", this.actualPosition, movement, this.step);

            if (this.step != 0 && this.Check(this.checkMovements[0], movement))
            {
                this.step = 1;
            }
            else
            {
                this.step = 0;
            }

            // New position is the actual position + movement
            this.actualPosition += movement;
            this.lastPosition = DateTime.Now;

            return false;
        }

        /// <summary>
        /// Average equal. Used to verify if the vector is similar.
        /// </summary>
        /// <param name="x">X movement to test.</param>
        /// <param name="y">Y movement to test.</param>
        /// <param name="z">Z movement to test.</param>
        /// <returns>Indicate if the movement is similar.</returns>
        private bool AvgEqual(float? x, float? y, float? z)
        {
            if (x.HasValue && y.HasValue && (x > y + 0.20 || x < y - 0.20))
            {
                return false;
            }

            if (x.HasValue && y.HasValue && (y > z + 0.20 || y < z - 0.20))
            {
                return false;
            }

            if (x.HasValue && y.HasValue && (z > x + 0.20 || z < x - 0.20))
            {
                return false;
            }

            return true;
        }
    }
}