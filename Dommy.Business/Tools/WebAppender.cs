//-----------------------------------------------------------------------
// <copyright file="WebAppender.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Tools
{
    using System;

    using JetBrains.Annotations;

    using log4net.Appender;
    using Microsoft.AspNet.SignalR.Client;

    /// <summary>
    /// Logger for display log on web interface.
    /// </summary>
    [UsedImplicitly]
    public sealed class WebAppender : AppenderSkeleton, IDisposable
    {
        /// <summary>
        /// Indicate that the web logger is started.
        /// </summary>
        private static bool isStarted;

        /// <summary>
        /// The connection.
        /// </summary>
        private HubConnection connection;

        /// <summary>
        /// Errors logger.
        /// </summary>
        private IHubProxy logger;

        /// <summary>
        /// Indicate that the web server is started.
        /// </summary>
        public static void WebStarted()
        {
            isStarted = true;
        }

        /// <summary>
        /// Dispose object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Append a new message.
        /// </summary>
        /// <param name="loggingEvent">Logging message.</param>
        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            if (isStarted)
            {
                bool init = true;
                if (this.connection == null)
                {
                    init = this.InitConnection();
                }

                if (init)
                {
                    this.logger.Invoke("send", loggingEvent.RenderedMessage);
                }
            }
        }

        /// <summary>
        /// Initialize connection.
        /// </summary>
        /// <returns>Indicate that the connection is open.</returns>
        private bool InitConnection()
        {
            try
            {
                this.connection = new HubConnection("http://localhost:5000");
                this.logger = this.connection.CreateHubProxy("logger");
                this.connection.Start().Wait();
                return true;
            }
            catch
            {
                this.connection = null;
                this.logger = null;
            }

            return false;
        }

        /// <summary>
        /// Dispose the logger.
        /// </summary>
        /// <param name="disposing">Indicate if dispose .Net object.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.connection != null)
                {
                    this.connection.Dispose();
                }
            }
        }
    }
}
