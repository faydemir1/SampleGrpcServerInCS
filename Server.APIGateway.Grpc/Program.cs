/*  Copyright 2021 KUVEYT TÜRK PARTICIPATION BANK INC.
 *
 *  Author: Fikri Aydemir
 *  Date  :	10/04/2020 15:14
 *  
 *  Released under MIT License
 *
 */

using System;

namespace Server.APIGateway.Grpc
{
    /// <summary>
    /// Main Program
    /// </summary> 
    public class Program
    {
        /// <summary>
        ///  Program Entry
        /// </summary> 
        public static void Main(string[] args)
        {
            using (var startup = new Startup())
            {
                startup.StartServer();

                ConsoleKeyInfo theKey;
                do
                {
                    theKey = Console.ReadKey();
                }
                while (theKey.Key != ConsoleKey.Q);
            }
        }
    }
}
