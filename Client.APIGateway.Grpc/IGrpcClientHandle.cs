/*  Copyright 2021 KUVEYT TÜRK PARTICIPATION BANK INC.
 *
 *  Author: Fikri Aydemir
 *  Date  :	10/04/2020 15:14
 *  
 *  Released under MIT License
 *
 */

using Common.APIGateway.Grpc;
using System.Threading.Tasks;

namespace Client.APIGateway.Grpc
{
    /// <summary>
    /// The Interface for grpc client
    /// </summary>
    public interface IGrpcClientHandle
    {
        /// <summary>
        /// Inserts message to kafka
        /// </summary>
        /// <param name="request">The request received from the client.</param>        
        /// <returns>The response</returns>
        KafkaInsertResponse InsertFromKafka(KafkaInsertRequest request);

        /// <summary>
        /// Inserts message to kafka
        /// </summary>
        /// <param name="request">The request received from the client.</param>        
        /// <returns>The response</returns>
        Task<KafkaInsertResponse> InsertFromKafkaAsync(KafkaInsertRequest request);

        /// <summary>
        /// Sends Notification
        /// </summary>
        /// <param name="request">The request received from the client.</param>
        /// <returns>The response.</returns>
        NotificationQueueResponse SendNotification(NotificationQueueRequest request);

        /// <summary>
        /// Sends Notification
        /// </summary>
        /// <param name="request">The request received from the client.</param>
        /// <returns>The response.</returns>
        Task<NotificationQueueResponse> SendNotificationAsync(NotificationQueueRequest request);
    }
}
