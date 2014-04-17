using System;
using System.Collections.Generic;

namespace Dommy.Extensions.Kinect
{
    public class MovementChecker
    {
        private JointType jointType;

        private int step = 0;

        private List<Func<Vector, bool>> checkMovements = new List<Func<Vector, bool>>();
        private Vector actualPosition;
        private DateTime lastPosition = DateTime.MinValue;
        private TimeSpan timeToExecute;

        public MovementChecker(JointType jointType, TimeSpan time)
        {
            this.jointType = jointType;
            this.timeToExecute = time;
        }

        public MovementChecker To(Func<Vector, bool> predicate)
        {
            // Add function to the list.
            this.checkMovements.Add(predicate);
            return this;
        }

        public MovementChecker ToRight(int v)
        {
            return this.To(m => m.X >= v);
        }

        public MovementChecker ToLeft(int v)
        {
            return this.To(m => -m.X >= v);
        }

        public bool Check(ISkeleton skeleton)
        {
            // Update workflow movement

            // Update current position
            Vector position = skeleton[this.jointType];

            if (this.actualPosition == null || DateTime.Now.Subtract(this.lastPosition) > this.timeToExecute)
            {
                this.actualPosition = position;
                this.lastPosition = DateTime.Now;
                return false;
            }

            // Create vector movement
            Vector movement = position - this.actualPosition;

            if (this.checkMovements[this.step](movement))
            {
                this.step++;
                // New position is the actual position + movement

                this.actualPosition += movement;
                this.lastPosition = DateTime.Now;
            }

            return this.step >= this.checkMovements.Count;
        }
    }
}