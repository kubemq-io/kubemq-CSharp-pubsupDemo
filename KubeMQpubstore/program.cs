using System;
using KubeMQ.SDK.csharp.Events;
using KubeMQ.SDK.csharp.Tools;

namespace KubeMQpubstore
{
    class Program
    {


        //The pub ClientID, can be set by environment var CLIENT
        private static string ClientID = Environment.GetEnvironmentVariable("CLIENT") ?? $"pub_Demo_{Environment.MachineName}";
        //The KubeMQ communication channel between pub sub, can be set by environment var CHANNEL 
        private static string ChannelName = Environment.GetEnvironmentVariable("CHANNEL") ?? "Test_Channel";

        private static bool pubLoop = true;
        static void Main(string[] args)
        {
            Console.WriteLine("[Demo] Start demo PUB");
            //KubeMQ address environment var "KubeMQServerAddress" GRPC port
            Console.WriteLine($"[Demo] KUBEMQ GRPC address:{Environment.GetEnvironmentVariable("KubeMQServerAddress")}");
            Console.WriteLine($"[Demo] ClientID:{ClientID}");
            Console.WriteLine($"[Demo] ChannelName:{ChannelName}");

            Console.WriteLine("[Demo] press Ctrl+c to stop");

            System.Threading.AutoResetEvent waitHandle = new System.Threading.AutoResetEvent(false);
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) =>
            {
                pubLoop = false;
                e.Cancel = true;
                waitHandle.Set();

            };

            ChannelParameters channelParameters = new ChannelParameters()
            {
                ChannelName = ChannelName,
                ClientID = ClientID,
                Store = true
            };


            //Create a new KubeMQ.SDK.csharp.Events.Channel 
            var eChannel = new Channel(channelParameters);
            //KubeMQ.SDK.csharp.Events.Result is the pub message status.
            Result result = null;

            Console.WriteLine("[Demo] Please enter message text to publish");

            while (pubLoop)
            {
                string msg = Console.ReadLine();

                if (msg == null)
                {
                    continue;
                }
                try
                {
                    result = eChannel.SendEvent(new KubeMQ.SDK.csharp.Events.Event()
                    {
                        //KubeMQ.SDK.csharp.ToolsConverter.ToByteArray, be sure do decode with the same encoder on sub.
                        Body = Converter.ToByteArray(msg)

                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Demo] Could not send messages, please check KubeMQ address, Error{ex.GetType()}");
                    waitHandle.Set();
                    break;
                }

                if (!result.Sent)
                {
                    Console.WriteLine($"[Demo] Could not send single message:{result.Error}");
                    continue;
                }

                Console.WriteLine($"[Demo] Sent message ID:{result.EventID}");
            }

            waitHandle.WaitOne();


        }
    }
}


    
    

