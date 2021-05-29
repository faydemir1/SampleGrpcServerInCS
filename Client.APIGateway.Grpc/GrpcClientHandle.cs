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
using System;
using System.IO;
using System.Threading.Tasks;

namespace Client.APIGateway.Grpc
{
    /// <summary>
    /// The implementation for grpc client
    /// </summary>
    public class GrpcClientHandle : IDisposable, IGrpcClientHandle
    {
        private GrpcAPIGatewayService.GrpcAPIGatewayServiceClient _clientHandle;
        private Channel _channel;

        private readonly string _grpcServerURI;
        private readonly int _port;
        private const string STR_CertsRootPath = "\\Certs";

        /// <summary>
        /// Public Ctor
        /// </summary>
        public GrpcClientHandle(string grpcServerURI, int port)
        {
            _grpcServerURI = grpcServerURI;
            _port = port;
            Init();
        }

        /// <summary>
        /// Initializer
        /// </summary>
        private void Init()
        {          
            _channel = new Channel(string.Format("{0}:{1}", _grpcServerURI, _port), ChannelCredentials.Insecure);
            _clientHandle = new GrpcAPIGatewayService.GrpcAPIGatewayServiceClient(_channel);
        }

        /// <summary>
        /// Inserts message to kafka
        /// </summary>
        /// <param name="request">The request received from the client.</param>        
        /// <returns>The response</returns>
        public KafkaInsertResponse InsertFromKafka(KafkaInsertRequest request)
        {
            return _clientHandle.InsertFromKafka(request);
        }

        /// <summary>
        /// Inserts message to kafka
        /// </summary>
        /// <param name="request">The request received from the client.</param>        
        /// <returns>The response</returns>
        public async Task<KafkaInsertResponse> InsertFromKafkaAsync(KafkaInsertRequest request)
        {
            var response = await _clientHandle.InsertFromKafkaAsync(request);
            Console.WriteLine("Response ==> " + response.Value);
            return response;
        }

        /// <summary>
        /// Sends Notification
        /// </summary>
        /// <param name="request">The request received from the client.</param>
        /// <returns>The response.</returns>
        public NotificationQueueResponse SendNotification(NotificationQueueRequest request)
        {
            return _clientHandle.SendNotification(request);
        }

        /// <summary>
        /// Sends Notification
        /// </summary>
        /// <param name="request">The request received from the client.</param>
        /// <returns>The response.</returns>
        public async Task<NotificationQueueResponse> SendNotificationAsync(NotificationQueueRequest request)
        {
            return await _clientHandle.SendNotificationAsync(request);
        }


        /// <summary>
        /// Creates certificate
        /// </summary>
        private static SslCredentials CreateSslClientCredentials()
        {
            var certsPath = string.Format("{0}{1}", Directory.GetCurrentDirectory(), STR_CertsRootPath);
            var cacert = File.ReadAllText(certsPath + @"\ca.crt");
            var servercert = File.ReadAllText(certsPath + @"\server.crt");
            var serverkey = File.ReadAllText(certsPath + @"\server.key");
            var keypair = new KeyCertificatePair(servercert, serverkey);
            //var sslCredentials = new SslCredentials(new List<KeyCertificatePair>() { keypair }, cacert, false);
            var sslCredentials = new SslCredentials(servercert, keypair);//new SslCredentials(cacert, keypair);
            return sslCredentials;
        }

        /// <summary>
        /// Method dispose
        /// </summary>
        public void Dispose()
        {
            if (_channel != null && (_channel.State != ChannelState.Shutdown || _channel.State != ChannelState.TransientFailure))
            {
                _channel.ShutdownAsync().Wait();
            }
        }
    }
}
