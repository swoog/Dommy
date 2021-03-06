﻿//-----------------------------------------------------------------------
// <copyright file="Kinect2SensorSelector.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Extensions.Kinect.Sdk2
{
    using System;
    using System.Linq;
    using System.Threading;
    using Microsoft.Kinect;
    using Ninject.Extensions.Logging;

    /// <summary>
    /// Make choice of the Kinect sensor.
    /// </summary>
    public class Kinect2SensorSelector : IDisposable
    {
        /// <summary>
        /// Sensor logger.
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Kinect2SensorSelector"/> class.
        /// </summary>
        /// <param name="logger">Sensor logger.</param>
        public Kinect2SensorSelector(ILogger logger)
        {
            this.logger = logger;
            this.Sensor = KinectSensor.GetDefault();
        }

        /// <summary>
        /// Gets instance of the kinect sensor.
        /// </summary>
        public KinectSensor Sensor { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the sensor is found.
        /// </summary>
        public bool IsKinectFound
        {
            get
            {
                return this.Sensor != null;
            }
        }

        /// <summary>
        /// Start sensor.
        /// </summary>
        public void Start()
        {
            if (this.IsKinectFound)
            {
                lock (this.Sensor)
                {
                    if (!this.Sensor.IsOpen)
                    {
                        this.logger.Info("Sensor found");
                        this.Sensor.Open();

                        this.logger.Info("Sensor initialized");
                    }
                }
            }
        }

        /// <summary>
        /// Stop sensor.
        /// </summary>
        public void Stop()
        {
            if (this.IsKinectFound && this.Sensor.IsOpen)
            {
                this.logger.Info("Sensor stoping...");
                this.Sensor.Close();
                this.logger.Info("Sensor stoped");
            }
        }

        /// <summary>
        /// Dispose kinect.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Dispose kinect.
        /// </summary>
        /// <param name="b">Indicate dispose finalize.</param>
        public void Dispose(bool b)
        {
            if (this.IsKinectFound)
            {
                this.Sensor.Close();
            }

            GC.SuppressFinalize(this);
        }
    }
}
