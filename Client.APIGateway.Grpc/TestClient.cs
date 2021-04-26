/*  Copyright 2021 KUVEYT TÜRK PARTICIPATION BANK INC.
 *
 *  Author: Fikri Aydemir
 *  Date  :	10/04/2020 15:14
 *  
 *  Released under MIT License
 *
 */

using Common.APIGateway.Grpc;
using System;

namespace Client.APIGateway.Grpc
{
    public class TestClient
    {
        private static string serverURI;
        private static int port;
        
        /// <summary>
        /// Main Method   
        /// </summary> 
        public static void Main(string[] args)
        {
            serverURI = "127.0.0.1";
            port = 3000;
            DoBasicTest();
        }

       
        public static void DoBasicTest()
        {
            try
            {
                string theUniqueId = System.Guid.NewGuid().ToString();

                var base64Data = "test";

                var queueMessage = new QueueMessage()
                {
                    UniqueId = theUniqueId,
                    BusinessKey = "123",
                    Base64Data = base64Data
                };

                var kafkaInsertRequest = new KafkaInsertRequest()
                {
                    InsertQueueMessage = queueMessage,
                    CountId = 0
                };

                var clientHandle = new GrpcClientHandle(serverURI, port);
                var response = clientHandle.InsertFromKafka(kafkaInsertRequest);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Exception thrown: " + exp.Message);
            }
        }

        public static void DoBasicNotificationTest()
        {
            try
            {
                string theUniqueId = System.Guid.NewGuid().ToString();
                var base64Data = "test"; ;

                var notificationRequest = new NotificationQueueRequest()
                {
                    UniqueId = theUniqueId,
                    BusinessKey = "123",
                    Data = base64Data
                };

                var clientHandle = new GrpcClientHandle(serverURI, port);
                var response = clientHandle.SendNotification(notificationRequest);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Exception thrown: " + exp.Message);
            }
        }
    }
}
