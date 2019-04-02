using KubeMQ.SDK.csharp.Events;
using KubeMQ.SDK.csharp.Subscription;
using KubeMQ.SDK.csharp.Tools;
using System;

namespace KubeMQsub
{
    class Program
    {  //The KubeMQ communication channel between pub sub
        const string eventChannelName = "TestCh_1";
        private static bool subLoop=true;

        static void Main(string[] args)
        {


            //KubeMQ address can be set as environment parameter "KubeMQServerAddress"
            //The address port is the Grpc communication KubeMQ port 
            Console.WriteLine($"[Demo] KUBEMQ GRPC address:{Environment.GetEnvironmentVariable("KubeMQServerAddress")}");


            //Start event subscriber

            Console.WriteLine("[Demo] Starting demo SUB");

            //create KubeMQ.SDK.csharp.Subscription.SubscribeRequest
            SubscribeRequest subscribeRequest = new SubscribeRequest()
            {
                Channel = eventChannelName, //Subscribed channel name
                SubscribeType = SubscribeType.Events, //sub channel pattern
                ClientID = "sub_Demo"  //sub name ID
            };

            //create a new KubeMQ.SDK.csharp.Events.Subscriber (KubeMQServerAddress environment var)
            Subscriber subscriber = new Subscriber();
            try
            {
                /// SubscribeToEvents HandleEventDelegate anonymous 
                subscriber.SubscribeToEvents(subscribeRequest, (eventReceive) =>
                {
                    //KubeMQ.SDK.csharp.ToolsConverter.Converter.FromByteArray decode message from byte[]
                    var recMsg = Converter.FromByteArray(eventReceive.Body);
                    Console.WriteLine($"[Demo] Receive message ID:{eventReceive.EventID} received:{recMsg}");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Demo] Did not subscribe, please check KubeMQ address, Error:{ex.GetType()}");
            }

            Console.WriteLine("[Demo] press Ctrl+c to stop");
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) =>
            {
                subLoop = false;
                e.Cancel = true;
            };
            while (subLoop)
            {
                Console.ReadKey();
            }

        }
    }
}
