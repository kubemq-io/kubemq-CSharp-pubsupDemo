using KubeMQ.SDK.csharp.Events;
using KubeMQ.SDK.csharp.Tools;
using System;

namespace KubeMQpub
{
    class Program
    {
        //The KubeMQ communication channel between pub sub
        const string eventChannelName = "TestCh_1";
        private static bool pubLoop=false;

        static void Main(string[] args)
        {
       
          
            //KubeMQ address can be set as environment parameter "KubeMQServerAddress"
            //The address port is the Grpc communication KubeMQ port 
            Console.WriteLine($"[Demo] KUBEMQ GRPC address:{Environment.GetEnvironmentVariable("KubeMQServerAddress")}");
            //Starting sync publisher
            Console.WriteLine("[Demo] Starting demo PUB");
            ChannelParameters eventChannelParameters = new ChannelParameters
            {
                //Publish channel name (must match the sub ChannelName)
                ChannelName = eventChannelName,
                //pub name ID
                ClientID = "pub_Demo"
            };



            //Create a new KubeMQ.SDK.csharp.Events.Channel 
            var eChannel = new Channel(eventChannelParameters);
            //KubeMQ.SDK.csharp.Events.Result is the pub message status.
            Result result = null;

            Console.WriteLine("[Demo] press Ctrl+c to stop");
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) =>
            {
                pubLoop = false;
                e.Cancel = true;
            };


            Console.WriteLine("[Demo] Please enter message text to publish");
            while (pubLoop)
            {
            
                string msg = Console.ReadLine();
          
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
                    Console.WriteLine($"[Demo] Could not send message, please check KubeMQ address, Error{ex.GetType()}");
                    continue;
                }

                if (!result.Sent)
                {
                    Console.WriteLine($"[Demo] Could not send message:{result.Error}");
                    continue;
                }

                Console.WriteLine($"[Demo] Sent message ID:{result.EventID}");
            }

        }       
    }
}
