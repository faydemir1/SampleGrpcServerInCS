 /* 
 *
 *  Author: Fikri Aydemir
 *  Date  :	10/04/2020 15:14
 *  
 *  Released under MIT License
 *
 */

using Common.APIGateway.Grpc;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Debugging;
using Serilog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace Server.APIGateway.Grpc
{
    /// <summary>
    /// Class providing the execution logic of Grpc Server
    /// </summary> 
    public class Startup : IDisposable
    {
        private global::Grpc.Core.Server _server;       
        private const string STR_GrpcServerDefaultPort = "GrpcServerDefaultPort";
        private const string STR_CertsRootPath = "\\Certs";
        private const string STR_AppName = "GrpcServer";
        private const string STR_QuitText = "Press q to stop the server...";       

        /// <summary>
        /// Public Ctor
        /// </summary> 
        public Startup()
        {
        }

        /// <summary>
        /// Initializer
        /// </summary> 
        public void StartServer()
        {
            try
            {
                var configuration = GetConfiguration();
                Log.Logger = CreateSerilogLogger(configuration);
                IHost host = CreateHostBuilder(configuration, Log.Logger);
                int serverPort = configuration.GetValue<int>(STR_GrpcServerDefaultPort);
                //var sslServerCredentials = CreateSslServerCredentials();
                const string hostAddr = "0.0.0.0";

                Log.Information("Starting {0} at port {1}", STR_AppName, serverPort);

                _server = new global::Grpc.Core.Server
                {
                    Services = { GrpcAPIGatewayService.BindService(new GrpcGatewayServerImpl()) },
                    Ports = { new ServerPort(hostAddr, serverPort, ServerCredentials.Insecure) } // sslServerCredentials) }
                };

                _server.Start();

                Log.Information(STR_QuitText);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Startup terminated unexpectedly ({ApplicationContext})!", STR_AppName);
            }
        }

        /// <summary>
        /// Configuration builder
        /// </summary> 
        private static IHost CreateHostBuilder(IConfiguration configuration, Serilog.ILogger logger)
        {
            var builder = Host.CreateDefaultBuilder()
                              .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                              .UseSerilog(logger, configuration)
                              .UseContentRoot(Directory.GetCurrentDirectory());

            return builder.Build();
        }

        /// <summary>
        /// Factory method for logger generation
        /// </summary> 
        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                      .WriteTo.Console()
                      .WriteTo.File("D:\\Logs\\GrpcAPIGateway\\log-.txt", rollingInterval: RollingInterval.Hour)
                      .CreateLogger();

            return Log.Logger;
        }

        /// <summary>
        /// Accesssor for configuration
        /// </summary>
        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
            return builder.Build();
        }

        private static SslServerCredentials CreateSslServerCredentials()
        {
            var certsPath = string.Format("{0}{1}", Directory.GetCurrentDirectory(), STR_CertsRootPath);
            var cacert = File.ReadAllText(certsPath + @"\server.crt");
            var servercert = File.ReadAllText(certsPath + @"\server.crt");
            var serverkey = File.ReadAllText(certsPath + @"\server.key");
            var keypair = new KeyCertificatePair(servercert, serverkey);
            var sslCredentials = new SslServerCredentials(new List<KeyCertificatePair>() { keypair }, servercert, false);//cacert, false);
            return sslCredentials;
        }

        public void Dispose()
        {
            if (_server != null)
            {
                _server.ShutdownAsync().Wait();
            }

            if (Log.Logger != null)
            {
                Log.CloseAndFlush();
            }
        }
    }

    public static class SerilogHostBuilderExtensions
    {
        public static IHostBuilder UseSerilog(this IHostBuilder builder,
            Serilog.ILogger logger = null, IConfiguration configuration = null, bool dispose = false)
        {
            builder.ConfigureServices((context, collection) =>
                collection.Configure<IConfiguration>(configuration).AddSingleton<ILoggerFactory>(services => new SerilogLoggerFactory(logger, dispose)));
            return builder;
        }
    }

    public class SerilogLoggerFactory : ILoggerFactory
    {
        private readonly SerilogLoggerProvider _provider;

        public SerilogLoggerFactory(Serilog.ILogger logger = null, bool dispose = false)
        {
            _provider = new SerilogLoggerProvider(logger, dispose);
        }

        public void Dispose() => _provider.Dispose();

        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            return _provider.CreateLogger(categoryName);
        }

        public void AddProvider(ILoggerProvider provider)
        {
            // Only Serilog provider is allowed!
            SelfLog.WriteLine("Ignoring added logger provider {0}", provider);
        }
    }
}
