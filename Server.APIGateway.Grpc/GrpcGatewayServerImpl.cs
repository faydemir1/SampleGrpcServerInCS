/*  Copyright 2021 KUVEYT TÜRK PARTICIPATION BANK INC.
 *
 *  Author: Fikri Aydemir
 *  Date  :	10/04/2020 15:14
 *  
 *  Released under MIT License
 *
 */

using System;
using System.Threading.Tasks;
using Common.APIGateway.Grpc;
using Grpc.Core;
using Serilog;

namespace Server.APIGateway.Grpc
{
    /// <summary>
    /// Claz GrpcGatewayServerImpl  
    /// </summary> 
    public class GrpcGatewayServerImpl : GrpcAPIGatewayService.GrpcAPIGatewayServiceBase
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<GrpcGatewayServerImpl>();

        /// <summary>
        /// Ctor for Class GrpcGatewayServerImpl 
        /// </summary>  
        public GrpcGatewayServerImpl()
        {
        }

        /// <summary>
        /// Inserts message to kafka
        /// </summary>
        /// <param name="request">The request received from the client.</param>
        /// <param name="context">The context of the server-side call handler being invoked.</param>
        /// <returns>The response to send back to the client (wrapped by a task).</returns>
        public override async Task<KafkaInsertResponse> InsertFromKafka(KafkaInsertRequest request, ServerCallContext context)
        {
            var kafkaInsertResponse = new KafkaInsertResponse();
            DoFibonacci();
            Log.Information("Insert Kafka is successful!");
            kafkaInsertResponse.Success = true;
            kafkaInsertResponse.Value = 0;
            return kafkaInsertResponse;
        }

        /// <summary>
        /// Sends Notification
        /// </summary>
        /// <param name="request">The request received from the client.</param>
        /// <param name="context">The context of the server-side call handler being invoked.</param>
        /// <returns>The response to send back to the client (wrapped by a task).</returns>
        public override async Task<NotificationQueueResponse> SendNotification(NotificationQueueRequest request, ServerCallContext context)
        {
            var notificationQueueResponse = new NotificationQueueResponse();
            DoFibonacci();
            Log.Information("Send Notfication is successful!");
            notificationQueueResponse.Success = true;
            notificationQueueResponse.Value = 0;
            return notificationQueueResponse;
        }
            
        private void DoFibonacci()
        {
            Random random = new Random();
            int n1 = 0, n2 = 1, n3, i, number;
            number = random.Next(0, 1000000);                    
            for (i = 2; i < number; ++i)  
            {
                n3 = n1 + n2;
                n1 = n2;
                n2 = n3;
            }
        }
    }
}

