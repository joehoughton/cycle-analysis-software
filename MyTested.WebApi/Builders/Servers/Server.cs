﻿// MyTested.WebApi - ASP.NET Web API Fluent Testing Framework
// Copyright (C) 2015 Ivaylo Kenov.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
namespace MyTested.WebApi.Builders.Servers
{
    using System;
    using System.Web.Http;
    using Common.Servers;
    using Contracts.Servers;
    using Utilities.Validators;

    /// <summary>
    /// Provides options to start and stop HTTP servers, as well as processing HTTP requests for full pipeline testing.
    /// </summary>
    public class Server : IServer
    {
        /// <summary>
        /// Starts new global HTTP server.
        /// </summary>
        /// <param name="httpConfiguration">Optional HTTP configuration to use. If no configuration is provided, the global configuration will be used instead.</param>
        /// <returns>Server builder.</returns>
        public IServerBuilder Starts(HttpConfiguration httpConfiguration = null)
        {
            if (httpConfiguration == null)
            {
                HttpConfigurationValidator.ValidateGlobalConfiguration("server pipeline");
                httpConfiguration = MyWebApi.Configuration;
            }

            HttpTestServer.StartGlobal(httpConfiguration);
            return this.Working();
        }

        /// <summary>
        /// Starts new global OWIN server.
        /// </summary>
        /// <typeparam name="TStartup">OWIN startup class to use.</typeparam>
        /// <param name="port">Network port on which the server will listen for requests.</param>
        /// <param name="host">Network host on which the server will listen for requests.</param>
        /// <returns>Server builder.</returns>
        public IServerBuilder Starts<TStartup>(int port = OwinTestServer.DefaultPort, string host = OwinTestServer.DefaultHost)
        {
            OwinTestServer.StartGlobal<TStartup>(this.GetStartOptions(port, host));
            return this.Working();
        }

        /// <summary>
        /// Configures global remote server.
        /// </summary>
        /// <param name="baseAddress">Base address to use for the requests.</param>
        /// <returns>Server builder.</returns>
        public IServerBuilder IsLocatedAt(string baseAddress)
        {
            RemoteServer.ConfigureGlobal(baseAddress);
            return this.WorkingRemotely();
        }

        /// <summary>
        /// Stops all currently started global HTTP or OWIN servers.
        /// </summary>
        public void Stops()
        {
            var httpServerStoppedSuccessfully = HttpTestServer.StopGlobal();
            var owinServerStoppedSuccessfully = OwinTestServer.StopGlobal();

            if (!httpServerStoppedSuccessfully && !owinServerStoppedSuccessfully)
            {
                throw new InvalidOperationException("There are no running test servers to stop. Calling MyWebApi.Server().Stops() should be done only after MyWebApi.Server.Starts() is invoked.");
            }
        }

        /// <summary>
        /// Processes HTTP requests on globally started HTTP or OWIN servers. If global OWIN server is started, it will be used. If not the method will check for global HTTP server to use. If such is not found, a new instance of HTTP server is created with the global HTTP configuration.
        /// </summary>
        /// <returns>Server builder to set specific HTTP requests.</returns>
        public IServerBuilder Working()
        {
            if (OwinTestServer.GlobalIsStarted)
            {
                return new ServerTestBuilder(OwinTestServer.GlobalClient);
            }

            if (HttpTestServer.GlobalIsStarted)
            {
                return new ServerTestBuilder(HttpTestServer.GlobalClient, transformRequest: true);
            }

            if (MyWebApi.Configuration != null)
            {
                return this.Working(MyWebApi.Configuration);
            }

            throw new InvalidOperationException("No test servers are started or could be started for this particular test case. Either call MyWebApi.Server().Starts() to start a new test server or provide global or test specific HttpConfiguration.");
        }

        /// <summary>
        /// Starts new HTTP server to process a request.
        /// </summary>
        /// <param name="httpConfiguration">HTTP configuration to use in the testing.</param>
        /// <returns>Server builder to set specific HTTP requests.</returns>
        public IServerBuilder Working(HttpConfiguration httpConfiguration)
        {
            return new ServerTestBuilder(HttpTestServer.CreateNewClient(httpConfiguration), transformRequest: true, disposeServer: true);
        }

        /// <summary>
        /// Starts new OWIN server to process a request.
        /// </summary>
        /// <typeparam name="TStartup">OWIN startup class to use.</typeparam>
        /// <param name="port">Network port on which the server will listen for requests.</param>
        /// <param name="host">Network host on which the server will listen for requests.</param>
        /// <returns>Server builder to set specific HTTP requests.</returns>
        public IServerBuilder Working<TStartup>(int port = OwinTestServer.DefaultPort, string host = OwinTestServer.DefaultHost)
        {
            var options = this.GetStartOptions(port, host);
            var server = OwinTestServer.CreateNewServer<TStartup>(options);
            return new ServerTestBuilder(server.HttpClient, disposeServer: true, server: server);
        }

        /// <summary>
        /// Processes HTTP request on globally configured remote HTTP server.
        /// </summary>
        /// <returns>Server builder to set specific HTTP requests.</returns>
        public IServerBuilder WorkingRemotely()
        {
            if (RemoteServer.GlobalIsConfigured)
            {
                return new ServerTestBuilder(RemoteServer.GlobalClient);
            }

            throw new InvalidOperationException("No remote server is configured for this particular test case. Either call MyWebApi.Server().IsLocatedAt() to configure a new remote server or provide test specific base address.");
        }

        /// <summary>
        /// Processes HTTP request on the remote HTTP server located at the provided base address.
        /// </summary>
        /// <param name="baseAddress">Base address to use for the requests.</param>
        /// <returns>Server builder to set specific HTTP requests.</returns>
        public IServerBuilder WorkingRemotely(string baseAddress)
        {
            return new ServerTestBuilder(RemoteServer.CreateNewClient(baseAddress), disposeServer: true);
        }

        private string GetStartOptions(int port, string host)
        {
            return string.Format("{0}:{1}", host, port);
        }
    }
}
