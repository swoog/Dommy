
namespace Dommy.Extensions.Kinect
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class MovementChecker
    {
        private BodyJointType jointType;

        private int step = 0;

        private List<Vector> checkMovements = new List<Vector>();
        private Vector actualPosition;
        private DateTime lastPosition = DateTime.MinValue;
        private TimeSpan timeToExecute;

        public MovementChecker(BodyJointType jointType, TimeSpan time)
        {
            this.jointType = jointType;
            this.timeToExecute = time;
        }

        public MovementChecker To(Vector vectore)
        {
            // Add function to the list.
            this.checkMovements.Add(vectore);
            return this;
        }

        public MovementChecker ToRight(int x)
        {
            return this.To(new Vector(x, 0, 0));
        }

        public MovementChecker ToLeft(int x)
        {
            return this.To(new Vector(-x, 0, 0));
        }

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
            Vector movement = position - this.actualPosition;

            if (Check(this.checkMovements[this.step], movement))
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
            else
            {
                return false;
            }
        }

        private bool Check(Vector vector, Vector movement)
        {
            float? averageX = vector.X == 0 ? default(float?) : movement.X / vector.X;
            float? averageY = vector.Y == 0 ? default(float?) : movement.Y / vector.Y;
            float? averageZ = vector.Z == 0 ? default(float?) : movement.Z / vector.Z;

            if (AvgEqual(averageX, averageY, averageZ))
            {
                if ((!averageX.HasValue || averageX >= 1)
                    && (!averageY.HasValue || averageY >= 1)
                    && (!averageZ.HasValue || averageZ >= 1))
                {
                    Debug.WriteLine("actual({0}), movement({1}) : Check Step {2}", this.actualPosition, movement, this.step);
                    return true;
                }
                else if ((!averageX.HasValue || (averageX >= 0 && averageX < 1))
                    && (!averageY.HasValue || (averageY >= 0 && averageY < 1))
                    && (!averageZ.HasValue || (averageZ >= 0 && averageZ < 1)))
                {
                    Debug.WriteLine("actual({0}), movement({1}) : progresse Step {2}", this.actualPosition, movement, this.step);
                    return false;
                }
            }

            Debug.WriteLine("actual({0}), movement({1}) : not equal to Step {2}", this.actualPosition, movement, this.step);

            if (this.step != 0 && Check(this.checkMovements[0], movement))
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

        private bool AvgEqual(float? x, float? y, float? z)
        {
            if (x.HasValue && y.HasValue && (x > y + 0.20 || x < y - 0.20))
            {
                return false;
            }
            else if (x.HasValue && y.HasValue && (y > z + 0.20 || y < z - 0.20))
            {
                return false;
            }
            else if (x.HasValue && y.HasValue && (z > x + 0.20 || z < x - 0.20))
            {
                return false;
            }

            return true;
        }
    }
}